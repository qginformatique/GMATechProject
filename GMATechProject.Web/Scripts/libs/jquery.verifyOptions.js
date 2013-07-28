/*
* jQuery Option Argument Verifier 1.0.0
*
* Copyright (c) 2008 Chris Leishman (chrisleishman.com)
* Dual licensed under the MIT (MIT-LICENSE.txt)
* and GPL (GPL-LICENSE.txt) licenses.
*/
(function($) {
    $.verifyOptions = function(options, required, defaults) {
        options = $.extend(defaults || {}, options || {});
        $.each(required || {}, function(name, type) {
            if (options[name] == null) throw "Missing required option '" + name + "'";
            if (typeof options[name] != type) throw "Type of option '" + name + "' is not " + type;
        });
        return options;
    };
})(jQuery);

