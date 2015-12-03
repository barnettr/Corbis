<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Cart.aspx.cs" Inherits="Corbis.Web.UI.Checkout.Cart"
    MasterPageFile="~/MasterPages/NoSearchBar.Master" Title="<%$ Resources: windowTitle %>" %>

<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="Corbis" TagName="CartItems" Src="CartItems.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="ContactCorbis" Src="~/UserControls/ContactControls/ContactCorbis.ascx" %>

<asp:Content ID="cartContent" runat="server" ContentPlaceHolderID="mainContent">

    <script language="javascript" type="text/javascript" src="/scripts/AlertWindow.js"></script>

    <script language="javascript" type="text/javascript">
        Corbis.Web.UI.Checkout.CartScriptService.ValidateItems = function(onSuccess, onFailed, userContext)
        {
            Corbis.Web.UI.Checkout.CartScriptService._staticInstance.ValidateItems(onSuccess, onFailed, userContext);
        }
        function pageLoad()
        {
            Corbis.Web.UI.Checkout.CartScriptService.set_defaultFailedCallback(methodFailed);
        }
        var RMPRICINGURL = '<%=Corbis.Web.UI.SiteUrls.RMPricing %>?ParentPage=Cart&CorbisId=';
        var RFPRICINGURL = '<%=Corbis.Web.UI.SiteUrls.RFPricing %>?ParentPage=Cart&CorbisId=';
        var RSPRICINGURL = '<%=Corbis.Web.UI.SiteUrls.RSPricing %>?ParentPage=Cart&CorbisId=';
        var DisplayTextPerContract = '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString((string)HttpContext.GetGlobalResourceObject("Resource", "AsPerContract")) %>';
        
        
        
        CorbisUI.GlobalVars.Cart = {
            checkoutAllId: '<%=checkoutAll.ClientID %>',
            deleteAllPriced: '<%= tibDeleteAllPriced.ClientID %>',
            deleteAllUnpriced: '<%= tibDeleteAllUnpriced.ClientID %>',
            clearAllButton: '<%= tibCartCaroselClearAll.ClientID %>',
            defaultUrl: '<%=Corbis.Web.UI.SiteUrls.Home %>',
            cartUrl: '<%=Corbis.Web.UI.SiteUrls.Cart %>'
        };
    </script>

    <div class="cartBar">
        <div class="wrap">
            <div class="cartInfo">
                <h2>
                    <Corbis:Localize ID="pageTitle" runat="server" meta:resourcekey="pageTitle" /></h2>
                <p class="cartInfoLine" runat="server" id="myCartDetail"></p>
            </div>
            <div id="returnToSearchButton" runat="server" class="right BTN-border-only">
                <div class="right">
                    <div class="center">
                        <Corbis:Localize ID="returnToSearch" runat="server" meta:resourcekey="returnToSearch" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="contentBar">
        <div class="wrap rounded">
            <div class="checkoutStage rounded" id="CheckoutBox">
                <div class="columnHeader">
                    <h3>
                        <Corbis:Localize ID="checkoutTitle" runat="server" meta:resourcekey="checkoutTitle"/></h3>
                    <div class="checkout" id="checkout">
                        <Corbis:GlassButton ID="checkoutBtn" runat="server" meta:resourceKey="checkoutBtn"
                            OnClientClick="if(!CorbisUI.Cart.CanContinue(this.id)){return false}"
                            PostBackUrl="~/Checkout/MainCheckout.aspx" />
                    </div>
                    <div class="checkoutTotals">
                        <span id="checkoutTotalCount1">
                            <Corbis:Label ID="checkoutTotalCount" runat="server" Text="0"/>
                        </span>
                        <Corbis:Localize ID="checkoutTotalItems" runat="server" meta:resourcekey="checkoutTotalItems"/>
                        <span id="checkoutTotalCost1">
                            <Corbis:Label ID="checkoutTotalCost" runat="server" Text="0.00"/>
                        </span>
                        <Corbis:Label ID="currencyCode" runat="server" />
                    </div>
                    <div class="clr">
                    </div>
                </div>
                <div id="cartCarousel">
                    <div id="cartCarouselContent">
                        <!-- <img id="cartCarouselPrevious" src="/images/spacer.gif" alt="move previous" onclick="javascript:scrollCarousel(true);" /> -->
                        <Corbis:Image ID="cartCarouselPrevious" CssClass="cartCarouselPrevious" ImageUrl="/images/spacer.gif" runat="server" onclick="javascript:scrollCarousel(true);" meta:resourcekey="cartCarouselPreviousImage"/>
                        <div id="emptyCartCarosel" class="displayNone">
                            <Corbis:Localize ID="emptyCartCaroselMessage" runat="server" meta:resourcekey="emptyCartCaroselMessage" />
                        </div>
                        <div id="cartCarouselWithItems">
                            <ul id="cartCarouselItems">
                                <Corbis:CartItems ParentPage="Cart" ID="checkoutItems" runat="server" 
                                    Zone="Checkout" RenderInList="true" EnableViewState="false" />
                            </ul>
                        </div>
                        <!-- <img id="cartCarouselNext" src="/images/spacer.gif" alt="move next" onclick="javascript:scrollCarousel(false);" /> -->
                        <Corbis:Image ID="cartCarouselNext" CssClass="cartCarouselNext" ImageUrl="/images/spacer.gif" runat="server" onclick="javascript:scrollCarousel(false);" meta:resourcekey="cartCarouselNextImage"/>
                    </div>
                </div>
                <div class="columnHeader">
                    &nbsp;
                    <div id="itemToCheckoutClear">
                    <Corbis:TextIconButton 
                        ID="tibCartCaroselClearAll" runat="server" Icon="clearAll" 
                        meta:resourceKey="clearItems" OnClientClick="clearCheckoutItems();" 
                    />
                    </div>
                    <div class="clr">
                    </div>
                </div>
                <div class="clr">
                </div>
            </div>
            
            <div class="roundcornerhack">
	            <div id="PricingContainer_wrapper">
	                <div class="leftColWrap">
		                <div class="leftCol">
			                <!-- Column one start -->
			                <div class="col2 rounded" id="PricedBox">
                                <div class="columnHeader">
                                    <div class="floatLeft">
                                        <Corbis:Localize ID="pricedItemsHeader" runat="server" meta:resourcekey="pricedItemsHeader"/>
                                    </div>
                                    <div class="floatRight ">
                                        <Corbis:GlassButton ID="checkoutAll" runat="server" meta:resourceKey="checkoutAllBtn"
                                            ButtonBackground="gray4b"
                                            OnClientClick="CorbisUI.Cart.BatchValidate.newBatch(); return false;"
                                         />
                                    </div>
                                    <div class="clr"> </div>
                                </div>
                                <div id="PricedZone" class="sameHeight">
                                    <Corbis:CartItems ParentPage="Cart" ID="pricedItems" runat="server" 
                                        Zone="Priced" />
                                    <div class="instructions displayNone" ID="pricedZoneInstruction" runat="server" >
										<Corbis:Localize ID="pricedMessage" runat="server" meta:resourcekey="pricedMessage" />
                                    </div>
                                    <div class="clr"> </div>
                                </div>
                                <div class="columnHeader" style="text-align:right">
                                    <Corbis:TextIconButton CssClass=MR_10 
                                        Icon=deleteAll runat=server ID=tibDeleteAllPriced
                                        OnClientClick="modalDeleteAlertTempDeleteAll('Priced',this);" 
                                        meta:resourceKey="deleteAll"  
                                    />
                                    <div class="clr"> </div>
                                </div>
                            </div>
			                <!-- Column one end -->
		                </div>
		                <div class="rightCol">
			                <!-- Column two start -->
			                <div class="col1 rounded" id="UnPricedBox">
                                <div class="columnHeader">
                                    <div class="floatLeft">
                                        <Corbis:Localize ID="unpricedItemsHeader" runat="server" meta:resourcekey="unpricedItemsHeader"/>
                                    </div>
                                    <!--<div class="smallIcon selectAll floatRight MR_10">
                                        <div class="actLikeLink" onclick="Corbis.Cart.showInlineAlert('IA_unpriced');">
                                            Select all</div>
                                    </div>-->
                                    <div class="clr"> </div>
                                </div>
                                <div id="UnPricedZone" class="unpricedZone sameHeight">
                                    <Corbis:CartItems ParentPage="Cart" ID="unPricedItems" runat="server" 
                                        Zone="UnPriced" />
                                    <div class="instructions displayNone" id="unpricedZoneInstruction" runat="server" >
										<Corbis:Localize ID="Localize1" runat="server" meta:resourcekey="unpricedMessage" />
                                    </div>
                                    <div class="clr"> </div>
                                </div>
                                <div class="columnHeader" style="text-align:right">
                                    <Corbis:TextIconButton CssClass=MR_10
                                        Icon=deleteAll runat=server  ID=tibDeleteAllUnpriced
                                        OnClientClick="javascript:modalDeleteAlertTempDeleteAll('UnPriced',this);" 
                                        meta:resourceKey="deleteAll"
                                    />
                                    
                                </div>
                                <div class="clr"> </div>
                               
                            </div>
			                <!-- Column two end -->
		                </div>
	                </div>
                </div>
                <div class="clr"> </div>
                
                <%-- HIDDEN --%>
                
                <div class="hidden" id="PriceMultipleBox">
                    <div class="columnHeader">
                        <h3><Corbis:Localize ID="priceMultipleImages" runat="server" meta:resourcekey="priceMultipleImages" /></h3>
                        <div class="priceNow" id="priceNow">
                            <Corbis:GlassButton ID="priceNowBtn" runat="server" meta:resourceKey="PriceNowBtn" />
                        </div>
                        <div class="clr">
                        </div>
                    </div>
                    <div id="PriceMultipleZone" class="sameHeight">
                        <Corbis:CartItems ParentPage="Cart" ID="pricingMultipleImages" runat="server" Zone="PriceMultiple" />
                        <div class="instructions displayNone">
							<Corbis:Localize ID="priceMultipleMessage" runat="server" meta:resourcekey="priceMultipleMessage" />
                        </div>
                    </div>
                    <div class="columnHeader" id=clearPriceParent style="text-align:right">
                        <Corbis:TextIconButton CssClass=MR_10 
                            Icon=clearAll runat=server ID=tibClearToPrice
                            OnClientClick="javascript:clearPricingItems();"
                            meta:resourcekey="clearItems"/>
                        <div class="clr">
                        </div>
                    </div>
                </div>
                
	        </div>
            

	        
            <%-- RESTORE THIS BELOW WHEN WE DO PRICE MULTIPLES
            <div class="roundcornerhack">
                <div class="colmask">
                    <div class="colmid">
                        <div class="colleft">
                            <div class="col1 rounded" id="UnPricedBox">
                                <div class="columnHeader">
                                    <div class="floatLeft">
                                        <Corbis:Localize ID="unpricedItemsHeader" runat="server" meta:resourcekey="unpricedItemsHeader"/>
                                    </div>
                                    <!--<div class="smallIcon selectAll floatRight MR_10">
                                        <div class="actLikeLink" onclick="Corbis.Cart.showInlineAlert('IA_unpriced');">
                                            Select all</div>
                                    </div>-->
                                    <div class="clr">
                                    </div>
                                </div>
                                <div id="UnPricedZone" class="unpricedZone sameHeight">
                                    <Corbis:CartItems ParentPage="Cart" ID="unPricedItems" runat="server" 
                                        Zone="UnPriced" />
                                    <div class="instructions displayNone" id="unpricedZoneInstruction" runat="server" >
										<Corbis:Localize ID="Localize1" runat="server" meta:resourcekey="unpricedMessage" />
                                    </div>
                                    <div class="clr">
                                    </div>
                                </div>
                                <div class="columnHeader" style="text-align:right">
                                    <Corbis:TextIconButton CssClass=MR_10
                                        Icon=deleteAll runat=server  ID=tibDeleteAllUnpriced
                                        OnClientClick="javascript:modalDeleteAlertTempDeleteAll('UnPriced',this);" 
                                        meta:resourceKey="deleteAll"
                                    />
                                    
                                    </div>
                                    <div class="clr">
                                    </div>
                               
                            </div>
                            <div class="col2 rounded" id="PricedBox">
                                <div class="columnHeader">
                                    <div class="floatLeft">
                                        <Corbis:Localize ID="pricedItemsHeader" runat="server" meta:resourcekey="pricedItemsHeader"/>
                                    </div>
                                    <!--<div class="smallIcon selectAll floatRight MR_10">
                                        <div class="actLikeLink" onclick="Corbis.Cart.showInlineAlert('IA_unpriced');">
                                            Select all
                                        </div>
                                    </div>-->
                                    <div class="clr">
                                    </div>
                                </div>
                                <div id="PricedZone" class="sameHeight">
                                    <Corbis:CartItems ParentPage="Cart" ID="pricedItems" runat="server" 
                                        Zone="Priced" />
                                    <div class="instructions displayNone" ID="pricedZoneInstruction" runat="server" >
										<Corbis:Localize ID="pricedMessage" runat="server" meta:resourcekey="pricedMessage" />
                                    </div>
                                    <div class="clr">
                                    </div>
                                </div>
                                <div class="columnHeader" style="text-align:right">
                                    <Corbis:TextIconButton CssClass=MR_10 
                                        Icon=deleteAll runat=server ID=tibDeleteAllPriced
                                        OnClientClick="modalDeleteAlertTempDeleteAll('Priced',this);" 
                                        meta:resourceKey="deleteAll"
                                    />
                                    <div class="clr">
                                    </div>
                                </div>
                            </div>
                            <div class="col3 rounded" id="PriceMultipleBox">
                                <div class="columnHeader">
                                    <h3><Corbis:Localize ID="priceMultipleImages" runat="server" meta:resourcekey="priceMultipleImages" /></h3>
                                    <div class="priceNow" id="priceNow">
                                        <Corbis:GlassButton ID="priceNowBtn" runat="server" meta:resourceKey="PriceNowBtn" />
                                    </div>
                                    <div class="clr">
                                    </div>
                                </div>
                                <div id="PriceMultipleZone" class="sameHeight">
                                    <Corbis:CartItems ParentPage="Cart" ID="pricingMultipleImages" runat="server" Zone="PriceMultiple" />
                                    <div class="instructions displayNone">
										<Corbis:Localize ID="priceMultipleMessage" runat="server" meta:resourcekey="priceMultipleMessage" />
                                    </div>
                                </div>
                                <div class="columnHeader" id=clearPriceParent style="text-align:right">
                                    <Corbis:TextIconButton CssClass=MR_10 
                                        Icon=clearAll runat=server ID=tibClearToPrice
                                        OnClientClick="javascript:clearPricingItems();"
                                        meta:resourcekey="clearItems"/>
                                    <div class="clr">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clr">
                    </div>
                </div>--%>
            </div>
            
            <div class="clr">
            </div>
            <div id="cartFooter" class="footStage">
                <h3>
                    <Corbis:Localize ID="aboutThisCartHeader" runat="server" meta:resourcekey="aboutThisCartHeader" />
                </h3>
                <p>
                    <span style="height: 15px; line-height: 15px;">
                        <Corbis:Localize ID="priceSubjectToChange" runat="server" meta:resourcekey="priceSubjectToChange" />
                    </span>
                    <Corbis:Image ID="howToPurchaseIcon" height="15" border="0" width="16" 
                            class="howToPurchaseTooltip" runat="server" ImageUrl="/Images/info.gif" 
                            meta:resourcekey="howToPurchaseIconImage" />
                    <br />
                    <Corbis:Localize ID="promoCodeNotice" runat="server" meta:resourcekey="promoCodeNotice" />
                </p>
                <div class="contactAE">
                    <span class="actLikeLink" id="cartContactLink" onclick="CorbisUI.ContactCorbis.ShowContactCorbisModal($(this).getParent().getParent()); return false;"><Corbis:Localize ID="contactAE" runat="server" meta:resourcekey="contactAE"  Visible="false"/></span>
                </div>
            </div>
        </div>
        
    </div>
    <Corbis:ModalPopup ContainerID="modalDeleteTemplate" Width="330" runat="server" meta:resourcekey="modalDeleteTemplate">                   
        <div class="ModalDeleteThumb">
            <div class="thumbWrap Relative">
                <div class="pinkyThumb" style="margin-left:0px">{0}
                </div>
            </div>
            <div class="clr">
            </div>
        </div>
        
        <div class="ModalDeleteMessage">
            <Corbis:Localize ID="modalDeleteMessage" runat="server" meta:resourcekey="modalDeleteMessage" />
            
            <div style="width:200px; margin-left: -10px;">
                <Corbis:GlassButton runat="server" 
                    ButtonStyle="Gray" 
                    ID="btnCancel" 
                    OnClientClick="MochaUI.CloseModal('modalDeleteTemplate');" 
                    CssClass="BTN_cancel" meta:resourcekey="noThanks" />
                <Corbis:GlassButton 
                    runat="server" 
                    ID="btnDelete" 
                    OnClientClick="myDeleteFunction('{1}', '{2}');MochaUI.CloseModal('modalDeleteTemplate');"
                     CssClass="BTN_delete" meta:resourcekey="btnDelete" />
            </div>
        </div>
	</Corbis:ModalPopup>
    <Corbis:ModalPopup ContainerID="modalErrorMsgTemplate" Width="330"  runat="server" meta:resourcekey="modalErrorMsgTemplate">                   
        <div class="ModalDeleteThumb">
            <div class="thumbWrap Relative">
                <div class="pinkyThumb" style="margin-left:0px">{0}
                </div>
            </div>
            <div class="clr">
            </div>
        </div>
        
        <div class="ModalDeleteMessage">
            <div >{1}</div>
            
            <div>
                <Corbis:GlassButton runat="server" 
                    ID="GlassButton4" 
                    OnClientClick="MochaUI.CloseModal('modalErrorMsgTemplate');" 
                    CssClass="BTN_delete" meta:resourcekey="continueButton" />
            </div>
        </div>
	</Corbis:ModalPopup>
    
	<Corbis:ModalPopup ContainerID="deleteCheckoutModal" runat="server" Width="400" meta:resourcekey="deleteCheckoutModal">                   
		<Corbis:GlassButton ID="clearCheckout" runat="server" CausesValidation="false" meta:resourcekey="deleteAllItems" OnClientClick="javascript:clearCheckoutItems(); return false;" style="float:left" />
		<Corbis:GlassButton ID="removeFromCart" runat="server" CausesValidation="false" meta:resourcekey="removeFromCart" OnClientClick="javascript:clearCheckoutItems(); return false;" Style="float: left" />
		<Corbis:GlassButton ID="cancelClearCheckout" runat="server" CausesValidation="false" meta:resourcekey="cancel" OnClientClick="javascript:HideModal('deleteCheckoutModal');return false;" Style="float:left" />
	</Corbis:ModalPopup>
	<Corbis:ModalPopup ContainerID="deletePriceMultipleModal" runat="server" meta:resourcekey="deletePriceMultipleModal">
		<Corbis:GlassButton ID="clearPriceMultiples" runat="server" CausesValidation="false" meta:resourcekey="deleteAllItems" OnClientClick="javascript:clearPricingItems(); return false;" />
		<Corbis:GlassButton ID="cancelClearPriceMultiples" runat="server" CausesValidation="false" meta:resourcekey="cancel" OnClientClick="javascript:HideModal('deletePriceMultipleModal');return false;" />
	</Corbis:ModalPopup> 
	<Corbis:ContactCorbis runat="server" />
	
	<%-- Display Single/Multiple images Modal popup for request price before the request price form opens up--%>
<%--    	<Corbis:ModalPopup ID="requestPriceImagesModal" ContainerID="requestPriceImagesModal" runat="server" Title='Request a price for:' >                   
            <div class="buttonsSpacing">
                <asp:RadioButtonList ID="imagesList" runat="server">
                    <asp:ListItem id="Singleimage" runat="server" meta:resourcekey="singleImageContent" CssClass="buttonsSpacing" Selected="True"></asp:ListItem>
                    <asp:ListItem id="MultipleImage" runat="server" meta:resourcekey="multipleImageContent" CssClass="buttonsSpacing"></asp:ListItem>
                </asp:RadioButtonList>		                        
            </div>
            <Corbis:GlassButton ID="cancel" runat="server" CausesValidation="false" meta:resourcekey="cancel"  CssClass="MB_10 MT_10"  ButtonBackground="e8e8e8" ButtonStyle="Gray" OnClientClick="javascript:HideModal('requestPriceImagesModal'); return false;"/>
            <Corbis:GlassButton ID="continue" runat="server" CausesValidation="false" meta:resourcekey="continue" CssClass="MB_10 MT_10" OnClientClick="javascript:openRequestPriceForSingleOrMultipleImages();return false;"/>
        </Corbis:ModalPopup>--%>
	
    <div id="Priced" style="display: none;" class="ExtendedWidth">
        <div class="ModalPopupPanelDialog ExtendedWidth">
            <div class="ModalTitleBar">
                <span class="Title">
                    <input type="image" class="Close" src="../Images/iconClose.gif" style="border-width: 0px;"
                    onclick="javascript:orangeBorderOff('Priced');return false;" />
                   <Corbis:Localize ID="pricedDeleteModalTitle" runat="server" meta:resourcekey="deleteAllModalTitle" />
                </span>
            </div>
            <div class="ModalPopupContent">
                <div>
                    <Corbis:Localize ID="pricedDeleteModalMessage" runat="server" meta:resourcekey="pricedDeleteModalMessage"/>
                </div>
                <div class="FormButtons">
                    <Corbis:GlassButton ID="PricedCancel" ButtonStyle="Gray" runat="server" CausesValidation="false" CssClass="GlassButton btnOrangedbdbdb"
                        meta:resourcekey="noThanks" OnClientClick="javascript:orangeBorderOff('Priced');return false;" />
                    <Corbis:GlassButton ID="PricedDelete" runat="server" CausesValidation="false" CssClass="GlassButton btnOrangedbdbdb"
                        meta:resourcekey="deleteThem" OnClientClick="javascript:deleteAllItems('Priced'); return false;" />
                </div>
            </div>
        </div>
    </div>
    <div id="UnPriced" style="display: none;" class="ExtendedWidth">
        <div class="ModalPopupPanelDialog ExtendedWidth">
            <div class="ModalTitleBar">
                <span class="Title">
                    <input type="image" class="Close" src="../Images/iconClose.gif" style="border-width: 0px;"
                    onclick="javascript:orangeBorderOff('UnPriced');return false;" />
                    <Corbis:Localize ID="unpricedDeleteModalTitle" runat="server" meta:resourcekey="deleteAllModalTitle" />
                </span>
                
            </div>
            <div class="ModalPopupContent">
                <div>
                    <Corbis:Localize ID="unpricedDeleteModalMessage" runat="server" meta:resourcekey="unpricedDeleteModalMessage" />
                </div>
                <div class="FormButtons">
                    <Corbis:GlassButton ID="UnPricedCancel" runat="server" ButtonStyle="Gray" CausesValidation="false" CssClass="GlassButton btnOrangedbdbdb"
                        meta:resourcekey="noThanks" OnClientClick="javascript:orangeBorderOff('UnPriced');return false;" />
                    <Corbis:GlassButton ID="UnPricedDelete" runat="server" CausesValidation="false" CssClass="GlassButton btnOrangedbdbdb"
                        meta:resourcekey="deleteThem" OnClientClick="javascript:deleteAllItems('UnPriced'); return false;" />
                </div>
            </div>
        </div>
    </div>
	<div id="unpricedToInvalidTargetModal" style="display: none;">
        <div class="ModalPopupPanelDialog">
            <div class="ModalTitleBar">
                <span class="Title">
                   <input type="image" class="Close" src="../Images/iconClose.gif" style="border-width: 0px;"
                    onclick="javascript:MochaUI.CloseModal('unpricedToInvalidTargetModal');return false;" /> 
                    <Corbis:Localize ID="youMustPriceThisImageTitle" runat="server" meta:resourcekey="youMustPriceThisImageTitle" /></span>
            </div>
            <div class="ModalPopupContent">
                <Corbis:Localize ID="youMustPriceThisImage" runat="server" meta:resourcekey="youMustPriceThisImage" />
                <div class="FormButtons">
                    <Corbis:GlassButton ID="cancelPriceImageFromInvalidModal" runat="server" ButtonStyle="Gray" CausesValidation="false"
                        meta:resourcekey="cancelPriceImageFromInvalidModal"
                        OnClientClick="javascript:MochaUI.CloseModal('unpricedToInvalidTargetModal');return false;" />
                    <Corbis:GlassButton ID="priceImageFromInvalidModal" runat="server" CausesValidation="false"
                         meta:resourcekey="priceImageFromInvalidModal"
                        OnClientClick="javascript:MochaUI.CloseModal('unpricedToInvalidTargetModal');openPriceWindow('{0}', '{1}', '{2}'); return false;" />
                    
                </div>
            </div>
        </div>
	</div>
	
	<div id="licenseAlertModal" style="display:none;">
	    <div class="ModalPopupPanelDialog">
	        <div class="ModalTitleBar">
	            <span class="Title">
	                <input type="image" class="Close" src="../Images/iconClose.gif" style="border-width:0px;" onclick="javascript:MochaUI.CloseModal('licenseAlertModal');return false;" />
                    <Corbis:Localize ID="licenseAlertTitle" runat="server" meta:resourcekey="licenseAlertTitle" /></span>
            </div>
		    <div class="ModalPopupContent">
		        <Corbis:Localize ID="licenseAlert" runat="server" meta:resourcekey="licenseAlert" />
	            <div class="FormButtons">
	                <Corbis:GlassButton ID="cancelLicenseAlertModal" runat="server" ButtonStyle="Gray" CausesValidation="false" meta:resourcekey="cancelPriceImageFromInvalidModal" OnClientClick="javascript:MochaUI.CloseModal('licenseAlertModal');return false;" />
			        <Corbis:GlassButton ID="priceImageFromLicenseAlertModal" runat="server" CausesValidation="false" meta:resourcekey="priceImageFromLicenseAlertModal" OnClientClick="javascript:MochaUI.CloseModal('licenseAlertModal');openPriceWindow('{0}', '{1}', '{2}'); return false;" />
			        
                </div>
            </div>
        </div>
	</div>
	
	<div id="someItemsCantCheckoutModal" style="display: none;">
        <div class="ModalPopupPanelDialog">
            <div class="ModalTitleBar clr">
                <span class="Title">
                    <Corbis:Localize ID="checkoutAlertTitle" runat="server" meta:resourcekey="checkoutAlert" /></span>
                <div class="clr"> </div>
            </div>
            <div class="ModalPopupContent">
                <div class="bold"><Corbis:Localize ID="checkoutAlertNote1" runat="server" meta:resourcekey="checkoutAlertNote1" /></div>
                <div class="MT_10"><Corbis:Localize ID="checkoutAlertNote2" runat="server" meta:resourcekey="checkoutAlertNote2" /></div>
                <div class="FormButtons">
                    <Corbis:GlassButton ID="GlassButton1" runat="server" ButtonStyle="Orange"
                        meta:resourcekey="continueButton"
                        OnClientClick="javascript:MochaUI.CloseModal('someItemsCantCheckoutModal');CorbisUI.Cart.BatchValidate.secondValidationStep();return false;" />
                    
                    
                </div>
            </div>
        </div>
	</div>
	
	<div id="batchItemLicenseAlert" style="display: none;">
        <div class="ModalPopupPanelDialog">
            <div class="ModalTitleBar clr">
                <span class="Title">
                    <Corbis:Localize ID="batchItemLicenseAlertTitle" runat="server" meta:resourcekey="licenseAlertTitle" /></span>
                <div class="clr"> </div>
            </div>
            <div class="ModalPopupContent">
                <div class="columnwrap relative">
                    <table border="0" cellpadding="5" cellspacing="0" width="100%">
                        <tr>
                            <td class="picture" valign="top"></td>
                            <td class="text" valign="top" style="padding-left: 20px;">
                                <div class="bold"><Corbis:Localize ID="Localize10" runat="server" meta:resourcekey="itemCannotBeCheckedOutNote1" /></div>
                                <div class="MT_10"><Corbis:Localize ID="Localize11" runat="server" meta:resourcekey="itemCannotBeCheckedOutNote2" /></div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="FormButtons MT_5 clr">
                    <Corbis:GlassButton ID="GlassButton2" runat="server" ButtonStyle="Orange"
                        meta:resourcekey="continueButton"
                        OnClientClick="javascript:MochaUI.CloseModal('batchItemLicenseAlert');CorbisUI.Cart.BatchValidate.paused = false; return false;" />
                    
                    
                </div>
            </div>
        </div>
	</div>
	
	<div id="batchPricingChangeAlert" style="display: none;">
        <div class="ModalPopupPanelDialog">
            <div class="ModalTitleBar clr">
                <span class="Title">
                    <Corbis:Localize ID="Localize2" runat="server" meta:resourcekey="pricingAlertTitle" /></span>
                <div class="clr"> </div>
            </div>
            <div class="ModalPopupContent">
                <div class="columnwrap relative">
                    <table border="0" cellpadding="5" cellspacing="0" width="100%">
                        <tr>
                            <td class="picture" valign="top"></td>
                            <td class="text" valign="top" style="padding-left: 20px;">
                                <div><Corbis:Localize ID="Localize5" runat="server" meta:resourcekey="pricingChangeNote1" /></div>
                                <div class="MT_10 bold"><Corbis:Localize ID="Localize6" runat="server" meta:resourcekey="previousPrice" /></div>
                                <div class="MT_10 bold"><Corbis:Localize ID="Localize7" runat="server" meta:resourcekey="currentPrice" /></div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="FormButtons">
                    <Corbis:GlassButton ID="GlassButton3" runat="server" ButtonStyle="Orange"
                        meta:resourcekey="continueButton"
                        OnClientClick="javascript:MochaUI.CloseModal('batchPricingChangeAlert');CorbisUI.Cart.BatchValidate.priceChangeMove(); return false;" />
                    
                    
                </div>
            </div>
        </div>
	</div>
	
	<div id="pricingChangeAlert" style="display: none;">
        <div class="ModalPopupPanelDialog">
            <div class="ModalTitleBar clr">
                <span class="Title">
                    <Corbis:Localize ID="Localize3" runat="server" meta:resourcekey="pricingAlertTitle" /></span>
                <div class="clr"> </div>
            </div>
            <div class="ModalPopupContent">
                <div class="columnwrap relative">
                    <table border="0" cellpadding="5" cellspacing="0" width="100%">
                        <tr>
                            <td class="picture" valign="top"></td>
                            <td class="text" valign="top" style="padding-left: 20px;">
                                <div><Corbis:Localize ID="Localize4" runat="server" meta:resourcekey="pricingChangeNote1" /></div>
                                <div class="MT_10 bold"><Corbis:Localize ID="Localize12" runat="server" meta:resourcekey="previousPrice" /></div>
                                <div class="MT_10 bold"><Corbis:Localize ID="Localize13" runat="server" meta:resourcekey="currentPrice" /></div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="FormButtons">
                    <div style="display: none;" class="dataToPass"></div>
                    <Corbis:GlassButton ID="GlassButton5" runat="server" ButtonStyle="Orange"
                        meta:resourcekey="continueButton"
                        OnClientClick="javascript:MochaUI.CloseModal('pricingChangeAlert');CorbisUI.Cart.priceChangeConfirmed(JSON.decode($(this).getParent('div.FormButtons').getElement('div.dataToPass').get('text'))); return false;" />
                    
                    
                </div>
            </div>
        </div>
	</div>
	
	
	
	<div id="batchValidatorIndicator">
	    <div class="dimmer"> </div>
	    <div class="notice">
	        <!-- <img src="/Images/ajax-loader-dark-transparent.gif" width="31" height="31" alt="" title="" /> -->
	        <Corbis:Image ID="ajaxLoaderDarkTransparent" runat="server" width="31" height="31" ImageUrl="/Images/ajax-loader2.gif" meta:resourcekey="ajaxLoaderDarkTransparentImage" />
	        <div class="standby"><strong><Corbis:Localize ID="Localize8" runat="server" meta:resourcekey="pleaseStandby" /></strong></div>
	        <div class="clr white"><strong><Corbis:Localize ID="Localize9" runat="server" meta:resourcekey="confirmingYourItems" /></strong></div>
	        <div class="clr MT_10"> </div>
	        <Corbis:GlassButton ID="cancelBatchValidationButton" runat="server" ButtonStyle="Orange"
	                    ButtonBackground="gray4b"
                        meta:resourcekey="cancelPriceImageFromInvalidModal"
                        OnClientClick="javascript:CorbisUI.Cart.BatchValidate.cancelValidation();return false;" />
	    </div>
	</div>
    <div id="searchProgIndicator" >
        <div class="mask"></div>
        <div id="searchProgContents">
	        <img border="0" alt="" src="/images/ajax-loader2.gif" />
	        <br />
	        <h1><Corbis:Label ID="searching" runat="server" meta:resourcekey="searching" /></h1>
	        <div id="processingFilters">
	            <Corbis:Label ID="procFilters" runat="server" meta:resourcekey="procFilters" />
	        </div>
        </div>
    </div>	
</asp:Content>
