<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="SignInStatus.ascx.cs" Inherits="Corbis.Web.UI.Authentication.SignInStatus" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<div>
    <div class="Title">
            <Corbis:Localize ID="name" runat="server" />
        <div onclick="CorbisUI.Auth.Check();return false;">
            <Corbis:HyperLink ID="signIn" runat="server" meta:resourcekey="signIn" />
        </div>
    </div>
    <div class="Links">
        <Corbis:Localize ID="signInOrRegister" runat="server" />
        <Corbis:Localize ID="register" runat="server" meta:resourcekey="register" />
        <Corbis:Hyperlink ID="signOut" runat="server" meta:resourcekey="signOut" />
     </div>
</div>
