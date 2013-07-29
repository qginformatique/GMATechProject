using System;
using System.Linq;
using System.Collections.Generic;

namespace GMATechProject.Domain.Suggestion
{
	/// <summary>
	/// Description of ISuggestRepository.
	/// </summary>
	public interface ISuggestRepository: IRepository<Suggest>
	{	
		Suggest GetSuggest (string seoTitle);

		PaginatedList<Suggest> ListEnabled (int pageIndex = 0, int pageSize = 10, string[] tags = null);
		
		IQueryable<Suggest> QueryPublished (string[] tags = null);
	}
}
