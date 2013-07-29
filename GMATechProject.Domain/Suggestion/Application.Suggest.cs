namespace GMATechProject.Domain
{
	#region Using Directives
	
	using System;
	using System.Collections.Generic;
	using System.Linq;
	
	using GMATechProject.Domain.Suggestion;
	using GMATechProject.Domain.Security;
	
	using FluentValidation;

	using MongoDB.Driver.Linq;
	
	#endregion

	public partial class Application : IApplication
	{
		private readonly ISuggestRepository _SuggestRepository;

		public Suggest CreateSuggest (Suggest suggest)
		{
			var result = this.CreateEntity<Suggest>(suggest, this._SuggestRepository);

			if (result.CreationDate == null || result.CreationDate == DateTime.MinValue) {
				result.CreationDate = DateTime.Now;
			}

			result.ModificationDate = DateTime.Now;

			result.Tags = new HashSet<string>();

			return result;
		}
		
		public Suggest UpdateSuggest (Suggest suggest)
		{
			if ( suggest.CreationDate == null || suggest.CreationDate == DateTime.MinValue) {
				suggest.CreationDate = DateTime.Now;
			}

			suggest.ModificationDate = DateTime.Now;

			return this.UpdateEntity(suggest, this._SuggestRepository);
		}

		public PaginatedList<Suggest> ListPublishedSuggest (int pageIndex = 0, int pageSize = 10, Roles role = Roles.None, string[] tags = null)
		{
			try
			{
				var tagsForbiddenForRole = this._RoleTagsBindingRepository.ListTagsForbiddenForRole(role);
				
				this._Logger.Debug("ListPublishedSuggest: role = " + role);
				this._Logger.Debug("ListPublishedSuggest: tags forbidden for role = " + string.Join(", ", tagsForbiddenForRole));
				
				var query =
					// We want all the enabled progressStep
					(from entity in this._SuggestRepository.QueryPublished (tags)
					 select entity);
				
				// If our user is not an administrator
				if(role != Roles.Administrator)
				{
					query = 
						(from entity in query
						 // Filter: all events without any tags
						 where (!entity.Tags.Any ())
						 // Or: @events who have all their tags bound to this role
						 || (!entity.Tags.ContainsAny(tagsForbiddenForRole))
						 select entity);
				}
				
				var result = query
					.OrderByDescending (entity => entity.CreationDate)
						.ToPaginatedList (pageIndex, pageSize);
				
				return result;
			}
			catch(Exception e){
				// Log exception
				_Logger.LogException(NLog.LogLevel.Error, e.Message, e);
			}
			return null;
		}
	}
}