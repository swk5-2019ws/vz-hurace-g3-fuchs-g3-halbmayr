using System;
using System.Threading;

namespace Hurace.Simulator
{
    public class RaceClockSimulation : Timer.IRaceClock
    {
        public RaceClockSimulation()
        {
            ThreadPool.QueueUserWorkItem(SimulateTimer);
        }

        public int MaxSensorIdValue { get; internal set; }

        public event Timer.TimingTriggeredHandler TimingTriggered;

        internal void RaiseRaceClockEvent(int sensorId, DateTime time)
        {
            this.TimingTriggered?.Invoke(sensorId, time);
        }

        private void SimulateTimer(object state)
        {
            while (true)
            {
                Thread.Sleep(500);
            }
        }
    }
}
