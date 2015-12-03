<%@ Page Language="C#" AutoEventWireup="true" EnableTheming="false" CodeBehind="ExpressCheckout_PricingBox.aspx.cs" Inherits="Corbis.Web.UI.Checkout.ExpressCheckout_PricingBox" %>

<%@ Register Assembly="Corbis.Web.UI.Controls" Namespace="Corbis.Web.UI.Controls" TagPrefix="Corbis" %>

    <head runat="server" visible="false"></head>
    <Corbis:Label runat="server" ID="lblPrice" CssClass="pricingLabels" meta:resourcekey="lblPrice" />
    <div class="PriceIndicator pricingValues">
        <span class="Price" id="piPrice" runat="server"><asp:Label runat="server" ID="lblThePrice" /></span>
        <span class="PriceCurrency" id="piPriceCode" runat="server"><asp:Label runat="server" ID="lblCurrencyCode" meta:resourcekey="lblCurrencyCode" /></span>
        
    </div>
    <div class="clr smallClear">&nbsp;</div>
    
    <Corbis:Label runat="server" ID="promotionLabel" CssClass="pricingLabels" meta:resourcekey="lblPromotion" />
    <div class="pricingValues">
        <span class="Price" id="piPromotion" runat="server"></span>
    </div>
    
    <div class="clr smallClear">
        &nbsp;</div>
    <div id="ksaHolder" runat="server" visible="false">
        <Corbis:Label runat="server" ID="ksaLabel"  CssClass="pricingLabels"  meta:resourcekey="lblKsa" />
        <div class="pricingValues">
            <span class="Price" id="piKsa" runat="server"></span> <span class="PriceCurrency"
                id="piKsaCode" runat="server"></span>
        </div>
        <div class="clr smallClear">&nbsp;</div>
    </div>
    <Corbis:Label runat="server" ID="taxLabel" meta:resourcekey="lbltax" CssClass="pricingLabels"   />
    <div class="pricingValues">
        <span class="Price" id="piTax" runat="server"></span>
    </div>
    <div class="clr smallClear">&nbsp;</div>
    
    <div class="totalsWrap">
        <Corbis:Label runat="server" CssClass="pricingLabels totalLabel" ID="totalLabel" meta:resourcekey="lblTotal" />
        <div class="pricingValues">
            <span class="Price" id="piTotal" runat="server"></span>
        </div>
    </div>
    <div class="clr smallClear"><br /></div>

