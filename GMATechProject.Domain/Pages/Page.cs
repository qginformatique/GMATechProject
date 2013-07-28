namespace GMATechProject.Domain.Pages
{
	#region Using Directives
	
	using System;
	using System.Collections.Generic;
	
	using GMATechProject.Domain.Plumbing;
	using FluentValidation.Attributes;
	
	#endregion

	/// <summary>
	/// A static page.
	/// </summary>
	[CollectionName("Page")]
	[Validator(typeof(PageValidator))]
	public class Page : Entity, IPublishable
	{		
        public string Title { get; set; }

        public string Content { get; set; }

		public PublicationState PublicationState { get; set; }

		public PageTemplate PageTemplate { get; set; }

		public DateTime CreationDate { get; set; }

		public DateTime ModificationDate { get; set; }

		public string FriendlyUrl { get; set; }
		
        public string SeoTitle { get; set; }

        public string SeoDescription { get; set; }

        public HashSet<string> SeoKeywords { get; set; }
		
		public HashSet<string> Tags { get; set; }
		
		public override string ToString ()
		{
			return string.Format ("[Page: Title={0}, Content={1}, PublicationState={2}, CreationDate={3}, ModificationDate={4}, FriendlyUrl={5}, SeoTitle={6}, SeoDescription={7}, SeoKeywords={8}, Tags={9}]", Title, Content, PublicationState, CreationDate, ModificationDate, FriendlyUrl, SeoTitle, SeoDescription, SeoKeywords, Tags);
		}
  	}
}