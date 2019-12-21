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
        private int sensorIdMaxValue;
        private readonly RaceClockSimulation raceClockSimulation;

        private CancellationTokenSource cancellationTokenSource;
        private readonly EventWaitHandle sensorSimulationExecutionHandle;

        public SimulatorConfigViewModel(RaceClockSimulation raceClockSimulation)
        {
            this.sensorSimulationExecutionHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

            this.ContinueToSimulationCommand = new AsyncDelegateCommand(
                this.ContinueToSensorSimulation);
            this.raceClockSimulation = raceClockSimulation ?? throw new ArgumentNullException(nameof(raceClockSimulation));
        }

        public int MaxSensorIdValue
        {
            get => this.raceClockSimulation.MaxSensorIdValue;
            set => base.Set(ref this.sensorIdMaxValue, value);
        }

        public AsyncDelegateCommand ContinueToSimulationCommand { get; set; }

        #region Methods
        #region InputValidators

        //todo: validate numerical input

        #endregion
        #region Command Methods

        public Task ContinueToSensorSimulation(object parameter)
        {
            if (parameter is null)
                throw new ArgumentNullException(nameof(parameter));
            else if (parameter is Window simulatorConfigWindow)
            {
                simulatorConfigWindow.Close();
                
                return Task.CompletedTask;
            }
            else
                throw new InvalidOperationException("Command parameter is not the window");
        }

        #endregion
        #region Helper

        private void SimulateTimeSensors(object param)
        {
            var token = (CancellationToken)param;
            while (!token.IsCancellationRequested)
            {
                //RaceClockSimulation.Instance.RaiseRaceClockEvent(0, DateTime.Now);
                Thread.Sleep(2000);
            }

            this.sensorSimulationExecutionHandle.Set();
        }

        #endregion
        #endregion
    }
}
