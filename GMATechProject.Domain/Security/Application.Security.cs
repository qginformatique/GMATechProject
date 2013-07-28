namespace GMATechProject.Domain
{
	#region Using Directives
	
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;
	using System.Security.Cryptography;
	using System.Text;

	using GMATechProject.Domain.Security;
	using GMATechProject.Domain.Mailing;
	
	using FluentValidation;
	
	#endregion

	public partial class Application : IApplication
	{
		#region Fields
		
		private readonly IIdentityRepository _IdentityRepository;
		private readonly IRoleTagsBindingRepository _RoleTagsBindingRepository;
		private readonly IMailTemplateRepository _MailTemplateRepository;
		private readonly IPasswordUtility _PasswordUtility;
		
		#endregion
		
		#region Identities 

		public Identity FindByExternalIdentifier(Guid identifier){
			this._Logger.Debug("Looking for : {0}", identifier);
			
			var query = (from identity in this._IdentityRepository.All()
			             where identity.ExternalIdentifier == identifier.ToString()
			             select identity);

			return query.FirstOrDefault();
		}
		
		public Identity FindByEmailAndPassword(string email, string password)
		{
			return this._IdentityRepository.FindByEmailAndPassword(email, password);
		}

		public Identity CreateIdentity (string email, string password, Roles role = Roles.User)
		{
			Identity result = null;

			var identity = new Identity ()
			{
				ExternalIdentifier = Guid.NewGuid().ToString(),
				Email = email, 
				Password = this._PasswordUtility.HashPassword(password ?? this._PasswordUtility.Generate ()), 
				Role = role
			};
			
			result = this._IdentityRepository.Add (identity);

			this._Logger.Debug("Identity created : {0}", result != null);

			if (result != null) {
				try
				{
					var configuration = ConfigurationHelper.Get<ApplicationConfiguration> ();
					
					if (configuration != null) {
						this.SendIdentityEmail (identity, "AccountActivation");
					}
				}
				catch(Exception exception)
				{
					this._Logger.ErrorException("Erreur lors de l'envoi du mail d'activation à " + email + " : " + exception.ToString(), exception);
				}
			}
			
			return result;
		}

		public Identity UpdateIdentityPassword (string id, string newPassword)
		{
			// Try to find an identity for this email/password
			var result = this._IdentityRepository.GetById (id);
			
			// If identity found
			if (result != null) {
				// Update its password
				result.Password = this._PasswordUtility.HashPassword (newPassword);
				
				// Persist the changes
				result = this._IdentityRepository.Update (result);
			}
			
			return result;
		}

		public Identity UpdateIdentityEmail (string email, string password, string newEmail)
		{
			// Try to find an identity for this email/password
			var result = this._IdentityRepository.FindByEmailAndPassword (email, password);
			
			// If identity found
			if (result != null) {
				result = this.UpdateIdentityEmail(result, newEmail);
			}
			
			return result;
		}
			
		public Identity UpdateIdentityEmail (string id, string newEmail)
		{
			// Try to load the identity
			var result = this._IdentityRepository.GetById (id);
			
			// If identity found
			if (result != null) {
				result = this.UpdateIdentityEmail (result, newEmail);
			}
			
			return result;
		}
		
		public Identity ValidateIdentityEmail (string email)
		{
			// Try to load the identity
			var result = this._IdentityRepository.GetById (email);
			
			// If identity found
			if (result != null) {
				result.EmailConfirmed = false;
				
				// Persist the changes
				result = this._IdentityRepository.Update (result);				
			}
			
			return result;			
		}
		
		public bool ResetIdentityPassword (string email)
		{
			var result = false;
			
			var identity = this._IdentityRepository.FindByEmail (email);
			
			if(identity != null)
			{
				this.SendIdentityEmail (identity, "AccountPasswordReset");
			}
			
			return result;
		}
		
		private Identity UpdateIdentityEmail (Identity identity, string newEmail)
		{
			// Try to load the identity
			Identity result = null;
			
			// If identity supplied
			if (identity != null) {
				// Update its email
				identity.Email = newEmail;
				identity.EmailConfirmed = false;
				
				// Persist the changes
				result = this._IdentityRepository.Update (identity);				
				
				if (result != null) {
					this.SendIdentityEmail (identity, "AccountActivationEmailChanged");
				}								
			}
			
			return result;
		}
		
		private void SendIdentityEmail (Identity identity, string mailTemplateId)
		{
			var configuration = ConfigurationHelper.Get<ApplicationConfiguration> ();

			if (configuration != null) {
				var template = this._MailTemplateRepository.GetByName (mailTemplateId);

				if (template != null) {
					FluentEmail
						//.From (configuration.AddressForEmailFromSystem, configuration.NameForEmailFromSystem)
						.FromDefault()
						.To (identity.Email)
						.UsingTemplate (template, identity)
						.Send ();
				} else {
					throw new Exception ("Could not find the mail template with code \"" + mailTemplateId + "\"");
				}
			}
		}
		
		#endregion
		
		#region Role/Tags associations
				
		public RoleTagsBinding BindTagToRole (Roles role, params string[] tags)
		{
			RoleTagsBinding result = null;
			
			// Try to find an existing binding for this role
			var entity = this._RoleTagsBindingRepository.QueryByRole (role).FirstOrDefault ();
			
			// If no binding already exists
			if (entity == null) {
				// Create one
				entity = new RoleTagsBinding ();
				entity.Role = role;
				
				// Add the tags
				foreach (var tag in tags) {
					entity.Tags.Add (tag);
				}
				
				result = this._RoleTagsBindingRepository.Add (entity);
			} else {
				var modified = false;
				
				foreach (var tag in tags) {
					// If this tag is not already bound to this role
					if (!entity.Tags.Contains (tag)) {
						// Add it
						entity.Tags.Add (tag);
						
						// Flag that we modified this binding
						modified = true;
					}
				}
				
				// If we modified the binding
				if (modified) {
					// Persists the changes
					result = this._RoleTagsBindingRepository.Update (entity);
				} else {
					result = entity;
				}
			}
			
			return result;
		}
		
		public RoleTagsBinding UnbindTagFromRole (Roles role, params string[] tags)
		{
			RoleTagsBinding result = null;
			
			// Try to find an existing binding for this role
			var entity = this._RoleTagsBindingRepository.QueryByRole (role).FirstOrDefault ();
			
			if (entity != null) {
				var modified = false;
				
				foreach (var tag in tags) {
					// If this tag is bound to this role
					if (entity.Tags.Contains (tag)) {
						// Remove it
						entity.Tags.Remove (tag);
						
						// Flag that we modified this binding
						modified = true;
					}
				}
				
				// If we modified the binding
				if (modified) {
					// Persists the changes
					result = this._RoleTagsBindingRepository.Update (entity);
				} else {
					result = entity;
				}
			}
			
			return result;
		}

		#endregion		
	}
}

