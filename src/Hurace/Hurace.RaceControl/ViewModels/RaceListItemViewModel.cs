using System;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceListItemViewModel : BaseViewModel
    {
        public RaceListItemViewModel(Domain.Race race)
        {
            Race = race ?? throw new ArgumentNullException(nameof(race));
        }

        public Domain.Race Race { get; }
    }
}
