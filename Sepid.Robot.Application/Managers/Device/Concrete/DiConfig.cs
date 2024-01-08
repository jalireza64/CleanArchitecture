using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sepid.Robot.Application.Managers.Device.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepid.Robot.Application.Managers.Device.Concrete
{
    public class DiConfig
    {
        public static IServiceCollection ConfigureDependencyInjection(IServiceCollection services)
        {
            services.AddScoped<IDeviceManager, DeviceManager>();

            return services;
        }
    }

}
