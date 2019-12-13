using Hurace.Core.DAL;
using Hurace.Core.Db.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hurace.Core.BL
{
    public class DomainObjectMapper
    {
        private readonly IDataAccessObject<Entities.Country> countryDao;
        private readonly IDataAccessObject<Entities.Race> raceDao;
        private readonly IDataAccessObject<Entities.RaceData> raceDataDao;
        private readonly IDataAccessObject<Entities.RaceState> raceStateDao;
        private readonly IDataAccessObject<Entities.RaceType> raceTypeDao;
        private readonly IDataAccessObject<Entities.Season> seasonDao;
        private readonly IDataAccessObject<Entities.SeasonPlan> seasonPlanDao;
        private readonly IDataAccessObject<Entities.Sex> sexDao;
        private readonly IDataAccessObject<Entities.Skier> skierDao;
        private readonly IDataAccessObject<Entities.StartList> startListDao;
        private readonly IDataAccessObject<Entities.StartPosition> startPositionDao;
        private readonly IDataAccessObject<Entities.TimeMeasurement> timemeasurementDao;
        private readonly IDataAccessObject<Entities.Venue> venueDao;

        private readonly Func<Type, Type, string> daoInitializedExceptionFormatter =
            (DaoT, DomainT) => $"{DaoT.Name} needed to generate instances of {DomainT.FullName}";

        public DomainObjectMapper(
            IDataAccessObject<Entities.Country> countryDao = null,
            IDataAccessObject<Entities.Race> raceDao = null,
            IDataAccessObject<Entities.RaceData> raceDataDao = null,
            IDataAccessObject<Entities.RaceState> raceStateDao = null,
            IDataAccessObject<Entities.RaceType> raceTypeDao = null,
            IDataAccessObject<Entities.Season> seasonDao = null,
            IDataAccessObject<Entities.SeasonPlan> seasonPlanDao = null,
            IDataAccessObject<Entities.Sex> sexDao = null,
            IDataAccessObject<Entities.Skier> skierDao = null,
            IDataAccessObject<Entities.StartList> startListDao = null,
            IDataAccessObject<Entities.StartPosition> startPositionDao = null,
            IDataAccessObject<Entities.TimeMeasurement> timemeasurementDao = null,
            IDataAccessObject<Entities.Venue> venueDao = null)
        {
            this.countryDao = countryDao;
            this.raceDao = raceDao;
            this.raceDataDao = raceDataDao;
            this.raceStateDao = raceStateDao;
            this.raceTypeDao = raceTypeDao;
            this.seasonDao = seasonDao;
            this.seasonPlanDao = seasonPlanDao;
            this.sexDao = sexDao;
            this.skierDao = skierDao;
            this.startListDao = startListDao;
            this.startPositionDao = startPositionDao;
            this.timemeasurementDao = timemeasurementDao;
            this.venueDao = venueDao;
        }

        public bool StartPositionListQueryable => startPositionDao != null && raceDao != null;
        public Func<int, Task<IEnumerable<Domain.StartPosition>>> StartPositionListGenerator =>
                async (startListId) =>
                {
                    if (startPositionDao is null)
                        throw new InvalidOperationException(
                            daoInitializedExceptionFormatter(
                                typeof(IDataAccessObject<Entities.StartPosition>),
                                typeof(Domain.StartPosition)));
                    if (raceDao is null)
                        throw new InvalidOperationException(
                            daoInitializedExceptionFormatter(
                                typeof(IDataAccessObject<Entities.Race>),
                                typeof(Domain.Race)));

                    var condition = new QueryConditionBuilder()
                        .DeclareCondition(nameof(Entities.StartPosition.StartListId), QueryConditionType.Equals, startListId)
                        .Build();

                    var startPositionEntities = await startPositionDao.GetAllConditionalAsync(condition);

                    var startPositions = new List<Domain.StartPosition>();
                    foreach (var startPositionEntity in startPositionEntities)
                    {
                        var raceCondition = new QueryConditionBuilder()
                            .DeclareConditionNode(
                                QueryConditionNodeType.Or,
                                () => new QueryConditionBuilder()
                                    .DeclareCondition(
                                        nameof(Entities.Race.FirstStartListId),
                                        QueryConditionType.Equals,
                                        startPositionEntity.StartListId),
                                () => new QueryConditionBuilder()
                                    .DeclareCondition(
                                        nameof(Entities.Race.SecondStartListId),
                                        QueryConditionType.Equals,
                                        startPositionEntity.StartListId))
                            .Build();

                        var raceEntity = (await this.raceDao.GetAllConditionalAsync(raceCondition))
                            .FirstOrDefault();

                        startPositions.Add(new Domain.StartPosition(
                            startPositionEntity.Id,
                            raceLoader: async () => await this.RaceGenerator(raceEntity.Id))
                        {
                            Position = startPositionEntity.Position
                        });
                    }

                    return startPositions;
                };

        public bool RaceQueryable => raceDao != null && raceTypeDao != null;
        public Func<int, Task<Domain.Race>> RaceGenerator =>
            async (raceId) =>
            {
                if (raceTypeDao is null)
                    throw new InvalidOperationException(
                        daoInitializedExceptionFormatter(
                            typeof(IDataAccessObject<Entities.RaceType>),
                            typeof(Domain.Race)));

                var raceEntity = await raceDao.GetByIdAsync(raceId);
                var raceTypes = await raceTypeDao.GetAllConditionalAsync();

                return new Domain.Race(
                    raceEntity.Id,
                    raceEntity.Date,
                    raceEntity.Description,
                    raceEntity.NumberOfSensors,
                    raceTypes.FirstOrDefault(rs => rs.Id == raceEntity.RaceTypeId).Label,
                    firstStartListLoader: async () => await StartPositionListGenerator(raceEntity.FirstStartListId),
                    secondStartListLoader: async () => await StartPositionListGenerator(raceEntity.SecondStartListId),
                    venueLoader: async () => await VenueGenerator(raceEntity.VenueId));
            };

        public bool SeasonQueryable => seasonDao != null && seasonPlanDao != null;
        public Func<int, Task<Domain.Season>> SeasonGenerator =>
            async (seasonId) =>
            {
                throw new NotImplementedException();
            };

        public bool VenueQueryable => venueDao != null && seasonPlanDao != null;
        public Func<int, Task<Domain.Venue>> VenueGenerator =>
            async (venueId) =>
            {
                if (venueDao is null)
                    throw new InvalidOperationException(
                        daoInitializedExceptionFormatter(
                            typeof(IDataAccessObject<Entities.Venue>),
                            typeof(Domain.Venue)));
                if (seasonPlanDao is null)
                    throw new InvalidOperationException(
                        daoInitializedExceptionFormatter(
                            typeof(IDataAccessObject<Entities.SeasonPlan>),
                            typeof(Domain.Venue)));

                var venueEntity = await venueDao.GetByIdAsync(venueId);

                var seasonPlanCondition = new QueryConditionBuilder()
                    .DeclareCondition(nameof(Entities.SeasonPlan.VenueId), QueryConditionType.Equals, venueId)
                    .Build();
                var seasonPlanTasks = (await seasonPlanDao.GetAllConditionalAsync(seasonPlanCondition))
                    .Select(sp => SeasonGenerator(sp.Id));

                return new Domain.Venue(
                    venueEntity.Id,
                    seasonLoader: async () => await Task.WhenAll(seasonPlanTasks))
                {
                    Name = venueEntity.Name
                };
            };
    }
}
