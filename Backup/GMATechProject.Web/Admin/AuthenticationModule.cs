namespace GMATechProject.Web.Admin
{
	#region Using Directives

	using System;

	using Nancy;
	using Nancy.Authentication.Forms;
	using Nancy.ModelBinding;

	using GMATechProject.Domain;

	#endregion

	public class AuthenticationModule : BaseModule
	{
		public AuthenticationModule (IApplication application): base(application)
		{
			Get["/admin/authentication"] = parameters => View[@"Admin.sshtml", new { NeedAuthentication = true }];
			Post["/admin/authentication"] = this.Authenticate;
			Get["/admin/authentication/identity"] = this.GetIdentity;
		}

		private dynamic GetIdentity (dynamic parameters)
		{
			Response result = null;

			if (this.CurrentUser != null 
			    && !string.IsNullOrEmpty(this.CurrentUser.Email)) {
				result = Response.AsJson (ActionResult.AsSuccess (this.CurrentUser));
			} else {
				result = Response.AsJson(ActionResult.AsGenericError());
			}

			return result;
		}

		private dynamic Authenticate(dynamic parameters)
		{
			Response result = null;

			var login = this.Bind<LoginParameters>();

			if(login != null)
			{
				var identity = this.Application.FindByEmailAndPassword(login.Email, login.Password);

				if(identity != null)
				{
					var responseFormsAuth = FormsAuthentication.UserLoggedInResponse(new Guid(identity.ExternalIdentifier), DateTime.Now.AddDays(7));

					result = Response.AsJson(ActionResult.AsSuccess(identity));

					this.Logger.Debug("responseFormsAuth.Cookies: " + responseFormsAuth.Cookies.Count );

					for(var index = 0; index < responseFormsAuth.Cookies.Count; index++){
						result.AddCookie(responseFormsAuth.Cookies[index]);
					}
				}
				else
				{
					result = Response.AsJson(ActionResult.AsError("Aucun utilisateur enregistré pour cet email et mot de passe."));
				}
			}
			else
			{
				result = Response.AsJson(ActionResult.AsGenericError());
			}

			return result;
		}

		private class LoginParameters{
			public string Email { get; set;}
			public string Password { get; set;}
		}
	}
}

