jQuery(function ($) {
	// Lazy initialize our namespace context
	if (typeof (window.app) == 'undefined') window.app = {};	
	
    /******************************************************************************
    A single identity
    ******************************************************************************/
    window.app.Identity = Model.extend({
		init: function (data, mappingOptions) {
	        var _self = this;
	        var _doc = $(document);

	        // Default values for an identity
	        var _defaults = {
	            id: '',
	            email: '',
	            role: '',
	            creationDate: new Date(), 
	            mustRegister: false
	        };

	        // Extend the supplied data with default values
	        var _extendedData = ko.utils.extend(_defaults, data);

	        // Define how to map the data to make the identity
	        var _mappingOptions = {
	            creationDate: {
	                // Function called when mapping the publicationDate 
	                create: function (options) {
	                    // map it as an observable with the date extension
	                    return ko.observable(new Date(options.data)).date();
	                }
	            }
	        };
	        
	        // Extend the supplied mappingOptions with default values
	        var _extendedMappingOptions = ko.utils.extend(_mappingOptions, mappingOptions);	 
	        
	        // Call the base constructor    
	        this._super(_extendedData, _extendedMappingOptions);
	        
	        if(this.mustRegister()){
	        	var dialogRegister = $('#dialogRegister');
				var formRegister = $('form', dialogRegister);
				
				dialogRegister.modal({keyboard: false, backdrop: 'static'});
				
				formRegister.submit(function(event){
					event.preventDefault();
					
					formRegister.ajaxSubmit({
						type: 'post',
						success: function(data){
							if(data.success){
								dialogRegister.modal('hide');
							}	
						}
					});
					
					return false;
				});
	        }
    	}
    });	
});
