<%@ Control Language="C#" AutoEventWireup="true" Codebehind="LightboxProducts.ascx.cs"
	Inherits="Corbis.Web.UI.Lightboxes.Products" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<div id="LightboxProducts" class="ProductResults">
    <div id="DownloadingProhibitedDiv" visible="true" class="DownloadingProhibitedDiv" runat="server">
        <Corbis:Image ID="noDownload" ImageUrl="/Images/alertYellow.gif" CssClass="downloadingProhibited" AlternateText="" runat="server" /><Corbis:Localize ID="downloadingProhibited" runat="server" meta:resourcekey="downloadingProhibited" />
    </div>
    <Corbis:Localize ID="emptyLightboxMessage" runat="server" meta:resourcekey="emptyLightboxMessage" Visible="false" />
	<asp:Repeater ID="results" runat="server" OnItemDataBound="Result_ItemDataBound">
		<ItemTemplate>
			<span class="ProductBlock" id="productBlock" runat="server">
                <Corbis:HoverButton runat="server" ID="btnClose" CssClass="hoverBtn closeIcon" OnClientClick="javascript: CorbisUI.Lightbox.Handler.showDeleteModal(this);return false;" onkeypress="return WebForm_FireDefaultButton(event, 'ctl00_mainContent_deleteProductModal_btnDelete_GlassButton');" meta:resourcekey="btnClose"  />
                <Corbis:HoverButton runat="server" ID="btnNote" CssClass="hoverBtn noteIcon" OnClientClick="javascript: CorbisUI.Lightbox.Handler.showNoteModal(this);return false;"/>
                <div class="productContent">
					<Corbis:CenteredImageContainer IsAbsolute="true" runat="server" ImageID="image" ID="thumbWrap" />
					<div class="lightboxProductDetails">
						<div class="categoryInfo"><%# Corbis.Web.UI.CorbisBasePage.GetEnumDisplayText<Corbis.CommonSchema.Contracts.V1.Category>((Corbis.CommonSchema.Contracts.V1.Category)Eval("Category"))%></div>
						<div class="collectionInfo"><%# Eval("MarketingCollection")%></div>
						<div class="valueType <%# Eval("LicenseModel")+"_color" %>">
							<Corbis:Label ID="corbisId" runat="server" Text='<%# Eval("CorbisId") %>' />
						</div>
						<div class="controlWrap">
							<div class="CTL_Item ACTV_Details LT active hasTooltip" title="<%# Corbis.Web.UI.CorbisBasePage.GetKeyedEnumDisplayText<Corbis.CommonSchema.Contracts.V1.LicenseModel>((Corbis.CommonSchema.Contracts.V1.LicenseModel)Eval("LicenseModel"), "LongText") %>">
								<span><%# Corbis.Web.UI.CorbisBasePage.GetEnumDisplayText<Corbis.CommonSchema.Contracts.V1.LicenseModel>((Corbis.CommonSchema.Contracts.V1.LicenseModel)Eval("LicenseModel"))%></span>
							</div>
							<!-- control icons go here -->
							<div class="iconStrip" align="right" runat="server" id="icons">
								<ul>
									<li class="ICN_similar" id="iconSimilarBlock" runat="server">
										<a href="javascript:void(0)" id="viewSimilarLink" onclick="javascript: EnlargeImagePopUp('../Enlargement/Enlargement.aspx?id=<%# Eval("CorbisId") %>&tab=related&caller=lightbox');return false;">
											<img src="/images/spacer.gif" meta:resourcekey="ViewSimilarMedia" runat="server" />
										</a>
									</li>
									<li class="ICN_quickpic QP_off" id="iconQuickpicBlock" runat="server">
								        <!-- TODO:Security - strip off angular brackets before using title.-->
										<a href="javascript:void(0)" onclick="javascript:(new CorbisUI.Lightbox.ProductBlock('<%# Eval("CorbisId") %>')).addToQuicPick(this,'<%# Eval("Url128") %>','<%# Eval("LicenseModel") %>','<%# ((decimal)Eval("AspectRatio")).ToString(System.Globalization.CultureInfo.InvariantCulture) %>','<%# Corbis.Web.UI.CorbisBasePage.EncodeToJsString(Server.HtmlEncode(Eval("Title").ToString())) %>'); return false;">
											<img id="qpIcon" src="/images/spacer.gif" meta:resourcekey="AddToQuickPic" runat="server"/>
										</a>
									</li>
									<li class="ICN_pricing" id="iconPricingBlock" runat="server">
										<a href="javascript:void(0)" id="priceImageLink" runat="server">
											<img src="/images/spacer.gif" id="pricingImage" runat="server" />
										</a>
									</li>
									<li class="ICN_cart" id="iconCartBlock" runat="server" meta:resourcekey="iconCartBlock">
										<a href="javascript:void(0)" id="cartLink" runat="server">
											<img src="/images/spacer.gif" meta:resourcekey="AddToCart" runat="server"/>
										</a>
									</li>
									<li class="ICN_expresscheckout" id="iconExpressCheckoutBlock" runat="server" meta:resourcekey="iconExpressCheckoutBlock">
										<a href="javascript:void(0)" onclick="javascript:CorbisUI.ExpressCheckout.Open('<%# Eval("CorbisId") %>','<%# Eval("ProductUid") %>', <%= LightboxId %>)">
											<img src="/images/spacer.gif" meta:resourcekey="ExpressCheckout" runat="server"/>
										</a>
									</li>
								</ul>
							</div>
							<div class="clr">
							</div>
						</div>
					</div>
				   <div class="productPriceInfo<%# (Profile.IsECommerceEnabled && ((Eval("EffectivePrice") != null && Eval("EffectivePrice").ToString() != "" && Eval("EffectivePrice").ToString() != ".00" ) || ((Corbis.LightboxCart.Contracts.V1.PriceStatus)Eval("EffectivePriceStatus")) != Corbis.LightboxCart.Contracts.V1.PriceStatus.Unknown) && (HttpContext.Current.Request.ServerVariables["URL"].ToLower().IndexOf("emaillightboxview.aspx") == -1))  ? "": " hdn" %>">
				        <div id="price" runat="server" class="price"></div>
				        <div id="priceStatus" runat="server" class="priceStatus"></div>
				    </div>
					
					<div class="note<%# Eval("Note") != null && Eval("Note").ToString() != ""? "": " hdn" %>">
						<div class="noteHeader">
							<Corbis:HoverButton runat="server" ID="deleteNotes" CssClass="noteDelete" OnClientClick="javascript:CorbisUI.Lightbox.Handler.showDeleteNoteModal(this);return false;" meta:resourcekey="deleteNotes"/>
							<Corbis:Localize ID="noteHeader" runat="server" meta:resourcekey="noteHeader" />
						</div>
						<div class="noteContent" noteUid="<%# Eval("NoteUid") %>"><%# Eval("Note") != null ? System.Text.RegularExpressions.Regex.Replace(Server.HtmlEncode(Eval("Note").ToString()), "[\r\n]+", "<br/>", RegexOptions.IgnoreCase) : ""%></div>
					</div>
					<div class="permissionsWrap" id="permissionsWrap" runat="server">
						<!-- permission indicators go here -->
		                
						<ul>
							<li id="mediaRestrictionsPermissions" runat="server"><img src="/images/spacer.gif" class='<%# "ICN_" + Eval("PricingIconDisplay") %>' id="dollarIndicator" runat="server" title='<%# GetLocalResourceObject(Eval("PricingIconDisplay").ToString() + "AltText")%>' alt='<%# GetLocalResourceObject(Eval("PricingIconDisplay").ToString() + "AltText")%>' /></li>
							<li id="pricingLevelPermissions" runat="server"><span>[<%# Eval("PricingLevelIndicator")%>]</span></li>
						</ul>
					</div>
					<!-- Request price link -->
					<div id="requestPriceDiv"  class="productPriceInfo" Visible="false" runat="server">
					        <div class="priceStatus requestPrice" id="requestPrice" runat="server" >
					            <a href="javascript:void(0);" onclick="javascript:CorbisUI.RequestPricing.showModal('<%# Eval("CorbisId") %>',<%= LightboxId %>, 'Lightbox');return false;">
					                <Corbis:Localize ID="requestPriceLink" runat="server" meta:resourcekey="requestPriceLink"/>
					            </a>
					        </div>							
					 </div>						 
					
				</div>
			</span>
		</ItemTemplate>
	</asp:Repeater>
	
	<%-- Selected images or all images in lightbox Modal popup for request price - Opens before the request price is launched--%>
    	<Corbis:ModalPopup ID="requestPriceImagesModal" ContainerID="requestPriceImagesModal" runat="server" meta:resourcekey="requestPriceImagesModal">                   
            <div class="requestPriceButtons">
                <asp:RadioButtonList ID="imagesList" runat="server">
                    <asp:ListItem id="Singleimage" runat="server" meta:resourcekey="singleImageContent" CssClass="buttonsSpacing" Selected="True"></asp:ListItem>
                    <asp:ListItem id="MultipleImage" runat="server" meta:resourcekey="multipleImageContent" CssClass="buttonsSpacing"></asp:ListItem>
                </asp:RadioButtonList>		                        
            </div>
            <div class="requestPriceButtons">
                <Corbis:GlassButton ID="cancel" runat="server" CausesValidation="false" meta:resourcekey="cancel"  CssClass="MB_10"  ButtonBackground="e8e8e8" ButtonStyle="Gray" OnClientClick="javascript:HideModal('requestPriceImagesModal'); return false;"/>
                <Corbis:GlassButton ID="continue" runat="server" CausesValidation="false" meta:resourcekey="continue" CssClass="MB_10" />
            </div>        
        </Corbis:ModalPopup>

	<script type="text/javascript">
   	    var addNoteTooltip = '<%= GetLocalResourceObject("addNote").ToString() %>';
   	    var editNoteTooltip = '<%= GetLocalResourceObject("editNote").ToString() %>';
   	   	</script>

</div>
