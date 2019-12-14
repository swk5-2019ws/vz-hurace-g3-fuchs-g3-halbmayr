using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Simulator
{
    public class RaceClock : Timer.IRaceClock
    {
        private static RaceClock singleInstance;

        private RaceClock() { }

        public static RaceClock Instance
        {
            get
            {
                if (singleInstance == null)
                    singleInstance = new RaceClock();
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
