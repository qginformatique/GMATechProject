namespace GMATechProject.Domain.Blog
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
	/// Description of ArticleRepositoryMongo.
	/// </summary>
	public class ArticleRepository : Repository<Article>, IArticleRepository
	{
		public Article GetArticle (string seoTitle)
		{
			Article result = null;
			
			// TODO: add verification (like seoTitle not null) otherwise return error			
			var query = 
				// From all our articles
				(from article in this.All ()
			// Filter: we want only the article with the given seoTitle
				 where article.FriendlyUrl == seoTitle || article.SeoTitle == seoTitle || article.Title == seoTitle
				 select article);
			
			// TODO: add verification (for example: check if an article is returned) otherwise return error
			result = query.FirstOrDefault ();
			
			return result;
		}
		
		public IQueryable<Article> QueryPublished (string[] tags = null)
		{
			var query = 
				// From all our articles
				(from article in this.All ()
			// Filter: we want only the published articles
				 where article.PublicationState == PublicationState.Published
			// Filter: which have been published today or sooner
				 where article.PublicationDate <= DateTime.Now
				 select article);
			
			// if some tags have been specified
			if (tags != null
				&& tags.Length > 0) {
				// Apply tags filtering
				query = 
					(from article in query
					 where article.Tags.Any (tag => tags.Contains (tag))
					 select article);
			}
			
			return query;
		}

		public override PaginatedList<Article> QuickSearch (string queryTerms, int pageIndex = 0, int pageSize = 10)
		{
			var query = 
				// From all our articles
				(from article in this.All ()
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
				 || article.Tags.Contains (queryTerms)
				 select article);
			

			query = query
			// Order them by date, the more recent first
				.OrderByDescending (article => article.PublicationDate);
			
			// Get the query's result as a paginated list
			var result = query.ToPaginatedList (pageIndex, pageSize);
			
			return result;
		}

		public IList<Article> ListHomeArticles (int pageIndex = 0, int pageSize = 10, bool homeSlider = false, string[] tags = null)
		{
			var query =
				// We want all the published articles
				(from article in this.QueryPublished(tags)				 
				 select article);
			
			// Filter HomeSlider Only
			if(homeSlider)
			{
				query =  query.Where(article => article.HomeSlider);
			}
			else
			{
				// Tous les articles sauf les 4 premiers articles affichés dans le slider
				var sliderArticles = query.Where(article => article.HomeSlider).OrderByDescending(article => article.PublicationDate).Take(4).ToList();
				var articles = query.ToList();
				var resultArticles = articles.Where( x => !sliderArticles.Any(y => x.Id == y.Id));
				
				query = resultArticles.AsQueryable();
			}
			
			var result = query
				.OrderByDescending (article => article.PublicationDate)
					.ToPaginatedList (pageIndex, pageSize).Items;
			
			return result;
		}
	}
}