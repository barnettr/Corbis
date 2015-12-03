<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JapaneseCultureName.ascx.cs" Inherits="Corbis.Web.UI.Registration.JapaneseCultureName" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<table class="container">

<tr class="FormRow" runat="server">

    <td class="FormLeft" style="width:30%">
     <Corbis:Localize ID="japaneseLastNameLabel" runat="server" Text="<%$ Resources:Accounts, JapaneseLastNameLabel %>" />
    </td>
    <td class="FormRight" style="width:25%">
     <Corbis:TextBox  validate="required" ID="lastName" required_message="<%$ Resources:Accounts, MemberValidationError_LastNameRequired %>" runat="server"/>
    </td>
    
    <td class="FormLeft" style="width:20%">
     <Corbis:Localize ID="japaneseFirstNameLabel" runat="server" Text="<%$ Resources:Accounts, JapaneseFirstNameLabel %>" />
    </td>
    <td class="FormRight" style="width:25%">
     <Corbis:TextBox  validate="required" ID="firstName" required_message="<%$ Resources:Accounts, MemberValidationError_FirstNameRequired %>" runat="server"/>
    </td>
  </tr>
  
  <tr class="FormRow" runat="server">
    <td class="FormLeft" style="width:30%">
     <Corbis:Localize ID="furiganaLastNameLabel" runat="server" Text="<%$ Resources:Accounts, FuriganaLastNameLabel %>" />
    </td>
    <td class="FormRight" style="width:25%">
     <Corbis:TextBox  validate="required" ID="furiganaLastName" required_message="<%$ Resources:Accounts, MemberValidationError_FuriganaLastNameRequired %>" runat="server"/>
    </td>
    
    <td class="FormLeft" style="width:20%">
     <Corbis:Localize ID="furiganaFirstNameLabel" runat="server" Text="<%$ Resources:Accounts, FuriganaFirstNameLabel %>" />
    </td>
    <td class="FormRight" style="width:25%">
     <Corbis:TextBox  validate="required" ID="furiganaFirstName" required_message="<%$ Resources:Accounts, MemberValidationError_FuriganaFirstNameRequired %>" runat="server"/>
    </td>
  </tr>
  

</table>
