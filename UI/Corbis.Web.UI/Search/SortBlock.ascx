<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SortBlock.ascx.cs" Inherits="Corbis.Web.UI.Search.SortBlock" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<div class="sortBlock floatRight" id="sortBlockId"> 
    <div class="arrowMeUp">
    </div>
    <div class="GlassButton" id="sortOptionTrigger" >
        <span class="Right"><span class="Center"><a href="javascript:void(0)">
            <div class="sortOptionText"><Corbis:Label ID="sortOptionText" runat="server"  /></div></a></span></span>
    </div>
    <asp:ListView ID="sortOptionList" runat="server" 
        ItemPlaceholderID="itemsPlaceHolder" EnableViewState="true" DataKeyNames="Value" 
        onselectedindexchanging="sortOptionList_SelectedIndexChanging"  >
        <LayoutTemplate>
            <ul class="SortOptionsMenuBottom" id="sortOptionsMenuDiv" style="display: none;">
                <asp:PlaceHolder ID="itemsPlaceHolder" runat="server" />
            </ul>
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
<script language="javascript">
    CorbisUI.addQueueItem('searchDomReady', 'sortOptions', function() {
        if ($('sortOptionTrigger') != null) {
            $('sortOptionTrigger').addEvent('click', function(e) {
                e.stopPropagation();
                $('sortOptionsMenuDiv').setStyle('display', 'block');
                var totalMenuDivs = $('sortOptionsMenuDiv').getElements('.MenuDiv');
                totalMenuDivs.each(function(item) {
                    if (totalMenuDivs.length == 3) {
                        $('sortOptionsMenuDiv').addClass('SortOptionsMenuBottom');
                    }
                    if (totalMenuDivs.length == 4) {
                        $('sortOptionsMenuDiv').addClass('SortOptionsMenuBottomFour');
                        $('sortOptionsMenuDiv').removeClass('SortOptionsMenuBottom');
                    }
                });
            });
        }
    });
</script>