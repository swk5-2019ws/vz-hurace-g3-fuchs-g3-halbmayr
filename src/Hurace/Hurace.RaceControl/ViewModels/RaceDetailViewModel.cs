using Hurace.Core.BL;
using Hurace.RaceControl.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

#pragma warning disable CA2227 // Collection properties should be read only
namespace Hurace.RaceControl.ViewModels
{
    public class RaceDetailViewModel : BaseViewModel
    {
        private Domain.StartPosition currentStartPosition;
        private IEnumerable<Domain.StartPosition> startList;

        internal bool currentlyRunning;

        private Domain.Race race;
        private Domain.Skier afterNextStartingSkier;
        private Domain.Skier nextStartingSkier;
        private Domain.Skier currentStartingSkier;
        private bool firstRun;
        private readonly IInformationManager informationManager;
        private readonly IRaceExecutionManager raceExecutionManager;
        private readonly MainViewModel mainVM;

        public RaceDetailViewModel(
            IInformationManager informationManager,
            IRaceExecutionManager raceExecutionManager,
            MainViewModel mainVM)
        {
            this.informationManager = informationManager ?? throw new ArgumentNullException(nameof(informationManager));
            this.raceExecutionManager = raceExecutionManager ?? throw new ArgumentNullException(nameof(raceExecutionManager));
            this.mainVM = mainVM ?? throw new ArgumentNullException(nameof(mainVM));
            this.currentlyRunning = false;
            this.Ranks = new ObservableCollection<Domain.RankedSkier>();
            this.Measurements = new ObservableCollection<Domain.ProcessedTimeMeasurement>();

            this.RegisterFailureCommand = new AsyncDelegateCommand(
                this.RegisterFailure);
            this.DisqualifyCommand = new AsyncDelegateCommand(
                this.Disqualify,
                this.CanDisqualify);
            this.ReleaseStartCommand = new AsyncDelegateCommand(
                this.ReleaseStart,
                this.CanReleaseStart);
        }

        #region Properties
        #region Commands

        public AsyncDelegateCommand RegisterFailureCommand { get; set; }
        public AsyncDelegateCommand DisqualifyCommand { get; set; }
        public AsyncDelegateCommand ReleaseStartCommand { get; set; }

        #endregion

        public bool CurrentStartingSkierTracked { get; set; }

        public bool FirstRun
        {
            get => firstRun;
            set => base.Set(ref this.firstRun, value);
        }

        public Domain.Race Race
        {
            get => race;
            set => base.Set(ref this.race, value);
        }

        public Domain.Skier AfterNextStartingSkier
        {
            get => afterNextStartingSkier;
            set => base.Set(ref this.afterNextStartingSkier, value);
        }

        public Domain.Skier NextStartingSkier
        {
            get => nextStartingSkier;
            set => base.Set(ref this.nextStartingSkier, value);
        }

        public Domain.Skier CurrentStartingSkier
        {
            get => currentStartingSkier;
            set => base.Set(ref this.currentStartingSkier, value);
        }

        public ObservableCollection<Domain.ProcessedTimeMeasurement> Measurements { get; set; }

        public ObservableCollection<Domain.RankedSkier> Ranks { get; }

        #endregion
        #region Methods
        #region Measurement-Logic

        private async Task OnTimeMeasured(Domain.ProcessedTimeMeasurement measurement, bool lastMeasurement)
        {
            int insertIndex = 0;
            if (this.Measurements.Count > 0)
            {
                insertIndex = this.Measurements.ToList()
                    .FindLastIndex(m => string.Compare(m.SensorString, measurement.SensorString, StringComparison.OrdinalIgnoreCase) < 0)
                    + 1;
            }

            Application.Current.Dispatcher.Invoke(
                () =>
                {
                    this.Measurements.Insert(insertIndex, measurement);
                });

            if (lastMeasurement)
            {
                this.raceExecutionManager.OnTimeMeasured -= this.OnTimeMeasured;

                await Task.Run(
                        async () =>
                        {
                            Thread.Sleep(5000);
                            await this.UpdateStartingSkiers().ConfigureAwait(false);
                        })
                    .ConfigureAwait(false);

                Application.Current.Dispatcher.Invoke(
                    () =>
                    {
                        this.Measurements.Clear();
                        this.currentlyRunning = false;
                    });
            }
        }

        #endregion
        #region Command-Methods

        private async Task RegisterFailure(object parameter)
        {
            this.currentlyRunning = false;

            var failureType = (await this.informationManager.GetAllRaceStatesAsync().ConfigureAwait(false))
                .First(rt => rt.Label == "NichtAbgeschlossen");

            await this.raceExecutionManager.GenerateSecondStartListIfNeeded(this.currentStartPosition)
                .ConfigureAwait(false);

            if (this.currentlyRunning)
            {
                await this.raceExecutionManager.StopTimeTrackingAsync(failureType)
                    .ConfigureAwait(false);
            }
            else
            {
                var raceData = await informationManager.GetRaceDataByRaceAndStartlistAndPositionAsync(
                        race,
                        FirstRun,
                        currentStartPosition.Position)
                    .ConfigureAwait(false);

                raceData.RaceState = new Domain.Associated<Domain.RaceState>(failureType);

                await informationManager.UpdateRaceStateOfRaceDataAsync(raceData).ConfigureAwait(false);
            }

            this.raceExecutionManager.OnTimeMeasured -= OnTimeMeasured;
            await this.UpdateStartingSkiers().ConfigureAwait(false);
        }

        private bool CanDisqualify(object parameter)
        {
            return this.currentlyRunning;
        }

        private async Task Disqualify(object parameter)
        {
            this.currentlyRunning = false;

            var failureType = (await this.informationManager.GetAllRaceStatesAsync().ConfigureAwait(false))
                .First(rt => rt.Label == "Disqualifiziert");

            await this.raceExecutionManager.GenerateSecondStartListIfNeeded(this.currentStartPosition)
                .ConfigureAwait(false);

            await this.raceExecutionManager.StopTimeTrackingAsync(failureType)
                .ConfigureAwait(false);

            this.raceExecutionManager.OnTimeMeasured -= OnTimeMeasured;
            await this.UpdateStartingSkiers().ConfigureAwait(false);
        }

        private bool CanReleaseStart(object parameter)
        {
            return !this.currentlyRunning;
        }

        private async Task ReleaseStart(object parameter)
        {
            this.currentlyRunning = true;

            await this.raceExecutionManager.StartTimeTrackingAsync(
                    this.Race,
                    this.FirstRun,
                    this.currentStartPosition.Position)
                .ConfigureAwait(false);

            this.raceExecutionManager.OnTimeMeasured += OnTimeMeasured;
        }

        #endregion
        #region Async-Loaders

        public async Task LoadRankList()
        {
            var ranks = await this.informationManager.GetRankedSkiersOfRaceAsync(race.Id)
                .ConfigureAwait(false);

            Application.Current.Dispatcher.Invoke(
                () =>
                {
                    this.Ranks.Clear();
                    foreach (var rank in ranks)
                    {
                        this.Ranks.Add(rank);
                    }
                });
        }

        internal async Task UpdateStartingSkiers()
        {
            Application.Current.Dispatcher.Invoke(() => this.Measurements.Clear());

            this.FirstRun = true;
            this.startList = (await this.informationManager.GetStartPositionListAsync(this.Race.Id, true)
                    .ConfigureAwait(false))
                .OrderBy(sp => sp.Position);

            this.currentStartPosition = await this.GetCurrentStartPosition(this.startList, true)
                .ConfigureAwait(false);
            if (this.currentStartPosition == null)
            {
                this.FirstRun = false;
                this.startList = (await this.informationManager.GetStartPositionListAsync(this.Race.Id, false)
                        .ConfigureAwait(false))
                    .OrderBy(sp => sp.Position);

                this.currentStartPosition = await this.GetCurrentStartPosition(this.startList, false)
                    .ConfigureAwait(false);

                if (this.currentStartPosition == null)
                {
                    this.mainVM.ExecutionRunning = false;
                    await this.mainVM.InitializeSelectedRace().ConfigureAwait(false);
                    return;
                }
            }

            this.CurrentStartingSkier = this.currentStartPosition.Skier.Reference;

            var startListList = this.startList.ToList();
            var nextStartPositionIndex = startListList.FindIndex(sp => sp == this.currentStartPosition) + 1;
            if (nextStartPositionIndex < startListList.Count)
            {
                this.NextStartingSkier = startListList.ElementAt(nextStartPositionIndex)
                    .Skier.Reference;

                var afterNextStartPositionIndex = nextStartPositionIndex + 1;
                this.AfterNextStartingSkier = afterNextStartPositionIndex < startListList.Count
                    ? startListList.ElementAt(afterNextStartPositionIndex)
                        .Skier.Reference
                    : null;
            }
            else
                this.NextStartingSkier = null;
        }

        private async Task<Domain.StartPosition> GetCurrentStartPosition(
            IEnumerable<Domain.StartPosition> startList,
            bool firstStartList)
        {
            foreach (var startPosition in startList)
            {
                var isNextStartPosition = await this.informationManager
                    .IsNextStartPositionAsync(this.Race, firstStartList, startPosition.Position)
                    .ConfigureAwait(false);

                if (isNextStartPosition)
                    return startPosition;
            }
            return null;
        }

        #endregion
        #endregion
    }
}
