namespace GMATechProject.Web.Public.Models
{
	#region Using Directives

	using System;

	using GMATechProject.Domain;
	using GMATechProject.Domain.Blog;
	
	#endregion

	/// <summary>
	/// Description of ArticlesModel.
	/// </summary>
	public class ArticlesModel : PublicSimpleModel
	{
		public PaginatedList<ArticleExtended> Articles { get; set; }
		
		public ArticlesModel(bool useConcatenatedResources, bool registerNewUser = false) : base (useConcatenatedResources, registerNewUser)
		{
		}
	}
}