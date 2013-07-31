namespace GMATechProject.Web.Admin
{
	#region Using Directives
	
	using System;
	using System.Linq;
	
	using Nancy;
	using Nancy.ModelBinding;
	
	using GMATechProject.Domain;
	using GMATechProject.Domain.Pages;
	
	#endregion
	
	/// <summary>
	/// Module responsible for pages management.
	/// </summary> 
	public class PageModule : 
		// PageModule is an EntityModule which manage Page entities with an IPageRepository
		EntityModule<Page, PageRepository>
	{
		/// <summary>
		/// Initializes a new instance of PageModule.
		/// </summary>
		/// <param name="repository">We will receive an instance of the IPageRepository by Nancy.TinyIOC container.</param>
		public PageModule(IApplication application, PageRepository repository) : base(application, repository)
        {
        }
		
		/// <summary>
		/// Overrides the module base path. This module will targets urls like "/admin/articles" ("/admin" is defined in BaseAdminModule)
		/// </summary>
		protected override string BaseModulePath {
			get { return "/pages"; }
		}

		protected override Response ListEntities(dynamic parameters)
		{
			Response result = null;

			// Try to bind that request parameters (querystring and/or form post) to a PaginationRequestParameters class
			var paginationParameters = this.Bind<PaginationRequestParameters>();
			
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
			
			// Request our repository for those pages
			var pages = (from page in this.QueryAll() select page)
			                // Order them by date, the more recent first
							.OrderByDescending (page => page.ModificationDate)			
			                // Just the specified page
			                .ToPaginatedList(pageIndex, pageSize);
			
			// Return a successfull action result with the entities
			result = Response.AsJson(ActionResult.AsSuccess(pages));
			
			return result;
		}
		
		protected override Page InnerCreateEntity (Page entity)
		{
			return this.Application.CreatePage(entity);
		}
		
		protected override Page InnerUpdateEntity(Page entity)
		{
			return this.Application.UpdatePage(entity);
		}
	}	
}