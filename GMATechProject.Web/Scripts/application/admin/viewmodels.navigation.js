jQuery(function($) {

    // Lazy initialize our namespace context
    if (typeof (window.app) == 'undefined') window.app = {};
    
    // Constants for the indexes of the navigation menu items (called sections)
    window.app.Sections = {
        Home: 0,
        Progress: 1,
        Articles: 2,
        Members: 3,
        Security: 4, 
        RoleTagsBindings: 5,
        Pages: 6
    };

    // Constants for the labels of the navigation menu items (called sections)
    window.app.SectionsAddress = [
        "accueil",
        "progress",
        "articles",
        "adherents",
        "securite", 
        "roletags",
        "pages"
    ];

    /******************************************************************************
    The ViewModel Responsible for navigation
    ******************************************************************************/
    window.app.NavigationModel = function (application, options) {
        // Reference to self: usefull in callback functions
        var _self = this;
        // Reference to the root HTML element for the navigation
        var _root = $("#sidebar");

        // Extend the default options with the options provided as parameters
        var _options = ko.utils.extend({
            active: app.Sections.Home
        }, options);
		
        // This property track the current active section (by default, the Home section)
        _self.navigationCurrent = ko.sectionObservable(_options.active);

        // This property is used for url history: it returns the label for the currently active section and can set it from the address parameter too
        // (when using the back/forward button of the browser or entering an url with this parameter set directly)
        _self.navigationForHistory = ko.computed({
        // Function used to retrieve the label of the current active section
            read: function() {
                // return the label at the index specified by the navigationCurrent property
                return encodeURIComponent(app.SectionsAddress[_self.navigationCurrent()]);
            },
            // Function used to set the current active section from the url parameter
            write: function(sectionLabel) {
                // Get the index of the section with the specified label
                var sectionIndex = app.SectionsAddress.indexOf(decodeURIComponent(sectionLabel));
                // Set it as the current active section
                _self.navigationCurrent(sectionIndex);
            }
        });

        // This function link the navigationForHistory property to the url parameter named "section"
        // Its default value is the label of the Home section
        ko.linkObservableToUrl(_self.navigationForHistory, "section", app.SectionsAddress[app.Sections.Home]);

        // Computed property returning a boolean which indicates if the current active section is the Home section
        _self.navigationHome = ko.computed(function() {
            return _self.navigationCurrent() == app.Sections.Home;
        });
        
        // Computed property returning a boolean which indicates if the current active section is the Progress section
        _self.navigationProgress = ko.computed(function() {
            return _self.navigationCurrent() == app.Sections.Progress;
        });

        // Computed property returning a boolean which indicates if the current active section is the Articles section
        _self.navigationArticles = ko.computed(function() {
            return _self.navigationCurrent() == app.Sections.Articles;
        });
        
        // Computed property returning a boolean which indicates if the current active section is the Pages section
        _self.navigationPages = ko.computed(function() {
            return _self.navigationCurrent() == app.Sections.Pages;
        });

        // Computed property returning a boolean which indicates if the current active section is the Members section
        _self.navigationMembers = ko.computed(function() {
            return _self.navigationCurrent() == app.Sections.Members;
        });

        // Computed property returning a boolean which indicates if the current active section is the Security section
        _self.navigationSecurity = ko.computed(function() {
            return _self.navigationCurrent() == app.Sections.Security;
        });

        // Computed property returning a boolean which indicates if the current active section is the RoleTagsBindings section
        _self.navigationRoleTagsBindings = ko.computed(function() {
            return _self.navigationCurrent() == app.Sections.RoleTagsBindings;
        });
        
        // Handler for the click event on the navigation-section button
        $(document).on("click", "a[navigation-section]", function (event) {
            event.preventDefault();
            
            var sectionLabel = $(this).attr("navigation-section");
            var sectionIndex = app.Sections[decodeURIComponent(sectionLabel)];
            
            // Change section
            _self.navigationCurrent(sectionIndex);
            
            // === Sidebar navigation === //
			var submenus = $('#sidebar li.has-submenu:not(.active) ul.submenu').slideUp();
			var submenus_parents = $('#sidebar li.has-submenu:not(.active)').removeClass('open');
			
			return false;
        });
    };
});