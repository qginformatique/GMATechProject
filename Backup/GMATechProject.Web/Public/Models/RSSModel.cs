namespace GMATechProject.Web.Public.Models
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	
	#endregion

	/// <summary>
	/// Description of RSSModel.
	/// </summary>
	public class RSSModel
	{		
		public IList<FeedItem> Items { get; set; }

		public RSSModel()
		{
		}
	}

	public class FeedItem
	{
		public string Title { get; set; }
		public string Link { get; set; }
		public string RSSGuid { get; set; }
		public string PublicationDate { get; set; }
		public string Description { get; set; }
		// Optionnal: used to add an image
		public string ImageUrl { get; set; }
	}
}