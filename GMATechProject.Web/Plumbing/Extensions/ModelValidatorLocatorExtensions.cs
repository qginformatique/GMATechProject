namespace GMATechProject.Web
{
	#region Using Directives
	
	using System;
	using Nancy.Validation;

	#endregion
	
	/// <summary>
	/// Description of ModelValidatorLocatorExtensions.
	/// </summary>
	public static class ModelValidatorLocatorExtensions
	{
		/// <summary>
		/// Get the <see cref="IModelValidator" /> for the specified type.
		/// </summary>
		public static IModelValidator GetValidator<T>(this IModelValidatorLocator locator, T item = null)
			where T : class
		{
			IModelValidator result = null;

			// Get the type from the item if it has been specified; otherwise get the type of the generic parameter
			var type = item != null ? item.GetType() : typeof(T);
						
			// return the validator for this type
			result = locator.GetValidatorForType(type);
			
			return result;
		}
	}
}
