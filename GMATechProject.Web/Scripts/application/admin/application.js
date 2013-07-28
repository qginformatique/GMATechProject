jQuery(function ($) {
    // Lazy initialize our namespace context
    if (typeof (window.app) == 'undefined') window.app = {};
    
    /******************************************************************************
    Class ApplicationModel
    ******************************************************************************/
    window.app.ApplicationModel = function (options) {
        var _self = this;
		var _container = $('#container');
		var _doc = $(document);
		var _dialogLogin = $('#dialogLogin');
		var _formLogin = $("form", _dialogLogin);

		_self.identity = ko.observable(false);
				
		_self.authenticate = function(response){
			if(response.success)
			{
				_self.identity(new app.Identity(response.data));
				
				_dialogLogin.modal('hide');
			}
			else if(response.errors) {        						
				for(var i = 0; i < response.errors.length; i++){
					// Show the login error and auto-close after 5 seconds
					showLoginAlert(response.errors[i], 5000);
				}
            }
		};
		
		if(options.user){
			$.ajax({
				url: '/admin/authentication/identity',
				success: _self.authenticate
			});
		}
		else {
        	_dialogLogin.modal({keyboard: false, backdrop: 'static'});
        	
        	_formLogin.validate({
				rules: {
					email: { required: true, email: true, notPlaceholder: true, maxlength: 256 },
					password: { required: true }
				}, 
				// Function called when the form is submited AND valid
	            submitHandler: function (form) {
	                $(form).ajaxSubmit({
	            		type: 'POST',
	                	success: _self.authenticate
	                });
	                
	                return false;
	            }
	        });
		};
		
		// Control the buttonLogOut Button Event
        _doc.on("click", ".buttonLogOut", function (event) {
        	event.preventDefault();
        	
        	// Log Out
        	_self.identity(false);
        	
        	// Show login modal
        	_dialogLogin.modal({keyboard: false, backdrop: 'static'});
        	
        	_formLogin.validate({
				rules: {
					email: { required: true, email: true, notPlaceholder: true, maxlength: 256 },
					password: { required: true }
				}, 
				// Function called when the form is submited AND valid
	            submitHandler: function (form) {
	                $(form).ajaxSubmit({
	            		type: 'POST',
	                	success: _self.authenticate
	                });
	                
	                return false;
	            }
	        });
		});
		
        _self.navigation = new app.NavigationModel(this, options.navigation);
        
        _self.progress = new app.ProgressModel(this, options.progress);

        _self.articles = new app.ArticlesModel(this, options.articles);
        
        _self.pages = new app.PagesModel(this, options.pages);
                
        _self.members = new app.MembersModel(this, options.members);

        _self.security = new app.SecurityModel(this, options.security);

		_self.roleTags = new app.RoleTagsModel(this, options.roleTags);
				
		// Initialise Notifications array
		_self.notifications = ko.observableArray([]);
		
        ko.applyBindings(_self);
                
        /*********************************************************************/
        // Alerts methods
        /*********************************************************************/
        _self.info = function(message) {
        	//showAlert(message, 'alert-info');
        	// Show the notification
        	var notification = $.notification({
				title: "Information",
				content: message,
				icon: '&#59141;',
				color: '#0095d3',
				timeout: 7000,
				type: 'information',
				showTime: true
			});
			// Add this notification to knockout notifications array
			_self.notifications.push(notification);
        };

        _self.success = function(message) {
        	//showAlert(message, 'alert-success');
        	// Show the notification
        	var notification = $.notification({
				title: "Succès",
				content: message,
				icon: '&#10003;',
				timeout: 7000,
				type: 'success',
				showTime: true
			});
			// Add this notification to knockout notifications array
			_self.notifications.push(notification);
        };
        
        _self.warning = function(message) {
        	//showAlert(message, '');
        	// Show the notification
        	var notification = $.notification({
				title: "Avertissement",
				content: message,
				icon: '&#59140;',
				color: '#ff8400',
				timeout: 7000,
				type: 'warning',
				showTime: true
			});
			// Add this notification to knockout notifications array
			_self.notifications.push(notification);
        };
        
        _self.error = function(message) {
        	//showAlert(message, 'alert-error');
        	// Show the notification
        	var notification = $.notification({
				title: "Erreur",
				content: message,
				timeout: 7000,
				icon: '&#9888;',
				type: 'error',
				showTime: true
			});
			// Add this notification to knockout notifications array
			_self.notifications.push(notification);
        };
        
        function showAlert(message, cssClass) {
        	var alert = $('<div class="alert ' + cssClass + ' fade in"><a class="close" data-dismiss="alert">×</a>' + message + '</div>');
        	
        	_container.prepend(alert.alert());
        };
        
        // Function to Show Login Alert in the modal (with delay in ms to auto close the alert)
        function showLoginAlert(message, delay) {
        	
        	var alert = $('<div class="alert fade in"><a class="close" data-dismiss="alert">×</a>' + message + '</div>');
        	
        	$("#dialogLogin .modal-body .login-alerts").append(alert.alert());
        	
			window.setTimeout(function() { alert.alert('close') }, delay);
		};
        
        /*********************************************************************/
        // Initialize jquery plugins
        /*********************************************************************/
        
        // When an ajax request succeed
        _container.ajaxSuccess(function (event, xhr, ajaxOptions) {
            // Parse the response as JSON    
            var response = $.parseJSON(xhr.responseText);
			
            // If we have a json result  && dialogLogin Modal is not in the foreground
            if (response && !$("#dialogLogin").hasClass("in"))
            {
				// if it contains some info messages
				if(response.infos) {
					// Show them
					for(var i = 0; i < response.infos.length; i++){
						_self.success(response.infos[i]);
					}
                }
                
				// if it contains some warning messages
				if(response.warnings) {
					// Show them
					for(var i = 0; i < response.warnings.length; i++){
						_self.warning(response.warnings[i]);
					}
                }

				// if it contains some error messages
				if(response.errors) {
					// Show them
					for(var i = 0; i < response.errors.length; i++){
						_self.error(response.errors[i]);
					}
                }
            }
        });

        // When an ajax request fails
        _container.ajaxError(function (event, request, ajaxOptions) {
        	switch(request.status)
        	{
        		case 404:
            		_self.error("L'élément demandé n'existe pas.");
            		break;
            		
				case 400:
            		_self.error("Vous n'êtes pas autorisé à effectuer cette action.");
            		break;
            		
        		default:
            		_self.error("Une erreur est survenue, veuillez ré-essayer plus tard.");
        			break;
    		}
        });
        
        // Set the default culture for jQuery UI datepicker to french
        $.datepicker.setDefaults($.datepicker.regional['fr']);
        
        // Set some default values for the jQuery validator
        $.validator.setDefaults({
            errorClass: "ui-state-error",
            ignore: '.ignore'
        });
        
        /* Default options for our wizards */
		$.db.jWizard.prototype.options.menuEnable = false;
		$.db.jWizard.prototype.options.buttons.jqueryui.enable = true;
		$.db.jWizard.prototype.options.buttons.cancelText = "Annuler";
		$.db.jWizard.prototype.options.buttons.previousText = "Précédent";
		$.db.jWizard.prototype.options.buttons.nextText = "Suivant";
		$.db.jWizard.prototype.options.buttons.finishText = "Terminer";
        $.db.jWizard.prototype.options.counter.enable = true;
        $.db.jWizard.prototype.options.counter.progressbar = true;
        $.db.jWizard.prototype.options.counter.location = 'header';
        $.db.jWizard.prototype.options.counter.appendText = '';
		$.db.jWizard.prototype.options.counter.separatorText = '/';
		$.db.jWizard.prototype.options.effects.enable = false;
         
        // Custom validation method which check wether the input's value is not equal to its placeholder attribute value
		$.validator.addMethod('notPlaceholder', function (val, el) {
			var placeholder = $(el).attr('placeholder');
			return (placeholder === undefined) || (placeholder !== undefined && val !== placeholder);
		}, $.validator.messages.required);
		
        // Custom validation method which check dates in french format
		jQuery.validator.addMethod("customDate", function (value, element) {
			return this.optional(element) || Date.parseExact(value, "dd/MM/yyyy");
		}, $.validator.messages.date);
    };
    
    /******************************************************************************
    Class EmptyPage: used to initialize paginated data
    ******************************************************************************/
    window.app.EmptyPage = {
		pageIndex: 0, 
		pageSize: 0, 
		total: 0, 
		items: []
	};
});