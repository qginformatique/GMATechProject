namespace GMATechProject.Domain.Mailing
{
	using System;

	[GMATechProject.Domain.Plumbing.CollectionName("MailTemplate")]
	public class MailTemplate : Entity
	{
		public MailTemplate ()
		{
		}
				
		public string Name { get; set; }

		public string Subject { get; set; }
	
		public string RawBody { get; set; }
		
		public string HtmlBody { get; set; }
	}
}

