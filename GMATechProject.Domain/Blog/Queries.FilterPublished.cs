using System;
using System.Linq;

namespace ABC.Domain
{
	public static partial class BlogQueries
	{
		public static IQueryable<Article> FilterPublished (this IQueryable<Article> query, string[] tags = null)
		{
			var result = 
				// From all our articles
				(from article in query
				// Filter: we want only the published articles
				 where article.State == ArticleState.Published
				// Filter: which have been published today or sooner
				 where article.PublicationDate <= DateTime.Now
				 select article);
			
			// if some tags have been specified
			if (tags != null
				&& tags.Length > 0) {
				// Apply tags filtering
				result = 
					(from article in result
					 where article.Tags.Any (tag => tags.Contains (tag.TagValue))
					 select article);
			}
			
			return result;
		}
	}
}

