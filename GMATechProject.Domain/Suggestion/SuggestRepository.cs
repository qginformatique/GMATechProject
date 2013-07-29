using System.Collections.Generic;

namespace GMATechProject.Domain.Suggestion
{
	#region Using Directives
	
	using System;
	using System.Linq;
	
	using GMATechProject.Domain.Plumbing;

	#endregion 
	
	/// <summary>
	/// Description of SuggestRepositoryMongo.
	/// </summary>
	public class SuggestRepository : Repository<Suggest>, ISuggestRepository
	{
		public Suggest GetSuggest (string seoTitle)
		{
			Suggest result = null;

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

		public PaginatedList<Suggest> ListEnabled (int pageIndex = 0, int pageSize = 10, string[] tags = null)
		{
			var query = 
				// From all our progressStep
				(from entity in this.All ()
				 // Filter: we want only the enabled progressStep
				 where entity.PublicationState == PublicationState.Published
			 // Filter: which the starting date is today or later
			 //	 where @event.StartDate >= DateTime.Today
				 select entity);
			
			// if some tags have been specified
			if (tags != null
				&& tags.Length > 0) {
				// Apply tags filtering
				query = 
					(from entity in query
					 where entity.Tags.Any (tag => tags.Contains (tag))
					 select entity);
			}

			// Get the query's result as a paginated list
			var result = query.ToPaginatedList (pageIndex, pageSize);
			
			return result;
		}
		
		public IQueryable<Suggest> QueryPublished (string[] tags = null)
		{
			var query = 
				// From all our progressStep
				(from entity in this.All ()
			// Filter: we want only the enabled events
				 where entity.PublicationState == PublicationState.Published
			// Filter: which have been published today or sooner
				 select entity);
			
			// if some tags have been specified
			if (tags != null
				&& tags.Length > 0) {
				// Apply tags filtering
				query = 
					(from entity in query
					 where entity.Tags.Any (tag => tags.Contains (tag))
					 select entity);
			}
			
			return query;
		}

		public override PaginatedList<Suggest> QuickSearch (string queryTerms, int pageIndex, int pageSize)
		{
			// TODO: apply tags filtering
			var query = 
				(from entity in this.All ()
				 where entity.Title.Contains (queryTerms)
				 select entity );
			
			var result = query.ToPaginatedList (pageIndex, pageSize);
			
			return result;
		}
	}
}
