namespace GMATechProject.Web.Public
{
	#region Using Directives

	using System;
	
	using Nancy;
	using Nancy.ModelBinding;
	using Nancy.Validation;
	
	using FluentValidation;
	using FluentValidation.Validators;
	
	using GMATechProject.Domain;
	using GMATechProject.Domain.Members;
	
	#endregion

	/// <summary>
	/// The module responsible for the members management.
	/// </summary>
	public class MemberModule : BaseModule
	{
		#region Fields
		
		private readonly IMemberRepository _Repository;
					
		private const string _ModulePath = "/public/members";
				
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets the entity's repository.
		/// </summary>
		protected IMemberRepository Repository
		{
			get 
			{
				return this._Repository;
			}
		}

		#endregion
				
		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the <see cref="MemberModule" />.
		/// </summary>
		/// <param name="repository">The member's repository</param>
		public MemberModule (IApplication application, IMemberRepository repository) : base(application, _ModulePath)
		{			
			this._Repository = repository;

			// Define a route for urls "/members/{id}" which will returns the member matching the specified slug
			this.Get["/{id}"] = parameters => 
			{
				return GetMember(parameters.id);
			};
			         
			// Bind the HTTP GET verb to the ListMembers method
			this.Get[""] = ListMembers;
        }		

		#endregion
		
		#region Protected Methods

		protected virtual Response GetMember(dynamic parameters)
		{
			Response result = null;
			
			// Get id
			var id = parameters.id;
			
			// Request our repository for this member
			var member = this.Repository.GetById(id);
			
			// Return a successfull action result with the events
			result = FormatterExtensions.AsJson(Response, ActionResult.AsSuccess(member));
			
			
			return result;
		}

		protected virtual Response ListMembers (dynamic parameters)
		{
			Response result = null;
			
			// Try to bind that request parameters (querystring and/or form post) to a PaginationRequestParameters class
			var paginationParameters = this.Bind<PaginationRequestParameters> ();
			
			// Default values for pageIndex and pageSize
			var pageIndex = 0;
			var pageSize = 8;
			
			// If we got some parameters
			if (paginationParameters != null) {
				// Get the pageIndex from them
				pageIndex = paginationParameters.PageIndex;
				
				// Get the pageSize from them
				pageSize = paginationParameters.PageSize;
			}
			
			// Request our repository for those members
			var members = this.Application
				// Just the specified page from the last published members
				.ListMembers(pageIndex, pageSize);
			
			// Return a successfull action result with the members
			result = Response.AsJson(ActionResult.AsSuccess(members));

			return result;
		}
		
		#endregion
	}
}