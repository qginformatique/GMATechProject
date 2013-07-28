(function () {
	this.Model = Class.extend({
		init: function (data, mappingOptions) {
            // Keep a reference to this instance (can be useful in callbacks)
            var _self = this;

            // Ask Knockout to map the json data as an observable
            ko.mapping.fromJS(data, mappingOptions, this);	
		}, 
		
		beginEdit: function () {
			// Loop over this object properties
			for( var propertyName in this){
				var property = this[propertyName];
				
				// If the property is an observable
				if(ko.isObservable(property) && (typeof property.beginEdit) == 'function'){
					property.beginEdit();
				}
			}
 		},

        rollback: function() {
			// Loop over this object properties
			for( var propertyName in this){
				var property = this[propertyName];
				
				// If the property is an observable
				if(ko.isObservable(property) && (typeof property.rollback) == 'function'){
					property.rollback();
				}
			}
     	},

        commit: function () {
			// Loop over this object properties
			for( var propertyName in this){
				var property = this[propertyName];
				
				// If the property is an observable
				if(ko.isObservable(property) && (typeof property.commit) == 'function'){
					property.commit();
				}
			}
     	},
        
		makeEditable: function() {
			// Loop over this object properties
			for( var property in this){
				// If the property is an observable
				if(ko.isObservable(this[property])){
					// Apply the editable behaviour
					this[property].extend({ editable: true });
				}
			}
		}
	});
})();
