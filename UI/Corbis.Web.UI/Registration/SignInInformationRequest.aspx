<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignInInformationRequest.aspx.cs" Inherits="Corbis.Web.UI.Registration.SignInInformationRequest" MasterPageFile="~/MasterPages/SignInModal.Master" Title="<%$ Resources: windowTitle %>" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<asp:Content ID="loginInformationRequestContent" ContentPlaceHolderID="mainContent" runat="server">
<style type="text/css">
* { overflow:hidden; }    /* Fix bug no. 15906 - forgotten password modal layout */
       
</style>
<div id="forgotPasswordModalContent">
<div class="titleWrapper">
    <div class="ModalTitleBar">
        <div class="samCloseButton" id="ambiguousCloseButton">
        <input class="Close" type="image" onclick="javascript:SecureModalPopupExit();return false;" style="border-width: 0px;" src="../Images/iconClose.gif"/>
        </div>
        <div class="floatLeft" id="TitleDiv"><Corbis:Label ID="ModalTitle" CssClass="Title" runat="server" meta:resourcekey="ModalTitle" /></div>
    </div>
</div>
<div class="ModalPopupContent" id="ModalPopupContent">
    <div class="ModalDescriptionText" id="emailDiv">
        <Corbis:Localize  runat="server" meta:resourcekey="IntroText1" />
    </div>
    <div class="ModalDescriptionText displayNone" id="securityQuestionDiv">
        <Corbis:Localize runat="server" meta:resourcekey="IntroText2" />
    </div>
    <div class="ModalDescriptionText displayNone" id="contactUsDiv">
        <br />
        <Corbis:Localize ID="ContactUsLabel" runat="server" />
    </div>
    <div class="ModalDescriptionText displayNone" id="successDiv">
        <br />
        <Corbis:Localize runat="server" meta:resourcekey="SuccessLabel" />
    </div>
    <div ID="ErrorSummaryPanel" class="ValidationSummary displayNone">
        <ul>
            
        </ul>
    </div>
    <table cellpadding="1" cellspacing="0" class="pop350" border="1">
    <tr class="FormRow" id="step1Row">
        <td class="FormLeft">
            <strong><Corbis:Label ID="emailLabel" runat="server" meta:resourcekey="emailLabel" /></strong>
        </td>
        <td class="FormRight">
            <Corbis:Textbox validate="custom1" custom1="validateEmail()" meta:resourcekey="email" ID="email" runat="server" />
        </td>
    </tr>
    <tr class="FormRow displayNone" id="step2Row">
        <td class="FormLeft">
            <strong id="securityQuestion"><Corbis:Label runat="server" meta:resourcekey="PetQuestionLabel" /></strong>
        </td>
        <td class="FormRight">
            <Corbis:Textbox validate="custom1" custom1="validateAnswer()" meta:resourcekey="answer" ID="answer" runat="server" />
        </td>
    </tr>
    </table>
    <div class="ButtonRow350 infoRequestFooter">
             <Corbis:GlassButton 
                        OnClientClick="SecureModalPopupExit();return false;" ID="CloseButton"
                        meta:resourceKey="CloseButton" runat="server" CssClass="displayNone" />
             <Corbis:GlassButton  ID="sendSignIn" meta:resourceKey="sendSignInButton" CssClass="submitButton" runat="server" validate="submit"/>&nbsp;    
              <Corbis:GlassButton
                ButtonStyle="Gray" ID="CancelButton"  CssClass="cancelButton"
                OnClientClick="SecureModalPopupExit();return false;" 
                meta:resourceKey="CancelButton" runat="server"
            />
         <div class="PrivacyPolicy" >
            <a title="https://secure.corbis.com" style="cursor:default;border-width: 0px;"><img alt="https://secure.corbis.com" style="border-width: 0px;" src="../Images/secure.gif"/></a>
            <Corbis:HyperLink ID="PrivacyPolicy" runat="server" meta:resourcekey="PrivacyPolicy" NavigateUrl="javascript:doPrivacyPolicy();" />
        </div>
    </div>
</div>
</div>
<script>
    window.addEvent('load', init);
    var _isEnterKey = false;
    function init() {
        resizeForgotPwd();
        
        
    }
    
    var _isStep1 = true;
    var _attempts = 0;
    function validateAnswer() {
        return _isStep1 || $('<%=answer.ClientID%>').value.length > 0;
    }
    function validateEmail() {
        return !_isStep1 || $('<%=email.ClientID%>').value.length > 0;
    }
    function resizeForgotPwd() {
        try {
            var iFrames = $$('iframe');
            var iDo = iFrames[0];
            iDo.src = ParentProtocol + "/Common/IFrameTunnel.aspx?windowid=secureSignIn&action=execute&actionArg=CorbisUI.Auth.SetErrorStyle(" + GetDocumentHeight() + ")&noclose=true";
        } catch (e) { }
    }

    function doPrivacyPolicy() {
        var iFrames = $$('iframe');
        var iDo = iFrames[0];
        iDo.src = ParentProtocol + "/Common/IFrameTunnel.aspx?windowid=secureSignIn&action=execute&actionArg=CorbisUI.Legal.OpenPolicyIModal()&noclose=true"
    }
    function SecureModalPopupExit() {
        if (_isEnterKey) {
            _isEnterKey = true;
            return;
        }
        else {
            var iFrames = $$('iframe');
            var iDo = iFrames[0];
            iDo.src = ParentProtocol + "/Common/IFrameTunnel.aspx?windowid=secureSignIn&action=close";
        }
    }
    function showStep2(q) {
        
        $('step1Row').addClass('displayNone');
        $('step2Row').removeClass('displayNone');
        $('emailDiv').addClass('displayNone');
        $('securityQuestion').set('html', q);
        $('securityQuestionDiv').removeClass('displayNone');
        $('ModalPopupContent').getElement('div.ValidationSummary').addClass('displayNone');
        _isStep1 = false;
        $('<%=answer.ClientID %>').focus();
    }

    function hideSteps() {
        $('step1Row').addClass('displayNone');
        $('step2Row').addClass('displayNone');
        $('emailDiv').addClass('displayNone');
        $('securityQuestionDiv').addClass('displayNone');
        $('<%=sendSignIn.ClientID %>').addClass('displayNone');
        $('<%=CancelButton.ClientID %>').addClass('displayNone');
        $('<%=CloseButton.ClientID %>').removeClass('displayNone');
        $('ModalPopupContent').getElement('div.ValidationSummary').addClass('displayNone');
    }
    
    function doFail() {
        hideSteps();
        $('contactUsDiv').removeClass('displayNone');
    }

    function doSuccess() {
        hideSteps();

        $('TitleDiv').set('html', "<Corbis:Localize runat='server' meta:resourcekey='SuccessTitle' />");
        $('successDiv').removeClass('displayNone')
            .set('html', String.format($('successDiv').get('html'), $('<%=email.ClientID %>').value));
    }
    var _formValidation = new CorbisFormValidator('aspnetForm', {
        resizeScript: "resizeForgotPwd()",
        tipsClass: 'displayNone',
        containerID: 'ModalPopupContent',
        fieldErrorClass: 'Error',
        submitByAjax: true,
        ajaxUrl: '/Registration/ForgotPassword.asmx/GetSecurityQuestion',
        ajaxAsync: false,
        onAjaxRequest: function() {
            _formValidation.options.ajaxData = {
                'email': $('<%=email.ClientID %>').value,
                'answer': $('<%=answer.ClientID %>').value
            };
        } .bind(this),
        onAjaxFailure: function(instance) {
            //alert('call failed');
        },
        onAjaxSuccess: function(response) {
            if (response && response.length > 0) {
                // bug 16714, someone must be modified response from string to array.
                //var responseText = response[1].replace(/(<([^>]+)>)/ig, "");
               // var responseindex = (Browser.Engine.trident) ? 0 : 1;
                //var responseText = response[responseindex].innerHTML;
                //var responseText = $$(response)[0].innerHTML;
                var responseText = response.replace(/(<([^>]+)>)/ig, "");
                if (_isStep1) {
                    if (responseText.toLowerCase().indexOf('invalidemail') != -1 || responseText.replace(/\s/g, '') == '') {
                        var errorTarget = $('ModalPopupContent').getElement('div.ValidationSummary');
                        errorTarget.removeClass('displayNone');
                        errorTarget.getElement('ul').set('html',
                        '<li elId=wsdl>' +
                        $('<%=email.ClientID %>').getProperty('custom1_message') +
                        '</li>'
                    );
                    }
                    else if (responseText.toLowerCase().indexOf('none') != -1) {
                        doFail();
                    }
                    else {
                        showStep2('<span>' + responseText + '</span>');
                        _formValidation.options.ajaxUrl = '/Registration/ForgotPassword.asmx/SubmitAnswer';
                        _isStep1 = false;
                    }
                }
                else {
                    _attempts++;
                    if (responseText.toLowerCase().indexOf('success') != -1) {
                        doSuccess();
                    }
                    else if (_attempts >= 3) {
                        doFail();
                    }
                    else {
                        var errorTarget = $('ModalPopupContent').getElement('div.ValidationSummary');
                        errorTarget.removeClass('displayNone');
                        errorTarget.getElement('ul').set('html',
                        '<li elId=wsdl>' +
                        $('<%=answer.ClientID %>').getProperty('custom1_message') +
                        '</li>'
                    );
                    }
                }
            }
            resizeForgotPwd();
        }
    });
</script>
<iframe id="iFrameHttp" runat="server" style="display:none" src="/common/IFrameTunnel.aspx"></iframe>
</asp:Content>
