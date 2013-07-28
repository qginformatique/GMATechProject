using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace ABC.Domain
{
	public static partial class Queries
	{
		public static TEntity GetById<TEntity>(this IQueryable<TEntity> query, object id)
			where TEntity : class, IEntity
		{
			var result = 
				(from entity in query
				 where entity.Id == id.ToString()
				 select entity );

			return result.FirstOrDefault();
		}

		public static IQueryable<TEntity> QuickSearch<TEntity>(this IQueryable<TEntity> query, string queryTerms)
			where TEntity : class, IEntity
		{
			var properties = typeof(TEntity).GetProperties();

			Expression expression = null;

			var expressionParameter = Expression.Parameter(typeof(TEntity), "entity");

			foreach (var property in properties) 
			{
				if(property.HasAttribute<SearchableAttribute>())
				{
					var expressionForProperty = Expression.Call(
						Expression.Property(expressionParameter, typeof(string).GetProperty(property.Name)), 
						typeof(string).GetMethod("Contains", System.Type.EmptyTypes),
						Expression.Constant(queryTerms));

					if(expression == null)
					{
						expression = expressionForProperty;
					}
					else
					{
						expression = Expression.OrElse(expression, expressionForProperty);
					}
				}
			}

            MethodCallExpression expressionWhere = Expression.Call(
                typeof(Queryable),
                "Where", 
                new Type[] { query.ElementType },
                query.Expression, 
                Expression.Lambda<Func<TEntity, bool>>(expression, new ParameterExpression[] { expressionParameter }));


			return query.Provider.CreateQuery<TEntity>(expressionWhere);
		}
	}
}

