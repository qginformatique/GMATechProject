namespace GMATechProject.Domain.Security
{
	#region Using Directives
	
	using System;
	using System.Linq;
	
	using GMATechProject.Domain.Plumbing;

	#endregion 

	public class IdentityRepository : Repository<Identity>, IIdentityRepository
	{	
		private readonly IPasswordUtility _PasswordUtility;

		public IdentityRepository (IPasswordUtility passwordUtility)
		{
			this._PasswordUtility = passwordUtility;
		}
		
		public override PaginatedList<Identity> QuickSearch (string queryTerms, int pageIndex, int pageSize)
		{
			var query = 
				(from identity in this.All ()
				 where identity.Email.Contains (queryTerms)
				 select identity);

			var result = query.ToPaginatedList (pageIndex, pageSize);
			
			return result;
		}
				
		public Identity FindByEmailAndPassword (string email, string password)
		{
			Identity result = null;
			
			// Get the hashed version of the password
			var hashedPassword = this._PasswordUtility.HashPassword (password);
			
			var query = 
				(from identity in this.All ()
				 where identity.Email == email
				 where identity.Password == hashedPassword
				 select identity);
			
			result = query.FirstOrDefault ();
			
			return result;
		}

				
		public Identity FindByEmail(string email)
		{
			Identity result = null;
						
			var query = 
				(from identity in this.All ()
				 where identity.Email == email
				 select identity);
			
			result = query.FirstOrDefault ();
			
			return result;
		}
	}
}

