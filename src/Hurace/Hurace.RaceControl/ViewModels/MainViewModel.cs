using Hurace.Core.BL;
using Hurace.RaceControl.ViewModels.Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Hurace.RaceControl.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private bool createRaceVisible = false;
        private bool executionRunning;
        private bool raceDetailViewVisible;
        private bool createRaceButtonVisible;

        private RaceDetailViewModel selectedRace;
        private CreateRaceViewModel createRaceViewModel;

        private readonly IServiceProvider serviceProvider;
        private readonly RaceClockResolver raceClockResolver;
        private readonly IInformationManager informationManager;
        private readonly IRaceExecutionManager raceExecutionManager;

        public MainViewModel(
            IServiceProvider serviceProvider,
            RaceClockResolver raceClockResolver,
            IInformationManager informationManager,
            IRaceExecutionManager raceExecutionManager)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.raceClockResolver = raceClockResolver ?? throw new ArgumentNullException(nameof(raceClockResolver));
            this.informationManager = informationManager ?? throw new ArgumentNullException(nameof(informationManager));
            this.raceExecutionManager = raceExecutionManager ?? throw new ArgumentNullException(nameof(raceExecutionManager));
            this.RaceListItemViewModels = new ObservableCollection<RaceDetailViewModel>();
            this.createRaceViewModel = new CreateRaceViewModel(informationManager);

            this.StartRaceExecutionCommand = new AsyncDelegateCommand(
                this.StartRaceExecution,
                this.CanStartRaceExecution);
            this.StartSimulatedRaceExecutionCommand = new AsyncDelegateCommand(
                this.StartSimulatedRaceExecution,
                this.CanStartSimulatedRaceExecution);
            this.StopRaceExecutionCommand = new AsyncDelegateCommand(
                this.StopRaceExecution,
                this.CanStopRaceExecution);
            this.DeleteRaceCommand = new AsyncDelegateCommand(
                this.DeleteRace);

            this.OpenCreateRaceCommand = new AsyncDelegateCommand(
                async _ =>
                {
                    Application.Current.Dispatcher.Invoke(
                        () =>
                        {
                            this.CreateRaceButtonVisible = true;
                            this.CreateRaceControlVisible = true;
                            this.RaceDetailControlVisible = false;
                        });

                    await this.CreateRaceViewModel.Initialize()
                        .ConfigureAwait(false);
                });

            this.CreateOrUpdateRaceCommand = new AsyncDelegateCommand(
                async _ =>
                {
                    var raceDetailViewModel = this.serviceProvider.GetRequiredService<RaceDetailViewModel>();

                    var tempRace = await informationManager.GetRaceByIdAsync(
                                await createRaceViewModel.CreateOrUpdateRace(new object())
                                    .ConfigureAwait(false),
                                raceTypeLoadingType: Domain.Associated<Domain.RaceType>.LoadingType.Reference,
                                venueLoadingType: Domain.Associated<Domain.Venue>.LoadingType.Reference,
                                seasonLoadingType: Domain.Associated<Domain.Season>.LoadingType.Reference)
                        .ConfigureAwait(false);

                    raceDetailViewModel.Race = tempRace;

                    Application.Current.Dispatcher.Invoke(
                        () =>
                        {
                            this.RaceListItemViewModels.Add(raceDetailViewModel);
                            this.CreateRaceButtonVisible = false;
                            this.CreateRaceControlVisible = false;
                            this.RaceDetailControlVisible = true;
                        });
                },
                null);

            this.OpenEditRaceCommand = new AsyncDelegateCommand(
                async _ =>
                {
                    Application.Current.Dispatcher.Invoke(
                        () =>
                        {
                            this.CreateRaceButtonVisible = false;
                            this.CreateRaceControlVisible = true;
                            this.RaceDetailControlVisible = false;
                        });

                    await this.CreateRaceViewModel.InitializeExistingRace(SelectedRace.Race)
                        .ConfigureAwait(false);
                });

        }

        private Task DeleteRace(object obj)
        {
            throw new NotImplementedException();
        }

        public bool ExecutionRunning
        {
            get => executionRunning;
            set => base.Set(ref this.executionRunning, value);
        }

        public AsyncDelegateCommand CreateOrUpdateRaceCommand { get; }
        public AsyncDelegateCommand OpenEditRaceCommand { get; }
        public AsyncDelegateCommand OpenCreateRaceCommand { get; set; }
        public AsyncDelegateCommand StartRaceExecutionCommand { get; }
        public AsyncDelegateCommand StartSimulatedRaceExecutionCommand { get; }
        public AsyncDelegateCommand StopRaceExecutionCommand { get; }
        public AsyncDelegateCommand DeleteRaceCommand { get; }

        public ObservableCollection<RaceDetailViewModel> RaceListItemViewModels { get; private set; }

        public bool CreateRaceButtonVisible
        {
            get => createRaceButtonVisible;
            set => base.Set(ref this.createRaceButtonVisible, value);
        }

        public bool CreateRaceControlVisible
        {
            get => createRaceVisible;
            set => base.Set(ref this.createRaceVisible, value);
        }

        public bool RaceDetailControlVisible
        {
            get => raceDetailViewVisible;
            set => base.Set(ref this.raceDetailViewVisible, value);
        }

        public RaceDetailViewModel SelectedRace
        {
            get => selectedRace;
            set
            {
                base.Set(ref this.selectedRace, value);
                this.RaceDetailControlVisible = true;
                this.CreateRaceControlVisible = false;
                this.SelectedRace.LoadRaceData();
            }
        }

        public CreateRaceViewModel CreateRaceViewModel
        {
            get => createRaceViewModel;
            set => base.Set(ref this.createRaceViewModel, value);
        }

        internal async Task InitializeAsync()
        {
            var raceListDetailViewModels =
                (await this.informationManager.GetAllRacesAsync(
                        raceTypeLoadingType: Domain.Associated<Domain.RaceType>.LoadingType.Reference,
                        venueLoadingType: Domain.Associated<Domain.Venue>.LoadingType.Reference,
                        seasonLoadingType: Domain.Associated<Domain.Season>.LoadingType.Reference)
                    .ConfigureAwait(false))
                .Select(race => (this.serviceProvider.GetRequiredService<RaceDetailViewModel>(), race));

            foreach (var (raceDetailViewModel, race) in raceListDetailViewModels)
            {
                raceDetailViewModel.Race = race;

                Application.Current.Dispatcher.Invoke(
                    () => this.RaceListItemViewModels.Add(raceDetailViewModel));
            }

            await createRaceViewModel.Initialize()
                .ConfigureAwait(false);
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

            var race = await informationManager.GetRaceByIdAsync(
                    this.SelectedRace.Race.Id,
                    raceTypeLoadingType: Domain.Associated<Domain.RaceType>.LoadingType.None,
                    venueLoadingType: Domain.Associated<Domain.Venue>.LoadingType.Reference,
                    seasonLoadingType: Domain.Associated<Domain.Season>.LoadingType.Reference,
                    startListLoadingType: Domain.Associated<Domain.StartPosition>.LoadingType.Reference,
                    skierLoadingType: Domain.Associated<Domain.Skier>.LoadingType.Reference,
                    skierSexLoadingType: Domain.Associated<Domain.Sex>.LoadingType.None,
                    skierCountryLoadingType: Domain.Associated<Domain.Country>.LoadingType.Reference)
                .ConfigureAwait(false);

            Application.Current.Dispatcher.Invoke(
                () =>
                {
                    this.ExecutionRunning = true;
                    this.SelectedRace.Race = race;
                });
        }
    }
}
