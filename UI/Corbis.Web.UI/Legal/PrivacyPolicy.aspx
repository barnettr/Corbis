<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/ModalPopup.Master" CodeBehind="PrivacyPolicy.aspx.cs" Inherits="Corbis.Web.UI.Legal.PrivacyPolicy" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="privacyPolicyContent" runat="server">
    <style>
        body { background-color: #e8e8e8; }
        .trustE a:hover {border-bottom:0;}
    </style>
    <div id="titleWrapper" class="titleWrapper">
        <div class="samCloseButton" id="ambiguousCloseButton">
            <Corbis:Image ID="privacyPolicyModalPopupImage" ImageUrl="/Images/iconClose.gif" runat="server" onclick="parent.CorbisUI.Legal.HidePrivacyPolicyModal();return false;" class="Close" meta:resourcekey="privacyPolicyModalPopupImage"/>
        </div>
        <div class="PrintDivSiteUsage" onclick="CorbisUI.Legal.CallPrintPolicy();">
            <div class="PrintIconImg"></div>
        </div>
        <div class="floatLeft">
            <Corbis:Localize ID="privacyPolicyLabel" runat="server" />
        </div>
    </div>
    
    <div id="contentWrapper" class="contentWrapper" runat="server">
        <div id="trusteDiv" class="trustE" style="float:right; vertical-align:top;" runat="server" visible="false">
            <Corbis:HyperLink ID="trust1EVerifyImage" Localize="true" runat="server" ImageUrl="/Images/trusteVerify.gif" NavigateUrl="http://www.truste.org/ivalidate.php?url=www.corbis.com&sealid=103" Target="_blank"></Corbis:HyperLink>
        </div>
        <Corbis:Localize ID="privacyPolicyText" Mode="Transform" runat="server" />
    </div>
    <div class="SiteUsageModalClose">
        <Corbis:GlassButton  ID="CloseModal" runat="server" ButtonBackground="e8e8e8"
            OnClientClick="parent.CorbisUI.Legal.HidePrivacyPolicyModal();return false;"
            meta:resourcekey="close"
        />
    </div>
<script language="javascript" type="text/javascript">
    CorbisUI.GlobalVars.PrivacyPolicy = {
        text: {
            copyrightMsg: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(Corbis.Web.Utilities.CopyrightHelper.CopyrightText) %>',
            privacyTitleMsg: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetLocalResourceString("privacyTitle.Text")) %>',
            altTextMsg: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetLocalResourceString("altText.Text")) %>',
            privacrPolicyTitle: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetGlobalResourceObject("Legal", "PrivacyPolicyTitle").ToString()) %>',
            CorbisUrl: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetLocalResourceString("corbisURL.Text")) %>',
            ServiceEmail: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetLocalResourceString("serviceEmail.Text")) %>',
            WatchdogURL: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetLocalResourceString("watchdogURL.Text")) %>'
        }
    };
</script>
</asp:Content>
