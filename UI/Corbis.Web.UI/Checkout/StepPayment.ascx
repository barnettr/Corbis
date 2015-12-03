<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StepPayment.ascx.cs"
    Inherits="Corbis.Web.UI.Checkout.StepPayment" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="Corbis" TagName="RoundCorners" Src="~/Accounts/RoundCorners.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="CreditCard" Src="~/Checkout/CreditCardBlock.ascx" %>
<%--
*******************************
* JAVASCRIPT GLOBAL VARS 
*******************************
--%>

<script type="text/javascript">

        // check if MainCheckout object has a Tabs definition
        if (typeof (CorbisUI.GlobalVars.MainCheckout.Tabs) == 'undefined') {
            CorbisUI.GlobalVars.MainCheckout.Tabs = {};
        }

        // since CorbisUI.GlobalVars.MainCheckout.Tabs
        // is set in the top of MainCheckout.aspx
        // we need to extend the object

        $extend(CorbisUI.GlobalVars.MainCheckout.Tabs, {
            paymentTabs: '<%= optionsBlockPayment.ClientID %>', 
            firstShow:<%=this.DefaultPayment%>       
        });
           
        CorbisUI.Checkout.PaymentOptions = {
        
           CorporatePane: '<%= corporateDisplay.ClientID %>', 
           CreditPane: '<%= creditDisplay.ClientID %>',
           selectedPayment:'<%= selectedPayment.ClientID %>', 
           selectedCreditUid: '<%= selectedCreditUid.ClientID %>', 
           updateCreditDisplay: '<%= updateCreditDisplay.ClientID %>',
           weDontUnderstandError: 'weDontUnderstandError'
         };
  
        paymentTabsIndex = 0;
        
</script>

<!--
*******************************
* PANE LAYOUT 
*******************************
-->
<div class="paneLayout" id="paymentPaneLayout">
    <h4>
        <Corbis:Localize ID="howPay" runat="server" meta:resourcekey="howPay" />
        <a href="javascript:void(0)" onclick="CorbisUI.Checkout.modals.securityQuestions(this,true,{vertical : 'bottom',horizontal : 'left'}); return false;">
            <Corbis:Localize ID="transactionSecurity" runat="server" meta:resourcekey="transactionSecurity" /></a>
    </h4>
    <asp:UpdatePanel id="paymentMethodsUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="optionsBlock" id="optionsBlockPayment" runat="server">
                <a href="javascript:void(0)" class="paymentOptionsCorporate" id="paymentOptionsCorporateAccount" onclick="PaymentPane('CorporateAccount');return false;"
                    runat="server"><span>
                        <Corbis:Localize ID="corporateOption" runat="server" meta:resourcekey="corporateOption" />
                    </span></a>
                <a href="javascript:void(0)" class="paymentOptionsSavedCard" id="paymentOptionsSavedCreditCard" onclick="PaymentPane('CreditCard');CorbisUI.Checkout.modals.newCreditCardModal(false); return false;"
                    runat="server"><span>
                        <Corbis:Localize ID="savedCardOption" runat="server" meta:resourcekey="savedCardOption" />
                    </span></a>
                <a href="javascript:void(0)" class="paymentOptionsNewCard" id="paymentOptionsNewCreditCard" onclick="PaymentPane('CreditCard');CorbisUI.Checkout.modals.newCreditCardModal(true); return false;"
                     runat="server"><span>
                        <Corbis:Localize ID="newCardOption" runat="server" meta:resourcekey="newCardOption" />
                    </span></a>
                <asp:HiddenField ID="selectedPayment" runat="server" EnableViewState="true" />
                <div class="clr">
                    &nbsp;</div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="paymentChosenBlock" class="MT_10">
        <div id="paymentChosen_Indicator">
            <div id="corporateDisplay" runat="server" class="subcontainer">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="corporateAccountPanel" runat="server">
                            <div id="corporateAccountSingle" runat="server">
                                <span class="bold">
                                    <Corbis:Localize ID="youveChosen" runat="server" meta:resourcekey="youveChosen" /></span>
                                <Corbis:Label ID="corporateAccountText" runat="server" />
                                <asp:HiddenField ID="corporateAccountID" runat="server" EnableViewState="true" />
                            </div>
                            <div id="corporateAccountMultiple" runat="server">
                                <span id="youveChosen" class="bold">
                                    <Corbis:Localize ID="youveChosenSingle" runat="server" meta:resourcekey="youveChosen" /></span>
                                <Corbis:DropDownList ID="corporateAccountsList" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="corporateAccountsList_SelectedIndexChanged">
                                </Corbis:DropDownList>
                            </div>
                            <div class="Error" runat="server" id="corporateAccountErrorBlock">
                                <div>
                                    <Corbis:Localize ID="corporateAccountMultipleErr" runat="server" Visible="false"
                                        meta:resourcekey="corporateAccountMultipleErr" />
                                    <Corbis:Localize ID="corporateAccountSingleErr" runat="server" Visible="false" meta:resourcekey="corporateAccountSingleErr" />
                                </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="creditDisplay" runat="server" class="subcontainer">
                <asp:UpdatePanel runat="server" ID="selectedCardUpdatePanel" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="updateCreditDisplay" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:Button ID="updateCreditDisplay" runat="server" CausesValidation="false" OnClick="cardSelector_CreditCardSelected" CssClass="hdn" />
                        <asp:Panel runat="server" ID="selectCardBlock">
                            <div id="paymentValidationSection">
                                <span class="bold">
                                    <Corbis:Localize ID="creditDisplyChosen" runat="server" meta:resourcekey="youveChosen" /></span>
                                <Corbis:Label ID="creditDisplyText" runat="server" Text="" />
                                <a href="javascript:void(0)" onclick="CorbisUI.Checkout.modals.newCreditCardModal(false);return false;">
                                    <Corbis:Localize ID="change2" runat="server" meta:resourcekey="change" /></a>.
                                <span class="bold">
                                    <Corbis:Localize ID="ccVerificationCode" runat="server" meta:resourcekey="ccVerificationCode" /></span>
                                <Corbis:TextBox ID="ccVerificationCodeTextBox" runat="server" MaxLength="4" Columns="4"
                                    validate="required;cvv" meta:resourcekey="ccVerificationCodeTextBox"/>
                                <div style="display: none">
                                    <asp:PlaceHolder ID="validatorContainer" runat="server" />
                                </div>
                                <Corbis:Image ID="iconhelp" CssClass="iconhelp" runat="server" ImageUrl="/Images/info.gif" />
                                <asp:HiddenField ID="selectedCreditUid" runat="server" EnableViewState="true" />
                                <Corbis:ValidationHub ID="paymentValidationHub" runat="server" UniqueName="Payment" IsPopup="false" SuccessScript="clickPromoValidateLink();" 
                                ContainerID="paymentValidationSection" AutoInit="false" 
                                FailScript="enablePaymentNext();"  />
                            </div>
                        </asp:Panel>
                        
                        <asp:Panel runat="server" ID="selectCardBlockErr" Visible="false">
                            <div class="Error">
                                <div>
                                    <strong>
                                        <Corbis:Localize runat="server" meta:resourcekey="weDontUnderstand" />
                                    </strong>
                                </div>
                            </div>
                        </asp:Panel>
                        
                        
                                
                        <asp:Panel runat="server" ID="cardExpiredErr" Visible="false">
                            <div class="Error">
                                <div>
                                    <strong>
                                        <Corbis:Localize ID="cardExpredErrDetail" runat="server" meta:resourcekey="cardExired" />
                                    </strong>
                                </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div id="paymentChosen_Error" class="Error" runat="server">
            <div>
                <strong>
                    <Corbis:Localize ID="creditReview" runat="server" meta:resourcekey="creditReview" />
                </strong>
                <Corbis:Localize ID="makeAnotherSelection" runat="server" meta:resourcekey="makeAnotherSelection" />
                <a id="paymentError" href="javascript:void(0)" onclick="javascript:CorbisUI.ContactCorbis.ShowContactCorbisModal(this);">
                    <Corbis:Localize ID="contactCorbis" runat="server" meta:resourcekey="contactCorbis" />
                </a>
                <Corbis:Localize ID="forAssistance2" runat="server" meta:resourcekey="forAssistance" />
            </div>
        </div>
                            <div class="Error" ID="weDontUnderstandError" style="display:none;">
                                <div>
                                    <strong>
                                        <Corbis:Localize ID="Localize2" runat="server" meta:resourcekey="weDontUnderstand" />
                                    </strong>
                                </div>
                            </div>
        
    </div>
    <div class="MT_10">
    </div>
    <hr />
    
        <asp:UpdatePanel ID="promoUpdatePanel" UpdateMode="Always" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
            
                <div class="paymentPromoCodeBlock" id="paymentPromoCodeBlock" runat="server">
                    <Corbis:WorkflowBlock WorkflowType="CART" runat="server">
                    <h4 class="paymentPromo_Header">
                        <Corbis:Localize ID="havePromo" runat="server" meta:resourcekey="havePromo" />
                    </h4>
                    <div style="margin-left: 20px">
                        <Corbis:Localize ID="promoCodeInfo" runat="server" meta:resourcekey="promoCodeInfo" /><br />
                        <asp:TextBox runat="server" ID="promoField" MaxLength="255" CssClass="paymentPromo_Input thingValue" />
                        <AJAXToolkit:TextBoxWatermarkExtender ID="promoCodeWatermark" runat="server" TargetControlID="promoField"
                            WatermarkText="<%$ Resources: Resource, Optional %>" WatermarkCssClass="Optional" />
                    </div>
                    </Corbis:WorkflowBlock>                    
                    <asp:Panel runat="server" class="ValidationSummary" ID="promoValidator">
                        <ul>
                            <li>
                                <Corbis:Localize runat="server" meta:resourcekey="promoValidator" /></li></ul>
                    </asp:Panel>
                    <div class="displayNone">
                        <asp:LinkButton ID="validatePromoLB" OnClick="validatePromoLB_Click" runat="server"
                            Text="test" />
                    </div>
                </div>
                
            </ContentTemplate>
        </asp:UpdatePanel>
    <script>
        function clickPromoValidateLink() {
            // ron: apparently this keeps the screen from going gray by
            // not using an async trigger
            if ($('paymentPaneLayout').getElement('div[id$=cardExpiredErr]') != null) {
                //console.log('in err');
                setTimeout(enablePaymentNext, 500); 
                return false;
            }
            if (weDontUnderstand()) {
                showWeDontUnserstand();
                //console.log('in understand');
                setTimeout(enablePaymentNext, 500);
                return;
            }
            var arg = '<%=validatePromoLB.ClientID %>'.replace(/_/g, '$');
            //console.log(arg);
            __doPostBack(arg, '');
        }
        function weDontUnderstand() {
            if ($(CorbisUI.Checkout.PaymentOptions.selectedPayment).value.contains('CreditCard') &&
                ($(CorbisUI.Checkout.PaymentOptions.selectedCreditUid) == null || $(CorbisUI.Checkout.PaymentOptions.selectedCreditUid).value == ''))
                return true;
            return false;
        }
        function showWeDontUnserstand() {
            $('weDontUnderstandError').setStyle('display', 'block');
        }

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(updateCCVTips);

        function updateCCVTips() { CorbisUI.QueueManager.CheckoutMacros.runItem('ccvReattach'); }

        function pageLoad(sender, args) {
            if (Sys != null && Sys.CultureInfo != null) {
                Sys.CultureInfo.CurrentCulture.numberFormat.CurrencySymbol = '';
            }
            enablePaymentNext();
            setTimeout(initPaymentValidation, 100);
        }
        function nextStep() {
            if ($(CorbisUI.Checkout.PaymentOptions.selectedPayment).value.contains('CorporateAccount')) {
                clickPromoValidateLink();
            }
            else {
                _PaymentValidation.validateAll();
            }
        }
    </script>

</div>
<div class="buttonBar">
    <!-- "Want to quit checkout?" modal -->
<Corbis:ModalPopup ContainerID="quitCheckout" runat="server" meta:resourcekey="areYouSure">
    <Corbis:GlassButton runat="server" ButtonStyle="Gray" CausesValidation="false" ID="continueCheckout" meta:resourcekey="continue" OnClientClick="HideModal('quitCheckout');return false;" />  
    <Corbis:GlassButton runat="server" CausesValidation="false" ID="quitCheckout" meta:resourcekey="quit" OnClientClick="GoToCartByFlow(true);return false;" />  
</Corbis:ModalPopup>

    <a href="javascript:OpenModal('quitCheckout'); ResizeModal('quitCheckout');"><Corbis:Localize ID="quit" runat="server" meta:resourcekey="quit" /></a>

    <ul class="buttonPositioner">
        <li class="previous">
            <Corbis:GlassButton ID="btnPaymentPrevious" OnClientClick="checkoutTabs.show(1);UpdateBasedOnCurrentTabIndex({tabIndex:1});return false;"
                runat="server" CssClass="backStepButton" ButtonStyle="Gray" ButtonBackground="gray36"
                meta:resourcekey="previousStep" />
        </li>
        <li class="next">
            <Corbis:GlassButtonEx ID="btnPaymentNext" OnClientClick="nextStep();return false;" ProgressPosition="Right" DisablePeriod="-1"
                runat="server" CausesValidation="true" CssClass="afterPreviousButton nextStepButton" ButtonStyle="Orange"
                meta:resourcekey="nextStep" />
        </li>
    </ul>
</div>
