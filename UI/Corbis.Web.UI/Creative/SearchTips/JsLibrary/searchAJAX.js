function getHTTPObject() {
  var xmlhttp;
  /*@cc_on
  @if (@_jscript_version >= 5)
    try {
      xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
    } catch (e) {
      try {
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
      } catch (E) {
        xmlhttp = false;
      }
    }
  @else
  xmlhttp = false;
  @end @*/
  if (!xmlhttp && typeof(XMLHttpRequest) != 'undefined') {
    try {
      xmlhttp = new XMLHttpRequest();
    } catch (e) {
      xmlhttp = false;
    }
  }
  return xmlhttp;
}

var http = getHTTPObject(); // We create the HTTP Object

function handleHttpResponse() {
	if (http.readyState == 4) {
		var toolDiv = document.getElementById("toolTip");
		// does our tooltip div still exist?
		if (toolDiv) {
			toolDiv.innerHTML += http.responseText;
			imageNodes = toolDiv.getElementsByTagName('img');
			// only one image
			if ( (findPosY(toolDiv) + imageNodes[0].height + 20) > (scrolledDown() + screenHeight()) )
			{
				yPos = (scrolledDown() + screenHeight() - imageNodes[0].height) - 50;
				toolDiv.style.top = yPos+'px';
			} else {
				if (findPosY(toolDiv) < scrolledDown())
				{
					yPos = scrolledDown() + 20;
					toolDiv.style.top = yPos+'px';
				}
			}
		}
	}
}