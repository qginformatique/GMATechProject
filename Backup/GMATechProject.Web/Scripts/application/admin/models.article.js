jQuery(function ($) {
    // Lazy initialize our namespace context
    if (typeof (window.app) == 'undefined') window.app = {};

    /******************************************************************************
    A single article
    ******************************************************************************/
    window.app.Article = Model.extend({
		init: function (data, mappingOptions) {
	        var _self = this;

	        // Default values for an article
	        var _defaults = {
	            id: '',
	            title: '',
	            content: '',
	            description: '',
	            imageUrl: '',
	            publicationState: 'Draft',
	            friendlyUrl: '',
	            seoTitle: '',
	            seoDescription: '',
	            seoKeywords: [],
	            tags: [],
	            homeSlider: false,
	            creationDate: new Date(),
	            modificationDate: new Date(),
	            publicationDate: new Date()
	        };

	        // Extend the supplied data with default values
	        var _extendedData = ko.utils.extend(_defaults, data);

	        // Define how to map the data to make the article
	        var _mappingOptions = {
	        	tags: {
	        		key: function(model) {
	        			return model;
	        		}
	        	},
	        	seoKeywords: {
	        		key: function(model) {
	        			return model;
	        		}
	        	},
	            creationDate: {
	                // Function called when mapping the creationDate 
	                create: function (options) {
	                    // map it as an observable with the date extension
	                    return ko.observable(new Date(options.data)).date();
	                },
	                 // Function called when mapping the creationDate 
	                update: function (options) {
	                    // map it as an observable with the date extension
	                    return ko.observable(new Date(options.data)).date();
	                }
	            },
	            modificationDate: {
	                // Function called when mapping the modificationDate 
	                create: function (options) {
	                    // map it as an observable with the date extension
	                    return ko.observable(new Date(options.data)).date();
	                },
	                 // Function called when mapping the modificationDate 
	                update: function (options) {
	                    // map it as an observable with the date extension
	                    return ko.observable(new Date(options.data)).date();
	                }
	            },
	            publicationDate: {
	                // Function called when mapping the publicationDate 
	                create: function (options) {
	                    // map it as an observable with the date extension
	                    return ko.observable(new Date(options.data)).date();
	                },
	                 // Function called when mapping the publicationDate 
	                update: function (options) {
	                    // map it as an observable with the date extension
	                    return ko.observable(new Date(options.data)).date();
	                }
	            }
	        };

			// Extend the supplied mappingOptions with default values
	        var _extendedMappingOptions = ko.utils.extend(_mappingOptions, mappingOptions);	 
	        
	        // Call the base constructor    
	        this._super(_extendedData, _extendedMappingOptions);

			this.makeEditable();
			
			// Image full path
	        _self.imageUrlPath = ko.computed(function(){
        		return "/Upload/images/actualites/" + this.imageUrl();
			}, this);
			
			this.enabled = ko.computed(function(){
				return this.publicationState() == 'Published';
			}, this);
			
			this.enable = function(){
				this.publicationState('Published');
			};
			
			this.disable = function(){
				this.publicationState('Draft');
			};					
    	}
    });
});