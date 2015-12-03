<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeSuccess.ascx.cs" Inherits="Corbis.Web.UI.Accounts.ChangeSuccess" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<div class="ChangeSuccessContent ModalPopupPanelDialog">
    <div class="ModalTitleBar">
        <span class="Title"><Corbis:Localize ID="editBusinessInformationTitle" runat="server" meta:resourcekey="Title"></Corbis:Localize></span>    
        <Corbis:ImageButton ID="changeSuccessClose" runat="server" OnClientClick="HideModal('changeSuccessDiv');return false;" CausesValidation="false" CssClass="Close" ImageUrl="/Images/iconClose.gif" />
    </div>
    <div class="ModalPopupContent">
        <asp:UpdatePanel ID="changePasswordPanel" runat="server">
            <ContentTemplate>
                <asp:Panel id="whileChanging" runat="server">
                    <div>
                        <Corbis:Localize ID="successMessage" runat="server" meta:resourcekey="SuccessMessage" />
                    </div>
                    <div class="Buttons">
                        <Corbis:GlassButton ID="changeClose" runat="server" OnClientClick="HideModal('changeSuccessDiv');return false;" CausesValidation="false" ButtonBackground="e8e8e8" meta:resourceKey="Close"/>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>