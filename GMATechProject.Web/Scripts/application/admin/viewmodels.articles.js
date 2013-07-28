jQuery(function ($) {
    // Lazy initialize our namespace context
    if (typeof (window.app) == 'undefined') window.app = {};

    /******************************************************************************
    The ViewModel Responsible for articles management
    ******************************************************************************/
    window.app.ArticlesModel = app.BaseEditionModel.extend({
    	init: function(application, options){
	        // Reference to self: usefull in callback functions
	        var _self = this;
	        // Reference to current document
	        var _doc = $(document);
	        // Reference to this modele root UI element 
	        var _root = $("#articles");
			
			var extendedOptions = $.extend({
				rootElement: '#articles', 
				section: app.Sections.Articles,
				moduleUrl: "/admin/articles",
				itemConstructor: app.Article, 
				messageDelete: function(item){
					return "Voulez-vous vraiment supprimer l'article: " + item.title();
				},
				validationRules: {
                    title: { required: true, notPlaceholder: true, maxlength: 256 },
                    content: { required: true },
                    description: { required: true, notPlaceholder: true, maxlength: 512 },
                    tags: { maxlength: 512 },
                    seotitle: { notPlaceholder: true, maxlength: 256 },
                    seodescription: { notPlaceholder: true, maxlength: 512 },
                    seokeywords: { maxlength: 512 },
                    publicationDate: { customDate: true }
                }
			}, options);

	        // Call the base constructor    
	        this._super(application, extendedOptions);
    	}
	});
});