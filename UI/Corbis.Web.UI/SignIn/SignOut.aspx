<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignOut.aspx.cs" Inherits="Corbis.Web.UI.SignIn.SignOut" MasterPageFile="~/MasterPages/ModalPopup.Master" Title="<%$ Resources: windowTitle %>" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<asp:Content ID="signOutContent" ContentPlaceHolderID="ModalPopupContentPlace" runat="server">
    <div id="SignOutContent">
        <Corbis:Localize ID="signedOut" runat="server" meta:resourcekey="signedOut" />
        <Corbis:Button ID="ok" runat="server" meta:resourcekey="ok" />
    </div>
</asp:Content>
