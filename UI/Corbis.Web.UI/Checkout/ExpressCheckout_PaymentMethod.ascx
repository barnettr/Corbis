<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpressCheckout_PaymentMethod.ascx.cs" Inherits="Corbis.Web.UI.Checkout.ExpressCheckout_PaymentMethod" %>
<%@ Register Assembly="Corbis.Web.UI.Controls" Namespace="Corbis.Web.UI.Controls" TagPrefix="Corbis" %>

<Corbis:AjaxDropDownList  runat="server" ID="selectPaymentMethod" Width="200">
    <asp:ListItem Value="none" Text="<%$ Resources:Resource, SelectOne %>" type="none" />
    
</Corbis:AjaxDropDownList>