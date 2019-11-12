using Hurace.Core.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hurace.Core.Dal.AdoPersistence
{
    internal class AdoTemplate
    {
        private readonly ConnectionFactory connectionFactory;

        public AdoTemplate(ConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(
            string sqlQuery,
            RowToEntityMapper<T> rowMapper,
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

        public async Task<T> QuerySingleAsync<T>(
            string sqlQuery,
            RowToEntityMapper<T> rowMapper,
            params QueryParameter[] queryParameters) where T : new()
        {
            return
                (await this.QueryAsync(sqlQuery, rowMapper, queryParameters))
                .SingleOrDefault();
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
