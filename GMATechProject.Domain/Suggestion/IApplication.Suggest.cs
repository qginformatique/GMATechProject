namespace GMATechProject.Domain
{
	#region Using Directives
	
	using System;
	using System.Collections.Generic;
	
	using GMATechProject.Domain.Suggestion;
	using GMATechProject.Domain.Security;
	
	using FluentValidation;
	
	#endregion

	public partial interface IApplication
	{
		Suggest CreateSuggest (Suggest suggest);
		
		Suggest UpdateSuggest (Suggest suggest);

		PaginatedList<Suggest> ListPublishedSuggest (int pageIndex = 0, int pageSize = 10, Roles role = Roles.None, string[] tags = null);
	}
}