namespace GMATechProject.Domain.Blog
{
	#region Using Directives
	
	using System;
	using System.Collections.Generic;
	
	using GMATechProject.Domain.Plumbing;
	using FluentValidation.Attributes;
	
	#endregion

	/// <summary>
	/// A blog article.
	/// </summary>
	[CollectionName("Article")]
	[Validator(typeof(ArticleValidator))]
	public class Article : Entity, IPublishable
	{
		public Article ()
		{
		}
		
        public string Title { get; set; }

        public string Content { get; set; }

        public string Description { get; set; }
		
		// Spécifie s'il faut l'afficher sur le Slider de la page d'accueil
		public bool HomeSlider { get; set; }

		public string ImageUrl { get; set; }

		public PublicationState PublicationState { get; set; }

		public DateTime CreationDate { get; set; }

		public DateTime ModificationDate { get; set; }

		public DateTime? PublicationDate { get; set; }

		public string FriendlyUrl { get; set; }
		
        public string SeoTitle { get; set; }

        public string SeoDescription { get; set; }

        public HashSet<string> SeoKeywords { get; set; }
		
		public HashSet<string> Tags { get; set; }
		
		public override string ToString ()
		{
			return string.Format ("[Article: Title={0}, Content={1}, Description={2}, State={3}, CreationDate={4}, ModificationDate={5}, PublicationDate={6}, FriendlyUrl={7}, SeoTitle={8}, SeoDescription={9}, SeoKeywords={10}, HomeSlider={11}, Tags={12}]", Title, Content, Description, PublicationState, CreationDate, ModificationDate, PublicationDate, FriendlyUrl, SeoTitle, SeoDescription, SeoKeywords, HomeSlider, Tags);
		}
  	}
}