using Microsoft.Extensions.Configuration;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Hurace.Core.Db.Connection
{
    /// <summary>
    /// Implementation of <see cref="IConnectionFactory"/> used for production
    /// </summary>
    public class DefaultConnectionFactory : IConnectionFactory
    {
        /// <summary>
        /// Get a new instance of <see cref="DefaultConnectionFactory"/>.
        /// This constructor assumes, that a AppSettings.json file exists in the build folder
        /// and that there exists a section with ConnectionString and ProviderName set as properties.
        /// </summary>
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

        /// <summary>
        /// Allocate a new <see cref="DbConnection"/> with the declared <see cref="DbProviderFactory"/>
        /// </summary>
        /// <returns>the newly created <see cref="DbConnection"/></returns>
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
                _ => throw new ArgumentOutOfRangeException($"Invalid {nameof(ProviderName)} \"{ProviderName}\""),
            };
        }
    }
}
