using Hurace.Core.BL;
using Hurace.Domain;
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
        private readonly IInformationManager raceInformationManager;
        private ObservableCollection<StartPosition> startList;

        public RaceDetailViewModel(IInformationManager raceInformationManager)
        {
            this.raceInformationManager = raceInformationManager ?? throw new ArgumentNullException(nameof(raceInformationManager));

            startList = new ObservableCollection<StartPosition>();
        }

        public ObservableCollection<StartPosition> StartList
        {
            get => startList;
            set => base.Set(ref this.startList, value);
        }

        public Race Race { get; set; }

        public async Task LoadRaceData()
        {
            var tempRace = await raceInformationManager.GetRaceByIdAsync(
                    Race.Id,
                    Associated<RaceState>.LoadingType.Reference,
                    Associated<RaceType>.LoadingType.Reference,
                    Associated<Venue>.LoadingType.Reference,
                    Associated<Season>.LoadingType.Reference,
                    Associated<StartPosition>.LoadingType.Reference,
                    Associated<Skier>.LoadingType.Reference,
                    Associated<Sex>.LoadingType.Reference,
                    Associated<Country>.LoadingType.Reference)
                .ConfigureAwait(false);

            foreach (var sp in tempRace.FirstStartList)
            {
                Application.Current.Dispatcher.Invoke(
                    () => StartList.Add(sp.Reference));
            }

            var sl = startList;
            StartList = sl;
        }
    }
}
