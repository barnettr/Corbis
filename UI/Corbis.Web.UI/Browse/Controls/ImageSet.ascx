<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ImageSet.ascx.cs" Inherits="Corbis.Web.UI.Browse.ImageSet" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<asp:Repeater ID="ImageSetImages" runat="server" OnItemDataBound="ImageSetImages_ItemDataBound">
	<HeaderTemplate>
		<div class="ImageSetContainer Node">
			<h3><asp:Literal ID="ImageSetHeader" runat="server" /></h3>
			<h3 class="subtitle"><asp:Literal ID="ImageSetSubHeader" runat="server" /></h3>
			<ul class="ImageSet">
	</HeaderTemplate>
	<ItemTemplate>
		<li id="ImageContainer" runat="server">
			<Corbis:CenteredImageContainer ID="thumbWrap" runat="server" ImageID="image" />
			<p>
				<strong><asp:Literal ID="ImageCaption" runat="server" /></strong><br />
				<asp:Literal ID="ImageDate" runat="server" /><br />
				<Corbis:Localize ID="SetIDText" runat="server" meta:resourcekey="SetIDText" /> <asp:Literal ID="ImageSetID" runat="server" /><br />
				<asp:HyperLink ID="ImageSetLink" runat="server" />
			</p>
		</li>
	</ItemTemplate>
	<FooterTemplate>
			</ul>
			<div class="ImageSetFooter">
				<asp:HyperLink ID="SeeImageSet" runat="server" />
			</div>
		</div>
	</FooterTemplate>
</asp:Repeater>