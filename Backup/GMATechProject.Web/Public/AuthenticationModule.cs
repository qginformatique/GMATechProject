using GMATechProject.Domain;

namespace GMATechProject.Web
{
	using System;
	using Nancy;
	using Nancy.ModelBinding;
	using GMATechProject.Web.Public.Models;
	
	public class AuthenticationModule : BaseModule
	{
		public AuthenticationModule (IApplication application) : base(application, "authentication")
		{
			this.Get["/{id}"] = this.AuthenticateOrRegister;
			this.Post["/Register"] = this.Register;
		}

		private dynamic Register(dynamic parameters)
		{
			Response result = null;
			var parametersRegistration = this.Bind<ParametersRegistration>();

			if(parametersRegistration != null)
			{
				if(parametersRegistration.Password == parametersRegistration.ConfirmPassword)
				{
					if(this.Application.UpdateIdentityPassword(parametersRegistration.Id, parametersRegistration.Password) != null)
					{
						result = Response.AsJson (ActionResult.AsSuccess ());
					}
					else
					{
						result = Response.AsJson (ActionResult.AsGenericError ());
					}
				}
				else
				{
					result = Response.AsJson (ActionResult.AsError("Votre mot de passe et sa confirmation ne sont pas identiques."));
				}
			}

			return result;
		}

		private dynamic AuthenticateOrRegister(dynamic parameters)
		{
			dynamic result = null;

			// If an identifier has been supplied
			if(parameters.id != null)
			{
				// This is a newly registered user who needs to set his password
				var model = new PublicModel(UseConcatenatedResources, true);
				model.IdNewUser = parameters.id;

				result = this.View["Public.sshtml", model];
			}
			else
			{
				// This is a user who want to authenticate
			}

			return result;
		}

		private class ParametersRegistration
		{
			public string Id {get; set;}
			public string Password {get; set;}
			public string ConfirmPassword {get; set;}
		}
	}
}

