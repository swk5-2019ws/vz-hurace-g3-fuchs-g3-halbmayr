using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.BL;
using Hurace.Core.DAL;
using Hurace.Core.DAL.AdoPersistence;
using Hurace.Core.Db.Connection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#pragma warning disable CA1822 // Mark members as static
namespace Hurace.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

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

            services.AddScoped<IRaceManager, RaceManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
