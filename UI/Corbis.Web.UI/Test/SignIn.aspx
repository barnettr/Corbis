<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="Corbis.Web.UI.Test.SignIn" MasterPageFile="~/MasterPages/ModalPopup.Master" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<asp:Content ID="signInContent" ContentPlaceHolderID="mainContent" runat="server">
    <div id="Div1">
        <div id="MustSignIn">
            <h2><Corbis:Localize ID="targetPageTitle" runat="server"></Corbis:Localize></h2>
            <Corbis:Localize ID="targetWarning" runat="server"></Corbis:Localize>
        </div>
        <h2><Corbis:Localize ID="pageTitle" runat="server" meta:resourcekey="pageTitle" /></h2>
        
        <div id="loginUnsuccessfulDiv" class="LoginUnsuccessful" runat="server" visible="false">
            <asp:Panel ID="unsuccessfulLogin" runat="server" meta:resourcekey="unsuccessfulLogin">
                    <br />
                    <div id="failureTryAgain" runat="server">
                    <Corbis:Localize id="tryAgain" runat="server"></Corbis:Localize>
                    </div>
            </asp:Panel>
        </div>

        <div id="UsernameLabel">
            <Corbis:Localize ID="usernameLabel" runat="server" meta:resourcekey="usernameLabel" />
        </div>
        <div id="UsernameField">
            <Corbis:TextBox ID="username" runat="server"></Corbis:TextBox>
            <asp:RequiredFieldValidator id="usernameRequired" runat="server" ControlToValidate="username" CssClass="Error" Display="Dynamic" meta:resourcekey="usernameRequired" /></div>
        <div id="PasswordLabel">
            <Corbis:Localize ID="passwordLabel" runat="server" meta:resourcekey="passwordLabel" />
        </div>
        <div id="PasswordField">
            <Corbis:TextBox ID="password" runat="server" TextMode="Password"></Corbis:TextBox>
            <asp:RequiredFieldValidator id="oldPasswordRequired" runat="server" ControlToValidate="password" CssClass="Error" Display="Dynamic" meta:resourcekey="oldPasswordRequired" /></div>
        <div id="SubmitButton">
            <Corbis:Button ID="validate" runat="server" meta:resourcekey="validate" />
        </div>
        <div id="ForgotPasswordLink">
            <Corbis:HyperLink ID="forgotPassword" runat="server" meta:resourcekey="forgotPassword" />
        </div>
        <div id="Or">
            <span><Corbis:Localize ID="orLabel" runat="server" meta:resourcekey="orLabel" /></span>
        </div>
        <div id="WhyRegister">
            <Corbis:Localize ID="createPrompt" runat="server" meta:resourcekey="createPrompt" />
        </div>
        <div id="RegisterButton">
            <Corbis:Button CausesValidation="false" ID="register" runat="server" meta:resourcekey="register" />
        </div>
        <div id="TrusteGraphic">
            <Corbis:HyperLink ID="truste" Localize="true" ImageUrl="/Images/en-US/eufinalmark.gif" Target="_blank" runat="server" />
        </div>
    </div>
</asp:Content>
