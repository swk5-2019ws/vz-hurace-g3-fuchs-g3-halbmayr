using Hurace.Core.DAL;
using Hurace.Core.Db.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hurace.Core.BL
{
    public class RaceManager
    {
        private readonly DomainObjectMapper domainObjectMapper;
        private readonly IDataAccessObject<Entities.Race> raceDao;
        private readonly IDataAccessObject<Entities.RaceType> raceTypeDao;

        public RaceManager(
            DomainObjectMapper domainObjectMapper,
            IDataAccessObject<Entities.Race> raceDao,
            IDataAccessObject<Entities.RaceType> raceTypeDao)
        {
            this.domainObjectMapper = domainObjectMapper ?? throw new ArgumentNullException(nameof(domainObjectMapper));
            this.raceDao = raceDao ?? throw new ArgumentNullException(nameof(raceDao));
            this.raceTypeDao = raceTypeDao ?? throw new ArgumentNullException(nameof(raceTypeDao));
        }

        public async Task<IEnumerable<Domain.Race>> GetAllRacesAsync()
        {
            var entityRaces = await raceDao.GetAllConditionalAsync();

            return await Task.WhenAll(
                entityRaces.Select(raceEntity => domainObjectMapper.RaceGenerator(raceEntity.Id)));
        }

        public async Task<bool> UpdateRaceAsync(Domain.Race race)
        {
            if (race is null)
                throw new ArgumentNullException(nameof(race));

            bool updateSuccess = true;

            if (domainObjectMapper.VenueQueryable && domainObjectMapper.SeasonQueryable)
            {
                foreach (var season in await (await race.Venue).Seasons)
                {
                    if (season.PropertiesChanged)
                    {
                        //todo update season if properties changed
                        throw new NotImplementedException();
                    }
                }
            }

            if (domainObjectMapper.VenueQueryable && (await race.Venue).PropertiesChanged)
            {
                //update venue if properties changed
                throw new NotImplementedException();
            }

            if (domainObjectMapper.StartPositionListQueryable)
            {
                var startPositions = new List<Domain.StartPosition>();
                startPositions.AddRange(await race.FirstStartList);
                startPositions.AddRange(await race.SecondStartList);

                foreach (var startPosition in startPositions)
                {
                    if (startPosition.PropertiesChanged)
                    {
                        //update first and second startlist if properties changed
                        throw new NotImplementedException();
                    }
                }
            }

            if (race.PropertiesChanged)
            {
                var raceTypeCondition = new QueryConditionBuilder()
                    .DeclareCondition(nameof(Entities.RaceType.Label), QueryConditionType.Equals, race.RaceType)
                    .Build();

                var alteredRaceTypeId = (await raceTypeDao.GetAllConditionalAsync(raceTypeCondition))
                    .First()
                    .Id;

                var updatedRaceEntity = new Entities.Race()
                {
                    Date = race.Date,
                    Description = race.Description,
                    NumberOfSensors = race.NumberOfSensors,
                    RaceTypeId = alteredRaceTypeId
                };

                updateSuccess = updateSuccess && await raceDao.UpdateAsync(updatedRaceEntity);
            }

            return updateSuccess;
        }
    }
}
