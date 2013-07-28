namespace ABC.Domain
{
	#region Using Directives
	
	using System;
	using System.Linq;
	
	using ABC.Domain.Plumbing;

	#endregion 
	
	public static partial class BlogQueries
	{				
		public static IQueryable<Article> QuickSearch (this IQueryable<Article> query, string queryTerms)
		{
			var result = 
				// From all our articles
				(from article in query
			// Filter: title contains the requested terms
				 where article.Title.Contains (queryTerms)
			// Filter: or description contains the requested terms
				|| article.Description.Contains (queryTerms)
			// Filter: or seo title contains the requested terms
				|| article.SeoTitle.Contains (queryTerms)
			// Filter: or seo description contains the requested terms
				|| article.SeoDescription.Contains (queryTerms)
			// Filter: or content contains the requested terms
				|| article.Content.Contains (queryTerms)
			// Filter: or seo keywords contains the requested terms
				|| article.SeoKeywords.Contains (queryTerms)
			// Filter: or tags contains the requested terms
				|| article.Tags.Any(t => t.TagValue.Contains(queryTerms))
				 select article);

			return result;
		}
	}
}
