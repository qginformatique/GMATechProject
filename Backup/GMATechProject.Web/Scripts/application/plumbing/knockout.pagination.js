(function ($) {
    /* Binding used to show a pagination header from a JSON representation of a ListedData */
    ko.bindingHandlers["paginationHeader"] = {
        /* Function called to apply this binding*/
        update: function (element, valueAccessor) {
            // Get the binding options
            var options = ko.utils.unwrapObservable(valueAccessor());

            var defaultOptions = {
                text: 'Éléments %(first)s à %(last)s sur %(total)s'
            };
            var opts = {};

            // If we have options
            if (options) {
                var page;
                // If the options have a 'page' property
                if (options.page) {
                    // Get the paged data
                    page = ko.utils.unwrapObservable(options.page);

                    // Extend the options with default options
                    opts = $.extend(defaultOptions, options);
                } else {
                    // Assume the options are the paged data
                    page = options;

                    // Set our inner options as the default options
                    opts = defaultOptions;
                }

                // If the options have a 'loctext' property
                if (opts.loctext) {
                    // Get the localized value and set the text property
                    opts.text = Globalize.localize(opts.loctext);
                }

                // IF we have paged data
                if (page) {
					var pageIndex = (ko.utils.unwrapObservable(page.pageIndex) - 1);
					pageIndex = pageIndex > 0 ? pageIndex : 0;
					var total = ko.utils.unwrapObservable(page.total);
					var pageSize = ko.utils.unwrapObservable(page.pageSize);
					
            		var totalPages = Math.ceil(total / pageSize);
	            	var firstItem = (pageIndex * pageSize) + 1;
                	var lastItem = pageIndex < totalPages - 1 ? ((pageIndex * pageSize) + pageSize) : (totalPages * pageSize) - ((totalPages * pageSize) - total);
                	
                    // Format the text
                    var text = total > 0 ? $.sprintf(opts.text, {
                        first: firstItem,
                        last: lastItem,
                        total: total
                    }) : 'Aucun élément';
                    // Set the text
                    $(element).text(text);
                }
            }
        }
    };

    /* Binding used to show a pagination links from a JSON representation of a ListedData */
    ko.bindingHandlers["paginationLinks"] = {
        /* Function called to apply this binding*/
        update: function (element, valueAccessor) {

            // Get the binding options
            var options = valueAccessor();

            // Extend them with default values
            options = $.extend(options, {
                // Number of links to generate before and after the current page link
                adjacents: 5,
                // The text for the previous page link
                previousText: '<',
                // The text for the next page link
                nextText: '>',
                // The text for the ellipsis link
                ellipsText: '...',
                // The CSS class for the current page link
                currentPageClass: 'active',
                // The CSS class for the current page link
                disabledPageClass: 'disabled',
                // Link wrapper
                linkWrapper: 'li'
            });

            // Internal helper function to generate a pagination link
            function buildLink(clickHandler, text, index, cssClass) {
                var result = $(document.createElement('a'))
                    .text(text)
                    .attr('href', '#')
                    .data("navigation", index);
                
                if(clickHandler){
                    result.click(function (event) {
                        // Cancel the link default behaviour
                        event.preventDefault();
                        // If we have a valid handler, call it
                        if (typeof clickHandler === 'function') clickHandler(index);
                        return false;
                    });
                }

                if (options.linkWrapper !== null) {
                    result = $(document.createElement("li")).append(result);
                }

                result.addClass(cssClass);

                return result;
            };

            // Internal helper function to generate multiple pagination link
            function buildLinks(container, clickHandler, indexStart, indexEnd, currentIndex) {
                for (var index = indexStart; index <= indexEnd; index++) {
                    container.append(buildLink(clickHandler, index, index, index == currentIndex ? options.currentPageClass : null));
                }
            };

            // Internal helper function to generate an ellipsis link
            function buildEllipLink() {
                return buildLink(null, options.ellipsText, 0, options.disabledPageClass);
            };

            // Get the page from the options
            var page = ko.utils.unwrapObservable(options.page);
			
            // If the page is valid
            if (page) {
				var pageIndex = ko.utils.unwrapObservable(page.pageIndex);
				var total = ko.utils.unwrapObservable(page.total);
				var pageSize = ko.utils.unwrapObservable(page.pageSize);
            	var totalPages = (pageSize != 0 && total != 0) ? Math.ceil(total / pageSize) : 1;
            	
                var container = $(element);
                // Clear the container for links
                container.html('');

                // Generate 'Previous' link
                container.append(buildLink(options.clickHandler, options.previousText, pageIndex - 1, (pageIndex == 1) ? "disabled" : ""));

                // If we have less than 20 pages, simply generate all links
                if (totalPages <= 20) {
                	if(total > 0)
                	{
	                    // Generate numbered links
	                   	buildLinks(container, options.clickHandler, 1, totalPages, pageIndex);
	                }
                }
                // If we have more than 20 pages, generate links digg style
                else {
                    // If we are in the middle
                    if ((totalPages - (options.adjacents * 2) > pageIndex)
                        && (pageIndex > (options.adjacents * 2))) {

                        // Generate numbered links for the first 2 pages
                        buildLinks(container, options.clickHandler, 1, 2);

                        // Add an ellips link ('...')
                        container.append(buildEllipLink());

                        // Generate numbered links to get the number of links specified by options.adjacents before AND after the current page
                        buildLinks(container, options.clickHandler, (pageIndex - options.adjacents), (pageIndex + options.adjacents), pageIndex);

                        // Add an ellips link ('...')
                        container.append(buildEllipLink());

                        // Generate numbered links for the last 2 pages
                        buildLinks(container, options.clickHandler, totalPages - 1, totalPages);
                    }
                    // If we are at the beginning
                    else if (pageIndex < (totalPages / 2)) {
                        // Generate numbered links to get 2 times the number of links specified by options.adjacents after the first page
                        buildLinks(container, options.clickHandler, 1, (2 + (options.adjacents * 2)), pageIndex);

                        // Add an ellips link ('...')
                        container.append(buildEllipLink());

                        // Generate numbered links for the last 2 pages
                        buildLinks(container, options.clickHandler, totalPages - 1, totalPages);
                    }
                    // If we are at the end
                    else {
                        // Generate numbered links for the first 2 pages
                        buildLinks(container, options.clickHandler, 1, 2);

                        // Add an ellips link ('...')
                        container.append(buildEllipLink());

                        // Generate numbered links to get 2 times the number of links specified by options.adjacents before the last page
                        buildLinks(container, options.clickHandler, (totalPages - (2 + (options.adjacents * 2))), totalPages, pageIndex);
                    }
                }

                // Generate 'Next' link
                container.append(buildLink(options.clickHandler, options.nextText, pageIndex + 1, (pageIndex == totalPages) ? "disabled" : ""));
            }
        }
    };
})(jQuery);