namespace GMATechProject.Domain
{
	#region Using Directives
	
	using System.Configuration;
	
	#endregion
	
    public class CustomConfigurationSection : ConfigurationSection
    {
        #region Public Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public virtual void Initialize()
        {
            this.Init();
            this.InitializeDefault();

            // TODO: Check for required properties with no default values, 
			// and in case the configuration is required, throw an exception
        }

        #endregion
    }
}