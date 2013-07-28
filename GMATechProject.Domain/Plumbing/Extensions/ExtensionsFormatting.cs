namespace GMATechProject.Domain
{
    #region Using Directives

    using System;
    using System.Globalization;

    #endregion

    /// <summary>
    /// Extensions methods which format types.
    /// </summary>
    public static class ExtensionsFormatting
    {
        #region Inner data

        /// <summary>
        /// Formatting information
        /// </summary>
        private static readonly NumberFormatInfo _Nfi = new NumberFormatInfo()
            {
                CurrencySymbol = "",
                CurrencyGroupSeparator = " ",
                CurrencyNegativePattern = 2,
                CurrencyDecimalDigits = 0,

            };

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Convert given string to a human-readable string
        /// Group numbers, like <c>3 654 332 843</c>
        /// </summary>
        public static string ToStringWithSpaces( this Int32 number )
        {
            // new version based on NumberFormatInfo
            return number.ToString("C", _Nfi);
        }

        /// <summary>
        /// Convert given string to a human-readable string
        /// Group numbers, like <c>14 000</c>
        /// </summary>
        public static string ToStringWithSpaces( this Int16 number )
        {
            // new version based on NumberFormatInfo
            return number.ToString("C", _Nfi);
        }

        /// <summary>
        /// Convert given string to a human-readable string
        /// Group numbers, like <c>332 332 654 332 843</c>
        /// </summary>
        public static string ToStringWithSpaces(this Int64 number)
        {
            // new version based on NumberFormatInfo
            return number.ToString("C", _Nfi);
        }

        #endregion
    }
}