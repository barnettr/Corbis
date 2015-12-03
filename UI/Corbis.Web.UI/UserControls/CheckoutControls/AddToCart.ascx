<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddToCart.ascx.cs" Inherits="Corbis.Web.UI.Checkout.AddToCart" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<Corbis:ModalPopup runat="server" ContainerID="addToCartConfirm" meta:resourcekey="addToCartConfirm" HideClose="true" CloseScript="MochaUI.HideModal('addToCartConfirm');return false;" Width="350">
    <Corbis:GlassButton ID="addToCartContinue" runat="server" ButtonStyle="Gray" meta:resourcekey="addToCartContinue" OnClientClick="MochaUI.HideModal('addToCartConfirm');return false;" />
     <Corbis:GlassButton ID="addToCartViewCart" runat="server" meta:resourcekey="addToCartViewCart" OnClientClick="if($('cartCount'))GoToCart();else if($(window.opener.document).getElement('#cartCount')){window.opener.GoToCart();window.close()}; return false;" />
</Corbis:ModalPopup>
