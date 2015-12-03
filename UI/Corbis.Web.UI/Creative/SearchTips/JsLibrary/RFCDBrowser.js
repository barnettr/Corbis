function UserAgentTest()
{
	var sUsrAgt		=	navigator.userAgent.toLowerCase();
	var iAppVer		=	parseInt( navigator.appVersion );

	var bIs_MacOS		=	sUsrAgt.indexOf( 'mac'     )!=-1;
	var bIs_Mozil		=	sUsrAgt.indexOf( 'mozilla' )!=-1;
	var bIs_Opera		=	sUsrAgt.indexOf( 'opera'   )!=-1;

	var bIs_Spoof		=	bIs_Opera || sUsrAgt.indexOf( 'spoofer' )!=-1;
	var bNo_Mozil		=	bIs_Spoof || sUsrAgt.indexOf( 'compatible' )!=-1;
	var bNo_Gecko		=	bIs_Spoof || sUsrAgt.indexOf( 'like gecko' )!=-1;

	this.Type_Ie4x		=	!bIs_Spoof && sUsrAgt.indexOf( 'msie 4'   )!=-1;
	this.Type_Ie5x		=	!bIs_Spoof && sUsrAgt.indexOf( 'msie 5'   )!=-1;

	this.Type_Ie50		=	!bIs_Spoof && sUsrAgt.indexOf( 'msie 5.0' )!=-1;
	this.Type_Ie55		=	!bIs_Spoof && sUsrAgt.indexOf( 'msie 5.5' )!=-1;

	this.Type_Ie6Up		=	!this.Type_Ie4x && !this.Type_Ie50 && !this.Type_Ie55 &&
					!bIs_Spoof && iAppVer > 3 && sUsrAgt.indexOf( 'msie' )!=-1;

	this.Type_Ns60		=	iAppVer == 5 && !bIs_Spoof && sUsrAgt.indexOf( 'netscape/6' )!=-1;
	this.Type_Ns40		=	iAppVer == 4 &&  bIs_Mozil && !bNo_Mozil;
	this.Type_Ns7Up		=	iAppVer >= 5 &&  bIs_Mozil && !bNo_Mozil && !this.Type_Ns60;

	this.Type_Ns7Mac	=	bIs_MacOS && this.Type_Ns7Up;

	this.Type_Op50		=	sUsrAgt.indexOf( 'opera 5' )!=-1 || sUsrAgt.indexOf( 'opera/5' )!=-1;
	this.Type_Op60		=	sUsrAgt.indexOf( 'opera 6' )!=-1 || sUsrAgt.indexOf( 'opera/6' )!=-1;
	this.Type_Op70		=	sUsrAgt.indexOf( 'opera 7' )!=-1 || sUsrAgt.indexOf( 'opera/7' )!=-1;

	this.Type_Gecko		=	!bIs_Spoof && !bNo_Gecko && sUsrAgt.indexOf( 'gecko'   )!=-1;
	this.Type_Safari	=	sUsrAgt.indexOf( 'safari'  )!=-1;

	this.Type_W3cDom	=	this.Type_Ie55  || this.Type_Ie6Up || this.Type_Ns60 || this.Type_Ns7Up ||
					this.Type_Gecko || this.Type_Op70;

	this.Type_Unkn		=	!this.Type_Ns40 && !this.Type_Ns60 && !this.Type_Ns7Up && !this.Type_Ie4x   && 
					!this.Type_Ie50 && !this.Type_Ie55 && !this.Type_Ie6Up && !this.Type_Safari && 
					!this.Type_Op50 && !this.Type_Op60 && !this.Type_Op70  && !this.Type_Gecko  &&
					!this.Type_Ie5x;

	this.Type_Down		=	!this.Type_Unkn && !( this.Type_W3cDom || this.Type_Ie4x || this.Type_Ie5x );


}

//' ====================================================================================
//' CdListItem - cd list-item object constructor.
//' 
//' ====================================================================================

function CdListItem( sCdTitle, sCdPrice, iCatIndex, sCdID )
{
	//' ----------------------------------------------------------------------------
	//' 

	this.ID    = sCdID;
	this.Title = sCdTitle;
	this.Price = sCdPrice;
	this.Index = iCatIndex;
}

//' ====================================================================================
//' IsJustBlank - Returns true if the string only contains white space or no characters
//' 
//' ====================================================================================
function IsJustBlank(sString)
{
  if (sString=="") return false;
  for (var i=0; i <= sString.length-1; i++)
  {
    if (sString.substring(i,1)!= " ")
    {
      return false;
    }
  }
  return true; 
}

//' ====================================================================================
//' GetPageByRfCdID - navigate to RfCd information page ( using RfCd identifier )
//' 
//' ====================================================================================
function GetPageByRfCdID( sCdRefID )
{
	//' ----------------------------------------------------------------------------
	//' 

	//alert( "Get Webpage ( CD Info, Order : " + sCdRefID + " )" );
	document.location.href = "/search/searchFrame.aspx?rfcduid=" + sCdRefID;
	return false;
}

//' ====================================================================================
//' BuildListHeader - 
//' 
//' ------------------------------------------------------------------------------------

function BuildListHeader( oMenuNode )
{
	//' ----------------------------------------------------------------------------
	//' 

	if( oMenuNode.ParentNode && oMenuNode.ParentNode.NodeText )
		return BuildListHeader( oMenuNode.ParentNode ) + " &gt; " + oMenuNode.NodeText;

	return oMenuNode.NodeText;
}

//' ====================================================================================
//' PopulateTitlesList - populate cd titles list based upon category/alpha identifiers.
//' 
//' ====================================================================================

function PopulateTitlesList( vCatIndex )
{
	//' ----------------------------------------------------------------------------
	//' render html to the cd titles section.

	RenderTitlesList( g_aCdListItems, vCatIndex );
}

//' ====================================================================================
//' RenderTitlesList : Method - attaches CD-list string arrays to the menu-list object.
//' 
//' ------------------------------------------------------------------------------------

function RenderTitlesList( aCdListItems, vCatIndex )
{
	//' ----------------------------------------------------------------------------
	//' down-level browsers with need to get the currently selected category from
	//' a document.cookie value...

	if( g_oUserAgentTest.Type_Down )
		var vCatIndex = location.hash.replace( /.*subcat=/, "" );
		
	if( vCatIndex == null ) return;

	//' ----------------------------------------------------------------------------
	//' 

	if( g_oUserAgentTest.Type_Down || !( window.AlphaString.length | window.CdCatString.length ) )
	{
		window.AlphaObject = new Array();
		window.CdCatObject = new Array();

		for( var ea =0;ea < aCdListItems.length;ea++ )
		{
			var sCdTitle = new String( aCdListItems[ ea ].Title );
			var iCdAlpha = sCdTitle.toUpperCase().charCodeAt( 0 ) - 65;
			if( iCdAlpha > 25 || iCdAlpha < 0 ) iCdAlpha = 26;

			if( !window.AlphaObject[ iCdAlpha ] )
				window.AlphaObject[ iCdAlpha ] = new Array();

			var oCdAlpha = window.AlphaObject[ iCdAlpha ];
			oCdAlpha[ oCdAlpha.length ] = aCdListItems[ ea ];

			//' alert( aCdListItems[ ea ].Index );

			var iCdIndex = parseInt( aCdListItems[ ea ].Index );
			if( String( iCdIndex ) == "NaN" ) continue;

			if( !window.CdCatObject[ iCdIndex ] )
				window.CdCatObject[ iCdIndex ] = new Array();

			var oCdCatOjb = window.CdCatObject[ iCdIndex ];
			oCdCatOjb[ oCdCatOjb.length ] = aCdListItems[ ea ];
		}
	}

	var sListHeader = new String(); 
	var sCdItemList = new String();
	var iCatIndex   = parseInt( vCatIndex );
	var sCatType    = iCatIndex != Number( vCatIndex ) ? "Alpha" : null;

	if( sCatType == "Alpha" )
	{
		iCatIndex = vCatIndex.toUpperCase().charCodeAt( 0 ) - 65;
		if( iCatIndex > 25 || iCatIndex < 0 ) iCatIndex = 26;
		sCdItemList = new String( "" );
		var aCdList = window.AlphaObject[ iCatIndex ];
		if( aCdList )
		{
			aCdList.sort(SortByTitle);
			sCdItemList = "";
			for( var ea = 0;ea < aCdList.length;ea++ ) {
				sCdItemList += "<A class=csCatLink href=# onclick=\"" +
				"return GetPageByRfCdID( '" + aCdList[ ea ].ID + 
				"' )\">" + aCdList[ ea ].Title; 
				if(!IsJustBlank(aCdList[ ea ].Price))
				{
					sCdItemList += "&#160;(" + aCdList[ ea ].Price + ")" 
				}
				sCdItemList += "</A><BR/>"
			}

			window.AlphaString[ iCatIndex ] = sCdItemList;
		}

		sListHeader = AlphabeticalListingText + " [ " + vCatIndex + " ]";
		window.MenuRoot.CloseMenus();
	}
	else
	{
		if( iCatIndex != Number( vCatIndex ) ) return;
		sCdItemList = new String( NoTitlesText );
		var aCdList = window.CdCatObject[ iCatIndex ];

		if( aCdList )
		{
			sCdItemList = "";
			for( var ea = 0;ea < aCdList.length;ea++ ) {
				sCdItemList += "<A class=csCatLink href=# onclick=\"" +
				"return GetPageByRfCdID( '" + aCdList[ ea ].ID + 
				"' )\">" + aCdList[ ea ].Title;
				
				if (!IsJustBlank(aCdList[ ea ].Price))
				{
					sCdItemList += "&#160;(" + aCdList[ ea ].Price + ")";
				}
				sCdItemList += "</A><BR/>";
			}

			window.CdCatString[ iCatIndex ] = sCdItemList;
		}

		sListHeader = BuildListHeader( window.MenuRoot.NodeItems[ iCatIndex ] );
	}

	if( g_oUserAgentTest.Type_Down )
	{
		document.open( 'text/html' );
		document.write( "<STY" + "LE> #CdCatList { DISPLAY: none } </ST" + "YLE>" );
		document.write( sCdItemList );
		document.close();
	}
	else
	{
		var oListItems        = document.getElementById( "CdCatList"  );
		var oListHeader       = document.getElementById( "CatListHdr" );
		oListItems.innerHTML  = sCdItemList;
		oListHeader.innerHTML = sListHeader;
	}
}

function SortByTitle(a,b) {
    aa = a.Title.toLowerCase();
    bb = b.Title.toLowerCase();
    if (aa==bb) return 0;
    if (aa<bb) return -1;
    return 1;
}

//' ====================================================================================
//' DownLevelMenuReload - 
//' 
//' ====================================================================================

function DownLevelMenuReload( vLinkIndex )
{
	if( g_oUserAgentTest.Type_Ns40 )
	{
		//' ns4 ...
		location.hash = "subcat=" + vLinkIndex;
		location.reload();
	}
	else
	{
		//' opera5, opera6 ...
		var sLocation = location.href.split( "#" )[ 0 ];
		location.replace( sLocation + "#subcat=" + vLinkIndex );
	}
	return false;
}

//' ====================================================================================
//' MenuLink_OnClick - 
//' 
//' ====================================================================================

function MenuLink_OnClick( oMenuLink, vLinkIndex )
{
	//' ----------------------------------------------------------------------------
	//' if down-level, reload the page after changing the document.hash value.

	if( g_oUserAgentTest.Type_Down ) return DownLevelMenuReload( vLinkIndex );

	//' ----------------------------------------------------------------------------
	//' toggle the selected sub-category div.

	var oMenuRoot  = window.MenuRoot;
	var oSubCatDiv = document.getElementById( "SubCatItems_" + vLinkIndex );

	if( oSubCatDiv && oMenuRoot.IsDomDoc )
		oSubCatDiv.className = 
		oSubCatDiv.className == "csCatItemShut" ? "csCatItemOpen" : "csCatItemShut";

	//' ----------------------------------------------------------------------------
	//' run script associated with selected link.
	oMenuRoot.NodeAction( vLinkIndex );

	//' ----------------------------------------------------------------------------
	//' hilight the selected category link.

	if( oMenuRoot.Selected ) oMenuRoot.Selected.className = "csCatLink";
	oMenuLink.className = "csSelLink";
	oMenuRoot.Selected  = oMenuLink;

	return false;
}

//' ====================================================================================
//' RenderAlpabeticalLinks.
//' 
//' ====================================================================================

function RenderAlpabeticalLinks()
{
	document.open( 'text/html' );
	for( var iAlpha = 65; iAlpha < 91; iAlpha++ )
	{
		var sAlpha = String.fromCharCode( iAlpha );
		document.write( 
			"<td><a class=\"csCatLink\" href='#subcat=" + sAlpha + "' onclick=\"" +
			"return MenuLink_OnClick( this, '" + sAlpha + "' )\">" + sAlpha + "</a>&nbsp;&nbsp</td>" );
	}
	document.close();
}

//' ====================================================================================
//' MakeMenuList : Object - extends the menu-list root node to the browser DOM.
//' 
//' ====================================================================================

function MakeMenuList()
{
	var oMenuRoot = new MakeMenuItem();

	//' ----------------------------------------------------------------------------
	//' expand window object to include arrays used in organizing the CD catalog.

	window.AlphaString = new Array();
	window.CdCatString = new Array();

	//' ----------------------------------------------------------------------------
	//' set method to attach CD-list arrays to the menu-list object.

	oMenuRoot.NodeAction = PopulateTitlesList;

	//' TODO -----------------------------------------------------------------------
	//' menu-state works, but is still under construction...

	if( g_oUserAgentTest.Type_Down && location.hash.length )
		oMenuRoot.MenuState = location.hash.replace( /.*subcat=/, "" );

	//' ----------------------------------------------------------------------------
	//' return the menu object.

	return oMenuRoot;
}

//' var STR_00 = "There are no titles in this category."

//' ====================================================================================
//' RenderMenuHTML - method to generate the client menu-list HTML.
//' 
//' ------------------------------------------------------------------------------------

function RenderMenuHTML()
{
	//' ----------------------------------------------------------------------------
	//' 

	switch( !this.NodeText ? 0 : !this.ChildNodes.length ? 1 : 2 )
	{
		//' --------------------------------------------------------------------
		//' if item has no childnodes, write link only.
		case 1 :

			document.write( 
				"<div class=\"csCatItem\"><a class=\"csCatLink\" href=# onclick=\"" + 
				"return MenuLink_OnClick( this, " + this.NodeIndex + 
				" )\">" + this.NodeText + "</a></div>" ); break;

		//' --------------------------------------------------------------------
		//' if item has childnodes, write link and open child menu-list divs.
		case 2 :
 
			//' TODO -------------------------------------------------------
			//' menu-state works, but is still under construction...

			var sState = "Shut";
			sState = this.NodeRoot.MenuState == String( this.NodeIndex ) ? "Open" : "Shut";

			for( var j = 0; j < this.ChildNodes.length; j++ )
				if( this.NodeRoot.MenuState == String( this.ChildNodes[ j ].NodeIndex ) ) sState = "Open";

			document.write( 
				"<div class=\"csCatItem\"><a class=\"csCatLink\" href=# onclick=\"" + 
				"return MenuLink_OnClick( this, " + this.NodeIndex + 
				" )\">" + this.NodeText + "</a><div id=SubCatItems_" + 
				this.NodeIndex + " class=\"csCatItem" + sState + "\">" );

		//' --------------------------------------------------------------------
		//' if has childnodes, call recursive to write children.
		default :

			for( var j = 0; j < this.ChildNodes.length; j++ )
				this.ChildNodes[ j ].RenderMenu();

			if( this.NodeText ) document.write( "</DIV></DIV>" );
	}
}

//' ====================================================================================
//' BuildMenuListFromArray - builds the global menu-list object from an supplied array.
//' returns -> a menu-list object ( which is then used to create a dhtml menu system. )
//' 
//' ------------------------------------------------------------------------------------

function BuildMenuListFromArray( aMenuList )
{
	var oMenuRoot = new MakeMenuList();

	for( var ea = 0;ea < aMenuList.length;ea++ )
	{
		var iNodeParent  = parseInt( aMenuList[ ea ].ParentIndex );
		var bIsChildNode = aMenuList[ ea ].ParentIndex && aMenuList[ iNodeParent ];
		var oNodeParent  = bIsChildNode ? aMenuList[ iNodeParent ].Container : oMenuRoot;

		aMenuList[ ea ].Container = 
			oNodeParent.AddChild( aMenuList[ ea ].NodeName );
	}

	return oMenuRoot;
}

//' ====================================================================================
//' MenuListNode - menu-list item node constructor.
//' 
//' ------------------------------------------------------------------------------------

function MenuListNode( sNodeName, iNodeParent )
{
	this.NodeName    = sNodeName;
	this.ParentIndex = iNodeParent;
	this.Container   = null;
}

//' ====================================================================================
//' AddChildNode - method of MakeMenuItem ( for constructing menu-list items )
//' 
//' ------------------------------------------------------------------------------------

function AddChildNode( sNodeText )
{
	var iNodeCount = this.ChildNodes.length;
	this.ChildNodes[ iNodeCount ] = new MakeMenuItem( sNodeText );

	var oChildNode = this.ChildNodes[ iNodeCount ];
	oChildNode.NodeRoot = this.NodeRoot;

	this.NodeRoot.NodeIndex++;
	oChildNode.NodeIndex = this.NodeRoot.NodeIndex;

	this.NodeRoot.NodeItems[ oChildNode.NodeIndex ] = oChildNode;
	oChildNode.ParentNode = this;

	return oChildNode;
}

//' ====================================================================================
//' MakeMenuItem - menu-list object constructor.
//' 
//' ------------------------------------------------------------------------------------

function MakeMenuItem( sNodeText )
{
	//' ----------------------------------------------------------------------------
	//' 

	this.NodeIndex  = 0;
	this.NodeText   = sNodeText;
	this.ChildNodes = new Array();
	this.ParentNode = null;

	this.AddChild   = AddChildNode;   //' method for adding menu-list items.
	this.RenderMenu = RenderMenuHTML; //' method for writing dhtml to document.

	//' ----------------------------------------------------------------------------
	//' 

	if( !arguments.length )
	{
		this.NodeItems  = new Array();
		this.NodeIndex  = -1;
		this.NodeRoot   = this;

		//' --------------------------------------------------------------------
		//' node-root's dhtml window and document properties.

		window.MenuRoot = this; //' global pointer to the menu-list root object.
		this.Selected   = null; //' holds currently selected link.

		//' --------------------------------------------------------------------
		//' boolean - is window dhtml-dom compliant?

		this.IsDomDoc = document.getElementById ? true : false;

		//' TODO ---------------------------------------------------------------
		//' menu-state works, but is still under construction...

		this.MenuState = null;

		if( !this.IsDomDoc && document.all )
		{
			//' ------------------------------------------------------------
			//' set W3CDOM compliance property for ie4 browsers.

			this.IsDomDoc = true;

			document.getElementById = function( sElmtName )
			{
				return document.all[ sElmtName ];
			}
		}

		this.Render = function()
		{
			//' ------------------------------------------------------------
			//' method for writing dhtml to the document.

			document.open( "text/html" );
			this.RenderMenu();
			document.close();
		}

		this.CloseMenus = function()
		{
			//' ------------------------------------------------------------
			//' method for closing all opened menu-parents.

			if( !this.IsDomDoc ) return;

			for( var ii=0; ii < this.NodeItems.length; ii++ )
			{
				var oSubCatDiv = document.getElementById( "SubCatItems_" + ii );
				if( oSubCatDiv ) oSubCatDiv.className = "csCatItemShut";
			}
		}

		this.NodeAction = function()
		{
			//' ------------------------------------------------------------
			//' method for running the menu-link script action.

			return false;
		}
	}
}

