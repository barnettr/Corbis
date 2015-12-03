<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditPaymentInformation.aspx.cs" MasterPageFile="~/MasterPages/ModalPopup.Master" Inherits="Corbis.Web.UI.Accounts.EditPaymentInformation" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>


<asp:Content ID="editPaymentInfoModalPopup" ContentPlaceHolderID="mainContent" runat="server">

<script type="text/javascript" language="javascript">

function bindExpDropdownsSaved() { }
function bindExpDropdownsNew() { }

window.addEvent('load', function() {
    parent.ResizeIModal('editPaymentInfoModalPopup', GetDocumentHeight());
    if (Browser.Engine.trident && $(document.body).getElement('input[id$=cardNumber]') != null && !$(document.body).getElement('input[id$=cardNumber]').disabled) {
        try {
            $(document.body).getElement('input[id$=cardNumber]').focus();
        }
        catch (e) {
        }
    }
})

</script>

<div id="EditPaymentInformationDiv" runat="server" class="editPaymentInformationDiv">
    <div class="titleWrapper">
        <span class="title"><Corbis:Localize ID="creditCardTitleAdd" runat="server" meta:resourcekey="CreditCardTitleAdd" /><Corbis:Localize ID="creditCardTitleEdit" runat="server" meta:resourcekey="CreditCardTitleEdit" /><Corbis:Localize ID="creditCardTitleDelete" runat="server" meta:resourcekey="CreditCardTitleDelete" /></span>
        <div class="personalCloseButton" id="personalCloseButton">
            <Corbis:Image ID="editPaymentInfoPopupImage" ImageUrl="/Images/iconClose.gif" runat="server" onclick="parent.MochaUI.CloseModal('editPaymentInfoModalPopup');return false;" class="Close" meta:resourcekey="editPaymentInfoPopupImage"/>
        </div>
    </div>
    <div id="addEditCreditCardDiv" class="addEditCreditCardDiv" runat="server">
        <div id="addEditCreditCardDiv">
        <Corbis:ValidationHub ID="vHub" SubmitForm="true"  
            IsIModal="true" runat="server" ContainerID="addEditCreditCardDiv"
            PopupID="editPaymentInfoModalPopup" 
        />  
        <table cellpadding="1" cellspacing="0" class="tableWidth">
            <tr class="FormRow">
                <td class="LeftForm alignLeft" class="fontSize12" colspan="2"><div class="checkLblPair">
                <Corbis:ImageCheckbox ID="defaultCreditCard" checked="false" runat="server" meta:resourcekey="DefaultCreditCardCheckBox">
                </Corbis:ImageCheckbox></div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                </td>
            </tr>
            <tr id="Tr1" class="FormRow" runat=server>
                <td class="LeftForm">
                    <Corbis:Label ID="CardTypeLabel" runat="server" meta:resourceKey="CardTypeLabel" />
                </td>
                <td class=FormRightCreditTop>
                    <Corbis:DropDownList runat="server" ID="cardTypeList" meta:resourcekey="cardTypeList" validate="custom1" custom1="validateCardType()" CssClass="creditCardField" EnableViewState="true"  />
                    <Corbis:Label runat="server" ID="cardTypeDisplayText" Visible="false" Text="Visa" />
                </td>
            </tr>
            <tr id="Tr2" class="FormRow" runat=server>
                <td class=LeftForm>
                    <Corbis:Label ID="cardNumberLabel" runat="server" meta:resourcekey="cardNumberLabel" /></td>
                <td class=FormRight>
                    <Corbis:TextBox runat="server" ID="cardNumber" MaxLength="16" CssClass="creditCardField" 
                        validate="required;digit;length[13,16]" 
                        EnableViewState="true" 
                        required_message="<%$ Resources:Accounts, CreditCardValidationError_InvalidCardNumber %>"
                        digit_message="<%$ Resources:Accounts, CreditCardValidationError_InvalidCardNumber %>"
                        length_str_message="<%$ Resources:Accounts, CreditCardValidationError_InvalidCardNumber %>"
                        autocomplete="off"                        
                    />
                    <Corbis:Label runat="server" ID="cardNumberDisplayText" Visible="false" Text="************1111" />
                </td>
            </tr>
            <tr id="Tr3" class="FormRow" runat=server>
                <td class=LeftForm>
                    <Corbis:Label ID="ExpirationDateLabel" runat="server" meta:resourcekey="ExpirationDateLabel" /></td>
                <td class=FormRight>
                    <Corbis:DropDownList runat="server" CssClass="cardMonth" ID="cardMonth" EnableViewState="true" />
                    <Corbis:DropDownList runat="server" CssClass="cardYear" ID="cardYear" EnableViewState="true"  />
                    <Corbis:TextBox 
                        validate="custom1" custom1="validateExpiry()" EnableViewState="true" 
                        ID="expirationDateText" CssClass="displayNone" runat="server"
                        custom1_message="<%$ Resources:Accounts, CreditCardValidationError_InvalidExpirationDate %>"
                    />
                </td>
            </tr>
            <tr runat="server" id="expiredCardRow" class="displayNone">
                <td colspan="2">
                    <div class="">
                        <ul><li><Corbis:Localize ID="expiredCardError" runat="server" meta:resourcekey="expiredCardError" /></li></ul>
                    </div>
                </td>
            </tr>
            <tr id="Tr4" class="FormRow" runat=server>
                <td class=LeftForm>
                    <Corbis:Label ID="CardholderLabel" runat="server" meta:resourcekey="CardholderLabel" /></td>
                <td class=FormRight>
                    <Corbis:TextBox  validate="required" ID="cardHolder" MaxLength="100" CssClass="creditCardField" runat="server" 
                        EnableViewState="true" 
                        required_message="<%$ Resources:Accounts, CreditCardValidationError_NameRequired %>"
                    />
                    
                </td>
            </tr>
        </table>

        <div class="ButtonRow350">
            <Corbis:LinkButton ID="lb" runat="server" OnClick="okBtn_OnClick" CssClass="ValidateClickLB displayNone" />
            <Corbis:GlassButton ID="okBtn" runat="server" OnClientClick="return false;" validate="submit" meta:resourceKey="save" CssClass="buttonsSpacing"/>
            <Corbis:GlassButton runat="server" ButtonStyle="Gray"  ID="cancelBtn" OnClientClick="parent.MochaUI.CloseModal('editPaymentInfoModalPopup');return false;" CssClass="buttonsSpacing"  meta:resourcekey="CancelEditAdd" />

            <div class="clr"></div>
        </div> 
        
        <script language="javascript">
            var _cardTypeList = $('<%=cardTypeList.ClientID %>');
            
            function buildExpDate() {
                $('<%=expirationDateText.ClientID %>').value = $('<%=cardMonth.ClientID %>').value + '/' + $('<%=cardYear.ClientID %>').value;
            }
            function bindExpDropdowns() {
                $('<%=cardMonth.ClientID %>').addEvent('change', buildExpDate);
                $('<%=cardYear.ClientID %>').addEvent('change', buildExpDate);
                //initCardValidation();
            }
            var _CardValidation;
            function initCardValidation() {
                _CardValidation = new CorbisFormValidator('aspnetForm', {
                    resizeScript: "parent.ResizeIModal('editPaymentInfoModalPopup', GetDocumentHeight());",
                    submitForm: false,
                    submitByAjax: false,
                    containerID: '<%=addEditCreditCardDiv.ClientID %>'
                });
            }

            function validateCardType() {
                var result = (_cardTypeList.selectedIndex == 0);
                _CardValidation.highlightRow(result, _cardTypeList);
                return !result;
            }

            function validateExpiry() {
                var selectedMonth = parseInt($('<%=cardMonth.ClientID %>').value, 10) - 1; // javascript months are zero based (january == 0, not 1)
                var selectedYear = parseInt($('<%=cardYear.ClientID %>').value, 10);
                var today = new Date();
                var expression = today.getFullYear() == selectedYear ?
            selectedMonth >= today.getMonth() : //cards that expire in current month are still valid
            today.getFullYear() < selectedYear;
                if (expression) {
                    // remove the validation that comes from the server on a saved card, if any
                    $('<%=expiredCardRow.ClientID %>').addClass('displayNone')
                }
                return expression;
            }

            window.addEvent('domready', initCardValidation);
        </script>
        </div>
    </div>
    <asp:Panel ID="deleteInformationPanel" runat="server">
        <div class="deleteMessage">
            <Corbis:Localize ID="confirmDeleteMessage" runat="server" meta:resourcekey="ConfirmDeleteMessage" /><Corbis:Localize ID="confirmDeleteDefaultMessage" runat="server" meta:resourcekey="ConfirmDeleteDefaultMessage" />
            <div class="clear MB_5"></div>
            <div class="profileButtons">
                <Corbis:GlassButton onclick="DeleteButton_Click"
                    ID="deleteYes" runat="server" CssClass="buttonsSpacing" 
                    CausesValidation="false" ButtonBackground=e8e8e8 
                    meta:resourceKey="Okay"
                />
                <Corbis:GlassButton 
                    ID="deleteNo" runat="server" 
                    CausesValidation="false" ButtonBackground=e8e8e8 ButtonStyle=gray CssClass="buttonsSpacing"
                    meta:resourceKey="Cancel" OnClientClick="parent.MochaUI.CloseModal('editPaymentInfoModalPopup');return false;"
                />
            </div>
        </div>
    </asp:Panel>  
</div>
<div class="clr"></div>

</asp:Content>
