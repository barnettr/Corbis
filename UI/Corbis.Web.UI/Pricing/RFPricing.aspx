<%@ Page Language="C#" AutoEventWireup="true" Codebehind="RFPricing.aspx.cs" MasterPageFile="~/MasterPages/ModalPopup.Master" Inherits="Corbis.Web.UI.Pricing.RFPricingPage" %>
<%@ Register Assembly="Corbis.Web.UI.Controls" Namespace="Corbis.Web.UI.Controls" TagPrefix="Corbis" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagName="PricingHeader" TagPrefix="Corbis" Src="PricingHeader.ascx" %>
<%@ Register TagPrefix="UC" TagName="InstantService" src="../Chat/UserControl/InstantServiceButtonSmall.ascx" %>
<%@ Register TagName="Restrictions" TagPrefix="IR" Src="../src/Image/Restrictions.ascx"  %>
<%@ Register TagPrefix="Corbis" TagName="ContactCorbis" Src="~/UserControls/ContactControls/ContactCorbis.ascx" %>

<asp:Content ID="rfPricingContent" ContentPlaceHolderID="mainContent" runat="server">
    <div id="OuterContainer" class="OuterContainer" onkeydown="CorbisUI.Pricing.RF.checkTab(this);">

    
            <div id="RFTitleBar" visible="true" class="TitleBar" runat="server">
            
                <div class="PriceImageTitle">
                    <Corbis:Label ID="pageTitle" CssClass="Title" meta:resourcekey="pageTitle" runat="server" />
                    <span class="SubTitle"><Corbis:Label ID="pageSubTitle" meta:resourcekey="pageSubTitle" runat="server" /></span>

                </div>
                
                
                <div class="CloseButton" style="border:0px;">
                    <a href="javascript:CorbisUI.Pricing.RF.updatedPricingDataCheck(this);"><img alt="" id="XClose" class="btnRFClose" style="border:0px;" src="/Images/iconClose.gif" /></a>
                </div>
                
                <div class="PrintDiv" meta:resourcekey="printDiv" runat="server" visible="false">
                    <asp:HyperLink runat="server" NavigateUrl="#" ID="printImgHyperlink" CssClass="PrintIconImg" meta:resourcekey="printImgHyperlink" />
                </div>
                
                <div class="ChatDiv" meta:resourcekey="chatDiv" runat="server">
                    <UC:InstantService id="instantService1" runat="server" />
                </div>
                
            </div>
            <asp:UpdatePanel ChildrenAsTriggers="true">
                <ContentTemplate>
                    <Corbis:ModalPopup ContainerID="confirmClose"  runat="server" Width="300" meta:resourcekey="confirmClose">  
                        <Corbis:GlassButton ID="cancelClose" runat="server" CausesValidation="false" meta:resourcekey="cancelClose"  OnClientClick="HideModal('confirmClose');return false;"  />
                        <Corbis:GlassButton ID="continueClose" runat="server" cssClass="btnGraydbdbdb" CausesValidation="false" meta:resourcekey="continueClose" OnClientClick="PricingModalPopupExit();return false;" />        
      	            </Corbis:ModalPopup>
                </ContentTemplate>
            </asp:UpdatePanel>
            
            <Corbis:PricingHeader id="pricingHeader" runat="server" />




            <div class="PricingGrid">
                <input type="hidden" name="hidAttributeUID" id="hidAttributeUID" runat="server" />
                <input type="hidden" name="hidAttributeValueUID" id="hidAttributeValueUID" runat="server" />
                
                
                <div class="RepeaterContainer rounded4" isInternal="true" isbottomonly="false">
                    <div id="customPricedDisplay" runat="server">
                        <div class="LicensingDetailsHeader">
                           <h3><asp:Label runat="server" ID="lblLicensingDetails" meta:resourcekey="lblLicensingDetails" /></h3>
                        </div>
                        <div class="licenseWrapper">
                            <div class="ffffcd">
                                <div id="contactNoAE" class="innerPadding" visible="true" runat="server">
                                    <img src="../../Images/alertYellow.gif" alt="" style="vertical-align: middle; margin-bottom: 3px" /> &nbsp;<asp:Label runat="server" ID="alertMessage" CssClass="boldAlert" meta:resourcekey="lblCustomPriceAlertMessage" />
                                </div>
                                <div id="contactAE" class="innerPadding" visible="false" runat="server">
                                    <img src="../../Images/alertYellow.gif" alt="" style="vertical-align: middle; margin-bottom: 3px" /> &nbsp;<asp:Label runat="server" ID="Label1" CssClass="boldAlert" meta:resourcekey="lblCustomPriceAlertMessageAE" />
                                </div>
                            </div>
                            <div id="fileSizeWrapper" class="fileSizeWrapper" visible="true" runat="server">
                                <asp:Label runat="server" ID="fileSize" CssClass="boldAlert" meta:resourcekey="lblFileSize" />
                                <asp:Label runat="server" CssClass="createSpace" ID="fileSizeValue"/>
                            </div>
                            <div id="dimensionSizeWrapper" class="dimensionSizeWrapper" visible="true" runat="server">
                                <asp:Label runat="server" ID="dimensions" CssClass="boldAlert" meta:resourcekey="lblDimensions" />
                                <asp:Label runat="server" CssClass="createSpaceTwo" ID="dimensionsValue"/>
                            </div>
                             <div id="itemNotAvailable" class="itemNotAvailable" visible="false" runat="server">
                                <p><Corbis:Label ID="lblApologyStatement" runat="server" meta:resourcekey="lblApologyStatement" /></p>
                                <p><Corbis:Label ID="lblContactStatement" runat="server" meta:resourcekey="lblContactStatement" /></p>
                                <Corbis:GlassButton ID="btnContactAE" CssClass="btnContactAE" meta:resourcekey="btnContactAE" runat="server" CausesValidation="false" />
                            </div>
                        </div>                    
                    </div>
                    <asp:Repeater ID="rpRFPricing" runat="server" OnItemDataBound="RFPricing_ItemDataBound">
                         <HeaderTemplate>
                            <div class="LicensingDetailsHeader">
                                <h3><asp:Label runat="server" ID="lblLicensingDetails" meta:resourcekey="lblLicensingDetails" /></h3>
                            </div>
                            <div class="RFPricingRepeater" id="repeaterInnerDiv">
                        </HeaderTemplate>
                        
                        <ItemTemplate>
                        <div class="Clear ListItemWrap">
                        <div class="RFPricingRowLeft" id="RFPricingRowLeft" runat="server">&nbsp;</div>
                        <div class="RFPricingRepeaterDataRow" id="RFPricingRepeaterDataRow" runat="server">
                            <input type="hidden" name="AttributeUID" id="AttributeUID" runat="server"  value='<%# Eval("AttributeUid") %>' />
                            <input type="hidden" name="ValueUID" id="ValueUID" runat="server" value='<%# Eval("ValueUid") %>' />
                            <input type="hidden" name="EffectivePrice" id="EffectivePrice" runat="server" value='<%# Eval("EffectivePrice") %>' />
                             <input type="hidden" name="EffectivePriceLocalized" id="EffectivePriceLocalized" runat="server" value='<%# Eval("EffectivePrice") %>' />
                            <input type="hidden" name="currecyCode" id="currecyCode" runat="server" value='<%# Eval("CurrencyCode") %>' />
                            
                            <ul style="width:100%">
                                <li style="width:25%">
                                    <asp:Label   ID="lblDisplayText" runat="server" Text='<%# Eval("ImageSize") %>'></asp:Label>
                                    <asp:Label   ID="uncompressedFileSize" runat="server" Text='<%# Eval("UncompressedFileSize") + "*" %>'  ></asp:Label>
                                </li>                                
                                <li style="width:50%; text-align:center">
                                    <asp:Label  ID="pixelHeight" runat="server" Text='<%# Eval("PixelHeight") %>'></asp:Label>                                 
                                    <asp:Label  ID="pixelWidth" runat="server" Text='<% # Eval("PixelWidth") %>'></asp:Label>
                                    &#149;                                 
                                    <asp:Label  ID="imageheight" runat="server" Text='<%# Eval("ImageHeight") %>'></asp:Label>                               
                                    <asp:Label  ID="imageWidth" runat="server" Text='<%# Eval("ImageWidth") %>'></asp:Label>                                    
                                    <asp:Label  ID="resolution" runat="server" Text='<%# Eval("Resolution") %>'></asp:Label> 
                                </li>
                              
                                <li class="Right" style="width:25%">
                                    <asp:Label  ID="lblPriceText" runat="server" Text='<%# Eval("EffectivePrice") %>'></asp:Label>
                                </li>
                            </ul>
                            </div>
                            <div class="RFPricingRowRight" id="RFPricingRowRight" runat="server">&nbsp;</div>
                            </div>
                          </ItemTemplate>
                          <FooterTemplate>
                            </div>
                          </FooterTemplate>
                          
                    </asp:Repeater>
                    <asp:Label runat="server" CssClass="RFPricingRepeaterNoResults" Visible="true" ID="rpRFPricingLabel" meta:resourcekey="rpRFPricingLabel" />
                 
                </div>
            </div>
            <div id="PricingBottom" class="PricingBottom" runat="server" visible="true">
                <div class="PriceMultipleRF">
                    <asp:Label runat="server" ID="lblPriceMayVary" meta:resourcekey="lblPriceMayVary" />
                </div>
                <div class="ViewRestrictions">
                    <Corbis:TextIconButton ID="TextIconButton1" runat="server" Icon="yellowAlert" OnClientClick="CorbisUI.Pricing.RF.OpenRestrictions(this)" meta:resourcekey="lnkViewRestrictions" />
                </div>
            </div>
            <div id="PricingBottomAE" class="PricingBottomAE" runat="server" visible="false">
                <div class="PriceMultipleRF">
                    <asp:Label runat="server" ID="_lblPriceMayVary" meta:resourcekey="lblPriceMayVary" />
                </div>
                <div class="ViewRestrictions">
                    <Corbis:TextIconButton ID="TextIconButton2" runat="server" Icon="yellowAlert" OnClientClick="CorbisUI.Pricing.RF.OpenRestrictions(this)" meta:resourcekey="lnkViewRestrictions" />
                </div>
            </div>   
            
            <Corbis:ModalPopup ID="ModalPopup2" ContainerID="restrictionsPopup" runat="server" Width="550" meta:resourcekey="restrictionsPopup">  
                <IR:restrictions id="ImageRestrictions" ShowHeader="false" runat="server" />
            </Corbis:ModalPopup>
            
            <Corbis:ModalPopup ID="ModalPopup1" ContainerID="priceStatusPopup" Width="350" runat="server" meta:resourcekey="priceStatusPopup">
        
                <Corbis:GlassButton ID="priceStatusCloseButton" runat="server" meta:resourcekey="priceStatusCloseButton" OnClientClick="HideModal('priceStatusPopup');return false;" />
            </Corbis:ModalPopup>
    
            <%--<div class="PricingFooterRF" id="PricingFooterAE" runat="server" visible="false">
                <asp:Label runat="server" Visible="false" ID="lblPricingFooterDisclaimerStrong" meta:resourcekey="lblPricingFooterDisclaimerStrong" />
                <asp:Label runat="server" ID="lblPricingFooterDisclaimer" CssClass="lblPricingFooterDisclaimer" meta:resourcekey="lblPricingFooterDisclaimer" />
            </div>--%>
            <div class="PricingFooterRF" id="PricingFooter" runat="server" visible="true">
                 <span id="priceFooter" class="priceFooter" visible="true" runat="server">
                        <asp:Label runat="server" ID="customPriceFooter" meta:resourcekey="lblCustomPriceFooter" />
                  </span>
                  <asp:Label runat="server" ID="lblPricingFooterDisclaimerStrong" meta:resourcekey="lblPricingFooterDisclaimerStrong" />
                <asp:Label runat="server" ID="lblPricingFooterDisclaimer" CssClass="lblPricingFooterDisclaimer" meta:resourcekey="lblPricingFooterDisclaimer" />
            </div>
            
    </div>
    
    <%--Pricing alert popup--%>
    <div id="customPriceExpired" style="width:350px; height:196px;display:none;">
        <div id="titleWrapper" class="titleWrapper">
            <span id="expiredTitle" class="expiredTitle"><Corbis:Localize ID="title" runat="server" meta:resourcekey="expiredTitle" /></span>
            <div class="expiredCloseButton" id="expiredCloseButton">
                <Corbis:Image ID="customPriceExpiredPopupCloseImage" ImageUrl="/Images/iconClose.gif" runat="server" onclick="return HideModal('customPriceExpired');return false;" class="Close" meta:resourcekey="contactUsForPriceCloseImage"/>
            </div>
        </div>
        <div id="dataContainer" class="dataContainer">
            <div id="expiredStrongBody" class="expiredStrongBody">
                <p><Corbis:Label ID="strongStatement" runat="server" meta:resourcekey="strongStatement" /></p>
            </div>
            <br />
            <div id="expiredMainBody">
                <p><Corbis:Label ID="mainStatement" runat="server" meta:resourcekey="mainStatement" /></p>
            </div>
            <Corbis:GlassButton ID="expiredGlass" CssClass="expiredGlass" meta:resourcekey="expiredGlass" runat="server" CausesValidation="false" OnClientClick="HideModal('customPriceExpired');return false;" />
        </div>
    </div>
    <%--End Pricing alert popup--%>
	<Corbis:ContactCorbis runat="server" />

    <script type="text/javascript" language="javascript">
        CorbisUI.Pricing.RF.vars.priceLabel = $('<%= pricingHeader.piPrice.ClientID %>');
        CorbisUI.Pricing.RF.vars.localizedValue = "";
        CorbisUI.Pricing.RF.vars.priceCode = $('<%= pricingHeader.piPriceCode.ClientID %>');
        CorbisUI.Pricing.RF.vars.hidAttributeUID = $('<%= hidAttributeUID.ClientID %>');
        CorbisUI.Pricing.RF.vars.hidAttributeValueUID = $('<%= hidAttributeValueUID.ClientID %>');
        CorbisUI.Pricing.RF.vars.cartButton = $('<%= pricingHeader.cartButton.ClientID %>');
        CorbisUI.Pricing.RF.vars.lightboxButton = $('<%=pricingHeader.lightboxButton.ClientID %>');
        CorbisUI.Pricing.RF.vars.myPricingList = new CorbisUI.Pricing.RF.PricingList();

        window.addEvent('domready', CorbisUI.Pricing.RF.DomReady);
        window.addEvent('load', CorbisUI.Pricing.RF.Resize);
    </script>
</asp:Content>


