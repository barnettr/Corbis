<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequestImageResearch.aspx.cs" Inherits="Corbis.Web.UI.CustomerService.RequestImageResearch" MasterPageFile="~/MasterPages/ModalPopup.Master" Title="<%$ Resources: windowTitle %>" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="requestImageResearchContent" ContentPlaceHolderID="mainContent" runat="server">
<style>
body { background-color: #e8e8e8; }
html {overflow:hidden !important;}
</style>
<div id="titleWrapper" class="titleWrapper">
    <asp:Localize ID="pageTitle" runat="server" meta:resourcekey="pageTitle" />
    <div class="samCloseButton" id="ambiguousCloseButton">
        <Corbis:Image ID="requestImageResearchModalPopupImage" ImageUrl="/Images/iconClose.gif" runat="server" onclick="parent.CorbisUI.Legal.HideRequestImageResearchModal();return false;" class="Close" meta:resourcekey="requestImageResearchModalPopupImage"/>
    </div>
</div>
<div class="ModalPopupContent RIRWrapper" id="ModalPopupContent">
    <div class="ModalDescriptionText">
        <asp:Localize ID="agreement" runat="server" meta:resourcekey="agreement" />
    </div>
    <hr />
    <Corbis:ValidationHub 
        ID="vhub" runat="server" IsIModal="true"
        PopupID="RequestImageResearchModal" SubmitForm="true"
        ResizeScript="setTimeout(resizeThisModal,100)"
    />
    
    <table cellpadding="1" cellspacing="0" class="pop500">
    <tr class="FormRow">
        <td colspan="2">
            <div class="ModalDescriptionText">
                <strong><asp:Localize runat="server" meta:resourcekey="imageDescriptionLabel" /></strong>
            </div>
            <Corbis:TextBox 
                ID="imageDescription" runat="server" 
                CssClass="RIR_howTo_input" validate="required"
                meta:resourcekey="imageDescription"
                TextMode="multiline"  rows="3"
            />
        </td>
    </tr>
    <tr class="FormRow">
        <td class="FormLeft">
            <Corbis:Label ID="projectDeadlineLabel" runat="server" meta:resourcekey="projectDeadlineLabel"/>
        </td>
        <td class="FormRight">
              <Corbis:TextBox ID="projectDeadline" runat="server" validate="required" meta:resourcekey="projectDeadline" />
        </td>
    </tr>
    <tr class="FormRow">
        <td class="FormLeft">
            <Corbis:Label ID="modelReleaseLabel" runat="server" meta:resourcekey="modelReleaseLabel"/>
        </td>
        <td class="FormRight">
             <Corbis:DropDownList ID="modelRelease" runat="server" validate="select" meta:resourcekey="modelRelease" />
       </td>
    </tr><tr class="FormRow">
        <td class="FormLeft">
            <Corbis:Label ID="projectClientLabel" runat="server" meta:resourcekey="projectClientLabel"/>
        </td>
        <td class="FormRight">
             <Corbis:TextBox ID="projectClient" runat="server" validate="required" meta:resourcekey="projectClient"/>
        </td>
    </tr><tr class="FormRow">
        <td class="FormLeft">
            <Corbis:Label ID="jobNumberLabel" runat="server" meta:resourcekey="jobNumberLabel"/>
        </td>
        <td class="FormRight">
             <Corbis:TextBox ID="jobNumber" runat="server" validate="required" meta:resourcekey="jobNumber"/>
        </td>
    </tr><tr class="FormRow">
        <td class="FormLeft">
            <Corbis:Label ID="natureOfBusinessLabel" runat="server" meta:resourcekey="natureOfBusinessLabel"/>
        </td>
        <td class="FormRight">
            <Corbis:DropDownList ID="natureOfBusiness" runat="server" validate="select"  meta:resourcekey="natureOfBusiness"/>
        </td>
    </tr><tr class="FormRow">
         <td class="FormLeft">
            <Corbis:Label ID="otherDescriptionLabel" runat="server" meta:resourcekey="otherDescriptionLabel"/>
        </td>
        <td class="FormRight" id="otherCell">
             <Corbis:TextBox ID="otherDescription" runat="server" />
             <AJAXToolkit:TextBoxWatermarkExtender ID="otherDescriptionWaterMark" runat="server" TargetControlID="otherDescription" WatermarkText="<%$ Resources: otherDescOptional %>" WatermarkCssClass="Optional" />
        </td>
    </tr>
    </table>
    <div class="ButtonRow500">

    <%-- send/cancel buttons --%>
        <Corbis:GlassButton 
            ID="sendButton" Text="Send" runat="server" meta:resourcekey="send" OnClick="Submit_Click" validate="submit" 
        />&nbsp;
        <Corbis:GlassButton  ID="CancelModal" runat="server" ButtonBackground="e8e8e8"
            OnClientClick="doClose();return false;"                        
            meta:resourcekey="cancel" ButtonStyle="Gray"
        />&nbsp;&nbsp;&nbsp;
        <asp:LinkButton ID="realSubmitButton" runat="server" OnClick="Submit_Click" CssClass="ValidateClickLB displayNone" />
    </div>         
</div>

<script language="javascript" type="text/javascript">
    function doClose() {
        parent.MochaUI.CloseModal('RequestImageResearchModal');
    }
    
    function doSuccess() {
        parent.OpenModal('success');
        doClose();
    }
    
    function resizeThisModal() {
        parent.ResizeIModal('RequestImageResearchModal', GetDocumentHeight());
        try {
            if (<%=vhub.ClientInstanceVariableName %>) {
                $('<%=otherDescription.ClientID%>').focus();
                $('<%=otherDescription.ClientID%>').blur();
            }
        } 
        catch (er) { }
    }
</script>


</asp:Content>
