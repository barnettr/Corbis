<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Corbis.Web.UI.Accounts.ChangePassword" MasterPageFile="~/MasterPages/ModalPopup.Master" Title="<%$ Resources: WindowTitle %>" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<asp:Content ID="changePasswordContent" ContentPlaceHolderID="mainContent" runat="server">
<style>
body{overflow:hidden;}
</style>
<div class="titleWrapper">
    <div class="ModalTitleBar">
        <div class="samCloseButton" id="ambiguousCloseButton">
        <input class="Close" type="image" onclick="javascript:doClose();return false;" class="borderWidth" src="../Images/iconClose.gif"/>
        </div>
        
        <div class="floatLeft"><Corbis:Label ID="ModalTitle" CssClass="Title" runat="server" meta:resourcekey="ModalTitle" /></div>
        
    </div>
</div>

<Corbis:Panel ID="changePasswordPanel" DefaultButton="changePasswordSave$GlassButton" runat="server">
<div class="ModalPopupContent" id="ModalPopupContent">
    <div class="ModalDescriptionText">
        <Corbis:Localize ID="ChangePasswordDescription" runat="server" meta:resourcekey="ChangePasswordDescription" />
    </div>
    <Corbis:ValidationHub 
        IsPopup="true" IsIModal="true"
        runat="server" ID="vhub" 
        SuccessScript="parent.CorbisUI.MyProfile.doChangePasswordSuccess()" 
        SubmitByAjax="true" UseOldAjaxBehavior="true" PopupID="changePassword"
        AjaxUrl="/Accounts/UpdatePasswordScriptService.asmx/UpdatePassword" OnAjaxRequest="setAjaxData" 
    />

    <table cellpadding="1" cellspacing="0" class="pop350">
    <tr class="FormRow">
        <td class="FormLeft">
            <strong><Corbis:Label ID="usernameLabel" runat="server" meta:resourcekey="usernameLabel" /></strong>
        </td>
        <td class="FormRight">
            <span><%=this.User.Identity.Name %></span>
        </td>
    </tr>
    <tr class="FormRow">
        <td class="FormLeft">
            <strong><Corbis:Label ID="oldPasswordLabel" runat="server" meta:resourcekey="oldPasswordLabel" /></strong>
        </td>
        <td class="FormRight">
            <Corbis:Textbox MaxLength="20"
                validate="length[2, 20]; required" meta:resourcekey="oldPassword"
                ID="oldPassword" runat="server" TextMode="Password"
            />
        </td>
    </tr>
    <tr class="FormRow">
        <td class="FormLeft">
            <strong><Corbis:Label ID="newPasswordLabel" runat="server" meta:resourcekey="newPasswordLabel"/></strong>
        </td>
        <td class="FormRight">
            <Corbis:Textbox MaxLength="20" 
                validate="length[8, 20]; required; custom1" custom1="newPasswordSameOld()" meta:resourcekey="newPassword"
                ID="newPassword" runat="server" TextMode="Password" 
            />
        </td>
    </tr>
    <tr class="FormRow">
        <td class="FormLeft">
            <strong><Corbis:Label ID="confirmNewPasswordLabel" runat="server" meta:resourcekey="confirmNewPasswordLabel" /></strong>
        </td>
        <td class="FormRight">
            <Corbis:Textbox MaxLength="20" 
                validate="custom1;required" meta:resourcekey="confirmPassword" custom1="newPasswordsMatch()"
                ID="confirmNewPassword" AllowPaste="false" runat="server" TextMode="Password"
            />
        </td>
    </tr>
    
    </table>
    <div class="ButtonRow350Profile">
        <div class="secureImage"><a title="https://secure.corbis.com" class="borderWidth"><img alt="https://secure.corbis.com" class="borderWidth" src="../Images/secure.gif"/></a></div>
        <Corbis:GlassButton ID="changePasswordSave"
            OnClick="changePasswordSave_Click"
            meta:resourceKey="Submit" runat="server"
            validate="submit"
        />
    </div>
    <div class="clr"></div>
</div>
</Corbis:Panel>
<script>
    var  _re, _rt, _rh;
    function setAjaxData() {
        <%=vhub.ClientInstanceVariableName %>.options.ajaxData = {
            'oldPassword': $('<%=oldPassword.ClientID %>').value,
            'newPassword': $('<%=newPassword.ClientID %>').value
        };
    }
    
    function doClose() {
        parent.MochaUI.CloseModal('changePassword');
    }
    
    function newPasswordsMatch() {
        var pwd = $('<%=newPassword.ClientID %>').value;
        return pwd == $('<%=confirmNewPassword.ClientID %>').value;
    }
    function newPasswordSameOld() {
        var pwd = $('<%=newPassword.ClientID %>').value;
        return pwd != $('<%=oldPassword.ClientID %>').value;
    }
</script>
</asp:Content>