<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExpressCheckout_OrderSubmissionError.aspx.cs" 
Inherits="Corbis.Web.UI.Checkout.ExpressCheckout_OrderSubmissionError"  MasterPageFile="~/MasterPages/ModalPopup.Master" Title="<%$ Resources: windowTitle %>"  %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register Assembly="Corbis.Web.UI.Controls" Namespace="Corbis.Web.UI.Controls" TagPrefix="Corbis" %>
<%@ Register TagPrefix="UC" TagName="InstantService" Src="../Chat/UserControl/InstantServiceButtonSmall.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="ExpressCheckoutPaymentMethod" Src="~/Checkout/ExpressCheckout_PaymentMethod.ascx" %>



<asp:Content ID="ExpressCheckoutPage" ContentPlaceHolderID="mainContent" runat="server">
    
    <div id="TitleBar" visible="true" class="TitleBar" runat="server">
        <div class="ExpressCheckoutImageTitle">
            <h1 class="Title" style="color:#1a1a1a;margin-left:12px;font-size:18px;"><Corbis:Label runat="server" ID="orderErrorTitle" meta:resourcekey="orderErrorTitle" /></h1>
        </div>
        <div class="CloseButton noUnderline">
            <a href="javascript:void(0);" onclick="CorbisUI.ExpressCheckout.CloseExpressCheckoutModal(this)">
                <img alt="" id="XClose" class="btnRFClose" src="/Images/iconClose.gif" /></a>
        </div>
        <Corbis:ModalPopup ID="closeModal" ContainerID="confirmClose" runat="server" Width="400"
            meta:resourcekey="confirmClose">  
            <Corbis:GlassButton ID="cancelClose" runat="server" cssClass="btnGraydbdbdb" CausesValidation="false" meta:resourcekey="cancelClose"  OnClientClick="HideModal('confirmClose');return false;"  />
            <Corbis:GlassButton ID="continueClose" runat="server" CausesValidation="false" meta:resourcekey="continueClose" OnClientClick="CorbisUI.ExpressCheckout.DoCloseExpressCheckoutModal();return false;" />        
        </Corbis:ModalPopup>
        <div class="ChatDiv">
            <UC:InstantService ID="instantService" runat="server" />
        </div>
        <div class="clr extraSmallClear">
            &nbsp;</div>
    </div>
    <div class="rounded" style="background-color:#e8e8e8;margin:12px;color:#1a1a1a;">
        <p style="padding:12px;">
            <Corbis:Label runat="server" ID="orderErrorLbl" meta:resourcekey="orderError" />
        </p>
        <br />
        <p>
        <Corbis:GlassButton runat="server" ID="contactButton" meta:resourcekey="contactButton" style="margin:12px;" />
        </p>
    </div>
    
    <script type="text/javascript">
        function SecureModalPopupExit() {

            var iFrames = $$('iframe');
            var iDo = iFrames[0];
            iDo.src = ParentProtocol + "/Common/IFrameTunnel.aspx?windowid=expressCheckout&action=close";
        }
        window.addEvent('load', function() {
            var iFrames = $$('iframe');
            var iDo = iFrames[0];
            iDo.src = ParentProtocol + "/Common/IFrameTunnel.aspx?windowid=expressCheckout&action=execute&actionArg=ResizeIModal('expressCheckout', " + GetDocumentHeight() + ")&noclose=true";
        });
    </script>
    <iframe id="iFrameHttp" runat="server" style="display:none;" src="/Common/IFrameTunnel.aspx"></iframe>
	
	<script type="text/javascript">
	
		CorbisUI.GlobalVars.DownloadSummary = {
			zipFilesHeaderTemplate: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetLocalResourceString("zipFilesHeader")) %>',
            imagesHeaderTemplate: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetLocalResourceString("imagesHeader")) %>',
			totalImageCount: 1
		}; 
	
	    <%//Load up Imodal resizer %>
        window.addEvent('load', CorbisUI.ExpressCheckout.Resize);
    </script>
</asp:Content>