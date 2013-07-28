namespace GMATechProject.Domain
{
	#region Using Directives
	
	using System;
	using System.Collections.Generic;
	using System.Linq;
	
	using GMATechProject.Domain.Pages;
	using GMATechProject.Domain.Security;
	
	using FluentValidation;
	using MongoDB.Driver.Linq;
	
	#endregion

	public partial class Application : IApplication
	{
		private readonly IPageRepository _PageRepository;
		
		public Page CreatePage (Page page)
		{
			var result = this.CreateEntity<Page> (page, this._PageRepository);

			if (result.CreationDate == default(DateTime)) {
				result.CreationDate = DateTime.Now;
			}

			result.ModificationDate = DateTime.Now;

			result.Tags = new HashSet<string>();
			result.SeoKeywords = new HashSet<string>();

			return result;
		}
		
		public Page UpdatePage (Page page)
		{
			if (page.CreationDate == default(DateTime)) {
				page.CreationDate = DateTime.Now;
			}

			page.ModificationDate = DateTime.Now;

			return this.UpdateEntity (page, this._PageRepository);
		}		
	}
}