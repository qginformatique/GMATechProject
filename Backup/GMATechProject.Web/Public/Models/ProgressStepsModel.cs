namespace GMATechProject.Web.Public.Models
{
	#region Using Directives

	using System;
	using System.Collections.Generic;

	using GMATechProject.Domain;
	using GMATechProject.Domain.Progress;
	
	#endregion

	/// <summary>
	/// Description of ProgressStepsModel.
	/// </summary>
	public class ProgressStepsModel : PublicSimpleModel
	{
		public IList<ProgressStepExtended> ProgressSteps { get; set; }

		public ProgressStepsModel(bool useConcatenatedResources, bool registerNewUser = false) : base (useConcatenatedResources, registerNewUser)
		{
		}
	}

	public class ProgressStepExtended : ProgressStep
	{
		public ProgressStepExtended()
		{
			Items = new List<ProgressStep> ();
		}

		public IList<ProgressStep> Items { get; set; }
	}
}