namespace GMATechProject.Web.Public.Models
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;

	using GMATechProject.Domain.Pages;
	
	#endregion

	/// <summary>
	/// Description of PublicSimpleModel.
	/// </summary>
	public class PublicSimpleModel
	{
		#region Properties

		public bool UseConcatenatedResources { get; set; }
		public bool RegisterNewUser { get; set; }
		public string IdNewUser { get; set; }
		public string ClientVersion { get; set; }
		public string HtmlTitle { get; set; }
		public MenuSimpleModel NavMenu { get; set; }
		public string MetaKeywords { get; set; }
		public string MetaDescription { get; set; }
		public Page Page { get; set; }
		
		#endregion
		
		public PublicSimpleModel(bool useConcatenatedResources, bool registerNewUser = false)
		{
			this.UseConcatenatedResources = useConcatenatedResources;
			this.RegisterNewUser = registerNewUser;
			this.ClientVersion = ConfigurationManager.AppSettings.Get("ClientVersion");

			this.MetaKeywords = "GMATechProject";

			this.NavMenu = new MenuSimpleModel();
		}
	}
}