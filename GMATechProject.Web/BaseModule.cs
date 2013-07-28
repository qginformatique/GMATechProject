namespace ABC.Web
{	
	#region Using Directives
	
	using Nancy;
	using Nancy.Security;
		
	using ABC.Domain.Security;
	
	#endregion
	
	/// <summary>
	/// Base class for all our modules.
	/// </summary>
	public abstract class BaseModule : NancyModule
	{	
		private readonly bool _Secured;
		
		/// <summary>
		/// Constructor for BaseModule
		/// </summary>
		/// <param name="modulePath">The base url for this module.</param>
		/// <param name="secured">Defines whether this module is secured.</param>
        /// <param name="role">Defines whether this module is accessible only to user with the specified role</param>
		protected BaseModule (string modulePath = "", bool secured = false, Roles role = Roles.None) : base(modulePath)
		{
			this._Secured = secured;
			
			// If this module is secured
			if (secured) {
				// Enable authentication check
				this.RequiresAuthentication ();
			}
			
			// If this module requires a specific role
			if (role != Roles.None) {
				// Enable role check
				this.RequiresClaims (new [] { role.ToString () });
			}
			
			//this.
		}
	}
}
