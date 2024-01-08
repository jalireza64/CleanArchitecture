using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sepid.Robot.Utility.EntityFramework
{
    public static class LinqExtensions
    {
        /// <summary>
        /// An extension method to chain include and join tables
        /// </summary>
        public static IQueryable<TEntity> IncludeMultiple<TEntity>(
            this IQueryable<TEntity> query,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
            where TEntity : class
        {
            if (include != null)
                query = include(query);
            return query;
        }

        /// <summary>
        /// Check if record is referenced from other entities
        /// </summary>
        public static bool IsReferenced<TEntity>(this TEntity item, out string reference)
            where TEntity : class
        {
            var type = typeof(TEntity);

            var refProps =
                from prop in type.GetProperties()
                where prop.PropertyType.IsGenericType &&
                      prop.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>)
                select prop;

            foreach (var refProp in refProps)
            {
                dynamic value = refProp.GetValue(item);
                if (value == null) continue;
                if (value.Count > 0)
                {
                    var refPropTypeInfo = refProp.PropertyType.GetTypeInfo();
                    string propTypeName = refPropTypeInfo.GetGenericArguments()[0]?.Name;
                    reference = propTypeName;
                    return true;
                }
            }
            reference = string.Empty;
            return false;
        }
    }
}
