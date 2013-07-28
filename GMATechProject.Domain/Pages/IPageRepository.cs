using System;
using System.Collections.Generic;
using System.Linq;
using GMATechProject.Domain;
using GMATechProject.Domain.Plumbing;
using GMATechProject.Domain.Security;

namespace GMATechProject.Domain.Pages
{
	/// <summary>
	/// Repository responsible for pages.
	/// </summary>
	public interface IPageRepository : IRepository<Page>
	{
		IQueryable<Page> QueryPublished(string[] tags = null);

		Page GetPage(string friendlyUrl);
	}
}