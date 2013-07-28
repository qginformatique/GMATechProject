using GMATechProject.Domain;

namespace GMATechProject.Web.Public
{
	#region Using Directives
	
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;

	using GMATechProject.Web.Public.Models;

	using Nancy;

	#endregion
	
	/// <summary>
	/// Description of BasePublicModule.
	/// </summary>
	public class BasePublicModule : BaseModule
	{
		public BasePublicModule(IApplication application) : base(application)
		{
		}
	}
}
