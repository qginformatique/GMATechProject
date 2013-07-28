namespace GMATechProject.Domain
{
    #region Using Directives

    using System;
    using System.Configuration;

    #endregion

    /// <summary>
    /// This attribute allows to defines the configuration section name of a configuration object and whether the section is required (must be present in 
    /// the configuration file).
    /// </summary>
    /// <remarks>
    /// <para>
    ///     <b>About the configuration section name:</b>
    ///     <para>
    ///     You can ommit to specify the configuration section name. In this case, it will be inferred from your configuration class name.
    ///     </para>
    ///     <example>
    ///     For a class named <c>ConfigurationMyConfig</c>, the configuration section name inferred will be <c>myConfig</c>.
    ///     </example>
    ///     <example>
    ///     For a class named <c>MyConfigConfiguration</c>, the configuration section name inferred will also be <c>myConfig</c>.
    ///     </example>
    /// </para>
    /// <para>
    ///     <b>About the <see cref="IsRequired"/> property:</b>
    ///     <para>
    ///     If you specify <see langword="true"/>, then the configuration section must be present in 
    ///     the configuration section or a <see cref="ConfigurationErrorsException"/> will be thrown.
    ///     </para>
    ///     <para>
    ///     If you specify <see langword="false"/>, then the configuration section is optionnal and when requested, a new instance initialized 
    ///     with the default values provided with the <see cref="ConfigurationPropertyAttribute.DefaultValue"/> property will be returned. Any properties 
    ///     with no default value provided will be initialized with it's type default value.
    ///     </para>
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigurationSectionAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSectionAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the configuration section.</param>
        public ConfigurationSectionAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSectionAttribute"/> class.
        /// </summary>
        /// <remarks>
        /// Note that if your configuration section is not required, all properties must supply a default value.
        /// </remarks>
        /// <param name="isRequired">if set to <see langword="true"/> if this configuration section is required. Default is <see langword="false"/>.</param>
        public ConfigurationSectionAttribute(bool isRequired)
        {
            this.IsRequired = isRequired;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSectionAttribute"/> class.
        /// </summary>
        /// <remarks>
        /// Note that if your configuration section is not required, all properties must supply a default value.
        /// </remarks>
        /// <param name="name">The name of the configuration section.</param>
        /// <param name="isRequired">if set to <see langword="true"/> if this configuration section is required. Default is <see langword="false"/>.</param>
        public ConfigurationSectionAttribute(string name, bool isRequired)
        {
            this.Name = name;
            this.IsRequired = isRequired;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this configuration section is required.
        /// </summary>
        /// <remarks>
        /// Note that if your configuration section is not required, all properties must supply a default value.
        /// </remarks>
        /// <value>
        /// 	<see langword="true"/> if this configuration section is required; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets the name of the configuration section.
        /// </summary>
        /// <value>The name of the configuration section.</value>
        public string Name { get; private set; }

        #endregion
    }
}