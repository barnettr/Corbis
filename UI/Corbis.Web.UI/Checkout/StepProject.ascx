<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StepProject.ascx.cs"
    Inherits="Corbis.Web.UI.Checkout.StepProject" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<div class="paneLayout" id="projectPaneLayout">
    <h4><Corbis:Localize runat="server" meta:resourcekey="followingInformation"/></h4>
    
    <Corbis:ValidationHub 
        ID="projectValidationHub" runat="server" IsPopup="false"
        ContainerID="projectPaneLayout" UniqueName="Project"
        SuccessScript="checkoutTabs.show(1, true);setTimeout(initDeliveryValidation,100);"
        SubmitByAjax="true" AjaxUrl="CheckoutService.asmx/ValidateProjectEncoding"
        OnAjaxRequest="SetProjectAjaxData" UseStandardAjaxBehavior="true"
    />
    <script>
        function SetProjectAjaxData() {
            <%=projectValidationHub.ClientInstanceVariableName %>.options.ajaxData = {
                'projectName' : $('<%=projectField.ClientID %>').value,
                'projectNameClientId' : '<%=projectField.ClientID %>',
                'jobNumber' : $('<%=jobField.ClientID %>').hasClass('Optional')? '' : $('<%=jobField.ClientID %>').value,
                'jobNumberClientId' : '<%=jobField.ClientID %>',
                'poNumber' : $('<%=poField.ClientID %>').hasClass('Optional') ? '' : $('<%=poField.ClientID %>').value,
                'poNumberClientId' : '<%=poField.ClientID %>',
                'licensee' : $('<%=licenseeField.ClientID %>').value,
                'licenseeClientId' : '<%=licenseeField.ClientID %>'
                };
        }
        
        
    </script>
    <table cellspacing="0">
        <tr runat="server" class="FormRow">
            <td class="thingType" nowrap>
                <Corbis:Localize ID="projectName" runat="server" meta:resourcekey="projectName" />
            </td>
            <td class="thingValue" style="width: 100%">
                <Corbis:TextBox runat="server" ID="projectField" CssClass="thingValue" Width="200"  MaxLength="50"
                    ValidatorContainer="validatorContainer" ValidateControl="true" ValidationGroup="Project"
                    ValidationObjectType="Corbis.WebOrders.Contracts.V1.ProjectInformation" ValidationPropertyName="Name"
                    ValidationRulesetName="ProjectInfo" />
            </td>
        </tr>
        <tr class="FormRow">
            <td class="thingType">
                <Corbis:Localize ID="jobNumber" runat="server" meta:resourcekey="jobNumber" />
            </td>
            <td class="thingValue">
                <asp:TextBox runat="server" ID="jobField" CssClass="thingValue" MaxLength="50" />
            </td>
        </tr>
        <tr class="FormRow">
            <td class="thingType">
                <Corbis:Localize ID="poNumber" runat="server" meta:resourcekey="poNumber" />
            </td>
            <td class="thingValue">
                <asp:TextBox runat="server" ID="poField" CssClass="thingValue"  MaxLength="50" />
            </td>
        </tr>
        <tr runat="server" class="FormRow">
            <td class="thingType">
                <Corbis:Localize ID="licensee" runat="server" meta:resourcekey="licensee" />
            </td>
            <td class="thingValue">
                <Corbis:TextBox runat="server" ID="licenseeField" CssClass="thingValue" Width="200"  MaxLength="50"
                    validate="required" meta:resourcekey="licenseeField"
                />
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td><div class="licenseeInfo">
                    <Corbis:Localize ID="notTransferable" runat="server" meta:resourcekey="notTransferable" /></div>
            </td>
         </tr>
    </table>
</div>
<AJAXToolkit:TextBoxWatermarkExtender ID="jobFieldWatermark" runat="server" TargetControlID="jobField"
    WatermarkText="<%$ Resources: Resource, Optional %>" WatermarkCssClass="Optional" />
<AJAXToolkit:TextBoxWatermarkExtender ID="poFieldWatermark" runat="server" TargetControlID="poField"
    WatermarkText="<%$ Resources: Resource, Optional %>" WatermarkCssClass="Optional" />
<div class="buttonBar ">
    <!-- "Want to quit checkout?" modal -->
    <Corbis:ModalPopup ContainerID="quitCheckout" runat="server" meta:resourcekey="areYouSure">
        <Corbis:GlassButton runat="server" ID="continueCheckout" meta:resourcekey="continue" ButtonStyle="Gray" CausesValidation="false" OnClientClick="HideModal('quitCheckout');return false;" />  
        <Corbis:GlassButton runat="server" ID="quitCheckout" meta:resourcekey="quit" CausesValidation="false" OnClientClick="GoToCartByFlow(true);return false;" />  
    </Corbis:ModalPopup>
    <a href="javascript:OpenModal('quitCheckout'); ResizeModal('quitCheckout');">
        <Corbis:Localize ID="quit" runat="server" meta:resourcekey="quit" /></a>
    <ul class="buttonPositioner">
        <li class="next">
            <Corbis:GlassButton ID="GlassButton1" runat="server" ButtonStyle="Orange"  OnClientClick="_ProjectValidation.validateAll();return false;" meta:resourcekey="nextStep" />

        </li>
        &nbsp;
    </ul>
</div>

