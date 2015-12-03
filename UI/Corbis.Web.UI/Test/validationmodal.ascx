<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="validationmodal.ascx.cs" Inherits="Corbis.Web.UI.Test.validationmodal" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<div class="ModalDescriptionText">
    This is a form
</div>
<Corbis:ValidationHub 
    ID="vhub" runat="server" ContainerID="validationModalContainer"
    IsIModal="false" PopupID="validationModalContainer" UniqueName="Modal"
    SuccessScript="alert('success!');HideModal('validationModalContainer');"
/>
<table cellpadding="1" cellspacing="0" class="pop350">
<tr class="FormRow">
    <td class="FormLeft">
        <span><strong>Required text field</strong></span>
    </td>
    <td class="FormRight">
        <Corbis:TextBox ID="req" runat="server" validate="required" required_message="Text field is required"/>
    </td>
</tr>
<tr class="FormRow">
    <td class="FormLeft">
        <span><strong>email (optional)</strong></span>
    </td>
    <td class="FormRight">
        <Corbis:TextBox ID="email" runat="server" validate="email" email_message="please use a proper email address"/>
    </td>
</tr>

<tr class="FormRow">
    <td class="FormLeft">
        <span><strong>phone (required)</strong></span>
    </td>
    <td class="FormRight">
        <Corbis:TextBox 
            ID="phone" runat="server" 
            validate="required;phone" 
            required_message="enter a valid phone number"
            phone_message="enter a valid phone number"
        />
    </td>
</tr>

<tr class="FormRow">
    <td class="FormLeft">
        <span><strong>required custom (type "i feel validated")</strong></span>
    </td>
    <td class="FormRight">
        <Corbis:TextBox 
            ID="custom1" runat="server" validate="required;custom1" 
            custom1="myCustomFunction()"
            required_message="custom field is required"
            custom1_message="not valid until you type 'i feel validated'"
        />
    </td>
</tr>

<tr class="FormRow">
    <td class="FormLeft">
        <span><strong>required custom 2 (consolidated error string - type 'valid')</strong></span>
    </td>
    <td class="FormRight">
        <Corbis:TextBox 
            ID="custom2" runat="server" validate="required;custom1" 
            required_message="custom2 has the same message twice"
            custom1_message="custom2 has the same message twice"
            custom1="myOtherCustomFunction()"
        />
    </td>
</tr>

<tr class="FormRow">
    <td class="FormLeft">
        <span><strong>required checkbox</strong></span>
    </td>
    <td class="FormRight">
        <Corbis:ImageCheckbox 
            ID="chk" runat="server" Text="I require checking" 
            validate="true" 
            checkbox_message="Please check the checkbox"
        />
    </td>
</tr>
<tr class="FormRow">
    <td class="FormLeft">
        <span><strong>required dropdown</strong></span>
    </td>
    <td class="FormRight">
        <Corbis:DropDownList ID="dropdown" runat="server" validate="select" select_message="please choose from the dropdown">
            <asp:ListItem>[make a selection]</asp:ListItem>
            <asp:ListItem>choice 1</asp:ListItem>
            <asp:ListItem>choice 2</asp:ListItem>
        </Corbis:DropDownList>
    </td>
</tr>

</table>
<div class="ButtonRow350">
    
    <Corbis:GlassButton ID="close" ButtonStyle="Gray" Text="Close" OnClientClick="HideModal('validationModalContainer');return false;" runat="server" />
    <Corbis:GlassButton Text="Validate!" validate="submit" runat="server" />
</div>

<script>
    function myCustomFunction(){
        return $('<%=custom1.ClientID %>').value == 'i feel validated';
    }
    function myOtherCustomFunction(){
        return $('<%=custom2.ClientID %>').value == 'valid';
    }
</script>
