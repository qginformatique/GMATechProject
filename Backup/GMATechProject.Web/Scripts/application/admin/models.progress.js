jQuery(function ($) {
    // Lazy initialize our namespace context
    if (typeof (window.app) == 'undefined') window.app = {};

    /******************************************************************************
    A single progress
    ******************************************************************************/
    window.app.Progress = Model.extend({
		init: function (data, mappingOptions) {
	        var _self = this;

	        // Default values for an event
	        var _defaults = {
	            id: '',
	            title: '',
	            description: '',
	            publicationState: 'Draft',
	            creationDate: new Date(),
	            modificationDate: new Date(),
	            notes: '',
	            currentProgress: 0,
	            friendlyUrl: '',
	            tags: []
	        };	    	
	
	        // Extend the supplied data with default values
	        var _extendedData = ko.utils.extend(_defaults, data);

	        // Define how to map the data to make the event
	        var _mappingOptions = {
	        	tags: {
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
	            }
	        };
	        
			// Extend the supplied mappingOptions with default values
	        var _extendedMappingOptions = ko.utils.extend(_mappingOptions, mappingOptions);	 	        
	        
	        // Call the base constructor    
	        _self._super(_extendedData, _extendedMappingOptions);
	        
			// Apply editable extension on the whole object
			_self.makeEditable();
			
			_self.enabled = ko.computed(function(){
				return _self.publicationState() == 'Published';
			}, _self);
			
			_self.enable = function(){
				_self.publicationState('Published');
			};
			
			_self.disable = function(){
				_self.publicationState('Draft');
			};
		}
	});
});