using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepid.Robot.Application.Configure
{
    public class Config
    {
        public IServiceCollection Configure(IServiceCollection services)
        {
            DiConfigure.ConfigureDependencyInjection(services);
            return services;
        }
    }
}
