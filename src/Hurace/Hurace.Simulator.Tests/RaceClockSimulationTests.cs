using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace Hurace.Simulator.Tests
{
    public class RaceClockSimulationTests
    {
        [Fact]
        public void TestTimingTriggered()
        {
            var counter = 0;

            var simulation = new RaceClockSimulation
            {
                UnwantedTimerTriggerChance = 0,
                TimerIntervalMean = 50,
                TimerIntervalStdDev = 0.1,
                TimerFailureChance = 0
            };
            simulation.TimingTriggered += (sensorId, time) => counter++;

            Thread.Sleep(110);

            Assert.Equal(2, counter);
        }
    }
}
