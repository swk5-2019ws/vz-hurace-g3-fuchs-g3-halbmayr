using Hurace.Core.BL;
using Hurace.RaceControl.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CA2227 // Collection properties should be read only
namespace Hurace.RaceControl.ViewModels
{
    public class CreateRaceViewModel : BaseViewModel
    {
        private readonly IRaceInformationManager raceManager;
        private readonly IDictionary<string, string> errors = new Dictionary<string, string>();
        private Domain.Race race;
        private ObservableCollection<Domain.RaceType> raceTypes;
        private Domain.RaceType selectedRaceType;
        private ObservableCollection<Domain.Venue> venues;
        private Domain.Venue selectedVenue;
        private int selectedNumberOfSensors;
        private DateTime selectedDate;
        private ObservableCollection<Domain.StartPosition> selectedStartPositions;
        private ObservableCollection<Domain.Skier> allSkiers;
        private Domain.Skier selectedSkier;

        public bool HasErrors => this.errors.Any();

        public CreateRaceViewModel(IRaceInformationManager raceManager)
        {
            this.raceManager = raceManager ?? throw new ArgumentNullException(nameof(raceManager));
            SelectedDate = DateTime.Now;
        }

        public async Task Initialize()
        {
            Venues = new ObservableCollection<Domain.Venue>(await raceManager.GetAllVenuesAsync(
                Domain.Associated<Domain.Country>.LoadingType.Reference));
            RaceTypes = new ObservableCollection<Domain.RaceType>(await raceManager.GetAllRaceTypesAsync());
            //AllSkiers = new ObservableCollection<Domain.Skier>(await raceManager)

        }
        public ObservableCollection<Domain.RaceType> RaceTypes
        {
            get => raceTypes;
            set => base.Set(ref this.raceTypes, value);
        }

        public Domain.RaceType SelectedRaceType
        {
            get => selectedRaceType;
            set => base.Set(ref this.selectedRaceType, value);
        }

        public ObservableCollection<Domain.Venue> Venues
        {
            get => venues;
            set => base.Set(ref this.venues, value);
        }

        public Domain.Venue SelectedVenue
        {
            get => selectedVenue;
            set => base.Set(ref this.selectedVenue, value);
        }

        public int SelectedNumberOfSensors
        {
            get => selectedNumberOfSensors;
            set => base.Set(ref this.selectedNumberOfSensors, value);
        }

        public DateTime SelectedDate
        {
            get => selectedDate;
            set => base.Set(ref this.selectedDate, value);
        }

        public ObservableCollection<Domain.Skier> AllSkiers
        {
            get => allSkiers;
            set => base.Set(ref this.allSkiers, value);
        }

        public Domain.Race Race
        {
            get => race;
            set
            {
                //TODO validation
                this.Set(ref this.race, value);
            }
        }
    }
}
