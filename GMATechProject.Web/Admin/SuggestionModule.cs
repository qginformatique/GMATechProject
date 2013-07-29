namespace GMATechProject.Web.Admin
{
	#region Using Directives
	
	using System;
	using System.Linq;
	
	using Nancy;
	using Nancy.ModelBinding;
	
	using GMATechProject.Domain;
	using GMATechProject.Domain.Suggestion;
	
	#endregion
	
	/// <summary>
	/// Module responsible for suggestion management.
	/// </summary> 
	public class SuggestionModule : 
		// AgendaModule is an EntityModule which manage Suggest entities with an ISuggestionRepository
		EntityModule<Suggest, SuggestRepository>
	{
		/// <summary>
		/// Initializes a new instance of ProgressModule.
		/// </summary>
		/// <param name="repository">We will receive an instance of the ISuggestRepository by Nancy.TinyIOC container.</param>
		public SuggestionModule (IApplication application, SuggestRepository repository) : base(application, repository)
		{
		}

		/// <summary>
		/// Overrides the module base path. This module will targets urls like "/admin/suggestion" ("/admin" is defined in BaseAdminModule)
		/// </summary>
		protected override string BaseModulePath {
			get { return "/suggestion"; }
		}
		
		protected override Suggest InnerCreateEntity (Suggest entity)
		{
			return this.Application.CreateSuggest(entity);
		}
		
		protected override Suggest InnerUpdateEntity (Suggest entity)
		{
			Console.WriteLine ("Module - Entity to update: " + entity);
			
			return this.Application.UpdateSuggest(entity);
		}

		protected override Response ListEntities(dynamic parameters)
		{
			Response result = null;
			
			// Try to bind that request parameters (querystring and/or form post) to a PaginationRequestParameters class
			var paginationRequestParameters = this.Bind<PaginationRequestParameters>();
			
			// Default values for pageIndex and pageSize
			var pageIndex = 0;
			var pageSize = 10;
			
			// If we got some parameters
			if(paginationRequestParameters != null)
			{
				// Get the pageIndex from them
				pageIndex = paginationRequestParameters.PageIndex;
				
				// Get the pageSize from them
				pageSize = paginationRequestParameters.PageSize;
			}

			// Request our repository for those progressStep
			var entities = (from entity in this.QueryAll() select entity)
				// Order them by date, the more recent first
				.OrderByDescending (entity => entity.CreationDate)			
				// Just the specified page
				.ToPaginatedList(pageIndex, pageSize);

			// Return a successfull action result with the entities
			result = Response.AsJson(ActionResult.AsSuccess(entities));
			
			return result;
		}
	}
}
