using GMATechProject.Domain;

namespace GMATechProject.Web
{
	#region Using Directives

	using System;
	using System.Configuration;
	using System.Net.Mail;

	using GMATechProject.Domain.Mailing;
	using GMATechProject.Web.Public.Models;
	
	using Nancy;
	using Nancy.ModelBinding;

	using FluentEmail;
	
	#endregion
	
    public class NewsletterModule : BaseModule
    {
		public NewsletterModule(IApplication application): base(application)
        {
			// Bind the HTTP POST verb with /newsletterJoin to the NewsletterJoin method
			// Deprecated : remplacé par MailChimp
			// this.Post["/newsletterJoin"] = NewsletterJoin;
        }

		protected virtual Response NewsletterJoin(dynamic parameters)
		{
			Response result = null;
			var parametersNewsletterJoin = this.Bind<ParametersNewsletterJoin>();
			
			// If we have an email adress
			if(parametersNewsletterJoin != null)
			{
				try
				{
					//var mailTemplate = "Nouvelle demande d'inscription à la newsletter pour l'adresse email @Model.Email";
					var email = Email.FromDefault()
							.To("communication@fibresud.org", "FIBRESUD - Communication")
							.BCC("contact@qginformatique.fr", "Quentin GARCIA - QG Informatique")
							.Subject("Demande d'inscription à la newsletter")
							.Body("<img src='http://www.fibresud.org/Resources/images/logo_fibresud.png' />"
							   + "<br/><br/>Nouvelle demande d'inscription à la newsletter pour l'adresse email <b>" + parametersNewsletterJoin.Email + "</b>"
							   + "<br/><br/>Nom: <b>" + parametersNewsletterJoin.LastName + "</b>"
							   + "<br/>Prénom: <b>" + parametersNewsletterJoin.FirstName + "</b>"
							   + "<br/>Organisme / Entreprise: <b>" + parametersNewsletterJoin.Organisation + "</b>"
							   + "<br/>Département: <b>" + parametersNewsletterJoin.Department + "</b>"
							   , true);
							//.UsingTemplateForBody(mailTemplate,  new { Email = parametersNewsletterJoin.Email }, true);
					
					//send normally
					email.Send();

					// Confirmation email for the subscriber
					var email2 = Email.FromDefault()
						.To(parametersNewsletterJoin.Email)
							.Subject("Inscription à la newsletter de fibresud.org")
							.Body("<img src='http://www.fibresud.org/Resources/images/logo_fibresud.png' />"
							      + "<br/><br/>Votre demande d'inscription à la newsletter pour l'adresse email <b>" + parametersNewsletterJoin.Email + "</b> a bien été reçue."
							      , true);
					
					//send normally
					email2.Send();
					
					// Return success result
					result = Response.AsJson(ActionResult.AsSuccess(new string[]{parametersNewsletterJoin.Email}));
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
				result = HttpStatusCode.NotFound;
			}
			
			return result;
		}
		
		private class ParametersNewsletterJoin
		{
			public string Email {get; set;}

			public string LastName {get; set;}

			public string FirstName {get; set;}

			public string Organisation {get; set;}

			public string Department {get; set;}
		}
    }
}