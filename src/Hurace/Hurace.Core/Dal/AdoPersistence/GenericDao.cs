using Hurace.Core.Db;
using Hurace.Core.Db.Connection;
using Hurace.Core.Db.Utilities;
using Hurace.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hurace.Core.Dal.AdoPersistence
{
    public class GenericDao<T> : IDataAccessObject<T> where T : DomainObjectBase, new()
    {
        private readonly AdoTemplate template;

        private SimpleSqlQueryGenerator<T> SqlQueryGenerator { get; }
        private RowMapper<T> RowMapper { get; }

        public GenericDao(IConnectionFactory connectionFactory)
        {
            template = new AdoTemplate(connectionFactory);

            RowMapper = new RowMapper<T>();
            SqlQueryGenerator = new SimpleSqlQueryGenerator<T>();
        }

        public async Task<T> CreateAsync(T newInstance)
        {
            (string query, QueryParameter[] parameters) = SqlQueryGenerator.GenerateCreateQuery(newInstance);
            int id = await template.ExecuteAsync(query, parameters);
            return await this.GetByIdAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await template.QueryAsync(
                SqlQueryGenerator.GenerateGetAllQuery(),
                RowMapper);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            (string query, QueryParameter[] parameters) = SqlQueryGenerator.GenerateGetByIdQuery(id);
            return await template.QuerySingleAsync(query, RowMapper, parameters);
        }

        public Task<bool> DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(T updatedInstance)
        {
            throw new NotImplementedException();
        }
    }
}
