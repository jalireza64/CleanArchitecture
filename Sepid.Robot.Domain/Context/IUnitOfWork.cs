using Sepid.Robot.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepid.Robot.Domain.Context
{
    public interface IUnitOfWork
    {
        void Save();
        Task SaveAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task MigrateAsync();
        Task EnsureCreatedAsync();
        IDatabaseTransaction BeginTransaction();
        IEntityRepository<TEntity, TKey> Set<TEntity, TKey>()
            where TEntity : class, IBaseEntity<TKey>
            where TKey : IComparable;
        IReadOnlyRepository<TEntity, TKey> SetReadOnly<TEntity, TKey>()
            where TEntity : class, IBaseEntity<TKey>
            where TKey : IComparable;
        IReadOnlyFreeRepository<TEntity, TKey> SetFreeReadOnly<TEntity, TKey>()
            where TEntity : class
            where TKey : IComparable;
    }
}
