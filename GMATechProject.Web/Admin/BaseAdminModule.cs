using GMATechProject.Domain;

namespace GMATechProject.Web.Admin
{
	#region Using Directives
	
	using System;
	using GMATechProject.Domain.Security;

	#endregion
	
	/// <summary>
	/// Description of BaseAdminModule.
	/// </summary>
	public abstract class BaseAdminModule : BaseModule
	{	
		// TODO: secured should be true and Role whatever minimum role will be required to access administration
		public BaseAdminModule(IApplication application, string modulePath = null, Roles role = Roles.Editor) : base(application, "/admin" + modulePath, false, role)
		{
		}
	}
}
