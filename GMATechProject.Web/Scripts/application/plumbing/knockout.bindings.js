(function ($) {
    ko.bindingHandlers['hoverOn'] = {
        'update': function (element, valueAccessor, allBindingsAccessor) {
            var cssHoverOn = ko.utils.unwrapObservable(valueAccessor());
            var cssHoverOff = ko.utils.unwrapObservable(allBindingsAccessor().hoverOff);

            var $element = $(element);
            $element
                .bind('mouseenter', function () {
                    if (cssHoverOff)
                        $element.removeClass(cssHoverOff);
                    if (cssHoverOn)
                        $element.addClass(cssHoverOn);
                }).bind('mouseleave', function () {
                    if (cssHoverOn)
                        $element.removeClass(cssHoverOn);
                    if (cssHoverOff)
                        $element.addClass(cssHoverOff);
                });
        }
    };

    ko.bindingHandlers['invisible'] = {
        'update': function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            var isCurrentlyVisible = !(element.style.display == "none");
            if (!value && !isCurrentlyVisible)
                element.style.display = "";
            else if ((value) && isCurrentlyVisible)
                element.style.display = "none";
        }
    };

    ko.bindingHandlers['slideVisible'] = {
        update: function (element, valueAccessor, allBindingsAccessor) {
            // First get the latest data that we're bound to
            var value = valueAccessor(), allBindings = allBindingsAccessor();

            // Next, whether or not the supplied model property is observable, get its current value
            var valueUnwrapped = ko.utils.unwrapObservable(value);

            // Grab some more data from another binding property
            var duration = allBindings.slideDuration || 600; // 400ms is default duration unless otherwise specified

            // Now manipulate the DOM element
            if (valueUnwrapped == true)
                $(element).slideDown(duration); // Make the element visible
            else
                $(element).slideUp(duration); // Make the element invisible
        }
    };

    /* Binding to fade in/out elements */
    ko.bindingHandlers['fadeVisible'] = {
        'init': function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var show = ko.utils.unwrapObservable(valueAccessor());
            // Apply initial visibility immediatly without animation
            show ? $(element).show() : $(element).hide();
        },

        'update': function (element, valueAccessor) {
            var show = ko.utils.unwrapObservable(valueAccessor());
            // On update, fade in/out
            show ? $(element).fadeIn() : $(element).fadeOut();
        }
    };

    /** Binding for adding a mask to input fields - jQuery Masked Input Plugin
    *  http://digitalbush.com/projects/masked-input-plugin/
    */
    ko.bindingHandlers['mask'] = {
        init: function (element, valueAccessor) {
            var options = valueAccessor();
            var mask = "";
            if (options === "date")
                mask = "99/99/9999";
            else if (options === "time")
                mask = "99:99";
            else if (options === "cpf")
                mask = "999.999.999-99";
            else if (options === "phone")
                mask = "(99) 9999-9999";
            else if (options === "cep")
                mask = "99999-999";
            else
                mask = options;

            $(element).mask(mask);
        }
    };

    /** Binding for form validation - jQuery Validation Plugin
    *  http://bassistance.de/jquery-plugins/jquery-plugin-validation/
    */
    ko.bindingHandlers['jqValidate'] = {
        init: function (element, valueAccessor) {
            var options = valueAccessor();
            var defaults = {
                errorClass: "ui-state-error",
                ignore: '.ignore',
                errorPlacement: function (error, element) {
                    error.appendTo(element.parent());
                }
            };

            $(element).validate(ko.utils.extend(defaults, options));
        }
    };

    ko.bindingHandlers['optionsClass'] = {
        'update': function (element, valueAccessor, allBindingsAccessor) {
            var optionElements = $(element).find('option');

            var allBindings = allBindingsAccessor();
            var value = ko.utils.unwrapObservable(allBindings["options"]);

            for (var i = 0, j = optionElements.length; i < j; i++) {
                var option = $(optionElements[i]);

                // Apply a value to the option element
                var optionClass = typeof valueAccessor() == "string" ? value[i][valueAccessor()] : value[i];
                optionClass = ko.utils.unwrapObservable(optionClass);
                option.addClass(optionClass);
            }
        }
    };

    // For certain common events (currently just 'dblclick', 'mouseenter', 'mouseleave'), allow a simplified data-binding syntax
    // e.g. dblclick:handler instead of the usual full-length event:{dblclick:handler}
    var eventHandlersWithShortcuts = ['dblclick', 'mouseenter', 'mouseleave'];
    ko.utils.arrayForEach(eventHandlersWithShortcuts, function (eventName) {
        ko.bindingHandlers[eventName] = {
            'init': function(element, valueAccessor, allBindingsAccessor, viewModel) {
                var newValueAccessor = function() {
                    var result = { };
                    result[eventName] = valueAccessor();
                    return result;
                };
                return ko.bindingHandlers['event']['init'].call(this, element, newValueAccessor, allBindingsAccessor, viewModel);
            }
        };
    });
    
	// Override default 'value' binding to better handle arrays
    ko.bindingHandlers['value'] = {
	    'init': function (element, valueAccessor, allBindingsAccessor) {
	        // Always catch "change" event; possibly other events too if asked
	        var eventsToCatch = ["change"];
	        var requestedEventsToCatch = allBindingsAccessor()["valueUpdate"];
	        if (requestedEventsToCatch) {
	            if (typeof requestedEventsToCatch == "string") // Allow both individual event names, and arrays of event names
	                requestedEventsToCatch = [requestedEventsToCatch];
	            ko.utils.arrayPushAll(eventsToCatch, requestedEventsToCatch);
	            eventsToCatch = ko.utils.arrayGetDistinctValues(eventsToCatch);
	        }

	        var valueUpdateHandler = function() {
	            var modelValue = valueAccessor();
	            var elementValue = ko.selectExtensions.readValue(element);
				
	           	if(Object.prototype.toString.apply(modelValue) !== '[object Array]') {
	           		// Same as writeValueToProperty function from knockout.debug.js
		            if (!modelValue || !ko.isObservable(modelValue)) {
		                var propWriters = allBindingsAccessor()['_ko_property_writers'];
		                if (propWriters && propWriters['value'])
		                    propWriters['value'](elementValue);
		            } else if (ko.isWriteableObservable(modelValue) && (modelValue.peek() !== elementValue)) {
		                modelValue(elementValue);
		            }
	        	}
	        	else {
	        		var elementValueAsArray = elementValue.split(',');
					// Same as writeValueToProperty function from knockout.debug.js
		            if (!modelValue || !ko.isObservable(modelValue)) {
		                var propWriters = allBindingsAccessor()['_ko_property_writers'];
		                if (propWriters && propWriters['value'])
		                    propWriters['value'](elementValueAsArray);
		            } else if (ko.isWriteableObservable(modelValue) && (modelValue.peek() !== elementValueAsArray)) {
		                modelValue(elementValueAsArray);
		            }
	        	}
	        }	        	        	        

	        // Workaround for https://github.com/SteveSanderson/knockout/issues/122
	        // IE doesn't fire "change" events on textboxes if the user selects a value from its autocomplete list
	        var ieAutoCompleteHackNeeded = ko.utils.ieVersion && element.tagName.toLowerCase() == "input" && element.type == "text"
	                                       && element.autocomplete != "off" && (!element.form || element.form.autocomplete != "off");
	        if (ieAutoCompleteHackNeeded && ko.utils.arrayIndexOf(eventsToCatch, "propertychange") == -1) {
	            var propertyChangedFired = false;
	            ko.utils.registerEventHandler(element, "propertychange", function () { propertyChangedFired = true });
	            ko.utils.registerEventHandler(element, "blur", function() {
	                if (propertyChangedFired) {
	                    propertyChangedFired = false;
	                    valueUpdateHandler();
	                }
	            });
	        }

	        ko.utils.arrayForEach(eventsToCatch, function(eventName) {
	            // The syntax "after<eventname>" means "run the handler asynchronously after the event"
	            // This is useful, for example, to catch "keydown" events after the browser has updated the control
	            // (otherwise, ko.selectExtensions.readValue(this) will receive the control's value *before* the key event)
	            var handler = valueUpdateHandler;
	            // if (ko.utils.stringStartsWith(eventName, "after")) {
	            // ko.utils.stringStartsWith function is not included in knockout release file (only in debug)
	            if (eventName.indexOf("after") === 0) {	           
	                handler = function() { setTimeout(valueUpdateHandler, 0) };
	                eventName = eventName.substring("after".length);
	            }
	            ko.utils.registerEventHandler(element, eventName, handler);
	        });
	    },
	    'update': function (element, valueAccessor) {
	        var valueIsSelectOption = (element && element.tagName && element.tagName.toLowerCase()) === "select";	            
	        var newValue = ko.utils.unwrapObservable(valueAccessor());
	        var elementValue = ko.selectExtensions.readValue(element);
	        var valueHasChanged = (newValue != elementValue);

	        // JavaScript's 0 == "" behavious is unfortunate here as it prevents writing 0 to an empty text box (loose equality suggests the values are the same).
	        // We don't want to do a strict equality comparison as that is more confusing for developers in certain cases, so we specifically special case 0 != "" here.
	        if ((newValue === 0) && (elementValue !== 0) && (elementValue !== "0"))
	            valueHasChanged = true;

	        if (valueHasChanged) {
	            var applyValueAction = function () { ko.selectExtensions.writeValue(element, newValue); };
	            applyValueAction();

	            // Workaround for IE6 bug: It won't reliably apply values to SELECT nodes during the same execution thread
	            // right after you've changed the set of OPTION nodes on it. So for that node type, we'll schedule a second thread
	            // to apply the value as well.
	            var alsoApplyAsynchronously = valueIsSelectOption;
	            if (alsoApplyAsynchronously)
	                setTimeout(applyValueAction, 0);
	        }

	        // If you try to set a model value that can't be represented in an already-populated dropdown, reject that change,
	        // because you're not allowed to have a model value that disagrees with a visible UI selection.
	        //if (valueIsSelectOption && (element.length > 0))
	        //	ensureDropdownSelectionIsConsistentWithModelValue(element, newValue, /* preferModelValue */ false);
	    }    
	};
	
	/* Manage Date Formatting */
	ko.bindingHandlers['dateString'] = {
	    'update': function(element, valueAccessor, allBindingsAccessor, viewModel) {
	        var value = valueAccessor(),
	            allBindings = allBindingsAccessor();
	        var valueUnwrapped = ko.utils.unwrapObservable(value);
	        var pattern = allBindings.datePattern || 'MM/dd/yyyy';
	        $(element).text(valueUnwrapped.toString(pattern));
	    }
	};
	
	/* Money Formatting */
	ko.bindingHandlers['money'] = {
	    'update': function(element, valueAccessor, allBindingsAccessor, viewModel) {
	        var value = valueAccessor(),
	            allBindings = allBindingsAccessor();
	        var valueUnwrapped = ko.utils.unwrapObservable(value);
	        $(element).text(valueUnwrapped.toFixed(2).replace('.', ',') + " €");
	    }
	};
	
	ko.bindingHandlers['addClass'] = {
        'init': function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            var $element = $(element);
            
            if(!$element.hasClass(value)){
				$element.addClass(value);
            }
        }
    };
    
    ko.bindingHandlers['refreshAddThis'] = {
	    'init': function(element, valueAccessor) {
	        window.addthis.ost = 0;
			window.addthis.ready();
	    }
	};
	
	ko.virtualElements.allowedBindings['refreshAddThis'] = true;
	
	//wrapper to an observable that requires accept/cancel
	ko.sectionObservable = function(initialValue) {
	    //private variables
	    var _actualValue = ko.observable(initialValue);

	    //computed observable that we will return
	    var result = ko.computed({
	        //always return the actual value
	        read: function() {
	           return _actualValue();
	        },
	        //stored in a temporary spot until commit
	        write: function(newValue) {
	        	if(!application.articles.isEditing() 
	        	&& !application.members.isEditing())
	        	{
	            	_actualValue(newValue);
	            }
	            else {
	            	$('html, body').animate({ scrollTop: 0 }, 'slow');
	            	application.warning('Vous ne pouvez pas changer de section si un élément est en cours de modification. Terminez les modifications ou annulez pour pouvoir changer de section.')
	            }
	        }
	    });

	    return result;
	};
	
})(jQuery);