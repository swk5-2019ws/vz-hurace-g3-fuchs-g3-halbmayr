using Hurace.Core.BL;
using Hurace.RaceControl.ViewModels.Shared;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

#pragma warning disable CA2227 // Collection properties should be read only
namespace Hurace.RaceControl.ViewModels
{
    public class RaceDetailViewModel : BaseViewModel
    {
        private Domain.Race race;
        private ObservableCollection<Domain.StartPosition> startList;

        private readonly IInformationManager informationManager;

        public RaceDetailViewModel(IInformationManager informationManager)
        {
            this.informationManager = informationManager ?? throw new ArgumentNullException(nameof(informationManager));

            startList = new ObservableCollection<Domain.StartPosition>();
            this.Ranks = new ObservableCollection<Domain.RankedSkier>();
        }

        public ObservableCollection<Domain.StartPosition> StartList
        {
            get => startList;
            set => base.Set(ref this.startList, value);
        }

        public Domain.Race Race
        {
            get => race;
            set => base.Set(ref this.race, value);
        }

        public ObservableCollection<Domain.RankedSkier> Ranks { get; }

        public async Task LoadRankList()
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
