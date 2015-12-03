<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreditCardBlock.ascx.cs"
    Inherits="Corbis.Web.UI.Checkout.CreditCardBlock"  %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="AJAXExtentions" Namespace="AjaxControlToolkit.WCSFExtensions"
    Assembly="AjaxControlToolkit.WCSFExtensions" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<asp:UpdatePanel runat="server">
<ContentTemplate>
<div id='<%=vhub.UniqueName %>CardContainer'>

    <Corbis:ValidationHub 
        ID="vhub" runat="server" IsIModal="false" 
        IsPopup="true" SubmitForm="true" AutoInit="false"
        ContainerID="<%=vhub.UniqueName %>CardContainer"
    />
    
    <table cellspacing="0" class=pop350>
    <tr id="cardListSection" class=FormRow runat=server>
        <td class="FormLeft">
            <Corbis:Label ID="selectACardLbl" runat="server" meta:resourcekey="selectACardLbl" /></td>
        <td class=FormRight>
            <asp:DropDownList 
                ID="creditCardList" EnableViewState="true" runat="server" 
                AutoPostBack="true" OnSelectedIndexChanged="creditCardList_SelectedIndexChanged"
            />
        </td>   
    </tr>
    <tr class="FormRow" runat=server>
        <td class="FormLeft">
            <Corbis:Label ID="cardTypeLabel" runat="server" meta:resourcekey="cardTypeLabel" />
        </td>
        <td class=FormRight>
            <Corbis:DropDownList runat="server" ID="cardTypeList" EnableViewState="true"  />
            <Corbis:Label runat="server" ID="cardTypeDisplayText" meta:resourcekey="cardTypeDisplayText" />
        </td>
        
    </tr>
    <tr class="FormRow" runat=server>
        <td class=FormLeft>
            <Corbis:Label ID="cardNumberLabel" runat="server" meta:resourcekey="cardNumberLabel" /></td>
        <td class=FormRight>
            <Corbis:TextBox runat="server" ID="cardNumber" MaxLength="16" validate="required;digit;length[16,17]" 
                EnableViewState="true"  meta:resourcekey="cardNumber" autocomplete="off" />
            <Corbis:Label runat="server" ID="cardNumberDisplayText" meta:resourcekey="cardNumberDisplayText" />
        </td>
    </tr>
    <tr class="FormRow" runat=server>
        <td class=FormLeft>
            <Corbis:Label ID="cardExpirationDateLabel" runat="server" meta:resourcekey="cardExpirationDateLabel" /></td>
        <td class=FormRight>
            <Corbis:DropDownList runat="server" CssClass="cardMonth" ID="cardMonth" EnableViewState="true" />
            <Corbis:DropDownList runat="server" CssClass="cardYear" ID="cardYear" EnableViewState="true"  />
            <Corbis:TextBox 
                validate="custom1" meta:resourcekey="expiryText" EnableViewState="true" 
                ID="expirationDateText" CssClass="displayNone" runat="server"
            />
        </td>
    </tr>
    <tr runat="server" id="expiredCardRow" class="displayNone">
        <td colspan="2">
            <div class="ValidationSummary">
                <ul>
                    <li>
                        <Corbis:Localize ID="expiredCardError" runat="server" meta:resourcekey="expiredCardError" />
                     </li>
                </ul>
            </div>
        </td>
    </tr>
    <tr class="FormRow" runat=server>
        <td class=FormLeft>
            <Corbis:Label ID="cardHolderLabel" runat="server" meta:resourcekey="cardHolderLabel" /></td>
        <td class=FormRight>
            <Corbis:TextBox  validate="required" ID="cardHolder" CssClass="cardHolder" runat="server" 
                meta:resourcekey="cardHolder" EnableViewState="true" 
            />
            
        </td>
    </tr>
    </table>

    <div class="ButtonRow350" >
        <Corbis:LinkButton ID="lb" runat="server" OnClick="okBtn_OnClick" CssClass="ValidateClickLB displayNone" />
        <Corbis:GlassButton ID="okBtn" runat="server"  validate="submit" meta:resourceKey="save" CssClass="buttonsSpacing"/>
        <Corbis:GlassButton runat="server" ButtonStyle="Gray"  ID="cancelBtn" OnClick="cancelBtn_OnClick"  meta:resourcekey="Cancel" />
        
        <div class="clr"></div>
    </div>        
</div>
</ContentTemplate>
</asp:UpdatePanel>
<script language=javascript type="text/javascript">
    function buildExpDate<%=vhub.UniqueName %>() {
        $('<%=expirationDateText.ClientID %>').value = $('<%=cardMonth.ClientID %>').value + '/' + $('<%=cardYear.ClientID %>').value;
    }
    function bindExpDropdowns<%=vhub.UniqueName %>() {
        $('<%=cardMonth.ClientID %>').addEvent('change', buildExpDate<%=CardType %>);
        $('<%=cardYear.ClientID %>').addEvent('change', buildExpDate<%=CardType %>);
        init<%=vhub.UniqueName %>Validation();
    }
    function bindExpDropdowns(){}
    
    
    function validate<%=vhub.UniqueName %>Expiry(){
        var selectedMonth = parseInt($('<%=cardMonth.ClientID %>', 10).value) - 1;// javascript months are zero based (january == 0, not 1)
        var selectedYear = parseInt($('<%=cardYear.ClientID %>', 10).value);
        var today = new Date();
        var expression =  today.getFullYear() == selectedYear ? 
            selectedMonth >= today.getMonth() : //cards that expire in current month are still valid
            today.getFullYear() < selectedYear;
        if (expression)
        {
            // remove the validation that comes from the server on a saved card, if any
            $('<%=expiredCardRow.ClientID %>').addClass('displayNone')
        }
        //alert(expression);
        return expression;
    }
    
</script>
 