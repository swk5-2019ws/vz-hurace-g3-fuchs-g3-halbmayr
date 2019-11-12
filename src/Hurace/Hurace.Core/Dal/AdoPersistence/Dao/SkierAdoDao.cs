using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Hurace.Core.Db;
using Hurace.Domain;

namespace Hurace.Core.Dal.AdoPersistence.Dao
{
    internal class AdoSkierDao : IDataAccessObject<Skier>
    {
        private readonly AdoTemplate template;

        private SimpleSqlQueryGenerator SqlQueryGenerator { get; }
        private RowToEntityMapper<Skier> RowMapper { get; }

        internal AdoSkierDao(ConnectionFactory connectionFactory)
        {
            template = new AdoTemplate(connectionFactory);

            var notQueryAbleProperties = new List<string>()
            {
                nameof(Skier.RaceDataIds),
                nameof(Skier.StartPositionIds)
            };

            RowMapper = new RowToEntityMapper<Skier>(notQueryAbleProperties);
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
                SqlQueryGenerator.GenerateGetAllSelectQuery<Skier>(),
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
