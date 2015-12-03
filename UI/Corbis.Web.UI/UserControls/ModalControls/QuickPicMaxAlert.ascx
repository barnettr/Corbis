<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuickPicMaxAlert.ascx.cs" Inherits="Corbis.Web.UI.UserControls.ModalControls.QuickPicMaxAlert" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<Corbis:ModalPopup ID="quickpicMaximumAlertModal" meta:resourcekey="quickpicMaximumAlert" runat="server" ContainerID="quickpicMaximumAlert" Width="310">
    <Corbis:GlassButton ID="closeButtonQuickpicMaximumAlert" meta:resourcekey="closeButton" OnClientClick="HideModal('quickpicMaximumAlert');return false;" CausesValidation="false" runat="server" />
</Corbis:ModalPopup>
