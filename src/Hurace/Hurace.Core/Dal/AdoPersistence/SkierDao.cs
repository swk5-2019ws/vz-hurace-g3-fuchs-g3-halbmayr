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

        private SimpleSqlQueryGenerator<Skier> SqlQueryGenerator { get; }
        private RowMapper<Skier> RowMapper { get; }

        public SkierDao(IConnectionFactory connectionFactory)
        {
            template = new AdoTemplate(connectionFactory);

            RowMapper = new RowMapper<Skier>();
            SqlQueryGenerator = new SimpleSqlQueryGenerator<Skier>(new List<string>());
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
                SqlQueryGenerator.GenerateGetAllQuery(),
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
