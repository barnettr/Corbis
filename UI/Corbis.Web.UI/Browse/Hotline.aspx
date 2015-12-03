<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Hotline.aspx.cs" Inherits="Corbis.Web.UI.Browse.Hotline" MasterPageFile="~/Browse/MasterPages/MasterBase.Master" Title="<%$ Resources: windowTitle %>" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="Browse" TagName="LeftMenu" src="Controls/BrowseLeftMenu.ascx" %>
<%@ Register TagPrefix="Browse" TagName="BackLink" src="Controls/BackToReferringPageLink.ascx" %>

<asp:Content ID="historicalNav" runat="server" ContentPlaceHolderID="leftContent">
    <div class="LeftColumn">
        <div class="LeftMenuBox">
            <Browse:LeftMenu runat="server" ID="BrowseLeft" />
        </div>
        <% //Marketing User generated modules go here %>
        <div class="LeftModuleBox" id="ZoneLeftModules" runat="server"></div>
    </div>
</asp:Content>

<asp:Content ID="historicalContent" runat="server" ContentPlaceHolderID="mainContent">
    <div class="MainColumn">
        <Browse:BackLink runat="server" ID="BackLink" />
		<div id="Header">
			<h1 class="PageTitle"><Corbis:Localize ID="PageTitle" runat="server" meta:resourcekey="pageTitle" /></h1>
			<h2 class="PageSubtitle"><Corbis:Localize ID="PageSubtitle" runat="server" meta:resourcekey="pageSubtitle" /></h2>
		</div>
        <% //Marketing User generated content goes here %>
        <div id="ZoneMainModules" runat="server"></div>
    </div>
</asp:Content>