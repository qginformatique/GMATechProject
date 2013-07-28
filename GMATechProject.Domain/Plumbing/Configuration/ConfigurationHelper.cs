namespace GMATechProject.Domain
{
    #region Using Directives

    using System;
    using System.Configuration;

    #endregion

    /// <summary>
    /// Helps to deal with the configuration file
    /// </summary>
    public static class ConfigurationHelper
    {
        #region Get From AppSettings methods

        /// <summary>
        /// Get the parameter from the configuration file, or return the given default value
        /// 
        /// Note : totalement inutile, car on ne peut pas mettre autre chose qu'un ValueType dans un fichier de config ;)
        /// </summary>
        /// <param name="key">The key of the parameter</param>
        /// <param name="defaultvalue">The value to be returned if no value is associated to the key</param>
        public static T Get< T >( string key, T defaultvalue )
        {
            T retour;

            // look into the config file
            object obj = ConfigurationManager.AppSettings[ key ];
			
            // if present
            if ( obj != null 
			    && obj is T)
			{
                // get it
                retour = (T) obj;
			}
            else
			{
                // return the default value
                retour = defaultvalue;
			}

            return retour;
        }

        #endregion

        #region Get Strongly Typed Configuration object mapped to configuration section

        /// <summary>
        /// Gets the configuration object of type <typeparamref name="TConfiguration"/> from the configuration.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This will get the name of the configuration section for the specified configuration object ( <typeparamref name="TConfiguration"/> ) 
        /// by checking whether it have a <see cref="ConfigurationSectionAttribute"/>. If the <typeparamref name="TConfiguration"/> does not 
        /// have this attribute, the name will be a camel case version of the type's name with the word <c>Configuration</c> removed if present.
        /// </para>
        /// <para>
        /// For example:
        ///     <para>
        ///     For the type <c>ConfigurationMyService</c>, the section name will be <c>myService</c>.
        ///     </para>
        ///     <para>
        ///     For the type <c>MyServiceConfiguration</c>, the section name will be <c>myService</c>.
        ///     </para>
        ///     <para>
        ///     For the type <c>MyService</c>, the section name will be <c>myService</c>.
        ///     </para>
        /// </para>
        /// </remarks>
        /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
        /// <returns>The configuration object of type <typeparamref name="TConfiguration"/> if found; otherwise, <see langword="null"/>.</returns>
        public static TConfiguration Get<TConfiguration> () where TConfiguration : class, new()
		{
			// Get the configuration section name for the requested type
			var nameConfigurationSection = GetConfigurationSectionName (typeof(TConfiguration));

			// Get the configuration section by using the standard ConfigurationManager but cast the result in  the requested type.
			var result = ConfigurationManager.GetSection (nameConfigurationSection) as TConfiguration;

			// if we have a valid configuration
			if (result == null) {
				// If the configuration has not been supplied, check whether it is required
				if (CheckWhetherConfigurationSectionIsRequired (typeof(TConfiguration))) {
					throw new ConfigurationErrorsException ("The configuration section \"" + nameConfigurationSection + "\" is required.");
				} else {
					try {
						// Try to create an instance of the configuration class
						result = new TConfiguration ();

						var config = result as CustomConfigurationSection;

						if (config != null) {
							config.Initialize ();       
						} else {
							throw new ConfigurationErrorsException ("The requested configuration class does not inherits from CustomConfigurationSection and cannot be created with default values." );
                        }
                    }
                    catch (Exception exception)
                    {
                        throw new ConfigurationErrorsException( "An error has occured while trying to get a default configuration for the configuration class {0}. Check the inner exception for details.", exception );
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Returns true if the given configuration object of type <typeparamref name="TConfiguration"/> is available in the configuration.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This will get the name of the configuration section for the specified configuration object ( <typeparamref name="TConfiguration"/> ) 
        /// by checking whether it have a <see cref="ConfigurationSectionAttribute"/>. If the <typeparamref name="TConfiguration"/> does not 
        /// have this attribute, the name will be a camel case version of the type's name with the word <c>Configuration</c> removed if present.
        /// </para>
        /// <para>
        /// For example:
        ///     <para>
        ///     For the type <c>ConfigurationMyService</c>, the section name will be <c>myService</c>.
        ///     </para>
        ///     <para>
        ///     For the type <c>MyServiceConfiguration</c>, the section name will be <c>myService</c>.
        ///     </para>
        ///     <para>
        ///     For the type <c>MyService</c>, the section name will be <c>myService</c>.
        ///     </para>
        /// </para>
        /// </remarks>
        /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
        /// <returns>The configuration object of type <typeparamref name="TConfiguration"/> if found; otherwise, <see langword="null"/>.</returns>
        public static bool IsAvailable<TConfiguration>() where TConfiguration : class, new()
        {
            // Get the configuration section name for the requested type
            var nameConfigurationSection = GetConfigurationSectionName( typeof(TConfiguration));

            // Get the configuration section by using the standard ConfigurationManager but cast the result in  the requested type.
            return (null != (ConfigurationManager.GetSection(nameConfigurationSection) as TConfiguration));
        }

        /// <summary>
        /// Checks the whether the requested configuration section is required.
        /// </summary>
        /// <param name="typeConfiguration">The type of the configuration section.</param>
        private static bool CheckWhetherConfigurationSectionIsRequired( Type typeConfiguration )
        {
            var result = false;

            // Retrive the configuration section name from the ConfigurationSectionAttribute
            var attribute = typeConfiguration.GetAttribute< ConfigurationSectionAttribute >();

            if (attribute != null)
            {
                result = attribute.IsRequired;
            }

            return result;
        }
		
        /// <summary>
        /// Gets the name of the configuration section for the specified configuration object type.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This will get the name of the configuration section for the specified configuration object ( <paramref name="typeConfiguration"/> ) 
        /// by checking whether it have a <see cref="ConfigurationSectionAttribute"/>. If the <paramref name="typeConfiguration"/> does not 
        /// have this attribute, the name will be a camel case version of the type's name with the word <c>Configuration</c> removed if present.
        /// </para>
        /// <para>
        /// For example:
        ///     <para>
        ///     For the type <c>ConfigurationMyService</c>, the section name will be <c>myService</c>.
        ///     </para>
        ///     <para>
        ///     For the type <c>MyServiceConfiguration</c>, the section name will be <c>myService</c>.
        ///     </para>
        ///     <para>
        ///     For the type <c>MyService</c>, the section name will be <c>myService</c>.
        ///     </para>
        /// </para>
        /// </remarks>
        /// <param name="typeConfiguration">The type of the configuration object.</param>
        /// <returns>The name of the configuration section.</returns>
        private static string GetConfigurationSectionName( Type typeConfiguration )
        {
            string result = null;

            // Retrive the configuration section name from the ConfigurationSectionAttribute
            var attribute = typeConfiguration.GetAttribute< ConfigurationSectionAttribute >();

            // If the attribute was found
            if ( attribute != null && !string.IsNullOrEmpty(attribute.Name))
                result = attribute.Name;

            // If we don't have a name, we'll try to generate it
            if ( string.IsNullOrEmpty( result ) )
            {
                result = typeConfiguration.Name
                    // Remove the Configuration (Pascal case) string from the name
                    .Replace("Configuration", string.Empty)
                    // Remove the configuration (lower case) string from the name
                    .Replace("configuration", string.Empty)
                    // Get the type name in camel case
                    .ToCamelCase();
            }

            return result;
        }

        #endregion
    }
}