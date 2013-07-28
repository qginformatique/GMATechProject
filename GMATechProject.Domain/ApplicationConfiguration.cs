namespace GMATechProject.Domain
{
	#region Using Directives
	
	using System;
	using System.Configuration;
	
	#endregion
	
	public class ApplicationConfiguration : CustomConfigurationSection
	{
		/// <summary>
		/// Gets or sets the email for system automatic emails.
		/// </summary>
		/// <value>
		/// The email for system automatic emails. 
		/// </value>
		[ConfigurationProperty("AddressForEmailFromSystem")]
		public string AddressForEmailFromSystem { 
			get { 
				return (string)this ["AddressForEmailFromSystem"]; 
			}
			set { 
				this ["AddressForEmailFromSystem"] = value; 
			}
		}
		
		/// <summary>
		/// Gets or sets the name for system automatic emails.
		/// </summary>
		/// <value>
		/// The name for system automatic emails.
		/// </value>
		[ConfigurationProperty("NameForEmailFromSystem")]
		public string NameForEmailFromSystem { 
			get { 
				return (string)this ["NameForEmailFromSystem"]; 
			}
			set { 
				this ["NameForEmailFromSystem"] = value; 
			}
		}		
		
		/// <summary>
		/// Gets or sets the name of the application.
		/// </summary>
		/// <value>
		/// The name of the application
		/// </value>
		[ConfigurationProperty("ApplicationName")]
		public string ApplicationName { 
			get { 
				return (string)this ["ApplicationName"]; 
			}
			set { 
				this ["ApplicationName"] = value; 
			}
		}				
	}
}

