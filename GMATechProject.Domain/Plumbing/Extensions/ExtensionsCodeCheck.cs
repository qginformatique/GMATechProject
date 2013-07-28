namespace PowerOn
{
    #region Using Directives

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    #endregion

    /// <summary>
    /// Extensions methods to check things such as :
    /// <list type="bullet">
    ///     <item>An object is not <see langword="null"/>.</item>
    ///     <item>An lambda expression matching the delegate <see cref="Func{ T, Boolean }"/> is <see langword="true"/>.</item>
    ///     <item>A <see cref="ICollection"/> is not empty.</item>
    /// </list>
    /// </summary>
    [ExcludeFromStackTrace]
    public static class ExtensionsCodeCheck
    {
        #region Ensure Methods

        /// <summary>
        /// Check the given oject meet the condition
        /// In case of a problem, perform a log
        /// http://aabs.wordpress.com/2008/01/18/c-by-contract-using-expression-trees/
        /// </summary>
        [ExcludeFromStackTrace]
        public static bool Ensure< T >( this T obj, Expression< Func< T, bool > > expr )
        {
            return InnerEnsure( obj, expr, null );
        }

        /// <summary>
        /// Check the given oject meet the condition
        /// In case of a problem, perform a log
        /// 
        /// TODO : extend to use with the logger (need to add +1 to the callstack dive)
        /// 
        /// http://aabs.wordpress.com/2008/01/18/c-by-contract-using-expression-trees/
        /// </summary>
        [ExcludeFromStackTrace]
        public static bool Ensure< T >( this T obj, Expression< Func< T, bool > > expr, string message )
        {
            return InnerEnsure( obj, expr, message );
        }

        #region Inner logic

        /// <summary>
        /// Primitive
        /// </summary>
        [ExcludeFromStackTrace]
        static bool InnerEnsure< T >( this T obj, Expression< Func< T, bool > > expr, string message )
        {
            // if no message
            if(string.IsNullOrEmpty(message))
                message = "Unspecified failure";

            // if the object is not a value type, check that it is not null
            if (typeof (T).IsClass)
            {
                // if the object is null
                if (Equals(obj, default(T)))
                {
                    // error message
                    message += " - Checked object is null in ";
                    // add the calling method
                    message += HelperTrace.GetCallingMethod().Name;
                    // log
                    Logger.Error(FailingType.UnexpectedBehavior, null, message);
                    // the expression cannot be true
                    return false;
                }
            }

            // if any condition
            if (expr != null)
            {
                // compile the expression
                var x = expr.Compile();
                // check it
                if (!x(obj))
                {
                    // error message
                    // add the expression text
                    message += " - while checking \"" + expr + "\" - ";
                    // add the calling method
                    message += HelperTrace.GetCallingMethod().Name;
                    // log
                    Logger.Error(FailingType.UnexpectedBehavior, null, message);
                    // the expression returned false
                    return false;
                }
            }
            // if we got there, then no condition has been given and the call was just to ensure the object is not null or 
            // the given expression returned true
            return true;
        }

        #endregion

        #endregion

        #region EnsureIsNotNull Methods

        /// <summary>
        /// Determines whether the specified object is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="message">The log message.</param>
        /// <returns>
        /// 	<see langword="true"/> if the specified object is not <see langword="null"/>; otherwise, <see langword="false"/>.
        /// </returns>
        [ExcludeFromStackTrace]
        public static bool EnsureIsNotNull<T>(this T obj, string message)
            where T : class
        {
            return Ensure(obj, item => item != null, message);
        }

        #endregion

        #region IsNotNull Methods

        /// <summary>
        /// Determines whether the specified object is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// 	<see langword="true"/> if the specified object is not <see langword="null"/>; otherwise, <see langword="false"/>.
        /// </returns>
        [ExcludeFromStackTrace]
        public static bool IsNotNull<T>( this T obj )
            where T : class
        {
            return (obj != null);
        }

        #endregion

        #region IsNotEmpty Methods

        /// <summary>
        /// Determines whether the specified string is null or empty.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// 	<see langword="true"/> if the specified string is null or empty; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsNullOrEmpty( this string obj )
        {
            return string.IsNullOrEmpty( obj );
        }

        /// <summary>
        /// Determines whether the specified string is not null or empty.
        /// </summary>
        [ExcludeFromStackTrace]
        public static bool IsNotEmpty( this string obj )
        {
            return !string.IsNullOrEmpty( obj );
        }

        // OM REMOVED : conflict
        ///// <summary>
        ///// Check the given array is empty or not
        ///// </summary>
        //[ExcludeFromStackTrace]
        //public static bool IsNotEmpty( this Array obj )
        //{
        //    return ( ( obj != null )
        //             && ( obj.Length > 0 ) );
        //}

        /// <summary>
        /// Check the given ArrayList is empty or not
        /// </summary>
        [ExcludeFromStackTrace]
        public static bool IsNotEmpty( this ArrayList obj )
        {
            return ( ( obj != null )
                     && ( obj.Count > 0 ) );
        }

        /// <summary>
        /// Check the given BitArray is empty or not
        /// </summary>
        [ExcludeFromStackTrace]
        public static bool IsNotEmpty( this BitArray obj )
        {
            return ( ( obj != null )
                     && ( obj.Count > 0 ) );
        }

        /// <summary>
        /// Check the given CollectionBase is empty or not
        /// </summary>
        [ExcludeFromStackTrace]
        public static bool IsNotEmpty( this CollectionBase obj )
        {
            return ( ( obj != null )
                     && ( obj.Count > 0 ) );
        }

        /// <summary>
        /// Check the given DictionaryBase is empty or not
        /// </summary>
        [ExcludeFromStackTrace]
        public static bool IsNotEmpty( this DictionaryBase obj )
        {
            return ( ( obj != null )
                     && ( obj.Count > 0 ) );
        }

        /// <summary>
        /// Check the given Hashtable is empty or not
        /// </summary>
        [ExcludeFromStackTrace]
        public static bool IsNotEmpty( this Hashtable obj )
        {
            return ( ( obj != null )
                     && ( obj.Count > 0 ) );
        }

        /// <summary>
        /// Check the given Queue is empty or not
        /// </summary>
        [ExcludeFromStackTrace]
        public static bool IsNotEmpty( this Queue obj )
        {
            return ( ( obj != null )
                     && ( obj.Count > 0 ) );
        }

        /// <summary>
        /// Check the given ReadOnlyCollectionBase is empty or not
        /// </summary>
        [ExcludeFromStackTrace]
        public static bool IsNotEmpty( this ReadOnlyCollectionBase obj )
        {
            return ( ( obj != null )
                     && ( obj.Count > 0 ) );
        }

        /// <summary>
        /// Check the given SortedList is empty or not
        /// </summary>
        [ExcludeFromStackTrace]
        public static bool IsNotEmpty( this SortedList obj )
        {
            return ( ( obj != null )
                     && ( obj.Count > 0 ) );
        }

        /// <summary>
        /// Check the given Stack is empty or not
        /// </summary>
        [ExcludeFromStackTrace]
        public static bool IsNotEmpty( this Stack obj )
        {
            return ( ( obj != null )
                     && ( obj.Count > 0 ) );
        }

        /// <summary>
        /// Check the given generic is empty or not
        /// </summary>
        [ExcludeFromStackTrace]
        public static bool IsNotEmpty< T >( this ICollection< T > obj )
        {
            return ( ( obj != null )
                     && ( obj.Count > 0 ) );
        }

        #endregion
    }
}