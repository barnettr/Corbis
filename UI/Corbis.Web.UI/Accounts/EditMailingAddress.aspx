<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditMailingAddress.aspx.cs" Inherits="Corbis.Web.UI.Accounts.EditMailingAddress" MasterPageFile="~/MasterPages/ModalPopup.Master" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<asp:Content ID="editMailingAddress" runat="server" ContentPlaceHolderID="mainContent">    
<script type="text/javascript" language="javascript">
    window.addEvent('load', function() {
        parent.ResizeIModal('editMailingAddress', GetDocumentHeight());
    })
</script>
<div class="MyPersonalInfoDiv" id="MyPersonalInfoDiv">
    <div class="titleWrapper">
        <span class="title"><Corbis:Localize ID="editMailingAddressTitle" runat="server" meta:resourcekey="EditMailingAddressTitle"></Corbis:Localize><Corbis:Localize ID="addMailingAddressTitle" runat="server" meta:resourcekey="AddMailingAddressTitle"></Corbis:Localize></span>   
        <div class="personalCloseButton" id="personalCloseButton" style="float:right;margin:0px 0px 0px 0px;vertical-align:top;cursor:pointer; position:absolute; right:7px;top:0px">
            <img alt="Close Modal Popup" onclick="parent.MochaUI.CloseModal('editMailingAddress');return false;" class="Close" src="/Images/iconClose.gif" />
        </div>
    </div>
    <div class="mailContentWrapper">   
                <asp:Panel ID="editMailingAddressPanel" runat="server">
                    <Corbis:ValidationGroupSummary 
                        ID="editMailingAddressValidationSummary" 
                        runat="server"
                        CssClass="Error"
                        ValidationGroup="EditMailingAddressValidationGroup" />
                    <asp:CustomValidator ID="nonAsciiValidator" runat="server" Display="None" ValidationGroup="EditMailingAddressValidationGroup" />        
                    <div class="clear"></div>
                    <table cellpadding="1" cellspacing="0">
                        <tr class="FormRow">
                            <td class="LeftForm alignLeft" colspan="2">
                                <div class="checkLblPair"><Corbis:ImageCheckbox ID="sameAsBusiness" checked="false" runat="server"  AutoPostback="true" meta:resourcekey="SameAsBusinessCheck"></Corbis:ImageCheckbox></div>
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
                            RowCssClass="FormRow"
                            LabelsCssClass="LeftForm"
                            FormFieldsCssClass="RightForm"
                            OptionFieldCssClass="Optional"/>
                    </table>
                    <div class="mailButtons">
                        <Corbis:GlassButton 
                            ID="editMailingAddressSave" runat="server" CssClass="buttonsSpacing" 
                            CausesValidation="true" meta:resourceKey="Save" 
                            ValidationGroup="EditMailingAddressValidationGroup"
                        />
                        <Corbis:GlassButton
                            ID="editMailingAddressCancel" runat="server" CssClass="buttonsSpacing" 
                            OnClientClick="parent.MochaUI.CloseModal('editMailingAddress');return false;"
                            CausesValidation="false" ButtonStyle=Gray meta:resourceKey="Cancel"
                        />
                    </div>
                    <div class="displayNone">
                        <asp:PlaceHolder ID="validatorContainer" runat="server"></asp:PlaceHolder>
                    </div>
                </asp:Panel>
    </div>
</div>
<div style="clear: both;"></div>
</asp:Content>
