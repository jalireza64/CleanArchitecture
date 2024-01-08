using Sepid.Robot.Application.Managers.Device.Model;
using Sepid.Robot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepid.Robot.Application.Managers.Device.Abstracts
{
    public interface IDeviceManager
    {
        Task<List<Domain.Entities.Device>> GetListAsync();

        Task<(bool success, Domain.Entities.Device)> CreateAsync(CreateDeviceModel model);
    }
}
