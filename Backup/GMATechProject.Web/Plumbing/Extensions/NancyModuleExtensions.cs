namespace GMATechProject.Web.Plumbing.Extensions
{
	#region Using Directives
	
	using System;
	using Nancy;
	using Nancy.Security;
	
	#endregion
	
	/// <summary>
	/// Description of NancyModuleExtensions.
	/// </summary>
	public static class NancyModuleExtensions
	{
		/// <summary>
		/// Validate CSRF validation token but do not throw an exception. Returns a boolean instead.
		/// </summary>
		/// <param name="validityPeriod">Optional: validaty period of the CSRF token.</param>
		/// <returns></returns>
		public static bool ValidateCsrfTokenNoThrow(this NancyModule module, TimeSpan? validityPeriod = null)
		{
			var result = false;
			
			try
			{
				// Call the standard ValidateCsrfToken method which throw exception when an invalid token is detected
				module.ValidateCsrfToken(validityPeriod);
				
				// If we didn't get an exception, then the token is valid
				result = true;
			}
			catch(CsrfValidationException)
			{
				result = false;
			}
			
			return result;
		}

	}
}
