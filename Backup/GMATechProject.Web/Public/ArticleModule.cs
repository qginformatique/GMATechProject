namespace GMATechProject.Web.Public
{
	#region Using Directives

	using System;
	using System.IO;
	using System.Collections.Generic;
	using System.Linq;
	
	using Nancy;
	using Nancy.ModelBinding;
	using Nancy.Responses;
	using Nancy.Validation;
	
	using FluentValidation;
	using FluentValidation.Validators;
	
	using GMATechProject.Domain;
	using GMATechProject.Domain.Blog;
	using GMATechProject.Web.Public.Models;
	
	#endregion

	/// <summary>
	/// The module responsible for the blog articles management.
	/// </summary>
	public class ArticleModule : BasePublicModule
	{
		#region Fields
		
		private readonly IArticleRepository _ArticleRepository;
				
		#endregion
		
		#region Properties

		IRootPathProvider _RootPathProvider { get; set; }
		
		/// <summary>
		/// Gets the entity's repository.
		/// </summary>
		protected IArticleRepository ArticleRepository
		{
			get 
			{
				return this._ArticleRepository;
			}
		}

		#endregion
				
		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ArticleModule" />.
		/// </summary>
		/// <param name="repository">The article's repository</param>
		public ArticleModule (IApplication application, IArticleRepository articleRepository, IRootPathProvider rootPathProvider) : base(application)
		{			
			this._ArticleRepository = articleRepository;
			this._RootPathProvider = rootPathProvider;

			// Bind the HTTP GET verb to the ListArticles method
			this.Get["/articles"] = ListArticles;

			// Bind the HTTP GET verb to the ListHomeArticles method
			this.Get["/articles/homeArticles"] = ListHomeArticles;

			// Bind the HTTP GET verb to the ListSliderArticles method
			this.Get["/articles/sliderArticles"] = ListSliderArticles;

			// Define a route for urls "/articles/{seoTitle}" which will returns the article matching the specified slug
			this.Get["/articles/{seoTitle}"] = GetArticle;

			// Define a route for urls "/article/{seoTitle}" which will returns the article matching the specified slug
			this.Get["/article/apercu/{seoTitle}"] = GetArticleBySeoTitle;

			// Define a route for urls "/article/{seoTitle}" which will returns the article matching the specified slug
			this.Get["/article/{seoTitle}"] = GetArticleBySeoTitle;

			// Define a route for urls "/articles/byId" which will returns the article matching the specified id
			this.Get["/articles/articles/byId"] = GetArticlebyId;

			// Bind the HTTP GET verb to the ListEvents method
			this.Get["/actualites/actualites"] = ListPublicArticles;
		}		
		
		#endregion
		
		#region Protected Methods

		protected Object ListPublicArticles (dynamic parameters)
		{
			// Optimisation SEO (anti duplicate Content).
			// Redirige vers l'url sans "/" de fin
			if(Request.Url.Path.EndsWith("/")) {
				return Response.AsRedirect(Request.Url.Path.TrimEnd('/'), RedirectResponse.RedirectType.Permanent);
			}

			// Try to bind that request parameters (querystring and/or form post) to a PaginationRequestParameters class
			var listEventsParameters = this.Bind<PaginationRequestParameters> ();
			
			// Default values for pageIndex and pageSize
			var pageIndex = 0;
			var pageSize = 10;
			
			// If we got some parameters
			if (listEventsParameters != null) {
				// Get the pageIndex from them
				pageIndex = listEventsParameters.PageIndex;
				
				// Get the pageSize from them
				pageSize = listEventsParameters.PageSize;
			}
			
			// Request our repository for those articles
			var articles = this.Application
				// Just the specified page from the last published articles
				.ListPublishedArticles(pageIndex, pageSize, this.CurrentUser.Role, null);

			var _resultArticles = new List<ArticleExtended>();
			foreach(var _article in articles.Items)
			{
				var articleExtended = new ArticleExtended();
				articleExtended.CreationDate = _article.CreationDate;
				articleExtended.Content = _article.Content;
				articleExtended.Description = _article.Description;
				articleExtended.FriendlyUrl = _article.FriendlyUrl;
				articleExtended.HomeSlider = _article.HomeSlider;
				articleExtended.Id = _article.Id;
				articleExtended.ImageUrl = _article.ImageUrl;
				articleExtended.ModificationDate = _article.ModificationDate;
				articleExtended.PublicationDate = _article.PublicationDate;
				articleExtended.PublicationState = _article.PublicationState;
				articleExtended.SeoDescription = _article.SeoDescription;
				articleExtended.SeoKeywords = _article.SeoKeywords;
				articleExtended.SeoTitle = _article.SeoTitle;
				articleExtended.Tags = _article.Tags;
				articleExtended.Title = _article.Title;
				
				_resultArticles.Add(articleExtended);
			}
			var _resultPaginatedArticles = new PaginatedList<ArticleExtended>(articles.PageIndex, articles.PageSize, articles.Total, _resultArticles);
			
			var model = new ArticlesModel (UseConcatenatedResources){
				Articles = _resultPaginatedArticles
			};

			model.NavMenu.NavSectionsMenuLeft[0].IsActive = "active";

			return this.View["Public/Pages/articles.cshtml", model];
		}

		public Object GetArticleBySeoTitle(dynamic parameters)
		{
			// Optimisation SEO (anti duplicate Content).
			// Redirige vers l'url sans "/" de fin
			if(Request.Url.Path.EndsWith("/")) {
				return Response.AsRedirect(Request.Url.Path.TrimEnd('/'), RedirectResponse.RedirectType.Permanent);
			}

			// Get seo title
			var seoTitle = parameters.seoTitle;
			
			// Request our repository for those articles
			Article article = _ArticleRepository
				// Get article from the giving seoTitle
				.GetArticle(seoTitle);

			var _articleExtended = new ArticleExtended();
			_articleExtended.Content = article.Content;
			_articleExtended.Description = article.Description;
			_articleExtended.HomeSlider = article.HomeSlider;
			_articleExtended.Id = article.Id;
			_articleExtended.ImageUrl = article.ImageUrl;
			_articleExtended.PublicationDate = article.PublicationDate;
			_articleExtended.PublicationState = article.PublicationState;
			_articleExtended.SeoDescription = article.SeoDescription;
			_articleExtended.SeoKeywords = article.SeoKeywords;
			_articleExtended.SeoTitle = article.SeoTitle;
			_articleExtended.Tags = article.Tags;
			_articleExtended.Title = article.Title;

			if (Request.Url.Path.Contains ("apercu") && _articleExtended.PublicationState == PublicationState.Draft) {
				_articleExtended.ShowDraft = true;
			} else {
				_articleExtended.ShowDraft = false;
			}

			var model = new ArticleModel (UseConcatenatedResources){
				Article = _articleExtended,
				HtmlTitle = article.Title,
				MetaDescription = article.SeoDescription.Replace ("\"", "'"),
				MetaKeywords = article.SeoKeywords.Count > 0 ? string.Join(", ", article.SeoKeywords) : ""
			};

			return this.View["Public/Pages/article.cshtml", model];
		}

		protected virtual Response GetArticle(dynamic parameters)
		{
			Response result = null;
			
			// Get seo title
			var seoTitle = parameters.seoTitle;
			
			// Request our repository for those articles
			var article = _ArticleRepository
				// Get article from the giving seoTitle
				.GetArticle(seoTitle);

			// Return a successfull action result with the articles
			//result = Response.AsJson(ActionResult.AsSuccess(article));
			result = FormatterExtensions.AsJson(Response, ActionResult.AsSuccess(article));
			
			return result;
		}

		protected virtual Response GetArticlebyId(dynamic parameters)
		{
			Response result = null;
			
			// Try to bind that request parameters (querystring and/or form post) to a RequestContentByIdParameters class
			var requestContentByIdParameters = this.Bind<RequestContentByIdParameters> ();

			// Request our repository for this article
			var article = _ArticleRepository
				// Get article from the giving seoTitle
				.GetById(requestContentByIdParameters.Id);

			// Return a successfull action result with the article
			result = FormatterExtensions.AsJson(Response, ActionResult.AsSuccess(article));

			return result;
		}

		protected virtual Response ListSliderArticles (dynamic parameters)
		{
			Response result = null;

			// Default values for pageIndex and pageSize
			var pageIndex = 0;
			var pageSize = 4;
			
			// Try to bind that request parameters (querystring and/or form post) to a PaginationRequestParameters class
			var paginationParameters = this.Bind<PaginationRequestParameters> ();

			// If we have pagination parameters
			if(paginationParameters != null)
			{
				pageIndex = paginationParameters.PageIndex;
				pageSize = paginationParameters.PageSize;
			}
			
			// Request our repository for those articles
			var articles = this.Application
				// Just the specified page from the last published articles
				.ListHomeArticles(pageIndex, pageSize, this.CurrentUser.Role, true, null);
			
			// Return a successfull action result with the articles
			result = Response.AsJson(ActionResult.AsSuccess(articles));
			
			return result;
		}

		protected virtual Response ListHomeArticles (dynamic parameters)
		{
			Response result = null;
			
			// Try to bind that request parameters (querystring and/or form post) to a PaginationRequestParameters class
			var paginationParameters = this.Bind<PaginationRequestParameters> ();
			
			// Default values for pageIndex and pageSize
			var pageIndex = 0;
			var pageSize = 4;
			
			// If we got some parameters
			if (paginationParameters != null) {
				// Get the pageIndex from them
				pageIndex = paginationParameters.PageIndex;
				
				// Get the pageSize from them
				pageSize = paginationParameters.PageSize;
			}
			
			// Request our repository for those articles
			var articles = this.Application
				// Just the specified page from the last published articles
				.ListHomeArticles(pageIndex, pageSize, this.CurrentUser.Role, false, null);
			
			// Return a successfull action result with the articles
			result = Response.AsJson(ActionResult.AsSuccess(articles));

			return result;
		}
		
		protected virtual Response ListArticles (dynamic parameters)
		{
			Response result = null;
			
			// Try to bind that request parameters (querystring and/or form post) to a PaginationRequestParameters class
			var paginationParameters = this.Bind<PaginationRequestParameters> ();
			
			// Default values for pageIndex and pageSize
			var pageIndex = 0;
			var pageSize = 10;
						
			// If we got some parameters
			if (paginationParameters != null) {
				// Get the pageIndex from them
				pageIndex = paginationParameters.PageIndex;

				// Get the pageSize from them
				pageSize = paginationParameters.PageSize;
			}
			
			// Request our repository for those articles
			var articles = this.Application
			// Just the specified page from the last published articles
				.ListPublishedArticles(pageIndex, pageSize, this.CurrentUser.Role, null);

			// Return a successfull action result with the articles
			result = Response.AsJson(ActionResult.AsSuccess(articles));
			
			return result;
		}
		
		#endregion
	}
}