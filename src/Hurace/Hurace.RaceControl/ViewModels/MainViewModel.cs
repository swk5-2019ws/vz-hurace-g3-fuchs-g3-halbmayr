using Hurace.Core.BL;
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

        public MainViewModel(IRaceInformationManager raceManager)
        {
            this.raceManager = raceManager ?? throw new ArgumentNullException(nameof(raceManager));

            this.RaceListItemViewModels = new ObservableCollection<RaceListItemViewModel>();
        }

        public ObservableCollection<RaceListItemViewModel> RaceListItemViewModels { get; private set; }
        public RaceListItemViewModel SelectedRace
        {
            get => selectedRace;
            set => base.Set(ref this.selectedRace, value);
        }

        internal async Task InitializeAsync()
        {
            var raceListItemViewModels = await Task.WhenAll(
                (await this.raceManager.GetAllRacesAsync(true))
                    .Select(
                        async race => new RaceListItemViewModel(
                                race,
                                await this.raceManager.GetSeasonByDate(race.Date))));

            foreach (var raceListItemViewModel in raceListItemViewModels)
                this.RaceListItemViewModels.Add(raceListItemViewModel);
        }
    }
}
