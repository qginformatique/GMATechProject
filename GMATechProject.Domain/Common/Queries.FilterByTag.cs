using System;
using System.Linq;

namespace ABC.Domain
{
	public static partial class TagQueries
	{
		public static IQueryable<Tag> FilterByTag(this IQueryable<Tag> query, string tag)
		{
			return query.Where(t => t.TagValue == tag);
		}
	}
}

