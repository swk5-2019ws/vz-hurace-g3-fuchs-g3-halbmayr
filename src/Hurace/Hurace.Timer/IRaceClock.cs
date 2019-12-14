using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Timer
{
    public delegate void TimingTriggeredHandler(int sensorId, DateTime time);

    public interface IRaceClock
    {
        public event TimingTriggeredHandler TimingTriggered;
    }
}
