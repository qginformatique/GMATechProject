namespace GMATechProject.Domain.Pages
{
	#region Using Directives

	using System;
	using FluentValidation;

	#endregion
	
	/// <summary>
	/// Validator for page.
	/// </summary>
	public class PageValidator : AbstractValidator<Page>, IValidator<Page>
	{
		public PageValidator()
		{
			this.RuleFor(page => page.Title)
				.NotEmpty()
					.WithMessage("Le titre est obligatoire.")
				.Length(0, 256)
					.WithMessage("Le titre ne doit pas dépasser 256 caractères.");

			/*this.RuleFor (page => page.CreationDate)
				.Must (ValidationMethods.BeAValidDate)
					.WithMessage ("Date incorrecte.");

			this.RuleFor(page => page.Title)
				.Must (ValidationMethods.BeAValidDate)
					.WithMessage ("Date incorrecte.");*/
		}
	}
}
