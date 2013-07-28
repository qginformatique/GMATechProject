jQuery(function ($) {
    // Lazy initialize our namespace context
    if (typeof (window.app) == 'undefined') window.app = {};

    /******************************************************************************
    The ViewModel Responsible for pages management
    ******************************************************************************/
    window.app.PagesModel = app.BaseEditionModel.extend({
    	init: function(application, options){
	        // Reference to self: usefull in callback functions
	        var _self = this;
	        // Reference to current document
	        var _doc = $(document);
	        // Reference to this modele root UI element 
	        var _root = $("#pages");
			
			var extendedOptions = $.extend({
				rootElement: '#pages', 
				section: app.Sections.Pages,
				moduleUrl: "/admin/pages",
				itemConstructor: app.Page, 
				messageDelete: function(item){
					return "Voulez-vous vraiment supprimer la page: " + item.title();
				},
				validationRules: {
                    title: { required: true, notPlaceholder: true, maxlength: 256 },
                    content: { required: true },
                    tags: { maxlength: 512 },
                    friendlyUrl: { required: true, maxlength: 256 },
                    seotitle: { notPlaceholder: true, maxlength: 256 },
                    seodescription: { notPlaceholder: true, maxlength: 512 },
                    seokeywords: { maxlength: 512 },
                    publicationDate: { customDate: true }
                }
			}, options);

	        // Call the base constructor    
	        this._super(application, extendedOptions);
	        
	        // PageTemplates Initialization
	        _self.pageTemplate = ko.observableArray([{
				text: "Normal",
				value: "Normal"
			}, {
				text: "Page métier",
				value: "Metier"
			}, {
				text: "Page sans titre",
				value: "SansTitre"
			}]);
    	}
	});
});