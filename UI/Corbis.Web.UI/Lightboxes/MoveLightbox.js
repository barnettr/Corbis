/***************************
    NAMESPACES
****************************/
if (typeof(CorbisUI) == 'undefined') {
    CorbisUI = {};
}
CorbisUI.MoveLightbox = {};

/***************************
    HANDLER FUNCTIONS
****************************/
function onGetLightBoxDropDownListForMoveSuccess(result, userName, methodName){   
    var pair, op;  
    var ddlLightBox = $('ddlLightBox');
    for (i = ddlLightBox.length - 1; i>1; i--) {  
        ddlLightBox.remove(i);
    }

    if (result && result.length > 0) {
        //neither way works for all browsers, so we have to do this instead.
        if (Browser.Engine.gecko) {
            var optionsString = '';
            result.each(function(item) {
                var pair = item.split('~');
                optionsString += String.format('<option value="{0}"><span>{1}</span></option>', pair[1], pair[0]);
            });
            ddlLightBox.set('html', ddlLightBox.get('html') + optionsString);
        }
        else {
            result.each(function(item) {
                var pair = item.split('~');
                var optionItem = new Element('option', {
                    'html': '<span>' + pair[0] + '</span>',
                    'value': pair[1]
                });
                optionItem.inject(ddlLightBox);
            });
        }
    }
}

CorbisUI.MoveLightbox.Handler = {
    showMoveModal: function(moveButton)
    {		
		var activeLightbox = $('Tree').getElement('div.Active');		
    	if (activeLightbox)
	    {
		    var selectedLightboxId = activeLightbox.get('id');
		    var sortBy = $('lightboxesContent').getElement('select.SortBy').value;
            sortBy = sortBy == "date" ? 2 : 1;		
            Corbis.Web.UI.Lightboxes.LightboxScriptService.GetLightBoxDropDownListForMove(selectedLightboxId, sortBy, onGetLightBoxDropDownListForMoveSuccess);
        }
             
        var moveLightboxModal = new CorbisUI.Popup('moveLightbox', {            
            showModalBackground: false,
            closeOnLoseFocus: true,
            centerOverElement: $('DetailsViewSide'), 
            positionVert: 'bottom', 
            positionHoriz: '130px',
            onHide: function(){moveButton.removeClass('selected');}
			}); 

			//set event to deselect menu button 
			$('moveLightbox').getElement('div.cancelButton').addEvent('click', function(){moveButton.removeClass('selected');});	      
    }
};

function moveLightBox()
{
    var ddlLightBox = $('ddlLightBox');
    var activeLightbox = $('Tree').getElement('div.Active');
    
    if(activeLightbox)
    {
	    LogOmnitureEvent("event13");  
	    var selectedLightBoxId = activeLightbox.get('id');
	    var newParentId = ddlLightBox.options[ddlLightBox.selectedIndex].value;	
	    Corbis.Web.UI.Lightboxes.LightboxScriptService.MoveLightBox(selectedLightBoxId, newParentId, function(){self.location.reload();});
    }	
}

function closeModal()
{
    if($('errorblock')!=null && $('errorblock').style.display == "block" )
    {
        $('errorblock').style.display = "none";
    }
    if($('mbInstructions') != null &&  $('mbInstructions').style.display == "none")
    {
        $('mbInstructions').style.display = "block";
        $('moveLightbox').getElement('tr.moveRow').style.backgroundColor = "#e8e8e8";
    }
	HideModal('moveLightbox');
	//moveButton declares and initializes in MyLightBoxes.aspx
	$('moveButton').removeClass('selected');
}
function reloadParentWindow()
{
    var moveLightbox = $('moveLightbox');
    if( moveLightbox != null)
        HideModal('moveLightbox');
}
     


