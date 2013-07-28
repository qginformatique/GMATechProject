namespace GMATechProject.Domain.Blog
{
	#region Using Directives

	using System;
	using FluentValidation;

	#endregion
	
	/// <summary>
	/// Validator for articles.
	/// </summary>
	public class ArticleValidator : AbstractValidator<Article>, IValidator<Article>
	{
		public ArticleValidator()
		{
			this.RuleFor(article => article.Title)
				.NotEmpty()
					.WithMessage("Le titre est obligatoire.")
				.Length(0, 256)
					.WithMessage("Le titre ne doit pas dépasser 256 caractères.");
			
			this.RuleFor(article => article.Description)
				.NotEmpty()
					.WithMessage("La description est obligatoire.")
				.Length(0, 512)
					.WithMessage("La description ne doit pas dépasser 512 caractères.");
		}
	}
}
