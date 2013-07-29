namespace GMATechProject.Domain.Progress
{
	#region Using Directives

	using System;
	using FluentValidation;

	#endregion
	
	/// <summary>
	/// Validator for ProgressStepValidator.
	/// </summary>
	public class ProgressStepValidator : AbstractValidator<ProgressStep>, IValidator<ProgressStep>
	{
		public ProgressStepValidator()
		{
			this.RuleFor(progressStep => progressStep.Title)
				.NotEmpty()
					.WithMessage("Le titre est obligatoire.")
				.Length(0, 256)
					.WithMessage("Le titre ne doit pas dépasser 256 caractères.");
		}
	}
}
