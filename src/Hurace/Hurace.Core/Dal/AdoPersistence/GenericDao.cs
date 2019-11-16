using Hurace.Core.Db;
using Hurace.Core.Db.Connection;
using Hurace.Core.Db.Utilities;
using Hurace.Domain;
using Microsoft.Data.SqlClient;
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
            (string createQuery, QueryParameter[] parameters) = SqlQueryGenerator.GenerateCreateQuery(newInstance);
            await template.ExecuteAsync(createQuery, parameters);

            string getLastGivenIdentityQuery = SqlQueryGenerator.GenerateGetLastIdentityQuery();
            int id = await template.QuerySingleIntAsync(getLastGivenIdentityQuery);

            return await this.GetByIdAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await template.QueryObjectSetAsync(
                SqlQueryGenerator.GenerateGetAllQuery(),
                RowMapper);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            (string query, QueryParameter[] parameters) = SqlQueryGenerator.GenerateGetByIdQuery(id);
            return await template.QuerySingleObjectAsync(query, RowMapper, parameters);
        }

        public async Task<bool> UpdateAsync(T updatedInstance)
        {
            (string query, QueryParameter[] parameters) = SqlQueryGenerator.GenerateUpdateQuery(updatedInstance);
            int affectedRows = await template.ExecuteAsync(query, parameters);
            return affectedRows == 1;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            (string query, QueryParameter[] parameters) = SqlQueryGenerator.GenerateDeleteByIdQuery(id);

            try
            {
                int numberOfAffectedRows = await template.ExecuteAsync(query, parameters);
                return numberOfAffectedRows == 1;
            }
            catch (SqlException)
            {
                return false;
            }
        }
    }
}
