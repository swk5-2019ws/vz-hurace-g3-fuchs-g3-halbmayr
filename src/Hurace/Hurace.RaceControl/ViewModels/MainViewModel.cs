using Hurace.Core.BL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hurace.RaceControl.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IRaceManager raceManager;

        public MainViewModel(IRaceManager raceManager)
        {
            this.raceManager = raceManager ?? throw new ArgumentNullException(nameof(raceManager));

            this.RaceListItemViewModels = new ObservableCollection<RaceListItemViewModel>();
        }

        public ObservableCollection<RaceListItemViewModel> RaceListItemViewModels { get; private set; }

        internal async Task InitializeAsync()
        {
            var raceListItemViewModels = await Task.WhenAll(
                (await this.raceManager.GetAllRacesAsync())
                    .Select(
                        async race => new RaceListItemViewModel(
                                race,
                                await this.raceManager.GetSeasonByDate(race.Date))));

            foreach (var raceListItemViewModel in raceListItemViewModels)
                this.RaceListItemViewModels.Add(raceListItemViewModel);
        }
    }
}
