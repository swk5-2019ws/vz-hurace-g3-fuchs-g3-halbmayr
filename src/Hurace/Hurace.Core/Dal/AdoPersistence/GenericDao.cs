using Hurace.Core.Db;
using Hurace.Core.Db.Connection;
using Hurace.Core.Db.Queries;
using Hurace.Core.Db.Utilities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hurace.Core.Dal.AdoPersistence
{
    public class GenericDao<T> : IDataAccessObject<T> where T : Entities.EntityObjectBase, new()
    {
        private readonly AdoTemplate template;

        private SqlQueryGenerator<T> SqlQueryGenerator { get; }
        private RowMapper<T> RowMapper { get; }

        public GenericDao(IConnectionFactory connectionFactory)
        {
            if (connectionFactory is null)
                throw new ArgumentNullException(nameof(connectionFactory));

            template = new AdoTemplate(connectionFactory);

            RowMapper = new RowMapper<T>();
            SqlQueryGenerator = new SqlQueryGenerator<T>();
        }

        public async Task<int> CreateAsync(T newInstance)
        {
            if (newInstance is null)
                throw new ArgumentNullException(nameof(newInstance));

            (string createQuery, QueryParameter[] parameters) = SqlQueryGenerator.GenerateInsertQuery(newInstance);
            int affectedRowCount = await template.ExecuteAsync(createQuery, parameters);

            if (affectedRowCount != 1)
                throw new InvalidOperationException($"The INSERT Query affected {affectedRowCount} rows -> should only affect 1");

            string getLastGivenIdentityQuery = SqlQueryGenerator.GenerateGetLastIdentityQuery();
            return await template.QuerySingleInt32Async(getLastGivenIdentityQuery);
        }

        public async Task<IEnumerable<T>> GetAllConditionalAsync(IQueryCondition condition = null)
        {
            (var query, var queryParameters) = SqlQueryGenerator.GenerateSelectQuery(condition);
            return await template.QueryObjectSetAsync(
                query,
                RowMapper,
                queryParameters);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            (string query, QueryParameter[] parameters) = SqlQueryGenerator.GenerateSelectQuery(id);

            return await template.QuerySingleObjectAsync(query, RowMapper, parameters);
        }

        public async Task<bool> UpdateAsync(T updatedInstance)
        {
            if (updatedInstance is null)
                throw new ArgumentNullException(nameof(updatedInstance));

            (string query, QueryParameter[] parameters) = SqlQueryGenerator.GenerateUpdateQuery(updatedInstance);
            int affectedRows = await template.ExecuteAsync(query, parameters);
            return affectedRows == 1;
        }

        public async Task<int> UpdateAsync(object updatedValues, IQueryCondition condition)
        {
            if (updatedValues is null)
                throw new ArgumentNullException(nameof(updatedValues));
            else if (condition is null)
                throw new ArgumentNullException(nameof(condition));
            else if (updatedValues.GetType().GetProperties().Length == 0)
                throw new InvalidOperationException(
                    $"Passed {nameof(updatedValues)} anonymous object does not contain any updated values");

            (string query, QueryParameter[] parameters) = SqlQueryGenerator.GenerateUpdateQuery(updatedValues, condition);
            return await template.ExecuteAsync(query, parameters);
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            (string query, QueryParameter[] parameters) = SqlQueryGenerator.GenerateDeleteQuery(id);

            int numberOfAffectedRows = await template.ExecuteAsync(query, parameters);
            return numberOfAffectedRows == 1;
        }

        public async Task<int> DeleteAsync(IQueryCondition condition)
        {
            if (condition is null)
                throw new ArgumentNullException(nameof(condition));

            (string query, QueryParameter[] parameters) = SqlQueryGenerator.GenerateDeleteQuery(condition);

            return await template.ExecuteAsync(query, parameters);
        }
    }
}
