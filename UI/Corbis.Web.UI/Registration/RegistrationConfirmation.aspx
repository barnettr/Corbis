<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegistrationConfirmation.aspx.cs" Inherits="Corbis.Web.UI.Registration.RegistrationConfirmation" MasterPageFile="~/MasterPages/MasterBase.Master" Title="<%$ Resources: windowTitle %>" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<asp:Content ID="registrationConfirmationContent" ContentPlaceHolderID="mainContent" runat="server">
    <div id="RegistrationConfirmationContent">
        <Corbis:Localize ID="thankYou" runat="server" meta:resourcekey="thankYou" />
        <Corbis:Button ID="ok" runat="server" meta:resourcekey="ok" />
    </div>
</asp:Content>
