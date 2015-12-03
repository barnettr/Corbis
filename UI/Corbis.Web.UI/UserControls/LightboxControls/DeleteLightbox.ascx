<%@ Control Language="C#" AutoEventWireup="true" Codebehind="DeleteLightbox.ascx.cs" Inherits="Corbis.Web.UI.Lightboxes.DeleteLightbox" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>


<Corbis:LinkButton ID="deleteLightboxHidden" runat="server" />
<div id="deleteLightboxModalPopup" style="display: none;" class="ModalPopupPanelDialog" >
	<div class="ModalTitleBar">
        <span class="Title" id="title">
		    <Corbis:ImageButton ID="deleteLightboxClose" runat="server" EnableViewState="false"
			CausesValidation="false" CssClass="Close" ImageUrl="/Images/iconClose.gif" OnClientClick="javascript:CloseDeleteLightboxPopup();return false;" />
			<Corbis:Localize ID="deleteLightboxTitle" runat="server" meta:resourcekey="deleteLightboxTitle" />
		</span>	
		 <span class="Title" id="titleSorry" style="display:none">
			<Corbis:Localize ID="deleteLightboxTitleSorry" runat="server" meta:resourcekey="deleteLightboxTitleSorry" />
		</span>	

	</div>
	<div class="ModalPopupContent">
		<span id="childrenLightboxMessage" style="display: none; text-align: center;">
			<Corbis:Localize ID="confirm" runat="server" meta:resourcekey="Confirm" />
		</span><span id="childrenMessageTemplate" style="display: none;text-align: center;">
			<Corbis:Localize ID="childrenMsgTemplate" runat="server" meta:resourcekey="childrenMsgTemplate" />
		</span><span id="sharedchildrenParentTemplate" style="display: none;text-align: center;">
			<Corbis:Localize ID="sharedchildrenbyParentMsg" runat="server" meta:resourcekey="sharedchildrenbyParentMsg" />
		</span><span id="sharedchildrenTemplate" style="display: none;text-align: center;">
			<Corbis:Localize ID="sharedchildrenMsg" runat="server" meta:resourcekey="sharedchildrenMsg" />
		</span>
		
		<div id="childrenMessage">
		</div>
		<div class="FormButtons" id="Buttons" style="text-align: right;">
		     <Corbis:GlassButton ID="cancel" runat="server" CausesValidation="false" ButtonStyle="Gray" EnableViewState="false" meta:resourceKey="Cancel" OnClientClick="javascript:CloseDeleteLightboxPopup();return false;" />
		     <Corbis:GlassButton ID="delete" runat="server" CausesValidation="false" EnableViewState="false" meta:resourceKey="Delete" />
		</div>
	</div>
</div>

