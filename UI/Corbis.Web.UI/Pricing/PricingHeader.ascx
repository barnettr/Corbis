<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PricingHeader.ascx.cs" Inherits="Corbis.Web.UI.Pricing.PricingHeader" %>
<%@ Register Assembly="Corbis.Web.UI.Controls" Namespace="Corbis.Web.UI.Controls" TagPrefix="Corbis" %>
<%@ Register TagPrefix="Corbis" TagName="AddToLightbox" Src="~/UserControls/LightboxControls/AddToLightbox.ascx" %>
    <input type="hidden" name="hidUpdatingCart" id="hidUpdatingCart" runat="server" value="False" enableviewstate="true" />
    <input type="hidden" name="hidUpdatingLightbox" id="hidUpdatingLightbox" runat="server" value="False" enableviewstate="true" />
    <div class="PriceImageHeader">
        <div class="ImageDetails">                        
            <div class="Thumbnail">
                <div class="Inner">
                    <Corbis:Image ID="imageThumbnail" IsAbsolute="true" runat="server" />
                </div>
            </div>

            <div class="ImageInfo"><br />
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
        <div class="PricingInfo"><br />
            <div class="PricingBox"><Corbis:GlassButton Enabled="false" OnClientClick="if(!CorbisUI.Pricing.IsAuthenticated(this.id)){return false;}" ID="cartButton" runat="server" CausesValidation="false" meta:resourcekey="addToCart"/>
                    <asp:Label runat="server" ID="lblPrice" CssClass="PriceLabel" meta:resourcekey="lblPrice" />
                    <div class="PriceIndicator">
                        <span class="Price" id="piPrice" runat="server">0.00</span>
                        <span class="PriceCurrency" id="piPriceCode" runat="server" ><asp:Label runat="server" ID="lblCurrencyCode" meta:resourcekey="lblCurrencyCode" /></span>
                    </div>
            </div>
            
            <div class="LightboxButton">
                <Corbis:GlassButton ID="lightboxButton" Enabled="false" runat="server" CausesValidation="false" meta:resourcekey="addToLightbox" />
                <Corbis:AddToLightbox ID="addToLightboxPopup" runat="server" UseDefalutHandler="false" PopulateDropdown="true"/>
           </div>
           
        </div>
        <div class="Clr"></div>
        <div id="imageContainer" class="imageContainer" runat="server" visible="false"><img alt="" class="iconError" src="/Images/iconError.png" /></div>
        <div id="licenseAlertDiv" runat="server" visible="false" class="licenseAlertDiv">
            <div id="headerText"><asp:Label runat="server" ID="alertHeading" meta:resourcekey="lblInvalidLicenseHeading" /></div>
            <div id="alertText"><asp:Label runat="server" ID="alertMessage" meta:resourcekey="lblInvalidLicenseMessage" /></div>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
    var isSavedUsage = false;

    function setIsSavedUsage() {
        isSavedUsage = true;
    }
    function ParentPageRedirect()
    {
        try
        {
            self.parent.frames.location.reload();
            parent.CloseModal('pricing');
        //window.close();
        }
        catch (Exception) {}
    }

    function disableCartAndLightBoxButtons() {
        $('<%=cartButton.ClientID%>').addClass('DisabledGlassButton');
        $('<%=lightboxButton.ClientID%>').addClass('DisabledGlassButton');
        $('<%=cartButton.ClientID%>' + '_GlassButton').setProperty('disabled', 'disabled');
        $('<%=lightboxButton.ClientID%>' + '_GlassButton').setProperty('disabled', 'disabled');
      
    }

    function enableCartAndLightBoxButtons() {
        if (!isSavedUsage) {
            if ($('<%=hidUpdatingCart.ClientID %>').value != 'True') {
                $('<%=cartButton.ClientID%>').removeClass('DisabledGlassButton');
                $('<%=cartButton.ClientID%>' + '_GlassButton').removeProperty('disabled');
            }


            if ($('<%=hidUpdatingLightbox.ClientID %>').value != 'True') {
                $('<%=lightboxButton.ClientID%>').removeClass('DisabledGlassButton');

                $('<%=lightboxButton.ClientID%>' + '_GlassButton').removeProperty('disabled');
            }
        } else {
            $('<%=cartButton.ClientID%>').removeClass('DisabledGlassButton');
            $('<%=lightboxButton.ClientID%>').removeClass('DisabledGlassButton');
            $('<%=cartButton.ClientID%>' + '_GlassButton').removeProperty('disabled');
            $('<%=lightboxButton.ClientID%>' + '_GlassButton').removeProperty('disabled');
        }
        
       
    }

   
   </script>