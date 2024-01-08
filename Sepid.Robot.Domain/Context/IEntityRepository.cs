using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Sepid.Robot.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sepid.Robot.Domain.Context
{
    public interface IEntityRepository<TEntity, TKey>
    where TEntity : class, IBaseEntity<TKey>
    where TKey : IComparable
    {
        #region Delete
        (bool success, TEntity obj) Delete(TKey id);

        Task<(bool success, TEntity obj)> DeleteAsync(TKey id);

        #endregion

        #region Update

        (bool success, TEntity obj) Update(TEntity input);

        Task<(bool success, TEntity obj)> UpdateAsync(TEntity input);

        #endregion

        #region Utilities

        long Count();

        Task<long> CountAsync();

        Task<bool> AnyAsync();

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

        #endregion

        #region Create

        (bool success, TEntity obj) Create(TEntity input);

        Task<(bool success, TEntity obj)> CreateAsync(TEntity input);

        Task<(bool success, TEntity obj)> CreateWithIdentityAsync(TEntity input);

        #endregion

        #region Get
        TEntity GetById(TKey id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

        Task<TEntity> GetByIdAsync(TKey id,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);
        TEntity GetByGuid(Guid id,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

        Task<TEntity> GetByGuidAsync(Guid id,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

        public IQueryable<TEntity> GetAll(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

        #endregion
    }
}
