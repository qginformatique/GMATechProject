namespace GMATechProject.Web.Admin
{
	#region Using Directives
	
	using System;
	using System.Linq;

	using Nancy;
	using Nancy.ModelBinding;
	
	using GMATechProject.Domain;
	using GMATechProject.Domain.Members;
	
	#endregion
	
	/// <summary>
	/// Module responsible for articles management.
	/// </summary> 
	public class MemberModule : 
		// MemberModule is an EntityModule which manage Member entities with an Member
		EntityModule<Member, MemberRepository>
	{
		/// <summary>
		/// Initializes a new instance of MemberModule.
		/// </summary>
		/// <param name="repository">We will receive an instance of the IMemberRepository by Nancy.TinyIOC container.</param>
		public MemberModule (IApplication application, MemberRepository repository) : base(application, repository)
		{
		}
		
		/// <summary>
		/// Overrides the module base path. This module will targets urls like "/admin/members" ("/admin" is defined in BaseAdminModule)
		/// </summary>
		protected override string BaseModulePath {
			get { return "/members"; }
		}
		
		protected override Member InnerCreateEntity (Member entity)
		{
			return this.Application.CreateMember(entity);
		}
		
		protected override Member InnerUpdateEntity (Member entity)
		{
			return this.Application.UpdateMember(entity);
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
			
			// Request our repository for those members
			var members = (from member in this.QueryAll() select member)
				// Order them by date, the more recent first
				.OrderBy (member => member.Name)			
					// Just the specified page
					.ToPaginatedList(pageIndex, pageSize);
			
			// Return a successfull action result with the entities
			result = Response.AsJson(ActionResult.AsSuccess(members));
			
			return result;
		}
	}
}