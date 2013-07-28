(function ($) {

    /** Binding for stylized select - jQuery UI Select Menu Widget
    *  https://github.com/fnagel/jquery-ui/wiki/Selectmenu
    */
    ko.bindingHandlers.jqCarousel = {		
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        	var $element = $(element);
        	
            var options = ko.utils.extend({
				visible: 1,
				step: 1,
				speed: 700,
				width: 665,
				height: 250
			}, ko.utils.unwrapObservable(valueAccessor));
            
			$element.rcarousel(options);
        }
    };

    /** Binding for links on which an opacity fiter is applied when hovered
    */
    ko.bindingHandlers.jqOpacityLink = {
		init: function (element, valueAccessor) {
			var $element = $(element);
			
			var options = ko.utils.extend({
				hovered: "0.7",
				normal: "1.0"
			}, ko.utils.unwrapObservable(valueAccessor));
			
			$element.hover(
				function() {
					$( this ).css( "opacity", options.hovered );
				},
				function() {
					$( this ).css( "opacity", options.normal );
				}
			);
		}
	};
	/**************************************************************************
    JQuery UI bindings
    **************************************************************************/

    /** Binding for stylized tags - jQuery UI Tags Widget
    */
    ko.bindingHandlers.jqTags = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var $element = $(element);
            var options = ko.utils.extend({
            }, valueAccessor());
            
            if(options.tags){
            	options.onTagAdded = function(event, tag){
            		event.preventDefault();
            		
    				options.tags.push($(tag).find('.tagit-label').text());
    				
    				return false;
            	};
            	
        		options.onTagRemoved = function(event, tag){
        			event.preventDefault();
        			
    				options.tags.remove($(tag).find('.tagit-label').text());
        				           	
    				return false;
            	};
			}
			
            $element
            	.val(ko.utils.unwrapObservable(options.tags))
            	.tagit(options).next('.tagit')
            	.addClass($element.attr('class'));
        }
    };


    /** Binding for stylized buttons - jQuery UI Button Widget
    *  http://jqueryui.com/demos/button/
    */
    ko.bindingHandlers.jqButtonInverted = {
        init: function (element, valueAccessor) {
            var options = valueAccessor();
            $(element).buttoninverted(options);
        }
    };

    /** Binding for stylized buttons - jQuery UI Button Widget
    *  http://jqueryui.com/demos/button/
    */
    ko.bindingHandlers.jqButton = {
        init: function (element, valueAccessor) {
            var options = valueAccessor();
            $(element).button(options);
        }
    };

    /** Binding for sliders - jQuery UI Slider Widget
    *  http://jqueryui.com/demos/slider/
    */
    ko.bindingHandlers.jqSlider = {
        init: function (element, valueAccessor) {
            var options = valueAccessor();
            $(element).slider(options);
        }
    };

    /** Binding for stylized select - jQuery UI Select Menu Widget
    *  https://github.com/fnagel/jquery-ui/wiki/Selectmenu
    */
    ko.bindingHandlers.jqSelect = {
        init: function (element, valueAccessor) {
            var options = valueAccessor();
            $(element).selectmenu(options);
        },
        update: function (element, valueAccessor) {
            var options = valueAccessor();
            $(element).selectmenu(options);
        }
    };

    /** Binding for stylized buttons - jQuery UI Wizard Widget
    *  http://dominicbarnes.us/jWizard/
    */
    ko.bindingHandlers.jqWizard = {
        init: function (element, valueAccessor) {
            var options = valueAccessor();
            var $element = $(element);

            $element.jWizard(
                ko.utils.extend({
                    menuEnable: false,
                    buttons: {
                        jqueryui: {
                            enable: true
                        },
                        cancelText: "Annuler",
                        previousText: "Précédent",
                        nextText: "Suivant",
                        finishText: "Terminer"
                    },
                    counter: {
                        enable: true,
                        progressbar: true,
                        location: 'header',
                        appendText: '',
                        separatorText: '/'
                    },
                    effects: { enable: true }
                }, options));
        }
    };

    /** Binding for stylized tooltips - jQuery UI ToolTip Widget
    *  http://jqueryui.com/demos/tooltip/
    */
    ko.bindingHandlers.jqToolTipHelp = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var settings = ko.bindingHandlers.jqToolTip.getOptions(element, valueAccessor, allBindingsAccessor, viewModel);
            settings.tooltipClass += "ui-widget-content ui-state-highlight ui-tooltip-help";
            $(element).tooltip(settings);
        }
    };

    /** Binding for stylized tooltips - jQuery UI ToolTip Widget
    *  http://jqueryui.com/demos/tooltip/
    */
    ko.bindingHandlers.jqToolTipNextElem = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var settings = ko.bindingHandlers.jqToolTip.getOptions(element, valueAccessor, allBindingsAccessor, viewModel);

            settings.content = function () {
                return $(element).next().html();
            };

            $(element).tooltip(settings);
        }
    };

    /** Binding for stylized tabs - jQuery UI Tabs Widget
    *  http://jqueryui.com/demos/tabs/
    */
    ko.bindingHandlers.jqTabs = {
        createTabs: function (element, options) {
            $(element).tabs(ko.utils.unwrapObservable(options));
        },
        init: function (element, valueAccessor) {
            var options = ko.utils.unwrapObservable(valueAccessor());
            var $element = $(element);
            ko.bindingHandlers.jqTabs.createTabs($element, options);
        },
        update: function (element, valueAccessor) {
            var options = ko.utils.unwrapObservable(valueAccessor());
            var $element = $(element);
            $element.tabs('destroy');
            ko.bindingHandlers.jqTabs.createTabs($element, options);
        }
    };

    /** Binding for stylized tabs - jQuery UI Tabs Widget
    *  http://jqueryui.com/demos/tabs/
    */
    ko.bindingHandlers.jqTabsHistory = {
        createTabs: function (element, options) {
            var observableSelected = false;

            // If we have a selected option 
            if (options.selected
            // and it is an observable (has a subscribe method)
                && options.selected.subscribe) {
                observableSelected = options.selected;

                options.selected = parseInt(ko.utils.unwrapObservable(observableSelected));

                observableSelected.subscribe(function (value) {
                    if (!element.data("selected-from-observable")) {
                        element.data("selected-from-observable", true);
                        element.tabs('select', parseInt(value));
                        element.data("selected-from-observable", false);
                    }
                });
            }

            var callbackSelect;

            // If we have a select callback
            if (options.select) {
                // store it
                callbackSelect = options.select;
            }

            options.select = function (event, ui) {
                if (!element.data("selected-from-tabs")) {
                    element.data("selected-from-tabs", true);

                    // If we have a callback
                    if (typeof callbackSelect === 'function') {
                        // call it
                        callbackSelect.call(this, event, ui);
                    }

                    // If we have an observable for selected 
                    if (observableSelected) {
                        // Update its value
                        observableSelected(ui.index);
                    }
                    element.data("selected-from-tabs", false);
                }
            };

            element.tabs(options);
        },
        init: function (element, valueAccessor) {
            var options = ko.utils.unwrapObservable(valueAccessor());
            var $element = $(element);
            ko.bindingHandlers.jqTabsHistory.createTabs($element, options);
        }
    };

    /** Binding for strict MVVM use, associating an observable with the jQuery object of the element */
    ko.bindingHandlers.jqElement = {
        init: function (element, valueAccessor) {
            var options = valueAccessor();
            options($(element));
        }
    };

    /** Binding for accordion widget - jQuery UI Accordion Widget
    *  http://jqueryui.com/demos/accordion/
    */
    ko.bindingHandlers.jqAccordion = {
        init: function (element, valueAccessor) {
            var options = valueAccessor();
            var jqElement = $(element);
            jqElement.accordion(options);
            jqElement.bind("valueChanged", function () {
                ko.bindingHandlers.jqAccordion.update(element, valueAccessor);
            });
        },
        update: function (element, valueAccessor) {
            var options = valueAccessor();
            $(element).accordion('destroy').accordion(options);
        }
    };

    /** Binding for adding stylized and rich multiselect - jQuery UI MultiSelect Widget
    *  http://www.erichynds.com/jquery/jquery-ui-multiselect-widget/
    */
    ko.bindingHandlers.jqMultiSelect = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var defaults = {
                click: function (event, ui) {
                    var selected_options = $.map($(element).multiselect("getChecked"), function (a) { return $(a).val() });
                    allBindingsAccessor()['selectedOptions'](selected_options);
                }
            };
            var options = $.extend(defaults, valueAccessor());
            allBindingsAccessor()['options'].subscribe(function (value) {
                ko.bindingHandlers.jqMultiSelect.regenerateMultiselect(element, options, viewModel);
            });
            allBindingsAccessor()['selectedOptions'].subscribe(function (value) {
                ko.bindingHandlers.jqMultiSelect.regenerateMultiselect(element, options, viewModel);
            });
        },
        regenerateMultiselect: function (element, options, viewModel) {
            if ($(element).next().hasClass("ui-multiselect")) {
                setTimeout(function () {
                    return $(element).multiselect("refresh").multiselectfilter({
                        label: options['filterLabel'] || "Search: "
                    }); ;
                }, 0);
            } else {
                setTimeout(function () {
                    if (options['filter'] === true) {
                        $(element).multiselect(options).multiselectfilter({
                            label: options['filterLabel'] || "Search: "
                        });
                    } else {
                        $(element).multiselect(options);
                    }
                    if (options['noChecks'] === true) {
                        $(element).next().next().find(".ui-helper-reset:first").remove();
                    }
                }, 0);
            }
        }
    };

    /** Binding for window dialogs - jQuery UI Dialog Widget
    *  http://jqueryui.com/demos/dialog/
    */
    ko.bindingHandlers.jqDialog = {
        init: function (element, valueAccessor) {
            var defaults = {
                modal: true,
                autoOpen: false,
                closeOnEscape: true,
                resizable: false,
                hide: 'fade',
                show: 'fade',
                width: 500
            };

            var options = $.extend(defaults, valueAccessor());
            $(element).dialog(options);
        }
    };

    /** Binding for elements on which clicks open dialogs - jQuery UI Dialog Widget
    *  http://jqueryui.com/demos/dialog/
    */
    ko.bindingHandlers.jqOpenDialog = {
        init: function (element, valueAccessor) {
            var selectorDialogElement = valueAccessor();
            var dialog = $(selectorDialogElement);

            $(element).on('click', function (event) {
                event.preventDefault();

                dialog.dialog('open');
                dialog.show();

                if (dialog.is(".ui-helper-hidden")) {
                    dialog.removeClass("ui-helper-hidden");
                }
            });
        }
    };

    /** Binding for autocomplete inputs - jQuery UI AutoComplete Widget
    *  http://jqueryui.com/demos/autocomplete/
    */
    ko.bindingHandlers.jqAutoComplete = {
        update: function (element, valueAccessor, allBindingsAccessor) {
            var $element = $(element);
            var allBindings = allBindingsAccessor();
            var defaults = {
                minLength: 3,
                wait: 500
            };

            var options = $.extend(defaults, valueAccessor());
            $element.autocomplete(options);

            if (typeof allBindings.jqAutoCompleteRender === 'function') {
                $element.data('autocomplete')._renderItem = allBindings.jqAutoCompleteRender;
            }
        }
    };

    ko.bindingHandlers.jqDatePicker = {
        init: function (element, valueAccessor) {
            var options = ko.utils.unwrapObservable(valueAccessor());

            $(element).datepicker(
                ko.utils.extend({
                    changeMonth: true,
                    changeYear: true,
                    hideIfNoPrevNext: true,
                    disabled: false
                }, options)
            );

            $(element).on('keypress', function (event) { event.preventDefault(); });
        },
        update: function (element, valueAccessor) {
            var options = ko.utils.unwrapObservable(valueAccessor());
            $(element).datepicker('option', options);
        }
    };

    //jqAuto -- main binding (should contain additional options to pass to autocomplete)
    //jqAutoSource -- the array to populate with choices (needs to be an observableArray)
    //jqAutoQuery -- function to return choices
    //jqAutoValue -- where to write the selected value
    //jqAutoSourceLabel -- the property that should be displayed in the possible choices
    //jqAutoSourceInputValue -- the property that should be displayed in the input box
    //jqAutoSourceValue -- the property to use for the value
    //jqAutoRender -- method for item rendering
    ko.bindingHandlers.jqAuto = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var options = valueAccessor() || {},
            allBindings = allBindingsAccessor(),
            unwrap = ko.utils.unwrapObservable,
            modelValue = allBindings.jqAutoValue,
            source = allBindings.jqAutoSource,
            query = allBindings.jqAutoQuery,
            valueProp = allBindings.jqAutoSourceValue,
            inputValueProp = allBindings.jqAutoSourceInputValue || valueProp,
            labelProp = allBindings.jqAutoSourceLabel || inputValueProp;

            //function that is shared by both select and change event handlers
            function writeValueToModel(valueToWrite) {
                if (ko.isWriteableObservable(modelValue)) {
                    modelValue(valueToWrite);
                } else {  //write to non-observable
                    if (allBindings['_ko_property_writers'] && allBindings['_ko_property_writers']['jqAutoValue'])
                        allBindings['_ko_property_writers']['jqAutoValue'](valueToWrite);
                }
            }

            //on a selection write the proper value to the model
            options.select = function (event, ui) {
                writeValueToModel(ui.item ? ui.item.actualValue : null);
            };

            //on a change, make sure that it is a valid value or clear out the model value
            options.change = function (event, ui) {
                var currentValue = $(element).val();
                var matchingItem = ko.utils.arrayFirst(unwrap(source), function (item) {
                    return unwrap(item[inputValueProp]) === currentValue;
                });

                if (!matchingItem) {
                    writeValueToModel(null);
                }
            };

            //hold the autocomplete current response
            var currentResponse = null;

            //handle the choices being updated in a DO, to decouple value updates from source (options) updates
            var mappedSource = ko.computed({
                read: function () {
                    var mapped = ko.utils.arrayMap(unwrap(source), function (item) {
                        var result = {};
                        result.label = labelProp ? unwrap(item[labelProp]) : unwrap(item).toString();  //show in pop-up choices
                        result.value = inputValueProp ? unwrap(item[inputValueProp]) : unwrap(item).toString();  //show in input box
                        result.actualValue = valueProp ? unwrap(item[valueProp]) : item;  //store in model
                        return result;
                    });
                    return mapped;
                },
                write: function (newValue) {
                    source(newValue);  //update the source observableArray, so our mapped value (above) is correct
                    if (currentResponse) {
                        currentResponse(mappedSource());
                    }
                },
                disposeWhenNodeIsRemoved: element
            });

            if (query) {
                options.source = function (request, response) {
                    currentResponse = response;
                    query.call(this, request.term, mappedSource);
                };
            } else {
                //whenever the items that make up the source are updated, make sure that autocomplete knows it
                mappedSource.subscribe(function (newValue) {
                    $(element).autocomplete("option", "source", newValue);
                });

                options.source = mappedSource();
            }

            var $element = $(element);

            //initialize autocomplete
            $(element).autocomplete(options);

            if (typeof allBindings.jqAutoRender === 'function') {
                $element.data('autocomplete')._renderItem = allBindings.jqAutoRender;
            }
        },
        update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            //update value based on a model change
            var allBindings = allBindingsAccessor(),
           unwrap = ko.utils.unwrapObservable,
           modelValue = unwrap(allBindings.jqAutoValue) || '',
           valueProp = allBindings.jqAutoSourceValue,
           inputValueProp = allBindings.jqAutoSourceInputValue || valueProp;

            //if we are writing a different property to the input than we are writing to the model, then locate the object
            if (valueProp && inputValueProp !== valueProp) {
                var source = unwrap(allBindings.jqAutoSource) || [];
                modelValue = ko.utils.arrayFirst(source, function (item) {
                    return unwrap(item[valueProp]) === modelValue;
                }) || {};
            }

            //update the element with the value that should be shown in the input
            $(element).val(modelValue && inputValueProp !== valueProp ? unwrap(modelValue[inputValueProp]) : modelValue.toString());
        }
    };
})(jQuery);