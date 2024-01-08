using Sepid.Robot.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepid.Robot.Domain.Context
{
    public interface IReadOnlyRepository<TEntity, TKey>
        where TEntity : class, IBaseEntity<TKey>
        where TKey : IComparable
    {
        IQueryable<TEntity> GetAll();
    }

    public interface IReadOnlyRepository<TQuery>
        where TQuery : class
    {
        IQueryable<TQuery> GetAll();
    }
}
