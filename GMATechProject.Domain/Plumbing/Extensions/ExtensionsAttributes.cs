namespace GMATechProject.Domain
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    #endregion

    /// <summary>
    /// Extensions methods to help getting <see cref="Attribute"/> from a <see cref="ICustomAttributeProvider"/>.
    /// </summary>
    /// <see cref="ICustomAttributeProvider"/>
    public static class ExtensionsAttributes
    {
        #region Public Static Methods

        /// <summary>
        /// Tries to find an <see cref="Attribute"/> of type <typeparamref name="TAttribute"/> 
        /// on the specified <paramref name="customAttributeProvider"/> and its ancestors.
        /// </summary>
        /// <remarks>
        /// This overload will call the <see cref="GetAttribute{TAttribute}(ICustomAttributeProvider,bool)"/> 
        /// with the <c>inherit</c> parameter set to <see langword="true"/>.
        /// </remarks>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="customAttributeProvider">The <see cref="ICustomAttributeProvider"/> from which to get the attribute.</param>
        /// <returns>
        /// The attribute of type <typeparamref name="TAttribute"/> if found; 
        /// otherwise <see langword="null"/>.</returns>
        /// <seealso cref="GetAttribute{TAttribute}(ICustomAttributeProvider,bool)"/>
        public static TAttribute GetAttribute< TAttribute >( this ICustomAttributeProvider customAttributeProvider ) 
            where TAttribute: Attribute
        {
            // Call the GetAttribute{TAttribute}(ICustomAttributeProvider,bool) by specifying to look 
            // for the attribute in the customAttributeProvider ancestors
            return GetAttribute< TAttribute >( 
                customAttributeProvider, 
                true );
        }

        /// <summary>
        /// Tries to find an <see cref="Attribute"/> of type <typeparamref name="TAttribute"/> 
        /// on the specified <paramref name="customAttributeProvider"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="customAttributeProvider">The <see cref="ICustomAttributeProvider"/> from which to get the attribute.</param>
        /// <param name="inherit">If <see langword="true"/>, specifies to also search the ancestors of element for custom attributes. </param>
        /// <returns>
        /// The first attribute of type <typeparamref name="TAttribute"/> if found; 
        /// otherwise <see langword="null"/>.
        /// </returns>
        /// <seealso cref="GetAttribute{TAttribute}(System.Reflection.ICustomAttributeProvider)"/>
        public static TAttribute GetAttribute< TAttribute >( this ICustomAttributeProvider customAttributeProvider, bool inherit ) 
            where TAttribute: Attribute
        {
            TAttribute retour = null;

            // if any attribute provider
            if(customAttributeProvider != null)
            {
                var attributes = customAttributeProvider.GetCustomAttributes(
                    typeof (TAttribute),
                    inherit);

                if (attributes.Length > 0)
                    retour = (TAttribute) attributes[0];
            }
            return retour;
        }

        /// <summary>
        /// Tries to find multiple <see cref="Attribute"/> of type <typeparamref name="TAttribute"/> 
        /// on the specified <paramref name="customAttributeProvider"/> and its ancestors.
        /// </summary>
        /// <remarks>
        /// This overload will call the <see cref="ListAttributes{TAttribute}(ICustomAttributeProvider,bool)"/> 
        /// with the <c>inherit</c> parameter set to <see langword="true"/>.
        /// </remarks>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="customAttributeProvider">The <see cref="ICustomAttributeProvider"/> from which to get the attribute.</param>
        /// <returns>
        /// A list of the attributes of type <typeparamref name="TAttribute"/> as an <see cref="IEnumerable{TAttribute}"/> if found; 
        /// otherwise <see langword="null"/>.
        /// </returns>
        /// <seealso cref="ListAttributes{TAttribute}(ICustomAttributeProvider,bool)"/>
        public static IEnumerable< TAttribute > ListAttributes< TAttribute >( this ICustomAttributeProvider customAttributeProvider )
            where TAttribute: Attribute
        {
            // Call the ListAttributes{TAttribute}(ICustomAttributeProvider,bool) by specifying to look 
            // for the attribute in the customAttributeProvider ancestors
            return ListAttributes<TAttribute>( 
                customAttributeProvider, 
                true );
        }

        /// <summary>
        /// Tries to find multiple <see cref="Attribute"/> of type <typeparamref name="TAttribute"/> 
        /// on the specified <paramref name="customAttributeProvider"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="customAttributeProvider">The <see cref="ICustomAttributeProvider"/> from which to get the attribute.</param>
        /// <param name="inherit">If <see langword="true"/>, specifies to also search the ancestors of element for custom attributes. </param>
        /// <returns>
        /// A list of the attributes of type <typeparamref name="TAttribute"/> as an <see cref="IEnumerable{TAttribute}"/> if found; 
        /// otherwise <see langword="null"/>.
        /// </returns>
        /// <seealso cref="ListAttributes{TAttribute}(System.Reflection.ICustomAttributeProvider)"/>
        public static IEnumerable< TAttribute > ListAttributes< TAttribute >( this ICustomAttributeProvider customAttributeProvider, bool inherit )
            where TAttribute: Attribute
        {
            IEnumerable<TAttribute> retour = null;

            // if any custom attribute provider
            if(customAttributeProvider != null)
            {
                var attributes = customAttributeProvider
                    .GetCustomAttributes(
                        typeof (TAttribute),
                        inherit);

                retour = attributes.Cast<TAttribute>();
            }
            return retour;
        }

        /// <summary>
        /// Gets the first property with the specified attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="PropertyInfo"/> of the first property with the specified attribute if found; otherwise, <see langword="null"/>.</returns>
        public static PropertyInfo GetFirstPropertyWithAttribute<TAttribute>( this Type type ) 
            where TAttribute: Attribute
        {
            return type
                // Get the public properties of the type
                .GetProperties()
                // Returns the first property which have the attribute, or null
                .FirstOrDefault( property => property.GetAttribute< TAttribute >() != null );
        }

        public static PropertyInfo GetFirstPropertyWithAttribute<TAttribute>( this Type type, Expression<Func<TAttribute, bool>> conditionExpression ) 
            where TAttribute: Attribute
        {
            return type
                // Get the public properties of the type
                .GetProperties()
                // Returns the first property which have the attribute, or null
                .FirstOrDefault( property =>
                                     {
                                         var attribute = property.GetAttribute< TAttribute >();
                                         return attribute != null && conditionExpression.Compile().Invoke( attribute );
                                     } );
        }

        #endregion
    }
}