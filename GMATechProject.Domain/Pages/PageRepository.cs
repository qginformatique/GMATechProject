namespace GMATechProject.Domain.Pages
{
	#region Using Directives
	
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;

	using MongoDB.Driver.Linq;
	
	using GMATechProject.Domain.Plumbing;
	using GMATechProject.Domain.Security;

	#endregion 
	
	/// <summary>
	/// Description of PageRepositoryMongo.
	/// </summary>
	public class PageRepository : Repository<Page>, IPageRepository
	{
		public Page GetPage (string friendlyUrl)
		{
			Page result = null;
			
			// TODO: add verification (like seoTitle not null) otherwise return error			
			var query = 
				// From all our pages
				(from page in this.All ()
			// Filter: we want only the article with the given seoTitle
				 where page.FriendlyUrl == friendlyUrl || page.SeoTitle == friendlyUrl || page.Title == friendlyUrl
				 select page);
			
			// TODO: add verification (for example: check if an page is returned) otherwise return error
			result = query.FirstOrDefault ();
			
			return result;
		}
		
		public IQueryable<Page> QueryPublished (string[] tags = null)
		{
			var query = 
				// From all our pages
				(from page in this.All ()
			// Filter: we want only the published pages
				 where page.PublicationState == PublicationState.Published
				 select page);
			
			// if some tags have been specified
			if (tags != null
				&& tags.Length > 0) {
				// Apply tags filtering
				query = 
					(from page in query
					 where page.Tags.Any (tag => tags.Contains (tag))
					 select page);
			}
			
			return query;
		}

		public override PaginatedList<Page> QuickSearch (string queryTerms, int pageIndex = 0, int pageSize = 10)
		{
			var query = 
				// From all our pages
				(from page in this.All ()
				// Filter: title contains the requested terms
				 where page.Title.Contains (queryTerms)
				// Filter: or seo title contains the requested terms
				 || page.SeoTitle.Contains (queryTerms)
				// Filter: or seo description contains the requested terms
				 || page.SeoDescription.Contains (queryTerms)
				// Filter: or content contains the requested terms
				 || page.Content.Contains (queryTerms)
				// Filter: or seo keywords contains the requested terms
				 || page.SeoKeywords.Contains (queryTerms)
				// Filter: or tags contains the requested terms
				 || page.Tags.Contains (queryTerms)
				 select page);
			

			query = query
			// Order them by date, the more recent first
				.OrderByDescending (page => page.ModificationDate);
			
			// Get the query's result as a paginated list
			var result = query.ToPaginatedList (pageIndex, pageSize);
			
			return result;
		}
	}
}