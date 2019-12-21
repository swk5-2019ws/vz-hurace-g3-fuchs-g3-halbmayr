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
        private ObservableCollection<Domain.Skier> allSkiers;
        private ObservableCollection<Domain.Season> seasons;
        private Domain.Season selectedSeason;
        private ObservableCollection<Domain.StartPosition> startPositions;
        private Domain.StartPosition selectedStartPosition;
        private ObservableCollection<Domain.Skier> skiers;
        private Domain.Skier selectedSkier;

        public AsyncDelegateCommand AddRacerToStartListCommand { get; }
        public AsyncDelegateCommand RemoveRacerFromStartListCommand { get; }
        public AsyncDelegateCommand MoveSelectedStartPositionUpCommand { get; }
        public AsyncDelegateCommand MoveSelectedStartPositionDownCommand { get; }

        public bool HasErrors => this.errors.Any();

        public CreateRaceViewModel(IRaceInformationManager raceManager)
        {
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
        }

        public async Task Initialize()
        {
            Venues = new ObservableCollection<Domain.Venue>(
                await raceManager.GetAllVenuesAsync(
                    Domain.Associated<Domain.Country>.LoadingType.Reference));

            RaceTypes = new ObservableCollection<Domain.RaceType>(
                await raceManager.GetAllRaceTypesAsync());

            Seasons = new ObservableCollection<Domain.Season>(
                await raceManager.GetAllSeasonsAsync());

            StartPositions = new ObservableCollection<Domain.StartPosition>();

            Skiers = new ObservableCollection<Domain.Skier>(
                (await raceManager.GetAllSkiersAsync()).OrderBy(skier => skier.LastName));
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
                if(SelectedStartPosition.Position < elem.Position)
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

        public ObservableCollection<Domain.Season> Seasons {
            get => seasons;
            set => base.Set(ref this.seasons, value);
        }

        public Domain.Season SelectedSeason {
            get => selectedSeason;
            set => base.Set(ref this.selectedSeason, value);
        }

        public ObservableCollection<Domain.StartPosition> StartPositions {
            get => startPositions;
            set => base.Set(ref this.startPositions, value);
        }

        public Domain.StartPosition SelectedStartPosition {
            get => selectedStartPosition;
            set => base.Set(ref this.selectedStartPosition, value);
        }

        public ObservableCollection<Domain.Skier> Skiers {
            get => skiers;
            set => base.Set(ref this.skiers, value);
        }

        public Domain.Skier SelectedSkier {
            get => selectedSkier;
            set => base.Set(ref this.selectedSkier, value);
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
