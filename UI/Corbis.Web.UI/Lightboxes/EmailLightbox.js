/***************************
    NAMESPACES
****************************/
if (typeof(CorbisUI) == 'undefined') {
    CorbisUI = {};
}

CorbisUI.EmailLightbox = {};

/***************************
    HANDLER FUNCTIONS
****************************/

CorbisUI.EmailLightbox.Handler = {
    showEmailModal: function(lightboxId, lightboxUid, lightboxName, onhideHandler)
    {
		var emailLightbox = $('emailLightbox');
		var titleSpan = emailLightbox.getElement('span.Title');
		var lightboxNameSpan = titleSpan.getElement('span');
		if (!lightboxNameSpan)
		{
			lightboxNameSpan = new Element('span', {
				'styles' : {'font-weight': 'normal'}
			});
			
			lightboxNameSpan.inject(titleSpan);
		}

		lightboxNameSpan.set('html', HtmlFieldEncode(lightboxName));
		
		emailLightbox.getElement('input.lightboxId').value = lightboxId;
		var lightboxLink = window.location.href.replace(window.location.pathname, String.format(CorbisUI.GlobalVars.EmailLightbox.lightboxLinkTemplate, lightboxUid));
		emailLightbox.getElement('input.lightboxLink').value = lightboxLink;
		$('lightboxLinkDisplay').set('text', lightboxLink);

        var emailLightboxModal = new CorbisUI.Popup('emailLightbox', { 
            createFromHTML: false,
            showModalBackground: false,
            closeOnLoseFocus: true,
            centerOverElement: $('DetailsViewSide'), 
            positionVert: 'bottom', 
            positionHoriz: 'left',
            onHide: onhideHandler
            });
        ResizeModal('emailLightbox');

		emailLightbox.getElement('div.cancelButton input').addEvent('click', onhideHandler);
		emailLightbox.getElement('input.Close').addEvent('click', onhideHandler);	
		
		var successModal = $('emailSuccess');
		successModal.getElement('div.closeButton input').addEvent('click', onhideHandler);
		successModal.getElement('input.Close').addEvent('click', onhideHandler);	
            
		window.addEvent('resize', function(){
			this.setPosition('emailLightbox');
		}.bind(emailLightboxModal));
    },
    
    closeModal: function()
    {
		var emailLightbox = $('emailLightbox')
		emailLightbox.getElement('#formFields').getElements('input,textarea').each(function(el){el.value='';});
		emailLightbox.getElement('div.Error').empty();
		emailLightbox.getElements('tr.ErrorRow').each(function(el){el.removeClass('ErrorRow');});
		emailLightbox.getElements('tr.ErrorHighlight').each(function(el){el.removeClass('ErrorHighlight');});
		ResizeModal('emailLightbox');
		HideModal('emailLightbox'); 
    },
    
    showSuccessModal: function()
    {
		CorbisUI.EmailLightbox.Handler.closeModal();
		new CorbisUI.Popup('emailSuccess', { 
            createFromHTML: false,
            showModalBackground: false,
            closeOnLoseFocus: true,
            centerOverElement: $('DetailsViewSide'), 
            positionVert: 'bottom', 
            positionHoriz: 'left'
        });
    },
    
    encodeFields: function()
    {
		var emailLightbox = $('emailLightbox')
		emailLightbox.getElement('#formFields').getElements('input,textarea').each(function(el){el.value=HtmlFieldEncode(el.value);});
    }
};

