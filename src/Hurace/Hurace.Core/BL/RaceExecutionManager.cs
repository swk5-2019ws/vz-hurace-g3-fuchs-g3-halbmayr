using Hurace.Timer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Core.BL
{
    public class RaceExecutionManager : IRaceExecutionManager
    {
        private readonly IRaceClock raceClock;

        public RaceExecutionManager(IRaceClock raceClock)
        {
            this.raceClock = raceClock ?? throw new ArgumentNullException(nameof(raceClock));
        }

        public Domain.Race TrackedRace { get; set; }
        public Domain.Skier TrackedSkier { get; set; }

        public event OnTimeMeasured OnTimeMeasured;

        public void StartTimeTracking(Domain.Race race, Domain.Skier skier)
        {
            //validate if there exists a startposition 
            //validate if the start poisition is the one up next

            this.raceClock.TimingTriggered += OnRaceSensorTriggered;
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
