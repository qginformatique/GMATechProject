namespace GMATechProject.Domain.Security
{
	#region Using Directives
	
	using System;
	using System.Collections.Generic;
	using System.Linq;
	
	using GMATechProject.Domain.Plumbing;

	#endregion
	
	public class RoleTagsBindingRepository : Repository<RoleTagsBinding>, IRoleTagsBindingRepository
	{
		public RoleTagsBindingRepository ()
		{
		}

		public override PaginatedList<RoleTagsBinding> QuickSearch (string queryTerms, int pageIndex, int pageSize)
		{
			var query = 
				(from rta in this.All ()
				 where rta.Tags.Any(t => t.Contains(queryTerms))
				 select rta);

			var result = query.ToPaginatedList (pageIndex, pageSize);
			
			return result;
		}
				
		public HashSet<string> ListTagsForRole (Roles role)
		{
			return this
				.QueryByRole(role)
				.Select(rta => rta.Tags)
				.FirstOrDefault ();
		}

		public IList<Roles> ListRolesForTag (string tag)
		{
			return this
				.QueryByTag(tag)
				.ToList ()
				.Select(rta => rta.Role)
				.ToList();
		}
				
		public IQueryable<RoleTagsBinding> QueryByRole (Roles role)
		{
			var query = 
				(from rta in this.All ()
				 where rta.Role == role
				 select rta);
			
			return query;			
		}
		
		public HashSet<string> ListTagsForbiddenForRole (Roles role)
		{
			var result = new HashSet<string>();

			var bindings = this.All().ToList();

			var bindingForRole = bindings.FirstOrDefault(b => b.Role == role);
			var bindingsForHigherRoles = bindings.Where(b => b.Role > role);

			if(bindingForRole != null)
			{
				var tagsBoundToRole = bindingForRole.Tags;
				var tagsBoundToHigherRoles = bindingsForHigherRoles.Select(b => b.Tags);

				foreach(var tags in tagsBoundToHigherRoles)
				{
					var tagsNotBoundToCurrentRole = tags.Where(tag => !tagsBoundToRole.Contains(tag));

					result.UnionWith(tagsNotBoundToCurrentRole);
				}
			}

			return result;			
		}
		
		public IQueryable<RoleTagsBinding> QueryByTag (string tag)
		{
			var query = 
				(from rta in this.All ()
				 where rta.Tags.Any (t => t == tag)
				 select rta);
			
			return query;			
		}
		
	}
}

