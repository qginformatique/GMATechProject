using System;

namespace GMATechProject.Domain
{
    /// <summary>
    /// Extension on the 'Type' type
    /// </summary>
    public static class ExtensionsType
    {
        /// <summary>
        /// Return true if given type has given attribute
        /// </summary>
        /// <typeparam name="TAttribute">The attribute type</typeparam>
        /// <param name="type">The working type</param>
        /// <returns>true if given type has given attribute</returns>
        public static bool HasAttribute<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            bool result = false;

            // if we have a given type
            if(type != null)
            {
                result = (type.GetAttribute<TAttribute>(false) != null);
            }
            return result;
        }
    }
}
