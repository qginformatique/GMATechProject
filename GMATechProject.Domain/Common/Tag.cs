namespace ABC.Domain
{
	#region Using Directives
	
	using System;
	using System.Collections.Generic;
	
	using MongoDB.Bson.Serialization.Attributes;
	
	#endregion

	[ABC.Domain.Plumbing.CollectionName("Tag")]
	public class Tag : IEntity
	{
		public string Id {
			get {
				return this.TagValue;
			}
			set {
				this.TagValue = value;
			}
		}
		
		[BsonIgnore]
		public string TagValue { get; set; }
		
		public Roles Role { get; set; }
	}
}
