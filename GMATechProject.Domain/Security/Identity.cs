namespace GMATechProject.Domain.Security
{
	#region Using Directives
	
	using System;
	using System.Collections.Generic;
	
	using GMATechProject.Domain.Plumbing;
	
	#endregion
	
	/// <summary>
	/// Description of Identity.
	/// </summary>
	[CollectionName("Identity")]
	public class Identity : Entity
	{
		public Identity ()
		{
			this.Role = Roles.User;
			this.CreationDate = DateTime.Today;
		}

		public string ExternalIdentifier { get; set; }

		public string Email { get; set; }
		
		public string Password { get; set; }
		
		public DateTime CreationDate { get; set; }
		
		public Roles Role { get; set; }
		
		public bool EmailConfirmed { get; set; }
		
		public override string ToString ()
		{
			return string.Format ("[Identity: Email={0}, Password={1}, CreationDate={2}, Role={3}, EmailConfirmed={4}]", Email, Password, CreationDate, Role, EmailConfirmed);
		}
	}
}
