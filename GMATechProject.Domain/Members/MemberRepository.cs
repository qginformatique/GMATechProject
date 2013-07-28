namespace GMATechProject.Domain.Members
{
	#region Using Directives
	
	using System;
	using System.Linq;
	
	using GMATechProject.Domain.Plumbing;

	#endregion 
	
	/// <summary>
	/// Description of MemberRepository.
	/// </summary>
	public class MemberRepository : Repository<Member>, IMemberRepository
	{
		public IQueryable<Member> QueryPublicDisplay ()
		{
			var query = 
				// From all our member
				(from member in this.All ()
				 where member.DisplayInMembersList
				 select member);
			
			return query;
		}

		public PaginatedList<Member> ListMembers (int pageIndex = 0, int pageSize = 10)
		{
			var query = 
				// From all our member
				(from member in this.All ()
				 select member);
			
			query = query
				// Order them by name
				.OrderBy(member => member.Name);
			
			// Get the query's result as a paginated list
			var result = query.ToPaginatedList (pageIndex, pageSize);
			
			return result;
		}

		public override PaginatedList<Member> QuickSearch (string queryTerms, int pageIndex, int pageSize)
		{
			// TODO: apply tags filtering
			var query = 
				(from member in this.All ()
				 where member.Name.Contains (queryTerms)
				 || member.ContactName.Contains (queryTerms)
				 || member.ContactTitle.Contains (queryTerms)
				 || member.Service.Contains(queryTerms)
				 || member.Website.Contains(queryTerms)
				 || member.ContactEmail.Contains(queryTerms)
				 || member.City.Contains(queryTerms)
				 || member.Address1.Contains(queryTerms)
				 || member.Address2.Contains(queryTerms)
				 select member );

			var result = query.ToPaginatedList (pageIndex, pageSize);
			
			return result;
		}				
	}
}