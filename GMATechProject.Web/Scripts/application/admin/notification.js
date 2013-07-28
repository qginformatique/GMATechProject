(function ($) {

    $.notification = function (settings) {
        var _self = this;
        
        var container, notification, hide, image, right, left, inner;
        
        settings = $.extend({
        	title: undefined,
        	content: undefined,
        	timeout: 0,
        	img: undefined,
        	border: true,
        	fill: false,
        	showTime: false,
        	click: undefined,
        	icon: undefined,
        	color: undefined,
        	// Amélioration de la gestion des différentes notifications
        	type: undefined
        }, settings);
        
        container = $("#notifications");
        if (!container.length) {
            container = $("<div>", { id: "notifications" }).appendTo( $("body") )
        };
        
		notification = $("<div>");
        notification.addClass("notification animated fadeInLeftMiddle fast");
     
     	// Utilisé pour récupérer les informations dans la barre d'informations
        _self.getType = settings.type;
        _self.getMessage = settings.content;
        
        // Amélioration de la gestion des différentes notifications
        if(settings.type != undefined) {
        	notification.addClass(settings.type);
        }
        
        if( $("#notifications .notification").length > 0 ) {
        	notification.addClass("more");
        } else {
        	container.addClass("animated flipInX").delay(1000).queue(function(){ 
        	    container.removeClass("animated flipInX");
        			container.clearQueue();
        	});
        }
        
        hide = $("<div>", {
			click: function () {
				if($(this).parent().is(':last-child')) {
				    $(this).parent().remove();
				    $('#notifications .notification:last-child').removeClass("more");
				} else {
					$(this).parent().remove();
				}
			}
		});
		
		hide.addClass("hide");

		left = $("<div class='left'>");
		right = $("<div class='right'>");
		
		if(settings.title != undefined) {
			var htmlTitle = "<h2>" + settings.title + "</h2>";
			notification.addClass("big");
		} else {
			var htmlTitle = "";
		}
		
		if(settings.content != undefined) {
			var htmlContent = settings.content;
		} else {
			var htmlContent = "";
		}
		
		inner = $("<div>", { html: htmlTitle + htmlContent });
		inner.addClass("inner");
		
		inner.appendTo(right);
		
		if (settings.img != undefined) {
			image = $("<div>", {
				style: "background-image: url('"+settings.img+"')"
			});
		
			image.addClass("img");
			image.appendTo(left);
			
			if(settings.border==false) {
				image.addClass("border")
			}
			
			if(settings.fill==true) {
				image.addClass("fill");
			}
			
		} else {
			if (settings.icon != undefined) {
				var iconType = settings.icon;
			} else {
				var iconType = '&#8505;';
			}	
			icon = $('<div class="icon">').html(iconType);
				
			if (settings.color != undefined) {
				icon.css("color", settings.color);
			}
			
			icon.appendTo(left);
		}

        left.appendTo(notification);
        right.appendTo(notification);
        
        hide.appendTo(notification);
        
        function timeSince(time){
        	var time_formats = [
        	  [2, "Une seconde", "Il y a 1 seconde"], // 60*2
        	  [60, "secondes", 1], // 60
        	  [120, "Une minute", "Il y a une minute"], // 60*2
        	  [3600, "minutes", 60], // 60*60, 60
        	  [7200, "Une heure", "Il y a 1 heure"], // 60*60*2
        	  [86400, "heures", 3600], // 60*60*24, 60*60
        	  [172800, "Un jour", "demain"], // 60*60*24*2
        	  [604800, "jours", 86400], // 60*60*24*7, 60*60*24
        	  [1209600, "Une semaine", "semaine prochaine"], // 60*60*24*7*4*2
        	  [2419200, "semaines", 604800], // 60*60*24*7*4, 60*60*24*7
        	  [4838400, "Un mois", "le mois prochain"], // 60*60*24*7*4*2
        	  [29030400, "mois", 2419200], // 60*60*24*7*4*12, 60*60*24*7*4
        	  [58060800, "Un an", "l'année prochaine"], // 60*60*24*7*4*12*2
        	  [2903040000, "années", 29030400], // 60*60*24*7*4*12*100, 60*60*24*7*4*12
        	  [5806080000, "Un sciècle", "le sciècle prochain"], // 60*60*24*7*4*12*100*2
        	  [58060800000, "sciècles", 2903040000] // 60*60*24*7*4*12*100*20, 60*60*24*7*4*12*100
        	];
        	
        	var seconds = (new Date - time) / 1000;
        	var token = "ago", list_choice = 1;
        	if (seconds < 0) {
        		seconds = Math.abs(seconds);
        		token = "from now";
        		list_choice = 1;
        	}
        	var i = 0, format;
        	
        	while (format = time_formats[i++]) if (seconds < format[0]) {
        		if (typeof format[2] == "string")
        			return format[list_choice];
        	    else
        			return Math.floor(seconds / format[2]) + " " + format[1];
        	}
        	return time;
        };
        
        if(settings.showTime != false) {
        	var timestamp = Number(new Date());
        	
        	timeHTML = $("<div>", { html: "<strong>" + timeSince(timestamp) + "</strong> ago" });
        	timeHTML.addClass("time").attr("title", timestamp);
        	timeHTML.appendTo(right);
        	
        	setInterval(
	        	function() {
	        		$(".time").each(function () {
	        			var timing = $(this).attr("title");
	        			$(this).html("<strong>" + timeSince(timing) + "</strong> ago");
	        		});
	        	}, 4000)
        	
        }

        notification.hover(
        	function () {
            	hide.show();
        	}, 
        	function () {
        		hide.hide();
        	}
        );
        
        notification.prependTo(container);
		notification.show();

        if (settings.timeout) {
            setTimeout(function () {
            	var prev = notification.prev();
            	if(prev.hasClass("more")) {
            		if(prev.is(":first-child") || notification.is(":last-child")) {
            			prev.removeClass("more");
            		}
            	}
            	notification.remove();
            }, settings.timeout)
        }
        
        if (settings.click != undefined) {
        	notification.addClass("click");
            notification.bind("click", function (event) {
            	var target = $(event.target);
                if(!target.is(".hide") ) {
                    settings.click.call(this)
                }
            })
        }
        
        return this
    }
})(jQuery);