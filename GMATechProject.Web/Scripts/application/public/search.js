jQuery(function ($) {
	// Lazy initialize our namespace context
	if (typeof (window.app) == 'undefined') window.app = {};
	
	var moduleUrl = "/public/search";

	/******************************************************************************
	The ViewModel Responsible for search management
	******************************************************************************/
	window.app.SearchModel = function (application, options) {
		// Reference to self: usefull in callback functions
		var _self = this;
		// Reference to current document
		var _doc = $(document);
		// Reference to this modele root UI element 
		var _root = $("#search");
		
		_self.showSearch = ko.observable(false);
		
        // Pagination data for the current page of search resulst (index, size, total)
        _self.searchResultsPage = ko.observable(
        	ko.mapping.fromJS({
        		pageIndex: 1,
        		pageSize: 0,
        		total: 0,
        		items: []
        	}, {
        		items: {
		            // The create option specify the KO mapping plugin to create a new SearchResult object from the provided data
		            create: function (options) {
		            	// option contains 2 properties: data (the current element in the items array) and parent (in our case, the SearchModel)
		                return new app.SearchResult(options.data);
		            },
		            
		            // The update option specify the KO mapping plugin to create a new SearchResult object from the provided data
		            update: function (options) {
		            	// option contains 2 properties: data (the current element in the items array) and parent (in our case, the SearchModel)
		                return new app.SearchResult(options.data);
		            },
		
		            // The key option specify the KO mapping plugin to use the content id and type to differentiate the contents
		            key: function (content) {
		            	// We don't call id as a function as it is not an observable at this point
		                return content.id + "|" + content.type;
		            }
	            }
        	})
    	);
    	
    	_doc.on('click', '.search-view', function(){
    		// Get the search result clicked
    		var item = ko.dataFor(this);
    		
    		_self.showSearch(false);
    		
    		switch(item.type){
    			case 'Article':
    				application.articles.showArticle(item.id);
    				break;
					
				case 'Page':
					application.pages.showPage(item.id);
					break;
    		}
    	});
    	  
    	    	  	    	
    	_doc.on('submit', '#form_search', function(){
    		var query = $('input[name="query"]', '#form_search').val();
    		
    		_self.loadData(query);
    	});
        
    	// Ajax Request
    	_self.loadData = function loadData(query, pageIndex, pageSize)
        {
        	$.ajax({
        		url: moduleUrl, 
        		cache: true,
        		data: {
        			query: query,
        			pageIndex: pageIndex,
        			pageSize: pageSize
        		}, 
        		success: function(response)
        		{
        			_self.showSearch(true);
        			
        			if(response.success)
        			{
        				ko.mapping.fromJS(response.data, _self.searchResultsPage());
        			}
        		}
        	});
        };
	};

	/******************************************************************************
	A single search result
	******************************************************************************/
	window.app.SearchResult = Model.extend({
		init: function (data) {
	        var _self = this;
			
	        // Default values for an article
	        var _defaults = {
	            id: '',
	            label: '',
	            description: '',
	            type: ''
	        };
			
	        // Extend the supplied data with default values
	        var _extendedData = ko.utils.extend(_defaults, data);
	        
	        // Call the base constructor    
	        this._super(_extendedData);
    	}
    });
});

