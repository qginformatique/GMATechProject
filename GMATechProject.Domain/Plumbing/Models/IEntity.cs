namespace GMATechProject.Domain
{
	using MongoDB.Bson.Serialization.Attributes;
	
	/// <summary>
	/// Redefine DreamSongs.MongoRepository.IEntity for easier replacement if needed later.
	/// </summary>
	public interface IEntity
	{
        /// <summary>
        /// Gets or sets the Id of the Entity.
        /// </summary>
        /// <value>Id of the Entity.</value>
        [BsonId]
        string Id { get; set; }		
	}
}
