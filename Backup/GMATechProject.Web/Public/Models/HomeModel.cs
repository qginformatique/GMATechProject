namespace GMATechProject.Web.Public.Models
{
	#region Using Directives

	using System;
	using System.Globalization;
	using System.Collections.Generic;

	using GMATechProject.Domain;
	using GMATechProject.Domain.Blog;
	
	#endregion

	/// <summary>
	/// Description of HomeModel.
	/// </summary>
	public class HomeModel : PublicSimpleModel
	{
		public IList<Article> Articles { get; set; }

		public HomeModel(bool useConcatenatedResources, bool registerNewUser = false) : base (useConcatenatedResources, registerNewUser)
		{
		}
	}
}