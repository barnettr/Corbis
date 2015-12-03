<%@ Page Language="C#" MasterPageFile="~/MasterPages/ModalPopup.Master" AutoEventWireup="true" CodeBehind="ExpressDownload.aspx.cs" Inherits="Corbis.Web.UI.Checkout.ExpressDownload" Title="<%$ Resources: windowTitle %>" %>
<%@ Register Assembly="Corbis.Web.UI.Controls" Namespace="Corbis.Web.UI.Controls" TagPrefix="Corbis" %>
<%@ Register TagPrefix="Corbis" TagName="OrderItems" Src="~/Checkout/DownloadItems.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="FileSize" Src="~/CommonUserControls/FileSizes.ascx" %>
<%@ Register TagPrefix="UC" TagName="InstantService" src="../Chat/UserControl/InstantServiceButtonSmall.ascx" %>

<asp:Content ID="ExpressDownload" ContentPlaceHolderID="mainContent" runat="server">

	<div id="expressDownloadTitleBar">
		<div class="title">
			<b><Corbis:Localize ID="pageTitle" meta:resourcekey="pageTitle" runat="server"/></b>&nbsp;<Corbis:Localize ID="pageTitleMessage" meta:resourcekey="pageTitleMessage" runat="server"/>
		</div>
		<div class="closeButton noUnderline">
            <a href="javascript:void(0);" onclick="SecureModalPopupExit();">
                <img alt="" id="XClose" class="btnRFClose" src="/Images/iconClose.gif" /></a>
        </div>
        
		 <div class="ChatDiv" meta:resourcekey="chatDiv" runat="server">
			 <UC:InstantService id="instantService1" runat="server" />
		</div>
	</div>
	<div id="expressDownloadBody">
		<div id="orderInfo" class="fl rounded4">
			<div id="CompletedOrder" orderUid="<%= OrderUid %>" orderNumber="<%= ConfirmationNumber %>">
				<Corbis:Label ID="orderSuccessMsg" CssClass="successMessage" runat="server" meta:resourcekey="orderSuccessMsg" />
				<div class="orderDetails">
					<Corbis:Localize ID="confirmationLabel" meta:resourcekey="confirmationLabel" runat="server"/>&nbsp;<Corbis:Localize ID="confirmationNumber" runat="server"/>
					<br />
					<Corbis:Localize ID="totalLabel" meta:resourcekey="totalLabel" runat="server"/>&nbsp;<Corbis:Localize ID="orderTotal" runat="server"/>
				</div>
				<div class="orderMessage">
					<Corbis:Localize ID="orderMessage" meta:resourcekey="orderMessage" runat="server"/>&nbsp;<Corbis:Localize ID="orderEmail" runat="server"/>
				</div>
				<Corbis:GlassButton ID="printSummary" runat="server" CssClass="noUnderline" ButtonStyle="Outline" meta:resourcekey="printSummary" OnClientClick="self.print(); return false;"/>
			</div>
		</div>
		<div id="imageDownload" class="fl rounded4">
			<div class="imageDownloadContent">
				<Corbis:Label ID="downloadHeading" CssClass="downloadHeading" runat="server" meta:resourcekey="downloadHeading" /><br />
				<corbis:OrderItems ID="expressImage" runat="server" />
				<div class="downloadMessage">
					<Corbis:Label ID="downloadMessage" runat="server" meta:resourcekey="downloadMessage" /><br />
					<a id="showFileSizeModal" href="javascript:void(0)" onclick="CorbisUI.Order.OpenFileSizeModal(); return false;"><Corbis:Localize ID="imageSizeQuestion" runat="server" meta:resourcekey="imageSizeQuestion" /></a>
				</div>
			</div>
		</div>
	</div>
	<div id="fileSizeModalWrap">
		<Corbis:FileSize ID="fileSizeModal" runat="server" />
	</div>
	<div id="downloadProgress" style="display: none;">
		<img border="0" alt="" src="/images/ajax-loader2.gif" />
		<br />
		<div class="standBy"><Corbis:Localize ID="standByMessage" runat="server" meta:resourcekey="standByMessage" /></div>
		<div class="downloadMessage"><Corbis:Localize ID="downloadProgressMessage" runat="server" meta:resourcekey="downloadProgressMessage" /></div>
		<Corbis:GlassButton OnClientClick="CorbisUI.Order.CancelDownload(); return false;" runat="server" ButtonBackground="gray36" meta:resourcekey="cancel" CssClass="cancelButton" />
	</div>	
	<div id="errorModalWrap">
		<Corbis:ModalPopup ID="errorModal" ContainerID="errorModal" Width="372" runat="server" CloseScript="CloseModal('errorModal');return false;" meta:resourcekey="errorModal">
			<div style="color: #1a1a1a;font-size:14px;font-weight:bold;">
				<Corbis:Localize ID="errorMessage" runat="server" meta:resourcekey="errorMessage" />
			</div>
			<div style="color: #1a1a1a;margin-top: 15px;margin-bottom: 10px;">
				<Corbis:Localize ID="contactMessage" runat="server" />
			</div>
			<Corbis:GlassButton ID="closeErrorModal" runat="server" CausesValidation="false" ButtonStyle="Orange" EnableViewState="false" meta:resourceKey="close" OnClientClick="CloseModal('errorModal');return false;" CssClass="closeButton" style="margin-right: -15px;" />
		</Corbis:ModalPopup>
    </div>
    <Corbis:ModalPopup ID="downloadErrorModal" ContainerID="downloadErrorModal" Width="372" runat="server" CloseScript="HideModal('downloadErrorModal');return false;" meta:resourcekey="downloadErrorModal">
		<div class="errorMessage">
			<Corbis:Localize ID="Localize1" runat="server" meta:resourcekey="errorMessage" />
		</div>
		<div class="contactMessage"><Corbis:Localize ID="Localize2" runat="server" /></div>
		<Corbis:GlassButton ID="GlassButton1" runat="server" CausesValidation="false" ButtonStyle="Orange" EnableViewState="false" meta:resourceKey="close" OnClientClick="HideModal('downloadErrorModal');return false;" CssClass="closeButton" />
    </Corbis:ModalPopup>
    
    <Corbis:ModalPopup ID="downloadImagesModal" ContainerID="downloadImagesModal" Width="372" runat="server" CloseScript="HideModal('downloadImagesModal');return false;" meta:resourcekey="downloadImagesModal">
		<div id="zipFilesHeader" class="zipFilesHeader">
		</div>
		<div id="imagesHeader" class="imagesHeader">
		</div>
		<hr />
		<div class="imageDownloadMessage">
			<Corbis:Localize ID="imageDownloadMessage" runat="server" meta:resourcekey="imageDownloadMessage" />
		</div>
		<div id="zipFiles" class="zipFiles">
		</div>
    </Corbis:ModalPopup>
    
    <script language="javascript" type="text/javascript">
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
    
		<%// This AJAX stuff does not play well in the js file %>
		Sys.Net.WebRequestManager.add_invokingRequest(onInvoke);
		Sys.Net.WebRequestManager.add_completedRequest(onComplete);

		function onInvoke(sender, args)
		{
			if (args._webRequest._url.endsWith('DownloadImage') || args._webRequest._url.endsWith('DownloadImages'))
			{
				CorbisUI.Order.OpenProgressModal(args.get_webRequest().get_url());
			}
		}

		function onComplete(sender, args)
		{
			if (sender.get_webRequest()._url.endsWith('DownloadImage') || sender.get_webRequest()._url.endsWith('DownloadImages'))
			{		
				CorbisUI.Order.HideProgressModal();
			}
		}

		function pageUnload()
		{
			Sys.Net.WebRequestManager.remove_invokingRequest(onInvoke);
			Sys.Net.WebRequestManager.remove_completedRequest(onComplete);
		}
		
    	</script>
</asp:Content>
