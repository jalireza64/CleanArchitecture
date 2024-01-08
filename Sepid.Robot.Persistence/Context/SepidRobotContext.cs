using Microsoft.EntityFrameworkCore;
using Sepid.Robot.Domain.Entities;
using Sepid.Robot.Domain.Interfaces;
using Sepid.Robot.Persistence.Interceptor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepid.Robot.Persistence.Context
{
    public class SepidRobotContext : DbContext, ISepidRobotContext
    {
        public SepidRobotContext(DbContextOptions<SepidRobotContext> options) : base(options)
        {

        }

        public void Commit()
        {

        }

        public void Rollback()
        {

        }

        #region DbSets
        public virtual DbSet<Device> Devices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "EF1000:Possible SQL injection vulnerability.", Justification = "<Pending>")]
        public async Task BeginEnableInsertIdentityAsync<TEntity>() where TEntity : class
        {
            var table = this.GetTableName<TEntity>();
            await this.Database.OpenConnectionAsync();
            await this.Database.ExecuteSqlRawAsync((string)$"SET IDENTITY_INSERT {table} ON");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "EF1000:Possible SQL injection vulnerability.", Justification = "<Pending>")]
        public async Task EndEnableInsertIdentityAsync<TEntity>() where TEntity : class
        {
            var table = this.GetTableName<TEntity>();
            await this.Database.ExecuteSqlRawAsync((string)$"SET IDENTITY_INSERT {table} OFF");
            this.Database.CloseConnection();
        }

        #endregion

        public async Task<bool> SaveAsync(CancellationToken cancellationToken)
        {
            try
            {
                var result = await base.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        int ISepidRobotContext.SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return this.SaveChanges(acceptAllChangesOnSuccess);
        }

        int ISepidRobotContext.SaveChanges()
        {
            return this.SaveChanges();
        }

        async Task<int> ISepidRobotContext.SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await this.SaveChangesAsync(cancellationToken);
        }






        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .AddInterceptors(new SoftDeleteInterceptor());


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>()
                .HasQueryFilter(x => x.IsDeleted == false);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SepidRobotContext).Assembly);
        }

        public string GetTableName<TEntity>()
            where TEntity : class
        {
            var mapping = this.Model.FindEntityType(typeof(TEntity));
            var schema = mapping.GetSchema();
            var tableName = mapping.GetTableName();
            return string.IsNullOrWhiteSpace(schema) ? $"{tableName}" : $"{schema}.{tableName}";
        }
    }
}
