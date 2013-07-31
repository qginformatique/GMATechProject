jQuery(function ($) {
    // Lazy initialize our namespace context
    if (typeof (window.app) == 'undefined') window.app = {};

	var moduleUrl = "/admin/roletags";
	
    /******************************************************************************
    The ViewModel Responsible for role/tags bindings management
    ******************************************************************************/
    window.app.RoleTagsModel = function (application, options) {
        // Reference to self: usefull in callback functions
        var _self = this;
        // Reference to current document
        var _doc = $(document);
        // Reference to this modele root UI element 
        var _root = $("#roletags");

		_self.roles = options.roles;
		_self.availableRoles = ko.observableArray(options.roles);

        // the bindings 
        _self.bindings = ko.observableArray(
        	ko.mapping.fromJS(
        		[], {
	            // The create option specify the KO mapping plugin to create a new RoleTagsBindings object from the provided data
	            create: function (options) {
	            	// option contains 2 properties: data (the current element in the items array) and parent (in our case, the SecurityModel)
	                return new app.RoleTagsBinding(options.data, null, _self.roles);
	            },
	            // The key option specify the KO mapping plugin to use the binding's role to differentiate the bindings
	            key: function (identity) {
	            	// We don't call id as a function as it is not an observable at this point
	                return identity.id;
	            }
            })
    	);
        
        // Create a KO computed
        ko.computed(function(){
        	// As we reference navigationCurrent, this funtion will be re-evaluated when it changes
        	var currentSection = ko.utils.unwrapObservable(application.navigation.navigationCurrent);
        	var identity = ko.utils.unwrapObservable(application.identity);
        	
        	// If we are in the role tags section
        	if(currentSection == app.Sections.RoleTagsBindings
        	&& identity)
        	{
        		// Load data
    			loadData();
        	}
        });
                                
        _doc.on('tagitontagadded', '.role-tags', function(event, tag) {
        	var binding = ko.dataFor(this);
        	
        	if(binding)
        	{
        		$.ajax({
	        		url: moduleUrl, 
            		type: 'PUT',
	        		async: false,
	        		data: {
	        			role: binding.role(), 
	        			tag: $(tag).find('.tagit-label').text()
	        		}, 
	        		success: function(response)
	        		{
	        			if(!response.success)
	        			{
	        				event.preventDefault();
	        				return false;
	        			}
	        			else
	        			{
	        				return true;
	        			}
	        		}
	        	});
        	}
        });
                         
        _doc.on('tagitontagremoved', '.role-tags', function(event, tag) {
        	var binding = ko.dataFor(this);
        	
        	if(binding)
        	{
        		$.ajax({
	        		url: moduleUrl, 
	        		type: 'DELETE',
	        		async: false,
	        		data: {
	        			role: binding.role(), 
	        			tag: $(tag).find('.tagit-label').text()
	        		}, 
	        		success: function(response)
	        		{
	        			if(!response.success)
	        			{
	        				event.preventDefault();
	        				return false;
	        			}
	        			else
	        			{
	        				return true;
	        			}
	        		}
	        	});
        	}
        });
                         
        function loadData()
        {
        	$.ajax({
        		url: moduleUrl, 
        		success: function(response)
        		{
        			if(response.success)
        			{
        				ko.mapping.fromJS(response.data, _self.bindings());
        			}
        		}
        	});
        };
        
        function reloadData()
        {
        	loadData();
        }
    };
});

