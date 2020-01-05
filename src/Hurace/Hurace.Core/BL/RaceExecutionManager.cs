using Hurace.Timer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hurace.Core.BL
{
    public class RaceExecutionManager : IRaceExecutionManager
    {
        private IRaceClock raceClock;
        private Domain.Race trackedRace;
        private Domain.Skier trackedSkier;
        private Domain.RaceData trackedRaceData;
        private int? trackedPosition;
        private IDictionary<int, (double mean, double standardDeviation)> measumentDistributionDictionary;
        private DateTime? initialMeasurement;

        private readonly IInformationManager informationManager;

        public RaceExecutionManager(IInformationManager informationManager)
        {
            this.informationManager = informationManager ?? throw new ArgumentNullException(nameof(informationManager));
        }

        public event OnTimeMeasured OnTimeMeasured;

        public IRaceClock RaceClock
        {
            get => raceClock;
            set
            {
                if (this.trackedRace is null)
                {
                    raceClock = value;
                }
                else
                {
                    throw new InvalidOperationException("Cannot replace RaceClock while tracking a skier");
                }
            }
        }

        public async Task StartTimeTracking(
            Domain.Race race,
            bool firstStartList,
            int position)
        {
            if (race is null)
                throw new ArgumentNullException(nameof(race));
            else if (this.RaceClock is null)
                throw new InvalidOperationException("Set RaceClock before tracking a race");
            else if (!await informationManager.IsNextStartposition(race, firstStartList, position).ConfigureAwait(false))
                throw new InvalidOperationException($"Position {position} is not the next position");
            else if (race.Venue is null || !race.Venue.Initialised)
                throw new ArgumentNullException($"FK of {nameof(Domain.Venue)} has to be set in passed {nameof(Domain.Race)} instance");
            else if (race.RaceType is null || !race.RaceType.Initialised)
                throw new ArgumentNullException($"FK of {nameof(Domain.RaceType)} has to be set in passed {nameof(Domain.Race)} instance");

            this.trackedRace = race;
            this.trackedPosition = position;
            this.trackedRaceData = await informationManager.GetRaceDataByRaceAndStartlistAndPosition(race, firstStartList, position);

            this.trackedSkier = await informationManager.GetSkierByRaceAndStartlistAndPosition(race, firstStartList, position)
                .ConfigureAwait(false);
            this.measumentDistributionDictionary =
                await informationManager.CalculateNormalDistributionOfMeasumentsPerSensor(
                    race.Venue.ForeignKey.Value, race.RaceType.ForeignKey.Value);

            var raceStates = await informationManager.GetAllRaceStates();
            trackedRaceData.RaceState = new Domain.Associated<Domain.RaceState>(raceStates.First(rs => rs.Label == "Laufend"));

            await informationManager.UpdateRaceData(trackedRaceData);

            this.raceClock.TimingTriggered += OnRaceSensorTriggered;
        }

        public async Task<bool> IsRaceStartable(int raceId)
        {
            var race = await informationManager.GetRaceByIdAsync(
                    raceId,
                    startListLoadingType: Domain.Associated<Domain.StartPosition>.LoadingType.ForeignKey)
                .ConfigureAwait(false);

            return race.Date == DateTime.Now.Date &&
                race.FirstStartList.Any();
        }

        private async void OnRaceSensorTriggered(int sensorId, DateTime measuredTime)
        {
            if (sensorId == 0 && initialMeasurement is null)
            {
                this.initialMeasurement = measuredTime;

                var newTimeMeasurement = await informationManager.CreateTimemeasurement(0, 0, this.trackedRaceData.Id, true)
                    .ConfigureAwait(false);

                this.OnTimeMeasured?.Invoke(this.trackedRace, this.trackedSkier, newTimeMeasurement);
            }
            else if (this.measumentDistributionDictionary.ContainsKey(sensorId))
            {
                (var mean, var stdDev) = measumentDistributionDictionary[sensorId];
                (var lowerBoundary, var upperBoundary) = Statistics.NormalDistribution.CalculateBoundaries(mean, stdDev, 0.95);

                var difference = (this.initialMeasurement.Value - measuredTime).TotalMilliseconds;

                var isTimeMeasurementValid = (lowerBoundary <= difference && difference <= upperBoundary) ||
                                             sensorId == this.trackedRace.NumberOfSensors;

                var newTimeMeasurement =
                    await informationManager.CreateTimemeasurement(
                        Convert.ToInt32(difference),
                        sensorId,
                        this.trackedRaceData.Id,
                        isTimeMeasurementValid)
                    .ConfigureAwait(false);

                if (isTimeMeasurementValid)
                {
                    this.OnTimeMeasured?.Invoke(this.trackedRace, this.trackedSkier, null);

                    if (sensorId == this.trackedRace.NumberOfSensors)
                    {
                        this.raceClock.TimingTriggered -= OnRaceSensorTriggered;
                        this.trackedRace = null;
                        this.trackedSkier = null;
                        this.trackedRaceData = null;
                        this.trackedPosition = null;
                        this.initialMeasurement = null;
                    }
                }
            }
        }
    }
}
