using System;

namespace GMATechProject.Domain.Mailing
{
	public interface IMailTemplateRepository : IRepository<MailTemplate>
	{
		MailTemplate GetByName(string name);
	}
}

