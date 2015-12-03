<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderHistorySummary.aspx.cs" MasterPageFile="~/MasterPages/AccountsMaster.Master" Inherits="Corbis.Web.UI.OrderHistory.OrderHistorySummary" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="Corbis" TagName="OrderItems" Src="~/OrderHistory/DownloadItems.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="ProjectBlock" Src="ProjectBlock.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="DeliveryBlock" Src="DeliveryBlock.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="PaymentBlock" Src="PaymentBlock.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="LicenseDetail" Src="LicenseDetail.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="FileSize" Src="~/CommonUserControls/FileSizes.ascx" %>

<asp:Content ID="OrderComplete" ContentPlaceHolderID="accountsContent" runat="server">
 
    <div id="CompletedOrder" orderUid="<%= OrderUid %>" orderNumber="<%= OrderNumber %>">

		<Corbis:Label ID="orderEmail" Visible="false" runat="server"/>
		<div id="orderInfo">
			<% //Download item section %>
			<div class="orderContentWrap rounded4">     
			
			  <!-- error handling div here -->
			  <div id="error" visible="false" runat="server">
                <div id="errorBlock">
                    <a class="chat" href="#"><Corbis:Localize ID="chat" runat="server" meta:resourcekey="chat" /></a>
                    <img alt="error" src="../Images/iconError.png" / class="errorImage" />
                    <p class="errorBlockSorry"><Corbis:Localize ID="wereSorry" runat="server" meta:resourcekey="wereSorry" /></p>                                 
                    <p class="errorBlockPlease"><Corbis:Localize ID="pleaseContact" runat="server" meta:resourcekey="pleaseContact" /></p>        
                    <Corbis:GlassButton ID="contactUsButton" runat="server" ButtonStyle="Orange" ButtonBackground="gray36" CausesValidation="false" meta:resourcekey="contactUs" />                     
                    
                    <div class="BTN-border-only" onclick="javascript:location.href='/OrderHistory/OrderHistory.aspx'">
                        <div class="right">
                            <div class="center">
                                <asp:Label ID="goToOrdersButton" runat="server" meta:resourceKey="goToOrdersButton" />
                            </div>
                        </div>
                    </div>
                </div>
               </div>
            <!-- end of end of error handling div -->
        <div id="block1" runat="server" visible="true">
				<div class="orderContent">
					<div class="orderContentHeader">
						<Corbis:GlassButton ID="downloadAll" runat="server" CssClass="downloadAll" ButtonBackground="gray36" meta:resourcekey="downloadAll" OnClientClick="CorbisUI.Order.DownloadImages(); return false;" />
						<h3><Corbis:Localize ID="downloadItemsHeader" runat="server" meta:resourcekey="downloadItemsHeader" /></h3>
						<Corbis:Label ID="downloadItemCount" CssClass="downloadItemCount" runat="server"/>
					</div>
					<div class="orderItems rounded4">
						<Corbis:OrderItems ID="orderItems" runat="server" />
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
			</div>
			<% //Order summary section %><div runat="server" id="block2" visible="true">
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
						<%--<Corbis:Image ID="selectOrderLinkImg" runat="server" imageUrl="/Images/iconHelp.png" onclick="OpenSelectOrder(this);return false;" />--%>
                        <div id="selectOrderLink" class="right1 OH-border-only1" onclick="OpenSelectOrder(this);return false;">
						   <div class="right1">
						    <div class="center1">
                                   <Corbis:Label ID="sortOptionText" CssClass="selectOptionText" runat="server" meta:resourcekey="selectOrder"  />
                                   <img src="../Images/mso_arrow.gif" />
						    </div>
						   </div>
					    </div>
						<div id="selectOrder"  style="display:none;" >
					        
					        
					        
					      <div class="mainShadowContainer"> 
					        <div class="selectOrderDiv mainShadow" onmouseover="ShowddlSelectOrder();" onmouseout="HideddlSelectOrder();"  id="ddlSelectOrder" name="ddlSelectOrder"  onchange="GetSelectedItem(this);">
                            </div>	
                          <div class="bottomLeftShadow">&nbsp;</div>
                <div class="topRightShadow">&nbsp;</div>
            </div>  
	                    </div>
	                  
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
											<td class="oddLicenseDetail"><Corbis:LicenseDetail ID="oddLicenseDetail" runat="server" /></td>
									</ItemTemplate>
									<AlternatingItemTemplate>
											<td class="evenLicenseDetail"><Corbis:LicenseDetail ID="evenLicenseDetail" runat="server" /></td>
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
						        <asp:Repeater ID="feeDetailsRepeater" runat="server">
			                    <ItemTemplate>
				                    <tr>
					                    <td class="leftLabels">
						                    <p>
							                    <%# Eval("DisplayText")%>
						                    </p>
					                    </td>
					                    <td>
						                    <p>
							                    <%# Eval("DisplayAmount") %>
						                    </p>
					                    </td>
				                    </tr>
			                    </ItemTemplate>
		                    </asp:Repeater>
						        <tr class="totalsDividerTop">
						            <td><Corbis:Localize ID="taxLabel" runat="server" meta:resourcekey="taxLabel" /></td>
						            <td><Corbis:Label ID="taxValue" runat="server" /></td>
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
			</div>
			<div runat="server" id="block4" visible="true">
			<div class="orderContentFooter">
				<a href="http://www.truste.org/ivalidate.php?url=www.corbis.com&sealid=102" target="_new" id="TRUSTeLink" runat="server"><img src="/Images/truste.gif" alt="" id="TRUSTe" runat="server" /></a>
			</div>
			</div>
		</div>
	</div>
	<Corbis:FileSize ID="fileSizeModal" runat="server" />
    <Corbis:ModalPopup ID="downloadErrorModal" ContainerID="downloadErrorModal" Width="372" runat="server" CloseScript="HideModal('downloadErrorModal');return false;" meta:resourcekey="downloadErrorModal">
		<div class="errorMessage">
			<Corbis:Localize ID="errorMessage" runat="server" meta:resourcekey="errorMessage" />
		</div>
		<div class="contactMessage"><Corbis:Localize ID="contactMessage" runat="server" /></div>
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
				<Corbis:GlassButton ID="downloadRfcd" OnClientClick="javascript:CorbisUI.Order.DownloadRfcdImages(); return false;" runat="server" ButtonStyle="Orange" ButtonBackground="e8e8e8" meta:resourcekey="downloadRfcd" Enabled="false" />
				<a id="A1" class="selectAllLink" href="javascript:void(0)" onclick="CorbisUI.Order.ToggleAllRfcd(this)" runat="server" meta:resourcekey="selectAll"><Corbis:Localize ID="selectAllText" runat="server" meta:resourcekey="selectAllText" /></a>
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
		<img border="0" alt="" src="/images/ajax-loader2.gif " />
		<br />
		<div class="standBy"><Corbis:Localize ID="standByMessage" runat="server" meta:resourcekey="standByMessage" /></div>
		<div class="downloadMessage"><Corbis:Localize ID="downloadMessage" runat="server" meta:resourcekey="downloadMessage" /></div>
		<div id="longWaitMessageDiv" class="longWaitMessage hdn"><Corbis:Localize ID="longWaitMessage" runat="server" meta:resourcekey="longWaitMessage" /></div>
		<Corbis:GlassButton ID="GlassButton1" OnClientClick="CorbisUI.Order.CancelDownload(); return false;" runat="server" ButtonBackground="gray36" meta:resourcekey="cancel" CssClass="cancelButton" />
	</div>	
	<script type="text/javascript">
	    var orderUid = window.location.href.split("?")[1].split("=")[1];
		CorbisUI.GlobalVars.DownloadSummary = {
			imagesHeaderTemplate: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetLocalResourceString("imagesHeader")) %>',
			zipFilesHeaderTemplate: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(this.GetLocalResourceString("zipFilesHeader")) %>',
			moreRestrictionText: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(HttpContext.GetLocalResourceObject("~/OrderHistory/LicenseDetail.ascx", "moreRestrictionsLink.Text").ToString()) %>',
			lessRestrictionText: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(HttpContext.GetLocalResourceObject("~/OrderHistory/LicenseDetail.ascx", "lessRestrictions").ToString()) %>',
			totalImageCount: <%= TotalImageCount %>
		}; 
		if($('downloadProducts')!=null)
		{
		    var downbutton = $('downloadProducts').getElement('div.GlassButton');
		    var downPeriod = $('downloadProducts').getElement('div.downloadPeriod'); 
		    var addcartEle = $('downloadProducts').getElement('div.addtoCart');
			var contactCorbisEle = $('downloadProducts').getElement('div.addtoCart');
			if( (downPeriod != null || addcartEle != null || contactCorbisEle != null) && downbutton==null )
			{
			    setGlassButtonDisabled('<%=downloadAll.ClientID %>', true);
			}
		}
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
