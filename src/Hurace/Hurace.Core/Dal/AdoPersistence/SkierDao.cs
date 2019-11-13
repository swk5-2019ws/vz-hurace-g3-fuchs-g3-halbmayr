using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Hurace.Core.Db;
using Hurace.Core.Db.Connection;
using Hurace.Core.Db.Utilities;
using Hurace.Domain;

namespace Hurace.Core.Dal.AdoPersistence
{
    public class SkierDao : IDataAccessObject<Skier>
    {
        private readonly AdoTemplate template;

        private SimpleSqlQueryGenerator SqlQueryGenerator { get; }
        private RowMapper<Skier> RowMapper { get; }

        public SkierDao(IConnectionFactory connectionFactory)
        {
            template = new AdoTemplate(connectionFactory);

            var notQueryAbleProperties = new List<string>()
            {
                nameof(Skier.RaceDataIds),
                nameof(Skier.StartPositionIds)
            };

            RowMapper = new RowMapper<Skier>(notQueryAbleProperties);
            SqlQueryGenerator = new SimpleSqlQueryGenerator(notQueryAbleProperties);
        }

        public Task<Skier> CreateAsync(Skier newInstance)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Skier>> GetAllAsync()
        {
            return await template.QueryAsync(
                SqlQueryGenerator.GenerateGetAllQuery<Skier>(),
                RowMapper);
        }

        public Task<Skier> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Skier updatedInstance)
        {
            throw new NotImplementedException();
        }
    }
}
