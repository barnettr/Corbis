<%@ Control Language="C#" AutoEventWireup="true" Codebehind="InvalidPaymentMethod.ascx.cs"
    Inherits="Corbis.Web.UI.Checkout.InvalidPaymentMethod" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<div class="paneLayout" id="invalidPaymentPaneLayout">
    <div class="positionMe">
        <div class="Error">
            <div>
                <h4><Corbis:Localize ID="paymentAlertTitle" runat="server" meta:resourcekey="invalidPaymentAlertTitle" /></h4>
                <p><Corbis:Localize ID="paymentAlertMessage" runat="server" meta:resourcekey="invalidPaymentAlertMessage" /></p>
                
            </div>
        </div>
        <div class="alertButtons">
            <Corbis:WorkflowBlock WorkflowType="CART" runat="server">
                <Corbis:GlassButton ID="btnApplyForCredit" OnClick="btnApplyCredit_Click"
                runat="server" CssClass="" ButtonStyle="Orange"
                meta:resourcekey="applyForCreditBtn" />
            </Corbis:WorkflowBlock>
            <Corbis:GlassButton ID="btnContactCorbis" runat="server" CssClass="" ButtonStyle="Orange" meta:resourcekey="contactCorbisBtn" />
        </div>
    </div>
</div>
<div class="buttonBar ">
    <a href="javascript:void(0)"  onclick="CorbisUI.Checkout.modals.quitCheckout(); return false;">
        <Corbis:Localize ID="quit" runat="server" meta:resourcekey="quit" />
        
    </a>

</div>
