<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneralAddress.ascx.cs" Inherits="Corbis.Web.UI.Browse.Controls.GeneralAddress" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<table class="container">
    <tr class="FormRow">
        <td class="FormLeft">
        <Corbis:Localize ID="address1Label" runat="server" meta:resourcekey="address1Label" />
        </td>
        <td class="FormRight">
         <Corbis:TextBox class="item" validate="required" meta:resourcekey="address1" ID="address1" runat="server"/>
        </td>
    </tr>
    
     <tr class="FormRow">
        <td class="FormLeft">
        <Corbis:Localize ID="address2Label" runat="server" meta:resourcekey="address2Label" />
        
        </td>
        <td class="FormRight">
         <Corbis:TextBox class="item" ID="address2" runat="server"/>
         <AJAXToolkit:TextBoxWatermarkExtender BehaviorID="address2MailingWatermark" WatermarkCssClass="RegistrationOptional" ID="address2LabelWatermarker"  TargetControlID="address2" meta:resourcekey="address2LabelWaterMaker" runat="server"/>
        </td>
    </tr>
    
     <tr class="FormRow">
        <td class="FormLeft">
        <Corbis:Localize ID="address3Label" runat="server" meta:resourcekey="address3Label" />
        </td>
        <td class="FormRight">
         <Corbis:TextBox class="item" ID="address3" runat="server"/>
          <AJAXToolkit:TextBoxWatermarkExtender BehaviorID="address3MailingWatermark" WatermarkCssClass="RegistrationOptional" ID="TextBoxWatermarkExtender1" TargetControlID="address3" meta:resourcekey="address2LabelWaterMaker" runat="server"/>
        </td>
    </tr>
    
     
     <tr class="FormRow">
        <td class="FormLeft">
        <Corbis:Localize ID="cityLabel" runat="server" meta:resourcekey="cityLabel" />
        </td>
        <td class="FormRight">
         <Corbis:TextBox class="item" validate="required" meta:resourcekey="city" ID="city" runat="server"/>
        </td>
    </tr>
    
    <tr class="FormRow">
        <td class="FormLeft">
        <Corbis:Localize ID="countryLabel" runat="server" meta:resourcekey="countryLabel" />
        </td>
        <td class="FormRight">
         <Corbis:DropDownList class="item" validate="custom1" custom1="validateMailingCountry()" meta:resourcekey="country" ID="country" EntityType="Country"  runat="server" />
        </td>
    </tr>
    
    <tr class="FormRow">
        <td class="FormLeft">
        <Corbis:Localize ID="stateLabel" runat="server" meta:resourcekey="stateLabel" />
        </td>
        <td class="FormRight">
         <Corbis:DropDownList class="item" validate="custom1" custom1="validateMailingState()" ID="state" meta:resourcekey="state" runat="server"  />
        </td>
    </tr>
    
    <tr class="FormRow">
        <td class="FormLeft">
        <Corbis:Localize ID="zipLabel" runat="server" meta:resourcekey="zipLabel" />
        </td>
        <td class="FormRight">
          <Corbis:TextBox class="item" validate="custom1" custom1="validateMailingZip()" meta:resourcekey="zip" ID="zip" runat="server"/>
        </td>
    </tr>
</table>