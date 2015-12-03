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

			for( var ea in this.ChildNodes )
				if( this.NodeRoot.MenuState == String( this.ChildNodes[ ea ].NodeIndex ) ) sState = "Open";

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

	for( var ea in aMenuList )
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
