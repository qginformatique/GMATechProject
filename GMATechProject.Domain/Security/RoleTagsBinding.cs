namespace GMATechProject.Domain.Security
{
	#region Using Directives
	
	using System;
	using System.Collections.Generic;
	
	using MongoDB.Bson.Serialization.Attributes;
	
	#endregion

	[GMATechProject.Domain.Plumbing.CollectionName("RoleTagsBinding")]
	public class RoleTagsBinding : Entity
	{
		public RoleTagsBinding ()
		{
			this.Tags = new HashSet<string> ();
		}

		public Roles Role { get; set; }
		
		public HashSet<string> Tags { get; set; }
	}
}
