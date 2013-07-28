namespace GMATechProject.Web.Public.Models
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Linq;
	
	#endregion

	public class MenuSimpleNavItem
	{
		public string Label { get; set; }
		public string History { get; set; }
		public string Css { get; set; }
		public string Href { get; set; }
		public MenuSimpleNavItemPosition Position { get; set; }
		public string IsActive { get; set; }

		public MenuSimpleNavItem()
		{
			Position = MenuSimpleNavItemPosition.MenuLeft;
		}
	}

	public enum MenuSimpleNavItemPosition {
		MenuLeft,
		MenuRight,
		Footer,
		Manual
	}

	/// <summary>
	/// Description of MenuSimpleModel.
	/// </summary>
	public class MenuSimpleModel
	{	
		// Menu Sections
		public IList<MenuSimpleNavItem> NavSections { get; set; }

		// = Menu Sections where position === menu
		public IList<MenuSimpleNavItem> NavSectionsMenuLeft { get; set; }

		// = Menu Sections where position === menu
		public IList<MenuSimpleNavItem> NavSectionsMenuRight { get; set; }

		// = Menu Sections where position === menu
		public IList<MenuSimpleNavItem> NavSectionsMenu { get; set; }

		// = Menu Sections where position === footer
		public IList<MenuSimpleNavItem> NavSectionsFooter { get; set; }

		// = Menu current active section (home by default)
		public MenuSimpleNavItem NavigationCurrent { get; set; }

		// = Menu current nav section (home by default) ?? same as current active section with history initialiser?
		public MenuSimpleNavItem CurrentNavSection { get; set; }

		public MenuSimpleModel()
		{
			// Pre Populate
			var sections = new List<MenuSimpleNavItem>();

			sections.Add(new MenuSimpleNavItem(){ Label = "Accueil", History = "", Css = "menu-home"});
			sections.Add(new MenuSimpleNavItem(){ Label = "Progression", History = "progression", Css = "menu-progression" });
			sections.Add(new MenuSimpleNavItem(){ Label = "Propositions", History = "propositions", Css = "menu-propositions" });
			sections.Add(new MenuSimpleNavItem(){ Label = "Nous contacter", History = "contact", Css = "menu-contact", Position = MenuSimpleNavItemPosition.MenuRight });

			sections.Add(new MenuSimpleNavItem(){ Label = "Mentions Légales", History = "mentions-legales", Css = "mentions-legales-link", Position = MenuSimpleNavItemPosition.Footer });
								
			NavSections = sections;
			NavSectionsMenuLeft = NavSections.Where(item => item.Position == MenuSimpleNavItemPosition.MenuLeft).ToList();
			NavSectionsMenuRight = NavSections.Where(item => item.Position == MenuSimpleNavItemPosition.MenuRight).ToList();
			NavSectionsFooter = NavSections.Where(item => item.Position == MenuSimpleNavItemPosition.Footer).ToList();
		}
	}
}