namespace GMATechProject.Domain
{
	#region Using Directives
	
	using System;
	using System.Collections.Generic;
	using System.Linq;
	
	using GMATechProject.Domain.Progress;
	using GMATechProject.Domain.Security;
	
	using FluentValidation;

	using MongoDB.Driver.Linq;
	
	#endregion

	public partial class Application : IApplication
	{
		private readonly IProgressRepository _ProgressRepository;

		public ProgressStep CreateProgressStep (ProgressStep progressStep)
		{
			var result = this.CreateEntity<ProgressStep>(progressStep, this._ProgressRepository);

			if (result.CreationDate == null || result.CreationDate == DateTime.MinValue) {
				result.CreationDate = DateTime.Now;
			}

			result.ModificationDate = DateTime.Now;

			result.Tags = new HashSet<string>();

			return result;
		}
		
		public ProgressStep UpdateProgressStep (ProgressStep progressStep)
		{
			if ( progressStep.CreationDate == null || progressStep.CreationDate == DateTime.MinValue) {
				progressStep.CreationDate = DateTime.Now;
			}

			progressStep.ModificationDate = DateTime.Now;

			return this.UpdateEntity(progressStep, this._ProgressRepository);
		}

		public PaginatedList<ProgressStep> ListPublishedProgressStep (int pageIndex = 0, int pageSize = 10, Roles role = Roles.None, string[] tags = null)
		{
			try
			{
				var tagsForbiddenForRole = this._RoleTagsBindingRepository.ListTagsForbiddenForRole(role);
				
				this._Logger.Debug("ListPublishedProgressStep: role = " + role);
				this._Logger.Debug("ListPublishedProgressStep: tags forbidden for role = " + string.Join(", ", tagsForbiddenForRole));
				
				var query =
					// We want all the enabled progressStep
					(from progressStep in this._ProgressRepository.QueryPublished (tags)
					 select progressStep);
				
				// If our user is not an administrator
				if(role != Roles.Administrator)
				{
					query = 
						(from progressStep in query
						 // Filter: all events without any tags
						 where (!progressStep.Tags.Any ())
						 // Or: @events who have all their tags bound to this role
						 || (!progressStep.Tags.ContainsAny(tagsForbiddenForRole))
						 select progressStep);
				}
				
				var result = query
					.OrderByDescending (progressStep => progressStep.CreationDate)
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