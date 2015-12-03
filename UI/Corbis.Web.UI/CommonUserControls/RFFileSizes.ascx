<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RFFileSizes.ascx.cs" Inherits="Corbis.Web.UI.CommonUserControls.RFFileSizes" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<Corbis:ModalPopup ID="fileSizeModal" ContainerID="fileSizeModal" Width="300"  
    runat="server" HideClose="true" meta:resourcekey="fileSizeModal" UseDefaultPadding="false">
    <br />
	<div style="margin-left:10px;text-align:left;">
		<div class="rfFileSizes"><Corbis:Localize ID="rfWebSize" runat="server" meta:resourcekey="rfWebSize" /></div>
		<div class="rfFileSizes"><Corbis:Localize ID="rfSmallSize" runat="server" meta:resourcekey="rfSmallSize" /></div>
		<div class="rfFileSizes"><Corbis:Localize ID="rfMediumSize" runat="server" meta:resourcekey="rfMediumSize" /></div>
		<div class="rfFileSizes"><Corbis:Localize ID="rfLargeSize" runat="server" meta:resourcekey="rfLargeSize" /></div>
		<div class="rfFileSizes"><Corbis:Localize ID="rfXLargeSize" runat="server" meta:resourcekey="rfXLargeSize" /></div>
	</div>
	<div class="footerMessage">
		<Corbis:Localize ID="footerMessage" runat="server" meta:resourcekey="footerMessage" />
	</div>
</Corbis:ModalPopup>    
