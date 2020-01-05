using Hurace.Core.BL;
using Hurace.Domain;
using Hurace.RaceControl.ViewModels.Shared;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

#pragma warning disable CA2227 // Collection properties should be read only
namespace Hurace.RaceControl.ViewModels
{
    public class RaceDetailViewModel : BaseViewModel
    {
        private readonly RaceClockResolver raceClockResolver;
        private readonly IInformationManager raceInformationManager;
        private readonly IRaceExecutionManager raceExecutionManager;
        private bool executionRunning;
        private bool raceCompleted;
        private ObservableCollection<StartPosition> startList;

        public RaceDetailViewModel(
            RaceClockResolver raceClockResolver,
            IInformationManager raceInformationManager,
            IRaceExecutionManager raceExecutionManager)
        {
            this.raceClockResolver = raceClockResolver ?? throw new ArgumentNullException(nameof(raceClockResolver));
            this.raceInformationManager = raceInformationManager ?? throw new ArgumentNullException(nameof(raceInformationManager));
            this.raceExecutionManager = raceExecutionManager ?? throw new ArgumentNullException(nameof(raceExecutionManager));

            startList = new ObservableCollection<StartPosition>();

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

        public ObservableCollection<StartPosition> StartList
        {
            get => startList;
            set => base.Set(ref this.startList, value);
        }

        public Race Race { get; set; }

        public AsyncDelegateCommand StartRaceExecutionCommand { get; }

        public AsyncDelegateCommand StartSimulatedRaceExecutionCommand { get; }

        public AsyncDelegateCommand StopRaceExecutionCommand { get; set; }

        public async Task LoadRaceData()
        {
            var tempRace = await raceInformationManager.GetRaceByIdAsync(Race.Id,
                    Associated<RaceType>.LoadingType.Reference,
                    Associated<Venue>.LoadingType.Reference,
                    Associated<Season>.LoadingType.Reference,
                    Associated<StartPosition>.LoadingType.Reference,
                    Associated<Skier>.LoadingType.Reference,
                    Associated<Sex>.LoadingType.Reference,
                    Associated<Country>.LoadingType.Reference)
                .ConfigureAwait(false);

            foreach (var sp in tempRace.FirstStartList)
            {
                Application.Current.Dispatcher.Invoke(
                    () => StartList.Add(sp.Reference));
            }

            var sl = startList;
            StartList = sl;
        }

        public bool CanStartRaceExecution(object argument)
        {
            return !ExecutionRunning;
        }

        public async Task StartRaceExecution(object argument)
        {
            MessageBox.Show("not supported yet");

            await Task.CompletedTask.ConfigureAwait(false);
            //await this.StartRaceLogic(this.raceClockResolver(false));
        }

        public bool CanStartSimulatedRaceExecution(object argument)
        {
            return this.CanStartRaceExecution(argument);
        }

        public async Task StartSimulatedRaceExecution(object argument)
        {
            var simulation = this.raceClockResolver(true) as Simulator.RaceClockSimulation;
            var simulatorConfigWindow = new Windows.SimulatorConfigWindow(simulation);
            simulatorConfigWindow.ShowDialog();

            await this.StartRaceLogic(simulation)
                .ConfigureAwait(false);
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
            this.raceExecutionManager.RaceClock = raceClock;

            var race = await raceInformationManager.GetRaceByIdAsync(
                    this.Race.Id,
                    raceTypeLoadingType: Associated<RaceType>.LoadingType.None,
                    venueLoadingType: Associated<Venue>.LoadingType.Reference,
                    seasonLoadingType: Associated<Season>.LoadingType.Reference,
                    startListLoadingType: Associated<StartPosition>.LoadingType.Reference,
                    skierLoadingType: Associated<Skier>.LoadingType.Reference,
                    skierSexLoadingType: Associated<Sex>.LoadingType.None,
                    skierCountryLoadingType: Associated<Country>.LoadingType.Reference)
                .ConfigureAwait(false);

            Application.Current.Dispatcher.Invoke(
                () =>
                {
                    this.ExecutionRunning = true;
                    this.Race = race;
                });
        }
    }
}
