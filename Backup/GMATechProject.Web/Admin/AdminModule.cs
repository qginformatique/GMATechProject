namespace GMATechProject.Web.Admin
{
	#region Using Directives

	using Nancy.Authentication.Forms;
	using Nancy.Security;

	using GMATechProject.Domain;
	using GMATechProject.Domain.Security;

    #endregion
    
    public class AdminModule : BaseAdminModule
    {
        public AdminModule (IApplication application) : base(application)
		{
			Get["/"] = Home;
        }

		private object Home (dynamic parameters)
		{
			return View[@"Admin.sshtml", new AdminModel() { 
				UseConcatenatedResources = UseConcatenatedResources,
				IsAuthenticated = this.Context.CurrentUser != null,
				ClientVersion = System.Configuration.ConfigurationManager.AppSettings.Get("ClientVersion")
            } ];
		}
    }

	public class AdminModel
	{
		public bool UseConcatenatedResources { get; set; }
		public bool IsAuthenticated { get; set; }
		public string ClientVersion { get; set; }
	}
}