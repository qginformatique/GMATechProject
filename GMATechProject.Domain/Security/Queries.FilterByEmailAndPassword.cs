using System;
using System.Linq;

namespace ABC.Domain
{
	public static partial class SecurityQueries
	{
		public static IQueryable<Identity> FilterByEmailAndPassword (this IQueryable<Identity> query, string email, string password)
		{
			var passwordUtility = new PasswordUtility();

			// Get the hashed version of the password
			var hashedPassword = passwordUtility.HashPassword (password);
			
			var result = 
				(from identity in query
				 where identity.Email == email
				 where identity.Password == hashedPassword
				 select identity);

			return result;
		}	
	}
}

