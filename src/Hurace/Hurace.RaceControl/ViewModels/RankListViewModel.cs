using Hurace.Core.BL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            this.Ranks = new ObservableCollection<Domain.RankedSkier>();
        }

        public ObservableCollection<Domain.RankedSkier> Ranks { get; }

        public async Task InitializeAsync()
        {
            var ranks = await this.informationManager.GetRankedSkiersOfRace(race.Id)
                .ConfigureAwait(false);

            Application.Current.Dispatcher.Invoke(
                () =>
                {
                    foreach (var rank in ranks)
                    {
                        this.Ranks.Add(rank);
                    }
                });
        }
    }
}
