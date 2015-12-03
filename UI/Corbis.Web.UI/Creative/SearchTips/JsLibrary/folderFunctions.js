if (!HttpUrl) {
	var HttpUrl = "http://" + window.location.host + "/";
}
if (!HttpsUrl) {
	var HttpsUrl = "https://" + window.location.host + "/";
}

function popUp(sURL, action) {
/********************************************************************/
/* Description														*/
/*     This function will return the full path for a given folder ID*/
/*																	*/
/* Parameters													    */		
/*    Name          Type          Description                       */
/*    -----------   ------------  --------------------------------  */
/*    sURL          String        the URL to be opened in pop up    */
/*    action        String        'add' or 'update' folder          */
/********************************************************************/
 	var pFolderWindow;
 	
	if (action == 'edit') 
	{
	
		if(activeFolderUID == '')
		 {
		
			alert(folderMessages[0]);
			
			return;
		
		
		} 
		
		
	}
	
	if (document.all) {
		var screenpos = 'left=230,top=0';
	} else {
		var screenpos = 'screenX=230,screeny=0';
	}

	if (action == 'edit') {	
		
		if(selectedFolder.level == 1) {
			var parentName = 'My Folders'
		} else {
			var parentName = selectedFolder.c1ParentFolderName;
		}
		
		
		folderpopup = window.open(sURL + "?fdid=" + selectedFolder.c1FolderUUID + '&actn=' + action + '&fname='+ escape(selectedFolder.c1FolderName) + '&pfname=' + escape(parentName)  , 'folderpopup', 'status=yes,scrollbars=yes,resizable=yes,width=595,height=500,' + screenpos);
		//folderpopup.focus();
		
	} else if(action == 'add') {
	
		pFolderWindow = window.open('/myfolders/addToMyFoldersPopup.asp?mode=newonly&area=folders' , 'addfolderpopup', 'status=yes,scrollbars=yes,resizable=yes,width=320,height=475,' + screenpos);
		pFolderWindow.focus();
	}
	

 
}

var newFolderCount = 0;
var newFolderName = '';
var chknum;

function createFolderName(fuid, nm){
/*****************************************************************/
/*  Description:												 */
/*     check if child folder exists with the same name           */
/*****************************************************************/
	newFolderName = nm;
	
	for (var i=0;i < allfolders.length;i++) {

		if(allfolders[i].c1FolderUUID == fuid) {
		
			if( allfolders[i].children.length > 0) {
				for (var j=0;j < allfolders[i].children.length;j++) {
					
					if (allfolders[allfolders[i].children[j]].c1FolderName.toUpperCase() == nm.toUpperCase()) {
																	
						chknum = nm.indexOf(" (" + newFolderCount + ")");
						
						newFolderCount++
						
						if(chknum != -1) {
							newFolderName = nm.substr(0, chknum) + " (" + newFolderCount + ")";
						
						} else {										
						
							newFolderName = nm + " (" + newFolderCount + ")";
							
						}
					
						
						//check new name
						createFolderName(fuid, newFolderName)
					}
				}
			
			}
			
			return newFolderName;			
		}
		
	}
	
	return nm;
	
	
}


function emailFolder() {

 if (selectedFolder == null) {
   
		selectedFolder = allfolders[0]
   
   }

 if(selectedFolder.id == 0) {
		
			alert('folderMessages[0]');
			
			return;
		
 }

 
 
		
 popUp('/myfolders/emailFolderPopup.asp', 'edit');
}


function createOrderFromFolder(totImages, s, mode, pageNumber) 
{
	var mediaids;

	var forms = document.getElementsByTagName("form");
	var formCOFF = forms[0];
		
	if(doneLoading) 
	{
		
		if(!canCheckout) 
		{
			var msg = folderMessages[1];
			alert(msg);
			return;
		}
		
		if (mode == 'all') 
		{
			totalSelectedImages = totImages;
			mdcnt = totImages;
			mediaids = '';
			formCOFF.action = HttpsUrl + 'shoppingcart/validateitems.aspx?fdid=' + activeFolderUID;
		} 
		else
		{
			formCOFF.action = HttpUrl + 'shoppingcart/CoffGeorge.aspx?qsPageNo=' + pageNumber + '&fdid=' + activeFolderUID;
		}
		
		if(activeFolderUID != '')
		{
			formCOFF.target = '_top';
			document.getElementById("fdid").value = activeFolderUID;
			document.getElementById("PageNumber").value = pageNumber;
			formCOFF.submit();  
		}
		else
		{
			var msg = folderMessages[0];
			alert(msg);
			return;
		}
	} 
	else 
	{  //!doneLoading
		return;
	}
}




function quickPurchase(iMediaID, sCorbisID) 
{
	var mediaids = '';
	var mdcnt = 0;
	var qp;
	var message = '';
	var badQPCount = 0;
	var iMediaCount = 0;
	var iQPLimit = 0;
	var bNoImages=false; // IR9912
	
	if (document.all) 
		var screenpos = 'left=230,top=0';
	else 
		var screenpos = 'screenX=230,screeny=0';
	
	//Selection across pages script
	if (window.MediaData) 
	{			
		if (iMediaID != '')		//signle image purchase
		{
			mdcnt++
			mediaids = iMediaID;
			iMediaCount = 1;
		} 
		else					//multiple images purchase
		{	
			if(document.doActionFormData.selectionList.value != '') 
			{
				
		 		var tmp = document.doActionFormData.selectionList.value.split(",");
				var tmpQP = document.doActionFormData.selectionQuickPurchase.value.split(",");
					
				for(var j = 0; j < tmp.length - 1; j++) 
				{
					qp = tmpQP[j]
					if (qp == 1) 
					{
						mdcnt++
						mediaids += tmp[j] + ","
						iMediaCount = iMediaCount + 1;
					}
					else
						badQPCount++;
				}			
			}
			else 
			{
			   bNoImages=true; // IR9912
			}
			
		}

		var cid;
		
		if (selectedFolder) 
			cid = selectedFolder.c1FolderUUID	
		else 
			cid = '';

		if(badQPCount > 0)
			alert(folderMessages[2]);
		else if(iMediaCount > 20)
		{
			var msg = folderMessages[3]
			msg = msg.replace(/\$MediaCount/i, iMediaCount)
			msg = msg.replace(/\$Number_Of_Images/i, iMediaCount - 20)
			alert(msg)
		}
		else if (bNoImages)
		{
		   alert(folderMessages[4]) // IR9912
		}
		else
		{
			folderpopup = window.open("/shoppingcart/QPSelectSize.asp?fdid=" + cid + "&mdid=" + escape(mediaids) + "&fake=", 'folderpopup', 'status=yes,scrollbars=yes,resizable=yes,width=600,height=500,' + screenpos);
			folderpopup.focus();
		}		
	}//no window media	
}

function requestApproval(sMediaID, sPaneArea,sMediaUID) {
    
	if (document.all) {
		var screenpos = 'left=10,top=0';
	} else {
		var screenpos = 'screenX=10,screeny=0';
	}
	
	var queryString = "";
	
	if(sMediaUID != "")
	{
		queryString = "?mediauids=" + sMediaUID
	}
	
	
	var folderpopup = window.open("/popup/requestApproval.aspx" + queryString, 'folderpopup', 'status=yes,scrollbars=yes,resizable=yes,width=770,height=600,' + screenpos);

	folderpopup.focus();

}

