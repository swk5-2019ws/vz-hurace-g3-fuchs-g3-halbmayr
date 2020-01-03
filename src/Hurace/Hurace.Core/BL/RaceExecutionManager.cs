using Hurace.Timer;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hurace.Core.BL
{
    public class RaceExecutionManager : IRaceExecutionManager
    {
        private IRaceClock raceClock;

        private readonly IInformationManager informationManager;

        public RaceExecutionManager(IInformationManager informationManager)
        {
            this.informationManager = informationManager ?? throw new ArgumentNullException(nameof(informationManager));
        }

        public event OnTimeMeasured OnTimeMeasured;

        public Domain.Race TrackedRace { get; set; }
        public Domain.Skier TrackedSkier { get; set; }
        public IRaceClock RaceClock
        {
            get => raceClock;
            set
            {
                if (this.TrackedRace is null &&
                    this.TrackedSkier is null)
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
            if (this.RaceClock is null)
                throw new InvalidOperationException("Set RaceClock before tracking a race");

            //validate if the start poisition is the one up next
            bool isNext = await informationManager.IsNextStartposition(race, firstStartList, position).ConfigureAwait(false);

            this.raceClock.TimingTriggered += OnRaceSensorTriggered;
        }

        public async Task<bool> IsRaceStartable(int raceId)
        {
            var race = await informationManager.GetRaceByIdAsync(
                raceId,
                startListLoadingType: Domain.Associated<Domain.StartPosition>.LoadingType.ForeignKey);

            return race.Date == DateTime.Now.Date &&
                race.FirstStartList.Any();
        }

        private void OnRaceSensorTriggered(int sensorId, DateTime time)
        {
            //todo: implement time validation

            bool isMeasurementValid = true;

            if (!isMeasurementValid)
            {
                //persist error
            }
            else
            {
                //persist successfully measured timemeasurement
                this.OnTimeMeasured?.Invoke(this.TrackedRace, this.TrackedSkier, null);
            }

            bool triggeredSensorWasLast = true;

            if (isMeasurementValid && triggeredSensorWasLast)
            {
                this.raceClock.TimingTriggered -= OnRaceSensorTriggered;
                this.TrackedRace = null;
                this.TrackedSkier = null;
            }

            throw new NotImplementedException();
        }
    }
}
