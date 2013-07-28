jQuery(function ($) {
    // Lazy initialize our namespace context
    if (typeof (window.app) == 'undefined') window.app = {};

    /******************************************************************************
    A single page
    ******************************************************************************/
    window.app.Page = Model.extend({
		init: function (data, mappingOptions) {
	        var _self = this;

	        // Default values for a page
	        var _defaults = {
	            id: '',
	            title: '',
	            content: '',
	            pageTemplate: null,
	            publicationState: 'Draft',
	            friendlyUrl: '',
	            seoTitle: '',
	            seoDescription: '',
	            seoKeywords: [],
	            tags: [],
	            creationDate: new Date(),
	            modificationDate: new Date()
	        };

	        // Extend the supplied data with default values
	        var _extendedData = ko.utils.extend(_defaults, data);

	        // Define how to map the data to make the page
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
	            },
	            modificationDate: {
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