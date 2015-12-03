<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PressReleaseList.ascx.cs" Inherits="Corbis.Web.UI.Corporate.Controls.PressReleaseList" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<asp:Repeater ID="PressReleaseYears" runat="server" OnItemDataBound="PressReleaseYears_OnItemDataBound">
	<HeaderTemplate>
		<h3><asp:Literal ID="YearNavTitle" runat="server" /></h3>
		<ul id="years" class="years">
	</HeaderTemplate>
	<ItemTemplate><li><asp:Literal ID="Year" runat="server" /></li></ItemTemplate>
	<FooterTemplate></ul></FooterTemplate>
</asp:Repeater>

<asp:Repeater ID="PressReleasesByYear" runat="server" OnItemDataBound="PressReleasesByYear_OnItemDataBound">
	<HeaderTemplate><ul id="pressReleases" class="pressReleases"></HeaderTemplate>
	<ItemTemplate><asp:Literal ID="PressRelease" runat="server" /></ItemTemplate>
	<FooterTemplate></ul></FooterTemplate>
</asp:Repeater>