<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GlobalNav.ascx.cs" Inherits="Corbis.Web.UI.Navigation.GlobalNav" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="Corbis" TagName="Checkout" Src="~/src/Navigation/CheckoutNavigation.ascx" %>
<%@ Register TagPrefix="Corbis" Assembly="Corbis.Web.UI"  Namespace="Corbis.Web.UI.Navigation"%>

<%@ Register TagPrefix="Corbis" TagName="SignInStatus" Src="~/src/Authentication/SignInStatus.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>

<div id="GlobalNav">
    <div id="BetaFlag" runat="server" class="BetaFlag">&nbsp;</div>
    <div id="Logo">
        <div class="Over" id="logoOver" runat="server">
            <Corbis:HyperLink id="home" CssClass="OverText" runat="server" meta:resourcekey="home" />
        </div>
    </div>
    <div id="Selectors">
        <div id="languageSelectorMenuDropShadow" style="display:none"></div>
        <div id="LanguageSelector" style="cursor:pointer;" onclick="CorbisUI.GlobalNav.ShowLanguages(true);">
            <div id="LanguageMenuTopDiv" runat="server" class="LanguageMenuTop">
                <div class="World"><img alt="" src="../../Images/ICN-world.png" onload="FixPng(this);" /></div>
                <div id="selectedlanguageDiv" class="SelectedLanguage">
                    <Corbis:Label ID="selectedLanguageText" runat="server"></Corbis:Label>
                </div>
                <div id="downArrowDiv" class="DownArrow"><img alt="" src="../../Images/spacer.gif" /></div>
            </div>
            <div id="languageSelectorMenu" class="LanguageSelectorMenu" onmouseover="CorbisUI.GlobalNav.ShowLanguages(true);" onmouseout="CorbisUI.GlobalNav.ShowLanguages(false);">
                <Corbis:LanguageSelectorMenu id="languageSelectorMenuID" EnableViewState="true" runat="server"> </Corbis:LanguageSelectorMenu>
            </div>
        </div>
    </div>
    <div id="Utility">
        <Corbis:Checkout ID="checkout" runat="server" />
        <div id="NavigationLinks">
            <div><Corbis:HyperLink ID="myAccount" runat="server" meta:resourcekey="myAccount" /></div>
            <div><Corbis:HyperLink ID="myLightboxes" runat="server" meta:resourcekey="myLightboxes" /></div>
            <div><Corbis:HyperLink ID="customerService" runat="server" meta:resourcekey="customerService" /></div>
        </div>
        <div class="Divider"></div>
        <div id="SignInStatus">
            <Corbis:SignInStatus ID="signInStatus" runat="server"></Corbis:SignInStatus>
        </div>
    </div>
   
    <div id="BrowseMenu" onclick="CorbisUI.GlobalNav.showBrowseMenu();">
        <!-- control for browse images here -->
        <div class="BrowseImages" id="browsesImagesDiv">
            <div class="Icon"><img alt="" src="../../Images/spacer.gif" /></div>
            <div class="Text"><Corbis:Label ID="browsesImagesTitle" runat="server" CssClass="Title" meta:resourcekey="browsesImagesTitle"></Corbis:Label></div>
        </div>

        <div class="DropDownMenu"  id="dropDownMenuDiv" onmousedown="CorbisUI.GlobalNav.vars._isBrowseFlyoutMousedown=true;event.cancelBubble=true">
           <Corbis:DropDownMenu id="dropDownMenu" runat="server"></Corbis:DropDownMenu>
        </div>
    </div>
</div>
<script type="text/javascript">
CorbisUI.GlobalNav.SetupBrowseNav();
</script>