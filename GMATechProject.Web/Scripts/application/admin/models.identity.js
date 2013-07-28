jQuery(function ($) {
    // Lazy initialize our namespace context
    if (typeof (window.app) == 'undefined') window.app = {};

    /******************************************************************************
    A single identity
    ******************************************************************************/
    window.app.Identity = Model.extend({
		init: function (data, mappingOptions) {
	        var _self = this;

	        // Default values for an identity
	        var _defaults = {
	            id: '',
	            email: '',
	            role: '',
	            creationDate: new Date().toString("dd/MM/yyyy")
	        };

	        // Extend the supplied data with default values
	        var _extendedData = ko.utils.extend(_defaults, data);

	        // Define how to map the data to make the identity
	        var _mappingOptions = {
	            creationDate: {
	                // Function called when mapping the publicationDate 
	                create: function (options) {
	                    // map it as an observable with the date extension
	                    return ko.observable(Date.parse(options.data)).extend({ editable: true }).date();
	                }
	            }
	        };
	        
	        // Extend the supplied mappingOptions with default values
	        var _extendedMappingOptions = ko.utils.extend(_mappingOptions, mappingOptions);	 
	        
	        // Call the base constructor    
	        this._super(_extendedData, _extendedMappingOptions);
		
			this.makeEditable();
    	}
    });
});


