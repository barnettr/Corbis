<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditMailingAddress.ascx.cs" Inherits="Corbis.Web.UI.Accounts.EditMailingAddress" %>
<%@ Register Assembly="Corbis.Web.UI.Controls" Namespace="Corbis.Web.UI.Controls" TagPrefix="Corbis" %>
<script type="text/javascript" language="javascript">
var domObj = $('editMailingAddressModalPopup');
if (domObj != null)
{            
    domObj.addEvent('resize', pageLoad);
    function pageLoad(sender, args)
    {
        domObj.setStyle('height', document.getCoordinates().height);
    }
}

</script>
<div class="ModalPopupPanel">
    <div class="ModalTitleBar">
        <span class="Title"><Corbis:Localize ID="editMailingAddressTitle" runat="server" meta:resourcekey="EditMailingAddressTitle"></Corbis:Localize><Corbis:Localize ID="addMailingAddressTitle" runat="server" meta:resourcekey="AddMailingAddressTitle"></Corbis:Localize></span>   
        <Corbis:ImageButton ID="editMailingAddressClose" runat="server" CausesValidation="false" CssClass="Close" ImageUrl="/Images/iconClose.gif" />
    </div>
    <div class="ModalPopupContent">   
        <asp:UpdatePanel ID="editMailingAddressUpdatePanel" runat="server">
            <ContentTemplate>
                <asp:Panel ID="editMailingAddressPanel" runat="server" DefaultButton="editMailingAddressSave$glassButton">
                    <Corbis:ValidationGroupSummary 
                        ID="editMailingAddressValidationSummary" 
                        runat="server"
                        CssClass="Error"
                        ValidationGroup="EditMailingAddressValidationGroup" />
                    <asp:CustomValidator ID="nonAsciiValidator" runat="server" Display="None" ValidationGroup="EditMailingAddressValidationGroup" />        
                    <div class="clear"></div>
                    <table cellpadding="1" cellspacing="0">
                        <tr class="FormRow">
                            <td class="FormLeft alignLeft" colspan="2">
                                <Corbis:CheckBox ID="sameAsBusiness" runat="server" AutoPostback="true" meta:resourcekey="SameAsBusinessCheck" />
                            </td>
                        </tr>
                        <Corbis:Address ID="mailingAddressControl" 
                            runat="server" 
                            DropdownOptionalText="<%$ Resources: Resource, Dashes %>" 
                            DropdownRequiredText="<%$ Resources: Resource, SelectOne %>" 
                            ValidationGroup="EditMailingAddressValidationGroup"
                            Address1Caption="<%$ Resources:Address, Address1 %>" 
                            Address2Caption="<%$ Resources:Address, Address2 %>"
                            Address3Caption="<%$ Resources:Address, Address3 %>"
                            CityCaption="<%$ Resources:Address, City %>" 
                            RegionCaption="<%$ Resources:Address, Region %>" 
                            CountryCaption="<%$ Resources:Address, Country %>" 
                            PostalCodeCaption="<%$ Resources:Address, PostalCode %>"
                            OptionalText="<%$ Resources:Resource, Optional %>"
                            Address1ErrorMessage="<%$ Resources: Address, Address1ErrorMessage %>"
                            CityErrorMessage="<%$ Resources: Address, CityErrorMessage %>"
                            CountryErrorMessage="<%$ Resources: Address, CountryErrorMessage %>" 
                            PostalCodeErrorMessage="<%$ Resources: Address, PostalCodeErrorMessage %>" 
                            RegionErrorMessage="<%$ Resources: Address, RegionErrorMessage %>" 
                            RowCssClass="ErrorRow"
                            LabelsCssClass="FormLeft"
                            FormFieldsCssClass="FormRight"
                            OptionFieldCssClass="Optional"/>
                    </table>
                    <div class="FormButtons">
                        <Corbis:GlassButton 
                            ID="editMailingAddressSave" runat="server" 
                            CausesValidation="true" meta:resourceKey="Save" 
                            ValidationGroup="EditMailingAddressValidationGroup"
                        />
                        <Corbis:GlassButton
                            ID="editMailingAddressCancel" runat="server" 
                            CausesValidation="false" ButtonStyle=Gray meta:resourceKey="Cancel"
                        />
                    </div>
                    <div class="displayNone">
                        <asp:PlaceHolder ID="validatorContainer" runat="server"></asp:PlaceHolder>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
