namespace GMATechProject.Web.Public.Models
{
	#region Using Directives

	using System;
	using System.Globalization;
	using System.Linq;

	using GMATechProject.Domain.Blog;
	
	#endregion

	/// <summary>
	/// Description of ArticleModel.
	/// </summary>
	public class ArticleModel : PublicSimpleModel
	{
		public ArticleExtended Article { get; set; }

		public ArticleModel(bool useConcatenatedResources, bool registerNewUser = false) : base (useConcatenatedResources, registerNewUser)
		{
		}
	}

	public class ArticleExtended : Article
	{
		public string IllustrationImageUrl { get { 

				return "/Upload/images/actualites/" + ((string.IsNullOrEmpty(ImageUrl) ? "actualites_1.jpg" : ImageUrl)); 
			} }
		public string MainIllustrationImageUrl { get { return "/Upload/images/actualites/" + ImageUrl.Insert( ImageUrl.LastIndexOf('/') != -1 ? ImageUrl.LastIndexOf('/') + 1 : 0, "illustration-"); } }
		public string PublicationDateInFrench { get { return String.Format(new CultureInfo("fr-FR") , "{0:d MMMM yyyy}", PublicationDate); } }
		public bool ShowDraft { get; set; }

		public string TruncateTitle { get { 
			var result = Title;
			if(Title.Length > 68)
			{
				var _titleArray = Title.Trim().Substring(0, 68).Split(' ');
				_titleArray = _titleArray.Take(_titleArray.Length - 1).ToArray();
				result = string.Join(" ", _titleArray) + "...";
			}
			return result;
		}}

		public string GetFriendlyUrl { get { 
			return "/article/" + FriendlyUrl ?? SeoTitle;
		}}
	}
}