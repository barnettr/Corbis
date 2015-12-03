<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/MasterBase.Master" CodeBehind="PageNotFound.aspx.cs" Inherits="Corbis.Web.UI.Errors.PageNotFound" Title="<%$ Resources: windowTitle %>" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<asp:Content ContentPlaceHolderID="mainContent" ID="mainContent" runat="server">
    <Corbis:Localize ID="Message" runat="server" meta:resourcekey="Message" />
</asp:Content>