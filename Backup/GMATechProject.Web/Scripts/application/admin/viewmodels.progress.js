jQuery(function ($) {
    // Lazy initialize our namespace context
    if (typeof (window.app) == 'undefined') window.app = {};

	var moduleUrl = "/admin/progress";

    /******************************************************************************
    The ViewModel Responsible for progress management
    ******************************************************************************/
    window.app.ProgressModel = app.BaseEditionModel.extend({
    	init: function(application, options){
	        // Reference to self: usefull in callback functions
	        var _self = this;
	        // Reference to current document
	        var _doc = $(document);
	        // Reference to this modele root UI element 
	        var _root = $("#progress");
	        
			var extendedOptions = $.extend({
				rootElement: '#progress', 
				section: app.Sections.Progress,
				moduleUrl: "/admin/progress",
				itemConstructor: app.Progress, 
				messageDelete: function(item){
					return 'Voulez-vous vraiment supprimer l\'évènement: ' + item.title();
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
