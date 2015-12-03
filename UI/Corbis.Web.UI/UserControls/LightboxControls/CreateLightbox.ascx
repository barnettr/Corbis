<%@ Control Language="C#" AutoEventWireup="true" Codebehind="CreateLightbox.ascx.cs" Inherits="Corbis.Web.UI.Lightboxes.CreateLightbox" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>




<div id="createLightboxModalPopup" style="display: none; width: 300px;">
	<div class="ModalPopupPanelDialog" style="width: 300px;">
		<div class="ModalTitleBar">
			<span class="Title">
			    <Corbis:ImageButton ID="createLightboxClose" runat="server" EnableViewState="false"
				CausesValidation="false" CssClass="Close" ImageUrl="/Images/iconClose.gif" OnClientClick="javascript:HideModal('createLightboxModalPopup');return false;" />
				<Corbis:Localize ID="createLightboxTitle" runat="server" meta:resourcekey="createLightboxTitle" />
			</span>
		</div>
		<div class="ModalPopupContent">
			<asp:Panel CssClass="FormButtons" DefaultButton="Create$glassBtnId$glassButton" runat="server">
				<asp:UpdatePanel ID="createLightboxPanel" runat="server" UpdateMode="conditional">
					<ContentTemplate>
						<Corbis:ValidationGroupSummary ID="createLightboxValidationSummary" runat="server" ValidationGroup="CreateLightbox" />
						<div class="clear">
						</div>
						<Corbis:TextBox ID="lightboxName" runat="server" MaxLength="40" Width="100%" ValidateControl="true"
							ValidationGroup="CreateLightbox" ValidationObjectType="Corbis.LightboxCart.Contracts.V1.ILightboxCartContractTransport"
							ValidationOperationName="CreateLightbox" ValidationRulesetName="CreateLightbox"
							ValidationParameterName="lightboxName" ValidatorContainer="lightboxNameValidatorContainer" ></Corbis:TextBox>
						<div class="displayNone">
							<asp:PlaceHolder ID="lightboxNameValidatorContainer" runat="server" />
						</div>
					</ContentTemplate>
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="Create" EventName="Click" />
						<asp:AsyncPostBackTrigger ControlID="setValidLink" EventName="Click" />
					</Triggers>
				</asp:UpdatePanel>
				<div class="MT_10">
					<Corbis:GlassButton ID="Cancel" runat="server" CssClass="btnGraydbdbdb" OnClientClick="javascript:HideModal('createLightboxModalPopup');return false;"
						Text="Cancel" EnableViewState="false" meta:resourceKey="cancel" Style="float: right" />
						<Corbis:LinkButton ID="setValidLink" runat="server" OnClick="setValidLink_Click" Text="set valid" CssClass="displayNone"/>
					<Corbis:GlassButtonEx ID="Create" runat="server" OnClientClick="javascript:var lightboxName = $(CorbisUI.GlobalVars.CreateLightbox.newLightboxName); lightboxName.setStyle('color', '#FFFFFF');" OnClick="Create_Click" Text="Save"
						EnableViewState="false" DisablePeriod="1000" meta:resourceKey="Create" />
				</div>
			</asp:Panel>
		</div>
	</div>
</div>
<script type="text/javascript" language="javascript">
	CorbisUI.GlobalVars.CreateLightbox = {
		newLightboxName: '<%= lightboxName.ClientID %>',
		cancelButtonID : '<%= Cancel.ClientID %>',
		saveButtonID   : '<%= Create.ClientID %>'
	}
</script>
