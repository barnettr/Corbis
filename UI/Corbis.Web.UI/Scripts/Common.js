/***************************
    NAMESPACES
****************************/
if (typeof(CorbisUI) == 'undefined') {
    CorbisUI = {};
}

var getFFVersion=navigator.userAgent.substring(navigator.userAgent.indexOf("Firefox")).split("/")[1];
var FFextraHeight=parseFloat(getFFVersion)>=0.1? 16 : 0; //extra height in px to add to iframe in FireFox 1.0+ browsers

function LogOmnitureEvent(eventName)
{
    try
    {
        s=s_gi(s_account);
        s.linkTrackVars="events";
        s.linkTrackEvents=eventName;
        s.events=eventName;
        s.tl(this,'o','My Link Name');
    }
    catch(e){}
}

function NewWindow(url,width,height,resizable)
{
    if(Browser.Engine.trident){
        resizable = (resizable == "True" ) ? "yes" : "no";
    }
    var windowName = "Corbis";
	(window.open(url, windowName, 'width=' + width + ',height=' + height + '' + ',resizable=' + resizable)).focus();
}

function PricingModalPopupExit()
{
    parent.MochaUI.CloseModal('pricing');
}

function UpdateCartCount(numItems)
{
    var cartCounter = CorbisUI.DomCache.get('cartCount');
    if (cartCounter) {
        cartCounter.setProperty('text', numItems);   
        fixIECheckoutWidgetWidth(); 
    }
}

function Click(buttonID)
{
    button = $get(buttonID);
    if (button)
    {
        button.click();
    }
}

function ToggleChecked(inputID)
{
    input = $get(inputID);
    if (input)
    {
        input.checked = !input.checked;
    }
}

function SetImage(imgID, path)
{
    img = $get(imgID);
    if (img)
    {
        img.src = path;
    }
}

function Hide(id)
{
    obj = $get(id);
    if (obj)
    {
        obj.style.display = 'none';
    }
}

function Show(id)
{
    obj = $get(id);
    if (obj)
    {
        obj.style.display = 'block';
    }
}
function NoPasteKey(e)
{
    var keyCode = e.keyCode ? e.keyCode : e.which;
    if ((e.ctrlKey == true && String.fromCharCode(keyCode).toLowerCase() == 'v') ||
        (e.shiftKey == true && keyCode == 45) /* Shift+Insert */
        )
    {
        return false;
    }
    return true;
}

function IE()
{
    if (/MSIE (\d+\.\d+);/.test(navigator.userAgent))
    {
        var index = navigator.userAgent.indexOf('MSIE');
        var version = parseFloat(navigator.userAgent.substring(index + 5));
        return version;
    }
    return null;
}

function FixPng(img)
{
    var version = IE();
    if (version && version < 7)
    {
        img.parentNode.style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + img.src + "', sizingMethod='crop')";
        img.className = 'displayNone';
        return;
    }
    img.className = 'block';
}

function DefineAnchorClick(linkId)
{
    link = $get(linkId);
    if (link && typeof(link.click == 'undefined'))
    {
        link.click = function()
        {
            // attached on load, detach after clicked to avoid IE memory leaks
            DetachEvent(link.id, "click", DefineAnchorClick);
            var result = true;
            if (link.onclick)
            {
                result = link.onclick();
            }
            if (typeof(result) == 'undefined' || result)
            {
                eval(link.href);
            }
        }
    }
}

function AttachEvent(objId, eventName, target)
{
    obj = $get(objId);
    if (!obj || !target)
    {
        return;
    }
    if (obj.addEventListener)
    {
        obj.addEventListener(eventName, target, false);
    }
    else if (obj.attachEvent)
    {
        obj.attachEvent("on" + eventName, target);
    }
}

function DetachEvent(objId, eventName, target)
{
    obj = $get(objId);
    if (!obj || !target)
    {
        return;
    }
	if (obj.removeEventListener)
	{
		obj.removeEventListener(eventName, target, false);
	}
	else if (obj.detachEvent)
	{
		obj.detachEvent("on" + eventName, target);
	}
}

function SortCollection(collectionContainer, attribToSortBy, sortDescending)
{
	var nodeCollection = collectionContainer.childNodes;

	var i = 0;
	var j = 0;
	var nodeArray = new Array();

	//only removing the nodes with the appropriate attribute for sort.
	while (i < nodeCollection.length)
	{
		if (nodeCollection.item(i).innerHTML && nodeCollection.item(i).attributes[attribToSortBy])
		{
			nodeArray[j] = collectionContainer.removeChild(nodeCollection.item(i));
			j++;
			i--;
		}
		
		i++;
	}

	//sort nodes
	nodeArray.sort(function(lhs, rhs)
		{
			if (lhs.attributes[attribToSortBy].value == rhs.attributes[attribToSortBy].value) return 0;
			if (sortDescending ^ (lhs.attributes[attribToSortBy].value.toUpperCase() < rhs.attributes[attribToSortBy].value.toUpperCase())) return -1;
			return 1;
		}
	);

	//put it all back in order.
	for(i=0; i<nodeArray.length; i++)
	{
		collectionContainer.appendChild(nodeArray[i]);
	}
}

function fixIECheckoutWidgetWidth(){
    //We had to remove the fixed width from the Checkout widget.
    //which is no problem in Firefox, Safari, Chrome et al.
    //But IE needs a little fixer upper to make sure the top and
    //bottom of the widget are the same width
    if(Browser.Engine.trident){
        var checkoutWidget = $('CheckoutWidget');
        if (checkoutWidget) {
            var checkout_wgt = checkoutWidget.getElement('div.Checkout');
            var cart_wgt = checkoutWidget.getElement('div.Cart');
            
            if(checkout_wgt){
                if(checkout_wgt.getStyle('width') < cart_wgt.getStyle('width'))
                        checkout_wgt.setStyle('width', cart_wgt.getStyle('width') );
                
                if(checkout_wgt.getStyle('width') > cart_wgt.getStyle('width')) 
                        cart_wgt.setStyle('width', checkout_wgt.getStyle('width') );
            }
         }
    }
}

window.addEvent('domready', function() {
    //We want to call this function before 'domready' or 'load'
    //So the user doesn't see the width mismatch
    fixIECheckoutWidgetWidth();
    //Sys.CultureInfo.CurrentCulture.numberFormat.CurrencySymbol = '';
    //new DropShadow('*.dropShadow', 'dropShadowBackground', 0, 0); 
});
window.addEvent('load',function(){
    //  To have 4 rounded corners: use 'rounded' class.
    //  To have top rounded corners only: use 'rounded4' class and 'internal="true"' attribute.
    //  To have bottom rounded corners only: use 'rounded4' class and 'isBottomOnly="true"' attribute.
    var RC4pxRad = $$('div.rounded4');
    if(RC4pxRad) new MooRC(RC4pxRad,{ radius: {x: 4, y: 4} });
    if($$('.rounded').length > 0) { Rounded('rounded', 4, 4); }
    
});

function pageLoad(sender, args) {
    //this has to be stored here, or a place when the whole page is ready, instead of DomReady
    if (Sys != null && Sys.CultureInfo != null) {
        Sys.CultureInfo.CurrentCulture.numberFormat.CurrencySymbol = '';
    }
    // this is just a temporary solution, we need to figure out how to make multiple pageLoad
    // calls to chain together and then move the following code into cart.aspx page instead 
    // of global area. 
    
//settings1 = {
//      tl: { radius: 10 },
//      tr: { radius: 10 },
//      bl: { radius: 10 },
//      br: { radius: 10 },
//      antiAlias: true,
//      autoPad: true
//    }
//settings2 = {
//      tl: { radius: 10 },
//      tr: { radius: 10 },
//      bl: { radius: 10 },
//      br: { radius: 10 },
//      antiAlias: true,
//      autoPad: true
//    }
//    //var divObj1 = $get('unpricedBox');
//    //var divObj2 = $get('pricedBox');
//    //var cornersObj1 = new curvyCorners(settings, divObj1);
//    //var cornersObj2 = new curvyCorners(settings, divObj2);

//    var cornersObj4 = new curvyCorners(settings1, "checkoutStage");
//    var cornersObj3 = new curvyCorners(settings2, "wrap");
//    
//    cornersObj4.applyCornersToAll();
//    //cornersObj1.applyCornersToAll();
//    //cornersObj2.applyCornersToAll();
//    cornersObj3.applyCornersToAll();
}

function CloseModalPopup(modalPopupName)
{
	var modalPopup = $find(modalPopupName); 
	if (modalPopup)
	{
		modalPopup.hide();
	}
}

function findChildByClass(item, className)
{
    // First check all immediate child items
    var child = item.firstChild;
    while( child != null )
    {
        if( child.className == className )
        {
            return child;
        }
        child = child.nextSibling;
    }
    
    // Not found, recursively check all child items
    child = item.firstChild;
    while( child != null )
    {
        var found = this._findChildByClass( child, className );
        if( found != null )
        {
            return found;
        }
        child = child.nextSibling;
    }
}

function ResizeIframe(frameid){
    var currentfr=parent.document.getElementById(frameid);
    if (currentfr && !window.opera){
        currentfr.style.display="block";
        if (currentfr.contentDocument && currentfr.contentDocument.body.offsetHeight) //ns6 syntax
            currentfr.height = currentfr.contentDocument.body.offsetHeight+FFextraHeight;
        else if (currentfr.Document && currentfr.Document.body.scrollHeight) //ie5+ syntax
            currentfr.height = currentfr.Document.body.scrollHeight;
    }
}


//   
//   Begin Script
//   Originally Build By: v-nicks 10/31/08
//   This code vertically centers two HTML elements within a page.
//    

function verticalMiddleGlobal(innerElement, outerElement){
    
        var elementToChange = outerElement;
        var innerElement = $(innerElement).getSize();
        var outerElement = $(outerElement).getSize();
        var deltaHeight = (innerElement.y - outerElement.y)/2;
       // alert(innerElement) 
        $(elementToChange).setStyle('margin-top', deltaHeight);                            
}
//   End Script
//   Originally Build By: Nick Stark 10/31/08
//   This code vertically centers two HTML elements within a page
//

function SetUniqueRadioButton(nameregex, current) {
//this is helper function to handle radio button in a repeater
    re = new RegExp(nameregex);
    radios = $$('input[type=radio]');
    radios.each(function(item) {
        item = $(item);
        if (re.test(item.name)) {
            item.checked = false;
        }
    });
//    for (i = 0; i < document.elements.length; i++) {
//        elm = document.forms[0].elements[i]
//        if (elm.type == 'radio') {
//            if (re.test(elm.name)) {
//                elm.checked = false;
//            }
//        }
//    }
    current.checked = true;
}

function HtmlDecode(s) 
{ 
      var out = ""; 
      if (s==null) return; 

      var l = s.length; 
      for (var i=0; i<l; i++) 
      { 
            var ch = s.charAt(i); 

            if (ch == '&') 
            { 
                var semicolonIndex = s.indexOf(';', i+1); 

                if (semicolonIndex > 0) 
                { 
                    var entity = s.substring(i + 1, semicolonIndex); 

                    if (entity.length > 1 && entity.charAt(0) == '#') 
                    { 
                        if (entity.charAt(1) == 'x' || entity.charAt(1) == 'X') 
                            ch = String.fromCharCode(eval('0'+entity.substring(1))); 
                        else 
                            ch = String.fromCharCode(eval(entity.substring(1))); 
                    } 
                    else 
                    { 
                        switch (entity) 
                        { 
                            case 'quot': ch = String.fromCharCode(0x0022); break; 
                            case 'amp': ch = String.fromCharCode(0x0026); break; 
                            case 'lt': ch = String.fromCharCode(0x003c); break; 
                            case 'gt': ch = String.fromCharCode(0x003e); break; 
                            case 'nbsp': ch = String.fromCharCode(0x00a0); break; 
                            case 'iexcl': ch = String.fromCharCode(0x00a1); break; 
                            case 'cent': ch = String.fromCharCode(0x00a2); break; 
                            case 'pound': ch = String.fromCharCode(0x00a3); break; 
                            case 'curren': ch = String.fromCharCode(0x00a4); break; 
                            case 'yen': ch = String.fromCharCode(0x00a5); break; 
                            case 'brvbar': ch = String.fromCharCode(0x00a6); break; 
                            case 'sect': ch = String.fromCharCode(0x00a7); break; 
                            case 'uml': ch = String.fromCharCode(0x00a8); break; 
                            case 'copy': ch = String.fromCharCode(0x00a9); break; 
                            case 'ordf': ch = String.fromCharCode(0x00aa); break; 
                            case 'laquo': ch = String.fromCharCode(0x00ab); break; 
                            case 'not': ch = String.fromCharCode(0x00ac); break; 
                            case 'shy': ch = String.fromCharCode(0x00ad); break; 
                            case 'reg': ch = String.fromCharCode(0x00ae); break; 
                            case 'macr': ch = String.fromCharCode(0x00af); break; 
                            case 'deg': ch = String.fromCharCode(0x00b0); break; 
                            case 'plusmn': ch = String.fromCharCode(0x00b1); break; 
                            case 'sup2': ch = String.fromCharCode(0x00b2); break; 
                            case 'sup3': ch = String.fromCharCode(0x00b3); break; 
                            case 'acute': ch = String.fromCharCode(0x00b4); break; 
                            case 'micro': ch = String.fromCharCode(0x00b5); break; 
                            case 'para': ch = String.fromCharCode(0x00b6); break; 
                            case 'middot': ch = String.fromCharCode(0x00b7); break; 
                            case 'cedil': ch = String.fromCharCode(0x00b8); break; 
                            case 'sup1': ch = String.fromCharCode(0x00b9); break; 
                            case 'ordm': ch = String.fromCharCode(0x00ba); break; 
                            case 'raquo': ch = String.fromCharCode(0x00bb); break; 
                            case 'frac14': ch = String.fromCharCode(0x00bc); break; 
                            case 'frac12': ch = String.fromCharCode(0x00bd); break; 
                            case 'frac34': ch = String.fromCharCode(0x00be); break; 
                            case 'iquest': ch = String.fromCharCode(0x00bf); break; 
                            case 'Agrave': ch = String.fromCharCode(0x00c0); break; 
                            case 'Aacute': ch = String.fromCharCode(0x00c1); break; 
                            case 'Acirc': ch = String.fromCharCode(0x00c2); break; 
                            case 'Atilde': ch = String.fromCharCode(0x00c3); break; 
                            case 'Auml': ch = String.fromCharCode(0x00c4); break;
                            case 'Aring': ch = String.fromCharCode(0x00c5); break;
                            case 'AElig': ch = String.fromCharCode(0x00c6); break; 
                            case 'Ccedil': ch = String.fromCharCode(0x00c7); break; 
                            case 'Egrave': ch = String.fromCharCode(0x00c8); break; 
                            case 'Eacute': ch = String.fromCharCode(0x00c9); break; 
                            case 'Ecirc': ch = String.fromCharCode(0x00ca); break; 
                            case 'Euml': ch = String.fromCharCode(0x00cb); break; 
                            case 'Igrave': ch = String.fromCharCode(0x00cc); break; 
                            case 'Iacute': ch = String.fromCharCode(0x00cd); break; 
                            case 'Icirc': ch = String.fromCharCode(0x00ce ); break; 
                            case 'Iuml': ch = String.fromCharCode(0x00cf); break; 
                            case 'ETH': ch = String.fromCharCode(0x00d0); break; 
                            case 'Ntilde': ch = String.fromCharCode(0x00d1); break; 
                            case 'Ograve': ch = String.fromCharCode(0x00d2); break; 
                            case 'Oacute': ch = String.fromCharCode(0x00d3); break; 
                            case 'Ocirc': ch = String.fromCharCode(0x00d4); break; 
                            case 'Otilde': ch = String.fromCharCode(0x00d5); break; 
                            case 'Ouml': ch = String.fromCharCode(0x00d6); break; 
                            case 'times': ch = String.fromCharCode(0x00d7); break; 
                            case 'Oslash': ch = String.fromCharCode(0x00d8); break; 
                            case 'Ugrave': ch = String.fromCharCode(0x00d9); break; 
                            case 'Uacute': ch = String.fromCharCode(0x00da); break; 
                            case 'Ucirc': ch = String.fromCharCode(0x00db); break; 
                            case 'Uuml': ch = String.fromCharCode(0x00dc); break; 
                            case 'Yacute': ch = String.fromCharCode(0x00dd); break; 
                            case 'THORN': ch = String.fromCharCode(0x00de); break; 
                            case 'szlig': ch = String.fromCharCode(0x00df); break; 
                            case 'agrave': ch = String.fromCharCode(0x00e0); break; 
                            case 'aacute': ch = String.fromCharCode(0x00e1); break; 
                            case 'acirc': ch = String.fromCharCode(0x00e2); break; 
                            case 'atilde': ch = String.fromCharCode(0x00e3); break; 
                            case 'auml': ch = String.fromCharCode(0x00e4); break; 
                            case 'aring': ch = String.fromCharCode(0x00e5); break; 
                            case 'aelig': ch = String.fromCharCode(0x00e6); break; 
                            case 'ccedil': ch = String.fromCharCode(0x00e7); break; 
                            case 'egrave': ch = String.fromCharCode(0x00e8); break; 
                            case 'eacute': ch = String.fromCharCode(0x00e9); break; 
                            case 'ecirc': ch = String.fromCharCode(0x00ea); break; 
                            case 'euml': ch = String.fromCharCode(0x00eb); break; 
                            case 'igrave': ch = String.fromCharCode(0x00ec); break; 
                            case 'iacute': ch = String.fromCharCode(0x00ed); break; 
                            case 'icirc': ch = String.fromCharCode(0x00ee); break; 
                            case 'iuml': ch = String.fromCharCode(0x00ef); break; 
                            case 'eth': ch = String.fromCharCode(0x00f0); break; 
                            case 'ntilde': ch = String.fromCharCode(0x00f1); break; 
                            case 'ograve': ch = String.fromCharCode(0x00f2); break; 
                            case 'oacute': ch = String.fromCharCode(0x00f3); break; 
                            case 'ocirc': ch = String.fromCharCode(0x00f4); break; 
                            case 'otilde': ch = String.fromCharCode(0x00f5); break; 
                            case 'ouml': ch = String.fromCharCode(0x00f6); break; 
                            case 'divide': ch = String.fromCharCode(0x00f7); break; 
                            case 'oslash': ch = String.fromCharCode(0x00f8); break; 
                            case 'ugrave': ch = String.fromCharCode(0x00f9); break; 
                            case 'uacute': ch = String.fromCharCode(0x00fa); break; 
                            case 'ucirc': ch = String.fromCharCode(0x00fb); break; 
                            case 'uuml': ch = String.fromCharCode(0x00fc); break; 
                            case 'yacute': ch = String.fromCharCode(0x00fd); break; 
                            case 'thorn': ch = String.fromCharCode(0x00fe); break; 
                            case 'yuml': ch = String.fromCharCode(0x00ff); break; 
                            case 'OElig': ch = String.fromCharCode(0x0152); break; 
                            case 'oelig': ch = String.fromCharCode(0x0153); break; 
                            case 'Scaron': ch = String.fromCharCode(0x0160); break; 
                            case 'scaron': ch = String.fromCharCode(0x0161); break; 
                            case 'Yuml': ch = String.fromCharCode(0x0178); break; 
                            case 'fnof': ch = String.fromCharCode(0x0192); break; 
                            case 'circ': ch = String.fromCharCode(0x02c6); break; 
                            case 'tilde': ch = String.fromCharCode(0x02dc); break; 
                            case 'Alpha': ch = String.fromCharCode(0x0391); break; 
                            case 'Beta': ch = String.fromCharCode(0x0392); break; 
                            case 'Gamma': ch = String.fromCharCode(0x0393); break; 
                            case 'Delta': ch = String.fromCharCode(0x0394); break; 
                            case 'Epsilon': ch = String.fromCharCode(0x0395); break; 
                            case 'Zeta': ch = String.fromCharCode(0x0396); break; 
                            case 'Eta': ch = String.fromCharCode(0x0397); break; 
                            case 'Theta': ch = String.fromCharCode(0x0398); break; 
                            case 'Iota': ch = String.fromCharCode(0x0399); break; 
                            case 'Kappa': ch = String.fromCharCode(0x039a); break; 
                            case 'Lambda': ch = String.fromCharCode(0x039b); break; 
                            case 'Mu': ch = String.fromCharCode(0x039c); break; 
                            case 'Nu': ch = String.fromCharCode(0x039d); break; 
                            case 'Xi': ch = String.fromCharCode(0x039e); break; 
                            case 'Omicron': ch = String.fromCharCode(0x039f); break; 
                            case 'Pi': ch = String.fromCharCode(0x03a0); break; 
                            case ' Rho ': ch = String.fromCharCode(0x03a1); break; 
                            case 'Sigma': ch = String.fromCharCode(0x03a3); break; 
                            case 'Tau': ch = String.fromCharCode(0x03a4); break; 
                            case 'Upsilon': ch = String.fromCharCode(0x03a5); break; 
                            case 'Phi': ch = String.fromCharCode(0x03a6); break; 
                            case 'Chi': ch = String.fromCharCode(0x03a7); break; 
                            case 'Psi': ch = String.fromCharCode(0x03a8); break; 
                            case 'Omega': ch = String.fromCharCode(0x03a9); break; 
                            case 'alpha': ch = String.fromCharCode(0x03b1); break; 
                            case 'beta': ch = String.fromCharCode(0x03b2); break; 
                            case 'gamma': ch = String.fromCharCode(0x03b3); break; 
                            case 'delta': ch = String.fromCharCode(0x03b4); break; 
                            case 'epsilon': ch = String.fromCharCode(0x03b5); break; 
                            case 'zeta': ch = String.fromCharCode(0x03b6); break; 
                            case 'eta': ch = String.fromCharCode(0x03b7); break; 
                            case 'theta': ch = String.fromCharCode(0x03b8); break; 
                            case 'iota': ch = String.fromCharCode(0x03b9); break; 
                            case 'kappa': ch = String.fromCharCode(0x03ba); break; 
                            case 'lambda': ch = String.fromCharCode(0x03bb); break; 
                            case 'mu': ch = String.fromCharCode(0x03bc); break; 
                            case 'nu': ch = String.fromCharCode(0x03bd); break; 
                            case 'xi': ch = String.fromCharCode(0x03be); break; 
                            case 'omicron': ch = String.fromCharCode(0x03bf); break; 
                            case 'pi': ch = String.fromCharCode(0x03c0); break; 
                            case 'rho': ch = String.fromCharCode(0x03c1); break; 
                            case 'sigmaf': ch = String.fromCharCode(0x03c2); break; 
                            case 'sigma': ch = String.fromCharCode(0x03c3); break; 
                            case 'tau': ch = String.fromCharCode(0x03c4); break; 
                            case 'upsilon': ch = String.fromCharCode(0x03c5); break; 
                            case 'phi': ch = String.fromCharCode(0x03c6); break; 
                            case 'chi': ch = String.fromCharCode(0x03c7); break; 
                            case 'psi': ch = String.fromCharCode(0x03c8); break; 
                            case 'omega': ch = String.fromCharCode(0x03c9); break; 
                            case 'thetasym': ch = String.fromCharCode(0x03d1); break; 
                            case 'upsih': ch = String.fromCharCode(0x03d2); break; 
                            case 'piv': ch = String.fromCharCode(0x03d6); break; 
                            case 'ensp': ch = String.fromCharCode(0x2002); break; 
                            case 'emsp': ch = String.fromCharCode(0x2003); break; 
                            case 'thinsp': ch = String.fromCharCode(0x2009); break; 
                            case 'zwnj': ch = String.fromCharCode(0x200c); break; 
                            case 'zwj': ch = String.fromCharCode(0x200d); break; 
                            case 'lrm': ch = String.fromCharCode(0x200e); break; 
                            case 'rlm': ch = String.fromCharCode(0x200f); break; 
                            case 'ndash': ch = String.fromCharCode(0x2013); break; 
                            case 'mdash': ch = String.fromCharCode(0x2014); break; 
                            case 'lsquo': ch = String.fromCharCode(0x2018); break; 
                            case 'rsquo': ch = String.fromCharCode(0x2019); break; 
                            case 'sbquo': ch = String.fromCharCode(0x201a); break; 
                            case 'ldquo': ch = String.fromCharCode(0x201c); break; 
                            case 'rdquo': ch = String.fromCharCode(0x201d); break; 
                            case 'bdquo': ch = String.fromCharCode(0x201e); break; 
                            case 'dagger': ch = String.fromCharCode(0x2020); break; 
                            case 'Dagger': ch = String.fromCharCode(0x2021); break; 
                            case 'bull': ch = String.fromCharCode(0x2022); break; 
                            case 'hellip': ch = String.fromCharCode(0x2026); break; 
                            case 'permil': ch = String.fromCharCode(0x2030); break; 
                            case 'prime': ch = String.fromCharCode(0x2032); break; 
                            case 'Prime': ch = String.fromCharCode(0x2033); break; 
                            case 'lsaquo': ch = String.fromCharCode(0x2039); break; 
                            case 'rsaquo': ch = String.fromCharCode(0x203a); break; 
                            case 'oline': ch = String.fromCharCode(0x203e); break; 
                            case 'frasl': ch = String.fromCharCode(0x2044); break; 
                            case 'euro': ch = String.fromCharCode(0x20ac); break; 
                            case 'image': ch = String.fromCharCode(0x2111); break; 
                            case 'weierp': ch = String.fromCharCode(0x2118); break; 
                            case 'real': ch = String.fromCharCode(0x211c); break; 
                            case 'trade': ch = String.fromCharCode(0x2122); break; 
                            case 'alefsym': ch = String.fromCharCode(0x2135); break; 
                            case 'larr': ch = String.fromCharCode(0x2190); break; 
                            case 'uarr': ch = String.fromCharCode(0x2191); break; 
                            case 'rarr': ch = String.fromCharCode(0x2192); break; 
                            case 'darr': ch = String.fromCharCode(0x2193); break; 
                            case 'harr': ch = String.fromCharCode(0x2194); break; 
                            case 'crarr': ch = String.fromCharCode(0x21b5); break; 
                            case 'lArr': ch = String.fromCharCode(0x21d0); break; 
                            case 'uArr': ch = String.fromCharCode(0x21d1); break; 
                            case 'rArr': ch = String.fromCharCode(0x21d2); break; 
                            case 'dArr': ch = String.fromCharCode(0x21d3); break; 
                            case 'hArr': ch = String.fromCharCode(0x21d4); break; 
                            case 'forall': ch = String.fromCharCode(0x2200); break; 
                            case 'part': ch = String.fromCharCode(0x2202); break; 
                            case 'exist': ch = String.fromCharCode(0x2203); break; 
                            case 'empty': ch = String.fromCharCode(0x2205); break; 
                            case 'nabla': ch = String.fromCharCode(0x2207); break; 
                            case 'isin': ch = String.fromCharCode(0x2208); break; 
                            case 'notin': ch = String.fromCharCode(0x2209); break; 
                            case 'ni': ch = String.fromCharCode(0x220b); break; 
                            case 'prod': ch = String.fromCharCode(0x220f); break; 
                            case 'sum': ch = String.fromCharCode(0x2211); break; 
                            case 'minus': ch = String.fromCharCode(0x2212); break; 
                            case 'lowast': ch = String.fromCharCode(0x2217); break; 
                            case 'radic': ch = String.fromCharCode(0x221a); break; 
                            case 'prop': ch = String.fromCharCode(0x221d); break; 
                            case 'infin': ch = String.fromCharCode(0x221e); break; 
                            case 'ang': ch = String.fromCharCode(0x2220); break; 
                            case 'and': ch = String.fromCharCode(0x2227); break; 
                            case 'or': ch = String.fromCharCode(0x2228); break; 
                            case 'cap': ch = String.fromCharCode(0x2229); break; 
                            case 'cup': ch = String.fromCharCode(0x222a); break; 
                            case 'int': ch = String.fromCharCode(0x222b); break; 
                            case 'there4': ch = String.fromCharCode(0x2234); break; 
                            case 'sim': ch = String.fromCharCode(0x223c); break; 
                            case 'cong': ch = String.fromCharCode(0x2245); break; 
                            case 'asymp': ch = String.fromCharCode(0x2248); break; 
                            case 'ne': ch = String.fromCharCode(0x2260); break; 
                            case 'equiv': ch = String.fromCharCode(0x2261); break; 
                            case 'le': ch = String.fromCharCode(0x2264); break; 
                            case 'ge': ch = String.fromCharCode(0x2265); break; 
                            case 'sub': ch = String.fromCharCode(0x2282); break; 
                            case 'sup': ch = String.fromCharCode(0x2283); break; 
                            case 'nsub': ch = String.fromCharCode(0x2284); break; 
                            case 'sube': ch = String.fromCharCode(0x2286); break; 
                            case 'supe': ch = String.fromCharCode(0x2287); break; 
                            case 'oplus': ch = String.fromCharCode(0x2295); break; 
                            case 'otimes': ch = String.fromCharCode(0x2297); break; 
                            case 'perp': ch = String.fromCharCode(0x22a5); break; 
                            case 'sdot': ch = String.fromCharCode(0x22c5); break; 
                            case 'lceil': ch = String.fromCharCode(0x2308); break; 
                            case 'rceil': ch = String.fromCharCode(0x2309); break; 
                            case 'lfloor': ch = String.fromCharCode(0x230a); break; 
                            case 'rfloor': ch = String.fromCharCode(0x230b); break; 
                            case 'lang': ch = String.fromCharCode(0x2329); break; 
                            case 'rang': ch = String.fromCharCode(0x232a); break; 
                            case 'loz': ch = String.fromCharCode(0x25ca); break; 
                            case 'spades': ch = String.fromCharCode(0x2660); break; 
                            case 'clubs': ch = String.fromCharCode(0x2663); break; 
                            case 'hearts': ch = String.fromCharCode(0x2665); break; 
                            case 'diams': ch = String.fromCharCode(0x2666); break; 
                            default: ch = ''; break; 
                    } 
                } 
                i = semicolonIndex; 
            } 
        } 

    out += ch; 
    } 

    return out;
}

function HtmlDecodeTextArea(s)
{
    return HtmlDecode(s).replace(/<br[//]?>/gi, '\n');
}

function HtmlEncode(str) {
	var div = document.createElement('div');
	var text = document.createTextNode(str);
	
	div.appendChild(text);
	//IE7 doesn't encode spaces, encoding here for consistency
	return div.innerHTML.replace(/ /g,'&nbsp;');
}

function HtmlFieldEncode(str) {
	return str.replace(/&/g, '&amp;').replace(/>/g, '&gt;').replace(/</g, '&lt;').replace(/"/g, '&quot;');
}

function HtmlEncodeTextArea(str) {
    return HtmlFieldEncode(str).replace(/[\r\n]+/g, '<br/>');
}

function UrlEncode(str) {
    return encodeURI(str.replace(/\s+/g, "+"));
}

//Pricing presenter needs this function as is
function PriceImage(source, modalWidth, modalHeight) {
    if (source.toString().indexOf('RMPricing') > -1) {
        if(CorbisUI.Auth.GetSignInLevel() < 1){
            //var actionArg = "OpenIModal('" + source + "', " + modalWidth + ", " + modalHeight + ")";
            var actionArg = "PriceImage_MAGIC('" + source + "', " + modalWidth + ", " + modalHeight + ")";
            //CorbisUI.Auth.Check(1, CorbisUI.Auth.ActionTypes.Execute, actionArg);
            CorbisUI.Auth.OpenSSLSignIn(CorbisUI.Auth.ActionTypes.Execute, actionArg);
        }else{
            OpenIModal(source, modalWidth, modalHeight);
        }
        
    } else {
        OpenIModal(source, modalWidth, modalHeight);
    }
}

function PriceImage_MAGIC(source, modalWidth, modalHeight){
    var temp = JSON.decode("{\"tempFunc\":function(){OpenIModal('" + source + "', " + modalWidth + ", " + modalHeight + ");}}");
    //var cookieItem = temp.tempFunc
    //CorbisUI.CookieEvents.addCookieEvent_altPath('/Search',temp.tempFunc);
    CorbisUI.CookieEvents.addCookieEvent(temp.tempFunc);
    window.location = window.location;
}

// Add to lightbox functionality
function showNewLightboxDiv() {
    $(CorbisUI.GlobalVars.AddToLightbox.createLightboxSection).removeClass('displayNone');
    ResizeModal('addToLightboxModalPopup');
}
function ResetNewOrAddDivs() {
    $(CorbisUI.GlobalVars.AddToLightbox.createLightboxSection).addClass('displayNone');
    if ($(CorbisUI.GlobalVars.AddToLightbox.addToLightboxSection)) $(CorbisUI.GlobalVars.AddToLightbox.addToLightboxSection).removeClass('displayNone');
    ResizeModal('addToLightboxModalPopup');
}

function ShowAddAllItemsToLightboxModal(offeringUid, ele, showError,addAll) {
    $(CorbisUI.GlobalVars.AddToLightbox.addAllItemsToLightboxHiddenName).value = addAll;
    
    ShowAddToLightboxModal(offeringUid, ele, showError);
}
function ReloadPageWithQueryParams(paramstr) {
    if(paramstr != null ){
        if(window.location.pathname.contains('RFPricing'))
        {
             window.location = window.location + '&' + paramstr +'&hidattributeValue=' + hidAttributeValueUID.value;
        }else if(window.location.pathname.contains('Enlargement'))
        {
            refreshEnlargementPage('AddToLightbox');
        }else
        {   
           window.location = window.location; 
        }
       
    }
}
function ShowAddToLightboxModal(offeringUid, ele, showError) {
    if (CorbisUI.Auth.GetSignInLevel() < 1) {
        CorbisUI.CookieEvents.addCookieEvent(function() { DoAddToLightboxModal(this.vars.OID, null, this.vars.SE) }, { OID: offeringUid, SE: showError } );
        CorbisUI.Auth.Check(1);
    } else {
        DoAddToLightboxModal(offeringUid, ele, showError);
    }
}

function DoAddToLightboxModal(offeringUid, ele, showError) {
    if (CorbisUI.Auth.GetSignInLevel() < 1) { return; }
    
    $(CorbisUI.GlobalVars.AddToLightbox.commandButtonName).value = "";
    //no button means no lightbox, so only add lightbox option, so we display it.
    if ($(CorbisUI.GlobalVars.AddToLightbox.createLightboxButtonName) && !showError) {
        $(CorbisUI.GlobalVars.AddToLightbox.createLightboxSection).addClass('displayNone');
        $(CorbisUI.GlobalVars.AddToLightbox.lightboxName).value = '';

    }
    else {
        $(CorbisUI.GlobalVars.AddToLightbox.createLightboxSection).removeClass('displayNone');
    }
    $(CorbisUI.GlobalVars.AddToLightbox.offeringUidHiddenName).value = offeringUid;
    $(CorbisUI.GlobalVars.AddToLightbox.addToNewLightboxSummary).removeClass('displayNone');

    if (showError) {
        showNewLightboxDiv();
    } else {
        $(CorbisUI.GlobalVars.AddToLightbox.addToNewLightboxSummary).addClass('displayNone');
        OpenModal('addToLightboxModalPopup');
        ResizeModal('addToLightboxModalPopup');
        /* For repositioning the Lightbox widget on top of the AddToLightBox icon */
        if(ele != null && typeof(ele) != 'boolean' ){
            var position = $(ele).getCoordinates();
            var dimensions = $('addToLightboxModalPopupWindow').getCoordinates();
	        $('addToLightboxModalPopupWindow').setStyles({
	            'top': position.top - (dimensions.height/2 - 15),
                'left': position.left -(dimensions.width/2 + 80)
	        });
	    }
    }
}

function RefreshOpener() {
    try {
        window.opener.location = window.opener.location;
    }
    catch (e) {
    }
}

function RefreshOpenerAndSelf() {
    RefreshOpener();
    window.location = window.location;
}

function GetDocumentHeight() {
    var docHeight;
    if (document.all) {
        docHeight = document.body.scrollHeight + 5;
    } else {
        docHeight = document.body.offsetHeight + 16;  //+ FFextraHeight;
    }
    return docHeight;
}

//Extends windows.location to included method for returning a query string parameter.
//If parameter does not exist return empty string.
var queryString =
{
    queryStringParam: function(param) {
        return $pick(window.location.search.match(new RegExp('(' + param + '=)([^?&]*)', 'i')), new Array('', '', ''))[2];
    }
}

$extend(window.location, queryString);


//double click solution
var currentTimeStamp = 0;
var curenntClickedTarget = '';
var BLOCKPERIOD = 1; //block one second
function allowClickEvent(newTarget) {
    /// newTarget: should be an unique id, such as the element id, product uid, etc
    /// this can be a generic solution for all click event
    if (newTarget != curenntClickedTarget) {
        currentTimeStamp = new Date();
        curenntClickedTarget = newTarget;
        return true;
    }
    newTimeStamp = new Date();
    
    if ((newTimeStamp - currentTimeStamp) / 1000 > BLOCKPERIOD) {
        currentTimeStamp = newTimeStamp;
        curenntClickedTarget = newTarget;
        //console.log('allowed click at ' + newTarget);
        return true;
    }
    //console.log('blocked click at ' + newTarget);
    return false;
}
//end double click solution
 
 function parseXMLDoc(xmlString) {
        var xmlDoc;
        try //Internet Explorer
        {
            xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
            xmlDoc.async = "false";
            xmlDoc.loadXML(xmlString);
        }
        catch (e) {
                try //Firefox, Mozilla, Opera, etc.
                {
                    parser = new DOMParser();
                    xmlDoc = parser.parseFromString(xmlString, "text/xml");
                }
                catch (e) {}
            }
       return xmlDoc;
    }

CorbisUI.Pager = {
	pageNumberChanged: function(element) {
		if (element.value != '' && !isNaN(element.value) && doPost) {
			textChanged = true;
			
			//reset other pager control, otherwise the first changed control would take precedence.
			$$('div[id=Pager]').each(function(el) {
				var pageNumber = el.getElement('input[type=text]');
				if (pageNumber != element) {
					pageNumber.value = el.getElement('input.origPageNumber').value;
				}
			});
			
			setTimeout('__doPostBack(\'' + element.name + '\',\'\')', 0);
		}; 
		doPost = false;
	},

	keyCheck: function(event, element) {
		doPost = false;
        if (textChanged) return false;

        //return key checks
        if (event.keyCode == 13) {

            //bypass cases where we don't want to post, like if value is empty or hasn't changed
            if (element.value == '' || element.value == $('Pager').getElement('input.origPageNumber').value) {
                return false;
            }

            //setting variable so only return key can trigger onchange, and not lost focus events
            doPost = true;
            
            //skip event post for safari, by skipping WebForm_TextBoxKeyHandler, as it does it by default
            if (Browser.Engine.webkit) return true;
        }
        //allow numbers and control characters only for fireefox
        else if (Browser.Engine.gecko) {
			if ((event.charCode < 48 || event.charCode > 57) && (event.keyCode == 0) && !event.ctrlKey && !event.altKey)
					return false;
        }
		//allow numbers and control characters only for others
        else if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode > 31 && event.keyCode != 127 && !event.ctrlKey && !event.altKey) return false;

        return (WebForm_TextBoxKeyHandler(event));
    }
};


 window.addEvent('load',function(){
        try{
       if(typeof(AjaxControlToolkit) != 'undefined' && typeof(AjaxControlToolkit.CalendarBehavior) != 'undefined' && typeof(AjaxControlToolkit.CalendarBehavior.prototype)!='undefined'
            && AjaxControlToolkit && AjaxControlToolkit.CalendarBehavior && AjaxControlToolkit.CalendarBehavior.prototype)
       {
           
         AjaxControlToolkit.CalendarBehavior.prototype._performLayout = function() 
         {
            var elt = this.get_element();if (!elt) return;if (!this.get_isInitialized()) return;if (!this._isOpen) return;var dtf = Sys.CultureInfo.CurrentCulture.dateTimeFormat;var selectedDate = this.get_selectedDate();var visibleDate = this._getEffectiveVisibleDate();var todaysDate = this.get_todaysDate();
            switch (this._mode) {

            case "days":
            var firstDayOfWeek = this._getFirstDayOfWeek();var daysToBacktrack = visibleDate.getDay() - firstDayOfWeek;if (daysToBacktrack <= 0)
            daysToBacktrack += 7;var startDate = new Date(visibleDate.getFullYear(), visibleDate.getMonth(), visibleDate.getDate() - daysToBacktrack, this._hourOffsetForDst);var currentDate = startDate;for (var i = 0;i < 7;i++) {
            var dayCell = this._daysTableHeaderRow.cells[i].firstChild;if (dayCell.firstChild) {
            dayCell.removeChild(dayCell.firstChild);}
            dayCell.appendChild(document.createTextNode(dtf.ShortestDayNames[(i + firstDayOfWeek) % 7]));}
            for (var week = 0;week < 6;week ++) {
            var weekRow = this._daysBody.rows[week];for (var dayOfWeek = 0;dayOfWeek < 7;dayOfWeek++) {
            var dayCell = weekRow.cells[dayOfWeek].firstChild;if (dayCell.firstChild) {
            dayCell.removeChild(dayCell.firstChild);}
            dayCell.appendChild(document.createTextNode(currentDate.getDate()));dayCell.title = currentDate.localeFormat("D");dayCell.date = currentDate;$common.removeCssClasses(dayCell.parentNode, [ "ajax__calendar_other", "ajax__calendar_active" ]);Sys.UI.DomElement.addCssClass(dayCell.parentNode, this._getCssClass(dayCell.date, 'd'));currentDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate() + 1, this._hourOffsetForDst);}
            }
            this._prevArrow.date = new Date(visibleDate.getFullYear(), visibleDate.getMonth() - 1, 1, this._hourOffsetForDst);this._nextArrow.date = new Date(visibleDate.getFullYear(), visibleDate.getMonth() + 1, 1, this._hourOffsetForDst);if (this._title.firstChild) {
            this._title.removeChild(this._title.firstChild);}
            this._title.appendChild(document.createTextNode(visibleDate.localeFormat("y")));this._title.date = visibleDate;
            break;

            case "months":
            for (var i = 0;i < this._monthsBody.rows.length;i++) {
            var row = this._monthsBody.rows[i];for (var j = 0;j < row.cells.length;j++) {
            var cell = row.cells[j].firstChild;cell.date = new Date(visibleDate.getFullYear(), cell.month, 1, this._hourOffsetForDst);cell.title = cell.date.localeFormat("Y");$common.removeCssClasses(cell.parentNode, [ "ajax__calendar_other", "ajax__calendar_active" ]);Sys.UI.DomElement.addCssClass(cell.parentNode, this._getCssClass(cell.date, 'M'));}
            }
            if (this._title.firstChild) {
            this._title.removeChild(this._title.firstChild);}
            this._title.appendChild(document.createTextNode(visibleDate.localeFormat("yyyy")));this._title.date = visibleDate;this._prevArrow.date = new Date(visibleDate.getFullYear() - 1, 0, 1, this._hourOffsetForDst);this._nextArrow.date = new Date(visibleDate.getFullYear() + 1, 0, 1, this._hourOffsetForDst);
            break;

            case "years":
            var minYear = (Math.floor(visibleDate.getFullYear() / 10) * 10);for (var i = 0;i < this._yearsBody.rows.length;i++) {
            var row = this._yearsBody.rows[i];for (var j = 0;j < row.cells.length;j++) {
            var cell = row.cells[j].firstChild;cell.date = new Date(minYear + cell.year, 0, 1, this._hourOffsetForDst);if (cell.firstChild) {
            cell.removeChild(cell.lastChild);} else {
            cell.appendChild(document.createElement("br"));}
            cell.appendChild(document.createTextNode(minYear + cell.year));$common.removeCssClasses(cell.parentNode, [ "ajax__calendar_other", "ajax__calendar_active" ]);Sys.UI.DomElement.addCssClass(cell.parentNode, this._getCssClass(cell.date, 'y'));}
            }
            if (this._title.firstChild) {
            this._title.removeChild(this._title.firstChild);}
            this._title.appendChild(document.createTextNode(minYear.toString() + "-" + (minYear + 9).toString()));this._title.date = visibleDate;this._prevArrow.date = new Date(minYear - 10, 0, 1, this._hourOffsetForDst);this._nextArrow.date = new Date(minYear + 10, 0, 1, this._hourOffsetForDst);
            break;
            }
            if (this._today.firstChild) {
            this._today.removeChild(this._today.firstChild);}
            this._today.appendChild(document.createTextNode(String.format(localizedWordToday+" : {0}", new Date().localeFormat("d"))));this._today.date = todaysDate;
           }
         }
        }catch(e)
        {}
       });
