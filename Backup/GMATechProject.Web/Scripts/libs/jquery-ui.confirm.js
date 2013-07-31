/*
* jQuery UI Confirmation Dialog 1.0.0
*
* Copyright (c) 2008 Chris Leishman (chrisleishman.com)
* Dual licensed under the MIT (MIT-LICENSE.txt)
* and GPL (GPL-LICENSE.txt) licenses.
*/
(function ($) {
    $.uiConfirm = function (o) {
        o = $.verifyOptions(o, {
            confirmed: 'function',
            cancelled: 'function',
            complete: 'function',
            modal: 'boolean',
            overlay: 'object',
            bgiframe: 'boolean',
            ok_text: 'string',
            cancel_text: 'string'
        }, {
            message: 'Confirm?',
            confirmed: function () { },
            cancelled: function () { },
            complete: function () { },
            modal: true,
            overlay: { opacity: 0.5, background: 'black' },
            bgiframe: true,
            ok_text: 'OK',
            cancel_text: 'Cancel'
        });

        var div = $('<div/>');

        if (typeof o.message === 'string') {
            div.html(o.message);
        } else if (typeof o.message === 'function') {
            div.html(o.message.call());
        } else {
            div.append(o.message);
        }
        var scope = this;

        var buttons = {};
        buttons[o.ok_text] = function () {
            div.dialog('close').remove();
            o.confirmed.call(scope);
            o.complete.call(scope);
        };
        buttons[o.cancel_text] = function () {
            div.dialog('close').remove();
            o.cancelled.call(scope);
            o.complete.call(scope);
        };

        var dialogOpts = $.extend({}, o, {
            autoOpen: true,
            draggable: false,
            resizable: false,
            buttons: buttons
        });

        var dialog = div.dialog(dialogOpts);
        dialog.closest('.ui-dialog').find('.ui-dialog-titlebar').remove();

        return dialog;
    };
})(jQuery);