namespace GMATechProject.Domain.Suggestion
{
	#region Using Directives
	
	using System;
	using System.Collections.Generic;

	using GMATechProject.Domain.Plumbing;

	using FluentValidation.Attributes;

	#endregion

	/// <summary>
	/// Description of Suggest.
	/// </summary>
	[CollectionName("Suggest")]
	[Validator(typeof(SuggestValidator))]
	public class Suggest : Entity, IPublishable
	{
		public Suggest ()
		{
		}
		
		public string Title { get; set; }

		public string Description { get; set; }

		public PublicationState PublicationState { get; set; }

		public DateTime CreationDate { get; set; }

		public DateTime ModificationDate { get; set; }

		public string FriendlyUrl { get; set; }

		public HashSet<string> Tags { get; set; }

		// Informations complémentaires affichées en pied de page de l'évènement
		public string Notes { get; set; }

		public override string ToString ()
		{
			return string.Format ("[Event: Title={0}, Description={1}, " +
			                      "PublicationState={2}, CreationDate={3}, ModificationDate={4}, " +
			                      "FriendlyUrl={5}, Tags={6}, Notes={7}]", 
			                      Title, Description, 
			                      PublicationState, CreationDate, ModificationDate, 
			                      FriendlyUrl, Tags, Notes);
		}
	}
}