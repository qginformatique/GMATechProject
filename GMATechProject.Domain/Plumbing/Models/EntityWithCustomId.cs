namespace GMATechProject.Domain
{
	public class EntityWithCustomId : IEntity
	{
		#region IEntity implementation
		
		public string Id { get; set; }
		
		#endregion
	}	
}
