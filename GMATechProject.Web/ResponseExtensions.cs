
using System;
using System.Collections.Generic;
using Nancy;
using Nancy.Validation;

namespace ABC.Web
{
	/// <summary>
	/// Description of ResponseExtensions.
	/// </summary>
	public static class ResponseExtensions
	{
		public static Response AsJsonErrorOnValidation(this IResponseFormatter responseFormatter, IEnumerable<ModelValidationError> errors)
		{
			var actionResult = new ActionResult(false);
			
			foreach (var error in errors) {
				foreach (var member in error.MemberNames) {
					actionResult.Errors.Add(error.GetMessage(member));
				}
			}
			
			return responseFormatter.AsJson(actionResult);
		}
	}
	

}
