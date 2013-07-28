namespace GMATechProject.Web.Admin
{
	#region Using Directives
	
	using System;
	using System.Linq;
	
	using Nancy;
	using Nancy.ModelBinding;
	
	using GMATechProject.Domain;
	using GMATechProject.Domain.Blog;
	
	#endregion
	
	/// <summary>
	/// Module responsible for articles management.
	/// </summary> 
	public class ArticleModule : 
		// ArticleModule is an EntityModule which manage Article entities with an IArticleRepository
		EntityModule<Article, ArticleRepository>
	{
		/// <summary>
		/// Initializes a new instance of ArticleModule.
		/// </summary>
		/// <param name="repository">We will receive an instance of the IArticleRepository by Nancy.TinyIOC container.</param>
		public ArticleModule(IApplication application, ArticleRepository repository) : base(application, repository)
        {
        }
		
		/// <summary>
		/// Overrides the module base path. This module will targets urls like "/admin/articles" ("/admin" is defined in BaseAdminModule)
		/// </summary>
		protected override string BaseModulePath {
			get { return "/articles"; }
		}

		protected override Response ListEntities(dynamic parameters)
		{
			Response result = null;

			// Try to bind that request parameters (querystring and/or form post) to a PaginationRequestParameters class
			var paginationParameters = this.Bind<GMATechProject.Web.PaginationRequestParameters>();
			
			// Default values for pageIndex and pageSize
			var pageIndex = 0;
			var pageSize = 10;
			
			// If we got some parameters
			if(paginationParameters != null)
			{
				// Get the pageIndex from them
				pageIndex = paginationParameters.PageIndex;
				
				// Get the pageSize from them
				pageSize = paginationParameters.PageSize;
			}
			
			// Request our repository for those articles
			var articles = (from article in this.QueryAll() select article)
			                // Order them by date, the more recent first
			                .OrderByDescending (article => article.PublicationDate)			
			                // Just the specified page
			                .ToPaginatedList(pageIndex, pageSize);
			
			// Return a successfull action result with the entities
			result = Response.AsJson(ActionResult.AsSuccess(articles));
			
			return result;
		}
		
		protected override Article InnerCreateEntity (Article entity)
		{
			return this.Application.CreateArticle(entity);
		}
		
		protected override Article InnerUpdateEntity(Article entity)
		{
			return this.Application.UpdateArticle(entity);
		}
	}	
}
