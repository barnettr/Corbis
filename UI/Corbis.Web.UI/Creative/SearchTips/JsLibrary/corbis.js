if (!CookieDomain) {
	var CookieDomain = window.location.host;
}
if (!HttpUrl) {
	var HttpUrl = "http://" + window.location.host + "/";
}
if (!HttpsUrl) {
	var HttpsUrl = "https://" + window.location.host + "/";
}

if (!languageCode)
{
	var languageCode = "";
 	if (document.cookie.indexOf("ja%2DJP") > -1)
	{
	  languageCode = "ja-JP"
	}
	else if( document.cookie.indexOf("zh%2DCHS") > -1)
	{
		languageCode = "zh-CHS";
	}
}

function openInfEn(qs, inf)
{
	var page = "/popup/";
	var f = "status=no,scrollbars=yes,resizable=yes,width=775,height=600";

	if(inf)
	{
		page+="Enlargement.aspx?"+qs;
		//page+="information.asp?"+qs;	
	}
	else
	{
		page+="Enlargement.aspx?"+qs;
		//page+="enlargement.asp?"+qs;
	}
	if (isMac())
	{
		f = "toolbar=yes," + f;
	}

	MM_openBrWindow(page,"",f)
}

function switchImageSetPanel(oDiv, bOn)
{
	
	if(bOn)
	{
		this.style.borderColor = 'black';
		this.style.backgroundColor = '#cc3333';
		this.style.color = '#ffffff';
	}
	else
	{
		this.style.borderColor = '#cc3333';
		this.style.backgroundColor = 'white';
		this.style.color = '#000000';
	}
	


}

var totalSelectedImages = 0;
var advancedWin


if(languageCode == 'ja-JP')
{
	if( document.all ) 
	{
		document.write( '<link rel="stylesheet" type="text/css" href="/style/ja-JP/iestyle.css">' )
	}
	else
	{
		document.write( '<link rel="stylesheet" type="text/css" href="/style/ja-JP/nsstyle.css">' )
		document.close()
	}
	//document.write( '<link rel="stylesheet" type="text/css" href="/style/ja-JP/style.css">' )
}
else if(languageCode == 'zh-CHS')
{
	if( document.all ) 
	{		
		document.write( '<link rel="stylesheet" type="text/css" href="/style/zh-CHS/iestyle.css">' )
	}
	else
	{
		document.write( '<link rel="stylesheet" type="text/css" href="/style/zh-CHS/nsstyle.css">' )
		document.close()
	}
	//document.write( '<link rel="stylesheet" type="text/css" href="/style/zh-CHS/style.css">' )
}
else
{
	if( document.all ) 
	{
	document.write( '<link rel="stylesheet" type="text/css" href="/iestylesheet.css">' )
	}
	else
	{
		document.write( '<link rel="stylesheet" type="text/css" href="/nnstylesheet.css">' )
		document.close()
	}
}

function isIE()
{
	var sBrow = navigator.appName;

	if(sBrow.indexOf("Microsoft") != -1)
	{
		return true;
	}
	else
	{
		return false;
	}
}

function SwitchImage(o, sNewImage) 
		
	{	
		o.src = sNewImage
		
	}

function isMac(){
	if (navigator.appVersion.indexOf("Macintosh",1)>0){
	   return true;
	}
	 return false;
}
 
function MM_openStandardPopup(theURL, winName, features){
  //this function ignores features, allows the to be set here
   var newFeatures = "status=no,scrollbars=yes,resizable=yes,width=770,height=670";
   if (isMac()){newFeatures = "toolbar=yes," + newFeatures}
   MM_openBrWindow(theURL,winName,newFeatures);
}

function MM_openGeorgePopup(theURL, winName){
   MM_openBrWindow(theURL,winName,"status=no,scrollbars=yes,resizable=no,width=750,height=670");
}

function openHelpWindow(theURL)
{
  //this function ignores features, allows the to be set here
	
	var oCookie = new Cookie("Preferences", null, "/", CookieDomain, true)
	oCookie.load()
	var sLang;
	if(window.location.href.indexOf("https://") == -1)
	{
		sLang = "/creative/"+oCookie.PLanguageCode;
	}
	else
	{
		sLang = "";
	}
	var newFeatures = "status=no,scrollbars=yes,resizable=yes,width=570,height=450";
	var helpWin = window.open(sLang+theURL, "Help", newFeatures);
	
	helpWin.focus()
}

function openHelp(url)
{
	// Open help window without SSL
	if(window.location.href.indexOf("https://") == 0) {
		url = HttpUrl + url;
	}
	var attr = "status=no,scrollbars=yes,resizable=yes,width=570,height=550";
	if (!url) {
		url = "/help";
	}
	var helpWin = window.open(url, "Help", attr);
	helpWin.focus();
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
function delay(win)
{
	advancedWin.focus()
	return true;
}
function openAdvancesearchWindow(theURL, theName)
{
	var newFeatures = "status=yes,scrollbars=yes,resizable=yes,width=570,height=500";
	advancedWin = window.open(theURL, theName, newFeatures);
	
	advancedWin.opener = self;
	var iTimeout = window.setTimeout("delay()", 500)

	document.frmASHidden.target = theName
	document.frmASHidden.submit();
}

function MM_jumpMenu(targ,selObj,restore){ //v3.0
  eval(targ+".location='"+selObj.options[selObj.selectedIndex].value+"'");
  if (restore) selObj.selectedIndex=0;
}

function MM_findObj(n, d) { //v3.0
  var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
    d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
  if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
  for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document); return x;
}

function MM_showHideLayers() { //v3.0
  var i,p,v,obj,args=MM_showHideLayers.arguments;
  for (i=0; i<(args.length-2); i+=3) if ((obj=MM_findObj(args[i]))!=null) { v=args[i+2];
    if (obj.style) { obj=obj.style; v=(v=='show')?'visible':(v='hide')?'hidden':v; }
    obj.visibility=v; }
}

function deselectAll() {
	document.forms[1].reset();
	for (var i=0; i<document.images.length; i++) {
		document.images[i].style.border='none'; 
	}
}

function selectAll() {
	var currCheckbox
	for (var i=0; i<document.forms[1].elements.length; i++) {
		document.forms[1].elements[i].checked = true;
	}
	for (var i=1; i<document.images.length; i++) {
		document.images[i].style.borderStyle='solid';
		document.images[i].style.borderColor='yellow';
		document.images[i].style.borderWidth='medium';

	}
}
	
function selectItem(image) {
currImage = image + 1;
	if (document.forms[1].elements[image].checked == false) {
		document.images[currImage].style.borderStyle='solid';
		document.images[currImage].style.borderColor='yellow';
		document.images[currImage].style.borderWidth='medium';
		document.forms[1].elements[image].checked = true;
	} else if (document.forms[1].elements[image].checked == true) {
		document.forms[1].elements[image].checked = false;
		document.images[currImage].style.border='none'; 
	}
}

function selectItemCheckbox(image) {
currImage = image + 1;
	if (document.forms[1].elements[image].checked == true) {
		document.images[currImage].style.borderStyle='solid';
		document.images[currImage].style.borderColor='yellow';
		document.images[currImage].style.borderWidth='medium';
		document.forms[1].elements[image].checked = true;
	} else if (document.forms[1].elements[image].checked == false) {
		document.forms[1].elements[image].checked = false;
		document.images[currImage].style.border='none'; 
	}
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

function openFlashWindow() 
{ 
	window.open("http://www.corbis.com/humans/primary.htm", "humansWin", "height=520,width=740,scrollbars=0,resizable=0,toolbar=0,menubar=0,location=0"); 
}




// Cookie Class

/*******************************************************
*	cookie.js
*
*	Provides a standard and easy way of retrieving and maniputlating
*	multi-dimensional cookies in client-side JavaScript.  Compatible with
*	IE and Netscape 4.x and above
*
*	Class contains dynamic properties assigned at runtime
*
*	For example:
*		var myCookie = new Cookie(document, "Stuff", "", "/", ".corbis.com", false)
*		myCookie.Name = "Darren Davis"
*
*	Class also contains a length property which returns the number of dynamic properties
*	assigned
*
*	Methods:
*
*		save()					-	Saves any current values in the object as a cookie 
*									on the browser
*		modify(name, value)		-	Updates the property (name) with the value.  Returns true 
*									on success
*		load()					-	Loads the values from an existing cookie (based on the 
*									name argument)into the object
*		remove					-	deletes the cookie from the cookie collection
*
*******************************************************/


//Constructor

function Cookie(name, exp, path, domain, secure)
{
	this.$document = document;
	this.$name = name;
	if(exp)
	{
		this.$expire = new Date(exp)
	}
	else
	{
		this.$expire = null
	}
	if(path) 
	{
		this.$path = path; 
	}
	else 
	{
		this.$path="/"
	}
	
	if(domain) 
	{
		this.$domain = domain; 
	}
	else 
	{
		this.$domain=null
	}
	
	if(secure) 
	{
		this.$secure = true; 
	}
	else 
	{
		this.$secure=false
	}
	this.length = 0	
	this.modify = Cookie_modify
	this.remove = Cookie_remove;
	this.save = Cookie_store;
	this.load = Cookie_load;
	
return this
}

function Cookie_store()
{
	//Loops through the properties of the cookie and writes them to the browser
	
	var cookieval = "";
	
	for(var prop in this)
	{
		if ((prop.charAt(0) == "$") || ((typeof this[prop]) == "function") || prop == "length")
		{
			continue;
		}
		if (cookieval != "") cookieval +='&'

		cookieval += prop + '=' + escape(this[prop])
	}
	
	var cookiething = this.$name + '=' + cookieval

	if(this.$expire)
	{
		cookiething += '; expires=' + this.$expire.toGMTString();
	}
	if (this.$path) 
	{
		cookiething+= '; path=' + this.$path
	}
	if (this.$domain) 
	{
		cookiething+= '; domain=' + this.$domain
	}
	if (this.$secure) 
	{
		cookiething+= '; secure'
	}
	this.$document.cookie = cookiething
}

function Cookie_load()
{
	var allcookies = this.$document.cookie;
	if(allcookies == "") 
	{
		return false;
	}
	
	var start = allcookies.indexOf(this.$name + '=');
	if(start == -1) 
	{
		return false;
	}
	
	start += this.$name.length +1;
	var end = allcookies.indexOf(";", start);
	if(end == -1) 
	{
		end = allcookies.length;
	}
	
	var cookieval = allcookies.substring(start, end)
	
	var a = cookieval.split('&');
		
	for(var i=0;i<a.length;i++)
	{
		a[i] = a[i].split('=');
	}
	for(var i=0;i<a.length;i++)
	{
		this[a[i][0]] = unescape(a[i][1])
	}
	
	this.length = a.length
	return true
} 

function Cookie_remove()
{
	var cookiething = this.$name + '=';
	if(this.$path) 
	{
		cookiething += '; path=' + this.$path;
	}
	if(this.$domain)
	{ 
		cookiething += '; domain=' + this.$domain;
	}
	cookiething += '; expires = Fri, 02-Jan-1970 00:00:00 GMT';
	
	this.$document.cookie = cookiething
}

// Modify Function has to re-write the cookie each time so it will persist.  After the 
// Cookie has been modified it must be reloaded to reflect the changes back to the client

function Cookie_modify(sName, sValue)
{
	if(sName == "")
		return false
	var bChanged = false;
	var cookieval = "";
	
	for(var prop in this)
	{
		if ((prop.charAt(0) == "$") || ((typeof this[prop]) == "function") || prop == "length" || prop == "allcookie")
		{
			continue;
		}
		if (cookieval != "") cookieval +='&'

		if(prop == sName)
		{
			cookieval += prop + '=' + escape(sValue)
			bChanged = true
		}
		else
		{
			cookieval += prop + '=' + escape(this[prop])
		}
	}
	
	var cookiething = this.$name + '=' + cookieval

	if(this.$expire)
	{
		cookiething += '; expires=' + this.$expire.toGMTString();
	}
	if (this.$path) 
	{
		cookiething+= '; path=' + this.$path
	}
	if (this.$domain) 
	{
		cookiething+= '; domain=' + this.$domain
	}
	
	if (this.$secure) 
	{
		cookiething+= '; secure'
	}
	this.$document.cookie = cookiething
	this.load()
	return bChanged;
}


/*
==================================================================
'	populateRegions
'	Purpose:	Clear the region box and call FillRegionBox
'
'	Parameters:	oCountryBox - the Country select box
'				oRegionBox - the Region box
'				strSelectedRegionCode - selected region code (used when editing a previously saved record)
'				aRegions - the array containing all of the regions for all of the countries
'				strSelectOneText - the localized version of "Select One" that appears at the top of the list
'				oSelectedRegionIndex - this is passed in as a hidden field in order to support Mac with older verions
'									   of Netscape.  It is used by the caller with a setTimeOut to display properly
'									   on these older verions.
'
'	Returns:	the modified region box and the selected region index
'
'	Author:		Debbie Douglas
'	Date:		05/05/02
'   Modified:   
'==================================================================
*/
function populateRegions(oCountryBox, oRegionBox, strSelectedRegionCode, aRegions, strSelectOneText, oSelectedRegionIndex) 
{
	var strCountryCode = oCountryBox.options[oCountryBox.selectedIndex].value
	clearBox(oRegionBox)
	FillRegionBox(aRegions, strCountryCode, oRegionBox, strSelectedRegionCode, strSelectOneText, oSelectedRegionIndex)
	return
}




/*
==================================================================
'	FillRegionBox
'	Purpose:	Populate the region box according to the selected country
'				If region has been previously selected, set the index
'
'	Parameters:	aRegions - the array containing all of the regions for all of the countries
'				strCountryCode - the selected country code
'				oRegionBox - the Region box
'				strSelectedRegionCode - selected region code (used when editing a previously saved record)
'				strSelectOneText - the localized version of "Select One" that appears at the top of the list
'				oSelectedRegionIndex - this is passed in as a hidden field in order to support Mac with older verions
'									   of Netscape.  It is used by the caller with a setTimeOut to display properly
'									   on these older verions.
'
'	Returns:	the modified region box and the selected region index
'
'	Author:		Debbie Douglas
'	Date:		05/05/02
'   Modified:   
'==================================================================
*/
function FillRegionBox(aRegions, strCountryCode, oRegionBox, strSelectedRegionCode, strSelectOneText, oSelectedRegionIndex)
{
	var strRegionsCountryCode
	var strRegionCode
	var strRegionDescription
	var iOptions
	var iPos1
	var iPos2
	var iSelIndex = 0
	var iCount = 0

	makeRegionOption(oRegionBox, strSelectOneText, "", false)

	for(x=0;x<aRegions.length;x++)
	{
		var sCompare = new String(aRegions[x])
			
		iPos1 = sCompare.indexOf("|")
		strRegionsCountryCode = sCompare.substring(0, iPos1)
		if(strRegionsCountryCode == strCountryCode)
		{
			iCount++
			iPos2 = sCompare.indexOf("^")
						
			strRegionCode = sCompare.substring(iPos1+1,iPos2)
			strRegionDescription = sCompare.substring(iPos2+1, sCompare.length)
			
		
			if(strRegionCode == strSelectedRegionCode)
			{

				iSelIndex = iCount 
				oSelectedRegionIndex.value = iSelIndex //support for Mac and older verions of Netscape
			
				makeRegionOption(oRegionBox, strRegionDescription, strRegionCode, true)
			}
			else
			{
				makeRegionOption(oRegionBox, strRegionDescription, strRegionCode, false)
			}
		}
	}
	if(oRegionBox.options.length>=2)
	{
		oRegionBox.selectedIndex = iSelIndex
	}
	else
	{
		clearBox(oRegionBox)
		makeRegionOption(oRegionBox, "---------", "", false)
	}
	return //iSelIndex;

}





/*
==================================================================
'	makeRegionOption
'	Purpose:	Creates a new option for the passed in region box
'
'	Parameters: oRegionBox - the Region box
'				strRegionDescription - the region description
'				strRegionCode - the region code
'				blnIsDefault - is this option default or selected
'
'	Returns:	the modified region box 
'
'	Author:		Debbie Douglas
'	Date:		05/05/02
'   Modified:   
'==================================================================
*/
function makeRegionOption(oRegionBox, strRegionDescription, strRegionCode, blnIsDefault)
{

	var opt = new Option(strRegionDescription, strRegionCode, false, false)
	var l = oRegionBox.options.length
	oRegionBox.options[l] = opt
	return;
}



function voidSelection(box)
{
	clearBox(box)
	makeRegionOption(box, "---------", "", true)
}

function clearBox(box)
{
	for(x=box.options.length;x>0;x--)
	{
		box.options[0] = null;
	}
	return
}


/*
==================================================================
'	populateDays
'	Purpose:	Populate days of the month based on passed in month
'
'	Parameters: MonthIndex - the selected month
'				DaysBox - the days of the month box
'
'	Returns:	the modified days box 
'
'	Author:		Debbie Douglas
'	Date:		08/21/02
'   Modified:   
'==================================================================
*/
function populateDays(MonthIndex, DaysBox)
{

	clearBox(DaysBox)
	switch (MonthIndex)
	{
		case 9:
		case 4:
		case 6:
		case 11:
			for (var day=1; day<=30; day++) 
			{
				if (day.length==1)
				{
					var dayValue = "0" + day
				}
				else
				{
					var dayValue = day
				}
				var opt = new Option(day, dayValue, false, false)
				var l = DaysBox.options.length 
				DaysBox.options[l] = opt
			}
			break;
		
		case 1:
		case 3:
		case 5:
		case 7:
		case 8:
		case 10:
		case 12:
			for (var day=1; day<=31; day++) 
			{
				if (day.length==1)
				{
					var dayValue = "0" + day
				}
				else
				{
					var dayValue = day
				}
				var opt = new Option(day, dayValue, false, false)
				var l = DaysBox.options.length 
				DaysBox.options[l] = opt
			}
			break;
			
		case 2:
			for (var day=1; day<=28; day++) 
			{
				if (day.length==1)
				{
					var dayValue = "0" + day
				}
				else
				{
					var dayValue = day
				}			
				var opt = new Option(day, dayValue, false, false)
				var l = DaysBox.options.length 
				DaysBox.options[l] = opt
			}	
			break;
	}

}

var movePending = false;
var oDebugDiv = null
function startmoving()
{
	e = window.event;
	oDebugDiv = e.srcElement	
	movePending = true;
}
function hover()
{
	if(movePending)
	{
		e = window.event;
			if (oDebugDiv)
			{
				oDebugDiv.style.visibility = 'visible';
				oDebugDiv.style.posLeft=e.clientX + 10  +  document.body.scrollLeft  
				oDebugDiv.style.posTop=e.clientY + document.body.scrollTop;
			}
	}
}
function unHover()
{
	e = window.event;
	
	if (e.type == 'mousedown') 
	{
		if (e.srcElement.tagName != "DIV")
		{
			movePending = false;
		}
	}
}

function jsWriteIt()
{
	if (document.all)
	{
	document.onmousemove = hover;
	document.onmousedown = unHover;
	}

	if(document.all['writeit'].length > 1)
	{
		for(var i = 0;i<document.all['writeit'].length;i++)
		{
			document.all['writeit'](i).onmousedown = startmoving;
		}
	}
	else
	{
		document.all['writeit'].onmousedown = startmoving;
	}
}

function fnChangeLanguage()
{	
	var sURL;
	sURL = window.top.location.href;	
	document.frmChangeLanguage.action = sURL;	
	document.frmChangeLanguage.submit();	
}
