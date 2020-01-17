using Hurace.Core.DAL;
using Hurace.Core.Db.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#pragma warning disable CA1502 // Avoid excessive complexity
#pragma warning disable IDE0039 // Use local function
#pragma warning disable IDE0046 // Convert to conditional expression
#pragma warning disable IDE0010 // Add missing cases
namespace Hurace.Core.BL
{
    public sealed class InformationManager : IInformationManager
    {
        #region Fields

        private readonly IDataAccessObject<Entities.Country> countryDao;
        private readonly IDataAccessObject<Entities.Race> raceDao;
        private readonly IDataAccessObject<Entities.RaceData> raceDataDao;
        private readonly IDataAccessObject<Entities.RaceState> raceStateDao;
        private readonly IDataAccessObject<Entities.RaceType> raceTypeDao;
        private readonly IDataAccessObject<Entities.Venue> venueDao;
        private readonly IDataAccessObject<Entities.SeasonPlan> seasonPlanDao;
        private readonly IDataAccessObject<Entities.Sex> sexDao;
        private readonly IDataAccessObject<Entities.Skier> skierDao;
        private readonly IDataAccessObject<Entities.StartList> startListDao;
        private readonly IDataAccessObject<Entities.StartPosition> startPositionDao;
        private readonly IDataAccessObject<Entities.TimeMeasurement> timeMeasurementDao;
        private readonly IDataAccessObject<Entities.Season> seasonDao;

        #endregion
        #region Constructors

        public InformationManager(
            IDataAccessObject<Entities.Country> countryDao,
            IDataAccessObject<Entities.Race> raceDao,
            IDataAccessObject<Entities.RaceData> raceDataDao,
            IDataAccessObject<Entities.RaceState> raceStateDao,
            IDataAccessObject<Entities.RaceType> raceTypeDao,
            IDataAccessObject<Entities.Season> seasonDao,
            IDataAccessObject<Entities.SeasonPlan> seasonPlanDao,
            IDataAccessObject<Entities.Sex> sexDao,
            IDataAccessObject<Entities.Skier> skierDao,
            IDataAccessObject<Entities.StartList> startListDao,
            IDataAccessObject<Entities.StartPosition> startPositionDao,
            IDataAccessObject<Entities.TimeMeasurement> timeMeasurementDao,
            IDataAccessObject<Entities.Venue> venueDao)
        {
            this.countryDao = countryDao ?? throw new ArgumentNullException(nameof(countryDao));
            this.raceDao = raceDao ?? throw new ArgumentNullException(nameof(raceDao));
            this.raceDataDao = raceDataDao ?? throw new ArgumentNullException(nameof(raceDataDao));
            this.raceStateDao = raceStateDao ?? throw new ArgumentNullException(nameof(raceStateDao));
            this.raceTypeDao = raceTypeDao ?? throw new ArgumentNullException(nameof(raceTypeDao));
            this.venueDao = venueDao ?? throw new ArgumentNullException(nameof(venueDao));
            this.seasonPlanDao = seasonPlanDao ?? throw new ArgumentNullException(nameof(seasonPlanDao));
            this.sexDao = sexDao ?? throw new ArgumentNullException(nameof(sexDao));
            this.skierDao = skierDao ?? throw new ArgumentNullException(nameof(skierDao));
            this.startListDao = startListDao ?? throw new ArgumentNullException(nameof(startListDao));
            this.startPositionDao = startPositionDao ?? throw new ArgumentNullException(nameof(startPositionDao));
            this.timeMeasurementDao = timeMeasurementDao ?? throw new ArgumentNullException(nameof(timeMeasurementDao));
            this.seasonDao = seasonDao ?? throw new ArgumentNullException(nameof(seasonDao));
        }

        #endregion
        #region Methods
        #region Country-Methods

        public async Task<IEnumerable<Domain.Country>> GetAllCountriesAsync()
        {
            return (await countryDao.GetAllConditionalAsync().ConfigureAwait(false))
                .Select(countryEntitiy => new Domain.Country
                {
                    Id = countryEntitiy.Id,
                    Name = countryEntitiy.Name
                });
        }

        public async Task<Domain.Country> GetCountryByIdAsync(int id)
        {
            var countryEntity = await countryDao.GetByIdAsync(id).ConfigureAwait(false);

            return countryEntity != null
                ? new Domain.Country
                {
                    Id = countryEntity.Id,
                    Name = countryEntity.Name
                }
                : null;
        }

        #endregion
        #region Race-Methods

        public async Task<int> CreateOrUpdateRaceAsync(Domain.Race race)
        {
            if (race == null) throw new ArgumentNullException(nameof(race));
            if (race.Id == -1)
            {
                return await CreateRace(race).ConfigureAwait(false);
            }
            else if (race.Id > -1 &&
                (await GetAllRacesAsync().ConfigureAwait(false)).Where(_race => _race.Id == race.Id).Count() == 1)
            {
                return await UpdateRace(race).ConfigureAwait(false);
            }
            throw new ArgumentOutOfRangeException(nameof(race), "trying to update non existent Race, RaceId out of Range");
        }

        public async Task<int> CreateRace(Domain.Race race)
        {
            if (race is null)
                throw new ArgumentNullException(nameof(race));

            int firstStartListId = await startListDao.CreateAsync(new Entities.StartList()).ConfigureAwait(false);
            int secondStartListId = await startListDao.CreateAsync(new Entities.StartList()).ConfigureAwait(false);

            foreach (var racer in race.FirstStartList)
            {
                await startPositionDao.CreateAsync(new Entities.StartPosition
                {
                    StartListId = firstStartListId,
                    SkierId = racer.Reference.Skier.Reference.Id,
                    Position = racer.Reference.Position
                }).ConfigureAwait(false);

                await raceDataDao.CreateAsync(new Entities.RaceData
                {
                    StartListId = firstStartListId,
                    SkierId = racer.Reference.Skier.Reference.Id,
                    RaceStateId = 4                                     // 4 == Startbereit
                }).ConfigureAwait(false);
            }

            return
                await raceDao.CreateAsync(new Entities.Race
                {
                    RaceTypeId = race.RaceType.Reference.Id,
                    VenueId = race.Venue.Reference.Id,
                    FirstStartListId = firstStartListId,
                    SecondStartListId = secondStartListId,
                    NumberOfSensors = race.NumberOfSensors,
                    Description = race.Description,
                    Date = race.Date,
                    GenderSpecificRaceId = race.GenderSpecificRaceId
                })
                .ConfigureAwait(false);
        }

        public async Task<int> UpdateRace(Domain.Race race)
        {
            if (race is null)
                throw new ArgumentNullException(nameof(race));

            Entities.Race entRace = await raceDao.GetByIdAsync(race.Id).ConfigureAwait(false);
            int firstStartListId = entRace.FirstStartListId;

            IEnumerable<Entities.StartPosition> oldStartPositions =
                await startPositionDao.GetAllConditionalAsync(
                    new QueryConditionBuilder().DeclareCondition("StartListId", QueryConditionType.Equals, firstStartListId).Build())
                .ConfigureAwait(false);

            IEnumerable<Entities.RaceData> oldRaceData =
                await raceDataDao.GetAllConditionalAsync(
                    new QueryConditionBuilder().DeclareCondition("StartListId", QueryConditionType.Equals, firstStartListId).Build())
                .ConfigureAwait(false);

            foreach (var sp in oldStartPositions)
            {
                await startPositionDao.DeleteByIdAsync(sp.Id).ConfigureAwait(false);
            }

            foreach (var rd in oldRaceData)
            {
                await raceDataDao.DeleteByIdAsync(rd.Id).ConfigureAwait(false);
            }

            foreach (var racer in race.FirstStartList)
            {
                await startPositionDao.CreateAsync(
                        new Entities.StartPosition
                        {
                            StartListId = firstStartListId,
                            SkierId = racer.Reference.Skier.Reference.Id,
                            Position = racer.Reference.Position
                        })
                    .ConfigureAwait(false);

                await raceDataDao.CreateAsync(
                        new Entities.RaceData
                        {
                            StartListId = firstStartListId,
                            SkierId = racer.Reference.Skier.Reference.Id,
                            RaceStateId = 4                                     // 4 == Startbereit
                        })
                    .ConfigureAwait(false);
            }

            await raceDao.UpdateAsync(
                    new Entities.Race
                    {
                        Id = race.Id,
                        RaceTypeId = race.RaceType.Reference.Id,
                        VenueId = race.Venue.Reference.Id,
                        FirstStartListId = firstStartListId,
                        SecondStartListId = entRace.SecondStartListId,
                        NumberOfSensors = race.NumberOfSensors,
                        Description = race.Description,
                        Date = race.Date,
                        GenderSpecificRaceId = race.GenderSpecificRaceId
                    })
                .ConfigureAwait(false);

            return race.Id;
        }

        public async Task<IEnumerable<Domain.Race>> GetAllRacesAsync(
            Domain.Associated<Domain.RaceType>.LoadingType raceTypeLoadingType = Domain.Associated<Domain.RaceType>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Venue>.LoadingType venueLoadingType = Domain.Associated<Domain.Venue>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Season>.LoadingType seasonLoadingType = Domain.Associated<Domain.Season>.LoadingType.None)
        {
            var raceEntities = await raceDao.GetAllConditionalAsync().ConfigureAwait(false);

            return await Task.WhenAll(
                raceEntities.Select(
                    async raceEntity => new Domain.Race
                    {
                        Date = raceEntity.Date,
                        Description = raceEntity.Description,
                        Id = raceEntity.Id,
                        NumberOfSensors = raceEntity.NumberOfSensors,
                        RaceType = await LoadAssociatedDomainObject(
                                raceTypeLoadingType,
                                async () => new Domain.Associated<Domain.RaceType>(
                                    await this.GetRaceTypeByIdAsync(raceEntity.RaceTypeId).ConfigureAwait(false)),
                                () => new Domain.Associated<Domain.RaceType>(raceEntity.RaceTypeId))
                            .ConfigureAwait(false),
                        Venue = await LoadAssociatedDomainObject(
                                venueLoadingType,
                                async () => new Domain.Associated<Domain.Venue>(
                                    await this.GetVenueByIdAsync(
                                            raceEntity.VenueId,
                                            seasonsOfVenueLoadingType: Domain.Associated<Domain.Season>.LoadingType.None)
                                        .ConfigureAwait(false)),
                                () => new Domain.Associated<Domain.Venue>(raceEntity.VenueId))
                            .ConfigureAwait(false),
                        Season = await LoadAssociatedDomainObject(
                                seasonLoadingType,
                                async () => new Domain.Associated<Domain.Season>(
                                    await GetSeasonByDateAsync(raceEntity.Date).ConfigureAwait(false)))
                            .ConfigureAwait(false)
                    }))
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Domain.Race>> GetAllRacesOfRaceTypesAndSeasonsAsync(
            IEnumerable<int> raceTypeIdSet,
            IEnumerable<int> seasonIdSet)
        {
            var raceTypeConditionBuilder = raceTypeIdSet is null || !raceTypeIdSet.Any()
                ? new QueryConditionBuilder()
                    .DeclareTautologyCondition()
                : new QueryConditionBuilder()
                    .DeclareConditionFromBuilderSet(
                        QueryConditionNodeType.Or,
                        raceTypeIdSet.Select(raceTypeId => new QueryConditionBuilder()
                            .DeclareCondition(nameof(Entities.Race.RaceTypeId), QueryConditionType.Equals, raceTypeId)));

            QueryConditionBuilder seasonConditionBuilder;
            if (seasonIdSet is null || !seasonIdSet.Any())
                seasonConditionBuilder = new QueryConditionBuilder()
                    .DeclareTautologyCondition();
            else
            {
                var seasonEntSet = await seasonDao.GetAllConditionalAsync().ConfigureAwait(false);
                var seasonPlanEntSet = (await seasonPlanDao.GetAllConditionalAsync().ConfigureAwait(false))
                    .Where(sp => seasonIdSet.Any(seasonId => seasonId == sp.SeasonId));

                seasonConditionBuilder = !seasonPlanEntSet.Any()
                    ? new QueryConditionBuilder()
                        .DeclareContradictingCondition()
                    : seasonConditionBuilder = new QueryConditionBuilder()
                        .DeclareConditionFromBuilderSet(
                            QueryConditionNodeType.Or,
                            seasonPlanEntSet.Select(sp => new QueryConditionBuilder()
                                .DeclareConditionNode(
                                    QueryConditionNodeType.And,
                                    () => new QueryConditionBuilder()
                                        .DeclareCondition(nameof(Entities.Race.VenueId), QueryConditionType.Equals, sp.VenueId),
                                    () => new QueryConditionBuilder()
                                        .DeclareConditionNode(
                                            QueryConditionNodeType.And,
                                            () => new QueryConditionBuilder()
                                                .DeclareCondition(
                                                    nameof(Entities.Race.Date),
                                                    QueryConditionType.GreaterThanOrEquals,
                                                    seasonEntSet.First(s => s.Id == sp.SeasonId).StartDate),
                                            () => new QueryConditionBuilder()
                                                .DeclareCondition(
                                                    nameof(Entities.Race.Date),
                                                    QueryConditionType.LessThanOrEquals,
                                                    seasonEntSet.First(s => s.Id == sp.SeasonId).EndDate)))));
            }

            var raceCondition = new QueryConditionBuilder()
                .DeclareConditionNode(
                    QueryConditionNodeType.And,
                    () => raceTypeConditionBuilder,
                    () => seasonConditionBuilder)
                .Build();

            var raceEntSet = await this.raceDao.GetAllConditionalAsync(raceCondition).ConfigureAwait(false);

            return await Task.WhenAll(
                    raceEntSet.Select(async raceEnt => await this.GetRaceByIdAsync(
                            raceEnt.Id,
                            overallRaceStateLoadingType: Domain.Associated<Domain.RaceState>.LoadingType.Reference,
                            raceTypeLoadingType: Domain.Associated<Domain.RaceType>.LoadingType.Reference,
                            venueLoadingType: Domain.Associated<Domain.Venue>.LoadingType.Reference,
                            seasonLoadingType: Domain.Associated<Domain.Season>.LoadingType.Reference)
                        .ConfigureAwait(false)))
                .ConfigureAwait(false);
        }

        public async Task<Domain.Race> GetRaceByIdAsync(
            int raceId,
            Domain.Associated<Domain.RaceState>.LoadingType overallRaceStateLoadingType = Domain.Associated<Domain.RaceState>.LoadingType.Reference,
            Domain.Associated<Domain.RaceType>.LoadingType raceTypeLoadingType = Domain.Associated<Domain.RaceType>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Venue>.LoadingType venueLoadingType = Domain.Associated<Domain.Venue>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Season>.LoadingType seasonLoadingType = Domain.Associated<Domain.Season>.LoadingType.None,
            Domain.Associated<Domain.StartPosition>.LoadingType startListLoadingType = Domain.Associated<Domain.StartPosition>.LoadingType.None,
            Domain.Associated<Domain.Skier>.LoadingType skierLoadingType = Domain.Associated<Domain.Skier>.LoadingType.None,
            Domain.Associated<Domain.Sex>.LoadingType skierSexLoadingType = Domain.Associated<Domain.Sex>.LoadingType.None,
            Domain.Associated<Domain.Country>.LoadingType skierCountryLoadingType = Domain.Associated<Domain.Country>.LoadingType.None)
        {
            var raceEntity = await this.raceDao.GetByIdAsync(raceId).ConfigureAwait(false);

            var race = raceEntity is null
                ? null
                : new Domain.Race
                {
                    Date = raceEntity.Date,
                    Description = raceEntity.Description,
                    Id = raceEntity.Id,
                    NumberOfSensors = raceEntity.NumberOfSensors,
                    OverallRaceState = await LoadAssociatedDomainObject(
                            overallRaceStateLoadingType,
                            async () => new Domain.Associated<Domain.RaceState>(
                                await LoadOverallRaceStateOfRace(raceEntity.Id).ConfigureAwait(false)),
                            async () => new Domain.Associated<Domain.RaceState>(
                                (await LoadOverallRaceStateOfRace(raceEntity.Id).ConfigureAwait(false)).Id))
                        .ConfigureAwait(false),
                    RaceType = await LoadAssociatedDomainObject(
                            raceTypeLoadingType,
                            async () => new Domain.Associated<Domain.RaceType>(
                                await this.GetRaceTypeByIdAsync(raceEntity.RaceTypeId).ConfigureAwait(false)),
                            () => new Domain.Associated<Domain.RaceType>(raceEntity.RaceTypeId))
                        .ConfigureAwait(false),
                    Season = await LoadAssociatedDomainObject(
                            seasonLoadingType,
                            async () => new Domain.Associated<Domain.Season>(
                                await GetSeasonByDateAsync(raceEntity.Date).ConfigureAwait(false)))
                        .ConfigureAwait(false),
                    Venue = await LoadAssociatedDomainObject(
                            venueLoadingType,
                            async () => new Domain.Associated<Domain.Venue>(
                                await this.GetVenueByIdAsync(
                                        raceEntity.VenueId,
                                        seasonsOfVenueLoadingType: Domain.Associated<Domain.Season>.LoadingType.None)
                                    .ConfigureAwait(false)),
                            () => new Domain.Associated<Domain.Venue>(raceEntity.VenueId))
                        .ConfigureAwait(false),
                    FirstStartList = await LoadAssociatedDomainObjectSet(
                            startListLoadingType,
                            async () =>
                                (await GetAllStartPositionsOfStartList(raceEntity.FirstStartListId, skierLoadingType)
                                    .ConfigureAwait(false))
                                .Select(startPosition => new Domain.Associated<Domain.StartPosition>(startPosition)),
                            async () =>
                                (await GetAllStartPositionsOfStartList(raceEntity.FirstStartListId, skierLoadingType)
                                    .ConfigureAwait(false))
                                .Select(startPosition => new Domain.Associated<Domain.StartPosition>(startPosition.Id)))
                        .ConfigureAwait(false),
                    SecondStartList = await LoadAssociatedDomainObjectSet(
                            startListLoadingType,
                            async () =>
                                (await GetAllStartPositionsOfStartList(raceEntity.SecondStartListId, skierLoadingType)
                                    .ConfigureAwait(false))
                                .Select(startPosition => new Domain.Associated<Domain.StartPosition>(startPosition)),
                            async () =>
                                (await GetAllStartPositionsOfStartList(raceEntity.SecondStartListId, skierLoadingType)
                                    .ConfigureAwait(false))
                                .Select(startPosition => new Domain.Associated<Domain.StartPosition>(startPosition.Id)))
                        .ConfigureAwait(false)
                };

            if (skierLoadingType != Domain.Associated<Domain.Skier>.LoadingType.None)
            {
                if (startListLoadingType != Domain.Associated<Domain.StartPosition>.LoadingType.Reference)
                    throw new InvalidOperationException("Cannot load skiers if startlists arent loaded.");

                var skierIds = new List<int>();
                skierIds.AddRange(race.FirstStartList.Select(position => position.Reference.Skier.Reference.Id));
                skierIds.AddRange(race.SecondStartList.Select(position => position.Reference.Skier.Reference.Id));

                if (skierIds.Any())
                {
                    switch (skierLoadingType)
                    {
                        case Domain.Associated<Domain.Skier>.LoadingType.ForeignKey:
                            race.Skiers = skierIds.Distinct()
                                .Select(skierId => new Domain.Associated<Domain.Skier>(skierId));
                            break;
                        case Domain.Associated<Domain.Skier>.LoadingType.Reference:
                            race.Skiers = await Task.WhenAll(
                                    skierIds.Distinct().Select(
                                        async skierId =>
                                            new Domain.Associated<Domain.Skier>(
                                                await GetSkierByIdAsync(
                                                    skierId,
                                                    skierSexLoadingType,
                                                    skierCountryLoadingType,
                                                    Domain.Associated<Domain.StartPosition>.LoadingType.None)
                                                .ConfigureAwait(false))))
                                .ConfigureAwait(false);
                            break;
                        default:
                            break;
                    }
                }
            }

            return race;
        }

        public async Task DeleteRaceAsync(int raceId)
        {
            var raceEntity = await this.raceDao.GetByIdAsync(raceId).ConfigureAwait(false);

            var startListIdSet = new List<int> { raceEntity.FirstStartListId, raceEntity.SecondStartListId };

            var raceDataIdSet = new List<int>();
            foreach (var startListId in startListIdSet)
            {
                var raceDataCondition = new QueryConditionBuilder()
                    .DeclareCondition(nameof(Entities.RaceData.StartListId), QueryConditionType.Equals, startListId)
                    .Build();

                raceDataIdSet.AddRange(
                    (await raceDataDao.GetAllConditionalAsync(raceDataCondition).ConfigureAwait(false))
                        .Select(rd => rd.Id));
            }

            if (raceDataIdSet.Any())
            {
                var timeMeasurementRemoveCondition = new QueryConditionBuilder()
                .DeclareConditionFromBuilderSet(
                    QueryConditionNodeType.Or,
                    raceDataIdSet.Select(
                        raceDataId => new QueryConditionBuilder()
                            .DeclareCondition(nameof(Entities.TimeMeasurement.RaceDataId), QueryConditionType.Equals, raceDataId)))
                .Build();
                await timeMeasurementDao.DeleteAsync(timeMeasurementRemoveCondition).ConfigureAwait(false);
            }

            var raceDataRemoveCondition = new QueryConditionBuilder()
                .DeclareConditionFromBuilderSet(
                    QueryConditionNodeType.Or,
                    startListIdSet.Select(
                        startListId => new QueryConditionBuilder()
                            .DeclareCondition(nameof(Entities.RaceData.StartListId), QueryConditionType.Equals, startListId)))
                .Build();
            await raceDataDao.DeleteAsync(raceDataRemoveCondition).ConfigureAwait(false);

            var startPositionRemoveCondition = new QueryConditionBuilder()
                .DeclareConditionFromBuilderSet(
                    QueryConditionNodeType.Or,
                    startListIdSet.Select(
                        startListId => new QueryConditionBuilder()
                            .DeclareCondition(nameof(Entities.StartPosition.StartListId), QueryConditionType.Equals, startListId)))
                .Build();
            await startPositionDao.DeleteAsync(startPositionRemoveCondition).ConfigureAwait(false);

            await raceDao.DeleteByIdAsync(raceId).ConfigureAwait(false);

            var startListRemoveCondition = new QueryConditionBuilder()
                .DeclareConditionFromBuilderSet(
                    QueryConditionNodeType.Or,
                    startListIdSet.Select(
                        startListId => new QueryConditionBuilder()
                            .DeclareCondition(nameof(Entities.StartList.Id), QueryConditionType.Equals, startListId)))
                .Build();
            await startListDao.DeleteAsync(startListRemoveCondition).ConfigureAwait(false);
        }

        public async Task GenerateSecondStartList(int raceId)
        {
            var raceEnt = await this.raceDao.GetByIdAsync(raceId).ConfigureAwait(false);

            var ranks = (await this.GetRankedSkiersOfRaceAsync(raceId).ConfigureAwait(false))
                .OrderBy(r => r.Rank)
                .Take(30)
                .OrderByDescending(r => r.Rank);

            var raceStateEntSet = await this.raceStateDao.GetAllConditionalAsync().ConfigureAwait(false);

            var startPositionCounter = 1;
            foreach (var rank in ranks)
            {
                var startPositionEnt = new Entities.StartPosition
                {
                    Position = startPositionCounter++,
                    SkierId = rank.Id,
                    StartListId = raceEnt.SecondStartListId
                };
                await this.startPositionDao.CreateAsync(startPositionEnt).ConfigureAwait(false);

                var raceDataEnt = new Entities.RaceData
                {
                    RaceStateId = raceStateEntSet.First(rs => rs.Label == "Startbereit").Id,
                    SkierId = rank.Id,
                    StartListId = raceEnt.SecondStartListId
                };
                await this.raceDataDao.CreateAsync(raceDataEnt).ConfigureAwait(false);
            }
        }

        #endregion
        #region RaceData-Methods

        public async Task<Domain.RaceData> GetRaceDataByRaceAndStartlistAndPositionAsync(
            Domain.Race race,
            bool firstStartList,
            int position,
            Domain.Associated<Domain.RaceState>.LoadingType raceStateLoadingType = Domain.Associated<Domain.RaceState>.LoadingType.Reference)
        {
            if (race is null)
                throw new ArgumentNullException(nameof(race));

            var raceEnt = await raceDao.GetByIdAsync(race.Id).ConfigureAwait(false);
            var startListId = firstStartList ? raceEnt.FirstStartListId : raceEnt.SecondStartListId;
            var skier = await GetSkierByRaceAndStartlistAndPositionAsync(race, firstStartList, position).ConfigureAwait(false);

            var raceDataCondition = new QueryConditionBuilder()
                .DeclareConditionNode(
                    QueryConditionNodeType.And,
                    () => new QueryConditionBuilder()
                        .DeclareCondition(nameof(Entities.RaceData.SkierId), QueryConditionType.Equals, skier.Id),
                    () => new QueryConditionBuilder()
                        .DeclareCondition(nameof(Entities.RaceData.StartListId), QueryConditionType.Equals, startListId))
                .Build();

            var raceDataEntSet = await raceDataDao.GetAllConditionalAsync(raceDataCondition).ConfigureAwait(false);
            if (raceDataEntSet.Count() != 1)
                throw new InvalidOperationException(
                    $"There exist multiple raceData-Entities that share the same " +
                    $"StartListId {startListId} and SkierId {skier.Id}");

            var raceDataEnt = raceDataEntSet.First();

            return new Domain.RaceData
            {
                Id = raceDataEnt.Id,
                RaceState = await this.LoadAssociatedDomainObject(
                        raceStateLoadingType,
                        async () => new Domain.Associated<Domain.RaceState>(
                            await GetRaceStateById(raceDataEnt.RaceStateId).ConfigureAwait(false)),
                        () => new Domain.Associated<Domain.RaceState>(raceDataEnt.RaceStateId))
                    .ConfigureAwait(false)
            };
        }

        public async Task<bool> UpdateRaceStateOfRaceDataAsync(Domain.RaceData raceData)
        {
            if (raceData is null)
                throw new ArgumentNullException(nameof(raceData));

            var updatedColumns = new
            {
                RaceStateId = raceData.RaceState.ForeignKey ?? raceData.RaceState.Reference.Id
            };
            var updateCondition = new QueryConditionBuilder()
                .DeclareCondition(nameof(Entities.RaceData.Id), QueryConditionType.Equals, raceData.Id)
                .Build();
            return (await raceDataDao.UpdateAsync(updatedColumns, updateCondition).ConfigureAwait(false))
                == 1;
        }

        public async Task<IEnumerable<Domain.RankedSkier>> GetRankedSkiersOfRaceAsync(int raceId)
        {
            var raceEntity = await raceDao.GetByIdAsync(raceId).ConfigureAwait(false);

            if (raceEntity == null) return null;

            var firstStartListCondition = new QueryConditionBuilder()
                .DeclareCondition(nameof(Entities.StartPosition.StartListId), QueryConditionType.Equals, raceEntity.FirstStartListId)
                .Build();
            var skierIdSet = (await startPositionDao.GetAllConditionalAsync(firstStartListCondition).ConfigureAwait(false))
                .Select(sp => sp.SkierId);

            var skierCondition = new QueryConditionBuilder()
                .DeclareConditionFromBuilderSet(
                    QueryConditionNodeType.Or,
                    skierIdSet.Select(
                        skierId => new QueryConditionBuilder()
                            .DeclareCondition(nameof(Entities.Skier.Id), QueryConditionType.Equals, skierId)))
                .Build();
            var skierEntSet = await skierDao.GetAllConditionalAsync(skierCondition).ConfigureAwait(false);

            Func<int, IQueryCondition> raceDataConditionGenerator =
                startListId =>
                {
                    return new QueryConditionBuilder()
                       .DeclareConditionNode(
                           QueryConditionNodeType.And,
                           () => new QueryConditionBuilder()
                               .DeclareCondition(nameof(Entities.RaceData.StartListId), QueryConditionType.Equals, startListId),
                           () => new QueryConditionBuilder()
                               .DeclareConditionFromBuilderSet(
                                   QueryConditionNodeType.Or,
                                   skierIdSet.Select(skierId => new QueryConditionBuilder()
                                       .DeclareCondition(nameof(Entities.RaceData.SkierId), QueryConditionType.Equals, skierId))))
                       .Build();
                };

            var firstRaceDataSet = await raceDataDao.GetAllConditionalAsync(
                    raceDataConditionGenerator(raceEntity.FirstStartListId))
                .ConfigureAwait(false);
            var secondRaceDataSet = await raceDataDao.GetAllConditionalAsync(
                    raceDataConditionGenerator(raceEntity.SecondStartListId))
                .ConfigureAwait(false);

            if (!firstRaceDataSet.Any())
                throw new InvalidOperationException("no results yet to this race");

            var timeMeasurementConditionBuilder = new QueryConditionBuilder()
                            .DeclareConditionFromBuilderSet(
                                QueryConditionNodeType.Or,
                                firstRaceDataSet.Select(rd => new QueryConditionBuilder()
                                    .DeclareCondition(nameof(Entities.TimeMeasurement.RaceDataId), QueryConditionType.Equals, rd.Id)));

            if (secondRaceDataSet.Any())
            {
                timeMeasurementConditionBuilder = new QueryConditionBuilder()
                    .DeclareConditionNode(
                        QueryConditionNodeType.Or,
                        () => timeMeasurementConditionBuilder,
                        () => new QueryConditionBuilder()
                            .DeclareConditionFromBuilderSet(
                                QueryConditionNodeType.Or,
                                secondRaceDataSet.Select(rd => new QueryConditionBuilder()
                                    .DeclareCondition(nameof(Entities.TimeMeasurement.RaceDataId), QueryConditionType.Equals, rd.Id))));
            }

            var timeMeasurementCondition = timeMeasurementConditionBuilder.Build();
            var timeMeasurementSet = await timeMeasurementDao.GetAllConditionalAsync(timeMeasurementCondition)
                .ConfigureAwait(false);

            var countryEntSet = await countryDao.GetAllConditionalAsync().ConfigureAwait(false);

            var rankedSkierTimeSet = new List<(
                Domain.RankedSkier RankedSkier,
                IEnumerable<TimeSpan> ElapsedMeasurementsInFirstRun,
                IEnumerable<TimeSpan> ElapsedMeasurementsInSecondRun)>();
            foreach (var skierEnt in skierEntSet)
            {
                var countryEnt = countryEntSet.First(c => c.Id == skierEnt.CountryId);

                var rankedSkier = new Domain.RankedSkier()
                {
                    Id = skierEnt.Id,
                    DateOfBirth = skierEnt.DateOfBirth,
                    FirstName = skierEnt.FirstName,
                    LastName = skierEnt.LastName,
                    ImageUrl = skierEnt.ImageUrl,
                    IsRemoved = skierEnt.IsRemoved,
                    Country = new Domain.Associated<Domain.Country>(new Domain.Country { Id = countryEnt.Id, Name = countryEnt.Name })
                };

                var firstRaceDataEnt = firstRaceDataSet.First(rd => rd.SkierId == skierEnt.Id);

                var firstFinalMeasurement = timeMeasurementSet
                    .FirstOrDefault(tm => tm.RaceDataId == firstRaceDataEnt.Id &&
                                          tm.IsValid &&
                                          tm.SensorId == raceEntity.NumberOfSensors - 1);

                rankedSkier.ElapsedTimeInFirstRun = firstFinalMeasurement != null
                    ? TimeSpan.FromMilliseconds(firstFinalMeasurement.Measurement)
                    : TimeSpan.MaxValue;

                var measurementsOfFirstRun = timeMeasurementSet
                    .Where(tm => tm.RaceDataId == firstRaceDataEnt.Id &&
                                 tm.IsValid)
                    .Select(tm => (tm.SensorId, TSMeasurement: TimeSpan.FromMilliseconds(tm.Measurement)))
                    .ToList();
                measurementsOfFirstRun.AddRange(
                    Enumerable.Range(0, raceEntity.NumberOfSensors)
                        .Where(sensorId => !measurementsOfFirstRun.Any(m => m.SensorId == sensorId))
                        .Select(sensorId => (sensorId, TimeSpan.MaxValue)));
                var elapsedMeasurementsInFirstRun = measurementsOfFirstRun
                    .OrderBy(m => m.SensorId)
                    .Select(m => m.TSMeasurement);

                var secondRaceDataEnt = secondRaceDataSet.FirstOrDefault(rd => rd.SkierId == skierEnt.Id);
                IEnumerable<TimeSpan> elapsedMeasurementsInSecondRun = null;
                if (secondRaceDataEnt != null)
                {
                    var secondFinalMeasurement = timeMeasurementSet
                        .FirstOrDefault(tm => tm.RaceDataId == secondRaceDataEnt.Id &&
                                              tm.IsValid &&
                                              tm.SensorId == raceEntity.NumberOfSensors - 1);

                    rankedSkier.ElapsedTimeInSecondRun = secondFinalMeasurement != null
                        ? TimeSpan.FromMilliseconds(secondFinalMeasurement.Measurement)
                        : TimeSpan.MaxValue;

                    var measurementsOfSecondRun = timeMeasurementSet
                        .Where(tm => tm.RaceDataId == secondRaceDataEnt.Id &&
                                     tm.IsValid)
                        .Select(tm => (tm.SensorId, TSMeasurement: TimeSpan.FromMilliseconds(tm.Measurement)))
                        .ToList();
                    measurementsOfSecondRun.AddRange(
                        Enumerable.Range(0, raceEntity.NumberOfSensors)
                            .Where(sensorId => !measurementsOfSecondRun.Any(m => m.SensorId == sensorId))
                            .Select(sensorId => (sensorId, TimeSpan.MaxValue)));
                    elapsedMeasurementsInSecondRun = measurementsOfSecondRun
                        .OrderBy(m => m.SensorId)
                        .Select(m => m.TSMeasurement);
                }

                rankedSkier.ElapsedTotalTime =
                    rankedSkier.ElapsedTimeInFirstRun != TimeSpan.MaxValue && rankedSkier.ElapsedTimeInSecondRun != TimeSpan.MaxValue
                        ? rankedSkier.ElapsedTimeInFirstRun + rankedSkier.ElapsedTimeInSecondRun
                        : TimeSpan.MaxValue;

                rankedSkierTimeSet.Add((rankedSkier, elapsedMeasurementsInFirstRun, elapsedMeasurementsInSecondRun));
            }

            var orderedRankedSkierTimeSet = rankedSkierTimeSet.OrderBy(rst => rst.RankedSkier.ElapsedTotalTime).ToList();

            var counter = 1;
            foreach (var rankedSkier in orderedRankedSkierTimeSet.Select(rst => rst.RankedSkier))
            {
                rankedSkier.Rank = counter++;
            }

            var (bestRankedSkier, bestElapsedMeasurementsInFirstRun, bestElapsedMeasurementsInSecondRun) =
                orderedRankedSkierTimeSet.First(ors => ors.RankedSkier.Rank == 1);

            for (int i = 0; i < orderedRankedSkierTimeSet.Count; i++)
            {
                var (rankedSkier, elapsedMeasurementsInFirstRun, elapsedMeasurementsInSecondRun) = orderedRankedSkierTimeSet[i];

                var notFirstRankedSkier = rankedSkier.Rank != 1;
                if (notFirstRankedSkier)
                {
                    if (rankedSkier.ElapsedTimeInFirstRun != TimeSpan.MaxValue)
                        rankedSkier.ElapsedTimeInFirstRun -= bestRankedSkier.ElapsedTimeInFirstRun;

                    elapsedMeasurementsInFirstRun = this.UpdateMeasurementsAccordingToBest(
                        elapsedMeasurementsInFirstRun,
                        bestElapsedMeasurementsInFirstRun);

                    if (rankedSkier.ElapsedTimeInSecondRun != TimeSpan.MaxValue)
                        rankedSkier.ElapsedTimeInSecondRun -= bestRankedSkier.ElapsedTimeInSecondRun;

                    elapsedMeasurementsInSecondRun = this.UpdateMeasurementsAccordingToBest(
                        elapsedMeasurementsInSecondRun,
                        bestElapsedMeasurementsInSecondRun);
                }

                rankedSkier.ElapsedTimeInFirstRunString = rankedSkier.ElapsedTimeInFirstRun != TimeSpan.MaxValue
                    ? this.FormatTimeSpan(rankedSkier.ElapsedTimeInFirstRun, notFirstRankedSkier)
                    : "n/a";

                rankedSkier.ElapsedMeasurementStringsInFirstRun = this.FormatElapsedMeasurements(
                    elapsedMeasurementsInFirstRun,
                    bestElapsedMeasurementsInFirstRun,
                    notFirstRankedSkier);

                rankedSkier.ElapsedTimeInSecondRunString = rankedSkier.ElapsedTimeInSecondRun != TimeSpan.MaxValue
                    ? this.FormatTimeSpan(rankedSkier.ElapsedTimeInSecondRun, notFirstRankedSkier)
                    : "n/a";

                rankedSkier.ElapsedMeasurementStringsInSecondRun = this.FormatElapsedMeasurements(
                    elapsedMeasurementsInSecondRun,
                    bestElapsedMeasurementsInSecondRun,
                    notFirstRankedSkier);

                rankedSkier.ElapsedTotalTimeString = rankedSkier.ElapsedTotalTime != TimeSpan.MaxValue
                    ? this.FormatTimeSpan(rankedSkier.ElapsedTotalTime, false)
                    : "n/a";
            }

            return orderedRankedSkierTimeSet.Select(rst => rst.RankedSkier);
        }

        #endregion
        #region RaceState-Methods

        public async Task<IEnumerable<Domain.RaceState>> GetAllRaceStatesAsync()
        {
            return (await raceStateDao.GetAllConditionalAsync().ConfigureAwait(false))
                .Select(raceStateEntity => new Domain.RaceState
                {
                    Id = raceStateEntity.Id,
                    Label = raceStateEntity.Label
                });
        }

        public async Task<Domain.RaceState> GetRaceStateById(int raceStateId)
        {
            var raceStateEnt = await raceStateDao.GetByIdAsync(raceStateId).ConfigureAwait(false);

            return new Domain.RaceState
            {
                Id = raceStateEnt.Id,
                Label = raceStateEnt.Label
            };
        }

        public async Task<Domain.RaceState> LoadOverallRaceStateOfRace(int raceId)
        {
            var raceEnt = await raceDao.GetByIdAsync(raceId).ConfigureAwait(false);

            var raceDataSet = new List<Entities.RaceData>();
            foreach (var startListId in new int[] { raceEnt.FirstStartListId, raceEnt.SecondStartListId })
            {
                var raceDataCondition = new QueryConditionBuilder()
                    .DeclareCondition(nameof(Entities.RaceData.StartListId), QueryConditionType.Equals, startListId)
                    .Build();

                raceDataSet.AddRange(
                    await raceDataDao.GetAllConditionalAsync(raceDataCondition)
                        .ConfigureAwait(false));
            }

            var raceStates = await raceStateDao.GetAllConditionalAsync().ConfigureAwait(false);
            var finishedRaceState = raceStates.First(rs => rs.Label == "Abgeschlossen");
            var runningRaceState = raceStates.First(rs => rs.Label == "Laufend");
            var readyToStartRaceState = raceStates.First(rs => rs.Label == "Startbereit");

            if (raceDataSet.All(rd => rd.RaceStateId != runningRaceState.Id && rd.RaceStateId != readyToStartRaceState.Id))
                return await GetRaceStateById(finishedRaceState.Id).ConfigureAwait(false);
            else if (raceDataSet.Any(rd => rd.RaceStateId == runningRaceState.Id))
                return await GetRaceStateById(runningRaceState.Id).ConfigureAwait(false);
            else
                return await GetRaceStateById(readyToStartRaceState.Id).ConfigureAwait(false);
        }

        #endregion
        #region RaceType-Methods

        public async Task<IEnumerable<Domain.RaceType>> GetAllRaceTypesAsync()
        {
            return (await raceTypeDao.GetAllConditionalAsync().ConfigureAwait(false))
                .Select(raceTypeEntity => new Domain.RaceType
                {
                    Id = raceTypeEntity.Id,
                    Label = raceTypeEntity.Label
                }); ;
        }

        public async Task<Domain.RaceType> GetRaceTypeByIdAsync(int id)
        {
            var raceTypeEntity = await raceTypeDao.GetByIdAsync(id).ConfigureAwait(false);

            return raceTypeEntity is null
                ? null
                : new Domain.RaceType
                {
                    Id = raceTypeEntity.Id,
                    Label = raceTypeEntity.Label
                };
        }

        #endregion
        #region Season-Methods

        public async Task<IEnumerable<Domain.Season>> GetAllSeasonsAsync()
        {
            return (await seasonDao.GetAllConditionalAsync().ConfigureAwait(false))
                .Select(seasonEntity => new Domain.Season
                {
                    Id = seasonEntity.Id,
                    Name = seasonEntity.Name,
                    StartDate = seasonEntity.StartDate,
                    EndDate = seasonEntity.EndDate
                });
        }

        public async Task<Domain.Season> GetSeasonByDateAsync(DateTime date)
        {
            var seasonCondition = new QueryConditionBuilder()
                .DeclareConditionNode(
                    QueryConditionNodeType.And,
                    () => new QueryConditionBuilder()
                        .DeclareCondition(nameof(Entities.Season.StartDate), QueryConditionType.LessThanOrEquals, date),
                    () => new QueryConditionBuilder()
                        .DeclareCondition(nameof(Entities.Season.EndDate), QueryConditionType.GreaterThanOrEquals, date))
                .Build();

            return (await seasonDao.GetAllConditionalAsync(seasonCondition).ConfigureAwait(false))
                .Select(seasonEntity => new Domain.Season
                {
                    Id = seasonEntity.Id,
                    Name = seasonEntity.Name,
                    StartDate = seasonEntity.StartDate,
                    EndDate = seasonEntity.EndDate
                })
                .FirstOrDefault();
        }

        public async Task<IEnumerable<Domain.Season>> GetAllSeasonsByVenueIdAsync(int venueId)
        {
            var seasonPlanCondition = new QueryConditionBuilder()
                   .DeclareCondition(nameof(Entities.SeasonPlan.VenueId), QueryConditionType.Equals, venueId)
                   .Build();
            var seasonPlanEntities = await seasonPlanDao.GetAllConditionalAsync(seasonPlanCondition).ConfigureAwait(false);

            return (await Task.WhenAll(
                        seasonPlanEntities.Select(async sp => await seasonDao.GetByIdAsync(sp.SeasonId).ConfigureAwait(false)))
                    .ConfigureAwait(false))
                .Select(seasonEntity => new Domain.Season
                {
                    Id = seasonEntity.Id,
                    Name = seasonEntity.Name,
                    StartDate = seasonEntity.StartDate,
                    EndDate = seasonEntity.EndDate
                });
        }

        #endregion
        #region Sex-Methods

        public async Task<Domain.Sex> GetSexByIdAsync(int sexId)
        {
            var sexEntity = await sexDao.GetByIdAsync(sexId).ConfigureAwait(false);
            return new Domain.Sex
            {
                Id = sexEntity.Id,
                Label = sexEntity.Label
            };
        }

        #endregion
        #region Skier-Methods

        public async Task<IEnumerable<Domain.Skier>> GetAllSkiersAsync(
            Domain.Associated<Domain.Sex>.LoadingType sexLoadingType = Domain.Associated<Domain.Sex>.LoadingType.Reference,
            Domain.Associated<Domain.Country>.LoadingType countryLoadingType = Domain.Associated<Domain.Country>.LoadingType.Reference,
            Domain.Associated<Domain.StartPosition>.LoadingType startPositionLoadingType = Domain.Associated<Domain.StartPosition>.LoadingType.None)
        {
            var skierEntities = await skierDao.GetAllConditionalAsync().ConfigureAwait(false);

            return await Task.WhenAll(
                    skierEntities.Select(
                        async skierEntity => new Domain.Skier
                        {
                            DateOfBirth = skierEntity.DateOfBirth,
                            FirstName = skierEntity.FirstName,
                            Id = skierEntity.Id,
                            ImageUrl = skierEntity.ImageUrl,
                            IsRemoved = skierEntity.IsRemoved,
                            LastName = skierEntity.LastName,
                            Country = await this.LoadAssociatedDomainObject(
                                    countryLoadingType,
                                    async () =>
                                        new Domain.Associated<Domain.Country>(
                                            await GetCountryByIdAsync(skierEntity.CountryId).ConfigureAwait(false)),
                                     () => new Domain.Associated<Domain.Country>(skierEntity.CountryId))
                                .ConfigureAwait(false),
                            Sex = await this.LoadAssociatedDomainObject(
                                    sexLoadingType,
                                    async () =>
                                        new Domain.Associated<Domain.Sex>(
                                            await GetSexByIdAsync(skierEntity.SexId).ConfigureAwait(false)),
                                    () => new Domain.Associated<Domain.Sex>(skierEntity.SexId))
                                .ConfigureAwait(false)
                        }))
                .ConfigureAwait(false);
        }

        public async Task<Domain.Skier> GetSkierByIdAsync(
            int skierId,
            Domain.Associated<Domain.Sex>.LoadingType sexLoadingType = Domain.Associated<Domain.Sex>.LoadingType.Reference,
            Domain.Associated<Domain.Country>.LoadingType countryLoadingType = Domain.Associated<Domain.Country>.LoadingType.Reference,
            Domain.Associated<Domain.StartPosition>.LoadingType startPositionLoadingType = Domain.Associated<Domain.StartPosition>.LoadingType.None)
        {
            var skierEntity = await skierDao.GetByIdAsync(skierId).ConfigureAwait(false);

            return new Domain.Skier
            {
                DateOfBirth = skierEntity.DateOfBirth,
                FirstName = skierEntity.FirstName,
                Id = skierEntity.Id,
                ImageUrl = skierEntity.ImageUrl,
                IsRemoved = skierEntity.IsRemoved,
                LastName = skierEntity.LastName,
                Country = await this.LoadAssociatedDomainObject(
                        countryLoadingType,
                        async () =>
                            new Domain.Associated<Domain.Country>(
                                await GetCountryByIdAsync(skierEntity.CountryId).ConfigureAwait(false)),
                        () => new Domain.Associated<Domain.Country>(skierEntity.CountryId))
                    .ConfigureAwait(false),
                Sex = await this.LoadAssociatedDomainObject(
                        sexLoadingType,
                        async () =>
                            new Domain.Associated<Domain.Sex>(
                                await GetSexByIdAsync(skierEntity.SexId).ConfigureAwait(false)),
                        () => new Domain.Associated<Domain.Sex>(skierEntity.SexId))
                    .ConfigureAwait(false)
            };
        }

        public async Task<Domain.Skier> GetSkierByRaceAndStartlistAndPositionAsync(Domain.Race race, bool firstStartList, int position)
        {
            if (race is null)
                throw new ArgumentNullException(nameof(race));

            var raceEnt = await raceDao.GetByIdAsync(race.Id).ConfigureAwait(false);

            var startPositionCondition = new QueryConditionBuilder()
                .DeclareConditionNode(
                    QueryConditionNodeType.And,
                    () => new QueryConditionBuilder()
                        .DeclareCondition(nameof(Entities.StartPosition.Position), QueryConditionType.Equals, position),
                    () => new QueryConditionBuilder()
                        .DeclareCondition(
                            nameof(Entities.StartPosition.StartListId),
                            QueryConditionType.Equals,
                            firstStartList ? raceEnt.FirstStartListId : raceEnt.SecondStartListId))
                .Build();
            var startPositionSet = await startPositionDao.GetAllConditionalAsync(startPositionCondition).ConfigureAwait(false);

            if (startPositionSet.Count() != 1) //startPositionSet is null
                throw new InvalidOperationException(
                    $"No Startpositions found for Race with id {race.Id} that have a position of {position}");

            var startPosition = startPositionSet.First();
            return await GetSkierByIdAsync(startPosition.SkierId).ConfigureAwait(false);
        }

        public async Task<Domain.Skier> GetSkierByStartPositionAsync(int startPositionId)
        {
            var startPositionEnt = await this.startPositionDao.GetByIdAsync(startPositionId).ConfigureAwait(false);

            return await this.GetSkierByIdAsync(
                    startPositionEnt.SkierId,
                    Domain.Associated<Domain.Sex>.LoadingType.Reference,
                    Domain.Associated<Domain.Country>.LoadingType.Reference,
                    Domain.Associated<Domain.StartPosition>.LoadingType.None)
                .ConfigureAwait(false);
        }

        public async Task<bool> IsLastSkierOfStartList(Domain.RaceData raceData)
        {
            if (raceData is null)
            {
                //todo: load raceData when a untracked racer skips race execution
                throw new ArgumentNullException(nameof(raceData));
            }

            var raceDataEnt = await this.raceDataDao.GetByIdAsync(raceData.Id).ConfigureAwait(false);

            var startPositionCondition = new QueryConditionBuilder()
                .DeclareCondition(nameof(Entities.StartPosition.StartListId), QueryConditionType.Equals, raceDataEnt.StartListId)
                .Build();
            var startPositionEntSet = await this.startPositionDao.GetAllConditionalAsync(startPositionCondition)
                .ConfigureAwait(false);

            var startPositionEnt = startPositionEntSet.First(sp => sp.SkierId == raceDataEnt.SkierId &&
                                                                sp.StartListId == raceDataEnt.StartListId);

            var raceDataCondition = new QueryConditionBuilder()
                .DeclareCondition(nameof(Entities.RaceData.StartListId), QueryConditionType.Equals, raceDataEnt.StartListId)
                .Build();
            var raceDataEntSet = await this.raceDataDao.GetAllConditionalAsync(raceDataCondition)
                .ConfigureAwait(false);

            var remainingStartPositions = raceDataEntSet
                .Select(rd => new
                {
                    RaceData = rd,
                    StartPosition = startPositionEntSet.First(sp => sp.StartListId == rd.StartListId &&
                                                                 sp.SkierId == rd.SkierId)
                })
                .Where(item => item.StartPosition.Position > startPositionEnt.Position);

            return !remainingStartPositions.Any();
        }

        #endregion
        #region StartPosition-Methods

        public async Task<IEnumerable<Domain.StartPosition>> GetStartPositionListAsync(int raceId, bool firstStartList)
        {
            var raceEnt = await this.raceDao.GetByIdAsync(raceId).ConfigureAwait(false);

            var startPositionCondition = new QueryConditionBuilder()
                .DeclareCondition(
                    nameof(Entities.StartPosition.StartListId),
                    QueryConditionType.Equals,
                    firstStartList ? raceEnt.FirstStartListId : raceEnt.SecondStartListId)
                .Build();

            var startPositionEntSet = await startPositionDao.GetAllConditionalAsync(startPositionCondition)
                .ConfigureAwait(false);

            var skierCondition = new QueryConditionBuilder()
                .DeclareConditionFromBuilderSet(
                    QueryConditionNodeType.Or,
                    startPositionEntSet.Select(sp => new QueryConditionBuilder()
                        .DeclareCondition(nameof(Entities.Skier.Id), QueryConditionType.Equals, sp.SkierId)))
                .Build();
            var skierEntSet = await this.skierDao.GetAllConditionalAsync(skierCondition).ConfigureAwait(false);

            var countrySet = await this.GetAllCountriesAsync().ConfigureAwait(false);

            var startPositionSet = new List<Domain.StartPosition>();
            foreach (var startPositionEnt in startPositionEntSet)
            {
                var skierEnt = skierEntSet.First(s => s.Id == startPositionEnt.SkierId);

                var startPosition = new Domain.StartPosition()
                {
                    Id = startPositionEnt.Id,
                    Position = startPositionEnt.Position,
                    Skier = new Domain.Associated<Domain.Skier>(new Domain.Skier
                    {
                        Id = skierEnt.Id,
                        FirstName = skierEnt.FirstName,
                        LastName = skierEnt.LastName,
                        DateOfBirth = skierEnt.DateOfBirth,
                        ImageUrl = skierEnt.ImageUrl,
                        IsRemoved = skierEnt.IsRemoved,
                        Country = new Domain.Associated<Domain.Country>(countrySet.First(c => c.Id == skierEnt.CountryId))
                    })
                };

                startPositionSet.Add(startPosition);
            }

            return startPositionSet;
        }

        public async Task<IEnumerable<Domain.StartPosition>> GetAllStartPositionsOfStartList(
            int startListId,
            Domain.Associated<Domain.Skier>.LoadingType skierLoadingType = Domain.Associated<Domain.Skier>.LoadingType.ForeignKey)
        {
            var condition = new QueryConditionBuilder()
                .DeclareCondition(
                    nameof(Entities.StartPosition.StartListId),
                    QueryConditionType.Equals,
                    startListId)
                .Build();

            return await Task.WhenAll(
                    (await startPositionDao.GetAllConditionalAsync(condition).ConfigureAwait(false))
                        .Select(
                            async startPositionE => new Domain.StartPosition
                            {
                                Id = startPositionE.Id,
                                Position = startPositionE.Position,
                                Skier = await LoadAssociatedDomainObject(
                                        skierLoadingType,
                                        async () =>
                                            new Domain.Associated<Domain.Skier>(
                                                await this.GetSkierByIdAsync(startPositionE.SkierId).ConfigureAwait(false)),
                                        () => new Domain.Associated<Domain.Skier>(startPositionE.SkierId))
                                    .ConfigureAwait(false)
                            }))
                .ConfigureAwait(false);
        }

        public async Task<bool> IsNextStartPositionAsync(Domain.Race race, bool firstStartlist, int position)
        {
            if (race is null)
                throw new ArgumentNullException(nameof(race));

            var raceEnt = await raceDao.GetByIdAsync(race.Id).ConfigureAwait(false);

            var startPositionCondition = new QueryConditionBuilder()
                .DeclareCondition(
                    nameof(Entities.StartPosition.StartListId),
                    QueryConditionType.Equals,
                    firstStartlist ? raceEnt.FirstStartListId : raceEnt.SecondStartListId)
                .Build();
            var startPositionEntities = await startPositionDao.GetAllConditionalAsync(startPositionCondition).ConfigureAwait(false);

            var raceDataConditionSet = startPositionEntities.Select(
                sp => new QueryConditionBuilder()
                    .DeclareConditionNode(
                        QueryConditionNodeType.And,
                        () => new QueryConditionBuilder()
                            .DeclareCondition(nameof(Entities.RaceData.StartListId), QueryConditionType.Equals, sp.StartListId),
                        () => new QueryConditionBuilder()
                            .DeclareCondition(nameof(Entities.RaceData.SkierId), QueryConditionType.Equals, sp.SkierId)));
            var raceDataCondition = new QueryConditionBuilder()
                .DeclareConditionFromBuilderSet(
                    QueryConditionNodeType.Or,
                    raceDataConditionSet)
                .Build();

            var raceDataSet = (await raceDataDao.GetAllConditionalAsync(raceDataCondition).ConfigureAwait(false))
                .Select(rd => (
                    RaceData: rd,
                    StartPosition: startPositionEntities.First(sp => sp.SkierId == rd.SkierId && sp.StartListId == rd.StartListId)
                ));
            var raceStateSet = await raceStateDao.GetAllConditionalAsync().ConfigureAwait(false);

            var startableRaceState = raceStateSet.First(rs => rs.Label == "Startbereit");
            var runningRaceState = raceStateSet.First(rs => rs.Label == "Laufend");
            var startableRaceDataSet = raceDataSet
                .Where(rd => rd.RaceData.RaceStateId == startableRaceState.Id ||
                             rd.RaceData.RaceStateId == runningRaceState.Id)
                .OrderBy(rd => startPositionEntities
                                   .First(sp => sp.SkierId == rd.RaceData.SkierId &&
                                                sp.StartListId == rd.RaceData.StartListId)
                                   .Position);

            return startableRaceDataSet.Any() &&
                startableRaceDataSet.First().StartPosition.Position == position;
        }

        #endregion
        #region TimeMeasurement-Methods

        public async Task<Domain.TimeMeasurement> CreateTimeMeasurementAsync(int measurement, int sensorId, int raceDataId, bool isValid)
        {
            var timeMeasurement = new Entities.TimeMeasurement
            {
                IsValid = isValid,
                Measurement = measurement,
                RaceDataId = raceDataId,
                SensorId = sensorId
            };

            timeMeasurement.Id = await timeMeasurementDao.CreateAsync(timeMeasurement).ConfigureAwait(false);

            return new Domain.TimeMeasurement
            {
                Id = timeMeasurement.Id,
                Measurement = timeMeasurement.Measurement,
                SensorId = timeMeasurement.SensorId
            };
        }

        public async Task<Dictionary<int, (double mean, double standardDeviation)>> CalculateNormalDistributionOfMeasumentsPerSensorAsync(
                int venueId, int raceTypeId)
        {
            var raceCondition = new QueryConditionBuilder()
                .DeclareConditionNode(
                    QueryConditionNodeType.And,
                    () => new QueryConditionBuilder()
                        .DeclareCondition(nameof(Entities.Race.RaceTypeId), QueryConditionType.Equals, raceTypeId),
                    () => new QueryConditionBuilder()
                        .DeclareCondition(nameof(Entities.Race.VenueId), QueryConditionType.Equals, venueId))
                .Build();

            var startListIdSet = (await raceDao.GetAllConditionalAsync(raceCondition).ConfigureAwait(false))
                .OrderByDescending(r => r.Date)
                .Take(15)
                .SelectMany(r => new[] { r.FirstStartListId, r.SecondStartListId });

            var raceDataSet = new List<Entities.RaceData>();
            foreach (var startListId in startListIdSet)
            {
                var raceDataCondition = new QueryConditionBuilder()
                    .DeclareCondition(nameof(Entities.RaceData.StartListId), QueryConditionType.Equals, startListId)
                    .Build();
                raceDataSet.AddRange(await raceDataDao.GetAllConditionalAsync(raceDataCondition).ConfigureAwait(false));
            }

            var timeMeasurementSet = new List<Entities.TimeMeasurement>();
            foreach (var raceData in raceDataSet)
            {
                var timeMeasurementCondition = new QueryConditionBuilder()
                    .DeclareConditionNode(
                        QueryConditionNodeType.And,
                        () => new QueryConditionBuilder()
                            .DeclareCondition(nameof(Entities.TimeMeasurement.RaceDataId), QueryConditionType.Equals, raceData.Id),
                        () => new QueryConditionBuilder()
                            .DeclareCondition(nameof(Entities.TimeMeasurement.IsValid), QueryConditionType.Equals, true))
                    .Build();
                timeMeasurementSet.AddRange(
                    await timeMeasurementDao.GetAllConditionalAsync(timeMeasurementCondition)
                        .ConfigureAwait(false));
            }

            return timeMeasurementSet
                .Where(measurement => timeMeasurementSet.Count(subMeasurement => subMeasurement.SensorId == measurement.SensorId) > 6)
                .GroupBy(
                    m => m.SensorId,
                    (sensorId, result) => (
                        SensorId: sensorId,
                        Mean: Statistics.NormalDistribution.CalculateMean(result.Select(m => m.Measurement)),
                        StdDev: Statistics.NormalDistribution.CalculateStandardDeviation(result.Select(m => m.Measurement))))
                .ToDictionary(
                    distribution => distribution.SensorId,
                    distribution => (distribution.Mean, distribution.StdDev));
        }

        public async Task<Domain.TimeMeasurement> GetTimeMeasurementByRaceDataAndSensorIdAsync(int raceDataId, int sensorId)
        {
            var measurementConditionBuilders = new List<QueryConditionBuilder>
            {
                new QueryConditionBuilder()
                    .DeclareCondition(nameof(Entities.TimeMeasurement.SensorId), QueryConditionType.Equals, sensorId),
                new QueryConditionBuilder()
                    .DeclareCondition(nameof(Entities.TimeMeasurement.RaceDataId), QueryConditionType.Equals, raceDataId),
                new QueryConditionBuilder()
                    .DeclareCondition(nameof(Entities.TimeMeasurement.IsValid), QueryConditionType.Equals, true)
            };

            var measurementCondition = new QueryConditionBuilder()
                .DeclareConditionFromBuilderSet(QueryConditionNodeType.And, measurementConditionBuilders)
                .Build();

            var timeMeasurementEntSet = await timeMeasurementDao.GetAllConditionalAsync(measurementCondition)
                .ConfigureAwait(false);

            var timeMeasurementEntSetCount = timeMeasurementEntSet.Count();
            if (timeMeasurementEntSetCount > 1)
                throw new InvalidOperationException(
                    $"Multiple VALID Timemeasurements found for the same raceDataId '{raceDataId}' " +
                    $"and sensorId '{sensorId}' -> atleast none or exactly one measurements should exist.");

            return timeMeasurementEntSetCount == 0
                ? null
                : new Domain.TimeMeasurement
                {
                    Id = timeMeasurementEntSet.First().Id,
                    Measurement = timeMeasurementEntSet.First().Measurement,
                    SensorId = timeMeasurementEntSet.First().SensorId
                };
        }

        #endregion
        #region Venue-Methods

        public async Task<IEnumerable<Domain.Venue>> GetAllVenuesAsync(
            Domain.Associated<Domain.Country>.LoadingType countryLoadingType = Domain.Associated<Domain.Country>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Season>.LoadingType seasonsOfVenueLoadingType = Domain.Associated<Domain.Season>.LoadingType.None)
        {
            var venueEntities = await venueDao.GetAllConditionalAsync().ConfigureAwait(false);
            var seasonPlanEntities = await seasonPlanDao.GetAllConditionalAsync().ConfigureAwait(false);
            var countryEntities = await countryDao.GetAllConditionalAsync().ConfigureAwait(false);

            return await Task.WhenAll(
                    venueEntities.Select(
                        async venueEntity => new Domain.Venue
                        {
                            Id = venueEntity.Id,
                            Name = venueEntity.Name,
                            Country = await LoadAssociatedDomainObject(
                                    countryLoadingType,
                                    async () => new Domain.Associated<Domain.Country>(
                                        (await GetAllCountriesAsync().ConfigureAwait(false))
                                            .First(c => c.Id == venueEntity.CountryId)),
                                    () => new Domain.Associated<Domain.Country>(venueEntity.CountryId))
                                .ConfigureAwait(false),
                            Seasons = await LoadAssociatedDomainObjectSet(
                                    seasonsOfVenueLoadingType,
                                    async () =>
                                        (await GetAllSeasonsAsync().ConfigureAwait(false))
                                            .Where(s => seasonPlanEntities.Any(sp => sp.SeasonId == s.Id &&
                                                                               sp.VenueId == venueEntity.Id))
                                            .Select(s => new Domain.Associated<Domain.Season>(s)),
                                    async () =>
                                        (await GetAllSeasonsAsync().ConfigureAwait(false))
                                            .Where(s => seasonPlanEntities.Any(sp => sp.SeasonId == s.Id &&
                                                                               sp.VenueId == venueEntity.Id))
                                            .Select(s => new Domain.Associated<Domain.Season>(s.Id)))
                                .ConfigureAwait(false)
                        }))
                .ConfigureAwait(false);
        }

        public async Task<Domain.Venue> GetVenueByIdAsync(
            int id,
            Domain.Associated<Domain.Country>.LoadingType countryLoadingType = Domain.Associated<Domain.Country>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Season>.LoadingType seasonsOfVenueLoadingType = Domain.Associated<Domain.Season>.LoadingType.None)
        {
            var venueEntity = await venueDao.GetByIdAsync(id).ConfigureAwait(false);

            return new Domain.Venue
            {
                Id = venueEntity.Id,
                Name = venueEntity.Name,
                Country = await LoadAssociatedDomainObject(
                        countryLoadingType,
                        async () => new Domain.Associated<Domain.Country>(
                            await GetCountryByIdAsync(venueEntity.CountryId)
                                .ConfigureAwait(false)),
                        () => new Domain.Associated<Domain.Country>(venueEntity.CountryId))
                    .ConfigureAwait(false),
                Seasons = await LoadAssociatedDomainObjectSet(
                        seasonsOfVenueLoadingType,
                        async () =>
                            (await GetAllSeasonsByVenueIdAsync(venueEntity.Id).ConfigureAwait(false))
                                .Select(s => new Domain.Associated<Domain.Season>(s.Id)),
                        async () =>
                            (await GetAllSeasonsByVenueIdAsync(venueEntity.Id).ConfigureAwait(false))
                                .Select(s => new Domain.Associated<Domain.Season>(s)))
                    .ConfigureAwait(false)
            };
        }

        #endregion
        #region Helper

        private IEnumerable<string> FormatElapsedMeasurements(
            IEnumerable<TimeSpan> elapsedMeasurements,
            IEnumerable<TimeSpan> bestElapsedMeasurements,
            bool notFirstRankedSkier)
        {
            var formattedMeasurements = new List<string>();

            var elapsedMeasurementsList = elapsedMeasurements.ToList();
            var bestElapsedMeasurementsList = bestElapsedMeasurements.ToList();
            for (int i = 0; i < elapsedMeasurementsList.Count; i++)
            {
                if (!notFirstRankedSkier)
                {
                    if (elapsedMeasurementsList[i] != TimeSpan.MaxValue)
                        formattedMeasurements.Add(this.FormatTimeSpan(elapsedMeasurementsList[i], false));
                    else
                        formattedMeasurements.Add("n/a");
                }
                else
                {
                    if (elapsedMeasurementsList[i] != TimeSpan.MaxValue &&
                        bestElapsedMeasurementsList[i] != TimeSpan.MaxValue)
                    {
                        formattedMeasurements.Add(this.FormatTimeSpan(elapsedMeasurementsList[i], true));
                    }
                    else if (elapsedMeasurementsList[i] != TimeSpan.MaxValue &&
                             bestElapsedMeasurementsList[i] == TimeSpan.MaxValue)
                    {
                        formattedMeasurements.Add(this.FormatTimeSpan(elapsedMeasurementsList[i], false));
                    }
                    else
                    {
                        formattedMeasurements.Add("n/a");
                    }
                }
            }

            return formattedMeasurements;
        }

        private IEnumerable<TimeSpan> UpdateMeasurementsAccordingToBest(
            IEnumerable<TimeSpan> elapsedMeasurements,
            IEnumerable<TimeSpan> bestElapsedMeasurements)
        {
            var newElapsedMeasurements = elapsedMeasurements.ToList();
            var bestElapsedMeasurementsList = bestElapsedMeasurements.ToList();
            for (int i = 0; i < newElapsedMeasurements.Count; i++)
            {
                if (newElapsedMeasurements[i] != TimeSpan.MaxValue &&
                    bestElapsedMeasurementsList[i] != TimeSpan.MaxValue)
                {
                    newElapsedMeasurements[i] -= bestElapsedMeasurementsList[i];
                }
            }
            return newElapsedMeasurements;
        }

        private string FormatTimeSpan(TimeSpan elapsedTime, bool addSignPrefix)
        {
            if (elapsedTime == TimeSpan.MaxValue)
                return "/";

            var prefix = "  ";
            if (elapsedTime < TimeSpan.Zero)
                prefix = "- ";
            else if (elapsedTime > TimeSpan.Zero)
                prefix = "+ ";

            return $"{(addSignPrefix ? prefix : "")}{elapsedTime.ToString("mm\\:ss\\.ff")}";
        }

        private async Task<Domain.Associated<T>> LoadAssociatedDomainObject<T>(
            Domain.Associated<T>.LoadingType desiredLoadingType,
            Func<Task<Domain.Associated<T>>> loadDomainObjectAsReference,
            Func<Task<Domain.Associated<T>>> loadDomainObjectAsForeignKey)
            where T : Domain.DomainObjectBase
        {
            switch (desiredLoadingType)
            {
                case Domain.Associated<T>.LoadingType.None:
                    return null;
                case Domain.Associated<T>.LoadingType.ForeignKey:
                    if (loadDomainObjectAsForeignKey == null)
                    {
                        var associatedDomainObject =
                            await LoadAssociatedDomainObject(
                                Domain.Associated<T>.LoadingType.Reference,
                                loadDomainObjectAsReference)
                            .ConfigureAwait(false);

                        if (associatedDomainObject.Reference == null)
                            return associatedDomainObject;

                        var reference = associatedDomainObject.Reference;
                        associatedDomainObject.Reference = null;
                        associatedDomainObject.ForeignKey = reference.Id;
                        return associatedDomainObject;
                    }
                    else
                    {
                        return await loadDomainObjectAsForeignKey().ConfigureAwait(false);
                    }
                case Domain.Associated<T>.LoadingType.Reference:
                    if (loadDomainObjectAsReference == null)
                        throw new InvalidOperationException(
                            $"Can't load associated domain-object {typeof(T).Name} as reference because loader is null");

                    return await loadDomainObjectAsReference().ConfigureAwait(false);
                default:
                    throw new InvalidOperationException(
                        $"Unknown value {desiredLoadingType} of {typeof(Domain.Associated<T>.LoadingType).Name}");
            }
        }

        private async Task<Domain.Associated<T>> LoadAssociatedDomainObject<T>(
            Domain.Associated<T>.LoadingType desiredLoadingType,
            Func<Task<Domain.Associated<T>>> loadDomainObjectAsReference,
            Func<Domain.Associated<T>> loadDomainObjectAsForeignKey = null)
            where T : Domain.DomainObjectBase
        {
            switch (desiredLoadingType)
            {
                case Domain.Associated<T>.LoadingType.None:
                    return null;
                case Domain.Associated<T>.LoadingType.ForeignKey:
                    if (loadDomainObjectAsForeignKey == null)
                    {
                        var associatedDomainObject =
                            await LoadAssociatedDomainObject(
                                Domain.Associated<T>.LoadingType.Reference,
                                loadDomainObjectAsReference)
                            .ConfigureAwait(false);

                        if (associatedDomainObject.Reference == null)
                            return associatedDomainObject;

                        var reference = associatedDomainObject.Reference;
                        associatedDomainObject.Reference = null;
                        associatedDomainObject.ForeignKey = reference.Id;
                        return associatedDomainObject;
                    }
                    else
                    {
                        return loadDomainObjectAsForeignKey();
                    }
                case Domain.Associated<T>.LoadingType.Reference:
                    if (loadDomainObjectAsReference == null)
                        throw new InvalidOperationException(
                            $"Can't load associated domain-object {typeof(T).Name} as reference because loader is null");

                    return await loadDomainObjectAsReference().ConfigureAwait(false);
                default:
                    throw new InvalidOperationException(
                        $"Unknown value {desiredLoadingType} of {typeof(Domain.Associated<T>.LoadingType).Name}");
            }
        }

        private async Task<IEnumerable<Domain.Associated<T>>> LoadAssociatedDomainObjectSet<T>(
            Domain.Associated<T>.LoadingType desiredLoadingType,
            Func<Task<IEnumerable<Domain.Associated<T>>>> loadDomainObjectAsReference,
            Func<Task<IEnumerable<Domain.Associated<T>>>> loadDomainObjectAsForeignKey)
            where T : Domain.DomainObjectBase
        {
            switch (desiredLoadingType)
            {
                case Domain.Associated<T>.LoadingType.None:
                    return null;
                case Domain.Associated<T>.LoadingType.ForeignKey:
                    if (loadDomainObjectAsForeignKey == null)
                        throw new InvalidOperationException(
                            $"Can't load associated domain-object {typeof(T).Name} as foreign-key because loader is null");
                    return await loadDomainObjectAsForeignKey().ConfigureAwait(false);
                case Domain.Associated<T>.LoadingType.Reference:
                    if (loadDomainObjectAsReference == null)
                        throw new InvalidOperationException(
                            $"Can't load associated domain-object {typeof(T).Name} as reference because loader is null");
                    return await loadDomainObjectAsReference().ConfigureAwait(false);
                default:
                    throw new InvalidOperationException(
                        $"Unknown value {desiredLoadingType} of {typeof(Domain.Associated<T>.LoadingType).Name}");
            }
        }

        #endregion
        #endregion
    }
}
