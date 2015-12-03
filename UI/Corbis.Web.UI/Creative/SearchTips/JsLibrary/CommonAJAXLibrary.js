//Common values for the ReadyState of the XMLHttpRequest object
var READYSTATE_UNINITIALIZED = 0;
var READYSTATE_LOADING = 1;
var READYSTATE_LOADED = 2;
var READYSTATE_INTERACTIVE = 3;
var READYSTATE_COMPLETE = 4;

//create HttpRequest oject
function CreateXmlHttpRequestObject()
{	
	if(window.XMLHttpRequest)
	{
		xmlHttpObj = new XMLHttpRequest();
	}
	else
	{
		try
		{
			xmlHttpObj = new ActiveXObject("Microsoft.XMLHTTP");
		}
		catch(e)
		{
			xmlHttpObj = new ActiveXObject("Msxml2.XMLHTTP");
		}
	}
	return xmlHttpObj;
}
