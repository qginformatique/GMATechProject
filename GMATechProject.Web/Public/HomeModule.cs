namespace GMATechProject.Web
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Web;

	using GMATechProject.Domain;
	using GMATechProject.Domain.Blog;
	using GMATechProject.Web.Public;
	using GMATechProject.Web.Public.Models;

	using Nancy;
	using Nancy.ModelBinding;
	using Nancy.Responses;

	using NLog;
	
	#endregion
	
	public class HomeModule : BasePublicModule
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

		public HomeModule(IApplication application, IArticleRepository articleRepository, IRootPathProvider rootPathProvider) : base(application)
        {
			this._ArticleRepository = articleRepository;
			this._RootPathProvider = rootPathProvider;

			// Render home page = public view
			Get["/"] = Public;
        }

		public Object Public(dynamic parameters)
		{
			// Optimisation SEO (anti duplicate Content)
			if(Request.Url.Query == "?section=home") {
				return Response.AsRedirect("/", RedirectResponse.RedirectType.Permanent);
			}

			var _homeArticles = _ArticleRepository.ListHomeArticles(0, 6, false, null);

			var model = new HomeModel (UseConcatenatedResources){
				Articles = _homeArticles
			};
			
			return this.View["Public/Home.cshtml", model];
		}
	}
}