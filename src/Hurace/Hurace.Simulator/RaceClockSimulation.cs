using System;

namespace Hurace.Simulator
{
    public class RaceClockSimulation : Timer.IRaceClock
    {
        public RaceClockSimulation()
        {
            //todo: start thread that simulated timer
        }

        public event Timer.TimingTriggeredHandler TimingTriggered;

        internal void RaiseRaceClockEvent(int sensorId, DateTime time)
        {
            this.TimingTriggered?.Invoke(sensorId, time);
        }
    }
}
