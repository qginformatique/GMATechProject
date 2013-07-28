jQuery(function ($) {
    // Lazy initialize our namespace context
    if (typeof (window.app) == 'undefined') window.app = {};
    
    /******************************************************************************
    Class ApplicationModel
    ******************************************************************************/
    window.app.ApplicationModel = function (options) {    
    	var _self = this;

		_self.identity = new app.Identity(options.identity);
		
    	_self.articles = new app.ArticlesModel(this, options.articles);
    	    	
    	_self.members = new app.MembersModel(this, options.members);

        ko.applyBindings(_self);
    };
    
    // Initalise images LazyLoad
	$("img[data-original]").lazyload().addClass("lazy-image");
	
	// Initialiser FancyBox
	// Utilisé sur la page Galerie -> Trophées du Bois
	$("a.fancy").fancybox();	   	
});