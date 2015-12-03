<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/ModalPopup.Master" CodeBehind="Imprint.aspx.cs" Inherits="Corbis.Web.UI.Legal.Imprint" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="imprintContent" runat="server">
    <style>
        body { background-color: #e8e8e8; }
        .contentWrapper {overflow-y:auto !important;}
    </style>
    <div id="titleWrapper" class="titleWrapper">
        <div class="samCloseButton" id="ambiguousCloseButton">
            <Corbis:Image ID="imprintModalPopupImage" ImageUrl="/Images/iconClose.gif" runat="server" onclick="parent.CorbisUI.Legal.HideImprintModal();return false;" class="Close" meta:resourcekey="imprintModalPopupImage"/>
        </div>
        <div class="PrintDivSiteUsage" onclick="CorbisUI.Legal.CallImprint();">
            <div class="PrintIconImg" ></div>
        </div>
        <div class="floatLeft">
            <Corbis:Localize ID="imprintLabel" runat="server" />
        </div>
    </div>
    
    <div id="contentWrapper" class="contentWrapper" runat="server">
        <div id="trusteDiv" visible="false" class="floatRight alignTop" style="float:right; vertical-align:top;" runat="server">
            <Corbis:HyperLink ID="trust1EVerifyImage" Localize="true" runat="server" ImageUrl="/Images/en-US/clickseal.gif" NavigateUrl="http://www.truste.org/ivalidate.php?url=www.corbis.com&sealid=103" Target="_blank"></Corbis:HyperLink>
        </div>
        <Corbis:Localize ID="imprintText" Mode="Transform" runat="server" />
    </div>
    <div class="SiteUsageModalClose">
        <Corbis:GlassButton  ID="CloseModal" runat="server" ButtonBackground="e8e8e8"
            OnClientClick="parent.CorbisUI.Legal.HideImprintModal();return false;"
            meta:resourcekey="close"
        />
    </div>
<script language="javascript" type="text/javascript">
    CorbisUI.GlobalVars.Imprint = {
        text: {
            copyrightMsg: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(Corbis.Web.Utilities.CopyrightHelper.CopyrightText) %>',
            WindowTitleText:  '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetGlobalResourceObject("Legal", "ImprintTitle").ToString()) %>',
            altTextMsg: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetLocalResourceString("altTextMsg.Text")) %>',
            ServiceGermanyEmail: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetLocalResourceString("serviceGermanyEmail.Text")) %>',
            ContactPageText: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetLocalResourceString("contactPageText.Text")) %>'
        }
    };
</script>
</asp:Content>
