using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

#pragma warning disable CA1001 // Types that own disposable fields should be disposable
namespace Hurace.Simulator.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private int sensorIdMaxValue;

        private CancellationTokenSource cancellationTokenSource;
        private readonly EventWaitHandle sensorSimulationExecutionHandle;

        public MainViewModel()
        {
            this.sensorSimulationExecutionHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

            this.StartSensorSimulationCommand = new AsyncDelegateCommand(
                this.StartSensorSimulation,
                this.CanStartSensorSimulation);

            this.StopSensorSimulationCommand = new AsyncDelegateCommand(
                this.StopSensorSimulation,
                this.CanStopSensorSimulation);
        }

        public int SensorIdMaxValue
        {
            get => sensorIdMaxValue;
            set => base.Set(ref this.sensorIdMaxValue, value);
        }

        public AsyncDelegateCommand StartSensorSimulationCommand { get; set; }
        public AsyncDelegateCommand StopSensorSimulationCommand { get; set; }

        #region Methods
        #region InputValidators

        //todo: validate numerical input

        #endregion
        #region Command Methods

        public bool CanStartSensorSimulation(object parameter = null)
        {
            return this.cancellationTokenSource == null;
        }

        public Task StartSensorSimulation(object parameter = null)
        {
            if (this.cancellationTokenSource == null)
            {
                this.cancellationTokenSource = new CancellationTokenSource();
                ThreadPool.QueueUserWorkItem(SimulateTimeSensors, this.cancellationTokenSource.Token);

                return Task.CompletedTask;
            }
            else
            {
                throw new InvalidOperationException("Simulation already running -> nothing to start");
            }
        }

        public bool CanStopSensorSimulation(object parameter = null)
        {
            return this.cancellationTokenSource != null;
        }

        public Task StopSensorSimulation(object parameter = null)
        {
            if (this.cancellationTokenSource != null)
            {
                this.cancellationTokenSource.Cancel();
                return Task.Run(
                    () =>
                    {
                        this.sensorSimulationExecutionHandle.WaitOne();

                        this.cancellationTokenSource = null;
                    });
            }
            else
            {
                throw new InvalidOperationException("Simulation not running -> nothing to stop");
            }
        }

        #endregion
        #region Helper

        private void SimulateTimeSensors(object param)
        {
            var token = (CancellationToken)param;
            while (!token.IsCancellationRequested)
            {
                RaceClock.Instance.RaiseRaceClockEvent(0, DateTime.Now);
                Thread.Sleep(2000);
            }

            this.sensorSimulationExecutionHandle.Set();
        }

        #endregion
        #endregion
    }
}
