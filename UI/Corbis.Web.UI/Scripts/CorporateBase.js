window.addEvent('load', function() {
	SetupPopupWindows();
	SetupModalWindows();
	if ($('pressReleases') != null){
		fixLinks();
		hideElements('Year2009');
	}
	DisableCurrentPageLink();
});

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
// MODAL windows that degrade gracefully. 
// To customize a window size add below like this "WIDTH_HEIGHT_modal"
// Javascript dependency Mootools 1.2+

function SetupModalWindows(){
	// get all links with a rel tag ending in 'modal'
	var linkList = $(document.body).getElements('a[rel$=modal]').each(function(el){
		var href = el.href;
		var rel = el.rel;
		var metadata = rel.split('_');
		var width = metadata[0];
		var height = metadata[1];
		el.addEvent('click', function(){
			OpenNewIModal(href, 600, 600, 'Popped');
			return false;
		});
	});
	
	if ($('modalWrapper') != null){
		var closeLinks = $('modalWrapper').getElements('.closeLink').each(function(el){
			el.addEvent('click', function(){
				parent.MochaUI.CloseModal('Popped');
			});
		});
		
		var printLinks = $('modalWrapper').getElements('.printLink').each(function(el){
			el.addEvent('click', function(){
				alert("here");
			});
		});
	}
}

// ----------------------------------------------------------------------------	
// TABS that degrade gracefully. 
// Based on "Let The Eat Cake" method: http://www.alistapart.com/articles/eatcake
// Modified to use Mootools library
// Javascript dependency Mootools 1.2+

function fixLinks() {
    // Find the link elements that exist within the ddiSearchNav container
    // and add the show() method to them
    $('years').getElements('a').each(function(el) {
        var href = el.href;
        var id = el.id;
        
        if (href.indexOf('#') != -1) { // jump ref
            var index = href.indexOf('#') + 1;
            href = 'javascript:show("' + href.substring(index) + '");';
            el.href = href;
        }
    });
}

function activateLink(exempt) {
    // Find the link that corresponds to the tab we are showing
    // and make it appear active
    $('years').getElements('a').each(function(el) {
        if (el.href.indexOf(exempt) != -1)
        {
            el.addClass("active");
        } else {
            el.removeClass("active");
        }
    });
}

function hideElements(exempt) {
    if (!exempt) exempt = '';
    $('pressReleases').getElements('li').each(function(el) {
		el.addClass("hidden");
    })
    // Find the li elements that exist within the pressReleases
    // container and hide them by default except the selected one
	var selector = 'li[class*=' + exempt + ']';
    $('pressReleases').getElements(selector).each(function(el) {
		el.removeClass("hidden");
    });
    activateLink(exempt);
}

function show(what) {
    hideElements(what);
}

function sendFocus(what) {
    var obj = $(what);
    obj.focus();
}

// ----------------------------------------------------------------------------	
// LEFTNAV modifications that degrade gracefully. 
// Javascript dependency Mootools 1.2+

function DisableCurrentPageLink(){
	var currentPage = location.href;
	var index = currentPage.lastIndexOf("/")+1;
	currentPage = currentPage.substring(index);
	// Find the link that corresponds to the page we are currently on
    // disable it and then give it a class of "active"
    $('leftNav').getElements('a').each(function(el) {
		var href = el.href;
		var i = href.lastIndexOf("/")+1;
		href = href.substring(i);
        if (href == currentPage)
        {
			el.href = "javascript:void(0);";
            el.addClass("active");
        } else {
            el.removeClass("active");
        }
    });
}