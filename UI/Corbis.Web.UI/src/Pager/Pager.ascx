<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Pager.ascx.cs" Inherits="Corbis.Web.UI.Navigation.Pager" EnableViewState="true" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<div id="Pager" >
    <asp:Button ID="previous" runat="server" OnClientClick="if (textChanged) return false; else return true;" OnCommand="ProcessCommand" CommandArgument="-1" Enabled="False"/>
    <Corbis:Label ID="prefixText" runat="server" />
	<asp:TextBox ID="pageNumber" runat="server" Text="1" MaxLength="3" onkeypress="return CorbisUI.Pager.keyCheck(event, this);" onchange="CorbisUI.Pager.pageNumberChanged(this);" OnTextChanged="ProcessTextChange"/>
    <Corbis:Label ID="postfixText" runat="server" />
    <asp:Button  ID="next" runat="server" OnClientClick="if (textChanged) return false; else return true;"  OnCommand="ProcessCommand" CommandArgument="1"/>
    <input type="hidden" ID="totalItems" class="totalItems" runat="server" />
    <input type="hidden" ID="pageSize" class="pageSize" runat="server" />
    <input type="hidden" ID="origPageNumber" class="origPageNumber" runat="server" value="1"/>
</div>
