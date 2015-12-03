var isIE5Mac=false;
if ((navigator.appVersion.indexOf("MSIE 5",1)>0) && (navigator.appVersion.indexOf("Macintosh",1)>0)){
   isIE5Mac=true;
}

function getAllMediaUIDs()
{
		var sIDs="";
		for (var i=0; i <= MediaData.length-1; i++)
		{
			sIDs += MediaData[i].mediaUID + ",";
		} 
		return sIDs;  
}
function getAllMediaIDs()
{
		return getAllMediaUIDs();  
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
function setPageSize(iPageSize)
{
	 document.doActionFormData.PageSize.value = iPageSize
}
function checkPageNumber(f,iTotalPages)
{
	 var maxPageNumber = iTotalPages;
	 if ((f.PageNumber.value > maxPageNumber) || (f.PageNumber.value <= 0) || isNaN(parseInt(f.PageNumber.value)))
	 {
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
			        
			        
			           showTab = "QuickPic"
			        
			        
			       
			        parent.frames[1].location = '/previewpane/previewpanes.aspx?tab=' + showTab + '&actn=addcart&area=' + sContainer + '&fdid=' + pubDLBXUID + '&mediauids=' + sMediaUID + '&corbisid=' + MediaData[iSeqNum].CorbisID + '&mainpage=' + sContainer
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
		alert(e.message);
	
	}
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
	
	if(!frmFld){
	 
	return};
	
	
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
function AddAllImagesToDefaultLightbox()
{
			  var sMediaIDs="";
			  var sChangedAtDates="";
			  
			  for (var i=0; i <= MediaData.length-1; i++)
			  {
				  sMediaIDs +=  MediaData[i].mediaUID + ","
				  sChangedAtDates += MediaData[i].ChangedAt + ","
				  updateIndicators(MediaData[i].mediaID,MediaData[i].mediaUID, "Folders", true)
			  } 
			
              parent.frames[1].addToLightbox("",sMediaIDs, "", false);
			
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

function refreshIndicators(iSeqNum)
{
	    if (pubIsImageSets){return;}
		updateDivHighlight(iSeqNum);
}

function getSequenceNumber(sMediaUID)
{
			
			for (var i=0; i <= MediaData.length-1; i++){
				if (MediaData[i].mediaUID.indexOf(sMediaUID)!=-1){
				   return i; 
				}
			} 
			return -1;  
}

function updateIndicators(sMediaID, sMediaUID, sContainer, bStatus)	
{			
			sContainer = sContainer.toLowerCase();
  			
  			if (pubIsImageSets){return;}
			var iSeqNum = getSequenceNumber(sMediaUID.substring(0,38));
 
			if (iSeqNum==-1) //MediaID not on this page
			{
				return;
			}
			
			if (bStatus)
			{ 
				addToInList(sMediaUID.substring(0,38), sContainer)
			}
			else
			{
				removeFromInList(sMediaUID.substring(0,38), sContainer)
			}
			
		   if (sContainer=="folders")
		   {
		      MediaData[iSeqNum].InLBX = bStatus
		   }
		   if (sContainer=="cart")
		   {
		      MediaData[iSeqNum].InCART = bStatus
		   }
		   if (sContainer=="pvquickpic")
		   {  
			  MediaData[iSeqNum].InQPPreview = bStatus
		   }
			
			updateDivHighlight(iSeqNum);
			
			return;
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
function updateIndicators_old(sMediaID, sMediaUID, sContainer, bStatus)
{
		   if (pubIsImageSets){return;}
		   var iSeqNum = getSequenceNumber(sMediaUID);
		   
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
		   
		   updateDivHighlight(iSeqNum);
		   
		   
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
	
	      
