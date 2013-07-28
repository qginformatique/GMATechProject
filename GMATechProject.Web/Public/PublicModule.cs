using GMATechProject.Domain;

namespace GMATechProject.Web.Public
{
	#region Using Directives
	
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.IO;
	using System.Linq;
	
	using GMATechProject.Domain.Blog;
	using GMATechProject.Domain.Pages;
	using GMATechProject.Web.Public.Models;

	using Nancy;
	using Nancy.Responses;

	#endregion
	
	/// <summary>
	/// Description of BasePublicModule.
	/// </summary>
	public class PublicModule : BasePublicModule
	{	
		#region Properties

		private readonly IArticleRepository _ArticleRepository;
		private readonly IPageRepository _PageRepository;

		IRootPathProvider _rootPathProvider { get; set; }

		#endregion

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

		/// <summary>
		/// Gets the entity's repository.
		/// </summary>
		protected IPageRepository PageRepository
		{
			get 
			{
				return this._PageRepository;
			}
		}

		public PublicModule(IApplication application, IRootPathProvider rootPathProvider,
		                    IArticleRepository articleRepository, IPageRepository pageRepository) : base(application)
		{
			this._ArticleRepository = articleRepository;
			this._PageRepository = pageRepository;
			this._rootPathProvider = rootPathProvider;

			// Bind the HTTP POST verb with /robots.txt with a response filled by text
			this.Get["/robots.txt"] = GetRobotTxt;

			// Bind the HTTP POST verb with /sitemap.xml to the GetSitemap method
			this.Get["/sitemap.xml"] = GetSitemap;
			
			// Define a route for urls "/{seoTitle}" which will returns the article matching the specified slug
			this.Get["/{category}/{seoTitle}"] = parameters => {
				// [BUG]: /sitemap.xml n'est pas prioritaire sur /{seoTitle}
				if(parameters.seoTitle == "rss")
					return GetRSSFeed(parameters);
				// fin de [BUG]
				return GetStaticPage(parameters);
			};

			// Define a route for urls "/{seoTitle}" which will returns the article matching the specified slug
			this.Get["/{seoTitle}"] = parameters => { 
				// [BUG]: /sitemap.xml n'est pas prioritaire sur /{seoTitle}
				if(parameters.seoTitle == "sitemap")
					return GetSitemap(parameters);
				// fin de [BUG]
				return GetStaticPage(parameters);
			};
		}

		public Object GetStaticPage(dynamic parameters)
		{
			// Optimisation SEO (anti duplicate Content).
			// Redirige vers l'url sans "/" de fin
			if(Request.Url.Path.EndsWith("/")) {
				return Response.AsRedirect(Request.Url.Path.TrimEnd('/'), RedirectResponse.RedirectType.Permanent);
			}

			// Get seo title
			var seoTitle = parameters.seoTitle;
			var category = parameters.category;
			
			var model = new PublicSimpleModel (UseConcatenatedResources){};

			// Try to get a page with this seoTitle
			Page page = _PageRepository.GetPage(category + "/" + seoTitle);
			// Request our repository for this page
			if(page == null) {
				page = _PageRepository
				// Get the page from the giving seoTitle
				.GetPage (seoTitle);
			}

			if (page != null) {
				model.HtmlTitle = page.Title;
				model.MetaDescription = page.SeoDescription;
				model.MetaKeywords = page.SeoKeywords.ToString();
				model.Page = page;
			}

			if (!string.IsNullOrEmpty (seoTitle)) {
				var viewPath = Path.Combine (Path.Combine(Path.Combine(Path.Combine(_rootPathProvider.GetRootPath(), "Views"), "Public"), "Pages"), seoTitle + ".cshtml");
				if (File.Exists (viewPath)) {
					return this.View ["Public/Pages/" + seoTitle + ".cshtml", model];
				}

				if (!string.IsNullOrEmpty (category))
				{
					viewPath = Path.Combine(Path.Combine(Path.Combine(Path.Combine(Path.Combine(_rootPathProvider.GetRootPath(), "Views"), "Public"), "Pages"), category), seoTitle + ".cshtml");
					if(File.Exists (viewPath)) {
						return this.View ["Public/Pages/" + category + "/" + seoTitle + ".cshtml", model];
					}
				}
			}

			return this.View["Public/Errors/404.cshtml", model];
		}

		public Object GetRobotTxt(dynamic parameters)
		{
			return Response.AsText("Sitemap: http://www.fibresud.org/sitemap.xml");
		}

		public Object GetSitemap(dynamic parameters)
		{
			var _model = new SitemapModel();
			var _urls = new List<SitemapURL>();
			
			/* Accueil */
			_urls.Add(new SitemapURL(){Loc = "http://www.fibresud.org", LastMod = "2013-04-19", ChangeFreq = ChangeFreq.daily, Priority = "1.0"});

			/* Rubrique: ACTUALITES */
			_urls.Add(new SitemapURL(){Loc = "http://www.fibresud.org/actualites/actualites", LastMod = "2013-04-19", ChangeFreq = ChangeFreq.daily, Priority = "0.8"});
			_urls.Add(new SitemapURL(){Loc = "http://www.fibresud.org/actualites/agenda", LastMod = "2013-04-19", ChangeFreq = ChangeFreq.daily, Priority = "0.8"});

			/* Autres pages statiques */
			_urls.Add(new SitemapURL(){Loc = "http://www.fibresud.org/adherents", LastMod = "2013-04-01", ChangeFreq = ChangeFreq.monthly, Priority = "0.5"});
			_urls.Add(new SitemapURL(){Loc = "http://www.fibresud.org/contact", LastMod = "2013-04-01", ChangeFreq = ChangeFreq.monthly, Priority = "0.5"});

			/* Pages dynamiques: Pages */
			var pages = this._PageRepository.QueryPublished();
			foreach(var page in pages)
			{
				_urls.Add(new SitemapURL(){
					Loc = "http://www.fibresud.org/" + page.FriendlyUrl ?? page.SeoTitle,
					LastMod = page.ModificationDate != default(DateTime) ? page.ModificationDate.ToString("yyyy-MM-dd") : DateTime.Today.ToString("yyyy-MM-dd"),
					ChangeFreq = ChangeFreq.monthly,
					Priority = "0.6"
				});
			};
			
			/* Pages dynamiques: Articles */
			var articles = this._ArticleRepository.QueryPublished();
			foreach(var article in articles)
			{
				_urls.Add(new SitemapURL(){
					Loc = "http://www.fibresud.org/article/" + article.FriendlyUrl ?? article.SeoTitle,
					LastMod = article.PublicationDate != null ? article.PublicationDate.Value.ToString("yyyy-MM-dd") : DateTime.Today.ToString("yyyy-MM-dd"),
					ChangeFreq = ChangeFreq.weekly,
					Priority = "0.7"
				});
			};
			
			/* Attribuer les urls au modèle */
			_model.Urls = _urls;
			
			return this.View["Sitemap.cshtml", _model].WithContentType("text/xml");
		}		

		public Object GetRSSFeed(dynamic parameters)
		{
			var _model = new RSSModel();
			var _items = new List<FeedItem>();

			/* Pages dynamiques: Pages */
			var pages = this._PageRepository.QueryPublished();
			foreach(var page in pages)
			{
				_items.Add(new FeedItem(){
					Title = page.Title,
					Link = "http://www.fibresud.org/" + page.FriendlyUrl ?? page.SeoTitle,
					RSSGuid = "http://www.fibresud.org/" + page.FriendlyUrl ?? page.SeoTitle,
					PublicationDate = page.ModificationDate != default(DateTime) ? page.ModificationDate.ToString("ddd, dd MMM yyyy HH:mm:ss K") : DateTime.Now.ToString("ddd, dd MMM yyyy HH:mm:ss K"),
					Description = ""
				});
			};
			
			/* Pages dynamiques: Articles */
			var articles = this._ArticleRepository.QueryPublished();
			foreach(var article in articles)
			{
				_items.Add(new FeedItem(){
					Title = article.Title,
					Link = "http://www.fibresud.org/article/" + article.FriendlyUrl ?? article.SeoTitle,
					RSSGuid = "http://www.fibresud.org/article/" + article.FriendlyUrl ?? article.SeoTitle,
					PublicationDate = article.PublicationDate != null ? article.PublicationDate.Value.ToString("ddd, dd MMM yyyy HH:mm:ss K") : DateTime.Now.ToString("ddd, dd MMM yyyy HH:mm:ss K"),
					Description = article.Description,
					ImageUrl = "http://www.fibresud.org/Upload/images/actualites/" + article.ImageUrl
				});
			};
			
			/* Attribuer les urls au modèle */
			_model.Items = _items.OrderByDescending(item => item.PublicationDate).ToList();
			
			return this.View["Public/RSS.cshtml", _model].WithContentType("text/xml");
		}
	}
}
