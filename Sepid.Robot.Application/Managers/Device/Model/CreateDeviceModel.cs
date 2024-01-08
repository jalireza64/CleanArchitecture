using Sepid.Robot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sepid.Robot.Application.Managers.Device.Model
{
    public class CreateDeviceModel
    {
        public CreateDeviceModel(string deviceId,
            string deviceSecret, string name,
            string region,string description)
        {

            Description = description;
            DeviceId = deviceId;
            DeviceSecret = deviceSecret;
            Name = name;
            Region = region;
        }

        public string Name { get; set; }
        public string DeviceId { get; set; }
        public string DeviceSecret { get; set; }
        public string Region { get; set; }
        public string Description { get; set; }


        public Domain.Entities.Device MapToEntity()
        {
            return new Domain.Entities.Device()
            {
                Description = this.Description,
                DeviceId = this.DeviceId,
                DeviceSecret = this.DeviceSecret,
                Name = this.Name,
                Region = this.Region,

                CreateDate = DateTime.Now,
                DeleteDate = null,
                Id = 0,
                GuidRow = Guid.NewGuid(),
                IsDeleted = false
            };
        }
    }
}
