using Hurace.Core.BL;
using Hurace.Domain;
using Hurace.RaceControl.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceDetailViewModel : BaseViewModel
    {
        private readonly RaceClockResolver raceClockResolver;
        private readonly IRaceInformationManager raceInformationManager;
        private readonly IRaceExecutionManager raceExecutionManager;
        private bool executionRunning;

        public RaceDetailViewModel(
            RaceClockResolver raceClockResolver,
            IRaceInformationManager raceInformationManager,
            IRaceExecutionManager raceExecutionManager)
        {
            this.raceClockResolver = raceClockResolver ?? throw new ArgumentNullException(nameof(raceClockResolver));
            this.raceInformationManager = raceInformationManager ?? throw new ArgumentNullException(nameof(raceInformationManager));
            this.raceExecutionManager = raceExecutionManager ?? throw new ArgumentNullException(nameof(raceExecutionManager));

            this.StartRaceExecutionCommand = new AsyncDelegateCommand(
                this.StartRaceExecution,
                this.CanStartRaceExecution);
            this.StartSimulatedRaceExecutionCommand = new AsyncDelegateCommand(
                this.StartSimulatedRaceExecution,
                this.CanStartSimulatedRaceExecution);
            this.StopRaceExecutionCommand = new AsyncDelegateCommand(
                this.StopRaceExecution,
                this.CanStopRaceExecution);
        }


        public bool ExecutionRunning
        {
            get => executionRunning;
            set => base.Set(ref this.executionRunning, value);
        }

        public Race Race { get; set; }
        public AsyncDelegateCommand StartRaceExecutionCommand { get; }
        public AsyncDelegateCommand StartSimulatedRaceExecutionCommand { get; }
        public AsyncDelegateCommand StopRaceExecutionCommand { get; set; }
        public ObservableCollection<StartPosition> StartList { get; }

        public bool CanStartRaceExecution(object argument)
        {
            return !ExecutionRunning;
        }

        public Task StartRaceExecution(object argument)
        {
            MessageBox.Show("start real race");
            this.ExecutionRunning = true;
            return Task.CompletedTask;
        }

        public bool CanStartSimulatedRaceExecution(object argument)
        {
            return this.CanStartRaceExecution(argument);
        }

        public Task StartSimulatedRaceExecution(object argument)
        {
            MessageBox.Show("start real simulated race");
            this.ExecutionRunning = true;
            return Task.CompletedTask;
        }

        public bool CanStopRaceExecution(object argument)
        {
            return this.ExecutionRunning;
        }

        public Task StopRaceExecution(object argument)
        {
            MessageBox.Show("stop race execution");
            this.ExecutionRunning = false;
            return Task.CompletedTask;
        }
    }
}
