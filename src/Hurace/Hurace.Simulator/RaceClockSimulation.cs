using System;

namespace Hurace.Simulator
{
    public class RaceClockSimulation : Timer.IRaceClock
    {
        private static RaceClockSimulation singleInstance;

        private RaceClockSimulation() { }

        public static RaceClockSimulation Instance
        {
            get
            {
                if (singleInstance == null)
                    singleInstance = new RaceClockSimulation();
                return singleInstance;
            }
        }

        public event Timer.TimingTriggeredHandler TimingTriggered;

        internal void RaiseRaceClockEvent(int sensorId, DateTime time)
        {
            this.TimingTriggered?.Invoke(sensorId, time);
        }
    }
}
