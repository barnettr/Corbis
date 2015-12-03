<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StepSubmit.ascx.cs"
    Inherits="Corbis.Web.UI.Checkout.StepSubmit" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="Corbis" TagName="OrderItems" Src="~/Checkout/DownloadItems.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="ProjectBlock" Src="~/Checkout/ProjectBlock.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="DeliveryBlock" Src="~/Checkout/DeliveryBlock.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="PaymentBlock" Src="~/Checkout/PaymentBlock.ascx" %>
<asp:UpdatePanel ID="stepSubmitUpdatePanel" runat="server" ChildrenAsTriggers="false"
    UpdateMode="Conditional">
    <ContentTemplate>
        <div class="orderContent">
            <div class="notification">
                <h3>
                    <Corbis:Localize ID="total" runat="server" meta:resourcekey="total"></Corbis:Localize>
                    <Corbis:Label ID="totalNumber" runat="server" />
                    </Corbis:Localize>
                </h3>
                <p class="bold">
                    <Corbis:Localize ID="verify" runat="server" meta:resourcekey="verify"></Corbis:Localize></p>
            </div>
            <div class="orderItems">
                <Corbis:ProjectBlock ID="projectBlock" runat="server" EnableEdit="true" />
                <hr />
                <Corbis:DeliveryBlock ID="deliveryBlock" runat="server" EnableEdit="true" />
                <hr />
                <Corbis:PaymentBlock ID="paymentBlock" runat="server" EnableEdit="true" />
                <hr />
                <div class="checkThis">
                    <h3>
                        <Corbis:Localize ID="restrictions" runat="server" meta:resourcekey="restrictions"></Corbis:Localize></h3>
                    <p class="bold">
                        <Corbis:Localize ID="mustCheck1" runat="server" meta:resourcekey="mustCheck"></Corbis:Localize>
                    </p>
                    <p class="imageCheckbox">
                        <Corbis:ImageCheckbox runat="server" ID="restrictionsCheckbox" Checked="false" EnableViewState="true"
                            OnClientChanged="submitBtnHandler();" />
                        <p class="checkThisText">
                            
                            <Corbis:Localize ID="restrictionsIveReadText" runat="server" meta:resourcekey="RestrictionsIveRead"></Corbis:Localize>
                        </p>
                    </p>
                </div>
                <hr />
                <div class="checkThis">
                    <h3>
                        <Corbis:Localize ID="termsConditions" runat="server" meta:resourcekey="termsConditions"></Corbis:Localize></h3>
                    <p class="bold">
                        <Corbis:Localize ID="mustCheck2" runat="server" meta:resourcekey="mustCheck"></Corbis:Localize></p>
                    <p class="imageCheckbox">
                        <Corbis:ImageCheckbox runat="server" ID="termsCheckbox" Checked="false" EnableViewState="true"
                            OnClientChanged="submitBtnHandler();" />
                        <p class="checkThisText">
                            <Corbis:Localize ID="termsIveReadText" runat="server" meta:resourcekey="TermsIveRead"></Corbis:Localize>
   
                        </p>
                    </p>
                </div>
                <div class="notification">
                    <h3>
                        <Corbis:Localize ID="note" runat="server" meta:resourcekey="note"></Corbis:Localize></h3>
                    <div>
                        <Corbis:Localize ID="thisTransaction" runat="server" meta:resourcekey="thisTransaction"></Corbis:Localize></div>
                    <div>
                        <Corbis:Label runat="server" ID="notificationEmails" />.
                    </div>
                </div>
                <div id="footerSection">
                       <!-- "Want to quit checkout?" modal -->
                        <Corbis:ModalPopup ContainerID="quitCheckout" runat="server" meta:resourcekey="areYouSure">
                            <Corbis:GlassButton runat="server" ButtonStyle="Gray" CausesValidation="false" ID="continueCheckout" meta:resourcekey="continue" OnClientClick="HideModal('quitCheckout');return false;" />  
                            <Corbis:GlassButton runat="server" CausesValidation="false" ID="quitCheckout" meta:resourcekey="quit" OnClientClick="GoToCartByFlow(true);return false;" />  
                        </Corbis:ModalPopup>

                        <a href="javascript:OpenModal('quitCheckout'); ResizeModal('quitCheckout');"><Corbis:Localize ID="quit" runat="server" meta:resourcekey="quit" /></a>

                    <ul class="buttonPositioner">
                        <li class="previous">
                            <Corbis:GlassButton ID="btnSumbmitPrevious" OnClientClick="checkoutTabs.show(2);UpdateBasedOnCurrentTabIndex({tabIndex:2});return false;"
                                runat="server" CssClass="backStepButton" ButtonStyle="Gray" 
                                meta:resourcekey="previousStep"></Corbis:GlassButton>
                        </li>
                        <li class="next">
                            <Corbis:GlassButton ID="btnSubmitNext" OnClick="submit_Click" OnClientClick="CorbisUI.Checkout.OpenProgressModal();return true;"
                                runat="server" CssClass="afterPreviousButton nextStepButton" ButtonStyle="Orange"
                                Enabled="false" meta:resourcekey="nextStep"></Corbis:GlassButton>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnSubmitNext" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>
<Corbis:ModalPopup ID="ViewOrderRestrictions" ContainerID="ViewOrderRestrictions" runat="server" Width="550"
    meta:resourcekey="restrictionsTitle">
    <div id="restrictionsContainer">
   
    <asp:Repeater runat="server" ID="multipleRestractions" OnItemDataBound="multipleRestractions_ItemDataBound" >
        <ItemTemplate>
            <div class="restrictionSection">
                <Corbis:CenteredImageContainer IsAbsolute="true" runat="server" ImageID="image" ID="thumbWrap1"  ImgCssClass="thumbWrap1" Size='90'  />
                    <div class="modelPropertyPair"> 
                        <span class="bold">
                            <Corbis:Localize ID="modelReleased" runat="server" meta:resourcekey="modelReleased"></Corbis:Localize>
                        </span>
                        <asp:label runat="server" ID="modelRelease" />
                    </div>
                    <div class="modelPropertyPair">
                        <span class="bold">
                            <Corbis:Localize ID="propertyReleased" runat="server" meta:resourcekey="propertyReleased"></Corbis:Localize>
                        </span>
                        <asp:label runat="server"  ID="propertyRelease" />
                    </div>
                <asp:PlaceHolder runat="server" ID="allRestrictions" ></asp:PlaceHolder>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    </div>
</Corbis:ModalPopup>

<script language="javascript" type="text/javascript">
    function submitBtnHandler() {
        if (getCheckedState('<%=termsCheckbox.ClientID %>') && getCheckedState('<%=restrictionsCheckbox.ClientID %>')) {
            setGlassButtonDisabled('<%=btnSubmitNext.ClientID %>', false);
        }
        else {
            setGlassButtonDisabled('<%=btnSubmitNext.ClientID %>', true);
        }
    }
</script>

<div id="downloadProgress" style="display: none;">
	<img border="0" alt="" src="/images/ajax-loader2.gif" />
	<br />
	<div class="standBy1"><Corbis:Localize ID="standByMessage1" runat="server" meta:resourcekey="standByMessage1" /></div>
	<div class="standBy2"><Corbis:Localize ID="standByMessage2" runat="server" meta:resourcekey="standByMessage2" /></div>
	<div class="standBy3"><Corbis:Localize ID="standByMessage3" runat="server" meta:resourcekey="standByMessage3" /></div>
	<div class="standBy3"><Corbis:Localize ID="standByMessage4" runat="server" meta:resourcekey="standByMessage4" /></div>
</div>	
