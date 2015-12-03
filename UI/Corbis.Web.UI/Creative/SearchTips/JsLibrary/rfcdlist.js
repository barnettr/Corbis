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

		for( var ea in aCdListItems )
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
			sCdItemList = "";
			for( var ea in aCdList ) {
				sCdItemList += "<A class=csCatLink href=# onclick=\"" +
				"return GetPageByRfCdID( '" + aCdList[ ea ].ID + 
				"' )\">" + aCdList[ ea ].Title + "&#160;(" + 
				aCdList[ ea ].Price + ")</A><BR />" }

			window.AlphaString[ iCatIndex ] = sCdItemList;
		}

		sListHeader = "Alphabetical Listing [ " + vCatIndex + " ]";
		window.MenuRoot.CloseMenus();
	}
	else
	{
		if( iCatIndex != Number( vCatIndex ) ) return;
		sCdItemList = new String( "There are no titles in this category." );
		var aCdList = window.CdCatObject[ iCatIndex ];

		if( aCdList )
		{
			sCdItemList = "";
			for( var ea in aCdList ) {
				sCdItemList += "<A class=csCatLink href=# onclick=\"" +
				"return GetPageByRfCdID( '" + aCdList[ ea ].ID + 
				"' )\">" + aCdList[ ea ].Title + "&#160;(" + 
				aCdList[ ea ].Price + ")</A><BR />" }

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
			"<td width=\"16\"><a class=\"csCatLink\" href='#subcat=" + sAlpha + "' onclick=\"" +
			"return MenuLink_OnClick( this, '" + sAlpha + "' )\">" + sAlpha + "</a></td>" );
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
