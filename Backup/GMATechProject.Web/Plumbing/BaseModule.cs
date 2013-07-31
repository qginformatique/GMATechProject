namespace GMATechProject.Web
{	
	#region Using Directives

	using System.Configuration;

	using Nancy;
	using Nancy.Security;
	using Nancy.ViewEngines;

	using NLog;

	using GMATechProject.Domain;
	using GMATechProject.Domain.Security;
	using GMATechProject.Web.Plumbing.Extensions;

	#endregion
	
	/// <summary>
	/// Base class for all our modules.
	/// </summary>
	public abstract class BaseModule : NancyModule
	{	
		#region Fields

		private readonly IApplication _Application;
		private readonly bool _Secured = false;
		private readonly Logger _Logger;
		private static bool? _UseConcatenatedResources;
		private Identity _UserIdentity;

		#endregion

		#region Properties

		protected static bool UseConcatenatedResources {
			get {
				return (_UseConcatenatedResources ?? (_UseConcatenatedResources = new bool?(bool.Parse(ConfigurationManager.AppSettings.Get("UseConcatenatedResources") ?? bool.FalseString )))).Value;
			}
		}

		protected Identity CurrentUser {
			get {
				// If we didn't initialized the user identity yet
				if(this._UserIdentity == null) {
					// If Nancy FormsAuthentication found a user
					if( this.Context.CurrentUser != null) {
						// Load this user's identity
						this._UserIdentity = this._Application.FindByExternalIdentifier(new System.Guid(this.Context.CurrentUser.UserName));
					}
					// If Nancy FormsAuthentication did not find a user
					else {
						// Initialize an anonymous identity
						this._UserIdentity = new Identity(){
							Role = Roles.None
						};
					}
				}

				return this._UserIdentity;
			}
		}

		protected IApplication Application { get { return this._Application; } }

		protected Logger Logger { get { return this._Logger; } }

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of <see cref="BaseModule"/>
		/// </summary>
		/// <param name="modulePath">The base url for this module.</param>
		/// <param name="secured">Defines whether this module is secured.</param>
        /// <param name="role">Defines whether this module is accessible only to user with the specified role</param>
		protected BaseModule(IApplication application, string modulePath = "", bool secured = false, Roles role = Roles.None) : base(modulePath)
		{
			this._Application = application;
			this._Secured = secured;
			
			// If this module is secured
			if(secured){
				// Enable authentication check
				this.RequiresAuthentication();

				// If this module requires a specific role
				if(role != Roles.None){
					// Enable role check
					this.RequiresClaims(new [] { role.ToString() });
				}
			}

			// Initialize the main application logger
			this._Logger = LogManager.GetLogger("WebApplicationLogger");

			// Before every requests, call the ProtectAgainstCrossScriptingAttacks method
			this.Before += ProtectAgainstCrossScriptingAttacks;
		}

		#endregion

		#region Methods

		protected virtual Response ProtectAgainstCrossScriptingAttacks(NancyContext nancyContext)
		{
			Response result = null;
			
			// If this module is secured 
			if(this._Secured)
			{
				// If the CSRF token is not valid
				if(!this.ValidateCsrfTokenNoThrow())
				{
					// Returns a 401
					result = HttpStatusCode.Unauthorized;
				}
			}
			
			return result;
		}

		#endregion
	}
}
