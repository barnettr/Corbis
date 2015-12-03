<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Slideshows.aspx.cs" Inherits="Corbis.Web.UI.Browse.Slideshows" MasterPageFile="~/Browse/MasterPages/NoGlobalNav.Master" Title="<%$ Resources: windowTitle %>" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="Browse" TagName="LeftMenu" src="Controls/BrowseLeftMenu.ascx" %>
<%@ Register TagPrefix="Browse" TagName="Slideshow" src="Controls/Slideshow.ascx" %>

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
        <% //Marketing User generated content goes here %>
        <div id="ZoneMainModules" runat="server">
			<Browse:Slideshow ID="Slideshow" runat="server" />
        </div>
    </div>
</asp:Content>