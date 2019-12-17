using Hurace.Core.BL;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Hurace.RaceControl.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IRaceInformationManager raceManager;
        private RaceDetailViewModel selectedRace;
        private readonly IServiceProvider serviceProvider;

        public MainViewModel(IServiceProvider serviceProvider, IRaceInformationManager raceManager)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.raceManager = raceManager ?? throw new ArgumentNullException(nameof(raceManager));

            this.RaceListItemViewModels = new ObservableCollection<RaceDetailViewModel>();
        }

        public ObservableCollection<RaceDetailViewModel> RaceListItemViewModels { get; private set; }
        public RaceDetailViewModel SelectedRace
        {
            get => selectedRace;
            set => base.Set(ref this.selectedRace, value);
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
        }
    }
}
