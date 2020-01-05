using Hurace.Core.BL;
using Hurace.RaceControl.ViewModels.Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Hurace.RaceControl.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IInformationManager raceManager;
        private readonly IServiceProvider serviceProvider;
        private RaceDetailViewModel selectedRace;
        private CreateRaceViewModel createRaceViewModel;
        private bool createRaceVisible = false;
        private bool raceDetailViewVisible;
        private bool createRaceButtonVisible;

        public MainViewModel(IServiceProvider serviceProvider, IInformationManager raceManager)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.raceManager = raceManager ?? throw new ArgumentNullException(nameof(raceManager));

            this.RaceListItemViewModels = new ObservableCollection<RaceDetailViewModel>();
            this.createRaceViewModel = new CreateRaceViewModel(raceManager);

            this.OpenCreateRaceCommand = new AsyncDelegateCommand(
                async _ =>
                {
                    this.CreateRaceButtonVisible = true;
                    this.CreateRaceControlVisible = true;
                    this.RaceDetailControlVisible = false;
                    await this.CreateRaceViewModel.Initialize();
                    return;
                });

            this.CreateOrUpdateRaceCommand = new AsyncDelegateCommand(
                async _ =>
                {
                    var rlvm = this.serviceProvider.GetRequiredService<RaceDetailViewModel>();
                    var tempRace = await raceManager.GetRaceByIdAsync(await createRaceViewModel.CreateOrUpdateRace(new object()),
                        raceTypeLoadingType: Domain.Associated<Domain.RaceType>.LoadingType.Reference,
                        venueLoadingType: Domain.Associated<Domain.Venue>.LoadingType.Reference,
                        seasonLoadingType: Domain.Associated<Domain.Season>.LoadingType.Reference);
                    rlvm.Race = tempRace;
                    this.RaceListItemViewModels.Add(rlvm);
                    this.CreateRaceButtonVisible = false;
                    this.CreateRaceControlVisible = false;
                    this.RaceDetailControlVisible = true;
                }
                , null);

            this.OpenEditRaceCommand = new AsyncDelegateCommand(
                async _ =>
                {
                    this.CreateRaceButtonVisible = false;
                    this.CreateRaceControlVisible = true;
                    this.RaceDetailControlVisible = false;
                    await this.CreateRaceViewModel.InitializeExistingRace(SelectedRace.Race);
                    return;
                });

        }

        public AsyncDelegateCommand CreateOrUpdateRaceCommand { get; }
        public AsyncDelegateCommand OpenEditRaceCommand { get; }
        public AsyncDelegateCommand OpenCreateRaceCommand { get; set; }

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
                (await this.raceManager.GetAllRacesAsync(
                    raceTypeLoadingType: Domain.Associated<Domain.RaceType>.LoadingType.Reference,
                    venueLoadingType: Domain.Associated<Domain.Venue>.LoadingType.Reference,
                    seasonLoadingType: Domain.Associated<Domain.Season>.LoadingType.Reference))
                .Select(race => (this.serviceProvider.GetRequiredService<RaceDetailViewModel>(), race));

            foreach (var (raceDetailViewModel, race) in raceListDetailViewModels)
            {
                raceDetailViewModel.Race = race;
                this.RaceListItemViewModels.Add(raceDetailViewModel);
            }

            await createRaceViewModel.Initialize();
        }
    }
}
