namespace GMATechProject.Web.Admin
{
	#region Using Directives
	
	using System;
	
	using GMATechProject.Domain;
	using GMATechProject.Domain.Security;
	
	using Nancy;
	using Nancy.ModelBinding;
	using Nancy.Validation;
	
	using FluentValidation;
	using FluentValidation.Validators;
	
	#endregion
	
	/// <summary>
	/// Module responsible for security management.
	/// </summary> 
	public class SecurityModule : 
		// SecurityModule is an EntityModule which manage Identity entities with an IIdentityRepository
		SimpleEntityModule<Identity, IIdentityRepository>
	{
		/// <summary>
		/// Initializes a new instance of SecurityModule.
		/// </summary>
		/// <param name="repository">We will receive an instance of the IIdentityRepository by Nancy.TinyIOC container.</param>
		public SecurityModule (IApplication application, IIdentityRepository repository) : base(application, repository)
		{
			// Bind the HTTP Put verb to the NewEntity method
			//this.Put [this.BaseModulePath + "/"] = CreateNewEntity;
			this.Post [this.BaseModulePath + "/"] = CreateNewEntity;
			
			// Bind the HTTP POST verb to the NewEntity method
			this.Post [this.BaseModulePath + "/UpdateEmail"] = UpdateEmail;
		}
		
		/// <summary>
		/// Overrides the module base path. This module will targets urls like "/admin/security" ("/admin" is defined in BaseAdminModule)
		/// </summary>
		protected override string BaseModulePath {
			get { return "/security"; }
		}
		
		private Response CreateNewEntity (dynamic parameters)
		{
			Response result = null;
			
			var identityParameters = this.Bind<NewIdentityParameters> ();

			this.Logger.Debug("NewIdentityParameters supplied : {0}", identityParameters != null);
			
			if (identityParameters != null) {
				// Create the identity
				var identity = this.Application.CreateIdentity(identityParameters.Email, null, identityParameters.Role);

				this.Logger.Debug("Identity created : {0}", identity != null);

				if (identity != null) {
					result = this.Response.AsJson (ActionResult.AsSuccess (identity, "L'utilisateur a été ajouté pour l'adresse " + identityParameters.Email + ". Un email lui a été envoyé afin qu'il puisse définir son mot de passe."));
				} else {
					result = this.Response.AsJson (ActionResult.AsGenericError());
				}
			} else {
				result = this.Response.AsJson (ActionResult.AsGenericError());			
			}
			
			return result;
		}

		private Response UpdateEmail (dynamic arg)
		{
			Response result = null;
			
			var identityParameters = this.Bind<UpdateEmailParameters> ();
			
			if (identityParameters != null) {
				// Create the identity
				var identity = this.Repository.GetById(identityParameters.Id);
				
				if (identity != null) {					
					result = this.Response.AsJson (ActionResult.AsSuccess (
						identity,
						"L'utilisateur a été ajouté pour l'adresse " + identityParameters.Email + ". Un email lui a été envoyé afin qu'il puisse définir son mot de passe."
					));
				}
			} else {
				result = this.Response.AsJson (ActionResult.AsGenericError());			
			}
			
			return result;
		}
	}
	
	public class NewIdentityParameters
	{
		public string Email { get; set; }
		public Roles Role { get; set; }
	}
	
	public class NewIdentityParametersValidator: AbstractValidator<NewIdentityParameters>
	{
		public NewIdentityParametersValidator ()
		{
			this.RuleFor (p => p.Email)
				.NotEmpty ()
				.EmailAddress ()
				.WithMessage ("Vous devez spécifier une adresse email valide.");
		}	
	}
	
	public class UpdateEmailParameters
	{
		public string Email { get; set; }
		public string Id { get; set; }
	}
	
	public class UpdateEmailParametersValidator: AbstractValidator<UpdateEmailParameters>
	{
		public UpdateEmailParametersValidator ()
		{
			this.RuleFor (p => p.Id)
				.NotEmpty ()
				.WithMessage ("Vous devez sélectionner un utilisateur.");
			
			this.RuleFor (p => p.Email)
				.NotEmpty ()
				.EmailAddress ()
				.WithMessage ("Vous devez spécifier une adresse email valide.");
		}	
	}	
}
