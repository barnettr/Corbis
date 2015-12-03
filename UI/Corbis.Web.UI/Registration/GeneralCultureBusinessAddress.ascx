<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneralCultureBusinessAddress.ascx.cs" Inherits="Corbis.Web.UI.Registration.GeneralCultureBusinessAddress" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<table class="container">
    <tr class="FormRow" runat="server">
        <td class="FormLeft">
        <Corbis:Localize ID="address1Label" runat="server" Text="<%$ Resources:Accounts, Address1Label %>" />
        </td>
        <td class="FormRight">
         <Corbis:TextBox class="item" validate="required" required_message="<%$ Resources:Accounts, MemberValidationError_BusinessAddress1Required %>" ID="address1" runat="server"/>
        </td>
    </tr>
    
     <tr class="FormRow" runat="server">
        <td class="FormLeft">
        <Corbis:Localize ID="address2Label" runat="server" Text="<%$ Resources:Accounts, Address2Label %>" />
        </td>
        <td class="FormRight">
         <Corbis:TextBox class="item" ID="address2" runat="server"/>
         <AJAXToolkit:TextBoxWatermarkExtender BehaviorID="address2BusinessWatermark" WatermarkCssClass="RegistrationOptional" ID="TextBoxWatermarkExtender1" TargetControlID="address2" WatermarkText="<%$ Resources:Resource, Optional %>" runat="server"/>
        </td>
    </tr>
    
     <tr class="FormRow" runat="server">
        <td class="FormLeft">
        <Corbis:Localize ID="address3Label" runat="server" Text="<%$ Resources:Accounts, Address3Label %>" />
        </td>
        <td class="FormRight">
         <Corbis:TextBox class="item" ID="address3" runat="server"/>
         <AJAXToolkit:TextBoxWatermarkExtender BehaviorID="address3BusinessWatermark" WatermarkCssClass="RegistrationOptional" ID="TextBoxWatermarkExtender2" TargetControlID="address3" WatermarkText="<%$ Resources:Resource, Optional %>" runat="server"/>
        </td>
    </tr>
    
     
     <tr class="FormRow" runat="server">
        <td class="FormLeft">
        <Corbis:Localize ID="cityLabel" runat="server" Text="<%$ Resources:Accounts, CityLabel %>" />
        </td>
        <td class="FormRight">
         <Corbis:TextBox class="item" validate="required" required_message="<%$ Resources:Accounts, MemberValidationError_BusinessCityRequired %>" ID="city" runat="server"/>
        </td>
    </tr>
    
    <tr class="FormRow" runat="server">
        <td class="FormLeft">
        <Corbis:Localize ID="countryLabel" runat="server" Text="<%$ Resources:Accounts, CountryLabel %>" />
        </td>
        <td class="FormRight">
         <Corbis:DropDownList class="item" validate="custom1" custom1="validateBusinessCountry()" custom1_message="<%$ Resources:Accounts, MemberValidationError_BusinessCountryRequired %>" ID="country" EntityType="Country"  runat="server" />
        </td>
    </tr>
    
    <tr class="FormRow" runat="server">
        <td class="FormLeft">
        <Corbis:Localize ID="stateLabel" runat="server" Text="<%$ Resources:Accounts, StateLabel %>"  />
        </td>
        <td class="FormRight">
         <Corbis:DropDownList class="item" validate="custom1" custom1="validateBusinessState()" ID="state" custom1_message="<%$ Resources:Accounts, MemberValidationError_BusinessStateRequired %>" runat="server"  />
        </td>
    </tr>
    
    <tr class="FormRow" runat="server" enableviewstate="false">
        <td class="FormLeft">
        <Corbis:Localize ID="zipLabel" runat="server" Text="<%$ Resources:Accounts, ZipLabel %>" />
        </td>
        <td class="FormRight">
          <Corbis:TextBox class="item" validate="custom1" custom1="validateBusinessZip()" custom1_message="<%$ Resources:Accounts, MemberValidationError_InvalidBillingPostalCode %>" ID="zip" runat="server"/>
        </td>
    </tr>
</table>