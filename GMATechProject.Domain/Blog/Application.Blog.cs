namespace GMATechProject.Domain
{
	#region Using Directives
	
	using System;
	using System.Collections.Generic;
	using System.Linq;
	
	using GMATechProject.Domain.Blog;
	using GMATechProject.Domain.Security;
	
	using FluentValidation;
	using MongoDB.Driver.Linq;
	
	#endregion

	public partial class Application : IApplication
	{
		private readonly IArticleRepository _ArticleRepository;
		
		public Article CreateArticle (Article article)
		{			
			var result = this.CreateEntity<Article> (article, this._ArticleRepository);

			if (result.CreationDate == null || result.CreationDate == DateTime.MinValue) {
				result.CreationDate = DateTime.Now;
			}

			result.ModificationDate = DateTime.Now;

			result.Tags = new HashSet<string>();
			result.SeoKeywords = new HashSet<string>();

			return result;
		}
		
		public Article UpdateArticle (Article article)
		{
			if (article.CreationDate == null || article.CreationDate == DateTime.MinValue) {
				article.CreationDate = DateTime.Now;
			}

			article.ModificationDate = DateTime.Now;

			return this.UpdateEntity (article, this._ArticleRepository);
		}

		public PaginatedList<Article> ListHomeArticles (int pageIndex = 0, int pageSize = 10, Roles role = Roles.None, bool homeSlider = false, string[] tags = null)
		{
			var tagsForbiddenForRole = this._RoleTagsBindingRepository.ListTagsForbiddenForRole(role);
			
			this._Logger.Debug("ListPublishedArticles: role = " + role);
			this._Logger.Debug("ListPublishedArticles: tags forbidden for role = " + string.Join(", ", tagsForbiddenForRole));
			
			var query =
				// We want all the published articles
				(from article in this._ArticleRepository.QueryPublished(tags)				 
				 select article);
			
			// If our user is not an administrator
			if(role != Roles.Administrator)
			{
				query = 
					(from article in query
					 // Filter: all events without any tags
					 where (!article.Tags.Any ())
					 // Or: @events who have all their tags bound to this role
					 || (!article.Tags.ContainsAny(tagsForbiddenForRole))
					 select article);
			}

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
					.ToPaginatedList (pageIndex, pageSize);
			
			return result;
		}

	
		public PaginatedList<Article> ListPublishedArticles (int pageIndex = 0, int pageSize = 10, Roles role = Roles.None, string[] tags = null)
		{
			var tagsForbiddenForRole = this._RoleTagsBindingRepository.ListTagsForbiddenForRole(role);

			this._Logger.Debug("ListPublishedArticles: role = " + role);
			this._Logger.Debug("ListPublishedArticles: tags forbidden for role = " + string.Join(", ", tagsForbiddenForRole));

			var query =
				// We want all the enabled events
				(from @event in this._ArticleRepository.QueryPublished(tags)				 
				 select @event);

			// If our user is not an administrator
			if(role != Roles.Administrator)
			{
				query = 
					(from article in query
					// Filter: all events without any tags
				 	where (!article.Tags.Any ())
					// Or: @events who have all their tags bound to this role
				 	|| (!article.Tags.ContainsAny(tagsForbiddenForRole))
			        select article);
			}
			
			var result = query
				.OrderByDescending (article => article.PublicationDate)
				.ToPaginatedList (pageIndex, pageSize);

			return result;
		}
		
	}
}