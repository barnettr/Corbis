<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckoutControl.ascx.cs" Inherits="Corbis.Web.UI.Navigation.CheckoutControl" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<div class="Cart" id="cartCountDiv">
    <div class="Right"><div class="Center"><div runat="server" meta:resourcekey="cartIcon">
        <span id="cartCount"><asp:Literal ID="cartCountLit" runat="server" /></span> <Corbis:Localize ID="cart" runat="server" meta:resourcekey="cart" /></div></div></div>
</div>
<div class="Checkout">
    <div class="Right"><div class="Center">
        <Corbis:HyperLink ID="checkout" navigateUrl="javascript:GoToCart(true)" runat="server" meta:resourcekey="checkout"/></div></div>
</div>