<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="EditBusinessInformation.ascx.cs" Inherits="Corbis.Web.UI.Accounts.EditBusinessInformation" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<script type="text/javascript" language="javascript">
var domObj = $('editBusinessInformationModalPopup');
if (domObj != null)
{            
    domObj.addEvent('resize', pageLoad);
    function pageLoad(sender, args)
    {
        domObj.setStyle('height', document.getCoordinates().height);
    }
}
</script>
<div class="ModalPopupPanel" id="ModalPopupPanel">
    <div class="ModalTitleBar">
        <span class="Title"><Corbis:Localize ID="editBusinessInformationTitle" runat="server" meta:resourcekey="EditBusinessInformationTitle"></Corbis:Localize></span>
        <Corbis:ImageButton ID="editBusinessInformationClose" runat="server" CausesValidation="false" CssClass="Close" ImageUrl="/Images/iconClose.gif"></Corbis:ImageButton>
    </div>
    <div class="ModalPopupContent">
        <asp:UpdatePanel ID="editBusinessInformationUpdatePanel" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="editBusinessInformationPanel" runat="server" DefaultButton="editBusinessInformationSave$GlassButton">
                    <Corbis:ValidationGroupSummary ID="editBusinessInformationValidationSummary" runat="server" CssClass="Error" ValidationGroup="EditBusinessInformationValidationGroup" />
                    <asp:CustomValidator ID="nonAsciiValidator" runat="server" Display="None" ValidationGroup="EditBusinessInformationValidationGroup" />
                    <div class="clear"></div>
                    <table cellpadding="1" cellspacing="0">
                        <tr runat="server" class="FormRow">
                            <td class="FormLeft"><Corbis:Localize ID="companyNameLabel" runat="server" meta:resourceKey="CompanyName"></Corbis:Localize></td>
                            <td class="FormRight"><Corbis:TextBox ID="companyName" runat="server" MaxLength="60" ValidateControl="true" ValidationGroup="EditBusinessInformationValidationGroup" ValidationObjectType="Corbis.Membership.Contracts.V1.MemberAddress" ValidationRulesetName="CreateMember" ValidationPropertyName="CompanyName" ValidatorContainer="validatorContainer"></Corbis:TextBox></td>
                        </tr>
                        <tr class="FormRow">
                            <td class="FormLeft"><Corbis:Localize ID="jobTitleLabel" runat="server" meta:resourceKey="JobTitle"></Corbis:Localize></v>
                            <td class="FormRight"><AJAXToolKit:TextBoxWatermarkExtender ID="jobTitleWatermark" runat="server" TargetControlID="jobTitle" WatermarkText="<%$ Resources: Resource, Optional %>" WatermarkCssClass="Optional" /><Corbis:TextBox ID="jobTitle" runat="server" MaxLength="512"></Corbis:TextBox></td>
                        </tr>
                        <tr runat="server" class="FormRow">
                            <td class="FormLeft"><Corbis:Localize ID="phoneLabel" runat="server" meta:resourceKey="Phone"></Corbis:Localize></td>
                            <td class="FormRight"><Corbis:TextBox ID="phone" runat="server" MaxLength="20" ValidateControl="true" ValidationGroup="EditBusinessInformationValidationGroup" ValidationObjectType="Corbis.Membership.Contracts.V1.MemberAddress" ValidationRulesetName="CreateMember" ValidationPropertyName="PhoneNumber" ValidatorContainer="validatorContainer"></Corbis:TextBox></td>
                        </tr>
                        <tr class="ModalSectionTitleBar">
                            <td colspan="2">
                                <span class="Title"><Corbis:Localize ID="businessAddressTitle" runat="server" meta:resourcekey="BusinessAddress"></Corbis:Localize></span>
                            </td>
                        </tr>
                        <Corbis:Address ID="businessAddress" runat="server"
                            DropdownOptionalText="<%$ Resources: Resource, Dashes %>"
                            DropdownRequiredText="<%$ Resources: Resource, SelectOne %>"
                            OptionalText="<%$ Resources: Resource, Optional %>"
                            ValidationGroup="EditBusinessInformationValidationGroup"
                            Address1Caption="<%$ Resources: Address1Caption %>"
                            Address2Caption="<%$ Resources: Address2Caption %>"
                            Address3Caption="<%$ Resources: Address3Caption %>"
                            CityCaption="<%$ Resources: CityCaption %>"
                            CountryCaption="<%$ Resources: CountryCaption %>"
                            RegionCaption="<%$ Resources: RegionCaption %>"
                            PostalCodeCaption="<%$ Resources: PostalCodeCaption %>"
                            Address1ErrorMessage="<%$ Resources: Address1ErrorMessage %>"
                            CityErrorMessage="<%$ Resources: CityErrorMessage %>"
                            PostalCodeErrorMessage="<%$ Resources: PostalCodeErrorMessage %>"
                            CountryErrorMessage="<%$ Resources: CountryErrorMessage %>"
                            RegionErrorMessage="<%$ Resources: RegionErrorMessage %>"
                            RowCssClass="FormRow"
                            LabelsCssClass="FormLeft"
                            FormFieldsCssClass="FormRight"
                            OptionFieldCssClass="Optional"/>
                    </table>
                    <div class="FormButtons">
                        <Corbis:GlassButton ID="editBusinessInformationSave" runat="server" CausesValidation="true" meta:resourceKey="Save" ValidationGroup="EditBusinessInformationValidationGroup"/>
                        <Corbis:GlassButton CausesValidation="false" ID="editBusinessInformationCancel" runat="server"  meta:resourceKey="Cancel"  ButtonStyle="Gray"/>
                    </div>
                    <div ID="validatorContainer" runat="server" class="displayNone"></div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
