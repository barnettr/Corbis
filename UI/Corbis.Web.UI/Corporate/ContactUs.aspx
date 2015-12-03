<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactUs.aspx.cs" Inherits="Corbis.Web.UI.Corporate.ContactUs" MasterPageFile="~/MasterPages/MasterBase.Master" Title="<%$ Resources: windowTitle %>" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<asp:Content ID="contactUsContent" runat="server" ContentPlaceHolderID="mainContent">
    <div id="ContactUsContent">
        <h2><Corbis:Localize ID="pageTitle" runat="server" meta:resourcekey="pageTitle" /></h2>
    </div>
</asp:Content>