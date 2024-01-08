using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Sepid.Robot.Domain.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Sepid.Robot.Domain.Entities.Base;
using Sepid.Robot.Domain.Interfaces;
using Sepid.Robot.Data.Access.Exceptions;
using Sepid.Robot.Data.Access.Utilities;
using Sepid.Robot.Utility.EntityFramework;

namespace Sepid.Robot.Data.Access.Repository
{
    public class EfEntityRepository<TEntity, TKey> : IEntityRepository<TEntity, TKey>
        where TEntity : class, IBaseEntity<TKey>
        where TKey : IComparable
    {
        public EfEntityRepository(ISepidRobotContext sepidRobotContext)
        {
            Context = sepidRobotContext ?? throw new ArgumentNullException(nameof(sepidRobotContext));
            _dbSet = Context.Set<TEntity>();
        }

        #region Helpers
        public ISepidRobotContext Context { get; }
        private readonly DbSet<TEntity> _dbSet;


        private TEntity InitializationObject(TEntity obj)
        {
            obj.CreateDate = DateTime.Now;
            obj.GuidRow = Guid.NewGuid();
            obj.IsDeleted = false;
            return obj;
        }

        private IEnumerable<TEntity> InitializationObjects(IEnumerable<TEntity> objs)
        {
            foreach (var obj in objs)
            {
                obj.CreateDate = DateTime.Now;
                obj.GuidRow = Guid.NewGuid();
                obj.IsDeleted = false;
                yield return obj;
            }
        }

        private void CheckExistence(TEntity obj, object id = null)
        {
            if (obj == null)
                throw new NotFoundException($"Item not found.");
        }

        private void CheckDeleteFlag(TEntity obj, object id = null)
        {
            if (obj.IsDeleted)
                throw new FetchDataException($"Item does not exist.");
        }

        #endregion

        #region Delete
        public (bool success, TEntity obj) Delete(TKey id)
        {
            try
            {
                var item = _dbSet.Find(id);
                CheckIfIsReference(item);
                CheckExistence(item);
                item.IsDeleted = true;
                return Update(item);
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error happened while deleting a record.";
                throw new DeleteDataException(exceptionMessage, ex);
            }
        }

        public async Task<(bool success, TEntity obj)> DeleteAsync(TKey id)
        {
            try
            {
                var item = await _dbSet.FindAsync(id);
                await CheckIfIsReferenceAsync(item);
                CheckExistence(item);
                CheckDeleteFlag(item);
                item.IsDeleted = true;
                return await UpdateAsync(item);
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error happened while deleting a record.";
                throw new DeleteDataException(exceptionMessage, ex);
            }
        }

        private async Task CheckIfIsReferenceAsync(TEntity item)
        {
            var loadItem = await RepositoryHelper<TEntity, TKey>
                .LoadAllRelationsAsync(item, _dbSet);
            if (loadItem.IsReferenced(out var referencedEntity))
            {
                string errorMessage = "This {0} is referenced to {1}, cannot delete it.";

                throw new DeleteDataException(errorMessage, null);
            }
        }

        private void CheckIfIsReference(TEntity item)
        {
            var loadItem = RepositoryHelper<TEntity, TKey>
                .LoadAllRelations(item, _dbSet);
            if (loadItem.IsReferenced(out var referencedEntity))
            {
                string errorMessage = "This {0} is referenced to {1}, cannot delete it.";
                throw new DeleteDataException(errorMessage, null);
            }
        }

        #endregion

        #region Update

        public (bool success, TEntity obj) Update(TEntity input)
        {
            try
            {
                var targetItem = _dbSet.Find(input.Id);
                CheckExistence(targetItem);
                Context.Entry(targetItem).CurrentValues.SetValues(input);
                return (true, input);
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error happened while updating a record.";
                throw new UpdateDataException(exceptionMessage, ex);
            }
        }

        public async Task<(bool success, TEntity obj)> UpdateAsync(TEntity input)
        {
            try
            {
                var targetItem = await _dbSet.FindAsync(input.Id);
                CheckExistence(targetItem);
                Context.Entry(targetItem).CurrentValues.SetValues(input);
                await this.Context.SaveChangesAsync();
                return (true, input);
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error happened while updating a record.";
                throw new UpdateDataException(exceptionMessage, ex);
            }
        }

        #endregion

        #region Utilities

        public long Count()
        {
            try
            {
                var data = _dbSet;
                return data.Count();
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error happened while fetching records.";
                throw new FetchDataException(exceptionMessage, ex);
            }
        }

        public async Task<long> CountAsync()
        {
            try
            {
                var data = _dbSet;
                return await data.CountAsync();
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error happened while fetching records.";
                throw new FetchDataException(exceptionMessage, ex);
            }
        }

        public async Task<bool> AnyAsync()
        {
            try
            {
                return await _dbSet
                    .AnyAsync();
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error happened while calcualte lenth of rows.";
                throw new FetchDataException(exceptionMessage, ex);
            }
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            try
            {
                return await _dbSet
                    .IncludeMultiple(include)
                    .AnyAsync(predicate);
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error happened while calcualte lenth of rows.";
                throw new FetchDataException(exceptionMessage, ex);
            }
        }

        #endregion

        #region Create

        public (bool success, TEntity obj) Create(TEntity input)
        {
            try
            {
                input = InitializationObject(input);
                _dbSet.Add(input);
                return (true, input);
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error happened while creating new record of data.";
                throw new CreateDataException(exceptionMessage, ex);
            }
        }

        public async Task<(bool success, TEntity obj)> CreateAsync(TEntity input)
        {
            try
            {
                input = InitializationObject(input);
                await _dbSet.AddAsync(input);
                await this.Context.SaveChangesAsync();
                return (true, input);
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error happened while creating new record of data.";
                throw new CreateDataException(exceptionMessage, ex);
            }
        }

        public async Task<(bool success, TEntity obj)> CreateWithIdentityAsync(TEntity input)
        {
            try
            {
                input = InitializationObject(input);
                await _dbSet.AddAsync(input);
                await this.Context.BeginEnableInsertIdentityAsync<TEntity>();
                await this.Context.SaveChangesAsync();
                await this.Context.EndEnableInsertIdentityAsync<TEntity>();
                return (true, input);
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error happened while creating new record of data.";
                throw new CreateDataException(exceptionMessage, ex);
            }
        }

        #endregion

        #region Get
        public TEntity GetById(TKey id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            try
            {
                var item = _dbSet
                    .AsNoTracking()
                    .IncludeMultiple(include)
                    .FirstOrDefault(x => x.Id.Equals(id) && !x.IsDeleted);

                CheckExistence(item);
                return item;
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error happened while fetching a record.";
                throw new FetchDataException(exceptionMessage, ex);
            }
        }

        public async Task<TEntity> GetByIdAsync(TKey id,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            try
            {
                var item = await _dbSet
                    .AsNoTracking()
                    .IncludeMultiple(include)
                    .FirstOrDefaultAsync(x => x.Id.Equals(id) && !x.IsDeleted);

                CheckExistence(item);
                return item;
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error happened while fetching a record.";
                throw new FetchDataException(exceptionMessage, ex);
            }
        }
        public TEntity GetByGuid(Guid id,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            try
            {
                var item = _dbSet
                    .AsNoTracking()
                    .IncludeMultiple(include)
                    .FirstOrDefault(x => x.GuidRow.Equals(id));

                CheckExistence(item, id);
                return item;
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error happened while fetching a record.";
                throw new FetchDataException(exceptionMessage, ex);
            }
        }

        public async Task<TEntity> GetByGuidAsync(Guid id,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            try
            {
                var item = await _dbSet
                    .AsNoTracking()
                    .IncludeMultiple(include)
                    .FirstOrDefaultAsync(x => x.GuidRow.Equals(id));
                CheckExistence(item, id);
                return item;
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error happened while fetching a record.";
                throw new FetchDataException(exceptionMessage, ex);
            }
        }

        public IQueryable<TEntity> GetAll(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            try
            {
                var rawdata = _dbSet
                    .IncludeMultiple(include);
                return rawdata;
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error happened while fetching records.";
                throw new FetchDataException(exceptionMessage, ex);
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            try
            {
                var rawdata = _dbSet
                    .AsNoTracking()
                    .IncludeMultiple(include);
                rawdata = predicate == null ? rawdata : rawdata.Where(predicate);
                return await rawdata.ToListAsync();
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error happened while fetching records.";
                throw new FetchDataException(exceptionMessage, ex);
            }
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            try
            {
                var item = _dbSet
                    .AsNoTracking()
                    .IncludeMultiple(include)
                    .FirstOrDefault(predicate);
                CheckExistence(item);
                return item;
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error happened while fetching a record.";
                throw new FetchDataException(exceptionMessage, ex);
            }
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            try
            {
                var item = await _dbSet
                    .AsNoTracking()
                    .IncludeMultiple(include)
                    .FirstOrDefaultAsync(predicate);

                CheckExistence(item);
                return item;
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error happened while fetching a record.";
                throw new FetchDataException(exceptionMessage, ex);
            }
        }

        #endregion

    }
}
