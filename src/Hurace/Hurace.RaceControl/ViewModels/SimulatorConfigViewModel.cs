using Hurace.RaceControl.ViewModels.Shared;
using Hurace.Simulator;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

#pragma warning disable CA1001 // Types that own disposable fields should be disposable
namespace Hurace.RaceControl.ViewModels
{
    public class SimulatorConfigViewModel : BaseViewModel
    {
        private double timerIntervalMean;
        private int maxSensorIdValue;
        private double timerIntervalStdDev;
        private double timerFailureChance;
        private double unwantedTimerTriggerChance;
        private readonly RaceClockSimulation raceClockSimulation;

        public SimulatorConfigViewModel(RaceClockSimulation raceClockSimulation)
        {
            this.ContinueToSimulationCommand = new AsyncDelegateCommand(
                this.ContinueToSensorSimulation);

            this.raceClockSimulation = raceClockSimulation ?? throw new ArgumentNullException(nameof(raceClockSimulation));

            this.MaxSensorIdValue = this.raceClockSimulation.MaxSensorIdValue;
            this.TimerIntervalMean = this.raceClockSimulation.TimerIntervalMean;
            this.TimerIntervalStdDev = this.raceClockSimulation.TimerIntervalStdDev;
            this.TimerFailureChance = this.raceClockSimulation.TimerFailureChance;
            this.UnwantedTimerTriggerChance = this.raceClockSimulation.UnwantedTimerTriggerChance;
        }

        public int MaxSensorIdValue
        {
            get => this.maxSensorIdValue;
            set => base.Set(ref this.maxSensorIdValue, value);
        }

        public double TimerIntervalMean
        {
            get => timerIntervalMean;
            set => base.Set(ref this.timerIntervalMean, value);
        }

        public double TimerIntervalStdDev
        {
            get => timerIntervalStdDev;
            set => base.Set(ref timerIntervalStdDev, value);
        }

        public double TimerFailureChance
        {
            get => timerFailureChance;
            set => base.Set(ref timerFailureChance, value);
        }

        public double UnwantedTimerTriggerChance
        {
            get => unwantedTimerTriggerChance;
            set => base.Set(ref unwantedTimerTriggerChance, value);
        }

        public AsyncDelegateCommand ContinueToSimulationCommand { get; set; }

        public Task ContinueToSensorSimulation(object parameter)
        {
            if (parameter is null)
                throw new ArgumentNullException(nameof(parameter));
            else if (parameter is Window simulatorConfigWindow)
            {
                simulatorConfigWindow.Close();

                this.raceClockSimulation.MaxSensorIdValue = this.MaxSensorIdValue;
                this.raceClockSimulation.TimerIntervalMean = this.timerIntervalMean;
                this.raceClockSimulation.TimerIntervalStdDev = this.TimerIntervalStdDev;
                this.raceClockSimulation.TimerFailureChance = this.TimerFailureChance;
                this.raceClockSimulation.UnwantedTimerTriggerChance = this.UnwantedTimerTriggerChance;

                return Task.CompletedTask;
            }
            else
                throw new InvalidOperationException("Command parameter is not the window");
        }
    }
}
