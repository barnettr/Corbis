<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreditCardAddEdit.aspx.cs" enableEventValidation="false"
    Inherits="Corbis.Web.UI.Checkout.CreditCardAddEdit" MasterPageFile="/MasterPages/ModalPopup.Master" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register Assembly="Corbis.Web.UI.Controls" Namespace="Corbis.Web.UI.Controls" TagPrefix="Corbis" %>
<%@ Register TagPrefix="Corbis" TagName="CreditCard" Src="~/Checkout/CreditCardBlock.ascx" %>





<asp:Content ID="ExpressCheckoutPage" ContentPlaceHolderID="mainContent" runat="server">
    <script language="javascript" type="text/javascript">
    
    function ResizeModal(div) {
        
        parent.ResizeIModal('creditCardAddEdit', GetDocumentHeight());
    }
    
    function bindExpDropdownsSaved() {}
    
    window.addEvent('load', ResizeModal);
    </script>

    <div class="ModalPopupPanelDialog" style="width: 350px;">
        <div class="ModalTitleBar mochaContent">
                <input class="Close" type="image" onclick="parent.CloseModal('creditCardAddEdit');return false;" style="border-width: 0px;" src="../Images/iconClose.gif"/>
                <Corbis:Label ID="pageTitle1" CssClass="Title" meta:resourcekey="addCard" runat="server" />
                <Corbis:Label ID="pageTitle2" CssClass="Title" meta:resourcekey="savedCard" runat="server" Visible="false" />
        </div>
    </div>
         <Corbis:CreditCard 
            ID="cardCreator" runat="server" 
            CloseButtonJS="parent.CloseModal('creditCardAddEdit');" 
            CreditCardUsageType="CreateNewCard" 
            OnCreditCardAdded="cardCreator_CreditCardAdded" 
            OnCancelClick="cardCreator_CancelClick" 
            ValidationGroup="AddCreditCard"
        />
        
        
        
        
        <Corbis:CreditCard
                ID="cardSelector" runat="server" 
                CloseButtonJS="parent.CloseModal('creditCardAddEdit');" 
                CreditCardUsageType="SelectFromSavedCards" 
                OnCreditCardSelected="cardSelector_CreditCardSelected"
                 OnCancelClick="cardSelector_CancelClick" 
                ValidationGroup="SaveCreditCard"
                visible="false"
            />
            
        <div class="clr smallClear"></div>

</asp:Content>
