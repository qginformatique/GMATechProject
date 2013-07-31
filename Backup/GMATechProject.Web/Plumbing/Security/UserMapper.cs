using System;

using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Security;

using GMATechProject.Domain;
using NLog;

namespace GMATechProject.Web
{
	public class UserMapper : IUserMapper
	{
		private IApplication _Application;
		private Logger _Logger;

		public UserMapper (IApplication application)
		{
			this._Application = application;
			this._Logger = LogManager.GetLogger("WebApplicationLogger");
			this._Logger.Debug("Initializing UserMapper");
		}

		#region IUserMapper implementation

		public IUserIdentity GetUserFromIdentifier (Guid identifier, NancyContext context)
		{
			this._Logger.Debug("Authenticating user \"" + identifier + "\"");

			UserIdentity result = null;

			var identity = this._Application.FindByExternalIdentifier(identifier);

			this._Logger.Debug("Authenticated \"" + identifier + "\" : " + (identity != null));

			if(identity != null)
			{
				try {
					result = new UserIdentity(identity);
					this._Logger.Debug("Authenticated \"" + result);
				} catch (Exception ex) {
					this._Logger.Error("User mapping failed : " + ex.ToString());
					
				}
			}
						
			return result;
		}

		#endregion
	}
}

