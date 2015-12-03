<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Overview.aspx.cs" Inherits="Corbis.Web.UI.Corporate.Overview" MasterPageFile="~/MasterPages/MasterBase.Master" Title="<%$ Resources: windowTitle %>" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<asp:Content ID="aboutCorbisContent" runat="server" ContentPlaceHolderID="mainContent">
    <div id="AboutCorbisContent">
        <h2><Corbis:Localize ID="pageTitle" runat="server" meta:resourcekey="pageTitle" /></h2>
    </div>
</asp:Content>