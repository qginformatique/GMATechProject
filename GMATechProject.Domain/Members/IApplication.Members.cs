namespace GMATechProject.Domain
{
	#region Using Directives
	
	using System;
	
	using GMATechProject.Domain.Members;
	
	using FluentValidation;
	
	#endregion
	
	public partial interface IApplication
	{
		Member CreateMember (Member member);
		
		Member UpdateMember (Member member);

		PaginatedList<Member> ListMembers (int pageIndex = 0, int pageSize = 10);
	}
}

