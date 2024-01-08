using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepid.Robot.Application.Configure
{
    public class DiConfigure
    {
        public static void ConfigureDependencyInjection(IServiceCollection services)
        {
            Managers.Device.Concrete.DiConfig.ConfigureDependencyInjection(services);
        }
    }
}
