<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetPasswordQuestion.aspx.cs" Inherits="Corbis.Web.UI.Registration.SetPasswordQuestion"  MasterPageFile="~/MasterPages/ModalPopup.Master"%>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<asp:Content ID="changePasswordContent" ContentPlaceHolderID="mainContent" runat="server">
<div id="changePasswordContentDiv" runat="server">
<div class="titleWrapper">
    <div class="ModalTitleBar">
        <div class="samCloseButton" id="ambiguousCloseButton">
        <img class="Close" onclick="doClose();return false;" style="border-width: 0px;" src="../Images/iconClose.gif"/>
        </div>
        
        <div class="floatLeft"><Corbis:Label ID="ModalTitle" CssClass="Title" runat="server" meta:resourcekey="ModalTitle" /></div>
        
    </div>
</div>

<div class="ModalPopupContent" id="ModalPopupContent">
    <div class="ModalDescriptionText">
        <Corbis:Localize ID="ChangePasswordDescription" runat="server" meta:resourcekey="ChangePasswordDescription" />
    </div>
    <Corbis:ValidationHub runat="server" ID="vhub" IsPopup="true" IsIModal="true" ResizeScript="resizeChangePassword()" ContainerID="ModalPopupContent" SubmitForm="true" />

    <table cellpadding="1" cellspacing="0" class="pop350">

    <tr id="Tr1" class="FormRow" runat="server">
        <td class="FormLeft">
            <strong><Corbis:Label ID="questionsLabel" runat="server" meta:resourcekey="questionsLabel" /></strong>
        </td>
        <td class="FormRight">
            <Corbis:DropdownList ID="questions" runat="server" />
        </td>
    </tr>
    <tr class="FormRow">
        <td class="FormLeft">
            <strong><Corbis:Label ID="answerLabel" runat="server" meta:resourcekey="answerLabel"/></strong>
        </td>
        <td class="FormRight">
            <Corbis:Textbox validate="required;" meta:resourcekey="answer"
                ID="answer" runat="server" />
        </td>
    </tr>

    
    </table>
    <div class="ButtonRow350Profile">
        <div class="secureImage"><a title="https://secure.corbis.com" style="cursor:default;border-width: 0px;"><img alt="https://secure.corbis.com" style="border-width: 0px;" src="../Images/secure.gif"/></a></div>
        <Corbis:GlassButton ID="save"
            OnClick="save_Click"
            meta:resourceKey="Submit" runat="server"
            validate="submit"
        />
        <asp:LinkButton ID="doIt" CssClass="ValidateClickLB displayNone" OnClick="save_Click" runat="server" />
    </div>
</div>
</div>
<div class="clr"></div>

<script>
    function setErrorText(responseText) {
        errorTarget = $('ModalPopupContent').getElement('div.ValidationSummary');
        errorTarget.removeClass('displayNone');
        errorTarget.getElement('ul').set('html', '<li elId=wsdl>' + responseText + '</li>');
        resizeChangePassword();
    }
    
    function doClose() {
        var iFrames = $$('iframe');
        var iDo = iFrames[0];
        iDo.src = ParentProtocol + "/Common/IFrameTunnel.aspx?windowid=secureSignIn&action=execute&actionArg=void(0)";
    }
    
    function resizeChangePassword() {
        var iFrames = $$('iframe');
        var iDo = iFrames[0];
        iDo.src = ParentProtocol + "/Common/IFrameTunnel.aspx?windowid=secureSignIn&action=execute&actionArg=ResizeIModal('secureSignIn', " + GetDocumentHeight()  + ")&noclose=true";
    }


</script>
<iframe id="iFrameHttp" runat="server" style="display:none" src="/common/IFrameTunnel.aspx"></iframe>
</asp:Content>