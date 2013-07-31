jQuery(function ($) {
    // Lazy initialize our namespace context
    if (typeof (window.app) == 'undefined') window.app = {};
    
    window.app.BaseEditionModel = Class.extend({
    	init: function (application, options) {
	        // Reference to self: usefull in callback functions
	        var _self = this;
	        
	        // Reference to current document
	        var _doc = $(document);
	        
	        // Reference to the application
	        this.application = application;
	        
	        // Reference to this modele root UI element 
	        this.root = $(options.rootElement);
	        
	        // Reference to the options
	        this.options = options;
	        
	        // Pagination data for the current page of items (index, size, total)
	        this.itemsPage = ko.observable(
	        	ko.mapping.fromJS(
	        		app.EmptyPage, {
	        		items: {
			            // The create option specify the KO mapping plugin to create a new iem from the provided data
			            create: function (options) {
			            	// option contains 2 properties: data (the current element in the items array) and parent
			                return new _self.options.itemConstructor(options.data);
			            },
			            // The key option specify the KO mapping plugin to use the item id to differentiate the items
			            key: function (item) {
			            	// We don't call id as a function as it is not an observable at this point
			                return item.id;
			            }
		            }
	        	})
	    	);

	        // Boolean value indicating wether we are adding a new item 
	        this.isAdding = ko.observable(false);

	        // Property containing the currently edited item (new or not)
	        this.editedItem = ko.observable(false);

	        // Boolean value indicating wether we are editing an item (edition & creation)
	        this.isEditing = ko.computed(function () {
	            // We are editing if an editeditem has been set
	            return ko.utils.unwrapObservable(this.editedItem) !== false;
	        }, this);
	        
	        // Create a KO computed which load data for the current section if it has been selected
	        ko.computed(function(){
	        	// As we reference navigationCurrent, this funtion will be re-evaluated when it changes
	        	var currentSection = ko.utils.unwrapObservable(application.navigation.navigationCurrent);
	        	var identity = ko.utils.unwrapObservable(application.identity);
	        	
	        	// If we are in the articles section
	        	if(currentSection == options.section
	        	&& identity)
	        	{
	        		// Load data
	    			this.loadData();
	        	}
	        }, this);
	        
	        // Handler for click event on the "action-add" button
	        this.root.on("click", ".action-add", function (event) {
	            event.preventDefault();

				// Create a new item
	            var item = new options.itemConstructor();
	            
	            // Enabled edition on it
	            item.beginEdit();

	            // Set the edited item as new item
	            _self.editedItem(item);
	            
	            // Flag the fact we are adding a new item
	            _self.isAdding(true);

	            // Initialize edition UI
	            _self.initializeEdition();
	        });

	        // Handler for click event on the buttons with "action-edit" CSS class
	        this.root.on("click", ".action-edit", function (event) {
	            event.preventDefault();
	            
	            // Get the item bound to this link
	            var item = ko.dataFor(this);

	            // Start edition on this item
	            item.beginEdit();

	            // We are editing this existing item
	            _self.editedItem(item);
	            
	            // We are not adding a new item
	            _self.isAdding(false);

	            // Initialize edition UI
	            _self.initializeEdition();
	        });
	        
	        // Handler for click event on the buttons with "action-delete" CSS class
	        this.root.on("click", ".action-delete", function (event) {
	            event.preventDefault();
	            // Get the item bound to this link
	            var item = ko.dataFor(this);

	            // Ask confirmation for deletion
	            $.uiConfirm({
	                message: _self.options.messageDelete(item),
	                cancel_text: "Annuler",
	                confirmed: function () {
	                	$.ajax({
	                		type: 'Delete', 
	                		url: _self.options.moduleUrl + '/' + item.id(),
	                		success: function(response){
	                			if(response.success){
				                    _self.itemsPage().items.remove(item);
				                    _self.reloadData();
	                			}
	                		}
	                	});
	                }
	            });
	        });
			
	        // Handler for click event on the buttons with "action-enable" CSS class
	        this.root.on("click", ".action-enable", function (event) {
	            event.preventDefault();
	            
	            // Get the item bound to this link
	            var item = ko.dataFor(this);

	            if(typeof item.enabled === 'function'
	            	&& typeof item.enable === 'function')
	            {
	            	if(!item.enabled())
	            	{
						$.ajax({
	                		type: 'PUT',
	                		url: _self.options.moduleUrl + '/publish/' + item.id(),
	                		success: function(response){
	                			if(response.success){
				                    item.enable();
	                			}
	                		}
	                	});
	            	}
	            	else
	            	{
	            		$.ajax({
	                		type: 'PUT',
	                		url: _self.options.moduleUrl + '/unpublish/' + item.id(),
	                		success: function(response){
	                			if(response.success){
				                    item.disable();
	                			}
	                		}
	                	});
	            	}
	            }
	        });
	        	            	        
	        this.root.on("click", ".pagination a", function (event) {
	        	event.preventDefault();
	        	var link = $(this);
	        	
	        	if(!link.parent().is('.disabled')){
		        	var pageIndex = link.data("navigation");

					_self.loadData(pageIndex);
				}

				return false;
			});
	        
	        // Handler for the changestep event of the jWizard plugin applied on the edition form
	        this.root.on("jwizardchangestep", ".form-edition", function (event, ui) {
	        	var form = $('.form-edition', _self.root);
	        	
                // Get the current step
                var currentStep = $("fieldset:eq(" + ui.currentStepIndex + ")", ".form-edition");
                
                // Get the current step
                var nextStep = $("fieldset:eq(" + ui.nextStepIndex + ")", ".form-edition");
                
                // Get all inputs from the current step
                var inputs = currentStep.find(":input");
                
                // Get all inputs from the current step
                var richInputs = currentStep.find(".rich-content");
                
                // Get all inputs from the next step
                var richInputsInNextStep = nextStep.find(".rich-content");
	            
	            // Current item (being edited)
	            var item = _self.editedItem();
	        	
	            richInputs.each(function(){
	            	var $this = $(this);
	            	var editor = $this.ckeditorGet();
	                
	                // Ask ckeditor to update the textarea 
	                editor.updateElement();
	                
	                $this.change();
	            });

	            // "manual" is always triggered by the user, never jWizard itself
	            if (ui.type !== "manual") {                
	            	// If we are moving forward in the steps
	                if (ui.currentStepIndex < ui.nextStepIndex) {
	                    // If we have inputs and they are some validation errors
	                    if (inputs.length && !inputs.valid()) {
	                        // return false to prevent the wizard to go next step
	                        return false;
	                    }
	                }

	            }
	        });

	        // Handler for the cancel event of the jWizard plugin applied on the edition form
	        this.root.on("jwizardcancel", ".form-edition", function (event, state) {
	            var message = '<h4>Voulez-vous vraiment annuler vos modifications ?</h4>';

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
	                        _self.editedItem().rollback();
	                    }

	                    // Reset the edition form and update our view model
	                    _self.resetEdition();
	                }
	            });
	        });

	        // Handler for the finish event of the jWizard plugin applied on the edition form
	        this.root.on("jwizardfinish", ".form-edition", function (event, state) {
	        	// Item edited and submitted. Commit() = reinitialize rollback
	        	_self.editedItem().commit();
	        	
	        	if(_self.options.beforeSaving
	        		&& typeof _self.options.beforeSaving === 'function'){
	        		_self.options.beforeSaving(function(){
						// Submit the form
	            		$(".form-edition", _self.root).submit();	        		
	        		});	
	        	}
	        	else {
	            	// Submit the form
	            	$(".form-edition", _self.root).submit();
            	}
            	
            	// Reload Data
            	_self.reloadData();
	        });
	        
	        // Handler for the quick search form
			$('.form-quick-search', this.root)
				.validate({
					errorClass: "ui-state-error-text",
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
			        				ko.mapping.fromJS(response.data, _self.itemsPage());
			        				
			        				// Display the button to reset the form and reload data
			        				$('.buttonSearchCancel', _self.root).show();
			        			}
		                 	}
		                });
		                
		                return false;
		            }
	        });
	        
	        // Control the buttonSearchCancel Button Event
	        this.root.on("click", ".buttonSearchCancel", function (event) {
	        	event.preventDefault();
	        	
	        	// Reset the quick search form
	        	$('.form-quick-search', _self.root).get(0).reset();
	        	
	        	// Reload Data
	        	_self.reloadData();
	        	
	        	// Hide this button
	        	$('.buttonSearchCancel', _self.root).hide();

				return false;
			});
	    }, 
	    
        loadData: function(pageIndex, pageSize)
        {
        	var _self = this;
        	
        	$.ajax({
        		url: this.options.moduleUrl,
        		cache: false,
        		data: {
        			pageIndex: pageIndex, 
        			pageSize: pageSize
        		}, 
        		success: function(response)
        		{
        			if(response.success)
        			{        				
        				ko.mapping.fromJS(response.data, _self.itemsPage());
        			}
        		}
        	});
        },
        
        reloadData: function ()
        {
        	var currentPage = ko.utils.unwrapObservable(this.itemsPage);
        	var currentPageIndex= ko.utils.unwrapObservable(currentPage.pageIndex);
        	
        	this.loadData(currentPageIndex);
        }, 
        
        initializeEdition: function() { 
           var _self = this;
        	
            // Apply validation on the edition form
            var form = $(".form-edition", this.root);
            
            form
            	.jWizard()
            	.validate({
	                errorClass: "ui-state-error-text",
	                rules: this.options.validationRules,
	                // Function called when the form is submited AND valid
	                submitHandler: function (form) {
	                	
	                	var item = getItemFromObservable(_self.editedItem);
	                	
	                    $.ajax({
	                    	url: _self.options.moduleUrl, 
	                		type: _self.isAdding() ? 'POST' : 'PUT',
	                		contentType: 'application/json',
	                		data: item,
	                    	success: function(response){
	                    		if(response.success){
				                    // If we are editing an existing item
				                    if (!_self.isAdding()) {
				                    	ko.mapping.fromJS(response.data, _self.editedItem());
				                    }
				                    // If we are adding a new item
				                    else {
				                        _self.itemsPage().items.mappedCreate(response.data);
				                    }
				                    
				                    // Reset the edition form and update our view model
				                    _self.resetEdition();			                    
	                    		}
	                    	}
	                    });
	                }
            	});

            var richContentInputs = $(".rich-content", form);
			
			// Apply the ckeditor plugin to the content textarea
        	richContentInputs.each(function(){
        		var $this = $(this);
        			$this.ckeditor();
            });
            
            $(".popoverlink").click(function(e){e.preventDefault();}).popover({html: true, placement: 'bottom', trigger: 'hover'});
            
            // Initialise dropzone (only for article)
            if(	_self.options.rootElement == "#articles" ) {
				var myDropzone = new Dropzone("div.dropzone", { 
					url: "/admin/fileManager/uploadArticleImage",
					maxFilesize: 4, // MB
					success: function(file, response) {	
						_self.editedItem().imageUrl(moment().format("YYYY_MM-MMMM/").toLowerCase() + normalize_filename(file.name));
					},
					enqueueForUpload: true
				});
			}
			
			// Initialise dropzone (only for event)
            if(	_self.options.rootElement == "#agenda" ) {
				var myDropzone = new Dropzone("div.dropzone", { 
					url: "/admin/fileManager/uploadEventImage",
					maxFilesize: 4, // MB
					success: function(file, response) {					
						_self.editedItem().imageUrl(moment().format("YYYY_MM-MMMM/").toLowerCase() + normalize_filename(file.name));
					},
					enqueueForUpload: true
				});
			}
        }, 
        
        resetEdition: function() {                
            var form = $(".form-edition", this.root);
			
			var richContentInputs = $(".rich-content", form);
			
            richContentInputs.each(function(){
            	var $this = $(this);
            	var editor = $this.ckeditorGet();
                
                // Ask ckeditor to update the textarea 
                editor.destroy();
                
                CKEDITOR.remove(editor);
            });
            
            form
            // Reset the form
        		.resetForm()
            // destroy the wizard plugin
                .jWizard('destroy');
            
            // Revert changes
            this.editedItem().rollback();
            // No edited item
            this.editedItem(false);
            // Not adding a new item anymore
            this.isAdding(false);            
        }
    });
    
    function normalize_filename(filename) {
	  // Trim && transform into lowerCase
	  filename = filename.replace(/^\s+|\s+$/g, '');
	  filename = filename.toLowerCase();
	  
	  // Get extension
	  var fileExt = "";
	  fileExt = filename.match(/\.[0-9a-z]+$/i).toString();
	  var filenameWithoutExtension = filename.replace(fileExt, '');
	  
	  // remove accents, swap ñ for n, etc
	  var from = "àáäâèéëêìíïîòóöôùúüûñç·/_,:;'";
	  var to   = "aaaaeeeeiiiioooouuuunc-------";
	  for (var i=0, l=from.length ; i<l ; i++) {
	    filenameWithoutExtension = filenameWithoutExtension.replace(new RegExp(from.charAt(i), 'g'), to.charAt(i));
	  }

	  filenameWithoutExtension = filenameWithoutExtension.replace(/[^a-z0-9 -]/g, '') // remove invalid chars
	    .replace(/\s+/g, '-') // collapse whitespace and replace by -
	    .replace(/-+/g, '-'); // collapse dashes

	  return filenameWithoutExtension + fileExt;
	}
    
    function string_to_slug(str) {
	  // Trim && transform into lowerCase
	  str = str.replace(/^\s+|\s+$/g, '');
	  str = str.toLowerCase();
	  
	  // remove accents, swap ñ for n, etc
	  var from = "àáäâèéëêìíïîòóöôùúüûñç·/_,:;'";
	  var to   = "aaaaeeeeiiiioooouuuunc-------";
	  for (var i=0, l=from.length ; i<l ; i++) {
	    str = str.replace(new RegExp(from.charAt(i), 'g'), to.charAt(i));
	  }

	  str = str.replace(/[^a-z0-9 -]/g, '') // remove invalid chars
	    .replace(/\s+/g, '-') // collapse whitespace and replace by -
	    .replace(/-+/g, '-'); // collapse dashes

	  return str;
	}
    
    function getItemFromObservable(observable) {
    	var result = ko.mapping.toJS(observable);
    	
    	for(var propertyName in result){
    		if(result[propertyName] instanceof Date){
    			result[propertyName] = result[propertyName].toJSON();
    		}
    	}
    	
    	return ko.toJSON(result);
    }
});