(function ($) {
/** Binding for stylized tooltips - jQuery BootStrap ToolTip Widget
*/
ko.bindingHandlers.jqToolTip = {
    update: function (element, valueAccessor) {
        var options = valueAccessor();
        $(element).tooltip(options);
    }
};
    
/** Binding for modal dialog - Twitter BootStrap Modal Widget
*/
ko.bindingHandlers.btModal = {
    update: function (element, valueAccessor) {
        var options = valueAccessor();
        $(element).modal(options);
    }
};
})(jQuery);