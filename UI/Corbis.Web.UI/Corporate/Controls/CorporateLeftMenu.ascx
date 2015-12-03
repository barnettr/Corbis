<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="CorporateLeftMenu.ascx.cs" Inherits="Corbis.Web.UI.Corporate.CorporateLeftMenu" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<div class="LeftMenuBox">
	<div class="LeftMenu">
		<ul id="LeftMenu" runat="server">
			<li class="parent"><Corbis:HyperLink ID="AboutCorbis" runat="server" meta:resourcekey="AboutCorbis" /></li>
			<li class="parent"><Corbis:HyperLink ID="Pressroom" runat="server" meta:resourcekey="Pressroom" />
				<ul>
					<li><Corbis:HyperLink ID="PressReleases" runat="server" meta:resourcekey="PressReleases" /></li>
					<li><Corbis:HyperLink ID="PressFactSheet" runat="server" meta:resourcekey="PressFactSheet" /></li>
				</ul>
			</li>
			<li class="parent"><Corbis:HyperLink ID="Employment" runat="server" meta:resourcekey="Employment" /></li>
			<li class="parent"><Corbis:HyperLink ID="CustomerService" runat="server" meta:resourcekey="CustomerService" /></li>
		</ul>
	</div>
</div>