using System;
using System.Linq;

namespace ABC.Domain
{
	public static partial class BlogQueries
	{
		public static IQueryable<Article> FilterBySeoTitle (this IQueryable<Article> query, string seoTitle)
		{
			IQueryable<Article> result = null;

			if(!string.IsNullOrEmpty(seoTitle))
			{
				result = 
					// From all our articles
					(from article in query
					// Filter: we want only the article with the given seoTitle
				 	where article.SeoTitle == seoTitle
					select article);

			}

			return result;
		}
	}
}

