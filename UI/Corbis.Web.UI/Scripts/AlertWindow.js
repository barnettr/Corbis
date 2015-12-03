MochaUI.extend({
	NewAlertFromDiv: function(el){
		el.setStyle('display','block')
		var elDimensions = el.getSize();
		var properties = {
    		title: '<div style="padding-left: 10px; padding-top; 55px;">Are you sure?</div>',
		    collapsible: false,
		    draggable: false,
			minimizable: false,
		    contentBgColor: '#dbdbdb',
		    useCanvasControls: false,
		    headerStartColor:[102,102,102],
		    headerStopColor:[102,102,102],
		    footerStartColor:[229,229,229],
		    footerStopColor:[229,229,229],
		    cornerRadius: 5,
		    headerHeight: 20,
		    footerHeight: 5,
			padding: 100,
        	scrollbars: false,
        	closable: true,
        	type: 'window',
			id: el.getProperty('id') + "Window",
		    height: 45,
		    width: elDimensions.x,
		    content: '' 
		};
	    
		// Create window
		new MochaUI.Window(properties);
		el.injectInside($(el.getProperty('id') + "Window_content"));
		$(el.getProperty('id') + "Window").injectInside(document.forms[0]);
	}
});

function OpenAlertWindow(divId, caller) {
            
        if ($(divId + "Window")) {
        $(divId + "Window").setStyle('display', 'block');
        } else {
        MochaUI.NewAlertFromDiv($(divId));
        var position = $(caller).getCoordinates();
        var dimensions = $(divId + 'Window').getCoordinates();
        $(divId + 'Window').setStyles({
				'top': position.top - (dimensions.height / 2),
				'left': position.left - (dimensions.width /2 )
			});
        
    }
}

function HideAlertWindow(divId) {
    //$(divId + "Window").setStyle('display', 'none');
    //MochaUI.closeWindow($(divId+ 'Window')); 

}
