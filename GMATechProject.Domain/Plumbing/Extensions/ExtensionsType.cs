namespace GMATechProject
{
	using System;
	using System.Collections;
	using System.Linq;

	public static class ExtensionsType
	{
		/// <summary>
		/// Checks to see if the type implements a given interface.
		/// </summary>
		/// <param name="baseType">The type to be extended.</param>
		/// <param name="interfaceType">The interface to check for on the type.</param>
		/// <returns>Returns <code>true</code> if the type implements the given interface.</returns>
		public static bool IsImplementationOf(this Type baseType, Type interfaceType)
		{
			if (baseType == interfaceType || (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == interfaceType))
				return true;
			
			return baseType.GetInterfaces().Any(e => (e.IsGenericType && e.GetGenericTypeDefinition() == interfaceType) || (!e.IsGenericType && e == interfaceType));
		}
	}
}

