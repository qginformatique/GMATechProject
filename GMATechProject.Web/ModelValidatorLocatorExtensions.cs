
using System;
using Nancy.Validation;

namespace ABC.Web
{
	/// <summary>
	/// Description of ModelValidatorLocatorExtensions.
	/// </summary>
	public static class ModelValidatorLocatorExtensions
	{
		public static IModelValidator GetValidator<T>(this IModelValidatorLocator locator, T item = null)
			where T : class
		{
			IModelValidator result = null;
			
			if(item != null)
			{
				result = locator.GetValidatorForType(item.GetType());
			}
			else
			{
				result = locator.GetValidatorForType(typeof(T));
			}
			
			return result;
		}
	}
}
