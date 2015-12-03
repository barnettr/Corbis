<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePasswordModal.aspx.cs" Inherits="Corbis.Web.UI.Accounts.ChangePasswordModal" MasterPageFile="~/MasterPages/ModalPopup.Master" Title="<%$ Resources: windowTitle %>" EnableTheming="false" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<asp:Content ID="changePasswordContent" ContentPlaceHolderID="ModalPopupContentPlace" runat="server" >
    <div class="ModalPopupPanel">
    <div id="whileChanging" runat="server">
        <div id="errorMessaging">
            <div><asp:RequiredFieldValidator id="oldPasswordRequired" runat="server" ControlToValidate="oldPassword" ForeColor=" " CssClass="Error" Display="Dynamic" meta:resourcekey="oldPasswordRequired" /></div>
            <div><asp:CustomValidator ID="oldPasswordInvalid" runat="server" ForeColor=" " CssClass="Error" Display="Dynamic" meta:resourcekey="oldPasswordInvalid" ValidateEmptyText="false" /></div>
            <div><asp:RequiredFieldValidator id="newPasswordRequired" runat="server" ControlToValidate="newPassword" ForeColor=" " CssClass="Error" Display="Dynamic" meta:resourcekey="newPasswordRequired" /></div>
            <div><asp:RegularExpressionValidator ID="newPasswordFormat" runat="server" ControlToValidate="newPassword" ForeColor=" " CssClass="Error" Display="Dynamic" meta:resourcekey="newPasswordFormat" ValidationExpression="<%$ Resources: Resource, Regex_Password %>" /></div>
            <div><asp:RequiredFieldValidator ID="confirmNewPasswordRequired" runat="server" ControlToValidate="confirmNewPassword" ForeColor=" " CssClass="Error" Display="Dynamic" meta:resourcekey="confirmNewPasswordRequired" /> </div>
            <div><asp:CompareValidator ID="passwordMatchValidator" runat="server" ControlToValidate="confirmNewPassword" ForeColor=" " CssClass="Error" Display="Dynamic" meta:resourcekey="passwordMatchValidator" ControlToCompare="newPassword" /></div>
        </div>
        <div class="FormRow">
        <div class="FormLeft">
            <Corbis:Localize ID="oldPasswordLabel" runat="server" meta:resourcekey="oldPasswordLabel" />
        </div>
        <div class="FormRight">
            <Corbis:Textbox ID="oldPassword" runat="server" TextMode="Password"></Corbis:Textbox>
        </div>
        </div>
        <div class="FormRow">
        <div class="FormLeft">
            <Corbis:Localize ID="newPasswordLabel" runat="server" meta:resourcekey="newPasswordLabel"/>
            <br />
            <Corbis:Localize ID="newPasswordTip" runat="server" meta:resourcekey="newPasswordTip" />
        </div>
        <div class="FormRight">
            <Corbis:Textbox ID="newPassword" runat="server" TextMode="Password"></Corbis:Textbox>
        </div>
        </div>
        <div class="FormRow">
        <div class="FormLeft">
            <Corbis:Localize ID="confirmNewPasswordLabel" runat="server" meta:resourcekey="confirmNewPasswordLabel" />
        </div>
        <div class="FormRight">
            <Corbis:Textbox ID="confirmNewPassword" AllowPaste="false" runat="server" TextMode="Password"></Corbis:Textbox>
        </div>
        </div>
        <div class="FormButtons">
            <Corbis:Button ID="save" runat="server" meta:resourcekey="save" />
            <Corbis:Button ID="cancel" runat="server" meta:resourcekey="cancel" OnClientClick="ModalPopupExit('cancelChangePassword');return false;" />
        </div>
    </div>
    <div id="changesuccess" runat="server" visible="false">
        <div align="center">
            <Corbis:Localize ID="changePasswordSuccessLabel" meta:resourcekey="changePasswordSuccessLabel" runat="server" />
        </div>
        <div align="center">
            <Corbis:Button ID="close" runat="server" meta:resourcekey="close" OnClientClick="ModalPopupExit('cancelChangePassword');return false;" />
        </div>
    </div>
    </div>
</asp:Content>