<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SiteUsageAgreement.aspx.cs" Inherits="Corbis.Web.UI.Legal.SiteUsageAgreement" MasterPageFile="~/MasterPages/ModalPopup.Master" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<asp:Content ID="siteUsageAgreementContent" ContentPlaceHolderID="mainContent" runat="server">
        <style>
        body { background-color: #e8e8e8; }
        </style>
        <div id="titleWrapper" class="titleWrapper">
            <asp:Localize ID="pageTitle" runat="server" />
            <div class="samCloseButton" id="ambiguousCloseButton">
                <Corbis:Image ID="siteUsageAgreementClosepImage" ImageUrl="/Images/iconClose.gif" runat="server" onclick="parent.CorbisUI.Legal.HideSiteUsageAgreementModal();return false;" class="Close" meta:resourcekey="siteUsageAgreementCloseImage"/>
            </div>
            <div class="PrintDivSiteUsage" onclick="CorbisUI.Legal.CallPrint();">
                <div class="PrintIconImg"></div>
            </div>
        </div>
        
        <div id="contentWrapper" class="contentWrapper" runat="server">
            <asp:Localize ID="agreement" runat="server" />
        </div>
        <div class="SiteUsageModalClose">
            <Corbis:GlassButton  ID="CloseModal" runat="server" ButtonBackground="e8e8e8"
                OnClientClick="parent.CorbisUI.Legal.HideSiteUsageAgreementModal();return false;"
                meta:resourcekey="close"
            />
        </div>
<script language="javascript" type="text/javascript">
    CorbisUI.GlobalVars.SiteUsageAgreement = {
        text: {
            copyrightMsg: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(Corbis.Web.Utilities.CopyrightHelper.CopyrightText) %>',
            windowTitleMsg: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetLocalResourceString("windowTitle.Text")) %>',
            altTextMsg: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetLocalResourceString("altTextMsg.Text")) %>',
            siteUsageAgreementHeadingText: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetGlobalResourceObject("Legal", "SiteUsageAgreementTitle").ToString()) %>'
        }
    };
</script>

</asp:Content>
