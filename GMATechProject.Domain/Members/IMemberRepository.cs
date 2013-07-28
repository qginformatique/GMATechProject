namespace GMATechProject.Domain.Members
{
	using System;
	using System.Linq;

	/// <summary>
	/// Description of IMemberRepository.
	/// </summary>
	public interface IMemberRepository : IRepository<Member>
	{
		PaginatedList<Member> ListMembers (int pageIndex = 0, int pageSize = 10);

		IQueryable<Member> QueryPublicDisplay ();
	}
}
