using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Hurace.Core.Db
{
    public class ConnectionFactory
    {
        public ConnectionFactory(string connectionString, string providerName)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            ProviderName = providerName ?? throw new ArgumentNullException(nameof(providerName));
        }

        private string ConnectionString { get; }
        private string ProviderName { get; }

        public async Task<DbConnection> CreateConnectionAsync()
        {
            var dbConnection = this.GetDbProviderFactory().CreateConnection();
            dbConnection.ConnectionString = ConnectionString;

            await dbConnection.OpenAsync();

            return dbConnection;
        }

        private DbProviderFactory GetDbProviderFactory()
        {
            switch (ProviderName)
            {
                case "Microsoft.Data.SqlClient":
                    return Microsoft.Data.SqlClient.SqlClientFactory.Instance;
                //case "MySql.Data.MySqlClient":
                //    return MySql.Data.MySqlClient.MySqlClientFactory.Instance;
                default:
                    throw new ArgumentOutOfRangeException($"Invalid {nameof(ProviderName)} \"{ProviderName}\"");
            }
        }
    }
}
