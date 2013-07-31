jQuery(function ($) {
    // Lazy initialize our namespace context
    if (typeof (window.app) == 'undefined') window.app = {};

	var moduleUrl = "/admin/suggestion";

    /******************************************************************************
    The ViewModel Responsible for suggestions management
    ******************************************************************************/
    window.app.SuggestionModel = app.BaseEditionModel.extend({
    	init: function(application, options){
	        // Reference to self: usefull in callback functions
	        var _self = this;
	        // Reference to current document
	        var _doc = $(document);
	        // Reference to this modele root UI element 
	        var _root = $("#suggestion");
	        
			var extendedOptions = $.extend({
				rootElement: '#suggestion', 
				section: app.Sections.Suggestion,
				moduleUrl: "/admin/suggestion",
				itemConstructor: app.Suggest, 
				messageDelete: function(item){
					return 'Voulez-vous vraiment supprimer la suggestion: ' + item.title();
				},
				validationRules: {
                    title: { required: true, notPlaceholder: true, maxlength: 256 },
                    description: { required: true, notPlaceholder: true, maxlength: 2048 },
                    notes: { maxlength: 1024 },
                    tags: { maxlength: 512 }
                }
			}, options);

	        // Call the base constructor    
	        this._super(application, extendedOptions);
	    }
    });
});
