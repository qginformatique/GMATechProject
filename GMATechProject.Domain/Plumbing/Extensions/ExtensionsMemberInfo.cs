namespace GMATechProject.Domain
{
    #region Using Directives

	using System;
    using System.Reflection;

    #endregion

    public static class ExtensionsMemberInfo
    {
        #region Public Static Methods

        /// <summary>
        /// Get the name of the <see cref="MemberInfo"/> in camel case.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The name of the <see cref="MemberInfo"/> in camel case</returns>
        public static string GetCamelCaseName( this MemberInfo value )
        {
            return value.Name.ToCamelCase();
        }

        /// <summary>
        /// Recursive : get interface named property
        /// (helps to deal with interface pseudo-inheritance that's not managed by Reflection
        /// </summary>
        public static PropertyInfo GetNestedProperty( this Type type, string name )
        {
            PropertyInfo retour = null;

            // check if the property is in this type
            retour = type.GetProperty( name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase );
            // if not found
            if ( retour == null )
            {
                // get interfaces
                Type[] interfaces = type.GetInterfaces();
                // if any
                if ( ( interfaces != null )
                     && ( interfaces.Length > 0 ) )
                {
                    // while not found
                    int i = 0;
                    while ( ( i < interfaces.Length )
                            && retour == null )
                    {
                        // recursive call
                        retour = GetNestedProperty( interfaces[ i ], name );

                        i++;
                    }
                }
            }
            return retour;
        }
		
        /// <summary>
        /// Gets the fullname, works even for generic type whose property 'FullName' is null
        /// </summary>
        /// <returns>A full name</returns>
        public static string GetFullNameSafe(this Type type)
        {
            string result;
            
            // if we got a type
            if(type != null)
            {
                // get that type fullname
                result = type.FullName;
                // if no fullname
                if(string.IsNullOrEmpty(result))
                {
                    // if we got a name
                    if(!string.IsNullOrEmpty(type.Name))
                    {
                        // if we got a namespace
                        if (!string.IsNullOrEmpty(type.Namespace))
                        {
                            // compute the full name
                            result = (type.Namespace + "." + type.Name);
                        }
                        // if no namespace
                        else
                        {
                            // just use type name
                            result = type.Name;
                        }
                    }
                    // if no type name (is that possible ?)
                    else
                    {
                        // use (null)
                        result = "(null)";
                    }
                }
            }
            // if no type
            else
            {
                // use (null)
                result = "(null)";
            }
            return result;
        }
		
        #endregion
    }
}