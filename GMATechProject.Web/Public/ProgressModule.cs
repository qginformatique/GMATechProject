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
	using GMATechProject.Domain.Progress;
	using GMATechProject.Web.Public.Models;
	
	#endregion

	/// <summary>
	/// The module responsible for the progress management.
	/// </summary>
	public class ProgressModule : BaseModule
	{
		#region Fields
		
		private readonly IProgressRepository _Repository;
									
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets the entity's repository.
		/// </summary>
		protected IProgressRepository Repository
		{
			get 
			{
				return this._Repository;
			}
		}

		#endregion
				
		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ProgressModule" />.
		/// </summary>
		/// <param name="repository">The member's repository</param>
		public ProgressModule (IApplication application, IProgressRepository repository) : base(application)
		{			
			this._Repository = repository;

			// Define a route for urls "/members/{id}" which will returns the member matching the specified slug
			this.Get["/{id}"] = parameters => 
			{
				return GetMember(parameters.id);
			};

			// Bind the HTTP GET verb to the ListMembers method
			this.Get["/progression"] = ListPublicProgressSteps;
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

		protected Object ListPublicProgressSteps (dynamic parameters)
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

			// Request our repository for those articles
			var progressSteps = this.Application
				// Just the specified page from the last published articles
				.ListPublishedProgressStep(pageIndex, pageSize, this.CurrentUser.Role, null);

			var result = new List<ProgressStepExtended>();
			foreach (var item in progressSteps.Items.Where (x => x.Tags.Count == 0)) {
				var progressStepExtended = new ProgressStepExtended ();

				progressStepExtended.CreationDate = item.CreationDate;
				progressStepExtended.CurrentProgress = item.CurrentProgress;
				progressStepExtended.Description = item.Description;
				progressStepExtended.FriendlyUrl = item.FriendlyUrl;
				progressStepExtended.Id = item.Id;
				progressStepExtended.ModificationDate = item.ModificationDate;
				progressStepExtended.Notes = item.Notes;
				progressStepExtended.PublicationState = item.PublicationState;
				progressStepExtended.Tags = item.Tags;
				progressStepExtended.Title = item.Title;

				progressStepExtended.Items = progressSteps.Items.Where(x => x.Tags.Contains(progressStepExtended.Title.ToLower())).ToList();

				result.Add (progressStepExtended);
			};

			var model = new ProgressStepsModel (UseConcatenatedResources) {
				ProgressSteps = result
			};

			model.NavMenu.NavSectionsMenuLeft[1].IsActive = "active";

			return this.View["Public/Pages/progression.cshtml", model];
		}
		
		#endregion
	}
}