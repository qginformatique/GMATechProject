namespace GMATechProject.Domain
{
    #region Using Directives

    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    #endregion

    /// <summary>
    /// Extensions methods for Expression.
    /// </summary>
    public static class ExtensionsExpressions
    {
        #region Public Static Methods

        /// <summary>
        /// Gets the MethodInfo for the method that is called in the provided <paramref name="expression"/>
        /// </summary>
        /// <typeparam name="TClass">The type of the class.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>Method info</returns>
        /// <exception cref="ArgumentException">The provided expression is not a method call</exception>
        public static MethodInfo GetMethod< TClass >( this Expression< Action< TClass > > expression )
        {
            var methodCall = expression.Body as MethodCallExpression;

            if ( methodCall == null )
                throw new ArgumentException( "Expected method call" );

            return methodCall.Method;
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <typeparam name="TClass">The type of the class.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static PropertyInfo GetProperty< TClass >( this Expression< Func< TClass, object > > expression )
        {
            MemberExpression memberExpression;

            // if the return value had to be cast to object, the body will be an UnaryExpression
            var unary = expression.Body as UnaryExpression;

            if ( unary != null )
            {
                // the operand is the "real" property access
                memberExpression = unary.Operand as MemberExpression;
            }
            else
            {
                // in case the property is of type object the body itself is the correct expression
                memberExpression = expression.Body as MemberExpression;
            }

            // check whether this expression targets a property
            if ( memberExpression == null || !( memberExpression.Member is PropertyInfo ) )
                throw new ArgumentException( "Expected property expression" );

            return ( PropertyInfo ) memberExpression.Member;
        }

        #endregion
    }
}