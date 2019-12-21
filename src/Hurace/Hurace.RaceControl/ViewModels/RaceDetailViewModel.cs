using Hurace.Core.BL;
using Hurace.Domain;
using Hurace.RaceControl.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        public async Task StartRaceExecution(object argument)
        {
            MessageBox.Show("not supported yet");
            //await this.StartRaceLogic(this.raceClockResolver(false));
        }

        public bool CanStartSimulatedRaceExecution(object argument)
        {
            return this.CanStartRaceExecution(argument);
        }

        public async Task StartSimulatedRaceExecution(object argument)
        {
            MessageBox.Show("start real race");

            var simulation = this.raceClockResolver(true) as Simulator.RaceClockSimulation;
            var simulatorConfigWindow = new Windows.SimulatorConfigWindow(simulation);
            simulatorConfigWindow.ShowDialog();

            await this.StartRaceLogic(simulation);
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

        private async Task StartRaceLogic(Timer.IRaceClock raceClock)
        {
            this.ExecutionRunning = true;

            this.raceExecutionManager.RaceClock = raceClock;

            this.Race = await raceInformationManager.GetRaceByIdAsync(
                this.Race.Id,
                raceTypeLoadingType: Associated<RaceType>.LoadingType.None,
                venueLoadingType: Associated<Venue>.LoadingType.Reference,
                seasonLoadingType: Associated<Season>.LoadingType.Reference,
                startListLoadingType: Associated<StartPosition>.LoadingType.Reference,
                skierLoadingType: Associated<Skier>.LoadingType.Reference,
                skierSexLoadingType: Associated<Sex>.LoadingType.None,
                skierCountryLoadingType: Associated<Country>.LoadingType.Reference);
        }
    }
}
