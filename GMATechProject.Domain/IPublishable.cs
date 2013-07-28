using System;

namespace GMATechProject.Domain
{
	public interface IPublishable
	{
		/// <summary>
		/// Gets or sets the state of the publication.
		/// </summary>
		/// <value>
		/// The state of the publication.
		/// </value>
		PublicationState PublicationState { get; set; }
	}
}