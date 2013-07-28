namespace GMATechProject.Domain.Members
{
	#region Using Directives
	
	using System;
	using System.Collections.Generic;
	
	using GMATechProject.Domain.Plumbing;
	using FluentValidation.Attributes;
	
	#endregion
	
	/// <summary>
	/// Description of Member.
	/// </summary>
	[CollectionName("Member")]
	[Validator(typeof(MemberValidator))]
	public class Member : Entity
	{
		public string Name { get; set; }
		
		public string Address1 { get; set; }
		
		public string Address2 { get; set; }
		
		public string ZipCode { get; set; }
		
		public string City { get; set; }
		
		public string Phone { get; set; }

		public string Email { get; set; }

		public string Website { get; set; }
		
		public string ContactName { get; set; }

		public Genre ContactGenre { get; set; }
		
		public string ContactTitle { get; set; }
		
		public string ContactEmail { get; set; }

		public string ContactPhone { get; set; }

		public string ContactMobile { get; set; }
		
		public bool Professional { get; set; }
		
		public string Service { get; set; }
		
		public DateTime? SubscribingDate { get; set; }

		// Détermine s'il faut afficher ce membre sur la page "Découvrez nos adhérents" visible au public
		public bool DisplayInMembersList {get; set; }
		
		public override string ToString ()
		{
			return string.Format ("[Member: Name={0}, Address1={1}, Address2={2}, ZipCode={3}, City={4}, Phone={5}, Email={6}, Website={7}, ContactName={8}, ContactGenre={9}, ContactTitle={10}, ContactEmail={11}, ContactPhone={12}, ContactMobile={13}, Professional={14}, Service={15}, SubscribingDate={16}, DisplayInMembersList={17}]", Name, Address1, Address2, ZipCode, City, Phone, Email, Website, ContactName, ContactGenre, ContactTitle, ContactEmail, ContactPhone, ContactMobile, Professional, Service, SubscribingDate, DisplayInMembersList);
		}
	}
}