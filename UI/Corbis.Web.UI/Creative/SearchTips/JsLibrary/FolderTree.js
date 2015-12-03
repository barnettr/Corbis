/******************************************************************************/
/* FolderTree.js                                                              */
/* Creator: Dwayne Johnson                                                    */
/* Original Date: 8/2000                                                      */
/* Description                                                                */
/*   This file houses the functions for displaying and navigating the folder  */
/*   tree.  The folder tree works in conjuction with FolderTreeScript.inc to  */
/*   create "windows explorer-like" functionality.							  */
/*													                          */
/*   Based on the FolderXML returned from the FolderAdapter, various          */
/*   properties of "folder" objects are populated.  The various properties    */
/*   determines how the tree is displayed. (ex. what icons to use, readonly   */
/*   access, etc.)	                                                          */
/*																			  */
/******************************************************************************/
  
 var iNetscapeOffSet = 0;

 //cache images 
 var iconShared = new Image();
 var iconPlus = new Image();
 var iconMinus = new Image();
 var iconBlank = new Image();
 var m_iLeft;
 
 
 /*********** Scroll Box Scripts**************/
 var iIntervalID;
    function scrollDown()
    {
      
      if (document.all)
      {
        var y;
       
                
        var sCode = "y = document.all['divScroll'].offsetTop;" +
                    "if(y + document.all['divScroll'].offsetHeight+150 <  document.all['foldertree'].offsetTop) document.all['divScroll'].style.top =  y + 'px'; else " +
                    "document.all['divScroll'].style.top = (y-20) + 'px';"

        iIntervalID = window.setInterval(sCode, 100);
        
       }
       else if(document.getElementById)
       {
        
        var y;
        var bottom = document.getElementById('divScroll').offsetHeight + 200;
        var sCode = "y = document.getElementById('divScroll').offsetTop;" +
                    "if(y-20 < -" + bottom + ") document.getElementById('divScroll').style.top = -" + bottom + "+ 'px'; else " +
                    "document.getElementById('divScroll').style.top = (y-20) + 'px';"

        iIntervalID = window.setInterval(sCode, 100);
      
      }
      
        else

        {
          
            var y;
            var bottom = document.layers['divContainer'].document.layers['divScroll'].clip.height - 20;
            sCode = "y = document.layers['divContainer'].document.layers['divScroll'].pageY;" +  
                    "document.layers['divContainer'].document.layers['divScroll'].visibility = 'hide';" +
                    "if(y-20 > -" + bottom + ")" +             
                    "document.layers['divContainer'].document.layers['divScroll'].moveBy(0,-20);" +
                    "document.layers['divContainer'].document.layers['divScroll'].visibility = 'show';"
            iIntervalID = window.setInterval(sCode, 100);
        }
      
    }
  
    function scrollUp()
    {
    
     
       if (document.all)
      {
        
        var y;
        var sCode = "y = document.all['divScroll'].offsetTop;" +
                    "if(y+20 >= 0) document.all['divScroll'].style.top = '0px'; else " +
                    "document.all['divScroll'].style.top = (y+20) + 'px';"
         
          
        iIntervalID = window.setInterval(sCode, 100);
       }
       else if(document.getElementById)
      {
        var y;
        var sCode = "y = document.getElementById('divScroll').offsetTop;" +
                    "if(y+20 >= 0) document.getElementById('divScroll').style.top = '0px'; else " +
                    "document.getElementById('divScroll').style.top = (y+20) + 'px';"
         
          
        iIntervalID = window.setInterval(sCode, 100);
      }
       else       
       {
             var y;
            
            sCode = "y = document.layers['divContainer'].document.layers['divScroll'].pageY;" +  
                    "document.layers['divContainer'].document.layers['divScroll'].visibility = 'hide';" +
                    "if(y-20 >= 0)document.layers['divContainer'].document.layers['divScroll'].moveTo(0,0);else " +             
                    "document.layers['divContainer'].document.layers['divScroll'].moveBy(0,20);" +
                    "document.layers['divContainer'].document.layers['divScroll'].visibility = 'show';"
            iIntervalID = window.setInterval(sCode, 100);
       
       }
     
    }
    
 /*********** Scroll Box Scripts**************/  

function folder() {
/********************************************************************/
/* Description														*/
/*     Constructor for the folder object                            */
/*																	*/
/* Parameters													    */		
/*		None														*/
/* Returns                                                          */
/*		Nothing														*/
/********************************************************************/

	//properties
	
	this.id = -1;							//unique folder id/same as index in allfolders array
	this.divID = '';						
	this.strLabel = '';                        //the folder's visible label
	this.isExpanded = false;
	this.isShared = false;
	this.children = 0
	this.level = -1;
	this.folderType = null;
	this.isSelected = false;
	this.parentID = -1;                              //parent id
	this.fullPath = '';
	this.readOnly = false;
	this.c1ParentFolderName = ''


	// JDD Corbis One specific properties
	this.c1FolderName = "";
	this.c1MemberUUID = "";
	this.c1FolderUUID = "";
	this.c1ParentFolderUUID = "";
	this.c1ParentName = "";
	this.c1FolderChangeDate = "";
	this.c1FolderCreateDate = "";
	this.c1FolderOwnerName = "";
	this.c1ClientName = "";
	this.c1SharedOut = false;     //is folder shared out to someone

}



function toggleClick(iFolderID) {
	
	var iFolderCount = 0
	
	 var  folderIdent = "tree_" + iFolderID + "_" + iFolderCount+ "_divFolder";
	 var toggleImageId = "tree_" + iFolderID + "_imgToggle";
	if(document.all) 
	{
		
		if(document.all[folderIdent].style.display == 'none')
		{
			//change toggle icon
			
			document.all[toggleImageId].src = '/images/arrow_down_tr.gif'
			
			
			while(document.all[folderIdent])
			{		
					document.all[folderIdent].style.display = 'block';
					iFolderCount++
					folderIdent = "tree_" + iFolderID + "_" + iFolderCount+ "_divFolder";
			}	
		}
		
		else //display == block

		{
			collapseFolders(iFolderID)
		}
	} 
	else if(document.getElementById)
	{	
		if(document.getElementById(folderIdent).style.display == 'none')
		{
			//change toggle icon
			document.getElementById(toggleImageId).src = '/images/arrow_down_tr.gif'
			
			
			while(document.getElementById(folderIdent))
			{				
					
					document.getElementById(folderIdent).style.display = 'block';
					iFolderCount++
					folderIdent = "tree_" + iFolderID + "_" + iFolderCount+ "_divFolder";
			}	
		}
		
		else //display == block

		{
			collapseFolders(iFolderID)
		}
	}
	else //netscape
	{
	    var obContainer = document.divContainer.document.divScroll
	    
		//if children are not visible then folder is collapsed
		if(obContainer.document.layers['divFolder' + iFolderID + '_0'].visibility == 'hide')
		{
			eval("obContainer.document.divFolder" + iFolderID + ".document.imgToggle" + iFolderID + ".src = iconMinus.src") 
			
			var offSet = showChildren("divFolder" + iFolderID)
			//move folder starting with the next folder on this level
			
			if(iFolderID.indexOf('_') != -1)
			{
				
				var iNextFolderNumber = parseInt(iFolderID.substr(iFolderID.lastIndexOf('_') + 1)) + 1
				
				iFolderID = iFolderID.substr(0,iFolderID.lastIndexOf('_') + 1) + iNextFolderNumber
				
				moveFolders(iFolderID.toString(), offSet)
				iFolderID = parseInt(iFolderID.substr(0, iFolderID.indexOf('_'))) + 1
			}
			else
			{
				iFolderID = parseInt(iFolderID) + 1
			}
			
			moveFolders(iFolderID.toString(), offSet)
		}
		else //children are visible
		{
		    
			eval("obContainer.document.divFolder" + iFolderID + ".document.imgToggle" + iFolderID + ".src = iconPlus.src") 
			//reset offset	
			iNetscapeOffSet = 0
			var offSet = hideChildren("divFolder" + iFolderID)
			
			if(iFolderID.indexOf('_') != -1)
			{
				
				var iNextFolderNumber = parseInt(iFolderID.substr(iFolderID.lastIndexOf('_') + 1)) + 1
				
				iFolderID = iFolderID.substr(0,iFolderID.lastIndexOf('_') + 1) + iNextFolderNumber
				moveFolders(iFolderID.toString(), -offSet)
				iFolderID = parseInt(iFolderID.substr(0, iFolderID.indexOf('_'))) + 1
			}
			else
			{
				iFolderID = parseInt(iFolderID) + 1
			}

			moveFolders(iFolderID.toString(), -offSet)

		}
		
	} //end if document.all

}

function showChildren(sFolderID)
{

    var obContainer = document.divContainer.document.divScroll
	var newTop = obContainer.document.layers[sFolderID].top
	var newLeft = m_iLeft
	var iOffSet = 0
	newTop += 17;
	
	var iFolderCount = 0

	while(obContainer.document.layers[sFolderID + "_" + iFolderCount])	
	{
		obContainer.document.layers[sFolderID + "_" + iFolderCount].top = newTop
		obContainer.document.layers[sFolderID + "_" + iFolderCount].left = newLeft
		obContainer.document.layers[sFolderID + "_" + iFolderCount].visibility = 'show'
	
		newTop += 17;
		iOffSet += 17
		iFolderCount++;
	}

	//return the amount to move other folders by
	
	return iOffSet;

}



function hideChildren(sFolderID)
{

	
	var iFolderCount = 0
	var iFolderID = sFolderID.substr(9)
	var obContainer = document.divContainer.document.divScroll
	eval("obContainer.document." + sFolderID + ".document.imgToggle" + iFolderID + ".src = iconPlus.src") 
	while(obContainer.document.layers[sFolderID + "_" + iFolderCount])	
	{
		
		obContainer.document.layers[sFolderID + "_" + iFolderCount].visibility = 'hide'
				
		iNetscapeOffSet += 17

		if(obContainer.document.layers[sFolderID + "_" + iFolderCount + "_0"])
		{
			if(obContainer.document.layers[sFolderID + "_" + iFolderCount + "_0"].visibility == 'show')
			{
				hideChildren(sFolderID + "_" + iFolderCount) 
			}

		}
		
		iFolderCount++;
	}
	
	
	//return the amount to move other folders by
	return iNetscapeOffSet;

}

function moveFolders(sFolderID, iOffSet)
{

	var iFolderCount
	var sParentFolderID
	var obContainer = document.divContainer.document.divScroll
	if(sFolderID.indexOf('_') != -1)
	{
		iFolderCount = parseInt(sFolderID.substr(sFolderID.lastIndexOf('_') + 1))
		sParentFolderID = sFolderID.substr(0,sFolderID.lastIndexOf('_') + 1)
	}
	else
	{
		iFolderCount = sFolderID
		sParentFolderID = ''
	}
    if(obContainer.document.layers['divShowTree'])
		{
		    obContainer.document.layers['divShowTree'].moveBy(0, iOffSet)
		}	
	while(obContainer.document.layers['divFolder' + sParentFolderID + iFolderCount])
	{
		//move owner labels
		
		if(obContainer.document.layers['divLabel' + iFolderCount])
		{	
			obContainer.document.layers['divLabel' + iFolderCount].moveBy(0, iOffSet)
			
		}
		
		
		if(obContainer.document.layers['divSharedLabel' + iFolderCount])
		{	
			obContainer.document.layers['divSharedLabel' + iFolderCount].moveBy(0, iOffSet)
		}

		obContainer.document.layers['divFolder' + sParentFolderID + iFolderCount].visibility = 'show'
		obContainer.document.layers['divFolder' + sParentFolderID + iFolderCount].moveBy(0, iOffSet)
		
		if(obContainer.document.layers['divFolder' + sParentFolderID + iFolderCount + '_0'])
		{
			if(obContainer.document.layers['divFolder' + sParentFolderID + iFolderCount + '_0'].visibility == 'show') 
			{
				moveFolders(sParentFolderID + iFolderCount + '_0', iOffSet)
			}
		}		
		
		iFolderCount++

		
	}
	


}

function collapseFolders(sFolderID)
{
	var iFolderCount = 0
	var toggleImageId = "tree_" + sFolderID + "_imgToggle";
	
	if(document.all)
	{
	    
		//change toggle icon
		document.all[toggleImageId].src = '/images/arrow_right_tr.gif'
		
		while(document.all['tree_' + sFolderID + "_" + iFolderCount + "_divFolder"])
		{		
				document.all['tree_' + sFolderID + "_" + iFolderCount+ "_divFolder"].style.display = 'none';
							
				if(document.all['tree_' + sFolderID + "_" + iFolderCount + "_0_divFolder"])
				{				
					collapseFolders(sFolderID + "_" + iFolderCount)
				}

				iFolderCount++
		}	
	} 
	else if(document.getElementById)
	{
		//change toggle icon
		document.getElementById(toggleImageId).src = '/images/arrow_right_tr.gif'
		
		while(document.getElementById('tree_' + sFolderID + "_" + iFolderCount+ "_divFolder"))
		{		
				document.getElementById('tree_' + sFolderID + "_" + iFolderCount+ "_divFolder").style.display = 'none';
							
				if(document.getElementById('tree_' + sFolderID + "_" + iFolderCount + "_0_divFolder"))
				{				
					collapseFolders(sFolderID + "_" + iFolderCount)
				}

				iFolderCount++
		}	
	}

}

function expandParentFolders(sSelectedFolderID)
{
	
	var iFolderCount = 0;
	
	if(sSelectedFolderID.indexOf('_') != -1)
	{
		var startID =  sSelectedFolderID.substr(0,sSelectedFolderID.lastIndexOf('_'));
		if(document.all) {
			document.all['imgToggle' + startID].src = '/images/arrow_down_tr.gif'

			while(document.all['divFolder' + startID + "_" + iFolderCount])
			{				
					
				document.all['divFolder' + startID + "_" + iFolderCount].style.display = 'block';
				iFolderCount++

			}	
		
			if(startID.lastIndexOf('_') != -1)
			{
				expandParentFolders(startID);
			}
		}
		else if(document.getElementById)
		{
			document.getElementById('imgToggle' + startID).src = '/images/arrow_down_tr.gif'

			while(document.getElementById('divFolder' + startID + "_" + iFolderCount))
			{				
					
				document.getElementById('divFolder' + startID + "_" + iFolderCount).style.display = 'block';
				iFolderCount++

			}	
		
			if(startID.lastIndexOf('_') != -1)
			{
				expandParentFolders(startID);
			}
		}
		else
		{
				
			
			var sFolderID = sSelectedFolderID.substr(0,sSelectedFolderID.lastIndexOf('_'))
			
			while(sFolderID.indexOf('_') != -1)
			{
				//show parent folders
				toggleClick(sFolderID);				
				sFolderID = sFolderID.substr(0,sFolderID.lastIndexOf('_'))
				
			}	
				
			toggleClick(sFolderID);
			
		}
		
		
	}
	
}