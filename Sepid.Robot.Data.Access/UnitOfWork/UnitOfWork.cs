using Microsoft.EntityFrameworkCore;
using Sepid.Robot.Data.Access.Repository;
using Sepid.Robot.Domain.Context;
using Sepid.Robot.Domain.Entities.Base;
using Sepid.Robot.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepid.Robot.Data.Access.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISepidRobotContext _context;

        public UnitOfWork(ISepidRobotContext dbContext)
        {
            _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IDatabaseTransaction BeginTransaction()
        {
            return new EfTransaction(_context);
        }

        public IEntityRepository<TEntity, TKey> Set<TEntity, TKey>()
            where TEntity : class, IBaseEntity<TKey>
            where TKey : IComparable
        {
            return new EfEntityRepository<TEntity, TKey>(_context);
        }

        public IReadOnlyRepository<TEntity, TKey> SetReadOnly<TEntity, TKey>()
            where TEntity : class, IBaseEntity<TKey>
            where TKey : IComparable
        {
            return new ReadOnlyRepository<TEntity, TKey>(_context);
        }

        public IReadOnlyFreeRepository<TEntity, TKey> SetFreeReadOnly<TEntity, TKey>()
            where TEntity : class
            where TKey : IComparable
        {
            return new ReadOnlyFreeRepository<TEntity, TKey>(_context);
        }

        public void Save() => _context.SaveChanges();

        public async Task SaveAsync(CancellationToken cancellationToken = default(CancellationToken)) => await _context.SaveChangesAsync(cancellationToken);

        public async Task MigrateAsync()
        {
            await _context.Database.MigrateAsync();
        }

        public async Task EnsureCreatedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
        }
    }
}
