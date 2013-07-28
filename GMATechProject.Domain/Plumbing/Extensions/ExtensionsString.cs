namespace GMATechProject.Domain
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    #endregion

    /// <summary>
    /// Extensions methods for the <see cref="string"/> class.
    /// </summary>
    public static class ExtensionsString
    {
        #region Constants

        private static readonly Regex DigitsRegex = new Regex(
            @"(\d)",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex ParseEmailRegex = new Regex(
            @"(?:(?<name>.*)\.(?<lastname>.*)|(?<name>.*))@.*",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex SplitWordsRegex = new Regex(
            @"(?<!^)(?=[A-Z])",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Capitalizes the specified string.
        /// </summary>
        /// <example>
        /// <c>"Capitalize ME. Capitalize me Too.".Capitalize()</c> will returns 
        /// <c>"Capitalize me. Capitalize me too."</c>.
        /// </example>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string Capitalize(this string value)
        {
            // first, we check whether the value is null
            if (value == null)
                return string.Empty;

            // Builder used to create the result
            var builder = new StringBuilder(255);

            // We'll work with that string (not modifying the original)
            var workingString = value;

            // We remove leading and ending spaces then we lowercase the whole string
            workingString = workingString.Trim().ToLower();

            // if the string is empty, we have nothing to do
            if (workingString.Length == 0)
                return value;

            // we get the sentences, by splitting the string on each "."
            var sentences = workingString.Split('.');

            // for each sentence
            for (var index = 0; index < sentences.Length; index++)
            {
                // check the sentence is not empty
                if (sentences[index].Trim().Length > 0)
                {
                    // We uppercase the sentence first letter and add it
                    builder.Append(sentences[index].Trim().Substring(0, 1).ToUpper());

                    // Add the rest of the sentence
                    builder.Append(sentences[index].Trim().Substring(1));

                    // if we have more than one sentence (even if one of them is empty), it means we had a point in that 
                    // string, so we add it
                    if (sentences.Length > 1)
                        builder.Append(".");

                    // If we have more sentences to process
                    if (index < sentences.Length - 2)
                    {
                        // Add a space
                        builder.Append(" ");
                    }
                }
            }

            // Return the result
            return builder.ToString();
        }

        /// <summary>
        /// Determines whether this instance contains the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="valueToSearch">The value to search.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns>
        /// 	<see langword="true"/> if this instance contains the specified value; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool Contains(this string value, string valueToSearch, StringComparison comparisonType)
        {
            // first, we check whether the value is null
            if (value == null)
                return false;

            // then, we check whether the valueToSearch is null
            if (valueToSearch == null)
                return false;

            // We call indexOf, specifying the comparison type
            return value.IndexOf(valueToSearch, comparisonType) >= 0;
        }

        /// <summary>
        /// Extracts the text between the two other fragments (<paramref name="start"/> and <paramref name="end"/>).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="start">The start fragment.</param>
        /// <param name="end">The end fragment.</param>
        /// <returns></returns>
        public static string Extract(this string value, string start, string end)
        {
            // first, we check whether the value is null
            if (value == null)
                return string.Empty;

            // then, we check whether the start is null
            if (start == null)
                return string.Empty;

            // finally, we check whether the end is null
            if (end == null)
                return string.Empty;

            // Get the index of the start string
            var startIndex = value.IndexOf(start) + start.Length;

            // If the index is -1, the start string was not found
            if (startIndex == -1)
            {
                // Return an empty string
                return string.Empty;
            }

            // Get the length to extract by getting the index of the end string and removing the startIndex
            var lengthToExtract = value.IndexOf(end) - startIndex;

            // If the length to extract is less than 0, the end string was not found
            if (lengthToExtract < 0)
            {
                // Return an empty string
                return string.Empty;
            }

            // Return the extracted string
            return value.Substring(startIndex, lengthToExtract);
        }

        /// <summary>
        /// Gets the value or default.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// Returns <paramref name="value"/> if it is not <see langword="null"/> or empty; otherwise, <see cref="string.Empty"/>.
        /// </returns>
        [Obsolete("Use the ?? operator instead")]
        public static string GetValueOrDefault(this string value)
        {
            // Simply call the overload which takes a default value by specifying an empty string
            return GetValueOrDefault(value, string.Empty);
        }

        /// <summary>
        /// Gets the value or default.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Returns <paramref name="value"/> if it is not <see langword="null"/> or empty; otherwise, <paramref name="defaultValue"/>.</returns>
        [Obsolete("Use the ?? operator instead")]
        public static string GetValueOrDefault(this string value, string defaultValue)
        {
            // Check whether the value is null or empty
            if (string.IsNullOrEmpty(value))
                // If it is, return the default value
                return defaultValue;

            // If it was not null or empty, return the value
            return value;
        }

        /// <summary>
        /// Guesses a name from an email adress.
        /// </summary>
        /// <example>
        /// <para>
        ///	<c>"toto.titi@tutu.com".GuessNameFromEmail()</c> would returns <c>Toto Titi</c>.
        /// </para>
        ///	<c>"toto@tutu.com".GuessNameFromEmail()</c> would returns <c>Toto</c>.
        /// <para>
        /// </para>
        /// </example>
        /// <param name="value">The value.</param>
        /// <returns>
        /// Capitalized name if found; otherwise <paramref name="value"/>.
        /// </returns>
        public static string GuessNameFromEmail(this string value)
        {
            // first, we check whether the value is null
            if (value == null)
                return string.Empty;

            var result = value;

            // Call our Regex
            var match = ParseEmailRegex.Match(value);

            // If we have a match
            if (match.Success)
            {
                // The first group is the first name (or last name)
                var name = match.Groups[1].Value;

                // We may have a second group which will be the last name (or first name)
                var lastname = match.Groups.Count > 2 ? match.Groups[2].Value : null;

                // The result will be the name, capitalized 
                result = name.Capitalize();

                // If we found a last name, add it, capitalized
                if (!string.IsNullOrEmpty(lastname))
                    result += " " + lastname.Capitalize();
            }

            // we remove any digits
            result = DigitsRegex.Replace(result, string.Empty);

            return result;
        }

        /// <summary>
        /// Makes a "pseudo" sentence from the value : split it into words and 
        /// join the resulting words with a space as separator.
        /// </summary>
        /// <example>
        /// <c>"MakePseudoSentence".MakePseudoSentence()</c> will returns 
        /// <c>"Make pseudo sentence"</c>.
        /// </example>
        /// <param name="value">The value.</param>
        /// <returns>A pseudo sentence from the value</returns>
        public static string MakePseudoSentence(this object value)
        {
            var result = string.Empty;

            // if the string is not empty
            if (value != null)
            {
                // We split the string value of the object into words and join the result by inserting spaces
                result = string.Join(" ", value.ToString().SplitWords());

                // Return the string capitalized
                return result.Capitalize();
            }

            // Simply return the empty string
            return result;
        }

        /// <summary>
        /// Replaces the spaces by underscores.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string MakeSpacesUnderscores(this string value)
        {
            // if the string is not empty
            if (!string.IsNullOrEmpty(value))
            {
                // Replace the spaces by underscores
                return value.Replace(' ', '_');
            }

            return string.Empty;
        }

        /// <summary>
        /// Determines whether the beginning of this instance matches any of the specified values.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="valuesToSearch">The values to search.</param>
        /// <returns>
        /// 	<see langword="true"/> if the beginning of this instance matches any of the specified values; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool StartsWith (this string value, params string[] valuesToSearch)
		{
			// if the string is empty
			if (string.IsNullOrEmpty (value))
                return false;

            return valuesToSearch
                // Take only the values which are not null or empty
                .Where(search => !string.IsNullOrEmpty (search))
                // Call the StartsWith method for each search value
                .Any(t => value.StartsWith(t));
        }

        /// <summary>
        /// Parses a camel cased or pascal cased string and returns an array
        /// of the words within the string.
        /// </summary>
        /// <example>
        /// The string "PascalCasing" will return an array with two
        /// elements, "Pascal" and "Casing".
        /// </example>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string[] SplitWords(this string value)
        {
            // If the value is null, return an empty array
            if (value == null) return new string[] { };

            // If the value is empty, return an array with an empty string
            if (value.Length == 0) return new[] { "" };

            var words = new StringCollection();

            var wordStartIndex = 0;

            var letters = value.ToCharArray();

            // Skip the first letter. we don't care what case it is.
            for (var index = 1; index < letters.Length; index++)
            {
                if (char.IsUpper(letters[index]))
                {
                    // Grab everything before the current index.
                    words.Add(new String(letters, wordStartIndex, index - wordStartIndex));

                    wordStartIndex = index;
                }
            }

            // We need to have the last word.
            words.Add(new String(letters, wordStartIndex, letters.Length - wordStartIndex));

            // Copy to a string array.
            var wordArray = new string[words.Count];

            words.CopyTo(wordArray, 0);

            return wordArray;
        }

        /// <summary>
        /// Get a camel case version of a Pascal case string (such as a type's name).
        /// </summary>
        /// <param name="value">The Pascal case string.</param>
        /// <returns>A camel case version of a Pascal case string.</returns>
        public static string ToCamelCase (this string value)
		{
			string result = null;

			// Check whether the value is null or empty
			if (!string.IsNullOrEmpty (value)) {
                // If it is valid, split it into words
                var words = SplitWords(value);

                // Loop over the words
                for (var index = 0; index < words.Length; index++)
                {
                    // If this is the first word
                    if (index == 0)
                    {
                        // Add it in lower case
                        result += words[index].ToLowerInvariant();
                    }
                    // If this is not the first word
                    else
                    {
                        // simply add it (it should already be capitalized)
                        result += words[index];
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get a Pascal case version of a camel case string (such as a type's name).
        /// </summary>
        /// <param name="value">The camel case string.</param>
        /// <returns>A Pascal case version of a camel case string.</returns>
        public static string ToPascalCase (this string value)
		{
			string result = null;

			// Check whether the value is null or empty
			if (!string.IsNullOrEmpty (value)) {
                // If it is valid, split it into words
                var words = SplitWords(value);

                // Loop over the words
                for (var index = 0; index < words.Length; index++)
                {
                    // If this is the first word
                    if (index == 0)
                    {
                        // Capitalize it (in case the original value was in camel case)
                        result += words[index][0].ToString().ToUpperInvariant() + words[index].Substring(1).ToLowerInvariant();
                    }
                    // If this is not the first word
                    else
                    {
                        // simply add it (it should already be capitalized)
                        result += words[index];
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Remove HTML tags from string using char array method.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="replaceBreakTagsWithLineBreak">if set to <see langword="true"/>, replaces the break tags (&lt;br /&gt;) with line break.</param>
        /// <returns></returns>
        public static string ConvertHtmlToPlainText(this string value, bool replaceBreakTagsWithLineBreak = true)
        {
            var cleanedValue = value;

            string[] oldWords = { "&nbsp;", "&amp;", "&quot;", "&lt;", "&gt;", "&reg;", "&copy;", "&bull;", "&trade;" };
            string[] newWords = { " ", "&", "\"", "<", ">", "®", "©", "•", "™" };

            for (var index = 0; index < oldWords.Length; index++)
            {
                cleanedValue = cleanedValue.Replace(oldWords[index], newWords[index]);
            }

            // Check if there are paragraphs (<p>)
            cleanedValue = cleanedValue.Replace("<p ", "\n<p ");

            var array = new char[cleanedValue.Length];
            var arrayIndex = 0;
            var inside = false;
            string tag = null;

            for (var index = 0; index < cleanedValue.Length; index++)
            {
                var let = cleanedValue[index];

                if (let == '<')
                {
                    inside = true;

                    if (replaceBreakTagsWithLineBreak)
                        tag = string.Empty;

                    continue;
                }

                if (let == '>')
                {
                    if (replaceBreakTagsWithLineBreak && inside && tag != null && tag.ToLowerInvariant() == "br")
                    {
                        var newline = Environment.NewLine;

                        for (var j = 0; j < newline.Length; j++)
                        {
                            array[arrayIndex] = newline[j];
                            arrayIndex++;
                        }
                    }

                    inside = false;
                    continue;
                }

                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
                else
                {
                    if (replaceBreakTagsWithLineBreak)
                        tag += let;
                }
            }

            return new string(array, 0, arrayIndex);
        }

        #endregion

        #region String to guid

        /// <summary>
        /// Convert a string to a guid
        /// 
        /// TODO : use Guid.TryParse when migrating to fx 4.0
        /// </summary>
        public static Guid ToGuid (this string input)
		{
			Guid retour;

			// if the string is valid
			if (!string.IsNullOrEmpty (input)) {
                try
                {
                    // try to convert
                    retour = new Guid(input);
                }
                catch (FormatException)
                {
                    retour = Guid.Empty;
                }
            }
            // if input is not valid
            else
                // return default value
                retour = Guid.Empty;

            return retour;
        }

        #endregion

        #region Formatting

        /// <summary>
        /// Formats the string using the specified parameters.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static string FormatMe( this string value, params object[] parameters )
        {
            // Check whether the value is null or empty
			if (!string.IsNullOrEmpty (value))
            {
                // If it is valid, call string.Format with it and the parameters
                value = string.Format( value, parameters );
            }

            // If anything goes wrong, return the unchanged value
            return value;
        }

        /// <summary>
        /// Used to get placeholders from a regex
        /// </summary>
        private static readonly Regex RegExGetFormattingPlaceHolders = new Regex(@"\{(?<index>\d+)\}", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>
        /// Fail safe verion of formatMe
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>resulting string</returns>
        public static string FormatMeFailSafe (this string value, params object[] parameters)
		{
			// if we have a valid string 
			if (!string.IsNullOrEmpty (value)) {
				// get all matches
				var matches = RegExGetFormattingPlaceHolders.Matches (value);

				// if any
				if (matches.Count > 0) {
					// get the max of those groups
					var maxIndex =
                        (from match in matches.Cast<Match> ()

                         select Int32.Parse (match.Groups ["index"].Value)
                        ).Max ();

					// create a new parameters array
					var parametersNew = new List<object> ();
					// loop
					for (int i = 0; i <= maxIndex; i++) {
						object item = null;

						// get current value
						if ((parameters != null)
							&& (i < parameters.Length)) {
							// get the parameters
							var parameter = parameters [i];
							// if not null
							if (parameter != null) {
								// if it's a type
								var type = (parameter as Type);
								// if it's one
								if (type != null) {
									// use specific type formatting
									item = type.GetFullNameSafe();
                                }
                                // if it's not a type
                                else
                                {
                                    try
                                    {
                                        // try to convert that item as a string
                                        item = parameter.ToString();
                                    }
                                    catch (Exception exc)
                                    {
                                        // if we had an exception, use its type
                                        item = "(" + exc.GetType().Name + ")";
                                    }
                                }
                            }
                        }

                        // if no item
                        if (item == null)
                            // use explicit (null) instead
                            item = "(null)";

                        // add a null parameter
                        parametersNew.Add(item);
                    }
                    // do the formatting
                    var array = parametersNew.ToArray();
                    value = value.FormatMe(array);
                }
                // if no formatting parameters, then just return the input string
            }
            // If anything goes wrong, return the unchanged value
            return value;
        }

        #endregion
    }
}


namespace PInvoke
{
    #region Using Directives

    using System;
    using System.Runtime.InteropServices;

    #endregion

    class ObjBase
    {
        /// <summary>
        /// This function converts a string generated by the StringFromCLSID function back into the original class identifier.
        /// </summary>
        /// <param name="sz">String that represents the class identifier</param>
        /// <param name="clsid">On return will contain the class identifier</param>
        /// <returns>
        /// Positive or zero if class identifier was obtained successfully
        /// Negative if the call failed
        /// </returns>
        [DllImport("ole32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true)]
        public static extern int CLSIDFromString(string sz, out Guid clsid);
    }
}
