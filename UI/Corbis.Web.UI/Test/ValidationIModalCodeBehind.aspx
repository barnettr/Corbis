<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/ModalPopup.Master" CodeBehind="ValidationIModalCodeBehind.aspx.cs" Inherits="Corbis.Web.UI.Test.ValidationIModalCodeBehind" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<asp:Content ID="sampleContent" ContentPlaceHolderID="mainContent" runat="server">
<style>
body{overflow:hidden;}
</style>
<div class="titleWrapper">
    <div class="ModalTitleBar">
        <div class="samCloseButton" id="ambiguousCloseButton">
        <input class="Close" type="image" onclick="javascript:doClose();return false;" style="border-width: 0px;" src="../Images/iconClose.gif"/>
        </div>
        <div class="floatLeft">Sample IModal with CodeBehind validation</div>
    </div>
</div>

<div class="ModalPopupContent" id="ModalPopupContent">
    <div class="ModalDescriptionText">
        This is tested first for a string length (3-7) on the client, then the view will return success only if it 'happy'/'amused' respectively. This is just to show how validation can be different on the client and the server.
    </div>
    <Corbis:ValidationHub 
        ID="vhub" runat="server" ContainerID="ModalPopupContent"
        IsIModal="true" PopupID="validationIModalCodeBehind" SubmitForm="true"
    />
    <table cellpadding="1" cellspacing="0" class="pop350">
    <tr class="FormRow" runat="server">
        <td class="FormLeft">
            <span><strong>Label 1</strong></span>
        </td>
        <td class="FormRight">
            <Corbis:Textbox 
                validate="length[3, 7];required" 
                length_str_message="(client) enter a string between 3 and 7 chars long"
                ID="textbox1" runat="server" meta:resourcekey="textbox1"
            />
        </td>
    </tr>
    <tr class="FormRow" runat="server">
        <td class="FormLeft">
            <span><strong>Label 2</strong></span>
        </td>
        <td class="FormRight">
            <Corbis:Textbox 
                validate="required" 
                required_message="textbox 2 is required"
                ID="textbox2" runat="server" 
            />
        </td>
    </tr>
    
    </table>
    <div class="ButtonRow350">
        <Corbis:GlassButton ID="SaveButton"
            Text="Save" runat="server"
            validate="submit" OnClientClick="return false;"
        />
        <asp:LinkButton ID="lb" CssClass="ValidateClickLB displayNone" runat="server" OnClick="lb_Click" />
    </div>
    <div class="clr"></div>
</div>
<script>
    
    function doClose() {
        parent.MochaUI.CloseModal('validationIModalCodeBehind');
    }
    
    
    function setAjaxData()
    {
        <%=vhub.ClientInstanceVariableName %>.options.ajaxData = {
                'param1': $('<%=textbox1.ClientID %>').value,
                'param2': $('<%=textbox2.ClientID %>').value
                };
    }
    
    
</script>


</asp:Content>