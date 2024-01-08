using Microsoft.EntityFrameworkCore;
using Sepid.Robot.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepid.Robot.Data.Access.Utilities
{
    public static class RepositoryHelper<TEntity, TKey>
        where TEntity : class, IBaseEntity<TKey>
        where TKey : IComparable
    {

        /// <summary>
        /// Load all relations instead on those relations which their names start with target entity
        /// Ex:     Person with relations to 1) Contracts, 2) PersonAccounts, 3) Files
        /// Ret:    Person included Contracts and Files
        /// </summary>
        public static async Task<TEntity> LoadAllRelationsAsync(TEntity item, DbSet<TEntity> dbSet)
        {
            IQueryable<TEntity> result = GetIncludedPropsQuery(item, dbSet);
            return await result.FirstOrDefaultAsync();
        }

        private static IQueryable<TEntity> GetIncludedPropsQuery(TEntity item, DbSet<TEntity> dbSet)
        {
            var type = typeof(TEntity);
            string entityName = type.Name;
            var refCollectionProps =
                from prop in type.GetProperties()
                where prop.PropertyType.IsGenericType && prop.PropertyType.IsPublic &&
                      prop.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>)
                select prop;

            var refEntityProps =
                from prop in type.GetProperties()
                where prop.PropertyType.IsClass && prop.PropertyType.IsPublic &&
                      (typeof(IBaseEntity<>).IsAssignableFrom(prop.PropertyType) ||
                       typeof(BaseEntity).IsAssignableFrom(prop.PropertyType))
                select prop;

            var refProps = refCollectionProps.Union(refEntityProps);

            var result = dbSet.Where(x => x.Id.Equals(item.Id));
            foreach (var refProp in refProps)
                if (!refProp.Name.StartsWith(entityName))
                    result = result.Include(refProp.Name);
            return result;
        }

        /// <summary>
        /// Load all relations instead on those relations which their names start with target entity
        /// Ex:     Person with relations to 1) Contracts, 2) PersonAccounts, 3) Files
        /// Ret:    Person included Contracts and Files
        /// </summary>
        public static TEntity LoadAllRelations(TEntity item, DbSet<TEntity> dbSet)
        {
            IQueryable<TEntity> result = GetIncludedPropsQuery(item, dbSet);
            return result.FirstOrDefault();
        }
    }
}
