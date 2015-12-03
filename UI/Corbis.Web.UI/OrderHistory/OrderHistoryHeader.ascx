<%@ Control Language="C#" AutoEventWireup="true" Codebehind="OrderHistoryHeader.ascx.cs"
    Inherits="Corbis.Web.UI.OrderHistory.OrderHistoryHeader" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="AJAXExtentions" Namespace="AjaxControlToolkit.WCSFExtensions"
    Assembly="AjaxControlToolkit.WCSFExtensions" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="Corbis" TagName="Pager" Src="~/src/Pager/Pager.ascx" %>
<%@ Register Src="../ImageGroups/RecentImage.ascx" TagName="RecentImage" TagPrefix="Corbis" %>
<%@ Register TagPrefix="Corbis" TagName="SortBlock" Src="~/OrderHistory/OrderHistorySortBlock.ascx" %>

<div>
    <% if (ShowHeader)
       { %>
    
    <% } %>
    <div id="headerHistory" style="margin:0px 0px 0px 10px;">
        <Corbis:Label runat="server" ID="indexInfo" CssClass="floatLeft labelOrderHistoryhHeader" />
        <div id="imagePaging" class="imagePaging">
            <Corbis:Pager ID="searchResultPager" runat="server" OnPageCommand="PageChanged" 
                PrevCssClass="PrevButton" NextCssClass="NextButton" 
                PrevDisabledCssClass="PrevButtonDisabled" NextDisabledCssClass="NextButtonDisabled"
                PageNumberCssClass="NavPageNumber" PrefixLabelCssClass="PagerLabelPreFix" PostfixLabelCssClass="PagerLabelPostFix" />
        </div>
        <div id="headerlist" runat="server">
            <span class="floatLeft seperaterInSearchHeader"></span>
            <Corbis:LinkButtonList ID="itemsPerPageList" runat="server" CssClass="itemsPerPageListHistory floatLeft"
                OnControlCommand="PageSizeChanged" Spacing="10px" />
            <span class="floatLeft seperaterInSearchHeader"></span>

            <span class="floatLeft">
            <Corbis:SortBlock runat="server" ID="sortBlock" OnGenericCommand="sortBlock_Sort" />
            </span>
        </div>
      
        <div class="clr" > </div>
    </div>
    
</div>
