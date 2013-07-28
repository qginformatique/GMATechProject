namespace GMATechProject.Domain
{
	#region Using Directives
	
	using System;
	using System.Linq;
	
	using GMATechProject.Domain.Members;
	
	using FluentValidation;
	
	#endregion

	public partial class Application : IApplication
	{
		private readonly IMemberRepository _MemberRepository;
		
		public Member CreateMember (Member member)
		{
			return this.CreateEntity<Member>(member, this._MemberRepository);
		}
		
		public Member UpdateMember (Member member)
		{
			return this.UpdateEntity (member, this._MemberRepository);
		}

		public PaginatedList<Member> ListMembers (int pageIndex = 0, int pageSize = 10)
		{
			this._Logger.Debug("ListMembers");
			
			var query =
				// We want all the members
				(from members in this._MemberRepository.QueryPublicDisplay()				 
				 select members);

			var result = query
				.OrderBy (member => member.Name)
				.ToPaginatedList (pageIndex, pageSize);
			
			return result;
		}
	}
}

