namespace GMATechProject.Domain.Mailing
{
	using System;
	using System.Linq;
	
	using GMATechProject.Domain.Plumbing;
	
	public class MailTemplateRepository : Repository<MailTemplate>, IMailTemplateRepository
	{
		public MailTemplate GetByName(string name)
		{
			var query = (from template in this.All()
			             where template.Name == name
			             select template);

			return query.FirstOrDefault();
		}
		
		public MailTemplateRepository ()
		{
		}

		public override PaginatedList<MailTemplate> QuickSearch (string query, int pageIndex, int pageSize)
		{
			throw new System.NotImplementedException ();
		}
	}
}

