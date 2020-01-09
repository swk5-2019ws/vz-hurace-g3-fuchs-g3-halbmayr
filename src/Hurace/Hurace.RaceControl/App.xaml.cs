using Hurace.Core.BL;
using Hurace.Core.DAL;
using Hurace.Core.DAL.AdoPersistence;
using Hurace.Core.Db.Connection;
using Hurace.Simulator;
using Hurace.Timer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace Hurace.RaceControl
{
    public delegate IRaceClock RaceClockResolver(bool useDevelopmentImplementation);

    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var raceControlWindow = ServiceProvider.GetRequiredService<MainWindow>();
            raceControlWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConnectionFactory, DefaultConnectionFactory>();

            services.AddScoped<IDataAccessObject<Entities.Country>, GenericDao<Entities.Country>>();
            services.AddScoped<IDataAccessObject<Entities.Race>, GenericDao<Entities.Race>>();
            services.AddScoped<IDataAccessObject<Entities.RaceData>, GenericDao<Entities.RaceData>>();
            services.AddScoped<IDataAccessObject<Entities.RaceState>, GenericDao<Entities.RaceState>>();
            services.AddScoped<IDataAccessObject<Entities.RaceType>, GenericDao<Entities.RaceType>>();
            services.AddScoped<IDataAccessObject<Entities.Season>, GenericDao<Entities.Season>>();
            services.AddScoped<IDataAccessObject<Entities.SeasonPlan>, GenericDao<Entities.SeasonPlan>>();
            services.AddScoped<IDataAccessObject<Entities.Sex>, GenericDao<Entities.Sex>>();
            services.AddScoped<IDataAccessObject<Entities.Skier>, GenericDao<Entities.Skier>>();
            services.AddScoped<IDataAccessObject<Entities.StartList>, GenericDao<Entities.StartList>>();
            services.AddScoped<IDataAccessObject<Entities.StartPosition>, GenericDao<Entities.StartPosition>>();
            services.AddScoped<IDataAccessObject<Entities.TimeMeasurement>, GenericDao<Entities.TimeMeasurement>>();
            services.AddScoped<IDataAccessObject<Entities.Venue>, GenericDao<Entities.Venue>>();

            services.AddSingleton<RaceClockSimulation>();
            //services.AddSingleton<(real implementation)>();
            services.AddTransient<RaceClockResolver>(
                serviceProvider =>
                    useDevelopmentImplementation =>
                    {
                        return useDevelopmentImplementation
                            ? serviceProvider.GetService<RaceClockSimulation>()
                            : /*serviceProvider.GetService<(real implementation)>()*/ null;
                    });

            services.AddScoped<IInformationManager, InformationManager>();
            services.AddScoped<IRaceExecutionManager, RaceExecutionManager>();

            services.AddTransient<ViewModels.RaceDetailViewModel>();
            services.AddSingleton<ViewModels.MainViewModel>();

            services.AddTransient<MainWindow>();
        }
    }
}
