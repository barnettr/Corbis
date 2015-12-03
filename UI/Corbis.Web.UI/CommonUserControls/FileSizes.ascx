<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileSizes.ascx.cs" Inherits="Corbis.Web.UI.CommonUserControls.FileSizes" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="Corbis" TagName="ContactCorbis" Src="~/UserControls/ContactControls/ContactCorbis.ascx" %>

<Corbis:ModalPopup ID="fileSizeModal" ContainerID="fileSizeModal" Width="428" runat="server" CloseScript="HideModal('fileSizeModal');return false;" meta:resourcekey="fileSizeModal" >
	<div class="headerMessage">
		<Corbis:Localize ID="headerMessage" runat="server" meta:resourcekey="headerMessage" />
	</div>
	<div class="fileSizes">
		<table cellspacing="0"><tr>  <!-- to solve bug no. 16285: allow text wrapping in languages such as German for the headers -->
		    <td class="rfFileSizeHeader"><Corbis:Localize ID="rfFileSizeHeader" runat="server" meta:resourcekey="rfFileSizeHeader" /></td>
		    <td class="betweenFileSizeHeaders">&nbsp;</td>
		    <td class="rmFileSizeHeader"><Corbis:Localize ID="rmFileSizeHeader" runat="server" meta:resourcekey="rmFileSizeHeader" /></td>
		</tr></table>
		<div class="rmFileSize"><Corbis:Localize ID="rmWebSize" runat="server" meta:resourcekey="rmWebSize" /></div>
		<div class="rfFileSize"><Corbis:Localize ID="rfWebSize" runat="server" meta:resourcekey="rfWebSize" /></div>
		<div class="rmFileSize"><Corbis:Localize ID="rmSmallSize" runat="server" meta:resourcekey="rmSmallSize" /></div>
		<div class="rfFileSize"><Corbis:Localize ID="rfSmallSize" runat="server" meta:resourcekey="rfSmallSize" /></div>
		<div class="rmFileSize"><Corbis:Localize ID="rmMediumSize" runat="server" meta:resourcekey="rmMediumSize" /></div>
		<div class="rfFileSize"><Corbis:Localize ID="rfMediumSize" runat="server" meta:resourcekey="rfMediumSize" /></div>
		<div class="rmFileSize"><Corbis:Localize ID="rmLargeSize" runat="server" meta:resourcekey="rmLargeSize" /></div>
		<div class="rfFileSize"><Corbis:Localize ID="rfLargeSize" runat="server" meta:resourcekey="rfLargeSize" /></div>
		<div class="rfFileSize"><Corbis:Localize ID="rfXLargeSize" runat="server" meta:resourcekey="rfXLargeSize" /></div>
	</div>
	<div class="footerMessage">
		<Corbis:Localize ID="footerMessage" runat="server" meta:resourcekey="footerMessage" />
	</div>
	<div class="button">
		<Corbis:GlassButton ID="close" runat="server" CausesValidation="false" ButtonStyle="Orange" EnableViewState="false" meta:resourceKey="close" OnClientClick="HideModal('fileSizeModal');return false;" CssClass="closeButton" />
	</div>
</Corbis:ModalPopup>    
<Corbis:ContactCorbis runat="server" />
