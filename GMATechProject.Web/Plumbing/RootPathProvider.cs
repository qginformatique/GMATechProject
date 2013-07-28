namespace GMATechProject.Web
{
	#region Using Directives

	using System;
	using System.IO;
	using System.Reflection;

	using Nancy;
    
	#endregion
        
    /// <summary>
    /// Custom root path provider which will always return the project directory.
    /// </summary>
    public class RootPathProvider : IRootPathProvider
    {
		public string GetRootPath()
		{
			// Get the current assembly location
			var assembly = Assembly.GetExecutingAssembly().Location;
			
			// Get the parent directory of the assembly directory which is the project directory
			var rootDirectory = Directory.GetParent(Path.GetDirectoryName(assembly));
			
			// Returns its path
			return rootDirectory.FullName;
		}
    }
}