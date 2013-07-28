using System;
using System.Collections.Generic;
using System.Linq;
using GMATechProject.Domain.Plumbing;
using GMATechProject.Domain.Security;

namespace GMATechProject.Domain.Blog
{
	/// <summary>
	/// Repository responsible for articles.
	/// </summary>
	public interface IArticleRepository : IRepository<Article>
	{
		IQueryable<Article> QueryPublished(string[] tags = null);
		
		Article GetArticle(string seoTitle);

		IList<Article> ListHomeArticles (int pageIndex = 0, int pageSize = 10, bool homeSlider = false, string[] tags = null);
	}
}
