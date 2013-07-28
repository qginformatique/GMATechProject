// Add indexOf() function to Array because IE8 (and older) doesn't implement it
// (source: https://developer.mozilla.org/en-US/docs/JavaScript/Reference/Global_Objects/Array/IndexOf @chapter: Compatibility)
if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function(obj, start) {
	    for (var i = (start || 0), j = this.length; i < j; i++) {
	         if (this[i] === obj) { return i; }
	    }
	    return -1;
	}
}

// Add trim() function to string because IE8 (and older) doesn't implement it
if(typeof String.prototype.trim !== 'function') {
  String.prototype.trim = function() {
    return this.replace(/^\s+|\s+$/g, ''); 
  }
}

function durationFormat(startDate, endDate, withYear)
{
	if(startDate != null && endDate != null)
	{
		// Date parsing
		var start = moment(startDate);
		var end = moment(endDate);
		var result = "";
		
		// Check if it is the same Day
		if(start.isSame(end))
		{
			// return "Le " + _startDate
	        result = "Le " + start.format("D MMMM");
	        
	        if(withYear)
			{			
				result += " " + start.year();
			}
		}
		else
		{
			if(withYear)
			{
				// Check if it is the same month
				if(start.month() == end.month())
				{
					// Check if endDate is next day of startDate
					if(start.date() + 1 == end.date())
					{
						result = start.format("D") + " et " + end.format("D MMMM");
					}
					else
					{
						result = "Du " + start.format("D") + " au " + end.format("D MMMM");
					}
					result += " " + start.year();
				}
				else
				{
					if(start.year() == end.year())
					{
						// return "Du " + _startDate + " au " + _endDate"
						result = "Du " + start.format("D MMMM") + " au " + end.format("D MMMM") + " " + start.year();
					}
					else
					{
						// return "Du " + _startDate + " au " + _endDate"
						result = "Du " + start.format("D MMMM") + " " + start.year() + " au " + end.format("D MMMM")+ " " + end.year();
					}
				}
			}
			else
			{
				// Check if it is the same month
				if(start.month() == end.month())
				{
					// Check if endDate is next day of startDate
					if(start.date() + 1 == end.date())
					{
						result = start.format("D") + " et " + end.format("D MMMM");
					}
					else
					{
						result = "Du " + start.format("D") + " au " + end.format("D MMMM");
					}
				}
				else
				{
					// return "Du " + _startDate + " au " + _endDate"
					result = "Du " + start.format("D MMMM") + " au " + end.format("D MMMM");
				}
			}
		}
		
		return result;
	}
	
	return null;
}

function readableFileSize(size) {
    var units = ['octets', 'Ko', 'Mo', 'Go'];
    var i = 0;
    while(size >= 1024) {
        size /= 1024;
        ++i;
    }
    return size.toFixed(1) + ' ' + units[i];
}