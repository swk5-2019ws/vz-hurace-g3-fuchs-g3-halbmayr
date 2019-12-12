using Hurace.Core.Db.Connection;
using Hurace.Core.Db.Extensions;
using Hurace.Core.Db.Queries;
using Hurace.Core.Db.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
namespace Hurace.Core.Dal.AdoPersistence
{
    internal class AdoTemplate
    {
        private readonly IConnectionFactory connectionFactory;

        public AdoTemplate(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<IEnumerable<T>> QueryObjectSetAsync<T>(
            string sqlQuery,
            RowMapper<T> rowMapper,
            params QueryParameter[] queryParameters) where T : new()
        {
            using var dbConnection = await connectionFactory.CreateConnectionAsync();
            using var dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandText = sqlQuery;
            dbCommand.AddParameters(queryParameters);

            var resultItems = new List<T>();
            using (var dbDataReader = await dbCommand.ExecuteReaderAsync())
            {
                while (await dbDataReader.ReadAsync())
                {
                    resultItems.Add(rowMapper.Map(dbDataReader));
                }
            }

            return resultItems;
        }

        public async Task<T> QuerySingleObjectAsync<T>(
            string sqlQuery,
            RowMapper<T> rowMapper,
            params QueryParameter[] queryParameters) where T : new()
        {
            return
                (await this.QueryObjectSetAsync(sqlQuery, rowMapper, queryParameters))
                .SingleOrDefault();
        }

        public async Task<int> QuerySingleInt32Async(
            string sqlQuery,
            params QueryParameter[] queryParameters)
        {
            using var dbConnection = await connectionFactory.CreateConnectionAsync();
            using var dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandText = sqlQuery;
            dbCommand.AddParameters(queryParameters);

            using var dbDataReader = await dbCommand.ExecuteReaderAsync();

            await dbDataReader.ReadAsync();
            return Convert.ToInt32(dbDataReader.GetValue(0));
        }

        public async Task<int> ExecuteAsync(
            string sqlQuery,
            params QueryParameter[] queryParameters)
        {
            using var dbConnection = await connectionFactory.CreateConnectionAsync();
            using var dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandText = sqlQuery;
            dbCommand.AddParameters(queryParameters);

            return await dbCommand.ExecuteNonQueryAsync();
        }
    }
}
