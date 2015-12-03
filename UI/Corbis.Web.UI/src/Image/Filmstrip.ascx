<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Filmstrip.ascx.cs" Inherits="Corbis.Web.UI.Image.Filmstrip" %>

<div id="filmstrip" runat="server">
	<img id="previous" src="/images/spacer.gif" alt="" runat="server" onclick="javascript:CorbisUI.Filmstrip.scrollFilmstrip(this, true);" />
	<div id="itemsWindowMessage" runat="server">
	</div>
	<div id="itemsWindow" runat="server">
		<ul id="items" runat="server">
			<%//Implement template here, if this is to be populated server side %>
		</ul>
	</div>
	<img id="next" src="/images/spacer.gif" alt="" runat="server" onclick="javascript:CorbisUI.Filmstrip.scrollFilmstrip(this, false);"/>
</div>
