using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepid.Robot.Domain.Entities.Base
{
    public interface IBaseEntity<TKey>
    {
        public long Id { get; set; }

        public Guid GuidRow { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? DeleteDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
