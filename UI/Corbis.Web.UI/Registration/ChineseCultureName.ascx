<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChineseCultureName.ascx.cs" Inherits="Corbis.Web.UI.Registration.ChineseCultureName" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<table class="container">
 <tr class="FormRow" runat="server">
    <td class="FormLeft">
     <Corbis:Localize ID="chineseLastNameLabel" runat="server" Text="<%$ Resources:Accounts, LastNameLabel %>" />
    </td>
    <td class="FormRight">
     <Corbis:TextBox class="item" validate="required" required_message="<%$ Resources:Accounts, MemberValidationError_LastNameRequired %>" ID="lastName" runat="server"/>
    </td>
  </tr>
  
 <tr class="FormRow" runat="server">
    <td class="FormLeft">
     <Corbis:Localize ID="chineseFirstNameLabel" runat="server" Text="<%$ Resources:Accounts, FirstNameLabel %>" />
    </td>
    <td class="FormRight">
     <Corbis:TextBox  class="item" validate="required" required_message="<%$ Resources:Accounts, MemberValidationError_FirstNameRequired %>" ID="firstName" runat="server"/>
    </td>
  </tr>
  
  
  
</table>