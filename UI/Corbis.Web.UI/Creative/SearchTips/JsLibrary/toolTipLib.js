var toolTipLib = { 
	xCord : 0,
	yCord : 0,
	attachToolTipBehavior: function() {
	var links = document.getElementsByTagName('a');
	var i;
	if (typeof(http) != "undefined" && http) {
		for ( i=0;i<links.length;i++ ) {
			if (links[i].id.indexOf('_iP_Img') > -1) {
				// Create a new div containing the link to show enlargement
				var imageNode = links[i].getElementsByTagName('img');
				if (imageNode && imageNode.length > 0) {
					var scrX = Number(findPosX(imageNode[0]));
					var scrY = Number(findPosY(imageNode[0]));
					var tp = parseInt(scrY);
					var lt = parseInt(scrX-10);
					// get the media uid
					var div = document.getElementById('expandLink' + i);
					if ( div ) {
						div.parentNode.removeChild(div);
					}
					var newDiv = document.createElement('div');
					newDiv.id = 'expandLink' + i;
					newDiv.style.position = "absolute";
					newDiv.style.top = tp+'px'; newDiv.style.left = lt+'px';
					document.body.appendChild(newDiv);
				
					newDiv = document.getElementById('expandLink' + i);
					newDiv.innerHTML = '<a id="link' + i + '" href="' + links[i].href + '" style="text-decoration:none">+</a>';
				
					newDiv = document.getElementById('link' + i);
				
					addEvent(newDiv,'mouseover',toolTipLib.tipOver,false);
					addEvent(newDiv,'mouseout',toolTipLib.tipOut,false);
				}
				
			}
		}
	}
	},
	tipOver: function(e) {
	obj = getEventSrc(e);
	toolTipLib.xCord = findPosX(obj);
	toolTipLib.yCord = findPosY(obj);
	tID = setTimeout("toolTipLib.tipShow(obj,'"+toolTipLib.xCord+"','"+toolTipLib.yCord+"')",500)
	},
	tipOut: function(e) {
		if ( window.tID )
		clearTimeout(tID);
		if ( window.opacityID )
		clearTimeout(opacityID);
		var l = getEventSrc(e);
		var div = document.getElementById('toolTip');
		if ( div ) {
		div.parentNode.removeChild(div);
		}
	},
	checkNode : function(obj) {
	var trueLink = obj;
		if ( trueLink.nodeName.toLowerCase() == 'a' ) {
		return trueLink;
		}
		while ( trueLink.nodeName.toLowerCase() != 'a' && trueLink.nodeName.toLowerCase() != 'body' )
		trueLink = trueLink.parentNode;
	return trueLink;
	},
	tipShow: function(obj,x,y) {
	
	var newDiv = document.createElement('div');
	var scrX = Number(x);
	var scrY = Number(y);
	var yOffset = -50;
	var tp = parseInt(scrY + yOffset);
		
	var xOffset = 20;
	if (scrX > 640) {
		xOffset = -420;		
	}
	var lt = parseInt(scrX + xOffset);
	var anch = toolTipLib.checkNode(obj);

	// new code
	var mediaUid = anch.href.substring(anch.href.indexOf("mediauids="));
	mediaUid = mediaUid.replace("mediauids=", "");
	mediaUid = mediaUid.substring(0, mediaUid.indexOf("}")+1);

	
	newDiv.id = 'toolTip';
	newDiv.style.top = tp+'px'; newDiv.style.left = lt+'px';
	document.body.appendChild(newDiv);	
	http.open("GET", "/search/ImageDetails.aspx?uid=" + escape(mediaUid), true); 
	http.onreadystatechange = handleHttpResponse; 
	http.send(null);
	newDiv.style.opacity = '.1';
	toolTipLib.tipFade('toolTip',10);
	},
	tipFade: function(div,opac) {
	var divobj = document.getElementById(div);
	var passed = parseInt(opac);
	var newOpac = parseInt(passed+10);
		if ( newOpac < 99 ) {
		divobj.style.opacity = '.'+newOpac;
		divobj.style.filter = "alpha(opacity:"+newOpac+")";
		opacityID = setTimeout("toolTipLib.tipFade('toolTip','"+newOpac+"')",60);
		}
		else { 
		divobj.style.opacity = '.99';
		divobj.style.filter = "alpha(opacity:100)";
		}
	}
};

function screenHeight() {
  myHeight = 0;
  if( typeof( window.innerHeight ) == 'number' ) {
    //Non-IE
    myHeight = window.innerHeight;
  } else if( document.documentElement &&
      ( document.documentElement.clientWidth || document.documentElement.clientHeight ) ) {
    //IE 6+ in 'standards compliant mode'
    myHeight = document.documentElement.clientHeight;
  } else if( document.body && ( document.body.clientHeight ) ) {
    //IE 4 compatible
    myHeight = document.body.clientHeight;
  }
  return myHeight;
}

function scrolledDown() {
  var scrOfY = 0;
  if( typeof( window.pageYOffset ) == 'number' ) {
    //Netscape compliant
    scrOfY = window.pageYOffset;
  } else if( document.body && ( document.body.scrollTop ) ) {
    //DOM compliant
    scrOfY = document.body.scrollTop;
  } else if( document.documentElement &&
      ( document.documentElement.scrollTop ) ) {
    //IE6 standards compliant mode
    scrOfY = document.documentElement.scrollTop;
  }
  return scrOfY;
}

addEvent(window,'load',toolTipLib.attachToolTipBehavior,false);
addEvent(window,'resize',toolTipLib.attachToolTipBehavior,false);