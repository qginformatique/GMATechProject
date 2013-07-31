namespace GMATechProject.Web
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;

	using Nancy;
	
	#endregion

	public static class ExpirationHeadersExtensions
	{
		public static void CheckForIfNonMatch (this NancyContext context)
		{
			var request = context.Request;
			var response = context.Response;

			string responseETag;

			if (response.Headers.TryGetValue ("ETag", out responseETag)) {
				if (request.Headers.IfNoneMatch.Contains (responseETag)) {
					context.Response = HttpStatusCode.NotModified;
				}
			}
		}

		public static void CheckForIfModifiedSince (this NancyContext context)
		{
			var request = context.Request;
			var response = context.Response;

			string responseLastModified;
		    
			if (response.Headers.TryGetValue ("Last-Modified", out responseLastModified)) {
				DateTime lastModified = new DateTime();

				if (request.Headers.IfModifiedSince.HasValue || !DateTime.TryParseExact (responseLastModified, "R", CultureInfo.InvariantCulture, DateTimeStyles.None, out lastModified))
				{
					if (lastModified <= request.Headers.IfModifiedSince.Value) {
					context.Response = HttpStatusCode.NotModified;
					}
				}
			}
		}
	}
}

