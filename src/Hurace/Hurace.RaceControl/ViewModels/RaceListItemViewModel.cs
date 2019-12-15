using System;

namespace Hurace.RaceControl.ViewModels
{
    public class RaceListItemViewModel : BaseViewModel
    {
        public RaceListItemViewModel(Domain.Race race, Domain.Season season)
        {
            Race = race ?? throw new ArgumentNullException(nameof(race));
            Season = season ?? throw new ArgumentNullException(nameof(season));
        }

        public Domain.Race Race { get; }
        public Domain.Season Season { get; }
    }
}
