<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Steps.ascx.cs" Inherits="Corbis.Web.UI.Checkout.Steps" %>
<%@ Register TagPrefix="Corbis" TagName="StepProject" Src="~/Checkout/StepProject.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="StepDelivery" Src="~/Checkout/StepDelivery.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="StepPayment" Src="~/Checkout/StepPayment.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="StepSubmit" Src="~/Checkout/StepSubmit.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="InvalidPaymentMethod" Src="~/Checkout/InvalidPaymentMethod.ascx" %>
<%@ Register Assembly="Corbis.Web.UI.Controls" Namespace="Corbis.Web.UI.Controls" TagPrefix="Corbis" %>
<div id="topNavDiv">
    <a id="projectTab" class="tab disabled bold"><Corbis:Localize ID="tabProject" runat="server" meta:resourcekey="tabProject" /></a>
    <a id="deliveryTab" class="tab disabled bold"><Corbis:Localize ID="tabDelivery" runat="server" meta:resourcekey="tabDelivery" /></a>
    <a id="paymentTab" class="tab disabled bold"><Corbis:Localize ID="tabPayment" runat="server" meta:resourcekey="tabPayment" /></a>
    <a id="reviewTab" class="tab disabled bold"><Corbis:Localize ID="tabReviewSubmit" runat="server" meta:resourcekey="tabReviewSubmit" /></a>
</div>

<asp:UpdatePanel ID="TabContainersUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="tabContainers" style="background-color: #363636; color: #363636;">
            <div id="projectPane" class="container rounded4" internal="true" isbottomonly="true" style="display: none">
                <Corbis:StepProject ID="stepProject" runat="server"></Corbis:StepProject>
            </div>
            <div id="deliveryPane" class="container rounded4" internal="true" isbottomonly="true"
                style="display: none">
                <Corbis:StepDelivery ID="stepDelivery" runat="server"></Corbis:StepDelivery>
            </div>
            <div id="paymentPane" class="container rounded4" internal="true" isbottomonly="true"
                style="display: none">
                <Corbis:StepPayment ID="stepPayment" runat="server"></Corbis:StepPayment>
            </div>
            <div id="reviewPane" class="container rounded4" internal="true" isbottomonly="true" style="display: none">
                <Corbis:StepSubmit ID="stepSubmit" runat="server"></Corbis:StepSubmit>
            </div>
            <div id="invalidPaymentPane" class="container rounded4" internal="true" isbottomonly="true" style="display: none">
                <Corbis:InvalidPaymentMethod ID="invalidPaymentMethod" runat="server"></Corbis:InvalidPaymentMethod>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

    <Corbis:ModalPopup ID="quitCheckoutModal" ContainerID="quitCheckoutModal"
         runat="server" meta:resourcekey="quitCheckoutModal" Width="340">
        <Corbis:Localize ID="quitCheckoutModalLabel" runat="server" meta:resourcekey="quitCheckoutModalLabel" />
            <div class="buttonsContainer">
                <Corbis:GlassButton ID="btnQuitCheckout" OnClientClick="MochaUI.CloseModal('quitCheckoutModal');return false;"
                runat="server" CssClass="GlassButtonLeft" ButtonStyle="Gray"
                meta:resourcekey="continueCheckoutBtn"></Corbis:GlassButton>

                <Corbis:GlassButton ID="btnContinueCheckout" OnClientClick="GoToCartByFlow(true);return false;"
                runat="server" CssClass="GlassButtonRight" ButtonStyle="Orange"
                meta:resourcekey="quitCheckoutBtn"></Corbis:GlassButton>
            </div>
    </Corbis:ModalPopup>
    <%--
****************************************
* USE SAVED CREDIT CARD MODAL TEMPLATE 
****************************************
--%>



<%--
****************************************
* ADD NEW CREDIT CARD MODAL TEMPLATE 
****************************************
--%>


    <!-- end of #checkoutStage -->


