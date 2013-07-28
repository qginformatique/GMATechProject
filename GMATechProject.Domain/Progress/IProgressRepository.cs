using System;
using System.Linq;
using System.Collections.Generic;

namespace GMATechProject.Domain.Progress
{
	/// <summary>
	/// Description of IProgressRepository.
	/// </summary>
	public interface IProgressRepository: IRepository<ProgressStep>
	{	
		ProgressStep GetProgressStep (string seoTitle);

		PaginatedList<ProgressStep> ListEnabled (int pageIndex = 0, int pageSize = 10, string[] tags = null);
		
		IQueryable<ProgressStep> QueryPublished (string[] tags = null);
	}
}
