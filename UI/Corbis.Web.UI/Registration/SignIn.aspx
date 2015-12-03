<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="SignIn.aspx.cs" Inherits="Corbis.Web.UI.Registration.SignIn" MasterPageFile="~/MasterPages/SignInModal.Master" Title="<%$ Resources: windowTitle %>" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<asp:Content ID="signInContent" ContentPlaceHolderID="mainContent" runat="server">
<style type="text/css">
* { overflow:hidden; }    /* Fix bug no. 15906 - forgotten password modal layout: remove scrollbars onload */
 
</style>
 <!-- if less than or equal to IE7: http://msdn.microsoft.com/en-us/library/ms537512(VS.85).aspx -->
 <!--[if lte IE 7]> 
     <style type="text/css">
    * {overflow:visible;}       /* Remove scrollbars onload. */
     </style>
 <![endif]-->

    <div id="SignInContent" class="SignInContent" runat="server">
        <div id="MustSignIn">
            <h2><Corbis:Localize ID="targetPageTitle" runat="server"></Corbis:Localize></h2>
            <Corbis:Localize ID="targetWarning" runat="server"></Corbis:Localize>
        </div>
       <%-- <h2><Corbis:Localize ID="pageTitle" runat="server" meta:resourcekey="pageTitle" /></h2>--%>
        <div id="titleWrapper" class="signInTitleWrapper">
            <Corbis:Localize ID="pageTitle" runat="server" meta:resourcekey="pageTitle" />
            <div class="signInCloseButton" id="signInCloseButton" onclick="SecureModalPopupExit();">
                <img alt="Close Modal Popup" onclick="" class="Close" src="/Images/iconClose.gif" />
            </div>
        </div>

        <div id="signInForm">
            <Corbis:ValidationHub ID="validationHub" ContainerID="signInForm" runat="server" SubmitByAjax="false" SubmitForm="true" ResizeScript="ResizeSignInModal()" />
            <div id="userNameWrapper" class="userNameWrapper">
                <div id="UsernameField" class="UsernameField">
                    <Corbis:TextBox ID="username" CssClass="username" runat="server"></Corbis:TextBox>
                </div>
                <div id="UsernameLabel" style="text-align:right; width:152px;">
                    <Corbis:Localize ID="usernameLabel" runat="server" meta:resourcekey="usernameLabel" />
                </div>
            </div>
            <div id="passwordWrapper" class="passwordWrapper">
                <div id="PasswordField">
                    <Corbis:TextBox ID="password" runat="server" TextMode="Password"></Corbis:TextBox><br />
                </div>
                <div id="PasswordLabel">
                    <Corbis:Localize ID="passwordLabel" runat="server" meta:resourcekey="passwordLabel" />
                </div>
            </div>
            <Corbis:LinkButton ID="lb" runat="server" OnClick="Validate_Click" CssClass="ValidateClickLB displayNone" />
        </div>
        <div id="forgotWrapper">

            <div id="SubmitButton">
                <Corbis:GlassButton ID="validate" runat="server" meta:resourcekey="validate"></Corbis:GlassButton>
            </div>
            <div id="RegisterButton">
                <Corbis:GlassButton ID="register" ButtonStyle="Outline" CausesValidation="false" runat="server" meta:resourcekey="register"></Corbis:GlassButton>
            </div>
             <div id="ForgotPasswordLink">
                <a title="https://secure.corbis.com" style="cursor:default;border-width: 0px;"><img alt="https://secure.corbis.com" style="border-width: 0px;" src="../Images/secure.gif"/></a>
                 <Corbis:HyperLink ID="forgotPassword" CssClass="forgotPassword" runat="server" meta:resourcekey="forgotPassword" />
            </div>
       </div>

        <div id="WhyRegisterWrapper" runat="server" class="WhyRegisterWrapper" visible="false">
            <div id="regWrapper" class="regWrapper">
                <Corbis:Localize ID="whyRegisterHeading" runat="server" meta:resourcekey="whyRegisterHeading" />
            </div>
            <div id="itemWrapper">
                <ul>
                    <Corbis:Localize ID="whyRegister" runat="server" meta:resourcekey="whyRegister" />
                    <li><Corbis:Localize ID="whyRegisterItem1" runat="server" meta:resourcekey="whyRegisterItem1" /></li>
                    <li><Corbis:Localize ID="whyRegisterItem2" runat="server" meta:resourcekey="whyRegisterItem2" /></li>
                    <li><Corbis:Localize ID="whyRegisterItem3" runat="server" meta:resourcekey="whyRegisterItem3" /></li>
                </ul>
            </div>
        </div>
        <div id="trustWrapper" class="hdn">
            <div id="TrusteGraphic">
                <Corbis:HyperLink ID="truste" Localize="true" ImageUrl="/Images/truste.gif" Target="_blank" runat="server" />
            </div>
        </div>
        <div class="clr"></div>
    </div>
    <script language="javascript" type="text/javascript">
        function SecureModalPopupExit() {
            
            var iFrames = $$('iframe');
            var iDo = iFrames[0];
            iDo.src = ParentProtocol + "/Common/IFrameTunnel.aspx?windowid=secureSignIn&action=close";
        }
        function SecureModalPopupExitAndRedirectToRegister() {
            var iFrames = $$('iframe');
            var iDo = iFrames[0];
            var sr = ParentProtocol + "/Common/IFrameTunnel.aspx?windowid=secureSignIn&action=execute&actionArg=redirectToRegister()";
            iDo.src = sr;
        }
        
        function setErrorStyle() {
            var g_firefox = document.getElementById && !document.all;
            try {
                $('userNameWrapper').style.cssText = "background-color:#ffffcd; height:24px; width:100%;";
                $('passwordWrapper').style.cssText = "background-color:#ffffcd; height:24px; margin-top:2px; width:100%;"
                var iFrames = $$('iframe');
                var iDo = iFrames[0];
                iDo.src = ParentProtocol + "/Common/IFrameTunnel.aspx?windowid=secureSignIn&action=execute&actionArg=CorbisUI.Auth.SetErrorStyle(" + GetDocumentHeight() + ")&noclose=true";
            }
            catch (e) {
            }
        }

        window.addEvent('domready', function() {
            try {
                var txtUsername = $('<%= username.ClientID %>');
                if (txtUsername) {
                    txtUsername.focus();
                }
                // Safari causes the page to load by itself in some places.  Check for that, and any places where
                // it accidentally opens outside of the iframe
                if (parent.location == window.location) {
                    window.location = "/default.aspx";
                }
            }
            catch (e) { }
        });

        function ResizeSignInModal() {
            var iFrames = $$('iframe');
            var iDo = iFrames[0];
            iDo.src = ParentProtocol + "/Common/IFrameTunnel.aspx?windowid=secureSignIn&action=execute&actionArg=ResizeIModal('secureSignIn', " + GetDocumentHeight() + ")&noclose=true";
        }
        
        window.addEvent('load', ResizeSignInModal);
    </script>
    <iframe id="iFrameHttp" runat="server" style="display:none" src="/common/IFrameTunnel.aspx"></iframe>
</asp:Content>
