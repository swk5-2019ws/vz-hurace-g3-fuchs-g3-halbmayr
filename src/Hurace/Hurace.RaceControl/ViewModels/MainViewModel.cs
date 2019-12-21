using Hurace.Core.BL;
using Hurace.RaceControl.ViewModels.Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Hurace.RaceControl.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IRaceInformationManager raceManager;
        private readonly IServiceProvider serviceProvider;
        private RaceDetailViewModel selectedRace;
        private CreateRaceViewModel newRace;
        private bool createRaceVisible = false;

        public MainViewModel(IServiceProvider serviceProvider, IRaceInformationManager raceManager)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.raceManager = raceManager ?? throw new ArgumentNullException(nameof(raceManager));

            this.RaceListItemViewModels = new ObservableCollection<RaceDetailViewModel>();
            this.newRace = new CreateRaceViewModel(raceManager);
            this.SwitchControlVisibilityCommand = new AsyncDelegateCommand(this.SetVisibility, _ => true);
        }

        private Task SetVisibility(object arg)
        {
            this.CreateRaceVisible = true;
            this.SelectedRace = null;
            return Task.CompletedTask;
        }
        public AsyncDelegateCommand SwitchControlVisibilityCommand { get; }

        public ObservableCollection<RaceDetailViewModel> RaceListItemViewModels { get; private set; }

        public bool CreateRaceVisible {
            get => createRaceVisible;
            set => base.Set(ref this.createRaceVisible, value);
        }

        public RaceDetailViewModel SelectedRace {
            get => selectedRace;
            set => base.Set(ref this.selectedRace, value);
        }

        public CreateRaceViewModel NewRace {
            get => newRace;
            set => base.Set(ref this.newRace, value);
        }

        internal async Task InitializeAsync()
        {
            var raceListDetailViewModels =
                (await this.raceManager.GetAllRacesAsync(
                    raceTypeLoadingType: Domain.Associated<Domain.RaceType>.LoadingType.Reference,
                    venueLoadingType: Domain.Associated<Domain.Venue>.LoadingType.Reference,
                    seasonLoadingType: Domain.Associated<Domain.Season>.LoadingType.Reference,
                    startListLoadingType: Domain.Associated<Domain.StartPosition>.LoadingType.Reference))
                .Select(race => (this.serviceProvider.GetRequiredService<RaceDetailViewModel>(), race));

            foreach (var (raceDetailViewModel, race) in raceListDetailViewModels)
            {
                raceDetailViewModel.Race = race;
                this.RaceListItemViewModels.Add(raceDetailViewModel);
            }
            await newRace.Initialize();
        }
    }
}
