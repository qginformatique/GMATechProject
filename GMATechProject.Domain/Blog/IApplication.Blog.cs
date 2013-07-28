namespace GMATechProject.Domain
{
	#region Using Directives
	
	using System;
	
	using GMATechProject.Domain.Blog;
	using GMATechProject.Domain.Plumbing;
	using GMATechProject.Domain.Security;
	
	using FluentValidation;
	
	#endregion
	
	public partial interface IApplication
	{
		Article CreateArticle (Article article);

		Article UpdateArticle (Article article);		
		
		PaginatedList<Article> ListPublishedArticles(int pageIndex = 0, int pageSize = 10, Roles role = Roles.None, string[] tags = null);

		PaginatedList<Article> ListHomeArticles(int pageIndex = 0, int pageSize = 4, Roles role = Roles.None, bool homeSlider = false, string[] tags = null);
	}
}

