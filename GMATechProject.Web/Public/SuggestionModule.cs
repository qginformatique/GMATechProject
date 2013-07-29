namespace GMATechProject.Web.Public
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Linq;
	
	using Nancy;
	using Nancy.ModelBinding;
	using Nancy.Validation;
	
	using FluentValidation;
	using FluentValidation.Validators;
	
	using GMATechProject.Domain;
	using GMATechProject.Domain.Suggestion;
	using GMATechProject.Web.Public.Models;
	
	#endregion

	/// <summary>
	/// The module responsible for the suggestion management.
	/// </summary>
	public class SuggestionModule : BaseModule
	{
		#region Fields
		
		private readonly ISuggestRepository _Repository;
									
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets the entity's repository.
		/// </summary>
		protected ISuggestRepository Repository
		{
			get 
			{
				return this._Repository;
			}
		}

		#endregion
				
		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the <see cref="SuggestionModule" />.
		/// </summary>
		/// <param name="repository">The suggestion's repository</param>
		public SuggestionModule (IApplication application, ISuggestRepository repository) : base(application)
		{			
			this._Repository = repository;

			// Define a route for urls "/members/{id}" which will returns the member matching the specified slug
			this.Get["/{id}"] = parameters => 
			{
				return GetSuggestion(parameters.id);
			};

			// Bind the HTTP GET verb to the ListMembers method
			this.Get["/suggestions"] = ListPublicSuggestions;
        }		

		#endregion
		
		#region Protected Methods

		protected virtual Response GetSuggestion(dynamic parameters)
		{
			Response result = null;
			
			// Get id
			var id = parameters.id;
			
			// Request our repository for this member
			var entity = this.Repository.GetById(id);
			
			// Return a successfull action result with the events
			result = FormatterExtensions.AsJson(Response, ActionResult.AsSuccess(entity));
			
			
			return result;
		}

		protected Object ListPublicSuggestions (dynamic parameters)
		{
			// Try to bind that request parameters (querystring and/or form post) to a PaginationRequestParameters class
			var _parameters = this.Bind<PaginationRequestParameters> ();

			// Default values for pageIndex and pageSize
			var pageIndex = 0;
			var pageSize = 10;

			// If we got some parameters
			if (_parameters != null) {
				// Get the pageIndex from them
				pageIndex = _parameters.PageIndex;

				// Get the pageSize from them
				pageSize = _parameters.PageSize;
			}

			// Request our repository for those entities
			var entities = this.Application
				// Just the specified page from the last published articles
				.ListPublishedSuggest(pageIndex, pageSize, this.CurrentUser.Role, null);

			var model = new SuggestionModel (UseConcatenatedResources) {
				Suggestions = entities
			};

			model.NavMenu.NavSectionsMenuLeft[2].IsActive = "active";

			return this.View["Public/Pages/suggestions.cshtml", model];
		}
		
		#endregion
	}
}