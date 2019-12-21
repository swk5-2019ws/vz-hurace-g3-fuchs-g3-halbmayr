using Hurace.Timer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hurace.Core.BL
{
    public class RaceExecutionManager : IRaceExecutionManager
    {
        private readonly IRaceClock raceClock;

        public RaceExecutionManager() { }

        public event OnTimeMeasured OnTimeMeasured;

        public Domain.Race TrackedRace { get; set; }
        public Domain.Skier TrackedSkier { get; set; }
        public IRaceClock RaceClock { get; set; }

        public void StartTimeTracking(IRaceClock raceClock, Domain.Race race, Domain.Skier skier)
        {
            //validate if there exists a startposition 
            //validate if the start poisition is the one up next

            this.raceClock.TimingTriggered += OnRaceSensorTriggered;
        }

        public Task<bool> IsRaceStartable(int raceId)
        {
            throw new NotImplementedException();
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
