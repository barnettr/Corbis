window.addEvent('load', function() {
	SetupBrowseNav();
	SetupPopupWindows();
});

function SetupBrowseNav() {
    //Find the 'Motion' link in dropdown and turn it white
    if ($('dropDownMenuLeftDiv') != null){
		var spanList = $('dropDownMenuLeftDiv').getElements('span');
		spanList.each(function(el) {
			if (el.id.indexOf('Creative') >= 0) {
				var theLink = el.getParent().getParent();
				theLink.href = "javascript:void(0);";
				theLink.setStyle('cursor', 'default');
				theLink.addEvents('click', function() { return false; });
			}
			if (el.id.indexOf('Editorial') >= 0) {
				var theLink = el.getParent().getParent();
				theLink.href = "javascript:void(0);";
				theLink.setStyle('cursor', 'default');
				theLink.addEvents('click', function() { return false; });
			}
            if (el.id.indexOf('Motion') >= 0) {
				var theLink = el.getParent().getParent();
				theLink.setProperty('target', '_blank');
				theLink.href = "http://www.corbismotion.com/";
                el.setStyle('color', 'white');
                el.getParent().getParent().addEvent('mouseover', function() {
                    el.getParent().getParent().getParent().addClass('MotionHover');
                });
                el.getParent().getParent().addEvent('mouseout', function() {
                    el.getParent().getParent().getParent().removeClass('MotionHover');
                });
            }
		});
    }
}

// ----------------------------------------------------------------------------	
// POPUP windows that degrade gracefully. 
// To customize a window size add below like this "WIDTH_HEIGHT_popup"
// Javascript dependency Mootools 1.2+

function SetupPopupWindows(){
	// get all links with a rel tag ending in 'popup'
	var linkList = $(document.body).getElements('a[rel$=popup]').each(function(el){
		var href = el.href;
		var rel = el.rel;
		var metadata = rel.split('_');
		var width = metadata[0];
		var height = metadata[1];
		el.addEvent('click', function(){
			OpenPopupWindow(href, width, height);
			return false;
		});
	});
}

function OpenPopupWindow(href, width, height){
	window.open(href,'Popped','resizable=yes,scrollbars=no,width='+width+',height='+height);
}

// ----------------------------------------------------------------------------	
// SEARCH RESULTS windows that degrade gracefully. 
// From popup slideshow windows, this script will detect if a parent window
// exists, if so, it will show the search results in the parent window. If not,
// it will show the search results in the current window.
// Javascript dependency Mootools 1.2+

function DisplaySearchResults(href){
	var displayWindow = window;
	
	// See if the user has closed the parent window, if not...
	if (window.opener)
		displayWindow = window.opener;
		
	// Move the window to the front and display the search results
	displayWindow.focus();
	displayWindow.location.href = "/Search/SearchResults.aspx?q=" + href;
}