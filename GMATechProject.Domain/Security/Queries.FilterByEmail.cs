using System;
using System.Linq;

namespace ABC.Domain
{
	public static partial class SecurityQueries
	{
		public static IQueryable<Identity> FilterByEmail(this IQueryable<Identity> query, string email)
		{
			var result = 
				(from identity in query
				 where identity.Email == email
				 select identity);

			return result;
		}
	}
}

