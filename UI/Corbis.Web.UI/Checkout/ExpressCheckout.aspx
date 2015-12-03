<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExpressCheckout.aspx.cs"
    Inherits="Corbis.Web.UI.Checkout.ExpressCheckout" MasterPageFile="/MasterPages/ModalPopup.Master"
    Title="<%$ Resources: windowTitle %>" %>
<%@ Import Namespace="Corbis.Framework.Globalization"%>    
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register Assembly="Corbis.Web.UI.Controls" Namespace="Corbis.Web.UI.Controls" TagPrefix="Corbis" %>
<%@ Register TagName="Restrictions" TagPrefix="IR" Src="../src/Image/Restrictions.ascx" %>
<%@ Register TagPrefix="UC" TagName="InstantService" Src="../Chat/UserControl/InstantServiceButtonSmall.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="ExpressCheckoutPaymentMethod" Src="~/Checkout/ExpressCheckout_PaymentMethod.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="ContactCorbis" Src="~/UserControls/ContactControls/ContactCorbis.ascx" %>

<asp:Content ID="ExpressCheckoutPage" ContentPlaceHolderID="mainContent" runat="server">
    <% //Image Header %>
    <div id="TitleBar" visible="true" class="TitleBar" runat="server">
        <div class="ExpressCheckoutImageTitle">
            <h1>
                <Corbis:Label ID="pageTitle" CssClass="Title" meta:resourcekey="pageTitle" runat="server" />
                <span class="SubTitle">
                    <Corbis:Label ID="imageLicenseType" runat="server" /></span>
            </h1>
        </div>
        <div class="CloseButton noUnderline">
            <a href="javascript:void(0);" onclick="CorbisUI.ExpressCheckout.CloseExpressCheckoutModal(this);">
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
    <input type="hidden" name="hidAttributeUID" id="hidAttributeUID" runat="server" />
    <input type="hidden" name="hidAttributeValueUID" id="hidAttributeValueUID" runat="server" />
    <input type="hidden" name="hidProductUid" id="hidProductUid" runat="server" />
    <input type="hidden" name="hidSavedUsageUid" id="hidSavedUsageUid" runat="server" />
    <input type="hidden" name="hidPriceCode" id="hidPriceCode" meta:resourcekey="lblCurrencyCode" runat="server" />
    <input type="hidden" name="hidCustomPricingExpired" id="hidCustomPricingExpired" runat="server" />
    
    <div id="expressCheckoutStageWrap">
<%--            
SuccessScript="alert('mike');"  
ctl00$mainContent$purchaseButton$GlassButton
--%>            
    

    

        <div class="ImageDetails">
            <div class="Thumbnail">
                <div class="Inner">
                    <Corbis:Image ID="imageThumbnail" IsAbsolute="true" runat="server" />
                </div>
            </div>
            <div class="ImageInfo">
                <br />
                <div class="ImageTitle">
                    <Corbis:Label ID="imageTitle" runat="server" />
                </div>
                <div class="ImageId">
                    <Corbis:Label ID="imageId" runat="server" />
                </div>
                <div class="PricingTier">
                    <Corbis:Label ID="pricingTier" runat="server" />
                     <Corbis:Label ID="licenseModal" runat="server" />
                </div>
            </div>
        </div>
        <div class="PageDetails">
            <h1>
                <Corbis:Label runat="server" ID="youAreAboutToPurchase"  /></h1>
            <p>
                <Corbis:Label runat="server" ID="pageDescription" meta:resourcekey="pageDescription" /></p>
        </div>
        <div class="ColumnedLayout TwoColumn_Fixed_LM" id="expressCheckoutDisplayColumns">
            <div class="TwoColumn_leftWrap">
                <div class="TwoColumn_mainWrap">
                    <div class="TwoColumn_mainColumn">
                    
                        <div class="purchaseInfo rounded4">
                            <div class="mask" runat="server" meta:resourcekey="step3Requirements"></div>
                            <Corbis:Label runat="server" CssClass="roundedHeaders" ID="purchaseTitle" meta:resourcekey="purchaseTitle" />
                            <div class="PricingBox rounded4">
                                <div class="innerWrap">
                                    <Corbis:Label runat="server" CssClass="darkGrayBold" ID="selectPaymentTitle" meta:resourcekey="selectPaymentTitle" />
                                    <div id="paymentMethodHolder" style="margin:0;">
                                        <select id="paymentMethodSelect">
                                            <Corbis:ExpressCheckoutPaymentMethod runat="server" ID="paymentMethodControl"></Corbis:ExpressCheckoutPaymentMethod>
                                        </select>
                                    </div>
                                    <div class="clr smallClear"></div>
                                    <div class="CreditCardExpiryError Error displayNone">
                                        <Corbis:Label runat="server" ID="expiredCreditCardError" meta:resourcekey="expiredCreditCardError" />
                                    </div>
                                    <div class="creditCardValidationCodeBox displayNone">
                                        
                                        <Corbis:Label runat="server" ID="creditCardValidationCodeLbl" meta:resourcekey="creditCardValidationCodeLbl" />
                                        <Corbis:TextBox runat="server" ID="creditCardValidationCode" Width="55"  MaxLength="4" autocomplete="off"/>
                                         <img onmouseover="javascript:CorbisUI.ExpressCheckout.FixValidationCodeCursor('true',this);" onmouseout="javascript:CorbisUI.ExpressCheckout.FixValidationCodeCursor('false',this);" src="/Images/spacer.gif" title='<Corbis:Localize ID="ccLocation" runat="server" meta:resourcekey="creditCardToolTip"/>'
                                                rel='<div id="ccVerificationHoverLeft"><img alt="" runat="server" meta:resourcekey="MCVisaVerification" /></div>
                                                <div id="ccVerificationHoverRight"><img alt="" runat="server" id="Img2"  meta:resourcekey="AmexVerification" /></div>'
                                                class="thumbWrap infoIcon" /> 
                                    </div>                                   
                                    <div class="clr smallClear">&nbsp;</div>
                                    <div class="dottedDivider">&nbsp;</div>
                                    <div class="clr extraSmallClear">&nbsp;</div>
                                    <div id="purchaseCheckboxes">
                                        <Corbis:ImageCheckbox runat="server" OnClientChanged="CorbisUI.ExpressCheckout.validateStep3()" Checked="false" ID="acceptRestrictions" />
                                        <p class="checkThisText">
                                            <Corbis:Label runat="server" ID="acceptRestrictionsText"  meta:resourcekey="acceptRestrictions" />
                                        </p>
                                        <Corbis:ImageCheckbox runat="server" OnClientChanged="CorbisUI.ExpressCheckout.validateStep3()" Checked="false" ID="acceptLicenseAgreement" />
                                        <p class="checkThisText">
                                            <Corbis:Label runat="server" ID="licenseAgreeement1" meta:resourcekey="acceptLicenseAgreement1" /> 
                                            <a runat="server" id="eulaLink" target="_blank"><Corbis:Label runat="server" ID="licenseAgreementLink" meta:resourcekey="acceptLicenseAgreementLink" /></a>.
                                            <Corbis:Label runat="server" ID="licenseAgreeement2" meta:resourcekey="acceptLicenseAgreement2" />                                      
                                        </p>
                                    </div>
                                    <div class="dottedDivider">&nbsp;</div>
                                    
                                    <div id="ajaxPriceLoader" class="ajaxPriceLoader">
                                        <div class="msg">&nbsp;</div>
                                    </div>
                                    
                                    <div id="pricingPane">
                                        <Corbis:Label runat="server" ID="lblPrice" CssClass="pricingLabels" meta:resourcekey="lblPrice" />
                                        <div class="PriceIndicator pricingValues">
                                            <span class="Price" id="piPrice" runat="server"></span> <span class="PriceCurrency"
                                                id="piPriceCode" runat="server">
                                                <asp:Label runat="server" ID="lblCurrencyCode" meta:resourcekey="lblCurrencyCode" /></span>
                                        </div>
                                        <div class="clr smallClear">
                                            &nbsp;</div>
                                        <Corbis:Label runat="server" ID="promotionLabel" CssClass="pricingLabels" meta:resourcekey="lblPromotion" />
                                        <div class="pricingValues">
                                            <span class="Price" id="piPromotion" runat="server"></span> <span class="PriceCurrency"
                                                id="piPromotionCode" runat="server"></span>
                                        </div>
                                        <div class="clr smallClear">
                                            &nbsp;</div>
                                        <div id="ksaHolder" runat="server" visible="false">
                                            <Corbis:Label runat="server" ID="ksaLabel"  CssClass="pricingLabels"  meta:resourcekey="lblKsa" />
                                            <div class="pricingValues">
                                                <span class="Price" id="piKsa" runat="server"></span> <span class="PriceCurrency"
                                                    id="piKsaCode" runat="server"></span>
                                            </div>
                                            
                                            <div class="clr smallClear">
                                                &nbsp;</div>
                                        </div>
                                        <Corbis:Label runat="server" ID="taxLabel" meta:resourcekey="lbltax"  CssClass="pricingLabels"/>
                                        <div class="pricingValues">
                                            <span class="Price" id="piTax" runat="server"></span> <span class="PriceCurrency"
                                                id="piTaxCode" runat="server"></span>
                                        </div>
                                        <div class="clr smallClear">
                                            &nbsp;</div>
                                        <div class="totalsWrap">
                                            <Corbis:Label runat="server" CssClass="pricingLabels totalLabel" ID="totalLabel"
                                                meta:resourcekey="lblTotal" />
                                            <div class="pricingValues">
                                                <span class="Price Bold" id="piTotal" runat="server"></span> <span class="PriceCurrency Bold"
                                                    id="piTotalCode" runat="server">
                                                    <asp:Label runat="server" ID="lblTotalCode" meta:resourcekey="lblCurrencyCode" /></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clr smallClear">
                                        &nbsp;</div>
                                    <div class="promoCodeBox">
                                        
                                        <Corbis:Label runat="server" CssClass="pricingLabels" ID="promotionCodeLabel" meta:resourcekey="lblPromotionCode" />
                                        <div class="pricingValuesPromo">                                        
                                            <Corbis:TextBox runat="server" CssClass="promotionCode" ID="promotionCode" Width="55"  />
                                            
                                            <Corbis:GlassButton runat="server" Enabled="false" ID="promotionCodeButton" CssClass="promotionCodeButton noUnderline" ButtonStyle="Outline" meta:resourcekey="btnApply" />
                                        </div>
                                        <div class="clr extraSmallClear"></div>
                                    </div>
                                    <div class="clr smallClear">
                                        &nbsp;</div>
                                    <div class="purchaseButton" runat="server" >

                                    <div id="purchaseButtonMask" class="purchaseButtonMask" runat="server" meta:resourcekey="step4Requirements"></div>
<%--                                    
                                        <Corbis:GlassButton runat="server" ID="purchaseButton" meta:resourcekey="purchaseButton" OnClientClick="
                                        if(!CorbisUI.ExpressCheckout.submitValidationCheck()) return false;
                                        _ExpressCheckoutValidation.validateAll(); 
                                        return false;" />
--%>

                                        <Corbis:GlassButton runat="server" ID="purchaseButton" meta:resourcekey="purchaseButton" OnClientClick="
                                        CorbisUI.ExpressCheckout.ValidateProjectASCText();
                                         
                                        return false;" />
                                    
                                    </div>
                                    <div id="important" class="agessaMessage" style="display:none;">
                                        <Corbis:Label runat="server" ID="important" meta:resourcekey="importantMessage" />
                                    </div>
                                    <div id="agessaBox" class="agessaMessage" style="display:none;">
                                        <Corbis:Label runat="server" ID="agessageMessage" meta:resourcekey="agessaMessage" />
                                    </div>
                                    <div id="taxErrorBox" class="agessaMessage" style="display:none;">
                                        <Corbis:Label runat="server" ID="taxErrorMessage" meta:resourcekey="taxErrorMessage" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <div class="floatBottomR">
                            <div id="TrusteGraphic" class="TrusteGraphic">
                                <Corbis:HyperLink NavigateUrl="javascript:CorbisUI.ExpressCheckout.OpenLegal();"
                                    ID="truste" CssClass="noUnderline" Localize="true" ImageUrl="/Images/truste.gif"
                                    runat="server" />
                            </div>
                            
                            <a title="https://secure.corbis.com" style="cursor:default;border-width: 0px;"><img alt="https://secure.corbis.com" style="border-width: 0px;" src="../Images/secure.gif"/></a>
                        </div>
                    </div>
                </div>
                <div class="TwoColumn_leftColumn">
                    <div class="leftInnerWrap">
                        
                        <div class="projectInfo rounded4" id="projectInfoId">
                            
                            <Corbis:Label runat="server" CssClass="roundedHeaders" ID="projectDetails" meta:resourcekey="projectDetails" />
                            <% //Validation %>
                            <div class="displayNone Error" id="projectValidate">
                                <div class="weightNormal"><asp:PlaceHolder ID="validatorContainer" runat="server"><Corbis:Localize runat="server" ID="validtionLocalizedMsg" Text="<%$ Resources: Resource, ContainsNonAsciiCharacters %>" /></asp:PlaceHolder></div>
                            </div>
                            <Corbis:ValidationGroupSummary HighlightRows="true" ID="projectValSummary" runat="server"
                                ValidationGroup="Project" DisplayMode="BulletList" ShowMessageBox="false" ShowSummary="true" />
                            <asp:CustomValidator ID="nonAsciiValidator" runat="server" Display="None" ValidationGroup="Project" />
                            <% //End Validation %>
                            
                            <div class="projectWrap rounded4">
                                <table cellspacing="0">
                                    <tr>
                                        <td class="projectFieldLabel">
                                            <Corbis:Localize ID="projectName" runat="server" meta:resourcekey="projectName" />
                                        </td>
                                        <td class="projectFieldLabel">
                                            <Corbis:Localize ID="jobNumber" runat="server" meta:resourcekey="jobNumber" />
                                        </td>
                                        <td class="projectFieldLabel">
                                            <Corbis:Localize ID="poNumber" runat="server" meta:resourcekey="poNumber" />
                                        </td>
                                        <td class="projectFieldLabel">
                                            <Corbis:Localize ID="licensee" runat="server" meta:resourcekey="licensee" />
                                            <Corbis:Image runat="server" ID="licenseeImageInfoIcon" ImageUrl="/Images/spacer.gif" CssClass="thumbWrap infoIcon" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="projectFields projectNameField">
                                            <Corbis:TextBox runat="server" ID="projectField" Width="150"  MaxLength="50"  />
                                        </td>
                                        <td class="projectFields jobField">
                                            <Corbis:TextBox Width="100" runat="server" ID="jobField"  MaxLength="50" />
                                        </td>
                                        <td class="projectFields purchaseOrderField">
                                            <Corbis:TextBox Width="100" runat="server" ID="poField"  MaxLength="50" />
                                        </td>
                                        <td class="projectFields licenseeField">
                                            <Corbis:TextBox runat="server" ID="licenseeField" Width="130"  MaxLength="50" WatermarkText="<%$ Resources: Resource, Required %>"
                                                WatermarkCssClass="Required" />
                                        </td>
                                    </tr>
                                </table>
                                
                            </div>
                        </div>
                        <div class="selectUse rounded4">
                            <div class="mask" runat="server"  meta:resourcekey="step2Requirements">
                            </div>
                            <Corbis:Label runat="server" CssClass="roundedHeaders" ID="selectUseTitle" meta:resourcekey="selectUse" />
                            <div class="selectUseInner">
                                <div id="RFCustomPricedDisplay" class="customPricedDisplay rounded4" runat="server">
                                    <div class="LicensingDetailsHeader">
                                        <h3>
                                            <asp:Label runat="server" ID="lblLicensingDetails" meta:resourcekey="lblLicensingDetails" /></h3>
                                    </div>
                                    <div class="licenseWrapper">
                                        <div class="yellowNotification Bold">
                                            <div id="pricedByAEDiv_RF" class="innerPadding" visible="true" runat="server">
                                                <img src="../../Images/alertYellow.gif" alt="" style="vertical-align: middle; margin-bottom: 3px" />
                                                &nbsp;<asp:Label runat="server" ID="alertMessage" CssClass="boldAlert" meta:resourcekey="pricedByAE" />
                                            </div>
                                            <div id="contactAEDiv_RF" class="innerPadding" visible="false" runat="server">
                                                <img src="../../Images/alertYellow.gif" alt="" style="vertical-align: middle; margin-bottom: 3px" />
                                                &nbsp;<asp:Label runat="server" ID="Label1" CssClass="boldAlert" meta:resourcekey="lblCustomPriceAlertMessageAE" />
                                            </div>
                                        </div>
                                        <div id="fileSizeWrapper" class="fileSizeWrapper" visible="true" runat="server">
                                            <asp:Label runat="server" ID="fileSize" CssClass="Bold" meta:resourcekey="lblFileSize" />
                                            <asp:Label runat="server" CssClass="createSpace" ID="fileSizeValue" />
                                        </div>
                                        <div id="dimensionSizeWrapper" class="dimensionSizeWrapper" visible="true" runat="server">
                                            <asp:Label runat="server" ID="dimensions" CssClass="Bold" meta:resourcekey="lblDimensions" />
                                            <asp:Label runat="server" CssClass="createSpaceTwo" ID="dimensionsValue" />
                                        </div>
                                        <div id="priceFooter" class="priceFooter" visible="true" runat="server">
                                            <asp:Label runat="server" ID="customPriceFooter" meta:resourcekey="lblCustomPriceFooter" />
                                        </div>
                                        <div id="itemNotAvailable" class="itemNotAvailable" visible="false" runat="server">
                                            <p>
                                                <Corbis:Label ID="lblApologyStatement" runat="server" meta:resourcekey="lblApologyStatement" /></p>
                                            <p>
                                                <Corbis:Label ID="lblContactStatement" runat="server" meta:resourcekey="lblContactStatement" /></p>
                                            <Corbis:GlassButton ID="btnContactAE" CssClass="btnContactAE" meta:resourcekey="btnContactAE"
                                                runat="server" CausesValidation="false" />
                                        </div>
                                    </div>
                                </div>
                                <div id="RFPricingWrap" runat="server" class="RFPricingWrap">
                                    <asp:Repeater ID="rpRFPricing" runat="server" OnItemDataBound="RFPricing_ItemDataBound">
                                        <HeaderTemplate>
                                            <div class="RFPricingRepeater rounded4" id="repeaterInnerDiv">
                                                <div class="LicensingDetailsHeader">
                                                    <h3>
                                                        <asp:Label runat="server" ID="lblLicensingDetails" meta:resourcekey="lblLicensingDetails" /></h3>
                                                </div>
                                                <div class="RFPricesInnerWrap">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div class="Clear ListItemWrap">
                                                <div class="RFPricingRowLeft" id="RFPricingRowLeft" runat="server">
                                                    &nbsp;</div>
                                                <div class="RFPricingRepeaterDataRow" id="RFPricingRepeaterDataRow" runat="server">
                                                    <input type="hidden" name="AttributeUID" id="AttributeUID" runat="server" value='<%# Eval("AttributeUid") %>' />
                                                    <input type="hidden" name="ValueUID" id="ValueUID" runat="server" value='<%# Eval("ValueUid") %>' />
                                                    <input type="hidden" name="HidEffectivePrice" id="HidEffectivePrice" runat="server" value='<%# Eval("EffectivePrice") %>' />
                                                     <input type="hidden" name="Currencycode" id="CurrencyCode" runat="server" value='<%# Eval("Currencycode") %>' />
                                                    <ul runat="server">
                                                        <li style="width: 135px;">
                                                            <asp:Label ID="lblDisplayText" runat="server" Text='<%# Eval("ImageSize") %>'></asp:Label>
                                                            <asp:Label ID="uncompressedFileSize" runat="server" Text='<%# Eval("UncompressedFileSize") + "*" %>'></asp:Label>
                                                        </li>
                                                        <li>
                                                            <asp:Label ID="pixelHeight" runat="server" Text='<%# Eval("PixelHeight") %>'></asp:Label>
                                                            <asp:Label ID="pixelWidth" runat="server" Text='<% # Eval("PixelWidth") %>'></asp:Label>
                                                            &#149;
                                                            <asp:Label ID="imageheight" runat="server" Text='<%# Eval("ImageHeight") %>'></asp:Label>
                                                            <asp:Label ID="imageWidth" runat="server" Text='<%# Eval("ImageWidth") %>'></asp:Label>
                                                            <asp:Label ID="resolution" runat="server" Text='<%# Eval("Resolution") %>'></asp:Label>
                                                        </li>
                                                        <li class="Right">
                                                            <asp:Label ID="lblPriceText" runat="server" Text='<%# Eval("EffectivePrice") + "" + Eval("Currencycode") + "" %>'></asp:Label>
                                                        </li>
                                                    </ul>
                                                </div>
                                                <div class="RFPricingRowRight" id="RFPricingRowRight" runat="server">
                                                    &nbsp;</div>
                                            </div>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </div> </div>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    <asp:Label runat="server" CssClass="RFPricingRepeaterNoResults" Visible="true" ID="rpRFPricingLabel"
                                        meta:resourcekey="rpRFPricingLabel" />
                                </div>
                                <div id="RMPricingWrap" runat="server" class="RMPricingWrap" visible="false">
                                    
                                    <div id="RMPricingDiv" class="CustomPricing rounded4">
                                        <div class="savedUsageHeader">
                                            <Corbis:Label runat="server" CssClass="roundedHeaders" ID="savedUsageHeader" meta:resourcekey="savedUsageHeader" />
                                            <br />
                                            <Corbis:HyperLink runat="server" ID="learnMoreLink" onclick="CorbisUI.ExpressCheckout.OpenLearnMore(this);return false;" CssClass="learnMoreLink" meta:resourcekey="learnMoreLink" />
                                            <div class="StartOver" style="display:none">
                                                <Corbis:ImageButton ID="startOverLinkImage" runat="server" OnClick="StartOver_Click" ImageUrl="/images/resetdbdbdb.gif" />
                                                <Corbis:LinkButton ID="startOverLink" runat="server" OnClick="StartOver_Click"  meta:resourcekey="startOverLink" />
                                            </div>
                                        </div>
                                        <div class="Error RMPricingError" runat="server">
                                            <Corbis:Label runat="server" ID="customPricingValidationError" meta:resourcekey="customPricingValidtionError" />
                                        </div>
                                        <div id="errorBlock" class="WarningMode">
                                            
                                            <!-- <img alt="error" src="../Images/iconError.png" / class="errorImage" /> -->
                                            <Corbis:Image ID="iconErrorImage" ImageUrl="/Images/iconError.png" CssClass="errorImage" runat="server" meta:resourcekey="iconErrorImage" />
                                            <Corbis:Label ID="pricedByAEStartDateError" runat="server" meta:resourcekey="pricedByAEStartDateError" />
                                            
                                        </div>

                                        <div class="RMPricingInnerWrap">
                                           
                                            <div class="SelectSavedUsage widthController" id="selectSavedUsageDiv">
                                                <div id="favoriteUseContainer" class="M_10" runat="server">
                                                    
                                                    <Corbis:Label runat="server" CssClass="Bold" ID="selectFavoriteUseLabel" meta:resourcekey="selectFavoriteUseLabel" />
                                                    
                                                    <div>
                                                        <Corbis:DropDownList Display="Static" Width="240" ID="savedUsages" meta:resourcekey="useFavoriteUse" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="StartDatePriced" class="StartDate padWhiteBoxes">
                                                <Corbis:Label runat="server" ID="startDateLbl1" meta:resourcekey="startDateLbl" />
                                                <div class="clr extraSmallClear">&nbsp;</div>
                                                <div id="startDatePricedInner" class="">
                                                    <asp:TextBox runat="server" CssClass="fValidate['date'] startDateEditMode" onchange="CorbisUI.ExpressCheckout.handleLicenseDateChange(this)" ID="pricedStartDateTextBox"  />
                                                    <asp:ImageButton runat="server" CssClass="startDateEditMode ICN-calendar" ImageUrl="~/Images/calendar.gif" ID="pricedstartDatePicker" />
                                                </div>
                                                <AJAXToolkit:CalendarExtender ID="pricedCalendarExtender" runat="server" PopUpPosition="TopLeft" TargetControlID="pricedStartDateTextBox" CssClass="aeCalendar" 
                                                        OnClientDateSelectionChanged="CorbisUI.ExpressCheckout.validateStartDate" PopupButtonID="pricedstartDatePicker" BehaviorID="Calendar1" />
                                                <div class="clr extraSmallClear"></div>
                                            </div>
                                            <div class="dottedLine widthController" id="dottedLine" runat="server">
                                            </div>
                                            <div id="Or" class="Or"><asp:Label runat="server" ID="or" meta:resourcekey="or" /></div>
                                            <div class="CreateNewUsage widthController" id="createNewUsageDiv">
                                                <div class="padWhiteBoxes">
                                                    <Corbis:Label runat="server" CssClass="Bold" ID="createAnewUse" meta:resourcekey="createAnewUseLabel" />
                                                    <div class="clr">&nbsp;</div>
                                                    <Corbis:GlassButton runat="server" ButtonStyle="Outline" ID="createNewUsageButton" meta:resourcekey="createUseLbl" />
                                                </div>
                                            </div>
                                            <div class="clr extraSmallClear">&nbsp;</div>
                                        </div>
                                        <div class="RMPricedAlreadyInnerWrap">
                                            <Corbis:Label runat="server" ID="previouslyPricedLbl" meta:resourcekey="previouslyPricedLbl" />
                                            <div class="StartDate padWhiteBoxes">
                                                <Corbis:Label runat="server" ID="Label3" meta:resourcekey="startDateLbl" />
                                                <div class="clr">&nbsp;</div>
                                                <div>
                                                    <Corbis:Localize ID="pricedAlreadyCalTitle" runat="server" meta:resourcekey="pricedByAEStartDateTitle" />
                                                    
                                                    <asp:TextBox runat="server" CssClass="fValidate['date'] startDateEditMode" onchange="CorbisUI.ExpressCheckout.handleLicenseDateChange(this)" ID="alreadyPricedTextBox"  />
                                                    <asp:ImageButton runat="server" CssClass="startDateEditMode ICN-calendar" ImageUrl="~/Images/calendar.gif" ID="alreadyPricedDatePicker" />
                                                </div>
                                                <AJAXToolkit:CalendarExtender ID="pricedAlreadyCalendarExtender" runat="server" PopUpPosition="TopLeft" TargetControlID="alreadyPricedTextBox" CssClass="aeCalendar" 
                                                        OnClientDateSelectionChanged="CorbisUI.ExpressCheckout.validateStartDate" PopupButtonID="alreadyPricedDatePicker" BehaviorID="Calendar3" />
                                                <div class="clr extraSmallClear"></div>
                                            </div>
                                        </div>
                                        <div class="RMPricedByAEInnerWrap">
                                            <Corbis:Label runat="server" ID="pricedByAE" meta:resourcekey="pricedByAE" />
                                            <div class="StartDate padWhiteBoxes">
                                                <Corbis:Label runat="server" ID="startDateLbl" meta:resourcekey="startDateLbl" />
                                                <div class="clr extraSmallClear">&nbsp;</div>
                                                <div>
                                                    <Corbis:Localize ID="pricedByAEStartDateTitle" runat="server" meta:resourcekey="pricedByAEStartDateTitle" />
                                                    
                                                    <asp:TextBox runat="server" CssClass="fValidate['date'] startDateEditMode" onchange="CorbisUI.ExpressCheckout.handleLicenseDateChange(this)" ID="pricedByAEStartDateTextBox"  />
                                                    <asp:ImageButton runat="server" CssClass="startDateEditMode ICN-calendar" ImageUrl="~/Images/calendar.gif" ID="pricedByAEstartDatePicker" />
                                                </div>
                                                <AJAXToolkit:CalendarExtender ID="pricedByAECalendarExtender" runat="server" PopUpPosition="TopLeft" TargetControlID="pricedByAEStartDateTextBox" CssClass="aeCalendar"  
                                                        OnClientDateSelectionChanged="CorbisUI.ExpressCheckout.validateStartDate" PopupButtonID="pricedByAEstartDatePicker" BehaviorID="Calendar2" />
                                                <div class="clr extraSmallClear"></div>
                                            </div>
                                            <div class="clr"></div>
                                        </div>
                                        
                                    </div>
                                    
                                    
                                    <div id="licenseDetailsDiv" class="LicenseDetails rounded4">
                                        <div class="licenseDetailsHeader">
                                            <Corbis:Label runat="server" ID="licenseDetailsHeader" CssClass="roundedHeaders" meta:resourcekey="licenseDetailsHeader" />
                                            <br />
                                            <asp:Label runat="server"  ID="favoriteUseTitle" CssClass="Bold licenseDetailsHeaderLbl" meta:resourcekey="FavoriteUseTitle" />
                                            <asp:Label runat="server" ID="favoriteUseValue" CssClass="licenseDetailsHeaderLbl"/>
                                        </div>
                                        <div class="ajaxLicenseLoader" id="ajaxLicenseLoader"><div class="msg">&nbsp;</div></div>
                                        <div id="licenseDetails" runat="server" class="licenseDetailsInnerWrap">
                                            <Corbis:Label runat="server" CssClass="licenseDetailsBlank" ID="licenseDetailsLbl" meta:resourcekey="licenseDetails" />
                                        </div>
                                    </div>
                                    
                                </div>
                            </div>
                            <div class="clr smallClear">&nbsp;</div>
                        </div>
                        <div class="floatBottomLR">
                            <div class="viewRestrictionsWrap">
                                <Corbis:TextIconButton ID="restrictionsAlertIcon" runat="server" Icon="yellowAlert"
                                    OnClientClick="CorbisUI.ExpressCheckout.ViewRestrictions(this)" meta:resourcekey="lnkViewRestrictions" />
                            </div>
                            <div class="pricesVaryWrap" ID="pricesVaryWrap"  runat="server" visible="false">
                                <Corbis:Label runat="server" Visible="true" ID="pricesMayVary" meta:resourcekey="pricesMayVary" />
                            </div>
                            <div class="fileSizeDisclaimer" ID="fileSizeDiv" runat="server" visible="false">
                                <Corbis:Label runat="server" Visible="true" ID="fileSizeDisclaimer" meta:resourcekey="fileSizeDisclaimer" />
                            </div>
                        </div>
                        
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="clr extraSmallClear"></div>
    
        
    <% // DIMMERS %>
	<%// Modal for downloading images %>
        <div id="downloadProgress" style="display: none;">
	        <img border="0" alt="" src="/images/ajax-loader2.gif" />
	        <br />
	        <div class="standBy1"><Corbis:Localize ID="standByMessage1" runat="server" meta:resourcekey="standByMessage1" /></div>
	        <div class="standBy2"><Corbis:Localize ID="standByMessage2" runat="server" meta:resourcekey="standByMessage2" /></div>
	        <div class="standBy3"><Corbis:Localize ID="standByMessage3" runat="server" meta:resourcekey="standByMessage3" /></div>
	        <div class="standBy3"><Corbis:Localize ID="standByMessage4" runat="server" meta:resourcekey="standByMessage4" /></div>
        </div>	
    
        
        <div id="imageCompressionIndicator" onclick="hideMSOWindow(true)">
            <div class="mask">
            </div>
            <div id="imageCompressionContents">
                <img border="0" alt="" src="/images/ajax-loader2.gif" />
                <br />
                <h1>
                    <Corbis:Label ID="pleaseStandBy2" runat="server" meta:resourcekey="pleaseStandBy" />
                    <span>
                        <Corbis:Label ID="weAreCompression" runat="server" meta:resourcekey="weAreCompression" /></span>
                </h1>
                <div id="imageCompressionCancelDiv">
                    <Corbis:GlassButton ID="imageCompressionCancel" runat="server" meta:resourcekey="imageCompressionCancel" />
                </div>
            </div>
        </div>
    
    <% //MODALS %>
    
        <Corbis:ModalPopup ID="restrictionsPopup" ContainerID="restrictionsPopup" runat="server"
            Width="550" meta:resourcekey="restrictionsPopup">  
        <div class="ImageDetailsRestricitons ImageDetails">
            <div class="Thumbnail">
                <div class="Inner">
                    <Corbis:Image ID="rstImageThumbnail" IsAbsolute="true" runat="server" />
                </div>
            </div>     
         </div>
         <div class="ImageInfo">
            <div id="modelReleased">
                <span class="Bold"><Corbis:Label ID="rstModelReleased" runat="server" meta:resourcekey="modelReleased"></Corbis:Label></span>
                <span class="value"> {1}</span>
            </div>
            <div id="propertyReleased">
                    <span class="Bold"><Corbis:Localize ID="rstPropertyReleased" runat="server" meta:resourcekey="propertyReleased"></Corbis:Localize></span>
                    <span class="value"> {2}</span>
            </div>
            <div>
                {3}
            </div> 
         </div>
        </Corbis:ModalPopup>
        <Corbis:ModalPopup ID="imageNotAvailableForCheckout" ContainerID="imageNotAvailableForCheckout" runat="server"
            Width="400" meta:resourcekey="imageNotAvailableForCheckout">  
            <Corbis:GlassButton ID="quitExpressCheckout" runat="server" cssClass="btnGraydbdbdb" CausesValidation="false" meta:resourcekey="quitExpressCheckout" OnClientClick="CorbisUI.ExpressCheckout.DoCloseExpressCheckoutModal();return false;" />        
            <Corbis:GlassButton ID="contactCorbis" runat="server" CausesValidation="false" meta:resourcekey="contactCorbis" />
        </Corbis:ModalPopup>
        <Corbis:ModalPopup ID="learnMorePopup" ContainerID="learnMorePopup" runat="server" Width="350" meta:resourcekey="learnMorePopup">
        
      	</Corbis:ModalPopup>

        <%--Pricing alert popup--%>
        <div id="customPriceExpired" style="width: 350px; height: 196px; display: none;">
            <div id="titleWrapper" class="titleWrapper">
                <span id="expiredTitle" class="expiredTitle">
                    <Corbis:Localize ID="title" runat="server" meta:resourcekey="expiredTitle" /></span>
                <div class="expiredCloseButton" id="expiredCloseButton">
                    <!-- <img alt="Close Modal Popup" onclick="return HideModal('customPriceExpired');return false;"
                        class="Close" src="/Images/iconClose.gif" /> -->
                    <Corbis:Image ID="pricingAlertPopupImage" ImageUrl="/Images/iconClose.gif" runat="server" onclick="HideModal('customPriceExpired');return false;" class="Close" meta:resourcekey="pricingAlertPopupImage"/>
                </div>
            </div>
            <div id="dataContainer" class="dataContainer">
                <div id="expiredStrongBody" class="expiredStrongBody">
                    <p>
                        <Corbis:Label ID="strongStatement" runat="server" meta:resourcekey="strongStatement" /></p>
                </div>
                <br />
                <div id="expiredMainBody">
                    <p>
                        <Corbis:Label ID="mainStatement" runat="server" meta:resourcekey="mainStatement" /></p>
                </div>
                <Corbis:GlassButton ID="expiredGlass" CssClass="expiredGlass" meta:resourcekey="expiredGlass"
                    runat="server" CausesValidation="false" OnClientClick="HideModal('customPriceExpired');return false;" />
            </div>
        </div>
        <%--End Pricing alert popup--%>
        
        <%--Corporate account under credit review popup--%>
        <Corbis:ModalPopup ID="CoporateAccountCreditReview" ContainerID="CoporateAccountCreditReview" runat="server"
            Width="330" meta:resourcekey="creditReview">  
            <Corbis:GlassButton ID="corporateReviewCloseButton" runat="server" cssClass="btnGraydbdbdb" CausesValidation="false" meta:resourcekey="continueClose" OnClientClick="CorbisUI.ExpressCheckout.DoCloseExpressCheckoutModal();return false;" />        
            <Corbis:GlassButton ID="corporateReviewContactButton" runat="server" CausesValidation="false" meta:resourcekey="contactCorbis"  OnClientClick="javascript:CorbisUI.ContactCorbis.ShowContactCorbisModal(this);return false;"  />
        </Corbis:ModalPopup>
        
        <%--No default payment method popup--%>
        <Corbis:ModalPopup ID="noDefaultPayment" ContainerID="noDefaultPayment" runat="server"
            Width="330" meta:resourcekey="noDefaultPayment">  
            <Corbis:GlassButton ID="noPaymentQuit" runat="server" cssClass="btnGraydbdbdb" CausesValidation="false" meta:resourcekey="quitExpressCheckout" OnClientClick="CorbisUI.ExpressCheckout.DoCloseExpressCheckoutModal();return false;" />        
            <Corbis:GlassButton ID="noPaymentContact" runat="server" CausesValidation="false" meta:resourcekey="contactCorbis"/>
        </Corbis:ModalPopup>
        
        <%--No default payment method popup--%>
        <Corbis:ModalPopup ID="correctCreditCardInfo" ContainerID="correctCreditCardInfo" runat="server"
            Width="330" meta:resourcekey="correctCreditCardInfo">         
            <Corbis:GlassButton ID="cardInfoClose" runat="server" CausesValidation="false" meta:resourcekey="closeButton"  OnClientClick="HideModal('correctCreditCardInfo');return false;"  />
        </Corbis:ModalPopup>
        
        <%--Bad promo code method popup--%>
        <Corbis:ModalPopup ID="badPromoCode" ContainerID="badPromoCode" runat="server"
            Width="330" meta:resourcekey="badPromoCode">         
            <Corbis:GlassButton ID="promoCodeClose" runat="server" CausesValidation="false" meta:resourcekey="closeButton"  OnClientClick="HideModal('badPromoCode');CorbisUI.ExpressCheckout.vars.PromoCodeBox.focus();return false;"  />
        </Corbis:ModalPopup>
         
         <%--Session timeout popup--%> 
        <Corbis:ModalPopup ID="sessionTimeout" ContainerID="sessionTimeout" runat="server"  Title="<%$ Resources:Resource, sessionTimeoutTitle %>"   CloseScript="handleSessionTimeout();return false;">
            <Corbis:Localize ID="sessionTimeoutMessage" runat="server" Text="<%$ Resources:Resource, sessionTimeoutMessage %>" />
		    <Corbis:GlassButton CssClass="closeSuccess"   ID="closeSessionTimeout" runat="server" OnClientClick="handleSessionTimeout();return false;" ButtonStyle="Orange" ButtonBackground="dbdbdb"  meta:resourcekey="closeButton" />
	    </Corbis:ModalPopup>
	
	<Corbis:LinkButton ID="sessionTimeoutRedirect" runat="server" OnClick="SessionTimeOut_Click" CssClass="displaynone" ></Corbis:LinkButton>
		<Corbis:ContactCorbis runat="server" />
    <% //END MODALS %>
    
    <input type="hidden" name="hidPaymentMethod" id="hidPaymentMethod" runat="server" />
    <input type="hidden" name="corporateID" id="corporateID" runat="server" />
    <input type="hidden" name="creditCardUID" id="creditCardUID" runat="server" />
    <input type="hidden" name="submitPurchaseClicked" id="submitPurchaseClicked" runat="server" />
    <input type="hidden" name="hidTriggerChangeEvent" id="hidTriggerChangeEvent" runat="server" />    
    <input type="hidden" name="hidRedirectUrl" id="hidRedirectUrl" runat="server" class="hidRedirectUrl" />
    <script language="javascript" type="text/javascript">
        function SecureModalPopupExit() {

            var iFrames = $$('iframe');
            var iDo = iFrames[0];
            iDo.src = CorbisUI.ExpressCheckout.vars.ParentProtocol + "/Common/IFrameTunnel.aspx?windowid=expressCheckout&action=execute&actionArg=CorbisUI.ExpressCheckout.CloseExpressCheckoutModal(this)";
        }
        function OpenCreateNewUsage(corbisId) {
            var iFrames = $$('iframe');
            var iDo = iFrames[0];
            iDo.src = CorbisUI.ExpressCheckout.vars.ParentProtocol + "/Common/IFrameTunnel.aspx?windowid=expressCheckout&action=execute&actionArg=CorbisUI.ExpressCheckout.openCreateNewUsage('" + corbisId +"')";
        }
        function setCreditCard(ccUid) {
            if (ccUid != null) {
                CorbisUI.ExpressCheckout.RefreshPaymentMethods(ccUid);
            }
        }
        function resizeExpressCheckout() {
                var iFrames = $$('iframe');
                var iDo = iFrames[0];
                iDo.src = CorbisUI.ExpressCheckout.vars.ParentProtocol + "/Common/IFrameTunnel.aspx?windowid=expressCheckout&action=execute&actionArg=ResizeIModal('expressCheckout', " + GetDocumentHeight() + ")&noclose=true";
        }
        window.addEvent('load', resizeExpressCheckout);
    </script>
    
    <iframe id="iFrameHttp" runat="server" style="display:none;" src="/Common/IFrameTunnel.aspx"></iframe>
    <script type="text/javascript">
    
        //Globals to be used by PricingList class in ExpressCheckout.js
        CorbisUI.ExpressCheckout.vars.DateFormat='<%=Language.CurrentCulture.DateTimeFormat.ShortDatePattern%>';
        CorbisUI.ExpressCheckout.vars.DateSeparator = '<%=Language.CurrentCulture.DateTimeFormat.DateSeparator %>';
        CorbisUI.ExpressCheckout.vars.ProjectField = $('<%= projectField.ClientID %>');
        CorbisUI.ExpressCheckout.vars.JobField = $('<%= jobField.ClientID %>');
        CorbisUI.ExpressCheckout.vars.PurchaseOrderField = $('<%= poField.ClientID %>');
        CorbisUI.ExpressCheckout.vars.LicenseeField = $('<%= licenseeField.ClientID %>');
        CorbisUI.ExpressCheckout.vars.RequiredText = '<%= RequiredText %>';
        CorbisUI.ExpressCheckout.vars.PurchaseButton = $('<%= purchaseButton.ClientID %>');
        CorbisUI.ExpressCheckout.vars.SubmitPurchaseClicked = $('<%= submitPurchaseClicked.ClientID %>');
        CorbisUI.ExpressCheckout.vars.PriceLabel = $('<%= piPrice.ClientID %>');
        CorbisUI.ExpressCheckout.vars.PriceCode = $('<%= hidPriceCode.ClientID %>');
        CorbisUI.ExpressCheckout.vars.PromoCodeBox = $('<%= promotionCode.ClientID %>');
        CorbisUI.ExpressCheckout.vars.PromoCodeButton = $('<%= promotionCodeButton.ClientID %>');
        CorbisUI.ExpressCheckout.vars.HidAttributeUID = $('<%= hidAttributeUID.ClientID %>');
        CorbisUI.ExpressCheckout.vars.HidAttributeValueUID = $('<%= hidAttributeValueUID.ClientID %>');
        CorbisUI.ExpressCheckout.vars.HidProductUid = $('<%= hidProductUid.ClientID %>');
        CorbisUI.ExpressCheckout.vars.hidSavedUsageUid = $('<%= hidSavedUsageUid.ClientID %>');
        CorbisUI.ExpressCheckout.vars.PaymentMethodBox = $('paymentMethodSelect');
        CorbisUI.ExpressCheckout.vars.ThePaymentMethodGuid = $('paymentMethodSelect').value;
        CorbisUI.ExpressCheckout.vars.ThePaymentType = $('paymentMethodSelect').options[$('paymentMethodSelect').selectedIndex].getAttribute("type");
        CorbisUI.ExpressCheckout.vars.MyPricingList = new PricingList();
        CorbisUI.ExpressCheckout.vars.SavedUsageDropdown = $('<%= savedUsages.ClientID %>');
        CorbisUI.ExpressCheckout.vars.FavoriteUseLabel = $('<%= favoriteUseValue.ClientID %>');
        CorbisUI.ExpressCheckout.vars.LicenseDetails = $('<%= licenseDetails.ClientID %>');
        CorbisUI.ExpressCheckout.vars.CorbisID = '<%= CorbisId %>';
        CorbisUI.ExpressCheckout.vars.ContractType = '<%= ContractType.ToString() %>';
        CorbisUI.ExpressCheckout.vars.PricedStartDateTextBox = $('<%= pricedStartDateTextBox.ClientID %>');
        CorbisUI.ExpressCheckout.vars.PricedByAE = '<%= PricedByAE.ToString() %>';
        CorbisUI.ExpressCheckout.vars.PricedByAEStartDateTextBox = $('<%= pricedByAEStartDateTextBox.ClientID %>');
        CorbisUI.ExpressCheckout.vars.AlreadyPricedTextBox = $('<%= alreadyPricedTextBox.ClientID %>');
        CorbisUI.ExpressCheckout.vars.AlreadyPriced = 'False';
        CorbisUI.ExpressCheckout.vars.licenseStartDate = null;  //Dont set it to new date 
        CorbisUI.ExpressCheckout.vars.ImageThumbnail = $('<%= rstImageThumbnail.ClientID %>');
        CorbisUI.ExpressCheckout.vars.DefaultDate = '<%=DateTime.Now.ToString("d") %>';
        CorbisUI.ExpressCheckout.vars.invalidPayment = <% if(!this.IsCreditEnable && !this.HasCorporateAccount){ %>true<% }else{ %>false<% } %>;
        CorbisUI.ExpressCheckout.vars.CardExpired = false;// CorbisUI.ExpressCheckout.vars.PaymentMethodBox.options[CorbisUI.ExpressCheckout.vars.PaymentMethodBox.selectedIndex].get('cardExpired');
        CorbisUI.ExpressCheckout.vars.ProjectNameText = '<%=ProjectFieldText%>';
        CorbisUI.ExpressCheckout.vars.LicenseType = '<%= LicenseModel %>';
        CorbisUI.ExpressCheckout.vars.LoadPricingText = '<%= LoadingPriceText %>';
        CorbisUI.ExpressCheckout.vars.LoadLicensingText = '<%= LoadingLicenseText %>';
        CorbisUI.ExpressCheckout.vars.CreditCardCVC = $('<%= creditCardValidationCode.ClientID %>');
        CorbisUI.ExpressCheckout.vars.LightboxId = '<%= LightboxId %>';
        CorbisUI.ExpressCheckout.vars.hidPaymentMethod = $('<%= hidPaymentMethod.ClientID %>');
        CorbisUI.ExpressCheckout.vars.corporateID = $('<%= corporateID.ClientID %>');
        CorbisUI.ExpressCheckout.vars.creditCardUID = $('<%= creditCardUID.ClientID %>');
        CorbisUI.ExpressCheckout.vars.AjaxLoader = $('ajaxLicenseLoader');
        CorbisUI.ExpressCheckout.vars.TotalPrice = $('<%=piTotal.ClientID %>');
        CorbisUI.ExpressCheckout.vars.PriceCodeDiv = $('<%=piPriceCode.ClientID %>');
        CorbisUI.ExpressCheckout.vars.TotalCodeDiv = $('<%=piTotalCode.ClientID %>');
        CorbisUI.ExpressCheckout.vars.customPriceExpired = '<%=hidCustomPricingExpired.Value %>';
        CorbisUI.ExpressCheckout.vars.customPriceExpiredDelay = '<%= CustomPriceExpiredDelay %>';
        CorbisUI.ExpressCheckout.vars.TriggerChangeEvent = '<%=hidTriggerChangeEvent.Value %>';
        CorbisUI.ExpressCheckout.vars.RedirectUrl = '<%= hidRedirectUrl.Value %>';
        CorbisUI.ExpressCheckout.vars.ExpressCheckoutSubmitUrl="ExpressCheckout.aspx?CorbisId={0}&hidAttributeValueUID={1}&currencyCode={2}&thePaymentMethod={3}&thePaymentType={4}&contractType={5}&promoCode={6}&LightboxId={7}&StartDate={8}";
        CorbisUI.ExpressCheckout.vars.licenseDetailsErrorMessage = '<%= LicenseDetailsErrorMessage %>';
        CorbisUI.ExpressCheckout.vars.pricingCalculationErrorMessage = '<%= PricingCalculationErrorMessage %>';
        CorbisUI.ExpressCheckout.vars.DataChanged = false;
        //Setup some page events
        window.addEvent('load', function() { 
            CorbisUI.ExpressCheckout.vars.ProjectField.value = CorbisUI.ExpressCheckout.vars.ProjectNameText;
            if (CorbisUI.ExpressCheckout.vars.LicenseType == 'RF') {
                if (CorbisUI.ExpressCheckout.vars.PricedByAE == 'True' &&
                    CorbisUI.ExpressCheckout.vars.customPriceExpired != 'True' &&
                    CorbisUI.ExpressCheckout.vars.HidAttributeValueUID.value == 'valid')
                {
                    //CorbisUI.ExpressCheckout.RFExpressCheckoutPricingAjax();
                }
                else 
                {
                    CorbisUI.ExpressCheckout.setupRFRepeater();
                }
            }
            if (CorbisUI.ExpressCheckout.vars.LicenseType == 'RM') {
                CorbisUI.ExpressCheckout.RMRegisterSelectEvent();
            }
            CorbisUI.ExpressCheckout.registerTooltips(false);
            CorbisUI.ExpressCheckout.NoPaymentMethod(); 
            CorbisUI.ExpressCheckout.SetupPaymentMethod(); 
            CorbisUI.ExpressCheckout.PromoCodeListener(); 
            if(CorbisUI.ExpressCheckout.vars.customPriceExpired == 'True'){
                CorbisUI.ExpressCheckout.OpenCustomPriceExpired(CorbisUI.ExpressCheckout.vars.customPriceExpiredDelay);
            }
            if(CorbisUI.ExpressCheckout.vars.TriggerChangeEvent == 'True') {
                CorbisUI.ExpressCheckout.ProcessSavedUsageChangeEvent();
            }
            CorbisUI.ExpressCheckout.setupPanels(); 
            CorbisUI.ExpressCheckout.validateStep1();
            if ( CorbisUI.ExpressCheckout.vars.LicenseType == 'RF' &&
                 CorbisUI.ExpressCheckout.vars.PricedByAE == 'True' &&
                 CorbisUI.ExpressCheckout.vars.customPriceExpired != 'True')
                
            {
                CorbisUI.ExpressCheckout.unlockStep2();
            }

         });
         window.setTimeout("CorbisUI.ExpressCheckout.ProcessPaymentMethodSelectionChange(null,true)",1000);
         
    function handleSessionTimeout() {
        HideModal('sessionTimeout');
        // __doPostBack($('<%=sessionTimeoutRedirect.ClientID %>').id.replace(/_/g, '$'), ''); 
        Corbis.Web.UI.Checkout.CheckoutService.SignOut();
        setTimeout("CloseExpressCheckoutIModalAfterTimeout();",1000);
    }
    
    function CloseExpressCheckoutIModalAfterTimeout() {
            
           var iFrames = $$('iframe');
           var iDo = iFrames[0];
           iDo.src = ParentProtocol + "/Common/IFrameTunnel.aspx?windowid=expressCheckout&action=execute&actionArg=''";
           
        }
    
    </script>
</asp:Content>
