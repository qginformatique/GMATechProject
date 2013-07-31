jQuery(function ($) {
    // Lazy initialize our namespace context
    if (typeof (window.app) == 'undefined') window.app = {};

    /******************************************************************************
    A single binding
    ******************************************************************************/
    window.app.RoleTagsBinding = Model.extend({
		init: function (data, mappingOptions, roles) {
	        var _self = this;

	        // Default values for an identity
	        var _defaults = {
	            id: '',
	            role: '',
	            tags: []
	        };

	        // Extend the supplied data with default values
	        var _extendedData = ko.utils.extend(_defaults, data);

	        // Define how to map the data to make the identity
	        var _mappingOptions = {
	        };
	        
	        // Extend the supplied mappingOptions with default values
	        var _extendedMappingOptions = ko.utils.extend(_mappingOptions, mappingOptions);	 
	        
	        // Call the base constructor    
	        this._super(_extendedData, _extendedMappingOptions);		
	
			this.makeEditable();
	
			_self.humanRole = ko.computed(function(){
				var text;
								
				$.each(roles, function(key, item){
					if(item.value == _self.role()){
						text = item.text;
						return false;
					}
					
					return true;
				});
				
				return text;
			});
		}
    });
});