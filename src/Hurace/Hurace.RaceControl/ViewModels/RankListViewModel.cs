using Hurace.Core.BL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Hurace.RaceControl.ViewModels
{
    public class RankListViewModel : BaseViewModel
    {
        private readonly IInformationManager informationManager;
        private readonly Domain.Race race;

        public RankListViewModel(
            IInformationManager informationManager,
            Domain.Race race)
        {
            this.informationManager = informationManager ?? throw new ArgumentNullException(nameof(informationManager));
            this.race = race ?? throw new ArgumentNullException(nameof(race));
            this.Ranks = new ObservableCollection<RankViewModel>();
        }

        public ObservableCollection<RankViewModel> Ranks { get; }

        public async Task InitializeAsync()
        {
            (var firstStartListData, var secondStartListData) =
                await informationManager.GetRankListOfRace(race.Id)
                    .ConfigureAwait(false);

            //initialize rank list
        }
    }
}
