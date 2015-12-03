/*
function openHelp(theURL)
{
  //this function ignores features, allows them to be set here
	var newFeatures = "status=no,scrollbars=yes,resizable=yes,width=570,height=450";
	var helpWin = window.open(theURL, "Help", newFeatures);
	
	helpWin.focus()
}*/

if (!HttpUrl) {
	var HttpUrl = "http://" + window.location.host + "/";
}
if (!HttpsUrl) {
	var HttpsUrl = "https://" + window.location.host + "/";
}

function openInfEn(qs, inf)
{
	var page = "/popup/Enlargement.aspx?"+qs;
	var f = "status=no,scrollbars=yes,resizable=yes,width=775,height=600";
	if (isMac())
	{
		f = "toolbar=yes," + f;
	}

	var w = window.open(page,"",f)
	w.focus();
}
function isMac()
{
	if (navigator.appVersion.indexOf("Macintosh",1)>0)
	{
	   return true;
	}
 return false;
}

function MM_openBrWindow(theURL,winName,features) 
{ 
	//Some Mac browsers return the parse date as a negative number.  Strip it out
	var sDate = Date.parse(Date()).toString()
	sDate = sDate.slice(1)
	iWindowCount = "win"+sDate 
	var pWindow = window.top.open(theURL,iWindowCount, features );
    	pWindow.focus()
}

// aspx pages use this function to Close the popup and refresh the parent window
function RefreshParentClosePopup()
{	
	var random = Math.random();
	var href = window.top.opener.top.location.href;
	if(href.indexOf('?') != -1)
	{
		href += '&i=' + random
	}
	else
	{
		href += '?i=' + random
	}
	window.top.opener.top.location = href;
	window.close();
}

// This function is used by the Enlargement to refresh the parent window with new search results.
function SetParentLocation(url)
{
	window.top.opener.top.location = url;
	window.top.opener.focus();
}
// This function is used by the Login popup to transfer. 
function SetParentLocationClosePopup(url)
{
	if (window.top.opener) {
		window.top.opener.top.location = url;
		window.close();
	} else {
		window.top.location = url;
	}
}

function OpenNewLanguagePopup(langCode)
{
	var url = '/popup/newLanguage.aspx?lcd=' + langCode ;
	var attr = "status=no,scrollbars=no,resizable=no,width=390,height=257";
	var langPopup = window.open(url,"langPopup",attr);	
}

function OpenPromoPopup(url,width,height)
{
	var attr = 'status=no,scrollbars=no,resizable=no,width=' + width + ',height=' + height;
	var promoPopup = window.open(url,"promoPopup",attr); 
}

// Used to open the help popup.
function openHelp(url)
{
	// Open help window without SSL
	if (!url) {
		url = "/help";
	}
	if(window.top.location.href.indexOf("https://") == 0) {
	
		url = HttpUrl + url;
	}
	var attr = "status=no,scrollbars=yes,resizable=yes,width=570,height=550";
	
	var helpWin = window.open(url, "Help", attr);
	helpWin.focus();
}

//Used by View Options popup.
//When image of view is clicked the corresponding radio button needs to be checked.
//That is the purpose of this function.
function ImageRadioClick(radioId)
{
	var radioObject = document.getElementById(radioId);
	radioObject.checked = true;
}

function newwindow(url)
{
	var params;
	var agent = navigator.userAgent;
	var windowName = "CorbisWindow";
	
	params = "";
	params += "height=556,"
	params += "width=603,"
	params += "left=50,"
	params += "top=50,"
	params += "alwaysRaised=0,"
	params += "directories=0,"
	params += "fullscreen=0,"
	params += "location=0,"
	params += "menubar=b,"
	params += "resizable=1,"
	params += "scrollbars=1,"
	params += "status=0,"
	params += "toolbar=0"
	
	var win = window.open(url,windowName,params);	
	 
	if (agent.indexOf("Mozilla/2") != -1 && agent.indexOf("Win") == -1) 
	{
		win = window.open(url, windowName , params);
	}
	
	if (!win.opener) 
	{
		win.opener = window;
	}
}


