using System.Collections.Generic;

namespace GMATechProject.Domain.Progress
{
	#region Using Directives
	
	using System;
	using System.Linq;
	
	using GMATechProject.Domain.Plumbing;

	#endregion 
	
	/// <summary>
	/// Description of ProgressRepositoryMongo.
	/// </summary>
	public class ProgressRepository : Repository<ProgressStep>, IProgressRepository
	{
		public ProgressStep GetProgressStep (string seoTitle)
		{
			ProgressStep result = null;

			// TODO: add verification (like seoTitle not null) otherwise return error			
			var query = 
				// From all our events
				(from progressStep in this.All ()
				 // Filter: we want only the event with the given seoTitle
				 where progressStep.FriendlyUrl == seoTitle || progressStep.Title == seoTitle
				 select progressStep);
			
			// TODO: add verification (for example: check if an article is returned) otherwise return error
			result = query.FirstOrDefault ();
			
			return result;
		}

		public PaginatedList<ProgressStep> ListEnabled (int pageIndex = 0, int pageSize = 10, string[] tags = null)
		{
			var query = 
				// From all our progressStep
				(from progressStep in this.All ()
				 // Filter: we want only the enabled progressStep
				 where progressStep.PublicationState == PublicationState.Published
			 // Filter: which the starting date is today or later
			 //	 where @event.StartDate >= DateTime.Today
				 select progressStep);
			
			// if some tags have been specified
			if (tags != null
				&& tags.Length > 0) {
				// Apply tags filtering
				query = 
					(from progressStep in query
					 where progressStep.Tags.Any (tag => tags.Contains (tag))
					 select progressStep);
			}

			// Get the query's result as a paginated list
			var result = query.ToPaginatedList (pageIndex, pageSize);
			
			return result;
		}
		
		public IQueryable<ProgressStep> QueryPublished (string[] tags = null)
		{
			var query = 
				// From all our progressStep
				(from progressStep in this.All ()
			// Filter: we want only the enabled events
				 where progressStep.PublicationState == PublicationState.Published
			// Filter: which have been published today or sooner
				 select progressStep);
			
			// if some tags have been specified
			if (tags != null
				&& tags.Length > 0) {
				// Apply tags filtering
				query = 
					(from progressStep in query
					 where progressStep.Tags.Any (tag => tags.Contains (tag))
					 select progressStep);
			}
			
			return query;
		}

		public override PaginatedList<ProgressStep> QuickSearch (string queryTerms, int pageIndex, int pageSize)
		{
			// TODO: apply tags filtering
			var query = 
				(from progressStep in this.All ()
				 where progressStep.Title.Contains (queryTerms)
				 select progressStep );
			
			var result = query.ToPaginatedList (pageIndex, pageSize);
			
			return result;
		}
	}
}
