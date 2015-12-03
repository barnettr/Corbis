<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MediaSetsProducts.ascx.cs" Inherits="Corbis.Web.UI.MediaSetSearch.MediaSetsProducts" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<div id="ProductResults">
 <div id="DownloadingProhibitedDiv" visible="true" class="DownloadingProhibitedDiv" runat="server">
        <Corbis:Image ID="noDownload" ImageUrl="/Images/alertYellow.gif" CssClass="downloadingProhibited" AlternateText="" runat="server" /><Corbis:Localize ID="downloadingProhibited" runat="server" meta:resourcekey="downloadingProhibited" />
    </div>
<corbis:Label Style="color:white" Id="count" runat="server" Visible="false"></corbis:Label>
<asp:Repeater runat ="server" ID="MediaSetRepeater" OnItemDataBound="Result_ItemDataBound">
<ItemTemplate>
<div class="ProductBlock" id="productBlock" runat="server">
    <Corbis:CenteredImageContainer ID="thumbWrap" CssClass="thumbWrap" IsAbsolute="true" runat="server" ImageID="image" meta:resourcekey="productBlockImg" />
	<Corbis:Label ID="MediaSetType" CssClass="MediaSetType" runat="server" ></Corbis:Label>
    <Corbis:Label ID="MediaSetCreditLine" CssClass="MediaSetCreditLine" runat="server" ></Corbis:Label>
	<Corbis:Label ID="MediaSetTitle" CssClass="MediaSetTitle" runat="server"  ></Corbis:Label>
	<Corbis:Label ID="MediaSetId" CssClass="MediaSetId" runat="server" Text='<%# Eval("MediaSetId") %>' ></Corbis:Label>
	<Corbis:Label ID="DatePhotographed" CssClass="DatePhotographed" runat="server"  ></Corbis:Label>
</div>

</ItemTemplate>

</asp:Repeater>
</div>

