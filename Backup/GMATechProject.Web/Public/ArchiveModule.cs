using GMATechProject.Domain;

namespace GMATechProject.Web.Public
{
	#region Using Directives

	using System;
	
	#endregion
	
	/// <summary>
	/// The module responsible for the blog archived articles management.
	/// </summary>
	public class ArchiveModule : BaseModule
	{
		public ArchiveModule(IApplication application) : base(application, "/archives")
		{
			// Define a route for urls "/archives" which will returns all articles's of all years and months
			Get[""] = parameters =>
	        {
	            return "All articles of all years and months.";
	        };
	 
			// Define a route for urls "/archives/{year}" which will returns all articles's of the specified year
         	Get[@"/(?<year>19[0-9]{2}|2[0-9]{3})"] = parameters =>
	        {
	            return String.Format("All articles of the year {0}",
	                parameters.year);
	        };
	 
			// Define a route for urls "/archives/{year}/{month}" which will returns all articles's of the specified month and year
	        Get[@"/(?<year>19[0-9]{2}|2[0-9]{3})/(?<month>0[1-9]|1[012])"] = parameters =>
        	{
	            return String.Format("All articles of month {0} of the year {1}",
	                parameters.month,
	                parameters.year);
	        };			
		}
	}
}
