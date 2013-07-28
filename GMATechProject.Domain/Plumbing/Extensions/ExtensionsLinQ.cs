using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;

namespace GMATechProject.Domain
{
    /// <summary>
    /// Extensions for LinQ
    /// </summary>
    /// <remarks>
    /// Concerning the sorting by expression, the code for IQueryable and IEnumerable are the same for 98%, but I could not factorize the last 2% with generics.</remarks>
    public static class ExtensionsLinQ
    {
        #region IQueryable sorting using expression

        /// <summary>
        /// Order the given IQueryable using the given sort expression
        /// </summary>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string sortExpression)
        {
            return ApplyOrder<T>(source, sortExpression, "OrderBy");
        }
        /// <summary>
        /// Order the given IQueryable using the given sort expression
        /// </summary>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string sortExpression)
        {
            return ApplyOrder<T>(source, sortExpression, "ThenBy");
        }
        /// <summary>
        /// Inner logic : order the given IQueryable using the given sort expression
        /// </summary>
        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string sortExpression, string methodName)
        {
            IOrderedQueryable<T> retour = null;

            // if we have a given expression
            if(!string.IsNullOrEmpty(sortExpression))
            {
                // split (to check of we have a descending ordering)
                string[] parts = sortExpression.Split(' ');

                // if we have enought parts
                if ((parts.Length > 0)
                    // and if the first one is valid
                    && !string.IsNullOrEmpty(parts[0]))
                {
                    // get the sort property
                    string property = parts[0];

                    // if we have 1 more parameter
                    if ((parts.Length > 1)
                        // and that parameter contains 'desc'
                        && (parts[1].ToLower().Contains("desc")))
                        // then we have a descending ordering
                        methodName += "Descending";

                    // split the property to get sub properties
                    string[] props = property.Split('.');
                    // at this point we are SURE to have at least 1 part in the "props" array

                    // get the type of T
                    Type type = typeof (T);
                    // create an expression
                    ParameterExpression arg = Expression.Parameter(type, "x");
                    Expression expr = arg;
                    // loop on the properties parts
                    int i = 0;
                    while ((i < props.Length)
                           && (expr != null)
                           && (type != null))
                    {
                        // get the property using its name
                        PropertyInfo pi = type.GetNestedProperty(props[i]);
                        // if found
                        if (pi != null)
                        {
                            // add that property to the expression
                            expr = Expression.Property(expr, pi);
                            // now we'll work on that property's type
                            type = pi.PropertyType;
                        }
                        // if not found
                        else
                            // nullate the expression
                            expr = null;

                        i++;
                    }

                    // if we have an expression
                    if (expr != null)
                    {
                        // create a delegate of type Func<T, U>
                        Type delegateType = typeof (Func<,>).MakeGenericType(typeof (T), type);
                        // create a lambda expression fro this delegate to pass into parameter
                        LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

                        // now get the method that satisfy the given conditions :
                        var selection =
                            typeof (Queryable)
                                .GetMethods()
                                // having the given name
                                .FirstOrDefault(method => method.Name == methodName
                                                          // with a generic definition
                                                          && method.IsGenericMethodDefinition
                                                          // with 2 parameters
                                                          && method.GetParameters().Length == 2
                                                          // with 2 generic arguments
                                                          && method.GetGenericArguments().Length == 2
                                );

                        // if found
                        if (selection != null)
                        {
                            // then add the genericparameters and invoke the method
                            retour =
                                (selection.
                                     MakeGenericMethod(typeof (T), type)
                                     .Invoke(null, new object[] {source, lambda})
                                 as IOrderedQueryable<T>);
                        }
                    }
                }
            }

            // if no result
            if(retour ==  null)
                // sort by 0
                retour = source.OrderBy(x => 0);

            return retour;
        }

        #endregion

        #region IEnumerable sorting using expression

        /// <summary>
        /// Order the given IEnumerable using the given sort expression
        /// </summary>
        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string sortExpression)
        {
            return ApplyOrder<T>(source, sortExpression, "OrderBy");
        }
        /// <summary>
        /// Order the given IEnumerable using the given sort expression
        /// </summary>
        public static IOrderedEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> source, string sortExpression)
        {
            return ApplyOrder<T>(source, sortExpression, "ThenBy");
        }
        /// <summary>
        /// Inner logic : order the given IEnumerable using the given sort expression
        /// 
        /// PROBLEM : sometimes the result is an IOrderedEnumerable, sometime a IOrderedQueryable
        /// dunno how to solve this at this moment
        /// to avoid the problem, always convert your source IEnumerable as a IQueryable, using AsQueryable()
        /// </summary>
        static IOrderedEnumerable<T> ApplyOrder<T>(IEnumerable<T> source, string sortExpression, string methodName)
        {
            IOrderedEnumerable<T> retour = null;

            // if we have a given expression
            if(!string.IsNullOrEmpty(sortExpression))
            {
                // split (to check of we have a descending ordering)
                string[] parts = sortExpression.Split(' ');

                // if we have enought parts
                if ((parts.Length > 0)
                    // and if the first one is valid
                    && !string.IsNullOrEmpty(parts[0]))
                {
                    // get the sort property
                    string property = parts[0];

                    // if we have 1 more parameter
                    if ((parts.Length > 1)
                        // and that parameter contains 'desc'
                        && (parts[1].ToLower().Contains("desc")))
                        // then we have a descending ordering
                        methodName += "Descending";

                    // split the property to get sub properties
                    string[] props = property.Split('.');
                    // at this point we are SURE to have at least 1 part in the "props" array

                    // get the type of T
                    Type type = typeof (T);
                    // create an expression
                    ParameterExpression arg = Expression.Parameter(type, "x");
                    Expression expr = arg;
                    // loop on the properties parts
                    int i = 0;
                    while((i < props.Length)
                        && (expr != null)
                        && (type != null))
                    {
                        // get the property using its name
                        PropertyInfo pi = type.GetNestedProperty(props[i]);
                        // if found
                        if(pi != null)
                        {
                            // add that property to the expression
                            expr = Expression.Property(expr, pi);
                            // now we'll work on that property's type
                            type = pi.PropertyType;
                        }
                        // if not found
                        else
                            // nullate the expression
                            expr = null;

                        i++;
                    }
                    // if we have an expression
                    if(expr != null)
                    {
                        // create a delegate of type Func<T, U>
                        Type delegateType = typeof (Func<,>).MakeGenericType(typeof (T), type);
                        // create a lambda expression fro this delegate to pass into parameter
                        LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

                        // now get the method that satisfy the given conditions :
                        var selection =
                            typeof (Enumerable)
                                .GetMethods()
                                // having the given name
                                .FirstOrDefault(method => method.Name == methodName
                                                          // with a generic definition
                                                          && method.IsGenericMethodDefinition
                                                          // with 2 parameters
                                                          && method.GetParameters().Length == 2
                                                          // with 2 generic arguments
                                                          && method.GetGenericArguments().Length == 2
                                );

                        // if found
                        if (selection != null)
                        {
                            // then add the genericparameters and invoke the method
                            retour =
                                (selection.
                                        MakeGenericMethod(typeof (T), type)
                                // NOTE : the lambda must be COMPILED before the call, for IOrderedEnumerable, but not for IOrderedQueryable
                                        .Invoke(null, new object[] { source, lambda.Compile() })
                                    as IOrderedEnumerable<T>);
                        }
                    }
                }
            }

            // if no result
            if(retour == null)
                // sort by 0
                retour = source.OrderBy(x => 0);

            return retour;
        }

        #endregion

        #region ToHashSet

        /// <summary>
        /// Convert given IQueryable to an HashSet
        /// </summary>
        public static HashSet<TKey> ToHashSet<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource,TKey> keyFunc)
        {
            HashSet<TKey> result = new HashSet<TKey>();

            // if input is valid
            if((source != null)
                && (keyFunc != null))
            {
                // loop on the source
                foreach(var item in source.ToList())
                {
                    // get the key
                    var key = keyFunc(item);
                    //  if not added
                    if(!result.Contains(key))
                        // add it to result
                        result.Add(key);
                }
            }
            return result;
        }

        #endregion

        #region Obsolete

        /* V2 : not bad but return a IOrderedEnumerable and not queryable
        /// <summary>
        /// Order by Extension to use a string
        /// 
        /// inspirated by http://stackoverflow.com/questions/1791950/iorderedenumerable-and-defensive-programming
        /// </summary>
        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> list, string sortExpression)
        {
            IOrderedEnumerable<T> retour = null;

            // default sort order is ascending
            bool descending = false;
            string sortProperty = string.Empty;

            // if any input expression
            if(!string.IsNullOrEmpty(sortExpression))
            {
                // split the expression
                string[] parts = sortExpression.Split(' ');

                // if we have enought parts
                if((parts.Length > 0)
                    // and if the first one is valid
                    && !string.IsNullOrEmpty(parts[0]))
                {
                    // get the first part as the sort property
                    sortProperty = parts[0];

                    // if we have 1 more parameter
                    if(parts.Length > 1)
                        //  check if it's 'descending'
                        descending = parts[1].ToLower().Contains("desc");

                }
            }
            // create a key selector function
            Func<T, object> keySelector = (elem) =>
            {
                // sort key
                object selector = 0;

                // if any sort Property
                if(!string.IsNullOrEmpty(sortProperty))
                {
                    // get the property associated to the given 
                    PropertyInfo pi = typeof(T).GetNestedProperty(sortProperty);
                    // if any
                    if(pi != null)
                        // then try to get the value
                        selector = pi.GetValue(elem, null);
                }
                return selector;
            };

            // if descending order
            if(descending)
                retour = list.OrderByDescending(keySelector);
            else
                retour = list.OrderBy(keySelector);

            return retour;
        }

        /// <summary>
        /// Order by Extension to use a string
        /// 
        /// inspirated by http://stackoverflow.com/questions/1791950/iorderedenumerable-and-defensive-programming
        /// </summary>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> list, string sortExpression)
        {
            IOrderedQueryable<T> retour = null;

            // default sort order is ascending
            bool descending = false;
            string sortProperty = string.Empty;

            // if any input expression
            if (!string.IsNullOrEmpty(sortExpression))
            {
                // split the expression
                string[] parts = sortExpression.Split(' ');

                // if we have enought parts
                if ((parts.Length > 0)
                    // and if the first one is valid
                    && !string.IsNullOrEmpty(parts[0]))
                {
                    // get the first part as the sort property
                    sortProperty = parts[0];

                    // if we have 1 more parameter
                    if (parts.Length > 1)
                        //  check if it's 'descending'
                        descending = parts[1].ToLower().Contains("desc");

                }
            }
            // create a key selector function
            Func<T, object> keySelector = (elem) =>
                {
                    // sort key
                    object selector = 0;

                    // if any sort Property
                    if (!string.IsNullOrEmpty(sortProperty))
                    {
                        // get the property associated to the given 
                        PropertyInfo pi = typeof (T).GetNestedProperty(sortProperty);
                        // if any
                        if (pi != null)
                            // then try to get the value
                            selector = pi.GetValue(elem, null);
                    }
                    return selector;
                };

            // if descending order
            if (descending)
                retour = (list.OrderByDescending(keySelector) as IOrderedQueryable<T>);
            else
                retour = (list.OrderBy(keySelector) as IOrderedQueryable<T>);

            return retour;
        }
        */
        /* v1 : return a IEnumerable, not a IOrderedEnumerable
                        /// <summary>
                        /// Order by Extension to use a string
                        /// 
                        /// TODO : v2, check http://stackoverflow.com/questions/1791950/iorderedenumerable-and-defensive-programming
                        /// </summary>
                        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> list, string sortExpression)
                        {
                            IEnumerable<T> retour = list;

                            // if the list is not null
                            if((list != null)
                                // and if any given sort expression
                                && !string.IsNullOrEmpty(sortExpression))
                            {
                                // split the expression
                                string[] parts = sortExpression.Split(' ');

                                // if we have enought parts
                                if((parts.Length > 0)
                                    // and if the first one is valid
                                    && !string.IsNullOrEmpty(parts[0]))
                                {
                                    // default sort order is ascending
                                    bool descending = false;

                                    // if we have 1 more parameter
                                    if(parts.Length > 1)
                                        //  check if it's 'descending'
                                        descending = parts[1].ToLower().Contains("desc");

                                    // get the property associated
                                    PropertyInfo prop = typeof(T).GetNestedProperty(parts[0]);

                                    // if found
                                    if(prop != null)
                                    {
                                        // if the order is descending
                                        if(descending)
                                            // order by this property in descending mode
                                            retour = list.OrderByDescending(x => prop.GetValue(x, null));
                                        // if it's not descending
                                        else
                                            // order by this property in ascending mode
                                            retour = list.OrderBy(x => prop.GetValue(x, null));
                                    }
                                    // if not found
                                    else
                                        // log
                                        Logger.Error("No property '" + parts[0] + "' in + " + typeof(T).Name + "'", null, FailingType.DevInconsistency);
                                }
                            }
                            return retour;
                        }
                */

        #endregion
    }
}
