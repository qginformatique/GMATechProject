jQuery(function ($) {
    // Lazy initialize our namespace context
    if (typeof (window.app) == 'undefined') window.app = {};

	var moduleUrl = "/admin/members";
	
    /******************************************************************************
    The ViewModel Responsible for members management
    ******************************************************************************/
    window.app.MembersModel = app.BaseEditionModel.extend({
    	init: function(application, options){
	        // Reference to self: usefull in callback functions
	        var _self = this;
	        // Reference to current document
	        var _doc = $(document);
	        // Reference to this modele root UI element 
	        var _root = $("#members");
	        
	        // Event Categories Initialization
	        _self.genres = ko.observableArray([{
				text: "M.",
				value: "Mister"
			}, {
				text: "Mme.",
				value: "Mistress"
			}]);

			var extendedOptions = $.extend({
				rootElement: '#members',
				section: app.Sections.Members,
				moduleUrl: "/admin/members",
				itemConstructor: app.Member,
				messageDelete: function(item){
					return 'Voulez-vous vraiment supprimer l\'adhérent: ' + item.name();
				},
				validationRules: {
	                    name: { required: true, notPlaceholder: true, maxlength: 256 },
	                    website: { url: true },
	                    email: { email: true},
	                    contactName: { required: true, maxlength: 256 },
	                    contactTitle: { maxlength: 256 },
	                    phone: { notPlaceholder: true, maxlength: 30 },
	                    contactEmail: { email: true },
	                    contactphone: { notPlaceholder: true, maxlength: 30 },
	                    contactMobile: { notPlaceholder: true, maxlength: 30 },
	                    address1: { required: true, notPlaceholder: true, maxlength: 256  },
	                    address2: { maxlength: 256 },
	                    city: { required: true, notPlaceholder: true, maxlength: 256  },
	                    zipCode: { required: true, notPlaceholder: true, maxlength: 5  },
	                    subscribingDate: { required: true, customDate: true }
	                }
			}, options);

	        // Call the base constructor    
	        this._super(application, extendedOptions);
		}
    });
});