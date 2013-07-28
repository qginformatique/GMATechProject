jQuery(function ($) {
	// Lazy initialize our namespace context
	if (typeof (window.app) == 'undefined') window.app = {};
	
	var moduleUrl = "/articles";

	/******************************************************************************
	The ViewModel Responsible for articles management
	******************************************************************************/
	window.app.ArticlesModel = function (application, options) {
		// Reference to self: usefull in callback functions
		var _self = this;
		// Reference to current document
		var _doc = $(document);
		// Reference to this modele root UI element 
		var _root = $("#articles");
		
        // Pagination data for the current page of articles (index, size, total)
        _self.articlesPage = ko.observable(
        	ko.mapping.fromJS({
        		pageIndex: 1,
        		pageSize: 0,
        		total: 0,
        		items: []
        	}, {
        		items: {
		            // The create option specify the KO mapping plugin to create a new Article object from the provided data
		            create: function (options) {
		            	// option contains 2 properties: data (the current element in the items array) and parent (in our case, the ArticlesModel)
		                return new app.Article(options.data);
		            },
		            
		            // The update option specify the KO mapping plugin to create a new Article object from the provided data
		            update: function (options) {
		            	// option contains 2 properties: data (the current element in the items array) and parent (in our case, the ArticlesModel)
		                return new app.Article(options.data);
		            },
		
		            // The key option specify the KO mapping plugin to use the article id to differentiate the articles
		            key: function (article) {
		            	// We don't call id as a function as it is not an observable at this point
		                return article.id;
		            }
	            }
        	})
    	);
    	
    	// Pagination data for the current page of articles on homepage (index, size, total)
        _self.articlesPageHome = ko.observable(
        	ko.mapping.fromJS({
        		pageIndex: 1, 
        		pageSize: 0, 
        		total: 0, 
        		items: []
        	}, {
        		items: {
		            // The create option specify the KO mapping plugin to create a new Article object from the provided data
		            create: function (options) {
		            	// option contains 2 properties: data (the current element in the items array) and parent (in our case, the ArticlesModel)
		                return new app.Article(options.data);
		            },
		            
		            // The update option specify the KO mapping plugin to create a new Article object from the provided data
		            update: function (options) {
		            	// option contains 2 properties: data (the current element in the items array) and parent (in our case, the ArticlesModel)
		                return new app.Article(options.data);
		            },		      
		
		            // The key option specify the KO mapping plugin to use the article id to differentiate the articles
		            key: function (article) {
		            	// We don't call id as a function as it is not an observable at this point
		                return article.id;
		            }
	            }
        	})
    	);
    	
    	// Pagination data for the current page of articles on homepage (index, size, total)
        _self.articlesPageSlider = ko.observable(
        	ko.mapping.fromJS({
        		pageIndex: 1, 
        		pageSize: 0, 
        		total: 0, 
        		items: []
        	}, {
        		items: {
		            // The create option specify the KO mapping plugin to create a new Article object from the provided data
		            create: function (options) {
		            	// option contains 2 properties: data (the current element in the items array) and parent (in our case, the ArticlesModel)
		                return new app.Article(options.data);
		            },
		            
		            // The update option specify the KO mapping plugin to create a new Article object from the provided data
		            update: function (options) {
		            	// option contains 2 properties: data (the current element in the items array) and parent (in our case, the ArticlesModel)
		                return new app.Article(options.data);
		            },		      
		
		            // The key option specify the KO mapping plugin to use the article id to differentiate the articles
		            key: function (article) {
		            	// We don't call id as a function as it is not an observable at this point
		                return article.id;
		            }
	            }
        	})
    	);
    	
    	// Property to set the currently displayed article
    	_self.currentArticle = ko.observable();
    	
        // This property is used for url history: it returns the label for the currently displayed article and can set it from the address parameter too
        // (when using the back/forward button of the browser or entering an url with this parameter set directly)
        _self.currentArticleForHistory = ko.computed({
            // Function used to retrieve the title and id of the currently displayed article
            read: function () {
            	var article = _self.currentArticle();
            	var urlPart;
            	
            	// If there is a displayed article
            	if(article)
            	{
            		// If SEO Title is not null or empty: display SEO Title in url
            		if(article.seoTitle() && article.seoTitle().length > 0)
            		{
            			urlPart = article.seoTitle();
            		}
            		// Else display Title in url
            		else
            		{
            			urlPart = article.title();
            		}
            		
            		// Add image url informations for Facebook
            		$('head meta[property="og:image"]').remove();
            		$('head').append('<meta property="og:image" content="' + article.illustrationImageUrl() + '" />');
            	}
            	
                // return the title and id of the article at the index specified by the currentArticle property
                return urlPart;
            },
            // Function used to set the displayed article from the url parameter
            write: function (articleIdentification) {
            	var article;
            	
                // Get the article specified
                if(articleIdentification){
		        	$.ajax({
		        		url: moduleUrl + "/" + articleIdentification,
		        		cache: true, 
		        		success: function(response)
		        		{
		        			if(response.success)
		        			{
				                // Set the current article (can be null)
				                _self.currentArticle(new app.Article(response.data));
		        			}
		        		}
		        	});                	
                }
            }
        });

        // This function link the currentArticleForHistory property to the url parameter named "a" for articles
        ko.linkObservableToUrl(_self.currentArticleForHistory, "a");    	
        
        // Manage click on facebook share article
        _doc.on("click", ".article_facebook_post", function (event) {
	        // calling the API ...
	        var obj = {
	          method: 'feed',
	          redirect_uri: window.location.href,
	          link: window.location.href,
	          picture: 'http://fibresud.org' + _self.currentArticle().illustrationImageUrl(),
	          name: _self.currentArticle().title(),
	          caption: 'Fibresud.org',
	          description: _self.currentArticle().description()
	        };

	        function callback(response) {};

	        FB.ui(obj, callback);
        });
    	
    	// Handler for click event on the btn-prev-article button
        _doc.on("click", ".btn-prev-article", function (event) {
        	event.preventDefault();
        	
            // Get the current article
            var currentArticle = _self.currentArticle();
            
            // Get all the articles currently loaded
            var articles = _self.articlesPage().items();                
           	
   			// Get the index of the current article		
			var currentIndex = articles.indexOf(currentArticle);
			
			// We want the index of the article before the current one
			var prevIndex = currentIndex - 1;
			
			// If the index is less than 0
			if(prevIndex < 0){
				// Get the last article (cycle)
				prevIndex = articles.length - 1;
			}
            
            // Set the current article
            _self.currentArticle(articles[prevIndex]);
        });
        
        // Handler for click event on the btn-next-article button
        _doc.on("click", ".btn-next-article", function (event) {
        	event.preventDefault();
        	
            // Get the current article
            var currentArticle = _self.currentArticle();
            
            // Get all the articles currently loaded
            var articles = _self.articlesPage().items();                
           	
   			// Get the index of the current article		
			var currentIndex = articles.indexOf(currentArticle);
			
			// We want the index of the article after the current one
			var nextIndex = currentIndex + 1;
			
			// If the index is greater than the total number of articles (-1 as we are zero indexed)
			if(nextIndex > (articles.length - 1)){
				// Get the first article (cycle)
				nextIndex = 0;
			}
            
            // Set the current article
            _self.currentArticle(articles[nextIndex]);
        });
        
        // Handler for click event on the [homePage]slider-article OR the [homePage]home-news element
        _doc.on("click", "li.slider-article, div.home-news, #articles-page .articles-news", function (event) {
        	event.preventDefault();
        	
        	// Redirect to the article page
        	window.location = $(this).attr("data-href");
        });
        
        // Handler for click event on the article button
        _doc.on("click", ".btn-article", function (event) {
        	event.preventDefault();
        
            // Get the article bound to this link
            var article = ko.dataFor(this);

            _self.currentArticle(article);
        });
        
        // Handler for click event on the articles button
        _doc.on("click", ".btn-articles", function (event) {
        	event.preventDefault();

            _self.currentArticle(null);
        });
        
        ko.postbox.subscribe("section-changed", function(newValue) {
        	// If in the articles section
			if(newValue == "actualites"){
				// Load data
    			_self.loadData();
			} 
        	// If in the home section
			else if(newValue == "home"){
				// Load data Home articles
    			loadDataHome(1, 4);
    			
    			// Load date Slider articles
    			loadDataSlider(1, 4);
    			
				// Clear the current article
            	_self.currentArticle(null);    			
			}
        	// If in the any other section
			else {
				// Clear the current article
            	_self.currentArticle(null);
			}
		}); 
        
        // Handler for click event on the "ui-carousel-prev" button
        _doc.on("click", "#ui-carousel-prev", function (event) {
        	event.preventDefault();
        	
        	var currentPageIndex = _self.articlesPageHome().pageIndex();
        	var pageIndex = currentPageIndex - 1;
        	
        	// Load data
        	loadDataHome(pageIndex, 3);
        });
        
        // Handler for click event on the "ui-carousel-next" button
        _doc.on("click", "#ui-carousel-next", function (event) {
        	event.preventDefault();
        	
        	var currentPageIndex = _self.articlesPageHome().pageIndex();
        	var pageIndex = currentPageIndex + 1;
        	
        	// Load data
        	loadDataHome(pageIndex, 3);
        });
    	
    	// Ajax Request
    	_self.loadData = function loadData(pageIndex, pageSize)
        {
        	$.ajax({
        		url: moduleUrl, 
        		cache: true,
        		data: {
        			pageIndex: pageIndex, 
        			pageSize: pageSize
        		}, 
        		success: function(response)
        		{
        			if(response.success)
        			{
        				ko.mapping.fromJS(response.data, _self.articlesPage());
        			}
        		}
        	});
        };
        
        function showArticle(id){        	
            // Get the article specified
            if(id){
	        	$.ajax({
	        		url: moduleUrl + "/byId",
	        		cache: true, 
	        		success: function(response)
	        		{
	        			if(response.success)
	        			{
			                // Set the current article (can be null)
			                _self.currentArticle(new app.Article(response.data));
	        			}
	        		}
	        	});                	
            }
        }
        
        // Ajax Request
        function loadDataHome(pageIndex, pageSize)
        {
        	$.ajax({
        		url: moduleUrl + "/homeArticles",
        		cache: true,
        		data: {
        			pageIndex: pageIndex, 
        			pageSize: pageSize
        		}, 
        		success: function(response)
        		{
        			if(response.success)
        			{
        				ko.mapping.fromJS(response.data, _self.articlesPageHome());
        			}
        		}
        	});
        };
        
        // Ajax Request
        function loadDataSlider(pageIndex, pageSize)
        {
        	$.ajax({
        		url: moduleUrl + "/sliderArticles",
        		cache: true,
        		data: {
        			pageIndex: pageIndex, 
        			pageSize: pageSize
        		}, 
        		success: function(response)
        		{
        			if(response.success)
        			{
        				ko.mapping.fromJS(response.data, _self.articlesPageSlider());
        			}
        		}
        	});
        };
	};

	/******************************************************************************
	A single article
	******************************************************************************/
	window.app.Article = Model.extend({
		init: function (data, mappingOptions) {
	        var _self = this;
			
	        // Default values for an article
	        var _defaults = {
	            id: '',
	            title: '',
	            imageUrl: '',
	            content: '',
	            description: '',
	            imageUrl: '',
	            state: 'Draft',
	            friendlyUrl: '',
	            seoTitle: '',
	            seoDescription: '',
	            seoKeywords: '',
	            tags: [],
	            homeSlider: false,
	            creationDate: new Date().toString("dd/MM/yyyy"),
	            modificationDate: new Date().toString("dd/MM/yyyy"),
	            publicationDate: new Date().toString("dd/MM/yyyy")
	        };
			
	        // Extend the supplied data with default values
	        var _extendedData = ko.utils.extend(_defaults, data);
			
	        // Define how to map the data to make the article
	        var _mappingOptions = {
	            // Instruct KO.mapping to ignore the tags property, we'll map it ourselves
	            ignore: ["tags"],
	            creationDate: {
	                // Function called when mapping the creationDate
	                create: function (options) {
	                    // map it as an observable with the date extension
	                    //return ko.observable(dateFormat(options.data));
	                    return ko.observable(moment(options.data));
	                }
	            },
	            modificationDate: {
	                // Function called when mapping the modificationDate
	                create: function (options) {
	                    // map it as an observable with the date extension
	                    //return ko.observable(dateFormat(options.data));
	                    return ko.observable(moment(options.data));
	                }
	            },
	            publicationDate: {
	                // Function called when mapping the publicationDate
	                create: function (options) {
	                    // map it as an observable with the date extension
	                    //return ko.observable(dateFormat(options.data));
	                    return ko.observable(moment(options.data));
	                }
	            }
	        };
	        
	        // Extend the supplied mappingOptions with default values
	        var _extendedMappingOptions = ko.utils.extend(_mappingOptions, mappingOptions);	        
	        
	        // Call the base constructor    
	        this._super(_extendedData, _extendedMappingOptions);
			
	        // Map the tag property as an observable array with the tags etension
	        //_self.tags = ko.observableArray(_extendedData.tags).tags();
		    
		    _self.illustrationImageUrl = ko.computed(function() {
		    	return "/Upload/images/actualites/" + ((_self.imageUrl() == null || _self.imageUrl() == '') ? "actualites_1.jpg" : _self.imageUrl());
		    });
    	}
    });
});
