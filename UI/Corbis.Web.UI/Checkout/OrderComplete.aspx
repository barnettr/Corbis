<%@ Page Language="C#" MasterPageFile="~/MasterPages/NoSearchBar.Master" AutoEventWireup="true" CodeBehind="OrderComplete.aspx.cs" Inherits="Corbis.Web.UI.Checkout.OrderComplete" Title="<%$ Resources: windowTitle %>" EnableViewState="false"%>
<%@ PreviousPageType VirtualPath="~/Checkout/MainCheckout.aspx" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="Corbis" TagName="OrderItems" Src="~/Checkout/DownloadItems.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="ProjectBlock" Src="~/Checkout/ProjectBlock.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="DeliveryBlock" Src="~/Checkout/DeliveryBlock.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="PaymentBlock" Src="~/Checkout/PaymentBlock.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="LicenseDetail" Src="~/Checkout/LicenseDetail.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="FileSize" Src="~/CommonUserControls/FileSizes.ascx" %>

<asp:Content ID="OrderComplete" ContentPlaceHolderID="mainContent" runat="server">
    <div class="checkoutBar">
        <div class="Info">
            <h2>
                <Corbis:WorkflowBlock WorkflowType="COFF" runat="server">
                    <Corbis:Localize ID="pageTitleCOFF" runat="server" meta:resourcekey="pageTitleCOFF" />
                </Corbis:WorkflowBlock>
                <Corbis:WorkflowBlock WorkflowType="CART" runat="server">
                    <Corbis:Localize ID="Localize1" runat="server" meta:resourcekey="pageTitle" />                  
                </Corbis:WorkflowBlock>                   
            </h2>
        </div>
        <div class="totalIndicator">
            <ul>
                <li class="totalLabel">
                    <Corbis:Localize ID="total" runat="server" meta:resourcekey="total" />
                </li>
                <li class="totalAmount"><span id="totalCost" runat="server" ></span></li>
            </ul>
        </div>
    </div>
    <div id="CompletedOrder" orderUid="<%= OrderUid %>" orderNumber="<%= ConfirmationNumber %>">
		<div id="orderCompletedHeader" class="orderMessageWrap rounded4">
			<div id="orderMessage" class="orderMessage">
				<h3 class="confirmationNo"><Corbis:Label ID="confirmationNumber" runat="server"/></h3>
				<h3><Corbis:Localize ID="orderMessageHeader" runat="server" meta:resourcekey="orderMessageHeader" /></h3>
				<span class="messageText"><Corbis:Localize ID="orderRecords" runat="server" meta:resourcekey="orderRecords" /></span><br />
				<span class="messageText"><Corbis:Label ID="orderEmail" runat="server"/></span>
			</div>
		</div>
		<div id="orderInfo">
			<% //Download item section %>
			<div class="orderContentWrap rounded4">
				<div class="orderContent">
					<div class="orderContentHeader">
						<Corbis:GlassButton ID="downloadAll" runat="server" CssClass="downloadAll" ButtonBackground="gray36" meta:resourcekey="downloadAll" OnClientClick="CorbisUI.Order.DownloadImages(); return false;" />
						<h3><Corbis:Localize ID="downloadItemsHeader" runat="server" meta:resourcekey="downloadItemsHeader" /></h3>
						<Corbis:Label ID="downloadItemCount" CssClass="downloadItemCount" runat="server"/>
					</div>
					<div class="orderItems rounded4">
						<corbis:OrderItems ID="orderItems" runat="server" />
					</div>
					<div class="downloadFooter">
						<div class="instructionalText">
							<Corbis:Localize ID="instructionalText" runat="server" 
                                meta:resourcekey="instructionalText" ReplaceKey="" ReplaceValue="" />
						</div>
						<div class="questionLink">
							<a href="javascript:void(0)" onclick="new CorbisUI.Popup('fileSizeModal', {createFromHTML: false, showModalBackground: false, closeOnLoseFocus: true, centerOverElement: this, positionVert: 'top', positionHoriz: 'left'}); return false;"><Corbis:Localize ID="imageSizeQuestion" runat="server" meta:resourcekey="imageSizeQuestion" /></a>
						</div>
					</div>
				</div>
			</div>
			<% //Order summary section %>
			<div id="orderSummary" class="orderContentWrap rounded4">
				<div class="orderContent">
					<div class="orderContentHeader">
						<div id="printOrder" class="right BTN-border-only" onclick="window.print(); return false;">
							<div class="right">
								<div class="center">
									<Corbis:Localize ID="printOrderText" runat="server" meta:resourcekey="printOrder" />
								</div>
							</div>
						</div>
						<h3><Corbis:Localize ID="orderSummary" runat="server" /></h3>
					</div>
					<div class="orderItems orderItemsSummary rounded4">
						<div class="headerL">
							<Corbis:Localize ID="orderDateLabel" runat="server"/>
						</div>
						<div class="headerR">
						    <Corbis:Localize ID="orderTotalItems" runat="server" />
						</div>
						<div class="clr">&nbsp;</div>
					    <Corbis:ProjectBlock ID="projectBlock" runat="server" EnableEdit="false" />
						<Corbis:DeliveryBlock ID="deliveryBlock" runat="server" EnableEdit="false" />
                        <Corbis:PaymentBlock ID="paymentBlock" runat="server"  EnableEdit="false" />                            
						<div id="licensePane" class="orderSummaryPanes">
							<h3><Corbis:Localize runat="server" ID="licenseDetailsPaneTitle" meta:resourcekey="licenseDetailsPaneTitle" /></h3>
						     <table class="liecenseDetails t100" border="0" cellpadding="0" cellspacing="0">
								<asp:Repeater ID="imageDetailRepeater" runat="server" OnItemDataBound="imageDetailRepeater_ItemDataBound">
									<ItemTemplate>
										<tr>
											<td class="oddLicenseDetail" style="vertical-align:top"><Corbis:LicenseDetail ID="oddLicenseDetail" runat="server" /></td>
									</ItemTemplate>
									<AlternatingItemTemplate>
											<td class="evenLicenseDetail" style="vertical-align:top"><Corbis:LicenseDetail ID="evenLicenseDetail" runat="server" /></td>
										</tr>
									</AlternatingItemTemplate>
									<FooterTemplate>
										<%if (isOdd) {%>
											<td class="evenLicenseDetail">&nbsp;</td>
										</tr>
										<%} %>
									</FooterTemplate>
								</asp:Repeater>
						    </table>
						</div>
						<div class="orderSummaryTotalsWrap">
						    <table class="orderSummaryTotals">
						        <tr>
						            <td class="Bold"><Corbis:Localize ID="subtotalLabel" runat="server" meta:resourcekey="subtotalLabel" /></td>
						            <td class="Bold"><Corbis:Label ID="subtotalValueLabel" runat="server" meta:resourcekey="perContract" /></td>
						        </tr>
						        <tr class="totalsDividerBottom">
						            <td class="Bold"><Corbis:Localize ID="promotionAppliedLabel" runat="server" meta:resourcekey="promotionAppliedLabel" /></td>
						            <td class="Bold"><Corbis:Label ID="promotionAppliedValue" runat="server" /></td>
						        </tr>
						        <tr class="totalsDividerTop">
						            <td><Corbis:Localize ID="taxLabel" runat="server" meta:resourcekey="taxLabel" /></td>
						            <td><Corbis:Label ID="taxValue" runat="server" /></td>
						        </tr>
						        <tr id="secondTax" runat="server" visible="false">
						            <td><Corbis:Localize ID="secondTaxlabel" runat="server" meta:resourcekey="taxLabel" /></td>
						            <td><Corbis:Label ID="secondTaxValue" runat="server" /></td>
						        </tr>
						        <tr class="totalsDividerBottom">
						            <td><Corbis:Localize ID="shippingLabel" runat="server" meta:resourcekey="shippingLabel" /></td>
						            <td><Corbis:Label ID="shippingValue" runat="server" /></td>
						        </tr>
						        <tr class="totalsDividerTop totalValue">
						            <td class="Bold"><Corbis:Localize ID="totalLabel" runat="server" meta:resourcekey="totalLabel" /></td>
						            <td class="Bold"><Corbis:Label ID="totalValue" runat="server" /></td>
						        </tr>
						    </table>
						  </div>
					</div>
				</div>
			</div>
			<div class="orderContentFooter">
				<a href="http://www.truste.org/ivalidate.php?url=www.corbis.com&sealid=102" target="_new" id="TRUSTeLink" runat="server"><img src="/Images/truste.gif" alt="" id="TRUSTe" runat="server" /></a>
			</div>
		</div>
	</div>
	<Corbis:FileSize ID="fileSizeModal" runat="server" />
    <Corbis:ModalPopup ID="downloadErrorModal" ContainerID="downloadErrorModal" Width="372" runat="server" CloseScript="HideModal('downloadErrorModal');return false;" meta:resourcekey="downloadErrorModal">
		<div class="errorMessage">
			<Corbis:Localize ID="errorMessage" runat="server" meta:resourcekey="errorMessage" />
		</div>
		<div class="contactMessage">
			<Corbis:Localize ID="contactMessage" runat="server" />
		</div>
		<Corbis:GlassButton ID="closeErrorModal" runat="server" CausesValidation="false" ButtonStyle="Orange" EnableViewState="false" meta:resourceKey="close" OnClientClick="HideModal('downloadErrorModal');return false;" CssClass="closeButton" />
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
    <Corbis:ModalPopup ID="rfcdDownloadTemplate" runat="server" ContainerID="rfcdDownloadTemplate" Width="810" CloseScript="HideModal('rfcdDownloadTemplate');return false;" meta:resourcekey="rfcdDownloadTemplate">
		<div class="rfcdInstruction">
			<span class="rfcdInstructionHeader"><Corbis:Localize ID="rfcdInstructionHeader" runat="server" meta:resourcekey="rfcdInstructionHeader" /></span><br />
			<span class="rfcdInstructionText"><Corbis:Localize ID="rfcdInstruction" runat="server" meta:resourcekey="rfcdInstruction" /></span>
			<div class="rfcdDownloadControls">
				<Corbis:GlassButton ID="downloadRfcd" OnClientClick="javascript:CorbisUI.Order.DownloadRfcdImages(); return false;" runat="server" ButtonStyle="Orange" ButtonBackground="e8e8e8" meta:resourcekey="downloadRfcd" Enabled="false" /><br /><br />
				<a class="selectAllLink" href="javascript:void(0)" onclick="CorbisUI.Order.ToggleAllRfcd(this)" runat="server" meta:resourcekey="selectAll"><Corbis:Localize ID="selectAllText" runat="server" meta:resourcekey="selectAllText" /></a>
			</div>
		</div>
		<div class="rfcdInfo">
			<div class="rfcdImageThumb">
				<img src=""/>
			</div>
			<span class="rfcdId"></span><br />
			<span class="licenseModel RF_color"></span><br /><br />
			<span class="rfcdTitle"></span><br /><br /><br />
			<span class="rfcdImageCount"></span>
			<div class="clr"><input type="hidden" id="selectedFileSize" /></div>
		</div>
		<div id="rfcdImages" class="rfcdImages">
			<ul>
			</ul>
		</div>
    </Corbis:ModalPopup>
	<%// Modal for downloading images %>
	<div id="downloadProgress" style="display: none;">
		<img border="0" alt="" src="/images/ajax-loader2.gif" />
		<br />
		<div class="standBy"><Corbis:Localize ID="standByMessage" runat="server" meta:resourcekey="standByMessage" /></div>
		<div class="downloadMessage"><Corbis:Localize ID="downloadMessage" runat="server" meta:resourcekey="downloadMessage" /></div>
		<div id="longWaitMessageDiv" class="longWaitMessage hdn"><Corbis:Localize ID="longWaitMessage" runat="server" meta:resourcekey="longWaitMessage" /></div>
		<Corbis:GlassButton OnClientClick="CorbisUI.Order.CancelDownload(); return false;" runat="server" ButtonBackground="gray36" meta:resourcekey="cancel" CssClass="cancelButton" />
	</div>	
	<script type="text/javascript">
		CorbisUI.GlobalVars.DownloadSummary = {
			imagesHeaderTemplate: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetLocalResourceString("imagesHeader")) %>',
			zipFilesHeaderTemplate: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetLocalResourceString("zipFilesHeader")) %>',
			moreRestrictionText: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(HttpContext.GetLocalResourceObject("~/Checkout/LicenseDetail.ascx", "moreRestrictionsLink.Text").ToString()) %>',
			lessRestrictionText: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(HttpContext.GetLocalResourceObject("~/Checkout/LicenseDetail.ascx", "lessRestrictions").ToString()) %>',
			totalImageCount: <%= TotalImageCount %>
		};
		
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
				HideModal('downloadProgress');
			}
		}

		function pageUnload()
		{
			Sys.Net.WebRequestManager.remove_invokingRequest(onInvoke);
			Sys.Net.WebRequestManager.remove_completedRequest(onComplete);
		}
	</script>
</asp:Content>
