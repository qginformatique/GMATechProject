(function () {
    ko.subscribable.fn.money = function () {
        var target = this;

        var format = function (value) {
        	if(value){
	            toks = value.toFixed(2).replace('-', '').split('.');
	            var display = '$' + $.map(toks[0].split('').reverse(), function (elm, i) {
	                return [(i % 3 === 0 && i > 0 ? ',' : ''), elm];
	            }).reverse().join('') + '.' + toks[1];
	
	            return value < 0 ? '(' + display + ')' : display;
            }
            
            return '';
        };

        var writeTarget = function (value) {
            target(parseFloat(value.replace(/[^0-9.-]/g, '')));
        };

        var result = ko.computed({
            read: function () {
                return target();
            },
            write: writeTarget
        });

        result.formatted = ko.computed({
            read: function () {
                return format(target());
            },
            write: writeTarget
        });

        return result;
    };

    ko.subscribable.fn.date = function (format) {
        var target = this;

        var _format = format || "dd/MM/yyyy";
        var writeTarget = function (value) {
	        if(typeof value === 'string')
	            target(Date.parse(value));
	        else
	        	target(value);
        };

        var result = ko.computed({
            read: function () {
                return target();
            },
            write: function () {
                return target();
            }
        });

        result.formatted = ko.computed({
            read: function () {
        		var date = target();
            	if(date){
            		return date.toString(_format);
                }
                
                return '';
            },
            write: writeTarget
        });

        return result;
    };
    
    // Display attribute under condition
    ko.bindingHandlers.attrIf = {
	    update: function (element, valueAccessor, allBindingsAccessor) {
	        var h = ko.utils.unwrapObservable(valueAccessor());
	        var show = ko.utils.unwrapObservable(h._if);
	        if (show) {
	            ko.bindingHandlers.attr.update(element, valueAccessor, allBindingsAccessor);
	        } else {
	            for (var k in h) {
	                if (h.hasOwnProperty(k) && k.indexOf("_") !== 0) {
	                    $(element).removeAttr(k);
	                }
	            }
	        }
	    }
	};
	
})();
