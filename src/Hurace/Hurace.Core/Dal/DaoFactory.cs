using Hurace.Core.Db;
using Hurace.Domain;
using Microsoft.Extensions.Configuration;
using System;

namespace Hurace.Core.Dal
{
    public static class DaoFactory
    {
        private static ConnectionFactory _connectionFactory;
        private static ConnectionFactory ConnectionFactory
        {
            get
            {
                if (_connectionFactory == null)
                {
                    var appSettingConfig = new ConfigurationBuilder()
                           .AddJsonFile("AppSettings.json", optional: false)
                           .Build();

                    var connectionStringConfigSection = appSettingConfig
                        .GetSection("HuraceDbConnectionString");

                    _connectionFactory = new ConnectionFactory(
                        connectionStringConfigSection["ConnectionString"],
                        connectionStringConfigSection["ProviderName"]);
                }
                return _connectionFactory;
            }
        }

        private static IDataAccessObject<Skier> DepositedSkierDao { get; set; }

        public static void ApplyDaoSet(params object[] daoSet)
        {
            foreach (var currentDao in daoSet)
            {
                if (currentDao is IDataAccessObject<Skier>)
                {
                    DepositedSkierDao = currentDao as IDataAccessObject<Skier>;
                }
                else
                {
                    throw new ArgumentException($"Parameter {nameof(daoSet)} contains unrecognized dao implementation!");
                }
            }
        }

        public static IDataAccessObject<Skier> CreateSkierDao()
        {
            return DepositedSkierDao ?? new AdoPersistence.Dao.AdoSkierDao(ConnectionFactory);
        }
    }
}
