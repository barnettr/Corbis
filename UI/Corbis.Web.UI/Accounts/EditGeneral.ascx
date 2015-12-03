<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditGeneral.ascx.cs" Inherits="Corbis.Web.UI.Accounts.EditGeneral" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register Assembly="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet" Namespace="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet" TagPrefix="Microsoft" %>
<%@ Register TagPrefix="AJAXExtentions" Namespace="AjaxControlToolkit.WCSFExtensions" Assembly="AjaxControlToolkit.WCSFExtensions" %>

<div id="shippingNameDiv" runat="server">
    <div class="FormLeft"><Corbis:Localize ID="shippingNameLabel" runat="server" meta:resourcekey="shippingNameLabel" /></div>
    <div class="FormRight"><Corbis:TextBox ID="shippingName" runat="server"></Corbis:TextBox></div>
</div>
<div id="userNameDiv" runat="server">
    <div class="FormLeft"><Corbis:Localize ID="userNameLabel" runat="server" meta:resourcekey="userNameLabel" /></div>
    <div class="FormRight"><Corbis:TextBox ID="userName" runat="server" ValidateControl="true" ValidationObjectType="Corbis.Membership.Contracts.V1.Member" ValidationRulesetName="CreateMember" ValidationPropertyName="UserName" ValidatorContainer="validatorContainer"></Corbis:TextBox></div>
</div>
<div id="companyNameDiv" runat="server">
    <div class="FormLeft"><Corbis:Localize ID="companyNameLabel" runat="server" meta:resourcekey="companyNameLabel" /></div>
    <div class="FormRight"><Corbis:TextBox ID="companyName" runat="server" ValidateControl="true" ValidationObjectType="Corbis.Membership.Contracts.V1.MemberAddress" ValidationRulesetName="CreateMember" ValidationPropertyName="CompanyName" ValidatorContainer="validatorContainer"></Corbis:TextBox></div>
</div>
<div id="jobTitleDiv" runat="server">
    <div class="FormLeft"><Corbis:Localize ID="jobTitleLabel" runat="server" meta:resourcekey="jobTitleLabel" /></div>
    <div class="FormRight"><Corbis:TextBox ID="jobTitle" runat="server"></Corbis:TextBox></div>
</div>
<div id="nameDiv" runat="server">
    <div class="FormLeft"><Corbis:Localize ID="firstNameLabel" runat="server" meta:resourcekey="firstNameLabel" /></div>
    <div class="FormRight"><Corbis:TextBox ID="firstName" runat="server" ValidateControl="true" ValidationObjectType="Corbis.Membership.Contracts.V1.Member" ValidationRulesetName="CreateMember" ValidationPropertyName="FirstName" ValidatorContainer="validatorContainer"></Corbis:TextBox></div>
    <div class="FormLeft"><Corbis:Localize ID="middleInitialLabel" runat="server" meta:resourcekey="middleInitialLabel" /></div>
    <div class="FormRight"><Corbis:TextBox ID="middleInitial" runat="server"></Corbis:TextBox></div>
    <div class="FormLeft"><Corbis:Localize ID="lastNameLabel" runat="server" meta:resourcekey="lastNameLabel" /></div>
    <div class="FormRight"><Corbis:TextBox ID="lastName" runat="server" ValidateControl="true" ValidationObjectType="Corbis.Membership.Contracts.V1.Member" ValidationRulesetName="CreateMember" ValidationPropertyName="LastName" ValidatorContainer="validatorContainer"></Corbis:TextBox></div>
</div>
<div id="emailDiv" runat="server">
    <div class="FormLeft"><Corbis:Localize ID="emailLabel" runat="server" meta:resourcekey="emailLabel" /></div>
    <div class="FormRight"><Corbis:TextBox ID="email" runat="server" ValidateControl="true" ValidationObjectType="Corbis.Membership.Contracts.V1.Member" ValidationRulesetName="CreateMember" ValidationPropertyName="Email" ValidatorContainer="validatorContainer"></Corbis:TextBox></div>
    <div class="FormLeft"><Corbis:Localize ID="emailConfirmLabel" runat="server" meta:resourcekey="emailConfirmLabel" /></div>
    <div class="FormRight"><Corbis:TextBox ID="emailConfirm" runat="server"></Corbis:TextBox>
        <asp:RequiredFieldValidator ID="emailConfirmRequired" runat="server" CssClass="Error" ControlToValidate="emailConfirm" Display="Dynamic" meta:resourcekey="emailConfirmRequired" Enabled="true" Visible="true" EnableClientScript="false" /> 
        <asp:CompareValidator ID="emailConfirmMatchValidator" runat="server" CssClass="Error" ControlToValidate="emailConfirm" Display="Dynamic" meta:resourcekey="emailConfirmMatchValidator" ControlToCompare="email" Enabled="true" Visible="true" EnableClientScript="false" /></div>
</div>
<div id="addressDiv" runat="server">
    <div id="AddressLabel1Div" runat="server" class="FormLeft"><Corbis:Localize ID="address1Label" runat="server" meta:resourcekey="address1Label" /></div>
    <div id="Address1Div" runat="server" class="FormRight"><Corbis:TextBox ID="address1" runat="server" ValidateControl="true" ValidationObjectType="Corbis.Membership.Contracts.V1.MemberAddress" ValidationRulesetName="CreateMember" ValidationPropertyName="Address1" ValidatorContainer="validatorContainer"></Corbis:TextBox></div>
    <div id="AddressLabel2Div" runat="server" class="FormLeft"><Corbis:Localize ID="address2Label" runat="server" meta:resourcekey="address2Label" /></div>
    <div id="Address2Div" runat="server" class="FormRight"><Corbis:TextBox ID="address2" runat="server"></Corbis:TextBox></div>
    <div id="AddressLabel3Div" runat="server" class="FormLeft"><Corbis:Localize ID="address3Label" runat="server" meta:resourcekey="address3Label" /></div>
    <div id="Address3Div" runat="server" class="FormRight"><Corbis:TextBox ID="address3" runat="server"></Corbis:TextBox></div>
    <div id="CityLabelDiv" runat="server" class="FormLeft"><Corbis:Localize ID="cityLabel" runat="server" meta:resourcekey="cityLabel" /></div>
    <div id="CityDiv" runat="server" class="FormRight"><Corbis:TextBox ID="city" runat="server" ValidateControl="true" ValidationObjectType="Corbis.Membership.Contracts.V1.MemberAddress" ValidationRulesetName="CreateMember" ValidationPropertyName="City" ValidatorContainer="validatorContainer"></Corbis:TextBox></div>
    <asp:UpdatePanel id="countryStateZip" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="FormLeft"><Corbis:Localize ID="countryLabel" runat="server" meta:resourcekey="countryLabel" /></div>
            <div class="FormRight"><Corbis:DropDownList AutoPostBack="True" ID="country" meta:resourcekey="country" runat="server" ValidateControl="true" ValidationObjectType="Corbis.Membership.Contracts.V1.MemberAddress" ValidationRulesetName="CreateMember" ValidationPropertyName="CountryCode" ValidatorContainer="validatorContainer" OnSelectedIndexChanged="Country_SelectedIndexChanged"></Corbis:DropDownList></div>
            <div class="FormLeft"><Corbis:Localize ID="stateLabel" runat="server" meta:resourcekey="stateLabel" /></div>
            <div class="FormRight"><Corbis:DropDownList Enabled="false" ID="state" runat="server" ValidateControl="true" ValidationObjectType="Corbis.Membership.Contracts.V1.MemberAddress" ValidationRulesetName="CreateMember" ValidationPropertyName="RegionCode" ValidatorContainer="validatorContainer"></Corbis:DropDownList></div>
            <div id="ZipLabelDiv" runat="server" class="FormLeft"><Corbis:Localize ID="zipLabel" runat="server" meta:resourcekey="zipLabel" /></div>
            <div id="ZipDiv" runat="server" class="FormRight"><Corbis:TextBox ID="zip" runat="server" ValidateControl="true" ValidationObjectType="Corbis.Membership.Contracts.V1.MemberAddress" ValidationRulesetName="CreateMember" ValidationPropertyName="PostalCode" ValidatorContainer="validatorContainer"></Corbis:TextBox></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div id="phoneDiv" runat="server">
    <div class="FormLeft"><Corbis:Localize ID="phoneLabel" runat="server" meta:resourcekey="phoneLabel" /></div>
    <div class="FormRight"><Corbis:TextBox ID="phone" runat="server" ValidateControl="true" ValidationObjectType="Corbis.Membership.Contracts.V1.MemberAddress" ValidationRulesetName="CreateMember" ValidationPropertyName="PhoneNumber" ValidatorContainer="validatorContainer"></Corbis:TextBox></div>
</div>
<div class="hidden">
    <asp:PlaceHolder ID="validatorContainer" runat="server"></asp:PlaceHolder>
</div>
