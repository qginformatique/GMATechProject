namespace GMATechProject.Domain
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Linq;

	#endregion
	
	public class PaginatedList<T>
	{
		public IList<T> Items { get; set; }
		
		public int PageIndex { get; set; }
		
		public int PageSize { get; set; }
		
		public int Total { get; set; }
		
		public PaginatedList(int pageIndex, int pageSize, int total, IList<T> items)
		{
			this.PageIndex = pageIndex;
			this.PageSize = pageSize;
			this.Total = total;
			this.Items = items;
		}
	}
	
	public static class QueryableExtensions
	{
		public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> queryable, int pageIndex = 0, int pageSize = 0)
		{
			// Ensure the page index is greater or equal to 0
			var sanitizedPageIndex = pageIndex >= 1 ? pageIndex : 1;
			// Specified page index should be indexed on 1 (page 1 is 1), but technically we use 0 indexed page numbers (page 1 is 0)
			var technicalPageIndex = sanitizedPageIndex > 0 ? sanitizedPageIndex - 1 : 0;
			
			// Ensure the page size is greater than 0
			var sanitizedPageSize = pageSize > 0 ? pageSize : 10;
			
			// Get the total number of items for this query
			var total = queryable.Count();
			
			var items = queryable
				// We don't want items before the requested page
				.Skip(technicalPageIndex * sanitizedPageSize)
				// Only take the specified number of items
				.Take(sanitizedPageSize)
				// Execute the query against database to get a list
				.ToList();
			
			// Create a new PaginatedList
			var result = new PaginatedList<T>(
				sanitizedPageIndex, 
				sanitizedPageSize, 
				total, 
				items);
			
			return result;
		}

		public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> queryable, int total, int pageIndex = 0, int pageSize = 0)
		{
			// Ensure the page index is greater or equal to 0
			var sanitizedPageIndex = pageIndex >= 1 ? pageIndex : 1;
			// Specified page index should be indexed on 1 (page 1 is 1), but technically we use 0 indexed page numbers (page 1 is 0)
			var technicalPageIndex = sanitizedPageIndex > 0 ? sanitizedPageIndex - 1 : 0;
			
			// Ensure the page size is greater than 0
			var sanitizedPageSize = pageSize > 0 ? pageSize : 10;

			var items = queryable
				// We don't want items before the requested page
				.Skip(technicalPageIndex * sanitizedPageSize)
				// Only take the specified number of items
				.Take(sanitizedPageSize)
				// Execute the query against database to get a list
				.ToList();
			
			// Create a new PaginatedList
			var result = new PaginatedList<T>(
				sanitizedPageIndex, 
				sanitizedPageSize, 
				total, 
				items);
			
			return result;
		}
	}
}
