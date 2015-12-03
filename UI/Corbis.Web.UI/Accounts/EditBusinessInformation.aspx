<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditBusinessInformation.aspx.cs" Inherits="Corbis.Web.UI.Accounts.EditBusinessInformation" MasterPageFile="~/MasterPages/ModalPopup.Master" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<asp:Content ID="editBusinessInformation" runat="server" ContentPlaceHolderID="mainContent">
    
<script type="text/javascript" language="javascript">
    window.addEvent('load', function() {
        parent.ResizeIModal('editBusinessInformation', GetDocumentHeight());
    })
</script>
<div class="MyPersonalInfoDiv" id="MyBusinessInfoDiv">
    <div class="titleWrapper">
        <span class="title"><Corbis:Localize ID="Localize1" runat="server" meta:resourcekey="EditBusinessInformationTitle"></Corbis:Localize></span>
        <div class="personalCloseButton" id="personalCloseButton">
            <img alt="Close Modal Popup" onclick="parent.MochaUI.CloseModal('editBusinessInformation');return false;" class="Close" src="/Images/iconClose.gif" />
        </div>
    </div>
    <div class="businessContentWrapper">
            <Corbis:ValidationHub ID="vHub" SubmitForm="true"  
            IsIModal="true" runat="server" ContainerID="MyBusinessInfoDiv"
            PopupID="editBusinessInformation" 
        />

            <div class="clear"></div>
            <table cellpadding="1" cellspacing="0" class="businessTable">
                <tr id="Tr1" runat="server" class="FormRow" enableviewstate="false" >
                    <td class="LeftForm">
                        <Corbis:Localize ID="companyNameLabel" runat="server" 
                         Text="<%$ Resources:Accounts, CompanyLabel %>" />
                    </td>
                    <td class="RightForm">
                        <Corbis:TextBox class="item" validate="required" required_message="<%$ Resources:Accounts, MemberValidationError_CompanyNameRequired %>" ID="companyName" runat="server"></Corbis:TextBox>
                    </td>
                </tr>
                <tr class="FormRow">
                    <td class="LeftForm">
                        <Corbis:Localize ID="jobTitleLabel" runat="server" Text="<%$ Resources:Accounts, JobTitleLabel %>" />
                    </td>
                    <td class="RightForm">
                      <Corbis:DropDownList class="item" validate="custom1" custom1="validateJobTitle()" ID="jobTitle" custom1_message="<%$ Resources:Accounts, MemberValidationError_JobTitleRequired %>" runat="server">
                      </Corbis:DropDownList>
                    </td>
                </tr>
                <tr id="Tr2" class="FormRow" runat="server" enableviewstate="false">
                    <td class="LeftForm">
                      <Corbis:Localize ID="phoneLabel" runat="server" Text="<%$ Resources:Accounts, PhoneLabel %>" />
                    </td>
                    <td class="RightForm">
                     <Corbis:TextBox class="item" validate="required" required_message="<%$ Resources:Accounts, MemberValidationError_BusinessPhoneRequired %>" ID="phone" runat="server" />
                    </td>
                </tr>
                <tr class="titleWrapper">
                    <td colspan="2" class="titleWrapper">
                        <div class="marginTop"><Corbis:Localize ID="businessAddressTitle" runat="server" meta:resourcekey="BusinessAddress"></Corbis:Localize></div>
                    </td>
                </tr>
                <Corbis:Address ID="businessAddress" runat="server"
                Address1Caption="<%$ Resources:Accounts, Address1Label %>"
                Address2Caption="<%$ Resources:Accounts, Address2Label %>"
                Address3Caption="<%$ Resources:Accounts, Address3Label %>"
                CityCaption="<%$ Resources:Accounts, CityLabel %>"
                CountryCaption="<%$ Resources:Accounts, CountryLabel %>"
                RegionCaption="<%$ Resources:Accounts, StateLabel %>"
                PostalCodeCaption="<%$ Resources:Accounts, ZipLabel %>"
                RowCssClass="FormRow"
                LabelsCssClass="LeftForm"
                FormFieldsCssClass="RightForm"
                OptionFieldCssClass="Optional"
                DropdownOptionalText="<%$ Resources: Resource, Dashes %>"
                DropdownRequiredText="<%$ Resources: Resource, SelectOne %>"
                OptionalText="<%$ Resources: Resource, Optional %>"
                ValidationGroup="ChangePersonalInfoValidationGroup" />
            </table>
            <div class="businessButtons">
                <Corbis:LinkButton ID="lb" runat="server" OnClick="Save_Click" CssClass="ValidateClickLB displayNone" />
                <Corbis:GlassButton ID="editBusinessInformationSave" runat="server" OnClientClick="return false;" validate="submit" meta:resourceKey="save" CssClass="buttonsSpacing"/>
                <Corbis:GlassButton CausesValidation="false" ID="editBusinessInformationCancel" CssClass="buttonsSpacing" OnClientClick="parent.MochaUI.CloseModal('editBusinessInformation');return false;" runat="server"  meta:resourceKey="Cancel"  ButtonStyle="Gray"/>
            </div>
        <div class="displayNone">
            <asp:PlaceHolder ID="validatorContainer" runat="server"></asp:PlaceHolder>
        </div>
    </div>
</div>
    <div class="clr"></div>

<script>

    function validateJobTitle() {
         var _jobTitleList = $('<%=jobTitle.ClientID %>');
         var result = (_jobTitleList.selectedIndex == 0);
        _Validation.highlightRow(result, _jobTitleList);
        return !result;
    }
</script>

</asp:Content>