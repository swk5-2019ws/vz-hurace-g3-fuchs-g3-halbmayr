using System;
using System.Threading;

#pragma warning disable CA5394 // Do not use insecure randomness
namespace Hurace.Simulator
{
    public class RaceClockSimulation : Timer.IRaceClock
    {
        public RaceClockSimulation()
        {
            ThreadPool.QueueUserWorkItem(SimulateTimer);
        }

        public event Timer.TimingTriggeredHandler TimingTriggered;

        public int MaxSensorIdValue { get; set; }
        public double TimerIntervalMean { get; set; } = 5000;
        public double TimerIntervalStdDev { get; set; } = 2000;
        public double TimerFailureChance { get; set; } = 0.15;
        public double UnwantedTimerTriggerChance { get; set; } = 0.1;

        private void SimulateTimer(object state)
        {
            var randomizer = new Random();

            while (true)
            {
                if (randomizer.NextDouble() > this.UnwantedTimerTriggerChance)
                {
                    Thread.Sleep(
                        (int)Core.Statistics.NormalDistribution.GenerateObservation(
                            this.TimerIntervalMean,
                            this.TimerIntervalStdDev));
                }

                if (randomizer.NextDouble() > this.TimerFailureChance)
                {
                    int sensorId = randomizer.Next(this.MaxSensorIdValue) + 1;
                    this.TimingTriggered?.Invoke(sensorId, DateTime.Now);
                }
            }
        }
    }
}
