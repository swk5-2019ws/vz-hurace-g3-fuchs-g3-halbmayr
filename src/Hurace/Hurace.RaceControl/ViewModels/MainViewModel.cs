using Hurace.Core.BL;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Hurace.RaceControl.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IRaceInformationManager raceManager;
        private RaceListItemViewModel selectedRace;
        private readonly IServiceProvider serviceProvider;

        public MainViewModel(IServiceProvider serviceProvider, IRaceInformationManager raceManager)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.raceManager = raceManager ?? throw new ArgumentNullException(nameof(raceManager));

            this.RaceListItemViewModels = new ObservableCollection<RaceListItemViewModel>();
        }

        public ObservableCollection<RaceListItemViewModel> RaceListItemViewModels { get; private set; }
        public RaceListItemViewModel SelectedRace
        {
            get => selectedRace;
            set => base.Set(ref this.selectedRace, value);
        }
        public RaceDetailViewModel DetailedSelectedRace { get; set; }


        internal async Task InitializeAsync()
        {
            var raceListItemViewModels = (await this.raceManager.GetAllRacesAsync())
                    .Select(race => new RaceListItemViewModel(race));

            foreach (var raceListItemViewModel in raceListItemViewModels)
                this.RaceListItemViewModels.Add(raceListItemViewModel);
        }
    }
}
