var iDelimiter = errorMessages[0]
var daysInMonth = new makeArray(12);
var bCheckSearchTextBox = true;
var bOtherCollectionsChecked = false;
var bReset = false;
daysInMonth[1] = 31;
daysInMonth[2] = 29;
daysInMonth[3] = 31;
daysInMonth[4] = 30;
daysInMonth[5] = 31;
daysInMonth[6] = 30;
daysInMonth[7] = 31;
daysInMonth[8] = 31;
daysInMonth[9] = 30;
daysInMonth[10] = 31;
daysInMonth[11] = 30;
daysInMonth[12] = 31;

if (!CookieDomain) {
	var CookieDomain = window.location.host;
}

function fnChangeLanguage()
{	
	if (self != top) {
		var sURL
		sURL = window.top.location.href;
		document.frmChangeLanguage.action = sURL;	
	}
	document.frmChangeLanguage.submit();
	
}
// JScript source code
var bAdvancedSearchOn = false;

function searchClick()
{
	
	if(search())
	{ 
		
		document.frmSearch.submit()
	}
}

function resetFilters()
{
	var frm = document.frmSearch;
	
	for (var i =0;i < frm.elements.length;i++)
	{
	
		switch(frm.elements[i].type)		
		{
			case "checkbox":
				if((frm.elements[i].name == "mdl") || (frm.elements[i].name == "chkSearchIn"))
				{
					 frm.elements[i].checked = false;
				}
				else
				{
					 frm.elements[i].checked = true;					
				}
				break;
			case "text":
				
				frm.elements[i].value = "";
				break;
			
		}
	
	}
	
	resetAdvancedSearchFilters();
	
	
}

function resetAdvancedSearchFilters()
{
	var frm = document.frmAdvancedSearch;
	
	for (var i =0;i < frm.elements.length;i++)
	{
	
		switch(frm.elements[i].type)		
		{
			case "checkbox":			
				frm.elements[i].checked = true;				
				break;
			case "text":
				if((frm.elements[i].name == "dt1") || (frm.elements[i].name == "dt2"))
				{
					frm.elements[i].value = errorMessages[1]
				}
				else
				{
					frm.elements[i].value = "";
				}
				
				break;
			case "textarea":
				frm.elements[i].value = "";
				break;
			case "select-one":
				frm.elements[i].selectedIndex = 0;
				break;
			case "radio":
				
				if(frm.elements[i].value == "age")
				{
					frm.elements[i].checked = true;		
				}
				break;
		
		}
		
		frm.chkLanguage.checked = false;
	
	}
}

function search()
{
	var frm = document.frmSearch;
	
	if(bAdvancedSearchOn)
	{
		frm.as.value = "True";
		if(!advSearchPopulated())
		{			
			if(frm.txt.value == "")
			{
				alert("Please enter a keyword");
				return false;
			}
			
			
			if(!checkBasicSearch()) return false;
			
		   return true;	
		   	    
		}
		
	}
	else
	{
		frm.as.value = ""
		
		if(document.getElementById('radNDS') != null)
		{		
			if(!CurrentEventsSearchOk())
			{
				alert("Please enter a keyword");
				return false;
			}
			else
			{
				return true;
			}
				
		}
				
		if(frm.txt.value == "")
		{
			alert("Please enter a keyword");
			return false;
		}
		
		if(!checkBasicSearch()) return false;
	}	
	
	return true;
		

}

function checkBasicSearch()
{
	var frm = document.frmSearch;
	var iClrCount = 0
		var iClrCheckedCount = 0
	    
		var iMtpCount = 0;
		var iMtpCheckedCount = 0
	    if(frm.selSearchIn)
	    {
			if(frm.chkSearchIn2)
			{
					
				if(frm.chkSearchIn2.checked)
				{
					
					if(frm.selSearchIn.options[frm.selSearchIn.selectedIndex].value != "results")
					{					
						
						frm.cat.value = frm.selSearchIn.options[frm.selSearchIn.selectedIndex].value;
						frm.chkSearchIn.value = "Category";
						
					}
					else
					{
						
						frm.chkSearchIn2.value = "True";
					}
					
				}
				else if(frm.chkSearchIn.checked)
				{
					
					frm.chkSearchIn.value = "True";
				}
				else
				{
					
					frm.chkSearchIn2.value = "False";
					
				}
				
			}
	    }
		for(var i = 0; i < frm.elements.length;i++)
		{   
	        
			if(!frm.chkSearchAllCollections)
			{
			//Check Collections (sygma roles only)
			if(frm.elements[i].name.indexOf('chkColl') != -1)
			{	     
				if(frm.elements[i].checked)
				{
					frm.Collections.value = frm.Collections.value + frm.elements[i].value + ","
		           
				}	    
		        
		        
			}
			}
		    
			
		    
			
			if(frm.elements[i].name.indexOf('chkColorFormat') != -1)
			{	     
				if(frm.elements[i].checked)
				{
					frm.clr.value = frm.clr.value + frm.elements[i].value + ","
        			iClrCount++
				}	
		        
				iClrCheckedCount++
			}    	     
		    
		    
			//Check Media Type
			if(frm.elements[i].name.indexOf('chkMediaType') != -1)
			{	     
				if(frm.elements[i].checked)
				{
					frm.mtp.value = frm.mtp.value + frm.elements[i].value + ","
        			iMtpCount++
				}	
		        
				iMtpCheckedCount++
			}    	 
	    	
		}//end for 
	    
	
	
		   
		if(frm.clr)
		{
			if(iClrCount == iClrCheckedCount) frm.clr.value = '';
		}
		   
			if(frm.mtp)
		{
			//If all checkboxes are checked then
			if(iMtpCount == iMtpCheckedCount) frm.mtp.value = '';
		}
		    
		   
		if(frm.chkRP)
		{
			if ((!frm.chkRP.checked) && (!frm.chkRF.checked)) 
			{
				alert (errorMessages[2]);
				return false;
			}
		}
		
		
		return true;
}

function CurrentEventsSearchOk()
{

	if(document.getElementById('photodt').value != '')
	{
		return true;	
	}
	
	
	if(document.getElementById('location').value != '')
	{
		return true;	
	}
	
	if(document.getElementById('src').value != '')
	{
		return true;	
	}
		
	if(document.getElementById('radNDS').checked)
	{
		if(document.getElementById('txtNDS').value != '')
		{
			return true;	
		}
	}
	if(document.getElementById('radBetween').checked)
	{
	
	}
	
	if(document.getElementById('txt').value != "")
	{		
		return true;
	}
	
	
	return false;
}


function advSearchPopulated()
{
	//if advanced search fields are populated return true
   //so that the search can be executed
    var iOriCount = 0
	var iOriCheckedCount = 0
		
	var frm = document.frmAdvancedSearch
	var basicFrm = document.frmSearch
	
	if (basicFrm.hidCollection.value == 'outline')
	{
	    var bOutline = true
	}
	else
	{
	    var bOutline = false
	}
	
	
	
	var iCollectionCount = 0
    var iCollectionCheckedCount = 0
    var iPOVCount = 0
    var iPOVCheckedCount = 0
	var iFileSizeCount = 0
    var iFileSizeCheckedCount = 0    	
	var iCatCount = 0
	var iCatCheckedCount = 0
  
    if(frm.typ) basicFrm.typ.value = frm.typ.value;
   
	//checkboxes
	for(var i = 0; i < frm.elements.length;i++)
	{   
	
	    if(frm.chkSearchAllCollections)  //sygma only searches don't have this checkbox
	    {	    
	        //Check Collections
	        if(frm.elements[i].name.indexOf('chkColl') != -1)
	        {	 
				if(frm.elements[i].value != "other")
	            { 
					if(frm.elements[i].checked)
					{
						basicFrm.Collections.value = basicFrm.Collections.value + frm.elements[i].value + ","
						iCollectionCheckedCount++
					}	    
					else
					{   
		                 
						basicFrm.othercollections.value = basicFrm.othercollections.value + frm.elements[i].value + ","	                    
		                 
					}
    	        }
    	        else
    	        {
    				
					bOtherCollectionsChecked = frm.elements[i].checked;
					
    	        }
	            iCollectionCount++
	        }
	    }
	    
	   
	    
	    //Check Orientation
		if(frm.elements[i].name.indexOf('chkOrientation') != -1)
		{	     
			if(frm.elements[i].checked)
			{
				basicFrm.ori.value = basicFrm.ori.value + frm.elements[i].value + ","
        		iOriCount++
			}	
		    
			iOriCheckedCount++
		}    	 
		    
		     
	    
	      //Check Point Of View
	     if(frm.elements[i].name.indexOf('chkPointOfView') != -1)
	    {	     
	       
	        if(frm.elements[i].checked)
	        {
	            basicFrm.PointOfView.value = basicFrm.PointOfView.value + frm.elements[i].value + ","
	           
	             iPOVCheckedCount++
	        }
	        iPOVCount++
	    }
	    
	    //Check Resolutions 
	     if(frm.elements[i].name.indexOf('chkFileSize') != -1)
	    {	     
	       
	        if(frm.elements[i].checked)
	        {
	            basicFrm.FileSize.value = basicFrm.FileSize.value + frm.elements[i].value + ","
	            
	                iFileSizeCheckedCount++  
	        }
	        iFileSizeCount++
	    }	    
	    
	    
	   
	if(frm.chkCat)
		{
			//Check Categories
			if(frm.elements[i].name.indexOf('chkCategory') != -1)
			{	     
				if(frm.elements[i].checked)
				{
					basicFrm.cat.value = basicFrm.cat.value + frm.elements[i].value + ","
        			iCatCount++
        			
				}	
		        
					iCatCheckedCount++          	        
			}		      
    	}
	  	   	   	    
	}
	
	if(frm.chkSearchAllCollections)
	{	    	
	    
	   if(frm.chkSearchAllCollections.checked)
	   {	    	
	        //If all checkboxes are checked then pass blank to search engine
            basicFrm.Collections.value = '';
            basicFrm.othercollections.value = '';
       }
       else
       {
			
            if(eval('frm.chkColl' + (iCollectionCount) + '.checked'))
            {
               
                basicFrm.Collections.value = '';                
            }
            else
            {
                if(iCollectionCheckedCount == iCollectionCount) basicFrm.Collections.value = '';
				
                basicFrm.othercollections.value = '';
                
            }
            
       }
    }
    
    
    if(basicFrm.ori)
	{
		if(iOriCount == iOriCheckedCount)
		{
			 basicFrm.ori.value = '';
		}
		else
		{
			copyAdvancedSearchValues()
		}
	}

    
    if(basicFrm.PointOfView){
        if(iPOVCheckedCount == iPOVCount)
        {
			 basicFrm.PointOfView.value = '';
		}
		else
		{
			copyAdvancedSearchValues()
		}
    }
  
    
    if(basicFrm.FileSize){
        if(iFileSizeCheckedCount == iFileSizeCount)
        {
			 basicFrm.FileSize.value = '';
		}
		 else
		{
			copyAdvancedSearchValues()
		}
    }
	
	
   	if(basicFrm.cat)
	{
		//If all checkboxes are checked then
		if(iCatCount == iCatCheckedCount)
		{
			basicFrm.cat.value = '';
		}
		 else
		{
			copyAdvancedSearchValues()
			
		}
	}
		 
	
		   
       //Track IR 11350
    if(basicFrm.Collections)
    {
		
        if  (basicFrm.Collections.value != '') 
        {
            copyAdvancedSearchValues()
		    return true;
        }
    }
    
    if(basicFrm.othercollections)
    {
		
        if  (basicFrm.othercollections.value != '') 
        {
            copyAdvancedSearchValues()
		    return true;
        }
    }
    
        //Track IR 11350
    if(basicFrm.PointOfView)
    {
        if  (basicFrm.PointOfView.value != '') 
        {
            copyAdvancedSearchValues()
		    return true;
        }
    }
    
          //Track IR 11350
    if(basicFrm.FileSize)
    {
        if  (basicFrm.FileSize.value != '') 
        {
            copyAdvancedSearchValues()
		    return true;
        }
    }
    
 
    
	var elementId;
    //find radDates element number
	for(var i = 0; i < frm.elements.length;i++)
	{
	    
	    if(frm.elements[i].name.indexOf('rdt') != -1)
	    {
	    
	        elementId = i;
	        i = frm.elements.length
	    }
	    
	 }
    
    //if the 'In the last <blank> days' option is selected
		// and there is no entry in then days then make the radio button value blank
	
	if(frm.elements[elementId].checked) 
	{
		    
		if (frm.nds.value == '')
		{			
			frm.elements[elementId].value = ''
		}
		else
		{
		   frm.dt1.value = errorMessages[1]
		   copyAdvancedSearchValues()	     
		   return true;		
		}
	}
	else
	{	
		if(frm.dt1.value != errorMessages[1])
		{
			 frm.nds.value = "";
			
			 copyAdvancedSearchValues()	     
			 return true;		
		}
	
		
	
	}
	
		
		
    
    
    /*********** Other Criteria**************/
	//Photographer  (Both)
	if(frm.pht)
	{
        if(!isWhitespace(frm.pht.value))
        {			
	       
	        copyAdvancedSearchValues()
	        return true;		
        }
        
        
    }

	
	if(frm.loc)
	{
	    //Location(Both)
	    if(!isWhitespace(frm.loc.value))
	    {			
		   copyAdvancedSearchValues()
		    
		    return true;		
	    }
	   
	 }
	 
	
	
    //Date Photographed(not outline)
    if(frm.phtdt)
	{
        if(!isWhitespace(frm.phtdt.value))
        {
		    if(frm.phtdt.value != errorMessages[1])
		    {	
		        copyAdvancedSearchValues()
		       
		        return true;
		    }
		}
		
		
	}
	
	
	 if (frm.pdt)
	 {
	    //Date Published(outline only)
	    if(!isWhitespace(frm.pdt.value))
	    {
		    if(frm.pdt.value != errorMessages[1])
		    {	
		         copyAdvancedSearchValues()
		        
		        return true;
		    }
	    }
	    
	    
	}
	
	if (frm.mag)
	 {
	    //Magazine (outline)
	    if(!isWhitespace(frm.mag.value))
	    {			
		    copyAdvancedSearchValues()
		    return true;		
	    }
	    
	 
	 }
	 
	 if(!isWhitespace(frm.img.value))
	{									
		
		copyAdvancedSearchValues()
		return true;
	}	
    
 
    if(frm.viw)
    {
		if(frm.viw.selectedIndex > 0)
		{
			copyAdvancedSearchValues()
			return true;
		}
    }
    
     if(frm.res)
    {
		if(frm.res.selectedIndex > 0)
		{			
			copyAdvancedSearchValues()
			return true;
		}
    }
    
	return false;
}

function copyAdvancedSearchValues()
{	
	var frm = document.frmAdvancedSearch
	var basicFrm = document.frmSearch
	
	
	if(basicFrm.Collections)
	{
		if(bOtherCollectionsChecked) 
		{
			basicFrm.Collections.value = '';
		}
		else
		{
			basicFrm.othercollections.value = '';
		}
	}
	
	if(frm.res) basicFrm.res.value = frm.res.options[frm.res.selectedIndex].value;
	if(frm.viw) basicFrm.viw.value = frm.viw.value;
	if(frm.ppl) basicFrm.ppl.value = frm.ppl.value
	if(frm.img) basicFrm.img.value = frm.img.value
	if(frm.nds)
	{
		if(frm.nds.value != "")
		{
			
			basicFrm.nds.value = "-"+frm.nds.value;
		}
		else
		{
			if(frm.dt1.value != errorMessages[1])
			{
				basicFrm.dt1.value = frm.dt1.value;
				basicFrm.dt2.value = frm.dt2.value;
			}
		}
	}
	if(frm.pht) basicFrm.pht.value = frm.pht.value;	
	if(frm.loc) basicFrm.loc.value = frm.loc.value;
	if(frm.phtdt) basicFrm.phtdt.value = frm.phtdt.value;
	if(frm.pdt)
	{
		
		if(frm.sdt2.selectedIndex > 0)
		{
			var pdtvalue = escape(frm.sdt2.options[frm.sdt2.selectedIndex].value) + frm.pdt.value;
			
			basicFrm.pdt.value = pdtvalue;
		}
	}
	if(frm.mag) basicFrm.mag.value = frm.mag.value;
	if(frm.chkLanguage)
	{
		basicFrm.lng.value = frm.chkLanguage.checked;
	}
	if(frm.src)  basicFrm.src.value = frm.source.value;
	
	basicFrm.as.value = "True";
	
}

function GetCookie (name) {  

	var arg = name + "=";  

	var alen = arg.length;  

	var clen = document.cookie.length;  

	var i = 0;  

	while (i < clen) {    

		var j = i + alen;    

		if (document.cookie.substring(i, j) == arg) return getCookieVal (j);    

		i = document.cookie.indexOf(" ", i) + 1;    

		if (i == 0) break;   

	}  

	return null;

}

function getCookieVal (offset) {  

	var endstr = document.cookie.indexOf (";", offset);  

	if (endstr == -1) { endstr = document.cookie.length; }

	return unescape(document.cookie.substring(offset, endstr));

}

function DeleteCookie (name,path,domain) {
  if (GetCookie(name)) {
    document.cookie = name + "=" +
      ((path) ? "; path=" + path : "") +
      ((domain) ? "; domain=" + domain : "") +
      "; expires=Thu, 01-Jan-70 00:00:01 GMT";
  }
}

function turnOnMSO()
{
	var frm = document.frmSearch;
	var searchData = GetCookie("SearchData")	
	var newSearchData = searchData
	
	if(searchData != null)
	{
		if(bAdvancedSearchOn)
		{
			newSearchData = searchData + "&as=True";
		}
		else
		{
			
			
			newSearchData = searchData.replace(/&as=True/i, "");
			newSearchData = newSearchData.replace(/as=True/i, "");
					
			newSearchData = newSearchData.replace("&as=", "");
		}
				
		DeleteCookie("SearchData","/",CookieDomain);
		
		document.cookie = "SearchData=" + newSearchData + ";path=/;domain=" + CookieDomain;
	}
	else
	{
		newSearchData = "as=True";
		document.cookie = "SearchData=" + newSearchData + ";path=/;domain=" + CookieDomain;
	}
			
}

function SwitchTabs(tabNumber)
{
	
	var preferences = GetCookie("Preferences")	
	var newPreferences = preferences
	
	if(preferences != null)
	{
								
		if(preferences.indexOf("tab=") !=-1)
		{	
			newPreferences = preferences.replace(/tab=\d/, "tab=" + tabNumber);
		}
		else
		{
			newPreferences = preferences  + "&tab=" + tabNumber;
		}
		
		DeleteCookie("Preferences","/",CookieDomain);
		
		document.cookie = "Preferences=" + newPreferences + ";path=/;domain=" + CookieDomain;
	}
	else
	{
		newPreferences = "tab=" + tabNumber;
		document.cookie = "Preferences=" + newPreferences + ";path=/;domain=" + CookieDomain;
	}
	
	//refresh page
	var href = window.top.location.href;
	window.top.location.href = href;
			
}



function ChangeSearchSort(dropDown)
{
	var frm = document.frmSearch;
	var searchData = GetCookie("SearchData")	
	var newSearchData = searchData
	var newSortOrder = dropDown.options[dropDown.selectedIndex].value
	
	if(searchData != null)
	{
		//Is there a sort order in the cookie?
		if(searchData.indexOf('sort=') != -1)//Yes.
		{
			
			//Replace its value with the new sort order value
		
			newSearchData = searchData.replace(/\&?sort=[0-3]?/i, "&sort=" + newSortOrder);
			
			
		}
		else//No
		{	
			 
			//Add a new sort order value to the cookie
			newSearchData = searchData + "&sort=" + newSortOrder;
		}
		DeleteCookie("SearchData","/",CookieDomain);		
		document.cookie = "SearchData=" + newSearchData + ";path=/;domain=" + CookieDomain;
	}
	var rand = Math.round(Math.random() * 20);
	//Refresh the search page so the new sort takes effect.
	window.location.href = "/search/searchresultsdisplay.asp?rnd=" + rand
	
}

function showAdvancedSearch(sOnText, sOffText)
{
	
	var frm = document.frmSearch;
	
	if(document.all)
	{
		
		if(document.all['divAdvancedSearch'].style.display == "none")
		{
			document.all['divAdvancedSearch'].style.display= "block"
			
			document.all['linkTurnOff'].innerHTML = errorMessages[3]
			bAdvancedSearchOn = true;
			//moveDown(230);
		}
		else
		{
			
			document.all['divAdvancedSearch'].style.display= "none"
			if(frm.hidCollection.value != 'outline')
			{
				resetAdvancedSearchFilters();
				
			}
			document.all['linkTurnOff'].innerHTML = errorMessages[4]
			bAdvancedSearchOn = false;
			//moveDown(-230);
		}
		
	}
	else if(document.getElementById)
	{
		
		if(document.getElementById('divAdvancedSearch').style.display == "none")
		{
			document.getElementById('divAdvancedSearch').style.display= "block"
			
			document.getElementById('linkTurnOff').innerHTML = errorMessages[3]
			bAdvancedSearchOn = true;
			
			//moveDown(230);
		}
		else
		{
			resetAdvancedSearchFilters();
			document.getElementById('divAdvancedSearch').style.display= "none"
		
			document.getElementById('linkTurnOff').innerHTML = errorMessages[4]
			bAdvancedSearchOn = false;
			
			//moveDown(-230);
		}
	}
	else //netscape 4
	{
		var loc = errorMessages[5];		
		document.location = loc + "?as=true";
	
	}
	
	turnOnMSO();
	
}

function hideAdvancedSearch()
{
	var loc = errorMessages[5];		
	document.location = loc + "?as=";
}

function checkSearchAllCategories()
{
	var bAllChecked = true;
	
	for(i = 1; i < 20;i++)
	{
		
		if(eval('document.frmAdvancedSearch.chkCategory_' + i))
		{
			if(!eval('document.frmAdvancedSearch.chkCategory_' + i + '.checked'))
			{
				bAllChecked = false;
			}
		}
	}
	
	if(bAllChecked)
	{
		eval('document.frmAdvancedSearch.chkCat.checked = true');
	}
	else
	{
		eval('document.frmAdvancedSearch.chkCat.checked = false');
	}
}

function checkSearchAll(checkboxName, searchAllName, checkboxCount)
{
	var bAllChecked = true;
	
	for(i = 1; i < checkboxCount+1;i++)
	{
	
		if(!eval('document.frmSearch.' + checkboxName + '_' + i + '.checked'))
		{
			bAllChecked = false;
		}
	}
	
	if(bAllChecked)
	{
		eval('document.frmSearch.' + searchAllName + '.checked = true');
	}
	else
	{
		eval('document.frmSearch.' + searchAllName + '.checked = false');
	}
	

}

function checkCollectionSearchAll()
{
	var bAllChecked = true;
	var iCount = document.frmAdvancedSearch.maxcol.value
	
	for(i = 1; i < iCount;i++)
	{
	
		if(!eval('document.frmAdvancedSearch.chkColl' + i + '.checked'))
		{
			bAllChecked = false;
		}
	}
	
	if(bAllChecked)
	{
		eval('document.frmAdvancedSearch.chkSearchAllCollections.checked = true');
	}
	else
	{
		eval('document.frmAdvancedSearch.chkSearchAllCollections.checked = false');
	}
	

}

function checkAllCollections(sCheckBoxName,oSearchAllCheckedBox)
{
    var frm = document.frmAdvancedSearch
    var bCheck = oSearchAllCheckedBox.checked
    
    for(var i=0;i< frm.elements.length;i++)
    {
        
        if(frm.elements[i].name.indexOf(sCheckBoxName) != -1)        
        {
           
            frm.elements[i].checked = bCheck;
                   
        }
    
    
    }
}

function selectInTheLast()
{

	for (var i=0;i < document.frmAdvancedSearch.elements.length;i++)
        {
            if(document.frmAdvancedSearch.elements[i].name == 'rdt')
            {
                document.frmAdvancedSearch.elements[i].checked=true;
                document.frmAdvancedSearch.elements[i + 2].checked=false;
                return;
            }
        
        }

}

function selectByDate()
{
//selects the "By Date" radio button

    for (var i=0;i < document.frmAdvancedSearch.elements.length;i++)
    {
        if(document.frmAdvancedSearch.elements[i].name == 'radDates')
        {
            document.frmAdvancedSearch.elements[i].checked=false;
            document.frmAdvancedSearch.elements[i + 2].checked=true;
            return;
        }
    
    }

}

function isNumeric(sField)
{

	if(sField.value != '')
	{
	
		if(isNaN(sField.value))
		{
		
			alert(errorMessages[6]);
			sField.focus();
			
		}
	
	}
}

// Make an array and populate it with zeroes
function makeArray(n) 
{
   for (var i = 1; i <= n; i++) 
   {
      this[i] = 0
   } 
   return this
}

// returns true if character expression s is a valid date string
// in the 20th or 21st century
// expects mm/dd/yyyy format
function isDateString(fieldRef)
{
	var d = new makeArray(3);
	var c, i, k;
	var s=fieldRef.value;

	if( s == errorMessages[1].toUpperCase() || s == errorMessages[1].toLowerCase()) 
	{
		return true;
	}
	
	if(s == "")
	{
		fieldRef.value = errorMessages[1].toLowerCase()
		return true
	
	}
	d[1] = d[2] = d[3] = 0;
	for (i=0, k=1; i<s.length; i++) 
	{
		c = s.charAt(i);
		if ( isDigit(c) ) 
		{
			d[k] = d[k]*10 + parseInt(c);
		}
		else 
		{
			if ( c != "/"  && c != " " && c != "-" ) 
			{
				warnInvalid(fieldRef, iDelimiter);
				return false
			}
			++k;
			if( k > 3 )
				break;
		}
	}

	var intYear = d[3];
	var intMonth = d[1];
	var intDay = d[2];
	var cMonth = ""+intMonth;
	var cDay = ""+intDay;

	if (intYear == 0 && intMonth == 0 && intDay == 0) 
		return true;		// Empty date is allowed

	if (intYear < 20)
		intYear += 2003;
		
	if (intYear < 100 && intYear > 20)
		intYear += 1900;
		
	if (intMonth < 1 || intMonth > 12) 
	{
		return warnInvalid(fieldRef, ""+intMonth+errorMessages[7]);
	}
	
	if (intYear < 1900 || intYear > 2020) 
	{
		return warnInvalid(fieldRef, errorMessages[8])
	}
	
	if (intDay < 1 || intDay > daysInMonth[intMonth]) return warnInvalid(fieldRef," "+intDay+errorMessages[9]+intMonth+"/"+intYear);
	
	if (intMonth == 2 && intDay > daysInFebruary(intYear)) return warnInvalid(fieldRef,""+intYear+errorMessages[10]);
	
	if (cMonth.length == 1)
		cMonth = "0"+cMonth;
		
	if (cDay.length == 1)
		cDay = "0"+cDay;
	
	fieldRef.value = "" + cMonth + "/" + cDay + "/" + intYear
	return true;
}

function daysInFebruary (year)
{
    return (((year % 4 == 0) && ( (!(year % 100 == 0)) || (year % 400 == 0) ) ) ? 29 : 28 );
}

function warnInvalid (theField, s)
{   
    alert(s)
	theField.focus()
    return false;
}

function isDigit (c)
{
	return ((c >= "0") && (c <= "9"));
}

function isWhitespace(s)
{
    
	var i, whitespace = ' ';
	if ((s == null) || (s.length == 0)) return true;

	for (i = 0; i < s.length; i++)
	{
	    var c = s.charAt(i);
	    if (whitespace.indexOf(c) == -1) return false;
	}
	return true;
}

function clearSearchText()
{
	
	var frm = document.frmSearch;
	var sSearchText = frm.hidKeyword.value;
	
		if(frm.selSearchIn)
		{
			
			if((frm.selSearchIn.selectedIndex == 0)||(document.all['divSearchInResults'].display == "none"))
			{
				
				if (sSearchText == frm.txt.value)
				{
				
						frm.txt.value = '';
						frm.txt.focus();  	
				    
				}
			}
			else if(frm.selSearchIn.selectedIndex > 0)
			{
				
				frm.cat.value = frm.selSearchIn.options[frm.selSearchIn.selectedIndex].value + ",";		
			}
		}
		else
		{
			if (sSearchText == frm.txt.value)
			{			
				frm.txt.value = '';
				frm.txt.focus();  				
			}
		}
	

}

function openLogin()
{
	var w = window.open('/login/Login.aspx','login','status=yes,scrollbars=yes,resizable=yes,width=550,height=550')
	w.focus();
}

function SwitchTabs(tabNumber)
{
	
	var preferences = GetCookie("Preferences")	
	var newPreferences = preferences
	
	if(preferences != null)
	{
			
		newPreferences = preferences.replace(/tab=./, "tab=" + tabNumber);
		
		DeleteCookie("Preferences","/",CookieDomain);
		
		document.cookie = "Preferences=" + newPreferences + ";path=/;domain=" + CookieDomain;
	}
	else
	{
		newPreferences = "tab=" + tabNumber;
		document.cookie = "Preferences=" + newPreferences + ";path=/;domain=" + CookieDomain;
	}
	
	//refresh page
	var href = window.top.location.href;
	window.top.location.href = href;
			
}

function SwitchTabs(tabNumber)
{
	
	var preferences = GetCookie("Preferences")	
	var newPreferences = preferences
	
	if(preferences != null)
	{
			
		newPreferences = preferences.replace(/tab=./, "tab=" + tabNumber);
		
		DeleteCookie("Preferences","/",CookieDomain);
		
		document.cookie = "Preferences=" + newPreferences + ";path=/;domain=" + CookieDomain;
	}
	else
	{
		newPreferences = "tab=" + tabNumber;
		document.cookie = "Preferences=" + newPreferences + ";path=/;domain=" + CookieDomain;
	}
	
	//refresh page
	var href = window.top.location.href;
	window.top.location.href = href;
			
}

function SwitchTabs(tabNumber)
{
	
	var preferences = GetCookie("Preferences")	
	var newPreferences = preferences
	
	if(preferences != null)
	{
			
		newPreferences = preferences.replace(/tab=./, "tab=" + tabNumber);
		
		DeleteCookie("Preferences","/",CookieDomain);
		
		document.cookie = "Preferences=" + newPreferences + ";path=/;domain=" + CookieDomain;
	}
	else
	{
		newPreferences = "tab=" + tabNumber;
		document.cookie = "Preferences=" + newPreferences + ";path=/;domain=" + CookieDomain;
	}
	
	//refresh page
	var href = window.top.location.href;
	window.top.location.href = href;
			
}

function SwitchTabs(tabNumber)
{
	
	var preferences = GetCookie("Preferences")	
	var newPreferences = preferences
	
	if(preferences != null)
	{
			
		newPreferences = preferences.replace(/tab=./, "tab=" + tabNumber);
		
		DeleteCookie("Preferences","/",CookieDomain);
		
		document.cookie = "Preferences=" + newPreferences + ";path=/;domain=" + CookieDomain;
	}
	else
	{
		newPreferences = "tab=" + tabNumber;
		document.cookie = "Preferences=" + newPreferences + ";path=/;domain=" + CookieDomain;
	}
	
	//refresh page
	var href = window.top.location.href;
	window.top.location.href = href;
			
}
