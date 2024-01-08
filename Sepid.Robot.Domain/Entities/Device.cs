using Sepid.Robot.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepid.Robot.Domain.Entities
{
    public class Device : BaseEntity
    {
        public Device()
        {

        }
        public string Name { get; set; }
        public string DeviceId { get; set; }
        public string DeviceSecret { get; set; }
        public string Region { get; set; }
        public string Description { get; set; }
    }
}
