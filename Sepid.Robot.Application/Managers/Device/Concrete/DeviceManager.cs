using Microsoft.EntityFrameworkCore;
using Sepid.Robot.Application.Managers.Device.Abstracts;
using Sepid.Robot.Application.Managers.Device.Model;
using Sepid.Robot.Domain.Context;
using Sepid.Robot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepid.Robot.Application.Managers.Device.Concrete
{
    public class DeviceManager : IDeviceManager
    {
        protected internal readonly IUnitOfWork UnitOfWork;
        public DeviceManager(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<List<Domain.Entities.Device>> GetListAsync()
        {
            var devices = await UnitOfWork.Set<Domain.Entities.Device, long>().GetAll().ToListAsync();
            return devices;
        }

        public async Task<(bool success, Domain.Entities.Device)> CreateAsync(CreateDeviceModel model)
        {
            bool success; Domain.Entities.Device result;
            using var transaction = UnitOfWork.BeginTransaction();
            try
            {
                var entityModel = model.MapToEntity();
                (success, result) = await UnitOfWork.Set<Domain.Entities.Device, long>().CreateAsync(entityModel);
                await UnitOfWork.SaveAsync();
                transaction.Commit();
                return (success, result);
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
