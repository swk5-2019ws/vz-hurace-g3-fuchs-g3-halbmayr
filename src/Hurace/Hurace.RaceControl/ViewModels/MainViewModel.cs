using Hurace.Core.BL;
using Hurace.RaceControl.ViewModels.Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

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
        private bool isCreateOperation;
        private bool createOrUpdateOperationCurrentlyRunning;

        #endregion
        #region ViewModels

        private RaceDetailViewModel selectedRace;
        private CreateRaceViewModel createRaceViewModel;
        private bool raceNotStarted;

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
            this.createOrUpdateOperationCurrentlyRunning = false;

            this.Stopwatch = new Stopwatch();
            this.DispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(10)
            };
            this.DispatcherTimer.Tick += this.OnDispatcherTimer;

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
            this.AbortRaceCreateOrUpdateCommand = new AsyncDelegateCommand(
                this.AbortRaceCreateOrUpdate,
                this.CanAbortRaceCreateOrUpdate);
            this.CreateOrUpdateRaceCommand = new AsyncDelegateCommand(
                this.CreateOrUpdateRace,
                this.CanCreateOrUpdateRace);
            this.CurrentSkierScreenCommand = new AsyncDelegateCommand(
                this.CreateCurrentSkierWindow,
                this.CanCreateCurrentSkierWindow);
            this.CurrentResultScreenCommand = new AsyncDelegateCommand(
                this.CreateCurrentResultWindow,
                this.CanCreateCurrentResultWindow);

            this.OpenCreateRaceCommand = new AsyncDelegateCommand(
                async _ =>
                {
                    this.isCreateOperation = true;

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

            this.OpenEditRaceCommand = new AsyncDelegateCommand(
                async _ =>
                {
                    this.isCreateOperation = false;

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

            this.ExecutionRunning = false;
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
        public AsyncDelegateCommand CurrentSkierScreenCommand { get; }
        public AsyncDelegateCommand CurrentResultScreenCommand { get; }
        public AsyncDelegateCommand AbortRaceCreateOrUpdateCommand { get; set; }

        #endregion

        public Stopwatch Stopwatch { get; private set; }

        public DispatcherTimer DispatcherTimer { get; private set; }

        public Window RaceExecutionWindow { get; set; }

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
                base.NotifyPropertyChanged(nameof(NotExecutionRunning));
                base.NotifyPropertyChanged(nameof(RaceNotCompleted));
                base.NotifyPropertyChanged(nameof(RankListVisible));
                base.NotifyPropertyChanged(nameof(RaceExecutionButtonVisible));
            }
        }

        public bool NotExecutionRunning => !this.ExecutionRunning;

        public bool RaceNotStarted
        {
            get => raceNotStarted;
            set
            {
                raceNotStarted = value;
                base.NotifyPropertyChanged(nameof(RankListVisible));
            }
        }

        public bool RaceExecutionButtonVisible => !this.ExecutionRunning && this.RaceNotCompleted;

        public bool RaceNotCompleted =>
            this.ExecutionRunning ||
            this.SelectedRace?.Race?.OverallRaceState?.Reference?.Id == 3 ||
            this.SelectedRace?.Race?.OverallRaceState?.Reference?.Id == 4;

        public bool RankListVisible => !this.ExecutionRunning && !this.RaceNotStarted;

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

        private bool CanCreateCurrentSkierWindow(object obj)
        {
            return RaceExecutionWindow != null
                ? RaceExecutionWindow.GetType() == typeof(Windows.CurrentResultWindow)
                : true;
        }

        private bool CanCreateCurrentResultWindow(object obj)
        {
            return RaceExecutionWindow != null
                ? RaceExecutionWindow.GetType() == typeof(Windows.CurrentSkierWindow)
                : true;
        }

        private Task CreateCurrentSkierWindow(object arg)
        {
            if (this.RaceExecutionWindow != null)
                RaceExecutionWindow.Close();

            RaceExecutionWindow = new Windows.CurrentSkierWindow
            {
                DataContext = this.SelectedRace
            };

            RaceExecutionWindow.Show();

            return Task.CompletedTask;
        }

        private Task CreateCurrentResultWindow(object arg)
        {
            if (this.RaceExecutionWindow != null)
                RaceExecutionWindow.Close();

            RaceExecutionWindow = new Windows.CurrentResultWindow
            {
                DataContext = this.SelectedRace
            };

            RaceExecutionWindow.Show();

            return Task.CompletedTask;
        }

        private bool CanCreateOrUpdateRace(object obj)
        {
            return this.CreateRaceViewModel.SelectedDate != default &&
                this.CreateRaceViewModel.SelectedNumberOfSensors != 0 &&
                this.CreateRaceViewModel.SelectedRaceType != null &&
                this.CreateRaceViewModel.SelectedVenue != null &&
                this.CreateRaceViewModel.SelectedSeason != null &&
                this.CreateRaceViewModel.StartPositions != null &&
                this.CreateRaceViewModel.StartPositions.Count > 0;
        }

        private async Task CreateOrUpdateRace(object arg)
        {
            this.createOrUpdateOperationCurrentlyRunning = true;

            var createdOrUpdatedRaceId = await createRaceViewModel.CreateOrUpdateRace(null).ConfigureAwait(false);

            var createdOrUpdatedRace = await informationManager.GetRaceByIdAsync(
                        createdOrUpdatedRaceId,
                        raceTypeLoadingType: Domain.Associated<Domain.RaceType>.LoadingType.Reference,
                        venueLoadingType: Domain.Associated<Domain.Venue>.LoadingType.Reference,
                        seasonLoadingType: Domain.Associated<Domain.Season>.LoadingType.Reference)
                .ConfigureAwait(false);

            RaceDetailViewModel createdOrUpdatedRaceVM;
            if (this.isCreateOperation)
            {
                createdOrUpdatedRaceVM = this.serviceProvider.GetRequiredService<RaceDetailViewModel>();
                createdOrUpdatedRaceVM.Race = createdOrUpdatedRace;

                Application.Current.Dispatcher.Invoke(
                    () =>
                    {
                        var insertIndex = this.RaceListItemViewModels
                            .ToList()
                            .FindLastIndex(raceVM => DateTime.Compare(raceVM.Race.Date, createdOrUpdatedRaceVM.Race.Date) <= 0) + 1;
                        this.RaceListItemViewModels.Insert(insertIndex, createdOrUpdatedRaceVM);
                    });
            }
            else
            {
                createdOrUpdatedRaceVM = this.RaceListItemViewModels
                    .ToList()
                    .Find(raceVM => raceVM.Race.Id == createdOrUpdatedRaceId);
            }

            Application.Current.Dispatcher.Invoke(
                () =>
                {
                    this.SelectedRace = createdOrUpdatedRaceVM;

                    this.CreateRaceButtonVisible = false;
                    this.CreateRaceControlVisible = false;
                    this.RaceDetailControlVisible = true;
                });

            this.createOrUpdateOperationCurrentlyRunning = false;
        }

        private bool CanAbortRaceCreateOrUpdate(object obj)
        {
            return !this.createOrUpdateOperationCurrentlyRunning;
        }

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
            return this.ExecutionRunning && !this.SelectedRace.currentlyRunning;
        }

        public async Task StopRaceExecution(object argument)
        {
            this.RaceExecutionWindow?.Close();
            this.RaceExecutionWindow = null;
            this.ExecutionRunning = false;
            this.raceExecutionManager.HaltTimeTracking();

            this.RaceNotStarted = await informationManager.WasRaceNeverStartedAsync(this.SelectedRace.Race.Id)
                .ConfigureAwait(true);

            if (this.RankListVisible)
                await this.SelectedRace.LoadRankList().ConfigureAwait(true);
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
        #region Event-Handler Methods

        private void OnDispatcherTimer(object sender, EventArgs e)
        {
            base.NotifyPropertyChanged(nameof(this.Stopwatch));
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
            if (!this.ExecutionRunning)
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

                if (this.RankListVisible)
                    await this.SelectedRace.LoadRankList().ConfigureAwait(true);

                this.SelectedRace.Race = race;
                base.NotifyPropertyChanged(nameof(RaceNotCompleted));
                this.ExecutionRunning = false;
            }
        }

        #endregion
        #region TimeTracking-Methods

        public void StartTimeTracking()
        {
            this.Stopwatch.Start();
            this.DispatcherTimer.Start();
        }

        public void StopTimeTracking()
        {
            this.DispatcherTimer.Stop();
            this.Stopwatch.Reset();
            base.NotifyPropertyChanged(nameof(this.Stopwatch));
        }

        #endregion
        #endregion
    }
}
