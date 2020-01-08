using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable CA1031 // Do not catch general exception types
#pragma warning disable CA5394 // Do not use insecure randomness
namespace Hurace.Simulator
{
    public class RaceClockSimulation : Timer.IRaceClock
    {
        private const int MaxTimerInterval = 20000;
#if DEBUG
        private const string TimeFormat = "HH:mm:ss.f";
#endif

        private readonly Thread simulation;

        public RaceClockSimulation()
        {
            this.TimerIntervalMean = 5000;
            this.TimerIntervalStdDev = 2000;
            this.TimerFailureChance = 0.05;
            this.UnwantedTimerTriggerChance = 0.05;

            this.simulation = new Thread(this.SimulateTimer);
            this.simulation.Start();
        }

        public event Timer.TimingTriggeredHandler TimingTriggered;

        public int MaxSensorIdValue { get; set; }
        public double TimerIntervalMean { get; set; }
        public double TimerIntervalStdDev { get; set; }
        public double TimerFailureChance { get; set; }
        public double UnwantedTimerTriggerChance { get; set; }

        private void SimulateTimer()
        {
            var randomizer = new Random();
            int currentSensorId = 0;

            while (true)
            {
                try
                {
                    if (randomizer.NextDouble() > this.UnwantedTimerTriggerChance)
                    {
                        var generatedInterval =
                            Math.Abs(
                                (int)Core.Statistics.NormalDistribution.GenerateObservation(
                                    this.TimerIntervalMean,
                                    this.TimerIntervalStdDev));

                        Thread.Sleep(generatedInterval > MaxTimerInterval ? MaxTimerInterval : generatedInterval);
                    }

                    currentSensorId =
                        currentSensorId == this.MaxSensorIdValue
                        ? 0
                        : currentSensorId + 1;

                    if (randomizer.NextDouble() > this.TimerFailureChance)
                    {

                        this.TimingTriggered?.Invoke(currentSensorId, DateTime.Now);

#if DEBUG
                        Debug.WriteLine($"{nameof(RaceClockSimulation)} ({DateTime.Now.ToString(TimeFormat)}): " +
                            $"triggered sensor {currentSensorId}");
#endif
                    }
                    else
                    {
#if DEBUG
                        Debug.WriteLine($"{nameof(RaceClockSimulation)} ({DateTime.Now.ToString(TimeFormat)}): " +
                            $"skipped sensor {currentSensorId}");
#endif
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    Debug.WriteLine(
                        $"{nameof(RaceClockSimulation)}: caught exception of type '{ex.GetType().Name}' " +
                        $"with message '{ex.Message}'");
#endif
                }
            }
        }
    }
}
