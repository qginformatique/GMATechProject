namespace GMATechProject.Domain.Security
{
	public interface IIdentityRepository : IRepository<Identity>
	{	
		Identity FindByEmail(string email);
		
		Identity FindByEmailAndPassword (string email, string password);
	}
}

