<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderHistorySortBlock.ascx.cs" Inherits="Corbis.Web.UI.OrderHistory.OrderHistorySortBlock" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>


<div class="sortBlock" id="sortBlockId"> 

    <div class="GlassButton" id="sortOptionTrigger" >
        <span class="Right"><span class="Center">    
            <a href="javascript:void(0)">
                <Corbis:Label ID="sortOptionText" CssClass="sortOptionText" runat="server"  />            
                <div class="arrowMeUp">&nbsp;</div>            
            </a>            

        </span></span>
    </div>
    <asp:ListView ID="sortOptionList" runat="server" 
        ItemPlaceholderID="itemsPlaceHolder" EnableViewState="true" DataKeyNames="Value" 
        onselectedindexchanging="sortOptionList_SelectedIndexChanging"  >
        <LayoutTemplate>
<!--[if lte IE 7]>
    <div class="IE7OrLess">   
<![endif]--> 
<div class="mainShadowContainer" id="sortOptionsMenuDivContainer" style="display: none;"> 
                <ul class="LanguageMenuBottom mainShadow" onmouseover="ShowSortOptionsDiv();" onmouseout="HideSortOptionsDiv();" id="sortOptionsMenuDiv">
                    <asp:PlaceHolder ID="itemsPlaceHolder" runat="server" />
                </ul>
                <div class="bottomLeftShadow">&nbsp;</div>
                <div class="topRightShadow">&nbsp;</div>
            </div>
<!--[if lte IE 7]>
    </div>
<![endif]-->        
</LayoutTemplate>
        <ItemTemplate>
            <li class="MenuDiv">
                <asp:LinkButton runat="server" ID="SelectButton" CommandName="Select">
                    <div class="MenuItem">
                        <span ><%# Eval("Text") %></span>
                    </div>
                </asp:LinkButton>
            </li>
        </ItemTemplate>
        <SelectedItemTemplate>
            <li class="MenuDiv" >
                    <div class="MenuItem Selected">
                        <span ><%# Eval("Text") %></span>
                    </div>
            </li>
        </SelectedItemTemplate>
    </asp:ListView>
</div>

<script language="javascript" type="text/javascript">
    window.addEvent('domready', function() {
        if ($('sortOptionTrigger') != null) {
            $('sortOptionTrigger').addEvent('click', function(e) {
                e.stopPropagation();
                $('sortOptionsMenuDivContainer').setStyle('display', 'block');
            });
        }
    });

    function HideSortOptionsDiv() {
        try { $('sortOptionsMenuDivContainer').setStyle('display', 'none'); } catch (er) { }
    }
    function ShowSortOptionsDiv() {
        try { $('sortOptionsMenuDivContainer').setStyle('display', 'block'); } catch (er) { }
    }
</script>