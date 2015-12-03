<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactCorbis.ascx.cs" Inherits="Corbis.Web.UI.CommonUserControls.ContactCorbis" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<Corbis:ModalPopup ID="contactCorbisModal" ContainerID="contactCorbisModal" Width="350" runat="server" CloseScript="HideModal('contactCorbisModal');return false;" meta:resourcekey="contactCorbisModal" >
	<div id="contactInfo">
	</div>
	<div class="button">
		<Corbis:GlassButton ID="close" runat="server" CausesValidation="false" ButtonStyle="Orange" EnableViewState="false" meta:resourceKey="close" OnClientClick="HideModal('contactCorbisModal');return false;" CssClass="closeButton" />
	</div>
</Corbis:ModalPopup>
<div id="contactAddressTemplate" class="hdn">
	<table cellpadding="0" cellspacing="0">
		<tbody>
			<tr>
				<td class="label">&nbsp;</td>
				<td class="data bold">{0}</td>
			</tr>
			<tr>
				<td class="label">&nbsp;</td>
				<td class="data">{1}</td>
			</tr>
			<tr>
				<td class="label"><Corbis:Localize ID="phoneLabel" runat="server" meta:resourcekey="phoneLabel" /></td>
				<td class="data">{2}</td>
			</tr>
			<tr>
				<td class="label"><Corbis:Localize ID="faxLabel" runat="server" meta:resourcekey="faxLabel" /></td>
				<td class="data">{3}</td>
			</tr>
			<tr>
				<td class="label"><Corbis:Localize ID="emailLabel" runat="server" meta:resourcekey="emailLabel" /></td>
				<td class="data"><a>{4}</a></td>
			</tr>
		</tbody>
	</table>
</div>