namespace GMATechProject.Domain.Members
{
	#region Using Directives
	
	using System;
	using FluentValidation;

	#endregion
	
	/// <summary>
	/// Validator for members.
	/// </summary>
	public class MemberValidator : AbstractValidator<Member>
	{
		public MemberValidator()
		{
			this.RuleFor(member => member.Name)
				.NotEmpty()
					.WithMessage("Le nom ou la raison sociale est obligatoire")
				.Length(0, 100)
					.WithMessage("Le nom ou la raison sociale ne doit pas dépasser 100 caractères.");
			
			this.RuleFor(member => member.ContactName)
				.Length(0, 100)
					.WithMessage("Le nom du contact ne doit pas dépasser 100 caractères.");

			this.RuleFor(member => member.ContactTitle)
				.Length(0, 256)
					.WithMessage("Le titre (ou statut) du contact ne doit pas dépasser 256 caractères.");

			this.RuleFor(member => member.Email)
				.EmailAddress()
					.WithMessage("L'adresse email est invalide.")
					.Length(0, 256)
					.WithMessage("L'adresse email du contact ne doit pas dépasser 256 caractères.")
					.When(member => member.Email.Length > 0);

			this.RuleFor(member => member.ContactEmail)
				.EmailAddress()
					.WithMessage("L'adresse email est invalide.")
				.Length(0, 256)
					.WithMessage("L'adresse email du contact ne doit pas dépasser 256 caractères.")
				.When(member => member.ContactEmail.Length > 0);
	        
			this.RuleFor(member => member.Address1)
				.NotEmpty()
					.WithMessage("L'adresse est obligatoire")
				.Length(0, 256)
					.WithMessage("L'adresse doit contenir entre 1 et 256 caractères.");
			
			this.RuleFor(member => member.Address2)
				.Length(0, 256)
					.WithMessage("Le complément d'adresse ne doit pas dépasser 256 caractères.");
			
			this.RuleFor(member => member.ZipCode)
				.NotEmpty()
					.WithMessage("Le code postal est obligatoire")
				.Length(5, 5)
					.WithMessage("Le code postal doit contenir 5 chiffres.");
	        
	        this.RuleFor(member => member.City)
				.NotEmpty()
					.WithMessage("La ville est obligatoire est obligatoire")
				.Length(0, 256)
					.WithMessage("La ville doit contenir entre 1 et 256 caractères.");
	        
	        this.RuleFor(member => member.ContactMobile)
				.Length(0, 15)
					.WithMessage("Le numéro de téléphone portable ne doit pas dépasser 15 caractères.");
	        
	        this.RuleFor(member => member.ContactPhone)
				.Length(0, 15)
					.WithMessage("Le numéro de téléphone ne doit pas dépasser 15 caractères.");
        
	        this.RuleFor(member => member.Website)
				.Length(0, 256)
					.WithMessage("L'adresse du site Internet ne doit pas dépasser 256 caractères.");
		}
	}
}