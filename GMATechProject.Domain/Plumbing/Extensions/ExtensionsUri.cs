namespace GMATechProject.Domain
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    /// Extensions methods for the <see cref="Uri"/> class.
    /// </summary>
    public static class ExtensionsUri
    {
        #region Public Static Methods

        /// <summary>
        /// Gets the extension of the uri.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The extension of the uri (with the leading dot) if found; otherwise, <see cref="string.Empty"/>.</returns>
        public static string GetExtension( this Uri value )
        {
            // Check whether the Uri is null
            if ( value != null )
            {
                string path = null;

                // If the Uri is absolute
                if ( value.IsAbsoluteUri )
                {
                    // Get the absolute path of the Uri (without the querystring)
                    path = value.AbsolutePath;
                }
                else
                {
                    // Get the string representation of the Uri
                    var uriAsString = value.ToString();
                    path = uriAsString;

                    // Get the position of the question mark indicating the beginning of the query string
                    var indexOfQueryString = uriAsString.IndexOf( "?" );

                    // If there is a query string
                    if ( indexOfQueryString > -1 )
                    {
                        // The path is what is before the query string 
                        path = uriAsString.Substring( 0, indexOfQueryString );
                    }
                }

                // Check whether the path contains a dot
                var indexOfPoint = path.IndexOf( "." );

                // If it does
                if ( indexOfPoint > -1 )
                {
                    // Return what's after the dot position, which will be the extension
                    return path.Substring( indexOfPoint );
                }
            }

            // If we got there, return an empty string
            return string.Empty;
        }

        #endregion
    }
}