using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepid.Robot.Domain.Interfaces
{
    public interface ISepidRobotContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbSet<Entities.Device> Devices { get; set; }

        Task<bool> SaveAsync(CancellationToken cancellationToken);

        void Commit();

        void Rollback();

        DatabaseFacade Database { get; }

        ChangeTracker ChangeTracker { get; }

        int SaveChanges();

        int SaveChanges(bool acceptAllChangesOnSuccess);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        EntityEntry Entry(object entity);

        Task BeginEnableInsertIdentityAsync<TEntity>() where TEntity : class;

        Task EndEnableInsertIdentityAsync<TEntity>() where TEntity : class;

    }
}
