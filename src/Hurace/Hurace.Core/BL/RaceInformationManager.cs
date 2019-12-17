using Hurace.Core.DAL;
using Hurace.Core.Db.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hurace.Core.BL
{
    public class RaceInformationManager : BaseInformationManager, IRaceInformationManager
    {
        #region Fields

        private readonly IDataAccessObject<Entities.Country> countryDao;
        private readonly IDataAccessObject<Entities.Race> raceDao;
        private readonly IDataAccessObject<Entities.RaceType> raceTypeDao;
        private readonly IDataAccessObject<Entities.Venue> venueDao;
        private readonly IDataAccessObject<Entities.SeasonPlan> seasonPlanDao;
        private readonly IDataAccessObject<Entities.Season> seasonDao;

        #endregion
        #region Constructors

        public RaceInformationManager(
            IDataAccessObject<Entities.Country> countryDao,
            IDataAccessObject<Entities.Race> raceDao,
            IDataAccessObject<Entities.RaceType> raceTypeDao,
            IDataAccessObject<Entities.Season> seasonDao,
            IDataAccessObject<Entities.SeasonPlan> seasonPlanDao,
            IDataAccessObject<Entities.Venue> venueDao)
        {
            this.countryDao = countryDao ?? throw new ArgumentNullException(nameof(countryDao));
            this.raceDao = raceDao ?? throw new ArgumentNullException(nameof(raceDao));
            this.raceTypeDao = raceTypeDao ?? throw new ArgumentNullException(nameof(raceTypeDao));
            this.venueDao = venueDao ?? throw new ArgumentNullException(nameof(venueDao));
            this.seasonPlanDao = seasonPlanDao ?? throw new ArgumentNullException(nameof(seasonPlanDao));
            this.seasonDao = seasonDao ?? throw new ArgumentNullException(nameof(seasonDao));
        }

        #endregion
        #region Methods
        #region Country-Methods

        public async Task<IEnumerable<Domain.Country>> GetAllCountries()
        {
            return (await countryDao.GetAllConditionalAsync())
                .Select(countryEntitiy => new Domain.Country
                {
                    Id = countryEntitiy.Id,
                    Name = countryEntitiy.Name
                });
        }

        public async Task<Domain.Country> GetCountryById(int id)
        {
            var countryEntity = await countryDao.GetByIdAsync(id);

            return new Domain.Country
            {
                Id = countryEntity.Id,
                Name = countryEntity.Name
            };
        }

        #endregion
        #region Race-Methods

        public async Task<IEnumerable<Domain.Race>> GetAllRacesAsync(
            Domain.Associated<Domain.RaceType>.LoadingType raceTypeLoadingType = Domain.Associated<Domain.RaceType>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Venue>.LoadingType venueLoadingType = Domain.Associated<Domain.Venue>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Season>.LoadingType seasonLoadingType = Domain.Associated<Domain.Season>.LoadingType.None)
        {
            var raceEntities = await raceDao.GetAllConditionalAsync();

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
                                await this.GetRaceTypeById(raceEntity.RaceTypeId)),
                            () => new Domain.Associated<Domain.RaceType>(raceEntity.RaceTypeId)),
                        Venue = await LoadAssociatedDomainObject(
                            venueLoadingType,
                            async () => new Domain.Associated<Domain.Venue>(
                                await this.GetVenueById(
                                    raceEntity.VenueId,
                                    seasonsOfVenueLoadingType: Domain.Associated<Domain.Season>.LoadingType.None)),
                            () => new Domain.Associated<Domain.Venue>(raceEntity.VenueId)),
                        Season = await LoadAssociatedDomainObject(
                            seasonLoadingType,
                            async () => new Domain.Associated<Domain.Season>(await GetSeasonByDate(raceEntity.Date))),
                        FirstStartList = null,
                        SecondStartList = null
                    }));
        }

        public async Task<Domain.Race> GetRaceById(
            int raceId,
            Domain.Associated<Domain.RaceType>.LoadingType raceTypeLoadingType = Domain.Associated<Domain.RaceType>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Venue>.LoadingType venueLoadingType = Domain.Associated<Domain.Venue>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Season>.LoadingType seasonsOfVenueLoadingType = Domain.Associated<Domain.Season>.LoadingType.None,
            Domain.Associated<Domain.StartPosition>.LoadingType startListLoadingType = Domain.Associated<Domain.StartPosition>.LoadingType.None)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region RaceType-Methods

        public async Task<IEnumerable<Domain.RaceType>> GetAllRaceTypesAsync()
        {
            return (await raceTypeDao.GetAllConditionalAsync())
                .Select(raceTypeEntity => new Domain.RaceType
                {
                    Id = raceTypeEntity.Id,
                    Label = raceTypeEntity.Label
                }); ;
        }

        public async Task<Domain.RaceType> GetRaceTypeById(int id)
        {
            var raceTypeEntity = await raceTypeDao.GetByIdAsync(id);

            return new Domain.RaceType
            {
                Id = raceTypeEntity.Id,
                Label = raceTypeEntity.Label
            };
        }

        #endregion
        #region Season-Methods

        public async Task<IEnumerable<Domain.Season>> GetAllSeasons()
        {
            return (await seasonDao.GetAllConditionalAsync())
                .Select(seasonEntity => new Domain.Season
                {
                    Id = seasonEntity.Id,
                    Name = seasonEntity.Name,
                    StartDate = seasonEntity.StartDate,
                    EndDate = seasonEntity.EndDate
                });
        }

        public async Task<Domain.Season> GetSeasonByDate(DateTime date)
        {
            var seasonCondition = new QueryConditionBuilder()
                .DeclareConditionNode(
                    QueryConditionNodeType.And,
                    () => new QueryConditionBuilder()
                        .DeclareCondition(nameof(Entities.Season.StartDate), QueryConditionType.LessThanOrEquals, date),
                    () => new QueryConditionBuilder()
                        .DeclareCondition(nameof(Entities.Season.EndDate), QueryConditionType.GreaterThanOrEquals, date))
                .Build();

            return (await seasonDao.GetAllConditionalAsync(seasonCondition))
                .Select(seasonEntity => new Domain.Season
                {
                    Id = seasonEntity.Id,
                    Name = seasonEntity.Name,
                    StartDate = seasonEntity.StartDate,
                    EndDate = seasonEntity.EndDate
                })
                .First();
        }

        public async Task<IEnumerable<Domain.Season>> GetAllSeasonsByVenueId(int venueId)
        {
            var seasonPlanCondition = new QueryConditionBuilder()
                   .DeclareCondition(nameof(Entities.SeasonPlan.VenueId), QueryConditionType.Equals, venueId)
                   .Build();
            var seasonPlanEntities = await seasonPlanDao.GetAllConditionalAsync(seasonPlanCondition);

            return (await Task.WhenAll(
                    seasonPlanEntities.Select(async sp => await seasonDao.GetByIdAsync(sp.SeasonId))))
                .Select(seasonEntity => new Domain.Season
                {
                    Id = seasonEntity.Id,
                    Name = seasonEntity.Name,
                    StartDate = seasonEntity.StartDate,
                    EndDate = seasonEntity.EndDate
                });
        }

        #endregion
        #region Venue-Methods

        public async Task<IEnumerable<Domain.Venue>> GetAllVenuesAsync(
            Domain.Associated<Domain.Country>.LoadingType countryLoadingType = Domain.Associated<Domain.Country>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Season>.LoadingType seasonsOfVenueLoadingType = Domain.Associated<Domain.Season>.LoadingType.None)
        {
            var venueEntities = await venueDao.GetAllConditionalAsync();
            var seasonPlanEntities = await seasonPlanDao.GetAllConditionalAsync();
            var countryEntities = await countryDao.GetAllConditionalAsync();

            return await Task.WhenAll(
                venueEntities.Select(
                    async venueEntity => new Domain.Venue
                    {
                        Id = venueEntity.Id,
                        Name = venueEntity.Name,
                        Country = await LoadAssociatedDomainObject(
                            countryLoadingType,
                            async () => new Domain.Associated<Domain.Country>(
                                (await GetAllCountries()).First(c => c.Id == venueEntity.CountryId)),
                            () => new Domain.Associated<Domain.Country>(venueEntity.CountryId)),
                        Seasons = await LoadAssociatedDomainObjectSet(
                            seasonsOfVenueLoadingType,
                            async () => (await GetAllSeasons())
                                .Where(s => seasonPlanEntities.Any(sp => sp.SeasonId == s.Id &&
                                                                   sp.VenueId == venueEntity.Id))
                                .Select(s => new Domain.Associated<Domain.Season>(s)),
                            async () => (await GetAllSeasons())
                                .Where(s => seasonPlanEntities.Any(sp => sp.SeasonId == s.Id &&
                                                                   sp.VenueId == venueEntity.Id))
                                .Select(s => new Domain.Associated<Domain.Season>(s.Id)))
                    }));
        }

        public async Task<Domain.Venue> GetVenueById(
            int id,
            Domain.Associated<Domain.Country>.LoadingType countryLoadingType = Domain.Associated<Domain.Country>.LoadingType.ForeignKey,
            Domain.Associated<Domain.Season>.LoadingType seasonsOfVenueLoadingType = Domain.Associated<Domain.Season>.LoadingType.None)
        {
            var venueEntity = await venueDao.GetByIdAsync(id);

            return new Domain.Venue
            {
                Id = venueEntity.Id,
                Name = venueEntity.Name,
                Country = await LoadAssociatedDomainObject(
                    countryLoadingType,
                    async () => new Domain.Associated<Domain.Country>(
                        await GetCountryById(venueEntity.CountryId)),
                    () => new Domain.Associated<Domain.Country>(venueEntity.CountryId)),
                Seasons = await LoadAssociatedDomainObjectSet(
                    seasonsOfVenueLoadingType,
                    async () => (await GetAllSeasonsByVenueId(venueEntity.Id))
                        .Select(s => new Domain.Associated<Domain.Season>(s.Id)),
                    async () => (await GetAllSeasonsByVenueId(venueEntity.Id))
                        .Select(s => new Domain.Associated<Domain.Season>(s)))
            };
        }

        #endregion
        #endregion
    }
}
