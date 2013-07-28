namespace GMATechProject.Domain
{
	#region Using Directives
	
	using System;
	
	using GMATechProject.Domain.Pages;
	using GMATechProject.Domain.Plumbing;
	using GMATechProject.Domain.Security;
	
	using FluentValidation;
	
	#endregion
	
	public partial interface IApplication
	{
		Page CreatePage (Page page);

		Page UpdatePage (Page page);
	}
}

