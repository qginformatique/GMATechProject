namespace GMATechProject.Web.Admin
{
	#region Using Directives
	
	using System;
	using System.Linq;
	
	using GMATechProject.Domain;
	using GMATechProject.Domain.Security;
	
	using Nancy;
	using Nancy.ModelBinding;
	using Nancy.Validation;
	
	using FluentValidation;
	using FluentValidation.Validators;
	
	#endregion

	public class RoleTagsModule : BaseAdminModule
	{
		private readonly IRoleTagsBindingRepository _Repository;

		public RoleTagsModule (IApplication application, IRoleTagsBindingRepository repository) : base(application)
		{
			this._Repository = repository;

			// Bind the HTTP GET verb to the ListEntities method
			this.Get [this.BaseModulePath] = ListEntities;
                        
			// Bind the HTTP PUT verb to the BindTagToRole method
			this.Put [this.BaseModulePath] = BindTagToRole;
			            
			// Bind the HTTP DELETE verb to the UnbindTagFromRole method
			this.Delete[this.BaseModulePath] = UnbindTagFromRole;

		}
		
		/// <summary>
		/// Overrides the module base path. This module will targets urls like "/admin/roleTags" ("/admin" is defined in BaseAdminModule)
		/// </summary>
		protected string BaseModulePath {
			get { return "/roletags"; }
		}

		protected Response ListEntities(dynamic parameters)
		{
			Response result = null;

			// Request our repository for those bindings
			var bindings = this._Repository
				.All()
				.OrderBy(x => x.Role)
				// Just the specified page
				.ToList();

			// Return a successfull action result with the bindings
			result = Response.AsJson(ActionResult.AsSuccess(bindings));
			
			return result;
		}

		protected Response BindTagToRole (dynamic parameters)
		{
			Response result = null;

			// Try to bind that request parameters (querystring and/or form post) to a RoleTagBindingParameters class
			var bindingParameters = this.Bind<RoleTagBindingParameters>();

			if(bindingParameters != null)
			{
				var binding = this.Application.BindTagToRole(bindingParameters.Role, bindingParameters.Tag);

				if(binding != null
				   && binding.Tags.Any(x => x == bindingParameters.Tag))
				{
					// Return a successfull action result with the articles
					result = Response.AsJson(ActionResult.AsSuccess(
						string.Format(
							"Le role {0} est associé au tag {1}", 
						bindingParameters.Role, 
						bindingParameters.Tag)));
				}
				else
				{
					result = Response.AsJson(ActionResult.AsGenericError());
				}
			}
			else
			{
				result = Response.AsJson(ActionResult.AsGenericError());
			}

			return result;
		}

		protected Response UnbindTagFromRole (dynamic parameters)
		{
			Response result = null;

			// Try to bind that request parameters (querystring and/or form post) to a RoleTagBindingParameters class
			var bindingParameters = this.Bind<RoleTagBindingParameters>();

			if(bindingParameters != null)
			{
				var binding = this.Application.UnbindTagFromRole(bindingParameters.Role, bindingParameters.Tag);

				if(binding != null
				   && !binding.Tags.Any(x => x == bindingParameters.Tag))
				{
					// Return a successfull action result with the articles
					result = Response.AsJson(ActionResult.AsSuccess(
						string.Format(
							"Le role {0} n'est plus associé au tag {1}", 
						bindingParameters.Role, 
						bindingParameters.Tag)));
				}
				else
				{
					result = Response.AsJson(ActionResult.AsGenericError());
				}
			}
			else
			{
				result = Response.AsJson(ActionResult.AsGenericError());
			}

			return result;
		}

		public class RoleTagBindingParameters
		{
			public Roles Role { get; set; }
			public string Tag { get; set; }
		}
	}
}

