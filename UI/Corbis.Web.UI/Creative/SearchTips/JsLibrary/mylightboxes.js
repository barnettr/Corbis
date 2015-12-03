function createLightbox()
{
	var w = window.open('createLightboxPopup.aspx?area=Lightbox','lbxoptions','width=600,height=450,scrollbars=yes')
	w.focus();
}


function lightboxOptions()
{
	var w = window.open('lightboxInfoPopup.aspx?fdid=' + lightboxUID,'lbxoptions','width=703,height=500,scrollbars=yes')
	w.focus();
}


function emailLightbox()
{
	var w = window.open('emailLightboxPopup.aspx?fdid=' + lightboxUID,'lbxoptions','width=703,height=500,scrollbars=yes')
	w.focus();
}

function openMessages()
{
		var w = window.open('lightboxMessagesPopup.asp','lbxoptions','width=600,height=450,scrollbars=yes')
	w.focus();
}


function priceItems()
{
	var w = window.top.open('/popup/pricinggeorge.aspx?Area=Folders&FDID=' + lightboxUID + '&qsPageNo=1','george','status=no,scrollbars=yes,resizable=no,width=700,height=570')
	w.focus();
		
    
}

function removeItems()
{
		
	var w = window.open('removegeorge.aspx?Area=Folders&FDID=' + lightboxUID + '&qsPageNo=1','george','status=no,scrollbars=yes,resizable=no,width=745,height=570')
	w.focus();
		
}

function copyItems()
{
	
		var w = window.open('mcgeorge.aspx?Area=Folders&FDID=' + lightboxUID + '&qsPageNo=1','george','status=no,scrollbars=yes,resizable=no,width=745,height=570')
		w.focus();
	
    
}

function changeLayout()
{
	var w =window.open('/popup/ChangeView.asp?container=Folders','ChangeView','status=no,scrollbars=no,resizable=no,width=416,height=440');
	w.focus();
}
function lbxOnChange(lbxDD)
{
	if (lbxDD.selectedIndex > 0) {
		window.top.location = "/myfolders/myfolders.aspx?fdid=" + lbxDD.options[lbxDD.selectedIndex].value;
	}
}
function DoNotCache()
{}

function addNote(sNote,sUID){	if (sNote != "") {		document.forms[0].Note.value = sNote;         // folder or foldermedia		document.forms[0].NoteUID.value = sUID;       // UID for media or folder		document.forms[0].submit();	}}
