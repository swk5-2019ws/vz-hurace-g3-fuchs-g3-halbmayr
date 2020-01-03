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
        private bool? trackingFirstStartList;
        private int? trackedPosition;
        private IDictionary<int, (double mean, double standardDeviation)> measumentDistributionDictionary;

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
            this.trackingFirstStartList = firstStartList;
            this.trackedPosition = position;

            this.trackedSkier = await informationManager.GetSkierByRaceAndStartlistAndPosition(race, firstStartList, position)
                .ConfigureAwait(false);
            this.measumentDistributionDictionary =
                await informationManager.CalculateNormalDistributionOfMeasumentsPerSensor(
                    race.Venue.ForeignKey.Value, race.RaceType.ForeignKey.Value);

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

        private void OnRaceSensorTriggered(int sensorId, DateTime time)
        {
            //todo: implement time validation -> use normal distribution of time measurements

            bool isMeasurementValid = true;

            if (!isMeasurementValid)
            {
                //persist error
            }
            else
            {
                //persist successfully measured timemeasurement
                this.OnTimeMeasured?.Invoke(this.trackedRace, this.trackedSkier, null);

                if (sensorId == this.trackedRace.NumberOfSensors)
                {
                    this.raceClock.TimingTriggered -= OnRaceSensorTriggered;
                    this.trackedRace = null;
                    this.trackedSkier = null;
                    this.trackingFirstStartList = null;
                    this.trackedPosition = null;
                }
            }

            throw new NotImplementedException();
        }
    }
}
