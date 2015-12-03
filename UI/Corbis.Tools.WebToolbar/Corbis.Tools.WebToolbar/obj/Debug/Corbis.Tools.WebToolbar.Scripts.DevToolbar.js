
var http_request = false;
function makePOSTRequest(url, parameters, callback) {
  http_request = false;
  if (window.XMLHttpRequest) { // Mozilla, Safari,...
     http_request = new XMLHttpRequest();
     if (http_request.overrideMimeType) {
         // set type accordingly to anticipated content type
        //http_request.overrideMimeType('text/xml');
        http_request.overrideMimeType('text/html');
     }
  } else if (window.ActiveXObject) { // IE
     try {
        http_request = new ActiveXObject("Msxml2.XMLHTTP");
     } catch (e) {
        try {
           http_request = new ActiveXObject("Microsoft.XMLHTTP");
        } catch (e) {}
     }
  }
  if (!http_request) {
     alert('Cannot create XMLHTTP instance');
     return false;
  }
  
  http_request.onreadystatechange = callback;
  http_request.open('POST', url, true);
  http_request.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
  http_request.setRequestHeader("Content-length", parameters.length);
  http_request.setRequestHeader("Connection", "close");
  http_request.send(parameters);
}

function getViewStateSize(url) 
{
    var poststr = "ViewState=" + document.getElementById('__VIEWSTATE').value;
    makePOSTRequest(url, poststr, getViewStateSize_Callback);
}

function getTotalPageSize()
{
    makePOSTRequest(location.href, '', getTotalPageSize_Callback);    
}

function getInfo(url)
{
    makePOSTRequest(url, '', getInfo_Callback);
}

function getViewStateSize_Callback() 
{
  if (http_request.readyState == 4) 
  {
     if (http_request.status == 200) 
     {
        result = http_request.responseText;
        document.getElementById('lcViewStateResult').innerHTML = getSizeDisplayText(result);            
     } 
     else 
     {
        alert('There was a problem with the request.');
     }
  }
}

function getTotalPageSize_Callback()
{
    if (http_request.readyState == 4) 
    {
         if (http_request.status == 200) 
         {
            result = http_request.getResponseHeader("Content-Length");
            document.getElementById('lcTotalPageSizeResult').innerHTML = getSizeDisplayText(result);            
         } 
         else 
         {
            alert('There was a problem with the request.');
         }
    }      
}

function getInfo_Callback()
{
    if (http_request.readyState == 4) 
    {
         if (http_request.status == 200) 
         {
            result = http_request.responseText;
            
            var divDiagnostics = document.getElementById('diagnosticsInfo');
            
            divDiagnostics.display = 'block';
            divDiagnostics.innerHTML = result;            
         } 
         else 
         {
            alert('There was a problem with the request.');
         }
    }  
}

function getSizeDisplayText(sizeInBytes)
{
    var resultString = '';

    //get mega bytes number
    var mega = sizeInBytes / 1000000;
    
    //re-evaluate the size depending on residue
    sizeInBytes = sizeInBytes % 1000000;
    
    //get kilo bytes number
    var kilo = sizeInBytes / 1000;
    
    if(mega >= 1)
    {
        resultString = "(" + mega.toFixed(2) + " MB)";
    }
    else
    {
        resultString = "(" + kilo.toFixed(2) + " KB)";
    }
    
    return resultString;
}