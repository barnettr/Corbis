<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="EditShippingAddress2.ascx.cs" Inherits="Corbis.Web.UI.Accounts.EditShippingAddress2" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<script type="text/javascript" language="javascript">
var domObj = $('editShippingAddressModalPopup');
if (domObj != null)
{            
    domObj.addEvent('resize', pageLoad);
    function pageLoad(sender, args)
    {
        domObj.setStyle('height', document.getCoordinates().height);
    }
}

</script>
<div id="modalPopupPanel" runat="server" class="ModalPopupPanel">

<asp:UpdatePanel ID="editShippingAddressUpdatePanel" runat="server">
<ContentTemplate>
    <%-- mike: something wrong here!!!--%>
    <div class="ModalTitleBar">
        <Corbis:Label CssClass="Title" ID="editShippingAddressTitle" runat="server" meta:resourcekey="addShippingAddress"/>
        <Corbis:ImageButton OnClientClick="MochaUI.HideModal('editShippingAddressModalPopup'); " ID="editShippingAddressClose" runat="server" CausesValidation="false" CssClass="Close" imageurl="/Images/iconClose.gif" />
    </div>
<%--                                <Corbis:ImageCheckbox runat="server" ID="sameAsBusinessAddr" AutoPostBack="true" TextStyle="Normal" OnCheckedChanged="sameAsBusinessAddr_CheckedChanged" meta:resourceKey="sameAsBusinessAddr"  />
--%>
   
        <asp:Panel ID="editShippingAddressPanel" runat="server" DefaultButton="editShippingAddressSave$glassButton">
            <div class="ModalPopupContent">
                <Corbis:ValidationGroupSummary 
                    ID="editShippingAddressValidationSummary" 
                    runat="server" ValidationGroup="EditShippingAddressValidationGroup" 
                />
                    <asp:CustomValidator ID="nonAsciiValidator" runat="server" Display="None" ValidationGroup="EditShippingAddressValidationGroup" />
                <div class="clear"></div>
                <table cellpadding="1" cellspacing="0" class="Address">
                    <tr class="FormRow">
                        <td class="FormLeft col2" colspan="2">
                            <Corbis:ImageCheckBox ID="sameAsBusinessAddr" runat="server"  meta:resourcekey="sameAddress" AutoPostBack="true"
                                oncheckedchanged="sameAsBusinessAddr_CheckedChanged" />
                        </td>
                    </tr>
                    <tr class="FormRow">
                        <td class="FormLeft col2" colspan="2">
                            <Corbis:ImageCheckBox ID="setAsDefault" runat="server" OnCheckedChanged="sameAsBusinessAddr_CheckedChanged" meta:resourcekey="SetAsDefault" />
                        </td>
                    </tr>
                    <tr runat="server" class="FormRow">
                        <td class="FormLeft">
                            <span><Corbis:Localize ID="addressNameLabel" runat="server" meta:resourceKey="Name"/></span>
                        </td>
                        <td class="FormRight">
                            <Corbis:TextBox ID="addressName" runat="server" MaxLength="100"
                                ValidateControl="true" ValidationGroup="EditShippingAddressValidationGroup" 
                                ValidationObjectType="Corbis.Membership.Contracts.V1.MemberAddress" 
                                ValidationRulesetName="CreateMember" ValidationPropertyName="Name" 
                                ValidatorContainer="validatorContainer"
                            />
                        </td>
                    </tr>
                    <tr runat="server" class="FormRow">
                        <td class="FormLeft">
                            <span><Corbis:Localize ID="companyNameLabel" runat="server" meta:resourceKey="CompanyName"/></span>
                        </td>
                        <td class="FormRight">
                            <Corbis:TextBox ID="companyName" runat="server" MaxLength="60"
                                ValidateControl="false" ValidationGroup="EditShippingAddressValidationGroup" 
                                ValidationObjectType="Corbis.Membership.Contracts.V1.MemberAddress" 
                                ValidationRulesetName="CreateMember" ValidationPropertyName="CompanyName" 
                                ValidatorContainer="validatorContainer"
                            />
                        </td>
                    </tr>
                    <Corbis:Address ID="shippingAddressControl" 
                        runat="server" 
                        DropdownOptionalText="<%$ Resources: Resource, Dashes %>"
                        DropdownRequiredText="<%$ Resources: Resource, SelectOne %>"
                        OptionalText="<%$ Resources: Resource, Optional %>"
                        ValidationGroup="EditShippingAddressValidationGroup"
                        Address1Caption="<%$ Resources:Address, Address1 %>" 
                        Address2Caption="<%$ Resources:Address, Address2 %>"
                        Address3Caption="<%$ Resources:Address, Address3 %>"
                        CityCaption="<%$ Resources:Address, City %>" 
                        RegionCaption="<%$ Resources:Address, Region %>" 
                        CountryCaption="<%$ Resources:Address, Country %>" 
                        PostalCodeCaption="<%$ Resources:Address, PostalCode %>"
                        Address1ErrorMessage="<%$ Resources: Address, Address1ErrorMessage %>"
                        CityErrorMessage="<%$ Resources: Address, CityErrorMessage %>"
                        CountryErrorMessage="<%$ Resources: Address, CountryErrorMessage %>" 
                        PostalCodeErrorMessage="<%$ Resources: Address, PostalCodeErrorMessage %>" 
                        RegionErrorMessage="<%$ Resources: Address, RegionErrorMessage %>" 
                        RowCssClass="FormRow"
                        LabelsCssClass="FormLeft"
                        FormFieldsCssClass="FormRight"
                        OptionFieldCssClass="Optional" 
                    />
                    <asp:HiddenField ID="addressUid" runat="server" />
                    <tr runat="server" class="FormRow">
                        <td class="FormLeft">
                            <Corbis:Localize ID="phoneLabel" runat="server" meta:resourceKey="Phone"></Corbis:Localize>
                        </td>
                        <td class="FormRight">
                            <Corbis:TextBox ID="phone" runat="server" MaxLength="20" ValidateControl="true" 
                            ValidationGroup="EditShippingAddressValidationGroup" 
                            ValidationObjectType="Corbis.Membership.Contracts.V1.MemberAddress" 
                            ValidationRulesetName="CreateMember" ValidationPropertyName="PhoneNumber" 
                            ValidatorContainer="validatorContainer"></Corbis:TextBox>
                        </td>
                    </tr>
                </table>
                <div class="FormButtons">
                    <Corbis:GlassButton
                        ID="editShippingAddressCancel" runat="server" 
                         OnClientClick="MochaUI.HideModal('editShippingAddressModalPopup');"
                         ButtonStyle=Gray CausesValidation="false" meta:resourceKey="Cancel"
                    />
                    <Corbis:GlassButton 
                        ID="editShippingAddressSave" runat="server" 
                        CausesValidation="true" meta:resourceKey="Save" 
                        ValidationGroup="EditShippingAddressValidationGroup"
                    />                    
                </div>
                <div class="displayNone">
                    <asp:PlaceHolder ID="validatorContainer" runat="server"></asp:PlaceHolder>
                </div>
            </div>
            <AJAXToolkit:TextBoxWatermarkExtender ID="companyFieldWatermark" runat="server" TargetControlID="companyName"
                WatermarkText="<%$ Resources: Resource, Optional %>" WatermarkCssClass="Optional" />

        </asp:Panel>
        <asp:Panel ID="deleteShippingAddressPanel" runat="server" DefaultButton="deleteYes$glassButton">
            <div class="ModalPopupContent">
                <Corbis:Localize ID="confirmDeleteMessage" runat="server" meta:resourcekey="ConfirmDeleteMessage" /><Corbis:Localize ID="confirmDeleteDefaultMessage" runat="server" meta:resourcekey="ConfirmDeleteDefaultMessage" />
                <div class="Buttons">
                    <Corbis:GlassButton
                        ID="deleteYes" runat="server" 
                        CausesValidation="false" ButtonBackground="e8e8e8" meta:resourceKey="Okay"
                    />
                    <Corbis:GlassButton
                        ID="deleteNo" runat="server" meta:resourceKey="Cancel"
                        CausesValidation="false" ButtonBackground=e8e8e8 ButtonStyle=gray 
                    />
                </div>
            </div>
        </asp:Panel>    
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="sameAsBusinessAddr" EventName="CheckedChanged" />
    </Triggers>
</asp:UpdatePanel>
</div>
