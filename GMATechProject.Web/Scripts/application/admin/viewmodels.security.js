jQuery(function ($) {
    // Lazy initialize our namespace context
    if (typeof (window.app) == 'undefined') window.app = {};

	var moduleUrl = "/admin/security";
	
    /******************************************************************************
    The ViewModel Responsible for security management
    ******************************************************************************/
    window.app.SecurityModel = function (application, options) {
        // Reference to self: usefull in callback functions
        var _self = this;
        // Reference to current document
        var _doc = $(document);
        // Reference to this modele root UI element 
        var _root = $("#security");

		_self.availableRoles = ko.observableArray([{
			text: "Utilisateur",
			value: "User"
		}, {
			text: "Rédacteur",
			value: "Editor"
		},{
			text: "Administrateur",
			value: "Administrator"
		}]);

        // Pagination data for the current page of users (index, size, total)
        _self.identitiesPage = ko.observable(
        	ko.mapping.fromJS(
        		app.EmptyPage, {
        		items: {
		            // The create option specify the KO mapping plugin to create a new Identity object from the provided data
		            create: function (options) {
		            	// option contains 2 properties: data (the current element in the items array) and parent (in our case, the SecurityModel)
		                return new app.Identity(options.data);
		            },		
		            // The key option specify the KO mapping plugin to use the identity id to differentiate the identities
		            key: function (identity) {
		            	// We don't call id as a function as it is not an observable at this point
		                return identity.id;
		            }
	            }
        	})
    	);

        // Boolean value indicating wether we are adding a new identity  
        _self.isAdding = ko.observable(false);

        // Property containing the currently edited identity (new or not)
        _self.editedIdentity = ko.observable(false);

        // Boolean value indicating wether we are editing an identity (edition & creation)
        _self.isEditing = ko.computed(function () {
            // We are editing if an editedIdentity has been set
            return _self.editedIdentity() !== false;
        });
        
        // Create a KO computed
        ko.computed(function(){
        	// As we reference navigationCurrent, this funtion will be re-evaluated when it changes
        	var currentSection = ko.utils.unwrapObservable(application.navigation.navigationCurrent);
        	var identity = ko.utils.unwrapObservable(application.identity);

        	// If we are in the security section
        	if(currentSection == app.Sections.Security
        	&& identity)
        	{
        		// Load data
    			loadData();
        	}
        });
        
        // Handler for click event on the "security-add" button
        _doc.on("click", "#identity-add", function (event) {
            event.preventDefault();

            var identity = new app.Identity();
            identity.beginEdit();

            // Update our view model:
            // Set the edited identity as new identity
            _self.editedIdentity(identity);
            // Flag the fact we are adding a new identity
            _self.isAdding(true);

            // Initialize edition UI
            initializeEdition();
        });

        _doc.on("click", "#security .pagination a", function (event) {
        	event.preventDefault();
        	var link = $(this);
        	
        	if(!link.parent().is('.disabled')){
	        	var pageIndex = link.data("navigation");
	        	
				loadData(pageIndex);
			}
			
			return false;
		});

        // Handler for click event on the buttons with "identity-delete" CSS class
        _doc.on("click", ".identity-delete", function (event) {
            event.preventDefault();
            // Get the identity bound to this link
            var identity = ko.dataFor(this);

            // Ask confirmation for deletion
            $.uiConfirm({
                message: 'Voulez-vous vraiment supprimer l\'utilisateur: ' + identity.email(),
                confirmed: function () {
                	$.ajax({
                		type: 'Delete', 
                		url: moduleUrl + '/' + identity.id(),
                		success: function(response){
                			if(response.success){
			                    reloadData();
                			}
                		}
                	});
                }
            });
        });
        
        // Handler for click event on the buttons with "identity-edit" CSS class
        _doc.on("click", ".identity-edit", function (event) {
            event.preventDefault();
            // Get the identity bound to this link
            var identity = ko.dataFor(this);

            // Start edition on this identity
            identity.beginEdit();

            // Update our view model
            // We are editing this existing identity
            _self.editedIdentity(identity);
            
            // We are not adding a new identity
            _self.isAdding(false);

            // Initialize edition UI
            initializeEdition();
        });
        
        // Handler for click event on the buttons with "identity-edit" CSS class
        _doc.on("click", ".identity-cancel", function (event) {
            event.preventDefault();
            var message = '<h4>Voulez-vous vraiment annuler vos modifications ?</h4>';

            if (_self.isAdding()) {
                message = '<h4>Voulez-vous vraiment annuler ce nouvel utilisateur ?</h4>';
            }

            // Request confirmation 
            $.uiConfirm({
                message: message,
                ok_text: 'Annuler',
                cancel_text: 'Ne pas annuler',
                width: 'auto',
                // Handler called when user confirm cancellation
                confirmed: function () {
                    // If we are editing an existing article
                    if (!_self.isAdding()) {
                        // Revert its changes if any
                        _self.editedIdentity().rollback();
                    }

                    // Reset the edition form and update our view model
                    resetEdition();
                }
            });
        });
        
		$('#identity-form-quick-search', _root).validate({
			rules: {
				query: { required: true, notPlaceholder: true, maxlength: 256 }
			}, 
			// Function called when the form is submited AND valid
            submitHandler: function (form) {
                $(form).ajaxSubmit({
            		type: 'POST',
                	success: function(response){
	        			if(response.success)
	        			{
	        				ko.mapping.fromJS(response.data, _self.identitiesPage());
	        				
	        				// Display the button to reset the form and reload data
		        			$('.buttonSearchCancel', _root).show();
	        			}
                 	}
                });
                
                return false;
            }
        });
        
        // Control the buttonSearchCancel Button Event
        _doc.on("click", ".buttonSearchCancel", function (event) {
        	event.preventDefault();
        	
        	// Reset the quick search form        	
        	$('.identity-form-quick-search', _root).get(0).reset();
        	
        	// Reload Data
        	_self.reloadData();
        	
        	// Hide this button
        	$('.buttonSearchCancel', _root).hide();

			return false;
		});
                 
        // Apply validation on the edition form
        $("#identity-form-edition", _root)
        	.validate({
                errorClass: "ui-state-error-text",
                rules: {
                    email: { required: true, notPlaceholder: true, maxlength: 256, email: true }
                },
                // Function called when the form is submited AND valid
                submitHandler: function (form) {
                    $(form).ajaxSubmit({
                    	url: moduleUrl, 
                		type: _self.isAdding() ? 'POST' : 'PUT',
                    	success: function(response){
                    		if(response.success){
			                    // If we are editing an existing article
			                    if (!_self.isAdding()) {
			                        // Commit its changes if any
			                        _self.editedIdentity().commit();
			                    }
			                    // If we are adding a new article
			                    else {
			                        // Push the saved article into our articles collection
			                        reloadData();
			                    }
			                    
			                    // Reset the edition form and update our view model
			                    resetEdition();			                    
                    		}
                    	}
                    });
                }
        	});  
            	                
        function initializeEdition() {
            //$("#identity-form-edition", _root).jWizard()
        }

        function resetEdition() {
            // Reset the form
            $("#identity-form-edition", _root)
                .resetForm();

            // Update our view model:
            // No edited identity
            _self.editedIdentity(false);
            // Not adding a new identity anymore
            _self.isAdding(false);
        };
        
        function loadData(pageIndex, pageSize)
        {
        	$.ajax({
        		url: moduleUrl, 
        		data: {
        			pageIndex: pageIndex, 
        			pageSize: pageSize
        		}, 
        		success: function(response)
        		{
        			if(response.success)
        			{
        				ko.mapping.fromJS(response.data, _self.identitiesPage());
        			}
        		}
        	});
        };
        
        _self.reloadData = function reloadData()
        {
        	loadData(_self.identitiesPage().pageIndex());
        }
    };
});