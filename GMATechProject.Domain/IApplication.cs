namespace GMATechProject.Domain
{
	#region using Directives 

	using System;
	using System.Collections.Generic;
	using GMATechProject.Domain.Security;

	#endregion

	public partial interface IApplication
	{
		void InitializeApplicationData();

		/// <summary>
		/// Searchs for publicly accessible contents matching the specified query.
		/// </summary>
		PaginatedList<SearchContentResult> SearchContents(string query, int pageIndex, int pageSize = 20, Roles role = Roles.None, string[] tags = null);
	}

	/// <summary>
	/// Describe an result of a global content search.
	/// </summary> 
	public class SearchContentResult
	{
		public string Label { get; set; }

		public string Description { get; set; }

		public string Id { get; set; }

		public ContentType Type { get; set; }
	}

	/// <summary>
	/// The available content types.
	/// </summary>
	public enum ContentType
	{
		Page, 
		Article,
		Event
	}
}

