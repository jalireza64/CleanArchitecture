using Microsoft.EntityFrameworkCore;
using Sepid.Robot.Data.Access.Exceptions;
using Sepid.Robot.Domain.Context;
using Sepid.Robot.Domain.Entities.Base;
using Sepid.Robot.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepid.Robot.Data.Access.Repository
{
    public class ReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity, TKey>
        where TEntity : class, IBaseEntity<TKey>
        where TKey : IComparable
    {
        private readonly DbSet<TEntity> _dbSet;
        public ISepidRobotContext Context { get; }

        public ReadOnlyRepository(ISepidRobotContext appdbcontext)
        {
            Context = appdbcontext ?? throw new ArgumentNullException(nameof(appdbcontext));
            _dbSet = Context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                var rawdata = _dbSet
                    .AsNoTracking();
                return rawdata;
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error happened while fetching records.";
                throw new FetchDataException(exceptionMessage, ex);
            }
        }
    }
}
