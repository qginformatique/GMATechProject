namespace GMATechProject.Domain
{
	#region Using Directives
	
	using System;
	using System.Collections.Generic;
	
	using GMATechProject.Domain.Security;
	
	using FluentValidation;
	
	#endregion
	
	public partial interface IApplication
	{			
		Identity CreateIdentity(string email, string password, Roles role);

		Identity FindByExternalIdentifier(Guid identifier);

		Identity FindByEmailAndPassword(string email, string password);

		Identity UpdateIdentityPassword(string id, string newpassword);

		Identity UpdateIdentityEmail(string email, string password, string newEmail);

		Identity UpdateIdentityEmail(string id, string newEmail);
		
		bool ResetIdentityPassword(string email);
		
		RoleTagsBinding BindTagToRole(Roles role, params string[] tags);
		
		RoleTagsBinding UnbindTagFromRole(Roles role, params string[] tags);
	}
}

