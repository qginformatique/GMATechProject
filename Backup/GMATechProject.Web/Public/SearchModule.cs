namespace GMATechProject.Web
{
	#region Using Directives

	using System;
	using System.Configuration;
	using System.Web;

	using GMATechProject.Domain;
	using GMATechProject.Web.Public.Models;

	using Nancy;
	using Nancy.ModelBinding;

	using NLog;

	#endregion

	public class SearchModule : BaseModule
	{
		public SearchModule (IApplication application) : base(application)
		{
			Get["/search/"] = Search;
		}

		public Object Search(dynamic parameters)
		{
			Response result = null;
			
			var searchParameters = this.Bind<QuickSearchWithPaginationRequestParameters> ();
			
			if (searchParameters != null) {
				if (!string.IsNullOrEmpty (searchParameters.Query)) {
					try
					{
						// Request our repository for articles matching the query
						var contents = this.Application.SearchContents(
								searchParameters.Query, 
								searchParameters.PageIndex, 
								searchParameters.PageSize, 
								this.CurrentUser.Role);
						
						// Return a successfull action result with the contents
						result = Response.AsJson (ActionResult.AsSuccess (contents));
					}
					catch(Exception exception){
						this.Logger.LogException(LogLevel.Error, string.Empty,exception);

						result = Response.AsJson(ActionResult.AsGenericError());
					}
				} else {
					result = Response.AsJson (ActionResult.AsError ("Vous n'avez pas spécifié de critères de recherche."));
				}
			} else {
				result = HttpStatusCode.BadRequest;
			}
			
			return result;
		}
	}
}

