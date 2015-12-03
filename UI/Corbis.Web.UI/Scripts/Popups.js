if (typeof(CorbisUI) == 'undefined') {
    CorbisUI = {};
}
CorbisUI.Popup = new Class({

    Implements: [Options],  
   
    //options  
    options: {  
        showModalBackground: true,
        centerOverElement: null,
        positionVert: 'top',
        positionHoriz: 'middle',
        closeOnLoseFocus: false
    },  
    
    setCloseOnLoseFocus: function(elementId) {
        if (this.options.closeOnLoseFocus) {
            $('modalOverlay').addEvent('click', function() { MochaUI.HideModal(elementId); } );
        }
    },
    
    setPosition: function(elementId) {
        var el = $(elementId + "Window");
        
        if (this.options.centerOverElement != null)
        {
            var openerElement = $(this.options.centerOverElement);
            if (openerElement) {
                var openerPos = openerElement.getCoordinates();
        		var scrollPos = document.getScroll();
        		
        		switch (this.options.positionVert) {
        		    case "top":
        		        el.setStyle("top", openerPos.top + openerPos.height - el.getCoordinates().height);
        		        break;
        		    case "middle":
        		        el.setStyle("top", openerPos.top + (openerPos.height/2) - (el.getCoordinates().height/2) );
        		        break;
        		    case "bottom":
        		        el.setStyle("top", openerPos.top);
        		        break;
        		}
        		
        		switch (this.options.positionHoriz) {
        		    case "left":
        		        el.setStyle("left", openerPos.left - el.getCoordinates().width + openerPos.width);
        		        break;
        		    case "middle":
        		        el.setStyle("left", openerPos.left + (openerPos.width/2) - (el.getCoordinates().width/2) );
        		        break;
        		    case "right":
        		        el.setStyle("left", openerPos.left);
        		        break;
        		}

            }
        }
        
    },

    initialize: function(elementId, options, openerElement) {
        this.setOptions(options);
        
        if (!$('modalOverlay')) {
            MochaUI.Modal = new MochaUI.Modal();
        }

        this.setCloseOnLoseFocus(elementId);

        if ($(elementId + "Window")) {
            $(elementId + "Window").setStyle('display', 'block');
	        if (Browser.Engine.trident4){
	            $('modalFix').setStyle('display', 'block');
	        }
        } else {
            MochaUI.NewModalFromDiv($(elementId));
        }
        
        this.setPosition(elementId);
    
        if (!this.options.showModalBackground) {
            $('modalOverlay').setStyle('opacity', .01);
        } else {
            $('modalOverlay').setStyle('opacity', .4);
        }
    
        
    }
    
    
    
});    