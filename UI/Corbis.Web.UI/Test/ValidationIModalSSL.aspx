<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/ModalPopup.Master" CodeBehind="ValidationIModalSSL.aspx.cs" Inherits="Corbis.Web.UI.Test.ValidationIModalSSL" %>
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
        <div class="floatLeft">Sample IModal with SSL and Ajax</div>
    </div>
</div>

<div class="ModalPopupContent" id="ModalPopupContent">
    <div class="ModalDescriptionText">
        Be sure to look at the codebehind on this one and include that on your implementation. Validation is tested first on the client to be digits only, then the wsdl will return success only if it is 2-4 chars in length. 
    </div>
    <Corbis:ValidationHub 
        runat="server" ID="vhub" ResizeScript="resizeThisModal()" PopupID="validationIModalSSL" 
        SuccessScript="setIFrameTunnelSource('execute&actionArg=parentSuccess()')" 
        SubmitByAjax="true" UseOldAjaxBehavior="true" IsIModal="true"
        AjaxUrl="/Test/ValidationTestService.asmx/ValidateLength" OnAjaxRequest="setAjaxData" 
    />

    <table cellpadding="1" cellspacing="0" class="pop350">
    <tr class="FormRow">
        <td class="FormLeft">
            <span><strong>Digit only</strong></span>
        </td>
        <td class="FormRight">
            <Corbis:Textbox 
                validate="digit; required" 
                digit_message="(client) only digits here!"
                ID="textbox1" runat="server" 
            />
        </td>
    </tr>
    <tr class="FormRow">
        <td class="FormLeft">
            <span><strong>I am required</strong></span>
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
            validate="submit"
        />
    </div>
    <div class="clr"></div>
</div>

    
<script>
    

    function setAjaxData()
    {
        <%=vhub.ClientInstanceVariableName %>.options.ajaxData = {
                'param1': $('<%=textbox1.ClientID %>').value,
                'param2': $('<%=textbox2.ClientID %>').value
            };
    }
    
    function doClose() {
        setIFrameTunnelSource('close');
    }
    
    function resizeThisModal() {
        setIFrameTunnelSource("execute&actionArg=ResizeIModal('validationIModalSSL'," + GetDocumentHeight() + ")&noclose=true");
    }

    function setIFrameTunnelSource(src)
    {
        var iFrames = $$('iframe');
        var iDo = iFrames[0];
        iDo.src = ParentProtocol +
            "/Common/IFrameTunnel.aspx?windowid=validationIModalSSL&action="
            + src;
    }
    
    
</script>
<iframe id="iFrameHttp" runat="server" style="display:none" src="/common/IFrameTunnel.aspx"></iframe>
</asp:Content>