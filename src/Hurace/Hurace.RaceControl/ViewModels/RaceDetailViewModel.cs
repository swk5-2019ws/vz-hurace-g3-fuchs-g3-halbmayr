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
        private Race race;
        private ObservableCollection<StartPosition> startList;

        public RaceDetailViewModel()
        {
            startList = new ObservableCollection<StartPosition>();
        }

        public ObservableCollection<StartPosition> StartList
        {
            get => startList;
            set => base.Set(ref this.startList, value);
        }

        public Race Race
        {
            get => race;
            set => base.Set(ref this.race, value);
        }
    }
}
