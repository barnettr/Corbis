<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailLightbox.ascx.cs" Inherits="Corbis.Web.UI.Lightboxes.EmailLightbox" EnableViewState="false" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<Corbis:ModalPopup ID="emailLightbox" runat="server" ContainerID="emailLightbox" Width="600" CloseScript="CorbisUI.EmailLightbox.Handler.closeModal(); return false;" meta:resourcekey="emailLightbox">
	<div class="instructions"><Corbis:Localize ID="instructions" runat="server" meta:resourcekey="instructions" /></div>
    <asp:UpdatePanel ID="emailLightboxUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="emailLightboxPanel" runat="server" DefaultButton="sendButton$glassButton">
				<div class="inputFields">
                    <Corbis:ValidationGroupSummary ID="emailLightboxValidationSummary" runat="server" CssClass="Error" ValidationGroup="emailLightboxValidationGroup" HighlightRows="true" />
                    <div class="clear"></div>
                    <table id="formFields" cellpadding="0" cellspacing="0">
						<tbody>
							<tr class="FormRow">
								<td class="FormLeft"><span><Corbis:Localize ID="fromLabel" runat="server" meta:resourcekey="fromLabel" /></span></td>
								<td class="FormRight"><span><Corbis:Localize ID="fromEmail" runat="server" /></span></td>
							</tr>
							<tr class="FormRow" runat="server">
								<td class="FormLeft"><span><Corbis:Localize ID="toLabel" runat="server" meta:resourcekey="toLabel" /></span></td>
								<td class="FormRight"><Corbis:TextBox ID="to" runat="server" MaxLength="255" ValidateControl="true" ValidatorContainer="validatorContainer" ValidationGroup="emailLightboxValidationGroup" ValidationObjectType="Corbis.LightboxCart.Contracts.V1.ILightboxCartContractTransport" ValidationOperationName="EmailLightbox" ValidationParameterName="toEmails" ValidationRulesetName="EmailLightbox" /></td>
							</tr>
							<tr class="FormRow" runat="server">
								<td class="FormLeft"><span><Corbis:Localize ID="subjectLabel" runat="server" meta:resourcekey="subjectLabel" /></span></td>
								<td class="FormRight"><Corbis:TextBox ID="subject" runat="server" MaxLength="50" ValidateControl="true" ValidatorContainer="validatorContainer" ValidationGroup="emailLightboxValidationGroup" ValidationObjectType="Corbis.LightboxCart.Contracts.V1.ILightboxCartContractTransport" ValidationOperationName="EmailLightbox" ValidationParameterName="subject" ValidationRulesetName="EmailLightbox" /></td>
							</tr>
							<tr class="FormRow">
								<td class="FormLeft"><span><Corbis:Localize ID="lightboxLinkLabel" runat="server" meta:resourcekey="lightboxLinkLabel" /></span></td>
								<td class="FormRight"><span id="lightboxLinkDisplay"><Corbis:Localize ID="lightboxLinkDisplay" runat="server" /></span></td>
							</tr>
							<tr class="FormRow messageRow">
								<td class="FormLeft"><span><Corbis:Localize ID="messageLabel" runat="server" meta:resourcekey="messageLabel" /></span></td>
								<td class="FormRight"><Corbis:TextBox ID="message" runat="server" CssClass="message" TextMode="MultiLine"  onkeypress="if (event.keyCode == 13) {event.cancelBubble=true;}"/></td>
							</tr>
						</tbody>
                    </table>
                    <div class="displayNone">
						<asp:TextBox CssClass="lightboxId" ID="lightboxId" runat="server" />
						<asp:TextBox CssClass="lightboxLink" ID="lightboxLink" runat="server" />
                        <asp:PlaceHolder ID="validatorContainer" runat="server"></asp:PlaceHolder>
                    </div>
				</div>
			</asp:Panel>
		</ContentTemplate>
		<Triggers>
			<asp:AsyncPostBackTrigger ControlID="sendButton" />
		</Triggers>
	</asp:UpdatePanel>
	<div class="modalButtons">
		<Corbis:GlassButton CssClass="cancelButton" ID="cancelButton" runat="server" ButtonStyle="Gray" ButtonBackground="dbdbdb" meta:resourcekey="cancelButton" OnClientClick="javascript:CorbisUI.EmailLightbox.Handler.closeModal();return false;" />
		<Corbis:GlassButton CssClass="sendButton" ID="sendButton" runat="server" ButtonStyle="Orange" ButtonBackground="dbdbdb" meta:resourcekey="sendButton" OnClick="sendButton_Click"/>
	</div>                   
</Corbis:ModalPopup>
<Corbis:ModalPopup ID="emailSuccess" ContainerID="emailSuccess" runat="server" meta:resourcekey="emailSuccess">
	<Corbis:GlassButton CssClass="closeButton" ID="closeSuccessModal" runat="server" ButtonStyle="Orange" ButtonBackground="dbdbdb" meta:resourcekey="closeButton" OnClientClick="javascript:HideModal('emailSuccess'); return false;"/>
</Corbis:ModalPopup>
<script language="javascript" type="text/javascript">
	CorbisUI.GlobalVars.EmailLightbox = {
		lightboxLinkTemplate: '<%= LightboxLinkTemplate %>'
	};
</script>