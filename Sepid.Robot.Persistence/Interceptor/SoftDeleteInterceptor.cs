using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Sepid.Robot.Domain.Entities.Base;

namespace Sepid.Robot.Persistence.Interceptor
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            if (eventData.Context is null) return result;

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry is not { State: EntityState.Deleted, Entity: IBaseEntity<long> delete }) continue;
                entry.State = EntityState.Modified;
                delete.IsDeleted = true;
                delete.DeleteDate = DateTime.Now;
            }
            return result;
        }
    }
}
