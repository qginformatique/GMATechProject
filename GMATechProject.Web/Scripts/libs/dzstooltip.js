(function($) {

    $.fn.dzstooltip = function(o) {

        var defaults = {
            settings_slideshowTime : '5' //in seconds
            , settings_autoHeight : 'on'
            , settings_skin : 'skin_default'
        }

        o = $.extend(defaults, o);
        this.each( function(){
            var cthis = jQuery(this);
            var cchildren = cthis.children();
            var currNr=-1;
            
            
            return this;
        })
    }
})(jQuery)
