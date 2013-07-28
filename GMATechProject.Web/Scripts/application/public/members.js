jQuery(function ($) {
	// Lazy initialize our namespace context
	if (typeof (window.app) == 'undefined') window.app = {};
	
	var moduleUrl = "/public/members";

	/******************************************************************************
	The ViewModel Responsible for articles management
	******************************************************************************/
	window.app.MembersModel = function (application, options) {
		// Reference to self: usefull in callback functions
		var _self = this;
		// Reference to current document
		var _doc = $(document);
		// Reference to this modele root UI element 
		var _root = $("#members");
		
        // Pagination data for the current page of members (index, size, total)
        _self.membersPage = ko.observable(
        	ko.mapping.fromJS({
        		pageIndex: 1,
        		pageSize: 0,
        		total: 0,
        		items: []
        	}, {
        		items: {
		            // The create option specify the KO mapping plugin to create a new Member object from the provided data
		            create: function (options) {
		            	// option contains 2 properties: data (the current element in the items array) and parent (in our case, the MembersModel)
		                return new app.Member(options.data);
		            },
		            
		            // The update option specify the KO mapping plugin to create a new Member object from the provided data
		            update: function (options) {
		            	// option contains 2 properties: data (the current element in the items array) and parent (in our case, the MembersModel)
		                return new app.Member(options.data);
		            },
		
		            // The key option specify the KO mapping plugin to use the article id to differentiate the members
		            key: function (member) {
		            	// We don't call id as a function as it is not an observable at this point
		                return member.id;
		            }
	            }
        	})
    	);
    	
    	// Property currentMember
    	_self.currentMember = ko.observable();
    	
    	// Handler for click event on the members-previous pagination link
        _doc.on("click", ".members-pagination .members-previous", function (event) {
        	event.preventDefault();
        	
        	if($(this).hasClass("disabled"))
        	{
        		return false;
        	}
        	
            // Get the current Page
            var currentPage = _self.membersPage();               
           	
   			// Get the index of the current page		
			var currentIndex = currentPage.pageIndex();
			
			// Get total number of pages
			var totalPages = ko.utils.range(1, Math.ceil(currentPage.total()/currentPage.pageSize()))
			
			// We want the index of the member before the current one
			var prevIndex = currentIndex - 1;
			
			// If the index is less than 0
			if(prevIndex < 0){
				// Get the last member (cycle)
				prevIndex = totalPages.length - 1;
			}
			
			// Set the current page
			_self.loadData(prevIndex, currentPage.pageSize());
        });
        
        // Handler for click event on the members-next pagination link
        _doc.on("click", ".members-pagination .members-next", function (event) {
        	event.preventDefault();
        	
            // Get the current Page
            var currentPage = _self.membersPage();               
           	
   			// Get the index of the current page		
			var currentIndex = currentPage.pageIndex();
			
			// Get total number of pages
			var totalPages = ko.utils.range(1, Math.ceil(currentPage.total()/currentPage.pageSize()))
			
			// We want the index of the member after the current one
			var nextIndex = currentIndex + 1;
			
			// If the index is greater than the total number of members (-1 as we are zero indexed)
			if(nextIndex > (totalPages.length)){
				// Get the first member (cycle)
				nextIndex = 0;
			}
			
			// Set the current page
			_self.loadData(nextIndex, currentPage.pageSize());
        });
    	
    	// Handler for click event on the btn-prev-member button
        _doc.on("click", ".btn-prev-member", function (event) {
        	event.preventDefault();
        	
            // Get the current member
            var currentMember = _self.currentMember();
            
            // Get all the members currently loaded
            var members = _self.membersPage().items();                
           	
   			// Get the index of the current member		
			var currentIndex = members.indexOf(currentMember);
			
			// We want the index of the member before the current one
			var prevIndex = currentIndex - 1;
			
			// If the index is less than 0
			if(prevIndex < 0){
				// Get the last member (cycle)
				prevIndex = members.length - 1;
			}
            
            // Set the current article
            _self.currentMember(members[prevIndex]);
        });
        
        // Handler for click event on the btn-next-member button
        _doc.on("click", ".btn-next-member", function (event) {
        	event.preventDefault();
        	
            // Get the current member
            var currentMember = _self.currentMember();
            
            // Get all the members currently loaded
            var members = _self.membersPage().items();                
           	
   			// Get the index of the current member		
			var currentIndex = articles.indexOf(currentMember);
			
			// We want the index of the article after the current one
			var nextIndex = currentIndex + 1;
			
			// If the index is greater than the total number of members (-1 as we are zero indexed)
			if(nextIndex > (members.length - 1)){
				// Get the first member (cycle)
				nextIndex = 0;
			}
            
            // Set the current member
            _self.currentMember(members[nextIndex]);
        });
    	
    	// Handler for click event on the member-view button
        _doc.on("click", ".member-view", function (event) {
        	event.preventDefault();
        
            // Get the member bound to this link
            var member = ko.dataFor(this);
            
            _self.currentMember(member);
        });
        
        // Handler for click event on the member button
        _doc.on("click", ".btn-member", function (event) {
        	event.preventDefault();
        
            // Get the member bound to this link
            var member = ko.dataFor(this);

            _self.currentMember(member);
        });
        
        // Handler for click event on the members button
        _doc.on("click", ".btn-members", function (event) {
        	event.preventDefault();

            _self.currentMember(null);
        });		
    	
    	// Ajax Request
    	_self.loadData = function loadData(pageIndex, pageSize)
        {
        	$.ajax({
        		url: moduleUrl, 
        		cache: false,
        		data: {
        			pageIndex: pageIndex, 
        			pageSize: pageSize
        		}, 
        		success: function(response)
        		{
        			if(response.success)
        			{
        				ko.mapping.fromJS(response.data, _self.membersPage());
        			}
        		}
        	});
        };
	};

	/******************************************************************************
	A single article
	******************************************************************************/
	window.app.Member = Model.extend({
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
    	}
    });
});
