<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyAccount.aspx.cs" Inherits="Corbis.Web.UI.Accounts.MyAccount" MasterPageFile="~/MasterPages/AccountsMaster.Master" Title="<%$ Resources: windowTitle %>" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<asp:Content ID="myAccountContent" ContentPlaceHolderID="accountsContent" runat="server">
    <div>
        <h2><Corbis:Localize ID="pageTitle" runat="server" meta:resourcekey="pageTitle" /></h2>
        Messaging<br />
        Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna
        aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea
        commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu
        feugiat nulla.<p />
        Notifications
    </div>
</asp:Content>