namespace GMATechProject.Domain.Suggestion
{
	#region Using Directives

	using System;
	using FluentValidation;

	#endregion
	
	/// <summary>
	/// Validator for SuggestValidator.
	/// </summary>
	public class SuggestValidator : AbstractValidator<Suggest>, IValidator<Suggest>
	{
		public SuggestValidator()
		{
			this.RuleFor(item => item.Title)
				.NotEmpty()
					.WithMessage("Le titre est obligatoire.")
				.Length(0, 256)
					.WithMessage("Le titre ne doit pas dépasser 256 caractères.");
		}
	}
}
