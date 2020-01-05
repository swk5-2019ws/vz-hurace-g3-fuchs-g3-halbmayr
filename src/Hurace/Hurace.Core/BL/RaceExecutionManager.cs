using Hurace.Timer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hurace.Core.BL
{
    public class RaceExecutionManager : IRaceExecutionManager
    {
        private Domain.Race trackedRace;
        private Domain.Skier trackedSkier;
        private Domain.RaceData trackedRaceData;
        private int? trackedPosition;

        private IDictionary<int, (double mean, double standardDeviation)> measurementDistributionDictionary;
        private IDictionary<int, DateTime> measurementTimeDictionary;

        private IRaceClock raceClock;

        private readonly IInformationManager informationManager;

        public RaceExecutionManager(IInformationManager informationManager)
        {
            this.informationManager = informationManager ?? throw new ArgumentNullException(nameof(informationManager));
            this.measurementTimeDictionary = new Dictionary<int, DateTime>();
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
            this.trackedRaceData =
                await informationManager.GetRaceDataByRaceAndStartlistAndPosition(race, firstStartList, position)
                    .ConfigureAwait(false);

            this.trackedSkier = await informationManager.GetSkierByRaceAndStartlistAndPosition(race, firstStartList, position)
                .ConfigureAwait(false);
            this.measurementDistributionDictionary =
                await informationManager.CalculateNormalDistributionOfMeasumentsPerSensor(
                        race.Venue.ForeignKey.Value, race.RaceType.ForeignKey.Value)
                    .ConfigureAwait(false);

            var raceStates = await informationManager.GetAllRaceStates().ConfigureAwait(false);
            trackedRaceData.RaceState = new Domain.Associated<Domain.RaceState>(raceStates.First(rs => rs.Label == "Laufend"));

            await informationManager.UpdateRaceData(trackedRaceData).ConfigureAwait(false);

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
            if (0 <= sensorId && sensorId < this.trackedRace.NumberOfSensors)
            {
                var wasAlreadyMeasured =
                    (await this.informationManager.GetTimeMeasurementByRaceDataAndSensorId(this.trackedRaceData.Id, sensorId)
                    .ConfigureAwait(false)) != null;

                bool newMeasurementPersistable = true;
                bool isTimeMeasurementValid = !wasAlreadyMeasured;
                int difference = 0;

                if (sensorId >= 0 && this.measurementTimeDictionary.ContainsKey(0))
                {
                    difference = Convert.ToInt32(
                        (measuredTime - this.measurementTimeDictionary[0]).TotalMilliseconds);

                    if (sensorId == this.trackedRace.NumberOfSensors)
                        isTimeMeasurementValid = isTimeMeasurementValid && true;
                    else if (this.measurementDistributionDictionary.ContainsKey(sensorId))
                    {
                        //time measurements has reference measurements
                        (var mean, var stdDev) = measurementDistributionDictionary[sensorId];
                        (var lowerBoundary, var upperBoundary) = Statistics.NormalDistribution.CalculateBoundaries(mean, stdDev, 0.95);

                        isTimeMeasurementValid =
                            isTimeMeasurementValid &&
                            lowerBoundary <= difference && difference <= upperBoundary;
                    }
                    else if (this.measurementTimeDictionary.ContainsKey(sensorId - 1))
                        isTimeMeasurementValid =
                            isTimeMeasurementValid &&
                            (measuredTime - this.measurementTimeDictionary[sensorId - 1]).TotalMilliseconds >= 500;
                    else
                        newMeasurementPersistable = false;
                }

                if (newMeasurementPersistable)
                {
                    var newTimeMeasurement =
                       await informationManager.CreateTimemeasurement(
                           Convert.ToInt32(difference),
                           sensorId,
                           this.trackedRaceData.Id,
                           isTimeMeasurementValid)
                       .ConfigureAwait(false);

                    if (isTimeMeasurementValid)
                    {
                        this.measurementTimeDictionary.Add(sensorId, measuredTime);
                        this.OnTimeMeasured?.Invoke(this.trackedRace, this.trackedSkier, newTimeMeasurement);

                        if (sensorId == this.trackedRace.NumberOfSensors - 1)
                        {
                            this.raceClock.TimingTriggered -= OnRaceSensorTriggered;

                            var raceStates = await informationManager.GetAllRaceStates().ConfigureAwait(false);

                            trackedRaceData.RaceState = new Domain.Associated<Domain.RaceState>(
                                raceStates.First(rs => rs.Label == "Abgeschlossen"));

                            await informationManager.UpdateRaceData(trackedRaceData).ConfigureAwait(false);

                            this.trackedRace = null;
                            this.trackedSkier = null;
                            this.trackedRaceData = null;
                            this.trackedPosition = null;
                            this.measurementTimeDictionary = null;
                        }
                    }
                }
            }
        }
    }
}
