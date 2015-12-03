<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Collections.aspx.cs" Inherits="Corbis.Web.UI.Browse.Collections" MasterPageFile="~/Browse/MasterPages/MasterBase.Master" Title="<%$ Resources: windowTitle %>" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="Browse" TagName="LeftMenu" src="Controls/BrowseLeftMenu.ascx" %>
<%@ Register TagPrefix="Browse" TagName="Collection" src="Controls/Collection.ascx" %>
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
        <% //Marketing User generated content goes here %>
        <div id="ZoneMainModules" runat="server">
			<Browse:Collection ID="Collection" runat="server" />
        </div>
    </div>
</asp:Content>