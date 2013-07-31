namespace GMATechProject.Web.Public.Models
{
	#region Using Directives

	using System;
	using System.Collections.Generic;

	using GMATechProject.Domain;
	using GMATechProject.Domain.Suggestion;
	
	#endregion

	/// <summary>
	/// Description of SuggestionModel.
	/// </summary>
	public class SuggestionModel : PublicSimpleModel
	{
		public PaginatedList<Suggest> Suggestions { get; set; }

		public SuggestionModel(bool useConcatenatedResources, bool registerNewUser = false) : base (useConcatenatedResources, registerNewUser)
		{
		}
	}
}