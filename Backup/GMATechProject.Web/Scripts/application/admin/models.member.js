jQuery(function ($) {
    // Lazy initialize our namespace context
    if (typeof (window.app) == 'undefined') window.app = {};

    /******************************************************************************
    A single member
    ******************************************************************************/
    window.app.Member  = Model.extend({
		init: function (data, mappingOptions) {
			var _self = this;
			
			// Default values for an member
			var _defaults = {
			    id: '',
			    name: '',
			    email: '',
			    website: '',
			    service: '',
			    professional: false,
			    displayInMembersList: false,
			    contactGenre: 'Mister',
			    contactName: '',
			    contactTitle: '',
			    contactEmail: '',
			    phone: '',
			    contactPhone: '',
			    contactMobile: '',
			    address1: '',
			    address2: '',
			    city: '',
			    zipCode: '',
			    subscribingDate: new Date()
			};
			
			// Extend the supplied data with default values
			var _extendedData = ko.utils.extend(_defaults, data);
			
			// Define how to map the data to make the member
			var _mappingOptions = {
				subscribingDate: {
			        // Function called when mapping the subscribingDate  
			        create: function (options) {
			            // map it as an observable with the date extension
			            return ko.observable(new Date(options.data)).date();
			            //return ko.observable(Date.parse(options.data)).date().extend({ editable: true });
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
