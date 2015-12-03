<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JapaneseCulture.ascx.cs" Inherits="Corbis.Web.UI.Registration.JapaneseCulture" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet" Namespace="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet" TagPrefix="Microsoft" %>
<%@ Register TagPrefix="AJAXExtentions" Namespace="AjaxControlToolkit.WCSFExtensions" Assembly="AjaxControlToolkit.WCSFExtensions" %>
    <div class="FormRow"><div class="FormLeft">
        <Corbis:Localize ID="japaneseLastNameLabel" runat="server" meta:resourcekey="japaneseLastNameLabel" /></div>
    <div class="FormRight">
        <div class="FormRightLeft">
            <Corbis:Localize ID="kanjiLastNameLabel" runat="server" meta:resourcekey="kanjiLastNameLabel" />
        </div>
        <Corbis:TextBox ID="lastName" runat="server" ValidateControl="true" ValidationObjectType="Corbis.Membership.Contracts.V1.Member" ValidationRulesetName="CreateMember" ValidationPropertyName="LastName" ValidatorContainer="lastNameContainer" />
        <asp:PlaceHolder ID="lastNameContainer" runat="server"/>
        <div class="FormRightLeft">
            <Corbis:Localize ID="furiganaLastNameLabel" runat="server" meta:resourcekey="furiganaLastNameLabel" />
        </div>
        <Corbis:TextBox ID="furiganaLastName" runat="server" ValidateControl="true" ValidationObjectType="Corbis.Membership.Contracts.V1.Member" ValidationRulesetName="CreateMember" ValidationPropertyName="FuriganaLastName" ValidatorContainer="lastNameFuriganaContainer" ></Corbis:TextBox>
		<asp:PlaceHolder ID="lastNameFuriganaContainer" runat="server"/>
    </div></div>
    <div class="FormRow"><div class="FormLeft">
        <Corbis:Localize ID="japaneseFirstNameLabel" runat="server" meta:resourcekey="japaneseFirstNameLabel" /></div>
    <div class="FormRight">
        <div class="FormRightLeft">
            <Corbis:Localize ID="kanjiFirstNameLabel" runat="server" meta:resourcekey="kanjiFirstNameLabel" />
        </div>
        <Corbis:TextBox ID="firstName" runat="server" ValidateControl="true" ValidationObjectType="Corbis.Membership.Contracts.V1.Member" ValidationRulesetName="CreateMember" ValidationPropertyName="FirstName" ValidatorContainer="firstNameContainer" />
        <asp:PlaceHolder ID="firstNameContainer" runat="server"/>
        <div class="FormRightLeft">
            <Corbis:Localize ID="furiganaFirstNameLabel" runat="server" meta:resourcekey="furiganaFirstNameLabel" />
        </div>
        <Corbis:TextBox ID="furiganaFirstName" runat="server" ValidateControl="true" ValidationObjectType="Corbis.Membership.Contracts.V1.Member" ValidationRulesetName="CreateMember" ValidationPropertyName="FuriganaFirstName" ValidatorContainer="firstNameFuriganaContainer" ></Corbis:TextBox>
        <asp:PlaceHolder ID="firstNameFuriganaContainer" runat="server"/>
    </div></div>
    <div class="FormRow"><div class="FormLeft">
        <Corbis:Localize ID="companyNameLabel" runat="server" meta:resourcekey="companyNameLabel" /></div>
    <div class="FormRight">
        <Corbis:TextBox ID="companyName" runat="server" ValidateControl="true" ValidationObjectType="Corbis.Membership.Contracts.V1.MemberAddress" ValidationRulesetName="CreateMember" ValidationPropertyName="CompanyName" ValidatorContainer="companyNameValidatorContainer" />
        <asp:PlaceHolder ID="companyNameValidatorContainer" runat="server"/>
    </div></div>
    <asp:UpdatePanel id="countryState" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="FormRow"><div class="FormLeft">
                <Corbis:Localize ID="countryLabel" runat="server" meta:resourcekey="countryLabel" /></div>
            <div class="FormRight">
                <Corbis:DropDownList AutoPostBack="True" ID="country" EntityType="Country" meta:resourcekey="country" runat="server" ValidateControl="true" ValidationObjectType="Corbis.Membership.Contracts.V1.MemberAddress" ValidationRulesetName="CreateMember" ValidationPropertyName="CountryCode" ValidatorContainer="countryValidatorContainer" OnSelectedIndexChanged="Country_SelectedIndexChanged" />
                <asp:PlaceHolder ID="countryValidatorContainer" runat="server"/>
            </div></div>
            <div class="FormRow"><div class="FormLeft">
                <Corbis:Localize ID="zipLabel" runat="server" meta:resourcekey="zipLabel" /></div>
            <div class="FormRight">
                <Corbis:TextBox ID="zip" runat="server" ValidateControl="true" ValidationObjectType="Corbis.Membership.Contracts.V1.MemberAddress" ValidationRulesetName="CreateMember" ValidationPropertyName="PostalCode" ValidatorContainer="zipValidatorContainer" />
				<asp:PlaceHolder ID="zipValidatorContainer" runat="server"/>
			</div></div>
            <div class="FormRow"><div class="FormLeft">
                <Corbis:Localize ID="stateLabel" runat="server" meta:resourcekey="stateLabel" /></div>
            <div class="FormRight">
				<!-- Have to disable ajax extender due to unexpected side affect cause by dynamic behaviour of this dropdown-->
                <Corbis:DropDownList Enabled="false" ID="state" meta:resourcekey="state" runat="server" ValidateControl="true" ValidationObjectType="Corbis.Membership.Contracts.V1.MemberAddress" ValidationRulesetName="CreateMember" ValidationPropertyName="RegionCode" ValidatorContainer="stateValidatorContainer" />
                <asp:PlaceHolder ID="stateValidatorContainer" runat="server"/>
            </div></div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="FormRow"><div class="FormLeft">
        <Corbis:Localize ID="cityLabel" runat="server" meta:resourcekey="cityLabel" /></div>
    <div class="FormRight">
        <Corbis:TextBox ID="city" runat="server" ValidateControl="true" ValidationObjectType="Corbis.Membership.Contracts.V1.MemberAddress" ValidationRulesetName="CreateMember" ValidationPropertyName="City" ValidatorContainer="cityValidatorContainer" />
		<asp:PlaceHolder ID="cityValidatorContainer" runat="server"/>
	</div></div>
    <div class="FormRow"><div class="FormLeft">
        <Corbis:Localize ID="address1Label" runat="server" meta:resourcekey="address1Label" /></div>
    <div class="FormRight">
        <Corbis:TextBox ID="address1" runat="server" ValidateControl="true" ValidationObjectType="Corbis.Membership.Contracts.V1.MemberAddress" ValidationRulesetName="CreateMember" ValidationPropertyName="Address1" ValidatorContainer="address1ValidatorContainer" />
		<asp:PlaceHolder ID="address1ValidatorContainer" runat="server"/>
	</div></div>
    <div class="FormRow"><div class="FormLeft">
        <Corbis:Localize ID="address2Label" runat="server" meta:resourcekey="address2Label" /></div>
    <div class="FormRight">
        <Corbis:TextBox ID="address2" runat="server"></Corbis:TextBox></div></div>
