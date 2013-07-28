namespace GMATechProject.Web.Public.Models
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	
	#endregion

	/// <summary>
	/// Description of ConfigurationModel.
	/// </summary>
	public class SitemapModel
	{		
		public IList<SitemapURL> Urls { get; set; }

		public SitemapModel()
		{
		}
	}

	public class SitemapURL
	{
		/* URL of the page. This URL must begin with the protocol (such as http) */
		public string Loc { get; set; }
		/* The date of last modification of the file: yyyy-mm-dd */
		public string LastMod { get; set; }
		/* How frequently the page is likely to change: see enum */
		public ChangeFreq ChangeFreq { get; set; }
		/* The priority of this URL relative to other URLs on your site: from 0.0 to 1.0*/
		public string Priority { get; set; }
	}

	public enum ChangeFreq
	{
		always,
		hourly,
		daily,
		weekly,
		monthly,
		yearly,
		never
	}
}