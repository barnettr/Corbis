<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ImageGroupsProducts.ascx.cs" Inherits="Corbis.Web.UI.ImageGroups.ImageGroupsProducts" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register Src="~/Accounts/RoundCorners.ascx" TagName="RoundCorners" TagPrefix="Corbis" %>
<div id="ProductResults" class="ProductResults">

    <div id="captionBtnHide" class="captionBtnHide" visible="false" onclick="CorbisUI.ImageGroups.toggleCaption();" runat="server">
        <span id="captionIconHideDiv" class="captionIconHide"></span>
        <span class="labelDivHide"><Corbis:Label ID="HideCaption" runat="server" CssClass="HideCaptionLabel" meta:resourcekey="HideCaption" /></span>
        <span class="labelDivShow displayNone"><Corbis:Label ID="ShowCaption" runat="server" CssClass="HideCaptionLabel" meta:resourcekey="ShowCaption" /></span>
    </div>
    <div style="clear:both;"></div>
    <div id="hideCaptionWrapper" class="rounded hideCaptionWrapper" visible="false" runat="server">    
        
        <div id="HideCaptionBody" class="hideCaptionBody">
            <div id="HideCaptionPadding" class="hideCaptionPadding">
                <Corbis:Label CssClass="captionHeader" ID="captionHeader" runat="server" />
                <Corbis:Label CssClass="captionContent" ID="HideCaptionLabel" runat="server" />
            </div>
        </div>
        
    </div>

 <div id="emptydiv" class="emptydiv"></div>
    <div id="DownloadingProhibitedDiv" visible="false" class="DownloadingProhibitedDiv" runat="server">
        <Corbis:Image ID="noDownload" ImageUrl="/Images/alertYellow.gif" CssClass="downloadingProhibited" AlternateText="" runat="server" /><Corbis:Localize ID="downloadingProhibited" runat="server" meta:resourcekey="downloadingProhibited" />
    </div> 
    <Corbis:LinkButton ID="hiddenRFPricingLink" CssClass="displayNone" runat="server"
        CausesValidation="false"></Corbis:LinkButton>
     <Corbis:Repeater ID="results" runat="server" OnItemDataBound="Result_ItemDataBound">
        <ItemTemplate>
			<span class="ProductBlock" id="productBlock" runat="server">
				<Corbis:CenteredImageContainer ID="thumbWrap" IsAbsolute="true" runat="server" ImageID="image" />
				<div class="Details">
					<div class="categoryInfo">
						<Corbis:Label id="searchCategory" runat="server"></Corbis:Label>
					</div>
					<div class="collectionInfo">
						<Corbis:Label id="marketingCollection" runat="server"></Corbis:Label>
					</div>
	                
					<div class="valueType <%# Eval("LicenseModel")+"_color" %>">
						<Corbis:Label ID="corbisId" runat="server" Text='<%# Eval("CorbisId") %>'></Corbis:Label>                               
	                    
					</div>
	                
					<div class="controlWrap">
						<div class="CTL_Item ACTV_Details LT active hasTooltip" title="<%# GetLocalResourceObject(Eval("LicenseModel") + "LongText") %>">
							<span alt="<%# GetLocalResourceObject(Eval("LicenseModel") + "ShortText") %>" title="<%# GetLocalResourceObject(Eval("LicenseModel") + "LongText") %>"><%# GetLocalResourceObject(Eval("LicenseModel") + "ShortText") %>
							</span>
						</div> 
						<!-- control icons go here -->
						<div class="iconStrip" align="right">
							<ul>
								<li class="ICN_similar" id="iconSimilarBlock" runat="server"><a href="javascript:void(0)" id="viewSimilarLink" runat="server"><img id="Img1" src="/images/spacer.gif" meta:resourcekey="ViewSimilarMedia" runat="server"/></a></li>
								<li class="ICN_lightbox" id="iconLightboxBlock" runat="server"><a href="javascript:void(0)" onclick="if (!allowClickEvent('<%# Eval("CorbisId") %>')) return false; CorbisUI.Handlers.Lightbox.addTo('<%# Eval("CorbisId") %>'); return false;"><img id="Img2" src="/images/spacer.gif" meta:resourcekey="AddToLightbox" runat="server"/></a></li>
								<!-- TODO:Security - strip off angular brackets before using title.-->
								<li class="ICN_quickpic QP_off" id="iconQuickpicBlock" runat="server"><a href="javascript:void(0)" onclick="javascript:CorbisUI.Handlers.Quickpic.moveQuickpic(this, '<%# Eval("CorbisId") %>','<%# Eval("Url128") %>','<%# Eval("LicenseModel") %>','<%# ((decimal)Eval("AspectRatio")).ToString(System.Globalization.CultureInfo.InvariantCulture) %>','<%# Corbis.Web.UI.CorbisBasePage.EncodeToJsString(Server.HtmlEncode(Eval("Title").ToString())) %>'); return false;"><img id="qpIcon" src="/images/spacer.gif" meta:resourcekey="AddToQuickPic" runat="server"/></a></li>
								<li class="ICN_pricing" id="iconPricingBlock" runat="server"><a href="javascript:void(0)" id="priceImageLink" runat="server"><img src="/images/spacer.gif" id="pricingImage" runat="server" /></a></li>
								<li id="iconCartBlock" runat="server" meta:resourcekey="cartIcon" ><a href="javascript:void(0)" onclick="CorbisUI.Handlers.Cart.addTo('<%# Eval("CorbisId") %>');return false;"><img id="addToCartImage" src="/images/spacer.gif" meta:resourcekey="AddToCart" runat="server"/></a></li>
								<li class="ICN_expresscheckout" id="iconExpressCheckoutBlock" runat="server" meta:resourcekey="cartIconExpress"><a href="javascript:void(0)" onclick="CorbisUI.ExpressCheckout.Open('<%# Eval("CorbisId") %>')"><img id="Img5" src="/images/spacer.gif" meta:resourcekey="ExpressCheckout" runat="server" /></a></li>
							</ul>
						</div>
						<div class="clr"> </div>
					</div>
					
					<div class="permissionsWrap" id="permissionsWrap" runat="server">
						<!-- permission indicators go here -->
		                
						<ul>
							<li id="mediaRestrictionsPermissions" runat="server"><img src="/images/spacer.gif" alt="" title="" class="ICN_tripleDollars" id="dollarIndicator" runat="server" /></li>
							<li id="pricingLevelPermissions" runat="server"><span>[<%# Eval("PricingLevelIndicatorForThumbnail")%>]</span></li>
						</ul>
					</div>					
				</div>
			</span>
		</ItemTemplate>
	</Corbis:Repeater>
</div>