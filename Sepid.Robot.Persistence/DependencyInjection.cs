using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sepid.Robot.Domain.Interfaces;
using Sepid.Robot.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepid.Robot.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SepidRobotContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SepidRobotContext"));
                options.EnableSensitiveDataLogging();
            });

            services.AddScoped<ISepidRobotContext>(provider => provider.GetService<SepidRobotContext>());

            return services;
        }
    }
}
