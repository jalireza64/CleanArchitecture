using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepid.Robot.Domain.Context
{
    public interface IReadOnlyFreeRepository<TEntity, TKey>
    where TEntity : class
    where TKey : IComparable
    {
        IQueryable<TEntity> GetAll();
    }
}
