namespace GMATechProject.Domain.Security
{
	#region Using Directives
	
	using System;
	using System.Collections.Generic;
	using System.Linq;

	#endregion
	
	public interface IRoleTagsBindingRepository : IRepository<RoleTagsBinding>
	{
		/// <summary>
		/// Lists the tags for the specified role.
		/// </summary>
		/// <returns>
		/// The tags for the specified role.
		/// </returns>
		/// <param name='role'>
		/// The Role.
		/// </param>
		HashSet<string> ListTagsForRole(Roles role);
				
		/// <summary>
		/// Lists the roles for the specified tag.
		/// </summary>
		/// <returns>
		/// The roles for the specified tag.
		/// </returns>
		/// <param name='tag'>
		/// The Tag.
		/// </param>
		IList<Roles> ListRolesForTag(string tag);

		HashSet<string> ListTagsForbiddenForRole(Roles role);

		IQueryable<RoleTagsBinding> QueryByRole(Roles role);

		IQueryable<RoleTagsBinding> QueryByTag(string tag);
	}
}

