using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sepid.Robot.Domain.Context;
using Sepid.Robot.Domain.Interfaces;
using Sepid.Robot.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepid.Robot.Data.Access.Configure
{
    public class Config
    {
        public IServiceCollection Configure(IServiceCollection services)
        {
            ConfigureDbContext(services);
            ConfigureDependencyInjection(services);

            return services;
        }

        #region DependencyInjection

        private static void ConfigureDependencyInjection(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddScoped<ISepidRobotContext, SepidRobotContext>();

            //services.AddScoped<DatabaseBootstrapper>();

        }

        #endregion DependencyInjection


        #region DbContext

        public static readonly LoggerFactory MyLoggerFactory =
            new LoggerFactory();

        public static readonly InMemoryDatabaseRoot InMemoryDatabaseRoot = new InMemoryDatabaseRoot();

        public void ConfigureDbContext(IServiceCollection services)
        {
            
                services.AddDbContext<SepidRobotContext>(options =>
                {
                    options.UseSqlServer("DefaultConnection");
                    options.UseLoggerFactory(MyLoggerFactory);
                });
        }

        #endregion DbContext
    }
}
