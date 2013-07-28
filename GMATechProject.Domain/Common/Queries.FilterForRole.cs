using System;
using System.Linq;

namespace ABC.Domain
{
	public static partial class Queries
	{
		public static IQueryable<TEntity> FilterForRole<TEntity>(this IQueryable<TEntity> query, Roles role)
			where TEntity : class, IWithTags
		{
			var result = 
				// From all our items 
				(from item in query
				 // Filter: all items without any tags
				 where item.Tags.Count() == 0
				// Or: items who have all their tags bound to this role
				 || !item.Tags.Any(tag => tag.Role > role)
				 // Or: If role is admin, we want all of them
				 || role == Roles.Administrator
				 select item);

			return result;
		}
	}
}

