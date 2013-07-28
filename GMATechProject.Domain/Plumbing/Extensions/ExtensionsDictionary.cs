namespace GMATechProject.Domain
{
    #region Using Directives

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    #endregion

    /// <summary>
    /// Extensions methods for the <see cref="IDictionary"/>.
    /// </summary>
    public static class ExtensionsDictionary
    {
        #region Public Static Methods

        /// <summary>
        /// Add the specified key with specified value.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="IDictionary"/> on which the method was called : fluent interface.</returns>
        public static IDictionary N( this IDictionary dictionary, object key, object value )
        {
            // We check whether the key is null
            if ( key != null )
            {
                // If it is not null, we add it to the dictionary 
                dictionary[ key ] = value;
            }

            // Returns the dictionary
            return dictionary;
        }

        /// <summary>
        /// Adds the specified key to a new <see cref="PowerOnDictionary"/> and returns it.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="condition">the condition which must be <see langword="true"/> to add the option.</param>
        /// <returns>The <see cref="IDictionary"/> on which the method was called : fluent interface.</returns>
        public static IDictionary N( this IDictionary dictionary, object key, object value, bool condition )
        {
            // We check whether the key is null
            if ( key != null )
            {
                // If the condition is true
                if ( condition )
                {
                    // If it is not null, we add it to the dictionary, the N() method will returns the dictionary
                    return dictionary.N( key, value );
                }
            }

            // If the condition is false, simply returns the dictionary
            return dictionary;
        }

        /// <summary>
        /// Add the specified key with specified value.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="IDictionary"/> on which the method was called : fluent interface.</returns>
        public static IDictionary<TKey, TValue> N<TKey, TValue>( this IDictionary<TKey, TValue> dictionary, TKey key, TValue value )
        {
            // If it is not null, we add it to the dictionary 
            dictionary[ key ] = value;

            // Returns the dictionary
            return dictionary;
        }

        /// <summary>
        /// Adds the specified key to the <see cref="IDictionary{TKey, TValue}"/> and returns it.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="condition">the condition which must be <see langword="true"/> to add the option.</param>
        /// <returns>The <see cref="IDictionary"/> on which the method was called : fluent interface.</returns>
        public static IDictionary<TKey, TValue> N<TKey, TValue>( this IDictionary<TKey, TValue> dictionary, TKey key, TValue value, bool condition )
        {
            // If the condition is true
            if ( condition )
            {
                // If it is not null, we add it to the dictionary, the N() method will returns the dictionary
                return dictionary.N( key, value );
            }

            // If the condition is false, simply returns the dictionary
            return dictionary;
        }

        /// <summary>
        /// Gets the value from the dictionary if it exists or return the specified default value.
        /// </summary>
        /// <remarks>
        /// This will catch any <see cref="InvalidCastException"/>. In those cases, it will return the default value.
        /// </remarks>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The value casted to the requested type if found; otherwise, <see langword="null"/>.</returns>
        public static TValue TryGetValue<TValue> (this IDictionary dictionary, object key, TValue defaultValue)
		{
			// Check whether the dictionary contains the requested key
			if (dictionary.Contains (key)) {
				// Get the value from the dictionary as an object
				var value = dictionary [key];

				// Check if the value can be cast to the requested type
				if (typeof(TValue).IsAssignableFrom (value.GetType ())) {
					// Return the value cast to the requested type
					return (TValue)value;
				}
                    
				throw new InvalidOperationException (
					string.Format (
						"Failed to cast the value for the key \"{0}\" into type {1}",
                        key,
                        typeof(TValue))
					);
                }

            return defaultValue;
        }

        #endregion
    }
}