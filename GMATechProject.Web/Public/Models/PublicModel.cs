namespace GMATechProject.Web.Public.Models
{
	#region Using Directives

	using System;
	
	#endregion

	/// <summary>
	/// Description of PublicModel.
	/// </summary>
	public class PublicModel
	{
		public bool UseConcatenatedResources { get; set; }
		public bool RegisterNewUser { get; set; }
		public string IdNewUser { get; set; }
		public string ClientVersion { get; set; }
		
		public PublicModel(bool useConcatenatedResources, bool registerNewUser = false)
		{
			this.UseConcatenatedResources = useConcatenatedResources;
			this.RegisterNewUser = registerNewUser;
		}
	}
}