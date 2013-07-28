using System;
using System.Collections.Generic;
using System.Linq;

namespace ABC.Domain
{
    /// <summary>
    /// Extension for enumerables
    /// </summary>
    public static class ExtensionsEnumerable
    {
        /// <summary>
        /// Convert given IEnumerable to a IListedData
        /// </summary>
        public static PaginatedList<T> ToPaginatedList<T>(this IEnumerable<T> source, int index, int size, int totalItems)
        {
            return new PaginatedList<T>(source, index, size, totalItems);
        }
        /// <summary>
        /// Convert given IQueryable to a IListedData
        /// </summary>
        public static PaginatedList<T> ToPaginatedList<T> (this IQueryable<T> source, int index, int size, int totalItems)
		{
			return new PaginatedList<T>(source, index, size, totalItems);
        }
        /// <summary>
        /// This tools is used to ensure we have no memory leak
        /// as it as been spotted experimentally
        /// </summary>
        public static void ClearSafe<TObject>(this ICollection<TObject> items)
        {
            // if the colection is not null
            if((items != null)
                && !items.IsReadOnly)
            {
                // empty
                items.Clear();
            }
        }
    }
}
