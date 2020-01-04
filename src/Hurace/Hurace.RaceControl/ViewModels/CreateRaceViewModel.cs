using Hurace.Core.BL;
using Hurace.RaceControl.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

#pragma warning disable CA1801 // Review unused parameters
#pragma warning disable CA1720 // Identifier contains type name
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable CA2227 // Collection properties should be read only
namespace Hurace.RaceControl.ViewModels
{
    public class CreateRaceViewModel : BaseViewModel
    {
        private readonly IInformationManager raceManager;
        private readonly IDictionary<string, string> errors = new Dictionary<string, string>();
        private Domain.Race race;
        private string description;
        private ObservableCollection<Domain.RaceType> raceTypes;
        private Domain.RaceType selectedRaceType;
        private ObservableCollection<Domain.Venue> venues;
        private Domain.Venue selectedVenue;
        private int selectedNumberOfSensors;
        private DateTime selectedDate;
        private ObservableCollection<Domain.Skier> allSkiers;
        private ObservableCollection<Domain.Season> seasons;
        private Domain.Season selectedSeason;
        private ObservableCollection<Domain.StartPosition> startPositions;
        private Domain.StartPosition selectedStartPosition;
        private ObservableCollection<Domain.Skier> skiers;
        private Domain.Skier selectedSkier;
        private bool loading = true;
        private bool menListSelected = true;
        private int genderSpecififcRaceId;
        private string createRaceHeader;

        private ObservableCollection<Domain.Skier> maleSkiers;
        private ObservableCollection<Domain.Skier> femaleSkiers;

        private ObservableCollection<Domain.StartPosition> maleStartPositions;
        private ObservableCollection<Domain.StartPosition> femaleStartPositions;

        public AsyncDelegateCommand AddRacerToStartListCommand { get; }
        public AsyncDelegateCommand RemoveRacerFromStartListCommand { get; }
        public AsyncDelegateCommand MoveSelectedStartPositionUpCommand { get; }
        public AsyncDelegateCommand MoveSelectedStartPositionDownCommand { get; }
        public AsyncDelegateCommand SelectMenListCommand { get; }
        public AsyncDelegateCommand SelectWomenListCommand { get; }

        public bool HasErrors => this.errors.Any();

        public CreateRaceViewModel(IInformationManager raceManager)
        {
            Loading = true;
            MenListSelected = true;

            this.raceManager = raceManager ?? throw new ArgumentNullException(nameof(raceManager));
            SelectedDate = DateTime.Now;

            this.AddRacerToStartListCommand = new AsyncDelegateCommand(
                AddRacerToStartList,
                (object obj) => selectedSkier != null);
            this.RemoveRacerFromStartListCommand = new AsyncDelegateCommand(
                RemoveRacerFromStartList,
                (object obj) => selectedStartPosition != null);
            this.MoveSelectedStartPositionUpCommand = new AsyncDelegateCommand(
                MoveRacerUpInStartList,
                (object obj) => selectedStartPosition != null && selectedStartPosition.Position > 1);
            this.MoveSelectedStartPositionDownCommand = new AsyncDelegateCommand(
                MoveRacerDownInStartList,
                (object obj) => selectedStartPosition != null && selectedStartPosition.Position < StartPositions.Count);
            this.SelectMenListCommand = new AsyncDelegateCommand(
                SelectMenList,
                (object obj) => !menListSelected);
            this.SelectWomenListCommand = new AsyncDelegateCommand(
                SelectWomenList,
                (object obj) => menListSelected);
        }

        public async Task InitializeExistingRace(Domain.Race race)
        {
            CreateRaceHeader = "Rennen bearbeiten";
            this.Race = race ?? throw new ArgumentNullException(nameof(race));

            Venues = new ObservableCollection<Domain.Venue>(
                await raceManager.GetAllVenuesAsync(
                    Domain.Associated<Domain.Country>.LoadingType.Reference));

            RaceTypes = new ObservableCollection<Domain.RaceType>(
                await raceManager.GetAllRaceTypesAsync());

            Seasons = new ObservableCollection<Domain.Season>(
                await raceManager.GetAllSeasonsAsync());

            await InitSkierLists();

            Loading = false;

            Domain.Race tempRace = await raceManager.GetRaceByIdAsync(Race.Id,
                Domain.Associated<Domain.RaceType>.LoadingType.Reference,
                Domain.Associated<Domain.Venue>.LoadingType.Reference,
                Domain.Associated<Domain.Season>.LoadingType.Reference,
                Domain.Associated<Domain.StartPosition>.LoadingType.Reference,
                Domain.Associated<Domain.Skier>.LoadingType.Reference,
                Domain.Associated<Domain.Sex>.LoadingType.Reference,
                Domain.Associated<Domain.Country>.LoadingType.Reference);

            SelectedVenue = Venues.FirstOrDefault(venue => venue.Id == tempRace.Venue.Reference.Id);
            SelectedSeason = Seasons.FirstOrDefault(season => season.Id == tempRace.Season.Reference.Id);
            SelectedRaceType = RaceTypes.FirstOrDefault(raceType => raceType.Id == tempRace.RaceType.Reference.Id);
            SelectedNumberOfSensors = tempRace.NumberOfSensors;
            SelectedDate = tempRace.Date;
            Description = tempRace.Description;

            if (race.GenderSpecificRaceId == 0)
                await SelectWomenList(new object());
            else
                await SelectMenList(new object());

            foreach (var racer in tempRace.FirstStartList)
            {
                StartPositions.Add(racer.Reference);
                skiers.Remove(skiers.FirstOrDefault(skier => skier.Id == racer.Reference.Skier.Reference.Id));
            }
        }

        public async Task Initialize()
        {
            Race = new Domain.Race
            {
                Id = -1
            };

            CreateRaceHeader = "Neues Rennen Anlegen";

            Venues = new ObservableCollection<Domain.Venue>(
                await raceManager.GetAllVenuesAsync(
                    Domain.Associated<Domain.Country>.LoadingType.Reference));

            RaceTypes = new ObservableCollection<Domain.RaceType>(
                await raceManager.GetAllRaceTypesAsync());

            Seasons = new ObservableCollection<Domain.Season>(
                await raceManager.GetAllSeasonsAsync());

            Description = "";

            await InitSkierLists();

            Loading = false;
        }

        public async Task InitSkierLists()
        {
            StartPositions = new ObservableCollection<Domain.StartPosition>();
            maleStartPositions = new ObservableCollection<Domain.StartPosition>();
            femaleStartPositions = new ObservableCollection<Domain.StartPosition>();

            Skiers = new ObservableCollection<Domain.Skier>(
                (await raceManager.GetAllSkiersAsync()).OrderBy(skier => skier.LastName));

            maleSkiers = new ObservableCollection<Domain.Skier>(Skiers.Where(skier => skier.Sex.Reference.Label == "Männlich"));
            femaleSkiers = new ObservableCollection<Domain.Skier>(Skiers.Where(skier => skier.Sex.Reference.Label == "Weiblich"));

            Skiers = maleSkiers;
            MenListSelected = true;
            genderSpecififcRaceId = 0;
        }

        public async Task CreateRace(object obj)
        {
            var tempStartList = new List<Domain.Associated<Domain.StartPosition>>();
            var tempSkiers = new List<Domain.Associated<Domain.Skier>>();

            foreach (var pos in StartPositions)
            {
                tempStartList.Add(new Domain.Associated<Domain.StartPosition>(pos));
            }

            foreach (var pos in StartPositions)
            {
                tempSkiers.Add(pos.Skier);
            }

            Race.Date = SelectedDate;
            Race.Description = Description;
            Race.NumberOfSensors = SelectedNumberOfSensors;
            Race.RaceType = new Domain.Associated<Domain.RaceType>(SelectedRaceType);
            Race.Venue = new Domain.Associated<Domain.Venue>(SelectedVenue);
            Race.Season = new Domain.Associated<Domain.Season>(SelectedSeason);
            Race.FirstStartList = tempStartList;
            Race.Skiers = tempSkiers;
            Race.GenderSpecificRaceId = genderSpecififcRaceId;

            await raceManager.CreateOrUpdateRace(Race);

            SelectedDate = DateTime.Now;
            Description = "";
            SelectedNumberOfSensors = 5;
            SelectedRaceType = null;
            SelectedVenue = null;
            SelectedSeason = null;
            await InitSkierLists();

            return;
        }

        private Task SelectMenList(object obj)
        {
            femaleSkiers = Skiers;
            femaleStartPositions = StartPositions;

            Skiers = maleSkiers;
            StartPositions = maleStartPositions;

            MenListSelected = true;
            genderSpecififcRaceId = 0;

            return Task.CompletedTask;
        }

        private Task SelectWomenList(object obj)
        {
            maleSkiers = Skiers;
            maleStartPositions = StartPositions;

            Skiers = femaleSkiers;
            StartPositions = femaleStartPositions;

            MenListSelected = false;
            genderSpecififcRaceId = 1;

            return Task.CompletedTask;
        }

        private Task MoveRacerUpInStartList(object obj)
        {
            Domain.StartPosition pos = StartPositions
                .Where(startPosition => startPosition.Position == SelectedStartPosition.Position - 1)
                .Single();

            pos.Position++;
            SelectedStartPosition.Position--;

            StartPositions = new ObservableCollection<Domain.StartPosition>(
                StartPositions.OrderBy(startPosition => startPosition.Position));

            return Task.CompletedTask;
        }

        private Task MoveRacerDownInStartList(object obj)
        {
            Domain.StartPosition pos = StartPositions
                .Where(startPosition => startPosition.Position == SelectedStartPosition.Position + 1)
                .Single();

            pos.Position--;
            SelectedStartPosition.Position++;

            StartPositions = new ObservableCollection<Domain.StartPosition>(
                StartPositions.OrderBy(startPosition => startPosition.Position));

            return Task.CompletedTask;
        }

        private Task AddRacerToStartList(object obj)
        {
            StartPositions.Add(new Domain.StartPosition
            {
                Position = StartPositions.Count + 1,
                Skier = new Domain.Associated<Domain.Skier>
                {
                    Reference = SelectedSkier
                }
            });
            skiers.Remove(SelectedSkier);

            return Task.CompletedTask;
        }

        private Task RemoveRacerFromStartList(object obj)
        {
            Skiers.Add(SelectedStartPosition.Skier.Reference);

            foreach (var elem in StartPositions)
            {
                if (SelectedStartPosition.Position < elem.Position)
                {
                    elem.Position--;
                }
            }

            StartPositions.Remove(SelectedStartPosition);

            //TODO make non hacky version of this
            ObservableCollection<Domain.StartPosition> tempList = StartPositions;
            StartPositions = null;
            StartPositions = tempList;

            return Task.CompletedTask;
        }

        #region Properties

        public string CreateRaceHeader 
        {
            get => createRaceHeader;
            set => base.Set(ref this.createRaceHeader, value);
        }
        public bool MenListSelected
        {
            get => menListSelected;
            set => base.Set(ref this.menListSelected, value);
        }
        public bool Loading
        {
            get => loading;
            set => base.Set(ref this.loading, value);
        }

        public string Description
        {
            get => description;
            set => base.Set(ref this.description, value);
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

        public ObservableCollection<Domain.Season> Seasons
        {
            get => seasons;
            set => base.Set(ref this.seasons, value);
        }

        public Domain.Season SelectedSeason
        {
            get => selectedSeason;
            set => base.Set(ref this.selectedSeason, value);
        }

        public ObservableCollection<Domain.StartPosition> StartPositions
        {
            get => startPositions;
            set => base.Set(ref this.startPositions, value);
        }

        public Domain.StartPosition SelectedStartPosition
        {
            get => selectedStartPosition;
            set => base.Set(ref this.selectedStartPosition, value);
        }

        public ObservableCollection<Domain.Skier> Skiers
        {
            get => skiers;
            set => base.Set(ref this.skiers, value);
        }

        public Domain.Skier SelectedSkier
        {
            get => selectedSkier;
            set => base.Set(ref this.selectedSkier, value);
        }

        public Domain.Race Race {
            get => race;
            set =>
                //TODO validation
                this.Set(ref this.race, value);
        }

        #endregion
    }
}
