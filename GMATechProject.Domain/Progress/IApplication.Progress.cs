namespace GMATechProject.Domain
{
	#region Using Directives
	
	using System;
	using System.Collections.Generic;
	
	using GMATechProject.Domain.Progress;
	using GMATechProject.Domain.Security;
	
	using FluentValidation;
	
	#endregion

	public partial interface IApplication
	{
		ProgressStep CreateProgressStep (ProgressStep progressStep);
		
		ProgressStep UpdateProgressStep (ProgressStep progressStep);

		PaginatedList<ProgressStep> ListPublishedProgressStep (int pageIndex = 0, int pageSize = 10, Roles role = Roles.None, string[] tags = null);
	}
}