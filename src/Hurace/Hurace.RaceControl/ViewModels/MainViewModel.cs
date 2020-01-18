using Hurace.Core.BL;
using Hurace.RaceControl.ViewModels.Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

#pragma warning disable CA1822 // Mark members as static
namespace Hurace.RaceControl.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Fields
        #region LayoutStates

        private bool createRaceVisible;
        private bool executionRunning;
        private bool raceDetailViewVisible;
        private bool createRaceButtonVisible;

        #endregion
        #region ViewModels

        private RaceDetailViewModel selectedRace;
        private CreateRaceViewModel createRaceViewModel;

        #endregion
        #region Dependencies

        private readonly IServiceProvider serviceProvider;
        private readonly RaceClockResolver raceClockResolver;
        private readonly IInformationManager informationManager;
        private readonly IRaceExecutionManager raceExecutionManager;

        #endregion
        #endregion
        #region Constructor

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

            this.createRaceVisible = false;

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
                this.DeleteRace,
                this.CanDeleteRace);
            this.AbortRaceCreateOrUpdateCommand = new AsyncDelegateCommand(this.AbortRaceCreateOrUpdate);

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
                },
                this.CanEditRace);
        }

        #endregion
        #region Properties
        #region Commands

        public AsyncDelegateCommand CreateOrUpdateRaceCommand { get; }
        public AsyncDelegateCommand OpenEditRaceCommand { get; }
        public AsyncDelegateCommand OpenCreateRaceCommand { get; set; }
        public AsyncDelegateCommand StartRaceExecutionCommand { get; }
        public AsyncDelegateCommand StartSimulatedRaceExecutionCommand { get; }
        public AsyncDelegateCommand StopRaceExecutionCommand { get; }
        public AsyncDelegateCommand DeleteRaceCommand { get; }
        public AsyncDelegateCommand AbortRaceCreateOrUpdateCommand { get; set; }

        #endregion

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

        public bool ExecutionRunning
        {
            get => executionRunning;
            set
            {
                base.Set(ref this.executionRunning, value);
                base.NotifyPropertyChanged(nameof(RaceNotCompleted));
            }
        }

        public bool RaceNotStarted { get; set; }

        public bool RaceNotCompleted =>
            this.ExecutionRunning ||
            this.SelectedRace?.Race?.OverallRaceState?.Reference?.Id == 3 ||
            this.SelectedRace?.Race?.OverallRaceState?.Reference?.Id == 4;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        public RaceDetailViewModel SelectedRace
        {
            get => selectedRace;
            set
            {
                base.Set(ref this.selectedRace, value);

                this.RaceDetailControlVisible = value != null;
                this.CreateRaceControlVisible = false;

                if (value != null)
                {
                    this.InitializeSelectedRace();
                }
            }
        }
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        public CreateRaceViewModel CreateRaceViewModel
        {
            get => createRaceViewModel;
            set => base.Set(ref this.createRaceViewModel, value);
        }

        public ObservableCollection<RaceDetailViewModel> RaceListItemViewModels { get; private set; }

        #endregion
        #region Methods
        #region Command-Methods

        private async Task DeleteRace(object obj)
        {
            await this.informationManager.DeleteRaceAsync(this.SelectedRace.Race.Id).ConfigureAwait(false);
            Application.Current.Dispatcher.Invoke(
                () =>
                {
                    this.RaceListItemViewModels.Remove(this.SelectedRace);
                    this.SelectedRace = null;
                });
        }

        private bool CanDeleteRace(object obj)
        {
            return !this.ExecutionRunning;
        }

        private bool CanEditRace(object obj)
        {
            return !this.ExecutionRunning && this.RaceNotStarted;
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

            simulation.MaxSensorIdValue = this.SelectedRace.Race.NumberOfSensors - 1;

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
                    raceTypeLoadingType: Domain.Associated<Domain.RaceType>.LoadingType.Reference,
                    venueLoadingType: Domain.Associated<Domain.Venue>.LoadingType.Reference,
                    seasonLoadingType: Domain.Associated<Domain.Season>.LoadingType.Reference,
                    startListLoadingType: Domain.Associated<Domain.StartPosition>.LoadingType.Reference,
                    skierLoadingType: Domain.Associated<Domain.Skier>.LoadingType.Reference,
                    skierSexLoadingType: Domain.Associated<Domain.Sex>.LoadingType.None,
                    skierCountryLoadingType: Domain.Associated<Domain.Country>.LoadingType.Reference)
                .ConfigureAwait(false);

            await this.SelectedRace.UpdateStartingSkiers()
                .ConfigureAwait(false);

            Application.Current.Dispatcher.Invoke(
                () =>
                {
                    this.SelectedRace.Race = race;
                    this.ExecutionRunning = true;
                });
        }

        private Task AbortRaceCreateOrUpdate(object arg)
        {
            this.CreateRaceControlVisible = false;
            return Task.CompletedTask;
        }

        #endregion
        #region Async-Loaders

        internal async Task InitializeAsync()
        {
            var raceListDetailViewModels = (await this.informationManager.GetAllRacesAsync(
                        raceTypeLoadingType: Domain.Associated<Domain.RaceType>.LoadingType.Reference,
                        venueLoadingType: Domain.Associated<Domain.Venue>.LoadingType.Reference,
                        seasonLoadingType: Domain.Associated<Domain.Season>.LoadingType.Reference)
                    .ConfigureAwait(false))
                .OrderBy(race => race.Date)
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

        public async Task InitializeSelectedRace()
        {
            var race = await informationManager.GetRaceByIdAsync(
                    this.selectedRace.Race.Id,
                    Domain.Associated<Domain.RaceState>.LoadingType.Reference,
                    Domain.Associated<Domain.RaceType>.LoadingType.Reference,
                    Domain.Associated<Domain.Venue>.LoadingType.Reference,
                    Domain.Associated<Domain.Season>.LoadingType.Reference,
                    Domain.Associated<Domain.StartPosition>.LoadingType.Reference,
                    Domain.Associated<Domain.Skier>.LoadingType.Reference,
                    Domain.Associated<Domain.Sex>.LoadingType.Reference,
                    Domain.Associated<Domain.Country>.LoadingType.Reference)
                .ConfigureAwait(true);

            this.RaceNotStarted = await informationManager.WasRaceNeverStartedAsync(race.Id).ConfigureAwait(true);

            if (race.OverallRaceState.Reference.Id != 3 &&
                race.OverallRaceState.Reference.Id != 4 &&
                race.Id == this.SelectedRace.Race.Id)
            {
                await this.SelectedRace.LoadRankList().ConfigureAwait(true);
            }

            this.SelectedRace.Race = race;
            base.NotifyPropertyChanged(nameof(RaceNotCompleted));
        }

        #endregion
        #endregion
    }
}
