using Microsoft.Extensions.Configuration;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Hurace.Core.Db.Connection
{
    public class DefaultConnectionFactory : IConnectionFactory
    {
        public DefaultConnectionFactory()
        {
            var appSettingConfig = new ConfigurationBuilder()
                           .AddJsonFile("AppSettings.json", optional: false)
                           .Build();

            var connectionStringConfigSection = appSettingConfig
                .GetSection("HuraceDbConnectionString");

            ConnectionString = connectionStringConfigSection["ConnectionString"];
            ProviderName = connectionStringConfigSection["ProviderName"];
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
            return ProviderName switch
            {
                "Microsoft.Data.SqlClient" => Microsoft.Data.SqlClient.SqlClientFactory.Instance,
                //case "MySql.Data.MySqlClient":
                //    return MySql.Data.MySqlClient.MySqlClientFactory.Instance;
                _ => throw new ArgumentOutOfRangeException($"Invalid {nameof(ProviderName)} \"{ProviderName}\""),
            };
        }
    }
}
