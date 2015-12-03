<%@ Control Language="C#" AutoEventWireup="true" Codebehind="AddToLightbox.ascx.cs"
	Inherits="Corbis.Web.UI.Lightboxes.AddToLightbox" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<div id="addToLightboxModalPopup" class="ModalPopupPanelDialog" style="display: none; width: 320px;">
	<div class="ModalTitleBar">
		<span class="Title">
		    <Corbis:ImageButton ID="addToLightboxClose" runat="server" EnableViewState="false"
			CausesValidation="false" CssClass="Close" ImageUrl="/Images/iconClose.gif" OnClientClick="javascript:ResetNewOrAddDivs();HideModal('addToLightboxModalPopup');return false;" />
			<Corbis:Localize ID="addToLightboxTitle" runat="server" meta:resourcekey="addToLightboxTitle" />
		</span>
	</div>
	<div class="ModalPopupContentLightbox addToLightBox">
		<asp:UpdatePanel ID="addToLightboxPanel" runat="server" UpdateMode="Always">
			<ContentTemplate>
				<input type="hidden" id="addAllItemsToLightboxHidden" runat="server" enableviewstate="false" value="" />
                <input type="hidden" id="commandButton" runat="server" enableviewstate="false" value="" />
				<input type="hidden" id="offeringUidHidden" runat="server" enableviewstate="false" value="" />
				<input type="hidden" id="newLightboxName" runat="server" enableviewstate="false" value="" />
				<asp:panel ID="AddToLightboxSection" runat="server" CssClass="MB_10" DefaultButton="addToLightboxBtn$GlassButton">
					<Corbis:DropDownList ID="lightboxDropDownList" runat="server" CssClass="InlineDropdown" EnableViewState="true" />
					<Corbis:GlassButton ID="addToLightboxBtn" runat="server" CausesValidation="false"
						EnableViewState="false" meta:resourceKey="addToLightbox" OnClick="addToLightboxBtn_Click" />
					<hr size="1" class="Divider" />
					<span class="actLikeLink" style="margin-left: 0px;"><a id="createNewLightboxBtn" onclick="showNewLightboxDiv(); return false;"
						href="javascript:void(0)" runat="server" meta:resourcekey="createNewLightboxAnchor">
						<Corbis:Localize ID="createNewLightboxLabel" runat="server" meta:resourcekey="createNewLightboxLabel" /></a></span>
				</asp:panel>
				<asp:panel id="createLightboxSection" class="displayNone" runat="server" DefaultButton="addToNewLightboxBtn$glassBtnId$GlassButton">
					<div class="MB_10">
						<Corbis:ValidationGroupSummary ID="addToNewLightboxSummary" runat="server" ValidationGroup="AddToNewLightbox" />
						<div class="clear">
						</div>
						<Corbis:TextBox CssClass="newLightboxName"
						    ID="lightboxName" runat="server" MaxLength="40" ValidateControl="true"
							ValidationGroup="AddToNewLightbox" ValidationObjectType="Corbis.LightboxCart.Contracts.V1.ILightboxCartContractTransport"
							ValidationOperationName="CreateLightbox" ValidationRulesetName="CreateLightbox"
							ValidationParameterName="lightboxName" ValidatorContainer="lightboxNameValidatorContainer"
						/>
						<div class="displayNone">
							<asp:PlaceHolder ID="lightboxNameValidatorContainer" runat="server" />
						</div>
					</div>
					<Corbis:GlassButtonEx ID="addToNewLightboxBtn" runat="server" CausesValidation="false" ProgressPosition="Right" DisablePeriod="-1"
						OnClick="addToNewLightboxBtn_Click" EnableViewState="false" meta:resourceKey="addToNewLightboxBtn" />
				</asp:panel>
			</ContentTemplate>
			<Triggers>
				<asp:AsyncPostBackTrigger ControlID="addToNewLightboxBtn" EventName="Click"/>
			</Triggers>
		</asp:UpdatePanel>
	</div>
</div>

<script language="javascript" type="text/javascript">
	CorbisUI.GlobalVars.AddToLightbox = {
		createLightboxButtonName: '<%= createNewLightboxBtn.ClientID %>',
		createLightboxSection: '<%= createLightboxSection.ClientID %>',
		addToLightboxSection: '<%= AddToLightboxSection.ClientID %>',
		addToNewLightboxSummary: '<%= addToNewLightboxSummary.ClientID %>',
		lightboxName: '<%= lightboxName.ClientID %>',
		offeringUidHiddenName: '<%= offeringUidHidden.ClientID %>',
		addAllItemsToLightboxHiddenName: '<%= addAllItemsToLightboxHidden.ClientID %>',
		commandButtonName: '<%= commandButton.ClientID %>'
	}

    function ParentPageRedirect()
    {
        try
        {
            self.parent.frames.location.reload();
            //window.close();
        }
        catch (Exception) {}
    }
</script>

