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

        public RaceManager(
            DomainObjectMapper domainObjectMapper,
            IDataAccessObject<Entities.Race> raceDao)
        {
            this.domainObjectMapper = domainObjectMapper ?? throw new ArgumentNullException(nameof(domainObjectMapper));
            this.raceDao = raceDao ?? throw new ArgumentNullException(nameof(raceDao));
        }

        public async Task<IEnumerable<Domain.Race>> GetAllRaces()
        {
            var entityRaces = await raceDao.GetAllConditionalAsync();

            return await Task.WhenAll(
                entityRaces.Select(raceEntity => domainObjectMapper.RaceGenerator(raceEntity)));
        }
    }
}
