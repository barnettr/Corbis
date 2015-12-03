var isPageLoaded = false;
var isNav4, isIE4, isIE5Mac
		
isIE5Mac=false;
if ((navigator.appVersion.indexOf("MSIE 5",1)>0) && (navigator.appVersion.indexOf("Macintosh",1)>0)){
   isIE5Mac=true;
}

if (parseInt(navigator.appVersion.charAt(0)) >= 4)
{
	var isIE4 = (navigator.appName=="Microsoft Internet Explorer") ? true: false;
}

if (parseInt(navigator.appVersion.charAt(0)) <= 4)
{
   var isNav4 = (String(navigator.appName) == "Netscape") ? true : false;
}
		
function getSequenceNumber(sMediaUID)
{
			for (var i=0; i <= MediaData.length-1; i++){
				if (MediaData[i].mediaUID==sMediaUID){
				   return i; 
				}
			} 
			return -1;  
}

function getAllMediaIDs()
{
		var sIDs="";
		for (var i=0; i <= MediaData.length-1; i++)
		{
			sIDs += MediaData[i].mediaID + ",";
		} 
		return sIDs;  
}



function updateIndicators(sMediaID, sMediaUID, sContainer, bStatus)
{
		   if (pubIsImageSets){return;}
		   var iSeqNum = getSequenceNumber(sMediaUID);

		   if (bStatus)
		   { 
		     addToInList(sMediaUID.substring(0,38), sContainer)
		   }
		   else
		   {
		     removeFromInList(sMediaUID.substring(0,38), sContainer)
		   }
		   
		   if (iSeqNum==-1) //MediaID not on this page
		   {
		     return;
		   }
		   
		   if (sContainer=="Folders")
		   {
		      MediaData[iSeqNum].InLBX = bStatus
		   }
		   if (sContainer=="Cart")
		   {
		      MediaData[iSeqNum].InCART = bStatus
		   }
		   if (sContainer=="pvquickpic")
		   {  
			  MediaData[iSeqNum].InQPPreview = bStatus
		   }
		   
		   updateLBXIndicator(iSeqNum);
		   updateCARTIndicator(iSeqNum);
		   updateQPIndicator(iSeqNum);
		   updateDivHighlight(iSeqNum);
		   
		   
}
	
function refreshIndicators(iSeqNum)
{
	    if (pubIsImageSets){return;}
	    updateLBXIndicator(iSeqNum);
		updateCARTIndicator(iSeqNum);
		updateDivHighlight(iSeqNum);
}
	
function updateQPIndicator(iSeqNum)
{
	      var sImage;
	      
	      if (pubIsImageSets){return;}
	      
		  
		  if (MediaData[iSeqNum].InQPPreview)
		  {
			 sImage = "/images/btn_box_on_cropped.gif";
		  }
		  else
		  {
			 sImage = "/images/btn_box_off_cropped.gif";
		  }
		   
		 
		if (!document.layers)
	       {  
			  if (document.images["imgQP" + iSeqNum])
			  {
			     document.images["imgQP" + iSeqNum].src = sImage
			  }
		 }
		 else
	     {
			    
			    if (document.layers["LayList"])
				{
				   if(eval("document.layers['LayList'].document.imgQP" + iSeqNum))
				   {
				     eval("document.layers['LayList'].document.imgQP" + iSeqNum + ".src ='" + sImage + "'")
				   }
			    }
			    else
			    {
				   if (eval("document.layers['LayMedia"  + iSeqNum + "'].document.imgQP" + iSeqNum))
				   {
				      eval("document.layers['LayMedia"  + iSeqNum + "'].document.imgQP" + iSeqNum + ".src ='" + sImage + "'")
				   }
			    }
		 }
		  
}
	
function updateLBXIndicator(iSeqNum)
{

	  var sImage;
	  if (pubIsImageSets){return;}
	  
		  if (MediaData[iSeqNum].InLBX)
		  {
			 sImage = "/images/btn_box_on_cropped.gif";
		  }
		  else
		  {
		     sImage = "/images/btn_box_off_cropped.gif";
		  }
	   
	     if (!document.layers)
	       {
		      if (document.images["imgLBX" + iSeqNum])
	          {
	              document.images["imgLBX" + iSeqNum].src = sImage
			  }
		   }
		 else
	       {
			    if (document.layers["LayList"])
				{
				   if(eval("document.layers['LayList'].document.imgLBX" + iSeqNum))
				   {
				     eval("document.layers['LayList'].document.imgLBX" + iSeqNum + ".src ='" + sImage + "'")
				   }
			    }
			    else
			    {
				  if(eval("document.layers['LayMedia"  + iSeqNum + "'].document.imgLBX" + iSeqNum))
				   {
				     eval("document.layers['LayMedia"  + iSeqNum + "'].document.imgLBX" + iSeqNum + ".src ='" + sImage + "'")
				   }
			    }
		   }
	     
}
	
function updateCARTIndicator(iSeqNum)
{
	  var sImage;
	  if (pubIsImageSets){return;}
	  if (MediaData[iSeqNum].InCART)
	  {
		 sImage = "/images/btn_box_on_cropped.gif";
	  }
	  else
	  {
	     sImage = "/images/btn_box_off_cropped.gif";
	  }
	  
	  if (!document.layers)
	  {
	    if (document.images["imgCART" + iSeqNum])
	      {
	         document.images["imgCART" + iSeqNum].src = sImage
		  }
	  }
	  else
	  {
	      if (document.layers["LayList"])
			{
			  if(eval("document.layers['LayList'].document.imgCART" + iSeqNum))
				{
			      eval("document.layers['LayList'].document.imgCART" + iSeqNum + ".src ='" + sImage + "'")
				}
		    }
		  else
		    {
			   if(eval("document.layers['LayMedia"  + iSeqNum + "'].document.imgCART" + iSeqNum))
			   {
			      eval("document.layers['LayMedia"  + iSeqNum + "'].document.imgCART" + iSeqNum + ".src ='" + sImage + "'")
			   }
		    }
	  }
}
	
	
function updateDivHighlight(iSeqNum)
{
		  var backgroundColor ="#FFFFFF";
		  var borderColor ="#FFFFFF";
		  
	      if (!MediaData[iSeqNum])return;
	      
		  //Set border and background color of div tag
		  if ((MediaData[iSeqNum].InCART || MediaData[iSeqNum].InLBX) || (MediaData[iSeqNum].sel==1 || MediaData[iSeqNum].InQPPreview))
		  {
		      backgroundColor = "#FFFFDF";
			  borderColor = "#CCCCCC";
		  }
		  
		  if (document.all)
		  {
			
			if (eval("document.all.LayMedia" + iSeqNum))
			 {
			   eval("document.all.LayMedia" + iSeqNum).style.backgroundColor=backgroundColor;
		       eval("document.all.LayMedia" + iSeqNum).style.borderColor=borderColor;
			  }
			  
			if (document.all["LayMedia" + iSeqNum] && isIE5Mac)
			{
			     document.all["LayMedia" + iSeqNum].style.backgroundColor=backgroundColor;
			}
			
		  }
		  else if (document.getElementById)
		  {
		       eval("document.getElementById('LayMedia" + iSeqNum + "').style.background=backgroundColor");
			   eval("document.getElementById('LayMedia" + iSeqNum + "').style.borderColor=borderColor");
		  }
		  
		  if (document.layers )
		  {
		       eval("document.LayMedia" + iSeqNum + ".bgColor=backgroundColor");
			   // No border color is NS4.* layers
		  }
		  
	
}

var sMedia_IDs = ''
function addToContainer(sMediaID, sMediaUID, sArea, sContainer)
{
	
		var iSeqNum = getSequenceNumber(sMediaUID);
		var showTab;
		 
		
		
		if (iSeqNum==-1) //MediaID not on this page
		{
		  return;
		}
	    
		
		if (sArea.toLowerCase()=="folders")
		{
	       
	        if (!MediaData[iSeqNum].InLBX){

			      if (m_isLoggedOnAndHasDefaultLBX)
				  {
				        if(parent.frames[1].bReadOnlyLightbox)
				        {
				            alert(m_sCont[0])
				        }
				        else
				        {
				            updateIndicators(sMediaID, sMediaUID, "Folders", true)
				            
				            parent.frames[1].location = '/previewpane/previewpanes.aspx?tab=lightbox&mainpage=Search&actn=addlightbox&mediauids=' + sMediaUID + '&corbisid=' + MediaData[iSeqNum].CorbisID;
				            sMedia_IDs = ''
	    			    }
				  }
                  else
				  {
	                    //let the previewpane take care of it
	                    parent.frames[1].addToLightbox(sMediaID, sMediaUID, MediaData[iSeqNum].CorbisID, false);
                  }			        
			  
			}
			else
			{
			   alert(m_sCont[1])
			}
			
		}
		else if (sArea.toLowerCase()=="cart")
		{
		    if (!MediaData[iSeqNum].InCART){
		    
			    if (pubIsLoggedOn)
				{		    
			        
			        parent.frames[1].location = '/previewpane/previewpanes.aspx?actn=addcart&area=' + sContainer + '&fdid=' + pubDLBXUID + '&mediauids=' + sMediaUID + '&corbisid=' + MediaData[iSeqNum].CorbisID + '&mainpage=' + sContainer
			        updateIndicators(sMediaID, sMediaUID, "Cart", true)
			        sMedia_IDs = ''
				}
			    else
				{
			         parent.frames[1].addToCart(sMediaID, sMediaUID, MediaData[iSeqNum].CorbisID, sContainer, false);
			    }
			}
			else
			{
			   alert(m_sCont[2])
			}
		
	        
	    }
	
}

function setCartCount(numOfItems)
{
	try
	{
		
		var link = window.frames[0].document.getElementById("header_hypCartItems");
	
		if(link != null)
		{				
			//get the current count
			var currentText = "";
			if(link.innerText)
			{
				currentText=link.innerText;
			}
			else
			{
				currentText = link.text; //firefox
			}		
			var spaceIndex = currentText.indexOf(" ");
			var cartUIDCount = parseInt(currentText.substr(0,spaceIndex));									
			
			//replace it with the new count
			var expression = new RegExp(cartUIDCount);
			var newText = currentText.replace(expression, numOfItems);				
			if(link.innerText)
			{
				link.innerText = newText;
			}
			else
			{
				link.innerHTML = newText;	
			}	
		}
	}
	catch(e)
	{
		
	
	}
}
	
var arrQP = sQuickPicIDs.split(",")        
var iQPImageLength = 0
iQPImageLength = arrQP.length
function addToQuickPic(sMediaUID, sCorbisID, sContainer)
{        
    
    if(iQPImageLength > 20)
    {
        alert(m_sCont[3]);
    }
    else
    {
        if(sQuickPicIDs.indexOf(sMediaUID) == -1)
        {
            if(parent.frames[1])
            {
                updateIndicators("", sMediaUID , "pvquickpic", true) //Need sMediaUID
                sQuickPicIDs += sMediaUID + ","
                sMedia_IDs += sMediaUID
                
                // Send sMediaUID, Action in query string
                parent.frames[1].location = '/previewpane/previewpanes.aspx?tab=QuickPic&actn=addquickpic&mediauids=' + sMediaUID + '&corbisid=' + sCorbisID + '&mainpage=' + sContainer
                iQPImageLength++
                sMedia_IDs = ''
            }
            
        }
        else
        {
            alert(m_sCont[4]);
        }
    }
}
function SwitchOffQuickPicIndicator(sMediaUID)
{
    //Find it in MediaData
    
    var iSeqNum = getSequenceNumber(sMediaUID)
    
    //Remove ID
    sQuickPicIDs = sQuickPicIDs.replace(sMediaUID + ",","");
   
    // update MediaData
    if (MediaData[iSeqNum])
    {
      MediaData[iSeqNum].InQPPreview = false;
    }
    
    // update indicator and background
    updateQPIndicator(iSeqNum);
	updateDivHighlight(iSeqNum);
 } 
	

function AddAllImagesToDefaultLightbox()
{
			  var sMediaIDs="";
			  var sChangedAtDates="";
			  
			  for (var i=0; i <= MediaData.length-1; i++)
			  {
				  sMediaIDs +=  MediaData[i].mediaID + ","
				  sChangedAtDates += MediaData[i].ChangedAt + ","
				  updateIndicators(MediaData[i].mediaID,MediaData[i].mediaUID, "Folders", true)
			  } 
			  
              parent.frames[1].addToLightbox(sMediaIDs, "", false);
			
}
	
function changeView(intCurrentPage, strCurrentFile, strQueryString)
{
				var intNewViewMode;
				var strNewListLength="";
				var sMaxHits="";
				var sRowsCols="";
				
			 
				intNewViewMode = document.frmViewModeForm.ViewMode.options[document.frmViewModeForm.ViewMode.selectedIndex].value
				if (document.frmViewModeForm.ListLength) {
				   strNewListLength = document.frmViewModeForm.ListLength.options[document.frmViewModeForm.ListLength.selectedIndex].value
				   strNewListLength = "&IPP=" + strNewListLength
				}
				
		 		if (document.frmViewModeForm.MaxHits){
		           sMaxHits = "&MaxHits=" + document.frmViewModeForm.MaxHits.options[document.frmViewModeForm.MaxHits.selectedIndex].value
				}         
				
				if (document.frmViewModeForm.Rows){
				     sRowsCols = "&rows=" + document.frmViewModeForm.Rows.options[document.frmViewModeForm.Rows.selectedIndex].value + "&Columns=" + document.frmViewModeForm.Columns.options[document.frmViewModeForm.Columns.selectedIndex].value
				}	
		
				document.doActionFormData.action = document.doActionFormData.action + "&ViewMode=" + intNewViewMode + strNewListLength + sMaxHits + sRowsCols;
				document.doActionFormData.submit();		
}
		
function DoNothing()
{return;}
		
function selMediaItem(num)
{
		   if(isPageLoaded==false){
			     return;
		   }
		   
		   if (MediaData[num].sel == 0)
		   {
		       MediaData[num].sel = 1
		   }
		   else {
		       MediaData[num].sel = 0
		   }
		   
		   if (MediaData[num].sel== 1){
		      addToSelectionList(num)
		   }
		   else{
		      removeFromSelectionList(num)
		   }
		   
		   if (pubViewUser!="Orders"){
		      updateDivHighlight(num)
		   }
		   return;
}
		
function initializeSelections()
{
		    
			
			if (!document.doActionFormData){return;}
			
			if (MediaURL) 
			{
			   if (!document.layers)  // Works for IE and Netscape 6.*/7
			   {
			     for (var iCnt=0; iCnt<=MediaURL.length-1; iCnt++)
			     {
				    eval("document.MediaSRC" + iCnt + ".src = MediaURL[" + iCnt + "]")
	    	     }
			   }
			}
			
			window.clearInterval(TimeOutID);
			
			return;
}
	
function itemsSelectedCount()
{
		   var intCount=0;
		   for (var i=0; i <= MediaData.length-1; i++){
			      if (MediaData[i].sel == 1) {
			        intCount=intCount+1
				  }
			} 
		    return intCount;
}
		
		
function MediaIDs(blnSelected)
{
		   var strIDs="";
		   for (var i=0; i <= MediaData.length-1; i++){
		      if (blnSelected) {
			      if (MediaData[i].sel == 1) {
			        strIDs = strIDs + MediaData[i].mediaID + ","
				  }
			  }
			  else{
			        strIDs = strIDs + MediaData[i].mediaID + ","
			  }
		    } 
			 return strIDs;
}

function getCorbisIDs(bSelected)
{
		var sIDs="";
		for (var i=0; i <= MediaData.length-1; i++){
			if (bSelected)
			{
				if (MediaData[i].sel == 1)
				{
				   sIDs += MediaData[i].CorbisID + ",";
				}
			}
			else
			{
				if (MediaData[i].sel == 0)
				{
				   sIDs += MediaData[i].CorbisID + ",";
				}
			}
		} 
		return sIDs;
}

function getMediaUIDs(bSelected)
{
		   var sUIDs="";
		   for (var i=0; i <= MediaData.length-1; i++){
		      if (bSelected) {
			      if (MediaData[i].sel == 1)
			      {
			        sUIDs += MediaData[i].mediaUID + ",";
				  }
			  }
			  else
			  {
			     if (MediaData[i].sel == 0)
			      {
			        sUIDs += MediaData[i].mediaUID + ",";
				  }
			  }
		    } 
			return sUIDs;
}

function CorbisIDs(bSelected)
{
		var strIDs="";
		for (var i=0; i <= MediaData.length-1; i++){
			if (blnSelected) {
				if (MediaData[i].sel == 1) {
					strIDs = strIDs + MediaData[i].CorbisID + ","
				}
			}
			else{
				    strIDs = strIDs + MediaData[i].CorbisID + ","
			}
		} 
		return strIDs;
}
	
		
function getChangedAt(blnSelected)
{
		  var strIDs="";
		  for (var i=0; i <= MediaData.length-1; i++){
		     if (blnSelected) {
			      if (MediaData[i].sel == 1) {
				      strIDs = strIDs + MediaData[i].ChangedAt + ","
				  }
			  }
			  else{
				  strIDs = strIDs + MediaData[i].ChangedAt + ","
			  }
		  } 
		  return strIDs;
}
		
		

		
function allMediaSelectionState(state)
{
		  //state 1 is selected and state=0 is deselect all;
		  
		  if (state==1){
		    if (areAllItemsSelected()){
			  	  return;
			}
		  }
		  
		  if(isPageLoaded==false){
		      return;
		 }
 
         
		 if (window.MediaData) {
			   for (var i=0; i <= MediaData.length-1; i++){
				      if (MediaData[i].imgID != -1) {
					        MediaData[i].sel = state
						   //added for selection across pages
						   if (MediaData[i].sel == 1){	
						      addToSelectionList(num)
						   }
						   else{
						      removeFromSelectionList(num)
						   }
					  }
				}
				updateHighliteAll();
		   }
		   return;
}
		
	
var SelectAllInCart=false;
function SubmitMe()
{
		var confirmMst;
		var strMediaIds;
		var iCountMedias;
		var intTotalMedias;
		var orderLimit;
		var warningLimit;
		
		var sFieldValue = document.doActionFormData.selectionUIDList.value;
		var sFieldValuePriced = document.doActionFormData.AllPriceImagesInCart.value
		 
		if (!document.doActionFormData){return;}
		
		if (sFieldValue.indexOf("{",1)!=-1)
		{
			if (!SelectAllInCart)
			{
			    strMediaIds = document.doActionFormData.selectionList.value.split(",");
			}
			else
			{
			    strMediaIds = document.doActionFormData.AllPriceImagesInCart.value.split(",");
			}
			
			iCountMedias = (strMediaIds.length) - 1;
			intTotalMedias = iCountMedias;
			CreateMediaList();
		}
		else
		{
			if (sFieldValuePriced.indexOf("{",1)==-1)
			{
			  alert(m_sCont[9]) //None priced
			}
			else if (sFieldValue.indexOf("{",1)==-1)
			{
			  alert(m_sCont[10]) //None selected
			}
		}
}
		
function CreateMediaList() // Shoppingcart only code.
{ 
		var sMediaUIDs;
		var mediaCounts; 
		var temp;
		if (document.doActionFormData.selectionUIDList.value != '')
		{
			temp = document.doActionFormData.selectionUIDList.value.split(",");
			mediaCounts = (temp.length) - 1;
			sMediaUIDs = document.doActionFormData.selectionUIDList.value;	
		}else{
			sMediaUIDs = '';
			mediaCounts = '';
		}
		if (sMediaUIDs=='')
		{
		  alert(m_sCont[5]);
		  return;
		}

		if (sMediaUIDs != '')
		{
		  document.frmShoppingCart.mediauids.value = sMediaUIDs
	    }
	    
	    
	    document.frmShoppingCart.action = "/shoppingcart/validateItems.aspx"
		document.frmShoppingCart.mediaCount.value = mediaCounts
		document.frmShoppingCart.hidAction.value = "buynow"
		document.frmShoppingCart.submit();
}
		
function areAllItemsSelected()
{ 
		var allSelected=true 
		for (var i=0; i <= MediaData.length-1; i++){
			if (MediaData[i].sel == 0) {
				allSelected = false
			}
		}
		return allSelected;
}
	
function createCorbisIDList()
{ 
		var strSelectedMediaItems="" 
		for (var i=0; i <= MediaData.length-1; i++){
			if (MediaData[i].sel == 1) {
				strSelectedMediaItems = strSelectedMediaItems + MediaData[i].CorbisID + ","
			}
		}
		return strSelectedMediaItems;
}
		
function createMediaListAC()
{ //Media selected across pages
		       if (!document.doActionFormData){return;}
			   if (pubViewMode!="4"){
			      return document.doActionFormData.selectionList.value;
			   }
			   else{
				  return MediaData[0].mediaID + ",";
			   }
			  return; 
}
		
function createCorbisIDListAC()
{ //Media selected across pages
		       if (!document.doActionFormData){return;}
			   if (pubViewMode!="4"){
			      return document.doActionFormData.selectionCorbisIDList.value;
			   }
			   else{
			      return MediaData[0].mediaID + ",";
			   }
			   return;
}
		
function checkPageNumber(f,iTotalPages)
{
			 var maxPageNumber = iTotalPages;
			 if ((f.PageNumber.value > maxPageNumber) || (f.PageNumber.value <= 0) || isNaN(parseInt(f.PageNumber.value))){
			     f.PageNumber.value = pubCurrentPageNo
			     return false;
			 }
			 else
			 {
				 if (parseInt(f.PageNumber.value)!=parseInt(pubCurrentPageNo))
				 {
					 doPageForm(f.PageNumber.value);
					 return false;
				 }
				 return false;
			 }
}
		
function checkPageNumberChangeOnDelete(intNumberToDelete)
{
			if (pubMediaCountThisPage==intNumberToDelete){
			  if (pubCurrentPageNo!=1){
			    return (pubCurrentPageNo-1);
			  }
			}
			else{
			    return pubCurrentPageNo;
			}
}
	
var bBusy = false;

function removeFromCart(sMediaUID)
{
		if(!bBusy)
		{
		  bBusy = true;
		  //document.doActionFormData.mchange.value = MediaData[getSequenceNumber(sMediaID)].ChangedAt + ","
		  document.doActionFormData.selectionUIDList.value = document.doActionFormData.selectionUIDList.value.replace(sMediaUID + ",","")
		  document.doActionFormData.spid.value = pubID
		  document.doActionFormData.qsPageNo.value = pubCurrentPageNo
		  document.doActionFormData.strQS.value = pubQueryString
		  document.doActionFormData.mduid.value = sMediaUID + ","
		  document.doActionFormData.actn.value = "deletemedia"
		  document.doActionFormData.submit()
		 }
		 else
		 {
			var href = document.location.href;
			document.location.replace(href);
		 }
}
		
function doPageForm(PageNo)
{
			if (document.doActionFormData.qsPageNo)
			{
				document.doActionFormData.qsPageNo.value = PageNo;
				document.doActionFormData.submit();
			}
			return false;
			 
}
		
function getRes() { 
		   var ItemNo=0; 
		   var strResolutions = "";
		   for (var num=0; num<=MediaData.length-1; num++) {
			    if (MediaData[num].sel == 1) {
					if (document.all) {
						strResolutions = strResolutions + document.all[("Res" + num)].value + ","
						if (document.all[("Res" + num)].value=="none"){
						  //Content
						  alert(m_sCont[6] + MediaData[num].CorbisID)  // Content
						  return "stop";
						}
					}
					else if (document.getElementById) //IR 10190, Netscape 6.* fix
					{
					  if (pubViewMode=="1")
			           {
			               var frm = document.CheckBoxMediaForm
			           }
			           else
			           {
			                var frm = eval("document.CheckBoxMediaForm" + num)
			            }
					    if (frm)
					    {
					    strResolutions =  strResolutions + frm.elements[("Res" + num)].value + ','
					    if (frm.elements[("Res" + num)].value=="none"){
						    alert(m_sCont[6] + MediaData[num].CorbisID)  // Content
						    return "stop";
						    }
						}
					}
					else{
					       var strRes
						   if (pubViewMode == "1"){
							   eval("ItemNo = document.layers['LayList'].document.forms[0].Res" + num + ".selectedIndex");
							   eval("strRes = document.layers['LayList'].document.forms[0].Res" + num + ".options[" + ItemNo + "].value + ','");
							   strResolutions = strResolutions + strRes;
							   if (strRes=="0,"){
							      //Content
								  alert(m_sCont[6] + MediaData[num].CorbisID)  // Content
						          return "stop";
							   }
						   }
						   else{
						        eval("ItemNo = document.layers['LayMedia' + num].document.forms[0].Res" + num + ".selectedIndex");
							    eval("strRes = document.layers['LayMedia' + num].document.forms[0].Res" + num + ".options[" + ItemNo + "].value + ','");
								strResolutions = strResolutions + strRes;
								if (strRes=="0,"){
								  //Content
								  alert(m_sCont[6] + MediaData[num].CorbisID) //Content
						          return "stop";
								}
						   }
					  }
					
				 }
			}
			return strResolutions;
}
		
		
function getThisRes(num) { 
		   var ItemNo=0; 
		   var strResolutions = "";
			if (document.all) { 
				strResolutions = strResolutions + document.all[("Res" + num)].value + ","
				if (document.all[("Res" + num)].value=="none"){
				          //Content
						  alert(m_sCont[6] + MediaData[num].CorbisID) //Content
						  return "stop";
				}
			}
			else if (document.getElementById)  //IR 10190, Netscape 6.* fix
			{
			   var frm;
			   if (pubViewMode=="1")
			   {
			      frm = document.CheckBoxMediaForm
			   }
			   else
			   {
			      frm = eval("document.CheckBoxMediaForm" + num)
			   }
			   
			   strResolutions = frm.elements[("Res" + num)].value + ','
			   
			   if (frm.elements[("Res" + num)].value=="none"){
					 alert(m_sCont[6] + MediaData[num].CorbisID)  // Content
					 return "stop";
			   }
			}
			else{
				  var strRes
				  if (pubViewMode == "1"){
						   eval("ItemNo = document.layers['LayList'].document.forms[0].Res" + num + ".selectedIndex");
						   eval("strRes = document.layers['LayList'].document.forms[0].Res" + num + ".options[" + ItemNo + "].value + ','");
						   strResolutions = strResolutions + strRes;
						   if (strRes=="0,"){
						       //Content
								alert(m_sCont[6] + MediaData[num].CorbisID)  
							    return "stop";
						   }
						   
				  }
				  else{
					       eval("ItemNo = document.layers['LayMedia' + num].document.forms[0].Res" + num + ".selectedIndex");
						   eval("strRes = document.layers['LayMedia' + num].document.forms[0].Res" + num + ".options[" + ItemNo + "].value + ','");
						   strResolutions = strResolutions + strRes;
						   if (strRes=="0,"){
						       //Content
								alert(m_sCont[6] + MediaData[num].CorbisID) // Content
							    return "stop";
				  }
			  }
			}
			return strResolutions;
}
		
function downLoadAll(RFCDUID,ORUID,ORID)
{
		  if ((anySelected()==true) && (getRes()!='stop'))
		  {
				fulfillmentPopup(RFCDUID, ORUID, ORID, getListOfSelectedMedia(), "", getRes());
		  }
		  return;
}
		 
var pDownLoad
var pInc=0
var iFFWindowCount = 1 
function fulfillmentPopup(RFCDUID, ORUID, ORID, ItemList, CorbisID, Sizes)
{             
           //alert( )   
		   //return;
			  //this function ignores features, allows the to be set here
			  var theURL = "/popup/Fulfillment.asp?CP=" + pubCurrentPageNo + "&PS=" + pubPageSize  + "&ItemList=" + ItemList + "&ORUID=" + escape(ORUID) + "&ORID=" + escape(ORID) +  "&Sizes=" + escape(Sizes) + "&rfcduid=" + RFCDUID //"&CorbisID=" + escape(CorbisID) +
			  var newFeatures = "status=no,scrollbars=yes,resizable=no,width=540,height=450";
			  iFFWindowCount+=1
			  pDownLoad = window.open(theURL,"OrderDownLoad" + iFFWindowCount,newFeatures + ",top=" + pInc + ", left=" + pInc);
			  pInc=pInc + 15
			  pDownLoad.focus()
}

function getListOfSelectedMedia()
{
	  var sReturn="";
	  
	  for (var i=0; i <= MediaData.length-1; i++){
			 if (MediaData[i].sel == 1) {
			    sReturn+= i + "," 
			}
	   }
	   return sReturn;
}
		
		
function setPageLoaded(){
			  isPageLoaded = true;
}
		
		
function getFieldObject(sCntr)
{  var frmFld
   if(sCntr=="Folders"){frmFld = document.doActionFormData.InLightbox}
   else if(sCntr=="Cart"){frmFld = document.doActionFormData.InCart}
   return frmFld;
}
		
function addToInList(sID, sCntr) {

	var frmFld
	var tmp
	
	frmFld=getFieldObject(sCntr);
	if(!frmFld){return};
	
	tmp = frmFld.value;
	tmp = tmp.replace("*","");

	if (tmp.indexOf(sID) == -1) {
		tmp	+= sID + ",";
		frmFld.value = tmp;
	}
	
}

function removeFromInList(sID, sCntr)
{
    var frmFld
	
	frmFld=getFieldObject(sCntr);
	if(!frmFld){return};
	
	var imgIDs = frmFld.value.split(",")
	var strImgIDs = frmFld.value
	
	if (imgIDs.length > 0)
	{
	  strImgIDs = "," + strImgIDs
	  strImgIDs=strImgIDs.replace("," + sID + ",",",")
	  strImgIDs=strImgIDs.slice(1)
	  if (strImgIDs=="")
	  {
	    frmFld.value = "*"
	  }
	  else
	  {
	    frmFld.value = strImgIDs;
	  }
	  
	}
}

function addToSelectionList(num)
{
    var sID = MediaData[num].mediaID
    var sUID = MediaData[num].mediaUID
    var sCorbisID = MediaData[num].CorbisID
    var sQP = MediaData[num].QP
    
	var frm = document.doActionFormData
	var tmp = frm.selectionList.value
	if (tmp.indexOf(sID) == -1) {
		tmp	+= sID + ",";
		
		frm.selectionList.value = tmp;
		frm.selectionUIDList.value = sUID+ ",";
		frm.selectionCorbisIDList.value += sCorbisID + ",";
		frm.selectionQuickPurchase.value += sQP + ",";
				
		totalSelectedImages++;
	}
	
}

function removeFromSelectionList(num)
{
	var sID = MediaData[num].mediaID
    var sUID = MediaData[num].mediaUID
    var sCorbisID = MediaData[num].CorbisID
    var sQP = MediaData[num].QP
    
	var frm = document.doActionFormData
		
	var imgIDs = frm.selectionList.value.split(",")
	
	var strImgIDs = frm.selectionList.value
	var strImgUIDs = frm.selectionUIDList.value
	var strCorbisIDs = frm.selectionCorbisIDList.value
	var strQuickPur = frm.selectionQuickPurchase.value
	
	if (imgIDs.length > 0) {
	
		strImgIDs = "," + strImgIDs
		strImgUIDs = "," + strImgUIDs
		strCorbisIDs = "," + strCorbisIDs
		strQuickPur = "," + strQuickPur
		
		strImgIDs=strImgIDs.replace("," + sID + ",",",")
		strImgUIDs=strImgUIDs.replace("," + sUID + ",",",")
		strCorbisIDs=strCorbisIDs.replace("," + sCorbisID + ",",",") 
		strQuickPur=strQuickPur.replace("," + sQP + ",",",")
		
		strImgIDs=strImgIDs.slice(1)
		strImgUIDs=strImgUIDs.slice(1)
		strCorbisIDs=strCorbisIDs.slice(1)
		strQuickPur=strQuickPur.slice(1)
		
		frm.selectionList.value = strImgIDs;
		frm.selectionUIDList.value = strImgUIDs;
		frm.selectionCorbisIDList.value = strCorbisIDs;
		frm.selectionQuickPurchase.value = strQuickPur;
		
		totalSelectedImages--;
	}

}

function clearSelectionList()
{

	if (document.doActionFormData) {
			var frm = document.doActionFormData
			
			frm.selectionList.value = '';
			frm.selectionCorbisIDList.value = '';
			frm.selectionQuickPurchase.value = '';
			
			if (document.frmViewModeForm) {
			var frm2 = document.frmViewModeForm

			frm2.selectionCorbisIDList.value = '';
			frm2.selectionList.value = '';
			frm2.selectionQuickPurchase.value = '';
			}
			
		totalSelectedImages = 0;
		
	}
}

function anySelected()
{
				   if (itemsSelectedCount() == 0){
				     alert(m_sCont[7]);
					 return false;
				   }
				   else if (itemsSelectedCount() > m_DownloadLimit) {
				     alert(m_sCont[8]);
				      return false;
				   }
				   else{
				      return true;
				   }
}
				
function doAuthorSearch(sName)
{
	        window.opener.window.location.href = "/search/searchresults.asp?" + sName
}

function setPageSize(iPageSize)
{
		  document.doActionFormData.PageSize.value = iPageSize
}

function reloadPreviewPane()
{
  parent.frames[1].location.reload()
}
		  function ExpandViewImageSet(bExpanded)
		  {
			if (bExpanded) 
			{
			  document.doActionFormData.ImgSetExpand.value = "false";
			}
			else
			{
			  document.doActionFormData.ImgSetExpand.value = "true";
			}
			document.doActionFormData.submit();
			
			
}

function loadVisibleDiv()
{
      

}

function addNote(sNote,sItemType,sUID)
{
	document.doActionFormData.Note.value = sNote;         // folder or foldermedia
	document.doActionFormData.NoteType.value = sItemType; // note contents
	document.doActionFormData.NoteUID.value = sUID;       // UID for media or folder
	document.doActionFormData.submit();
}
