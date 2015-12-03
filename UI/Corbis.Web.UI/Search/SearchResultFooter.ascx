<%@ Control Language="C#" AutoEventWireup="true" Codebehind="SearchResultFooter.ascx.cs"
    Inherits="Corbis.Web.UI.Search.SearchResultFooter" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="AJAXExtentions" Namespace="AjaxControlToolkit.WCSFExtensions"
    Assembly="AjaxControlToolkit.WCSFExtensions" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="Corbis" TagName="Pager" Src="~/src/Pager/Pager.ascx" %>
<div class="Clr"></div>
<div id="SearchResultsFooter">
    <Corbis:Label runat="server" ID="indexInfo" CssClass="floatLeft labelInSearchHeader" />

    <div id="imagePaging">
        <Corbis:Pager ID="searchResultPager" runat="server" OnPageCommand="PageChanged"
            PrevCssClass="PrevButton" NextCssClass="NextButton"
            PrevDisabledCssClass="PrevButtonDisabled" NextDisabledCssClass="NextButtonDisabled"
            PageNumberCssClass="NavPageNumber" PrefixLabelCssClass="PagerLabelPreFix" PostfixLabelCssClass="PagerLabelPostFix" />
    </div>
 </div>
