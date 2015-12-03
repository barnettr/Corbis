<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pressroom.aspx.cs" Inherits="Corbis.Web.UI.Corporate.Pressroom" MasterPageFile="~/MasterPages/MasterBase.Master" Title="<%$ Resources: windowTitle %>" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<asp:Content ID="pressroomContent" runat="server" ContentPlaceHolderID="mainContent">
    <div id="PressroomContent">
        <h2><Corbis:Localize ID="pageTitle" runat="server" meta:resourcekey="pageTitle" /></h2>
    </div>
</asp:Content>