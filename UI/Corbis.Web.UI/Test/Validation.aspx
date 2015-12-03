<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Validation.aspx.cs" Inherits="Corbis.Web.UI.Test.Validation" MasterPageFile="~/MasterPages/NoSearchBar.Master"  %>
<%@ Register Assembly="Corbis.Web.UI.Controls" Namespace="Corbis.Web.UI.Controls" TagPrefix="Corbis" %>
<%@ Register TagPrefix="Corbis" TagName="ValidationSample" Src="~/Test/ValidationModal.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="ET" Src="~/Test/ValidationUsingErrorTracker.ascx" %>
<asp:Content ID="defaultContent" ContentPlaceHolderID="mainContent" runat="server">
<style>
div.content
{
    height:800px;
    color:#e7e7e7;
    margin-top:150px;
    padding-left:20px;
    font:trebuchet ms bold 22px;

}
</style>
<script>
    function launchValidationModal() {
        OpenModal('validationModalContainer');
        setTimeout("initModalValidation('validationModalContainer')", 100);
    }
    function launchETModal() {
        OpenModal('ETModalContainer');
        setTimeout("initETModal('ETModalContainer')", 100);
    }
    function parentSuccess() {
        alert('hello from script on containing page\n\nyou could launch a modal or something here');
    }
</script>

<div class="content">
    <div style="width:400px;background:#dedede;color:#666666; padding:10px 10px 10px 24px">
        
        <strong style="margin-left:-16px;font-size:16px">Features:</strong>
        <ul>
            <li>Automatic bind to enter key</li>
            <li>Automatic focus on first form element</li>
            <li>Default ajax behavior (displays text returned as an error unless "success" is returned)</li>
            <li>More tba...</li>
        </ul>
        <br />
        <strong style="margin-left:-16px;font-size:16px">Launching modal notes:</strong>
        <ul>
            <li>Modal Popup now has UseDefaultPadding property. Set to false.</li>
            <li>Use initValidation when opening popup.</li>
        </ul>
        <br />
        <strong style="margin-left:-16px;font-size:16px">Within the modal user control:</strong>
        <ul>
            <li>Include an error summary panel ID=ErrorSummaryPanel Class="ValidationSummary displayNone"</li>
            <li>Nest controls to be validated in a div or tr class=FormRow</li>
            <li>Each element to validate gets a validate property with a semicolon delimited array of validation values (e.g., validate="required;phone")</li>
            <li>For each value, include an error message (e.g. required_message="enter a valid phone" phone_message="enter a valid phone"). Do this in the resx file.</li>
            <li>An element can use a custom function</li>
        </ul>
        <br />
        <div>(Please see the code for all details)</div>
        <div><a href="http://www.youtube.com/watch?v=Cbk980jV7Ao">Bonus material</a></div>
    </div>
    
    <br />
    <h3>Validation Samples</h3>
    <br />
    <h4>Regular Modal</h4>
    <br />
    <div>
        <Corbis:GlassButton ButtonBackground="gray36" Text="Launch Modal" OnClientClick="launchValidationModal();return false;" runat="server" />
    </div>
    
    <br />
    <br />
    <h4>Error Tracker Modal (different scripts)</h4>
    <br />
    <div>
        <Corbis:GlassButton ButtonBackground="gray36" Text="Launch ET Modal" OnClientClick="launchETModal();return false;" runat="server" />
    </div>
    
    <br />
    <br />
    <h4>IModal</h4>
    <br />
    <div>
        <Corbis:GlassButton ID="GlassButton1" ButtonBackground="gray36" Text="Launch IModal" OnClientClick="OpenNewIModal('/Test/ValidationIModal.aspx', 350, 250, 'validationIModal');return false;" runat="server" />
    </div>
    <br />
    <br />
    <h4>IModal CodeBehind Validation</h4>
    <br />
    <div>
        <Corbis:GlassButton ID="gb" ButtonBackground="gray36" Text="Launch IModal CodeBehind" OnClientClick="OpenNewIModal('/Test/ValidationIModalCodeBehind.aspx', 350, 250, 'validationIModalCodeBehind');return false;" runat="server" />
    </div>
    <br />
    <br />
    <h4>SSL IModal (using iframetunnel)</h4>
    <br />
    <div>
        <Corbis:GlassButton ID="GlassButton2" ButtonBackground="gray36" Text="Launch SSL IModal" OnClientClick="OpenNewIModal('/Test/ValidationIModalSSL.aspx', 350, 250, 'validationIModalSSL');return false;" runat="server" />
    </div>
    
    <Corbis:ModalPopup 
        ID="validationModal" ContainerID="validationModalContainer" 
        runat="server" Width="350" Title="Validation Sample" UseDefaultPadding="false"
        ><Corbis:ValidationSample ID="vs" runat="server" />
    </Corbis:ModalPopup>
    <Corbis:ModalPopup 
        ID="ETModal" ContainerID="ETModalContainer" 
        runat="server" Width="350" Title="Error Tracking Sample" UseDefaultPadding="false"
        ><Corbis:ET ID="et" runat="server" />
    </Corbis:ModalPopup>
    
    <%--<asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:DropDownList ID="dd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dd_changed">
                <asp:ListItem Value="1" Text="1"/>
                <asp:ListItem Value="2" Text="2"/>
                <asp:ListItem Value="3" Text="3"/>
            </asp:DropDownList><br />
            <asp:TextBox ID="tb" runat="server"/><br />
            <asp:Button ID="btn" runat="server" OnClick="btn_click" Text="save" />
            <asp:LinkButton CssClass="displayNone" ID="btn2" runat="server" OnClick="btn_click" Text="foo" />
            <br />Saved value = 
            <asp:Label ID="lbl" runat="server" Text="[saved value]" />
            <button onclick="silly()">simulate js call</button>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <script>
        function silly() {
            __doPostBack(('<%=btn2.ClientID %>').replace(/_/g, '$'), '');
        }
    
    </script>--%>
</div>
</asp:Content>