using GMATechProject.Domain;

namespace GMATechProject.Web
{
	#region Using Directives

	using System;
	using System.Configuration;
	using System.IO;
	using System.Net;
	using System.Net.Mail;
	using System.Text;
	using System.Web;

	using GMATechProject.Domain.Mailing;
	using GMATechProject.Web.Public.Models;
	
	using Nancy;
	using Nancy.ModelBinding;

	using FluentEmail;
	
	#endregion
	
    public class ContactModule : BaseModule
    {
		public ContactModule(IApplication application) : base(application)
        {
			// Bind the HTTP POST verb with /contact to the Contact method
			this.Post["/contact"] = Contact;
        }

		protected bool CheckCaptcha(string reCaptchaChallenge, string reCaptchaResponse, string userHostAddress)
		{
			if(reCaptchaChallenge != null && reCaptchaResponse != null && userHostAddress != null)
			{
				var httpWebRequest = WebRequest.Create ("http://www.google.com/recaptcha/api/verify");
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "application/x-www-form-urlencoded";
				
				string s = string.Format ("privatekey={0}&remoteip={1}&challenge={2}&response={3}", 
				                          new object[] {
					HttpUtility.UrlEncode ("6LcpRNsSAAAAAPUuVfnP1o6EUuSVt_mQvhUJrBsS"),
					HttpUtility.UrlEncode (userHostAddress),
					HttpUtility.UrlEncode (reCaptchaChallenge),
					HttpUtility.UrlEncode (reCaptchaResponse)
				});
				byte[] bytes = Encoding.ASCII.GetBytes (s);
				using (var requestStream = httpWebRequest.GetRequestStream ())
				{
					requestStream.Write (bytes, 0, bytes.Length);
				}
				string[] array;
				try
				{
					using (var webResponse = httpWebRequest.GetResponse ())
					{
						using (var textReader = new StreamReader (webResponse.GetResponseStream (), Encoding.UTF8))
						{
							array = textReader.ReadToEnd ().Split (new string[]{"\n","\\n"}, StringSplitOptions.RemoveEmptyEntries);
						}
					}
				}
				catch (Exception exception)
				{
					this.Logger.ErrorException("Exception during CheckCaptcha", exception);
					//return Response.AsJson(ActionResult.AsError(new string[]{"Application", ex.Message}));
					return false;
				}
				string text = array[0];
				if (text != null)
				{
					if (text == "true")
					{
						//return Response.AsJson(ActionResult.AsSuccess("Valide!"));
						return true;
					}
					if (text == "false")
					{
						//return Response.AsJson(ActionResult.AsError(array [1].Trim (new char[]{'\''})));
						return false;
					}
				}
			}

			return false;
		}

		protected virtual Response Contact(dynamic parameters)
		{
			Response result = null;
			var parametersContact = this.Bind<ParametersContact>();
			
			// If we have data
			if(parametersContact != null)
			{
				try
				{
					// Check Captcha
					var checkCaptcha = CheckCaptcha(parametersContact.ReCaptchaChallenge, parametersContact.ReCaptchaResponse, Request.UserHostAddress);
					if(checkCaptcha != true)
					{
						return Response.AsJson(ActionResult.AsError("Captcha invalide!"));
					}

					var email = Email.FromDefault()
							.To("communication@fibresud.org", "FIBRESUD - Communication")
							//.To("quentin.garcia@gmail.com", "Quentin GARCIA - QG Informatique")
							.BCC("contact@qginformatique.fr", "Quentin GARCIA - QG Informatique")
							.Subject("Nouveau message reçu sur Fibresud.org")
							.Body("<img src='http://www.fibresud.org/Resources/images/logo_fibresud.png' /><br/><br/>Message reçu sur fibresud.org<br/><br/>Nom & Prénom: <b>" + parametersContact.Name + "</b>"
							      + "<br/>Email: <b>" + parametersContact.Email + "</b>"
							      + "<br/>Téléphone: <b>" + parametersContact.Phone + "</b>"
							      + "<br/>Sujet: <b>" + parametersContact.Subject + "</b>"
							      + "<br/><br/>Message: <b>" + parametersContact.Message + "</b>", 
							      true);

					//send normally
					email.Send();

					// Email confirmation for the user
					var email2 = Email.FromDefault()
							.To(parametersContact.Email)
							.Subject("Message envoyé à Fibresud.org")
							.Body("<img src='http://www.fibresud.org/Resources/images/logo_fibresud.png' /><br/><br/>Bonjour, votre message a bien été envoyé à FIBRESUD"
							      + "<br/><br/>Sujet: " + parametersContact.Subject
							      + "<br/><br/>Message: " + parametersContact.Message, 
							      true);

					//send normally
					email2.Send();

					// Return success result
					result = Response.AsJson(ActionResult.AsSuccess());
				}
				catch(Exception exception)
				{
					// TODO: logs
					
					// Return a failed action result
					result = Response.AsJson(ActionResult.AsError("[" + exception.Message + "] " + exception.InnerException.Message));
					//result = Response.AsJson(ActionResult.AsError("Une erreur est survenue."));
				}
			}
			
			// If we don't have a result yet
			if(result == null)
			{
				// NOTE: this could be better
				// Returns a 404
				result = Nancy.HttpStatusCode.NotFound;
			}
			
			return result;
		}
		
		private class ParametersContact
		{
			public string Name {get; set;}
			public string Email {get; set;}
			public string Phone {get; set;}
			public string Subject {get; set;}
			public string Message {get; set;}
			public string ReCaptchaChallenge { get; set; }
			public string ReCaptchaResponse { get; set; }
		}
    }
}