namespace GMATechProject.Web.Admin
{
	#region Using Directives
	
	using System;
	using System.Linq;
	
	using Nancy;
	using Nancy.ModelBinding;
	
	using GMATechProject.Domain;
	using GMATechProject.Domain.Progress;
	
	#endregion
	
	/// <summary>
	/// Module responsible for progress management.
	/// </summary> 
	public class ProgressModule : 
		// AgendaModule is an EntityModule which manage ProgressStep entities with an IProgressStepRepository
		EntityModule<ProgressStep, ProgressRepository>
	{
		/// <summary>
		/// Initializes a new instance of ProgressModule.
		/// </summary>
		/// <param name="repository">We will receive an instance of the IProgressStepRepository by Nancy.TinyIOC container.</param>
		public ProgressModule (IApplication application, ProgressRepository repository) : base(application, repository)
		{
		}

		/// <summary>
		/// Overrides the module base path. This module will targets urls like "/admin/progress" ("/admin" is defined in BaseAdminModule)
		/// </summary>
		protected override string BaseModulePath {
			get { return "/progress"; }
		}
		
		protected override ProgressStep InnerCreateEntity (ProgressStep entity)
		{
			return this.Application.CreateProgressStep(entity);
		}
		
		protected override ProgressStep InnerUpdateEntity (ProgressStep entity)
		{
			Console.WriteLine ("Module - Entity to update: " + entity);
			
			return this.Application.UpdateProgressStep(entity);
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
			var progressSteps = (from progressStep in this.QueryAll() select progressStep)
				// Order them by date, the more recent first
				.OrderByDescending (progressStep => progressStep.CreationDate)			
				// Just the specified page
				.ToPaginatedList(pageIndex, pageSize);

			// Return a successfull action result with the entities
			result = Response.AsJson(ActionResult.AsSuccess(progressSteps));
			
			return result;
		}
	}
}
