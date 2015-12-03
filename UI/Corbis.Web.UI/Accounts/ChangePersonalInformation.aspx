<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePersonalInformation.aspx.cs" MasterPageFile="~/MasterPages/ModalPopup.Master" Inherits="Corbis.Web.UI.Accounts.ChangePersonalInformation" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<asp:Content ID="MyPersonalInfo" ContentPlaceHolderID="mainContent" runat="server">
<script type="text/javascript" language="javascript">
    window.addEvent('load', function() {
        parent.ResizeIModal('personalInfoModalPopup', GetDocumentHeight());
    })
</script>
<div class="MyPersonalInfoDiv" id="MyPersonalInfoDiv">
    <div class="titleWrapper">
        <span class="title"><Corbis:Localize ID="title" runat="server" meta:resourcekey="title"></Corbis:Localize></span>
        <div class="personalCloseButton" id="personalCloseButton">
            <Corbis:Image ID="closePersonalInfoPopupImage" ImageUrl="/Images/iconClose.gif" runat="server" onclick="parent.MochaUI.CloseModal('personalInfoModalPopup');return false;" class="Close" meta:resourcekey="closePersonalInfoPopupImage"/>
        </div>
    </div>
    <div class="profileContentWrapper">

        <Corbis:ValidationHub ID="vHub" SubmitForm="true"  
            IsIModal="true" runat="server" ContainerID="MyPersonalInfoDiv"
            PopupID="personalInfoModalPopup" 
        />
        <table cellpadding="1" cellspacing="0" class="tableWidth fontSize12">
            <tr runat="server" class="FormRow">
                <td class="LeftForm">
                    <Corbis:Localize ID="usernameLabel" runat="server" Text="<%$ Resources:Accounts, UserNameLabel %>" />
                </td>
                <td class="RightForm">
                    <Corbis:TextBox 
                        ID="username" CssClass="textBoxLength" runat="server" 
                        MaxLength="255" validate="required" 
                        required_message="<%$ Resources:Accounts, MemberValidationError_UserNameRequired %>"
                    />
                </td>
            </tr>
            <tr runat="server" class="FormRow" enableviewstate="false">
                <td class="LeftForm">
                    <Corbis:Localize ID="name1Label" runat="server" />
                </td>
                <td class="RightForm">
                    <Corbis:TextBox 
                        ID="name1" CssClass="textBoxLength" 
                        runat="server" MaxLength="30" validate="required"
                    />
                </td>
            </tr>
            <tr runat="server" class="FormRow" enableviewstate="false">
                <td class="LeftForm">
                    <Corbis:Localize ID="name2Label" runat="server" />
                </td>
                <td class="RightForm">
                    <Corbis:TextBox 
                        ID="name2" CssClass="textBoxLength" runat="server" 
                        MaxLength="30" validate="required"
                    />
                </td>
            </tr>
            <tr id="furiganaLastNameRow" runat="server" class="FormRow">
                <td class="LeftForm">
                    <Corbis:Localize ID="furiganaLastNameLabel" runat="server"  
                     Text="<%$ Resources:Accounts, FuriganaLastNameLabel %>" />
                </td>
                <td class="RightForm">
                    <Corbis:TextBox 
                        ID="furiganaLastName" CssClass="textBoxLength" runat="server" 
                        MaxLength="30" validate="required"
                        required_message="<%$ Resources:Accounts, MemberValidationError_FuriganaLastNameRequired %>"    
                    />
                </td>
            </tr>
            <tr id="furiganaFirstNameRow" runat="server" class="FormRow">
                <td class="LeftForm">
                    <Corbis:Localize ID="furiganaFirstNameLabel" runat="server" 
                     Text="<%$ Resources:Accounts, FuriganaFirstNameLabel %>" />
                </td>
                <td class="RightForm">
                    <Corbis:TextBox
                        ID="furiganaFirstName" CssClass="textBoxLength" runat="server" 
                        MaxLength="30" validate="required" 
                        required_message="<%$ Resources:Accounts, MemberValidationError_FuriganaFirstNameRequired %>"
                    />
                </td>
            </tr>
            <tr runat="server" class="FormRow">
                <td class="LeftForm">
                    <Corbis:Localize ID="emailLabel" runat="server" 
                    Text="<%$ Resources:Accounts, EmailLabel %>" />
                </td>
                <td class="RightForm">
                    <Corbis:TextBox 
                        ID="email" CssClass="textBoxLength" runat="server" 
                        MaxLength="255" validate="required"
                        email_message="<%$ Resources:Accounts, MemberValidationError_InvalidEmailAddress %>" 
                        required_message="<%$ Resources:Accounts, MemberValidationError_EmailAddressRequired %>"    
                    />
                </td>
            </tr>
            <tr runat="server" class="FormRow">
                <td class="LeftForm">
                    <Corbis:Localize ID="confirmEmailLabel" runat="server" 
                     Text="<%$ Resources:Accounts, ConfirmEmailLabel %>" />
                </td>
                <td class="RightForm">
                    <Corbis:TextBox
                        ID="confirmEmail" CssClass="textBoxLength" 
                        runat="server" MaxLength="255" AllowPaste="false"
                        validate="custom1;required" custom1="checkEmailsMatch()" 
                        custom1_message="<%$ Resources:Accounts, MemberValidationError_ConfirmEmailMatch %>" 
                        required_message="<%$ Resources:Accounts, MemberValidationError_ConfirmEmailRequired %>"   
                    />
                    <script>
                        function checkEmailsMatch() {
                            if ($('<%=confirmEmail.ClientID %>').value == '') {
                                return true;
                            }

                            return $('<%=email.ClientID%>').value.toLowerCase() == $('<%=confirmEmail.ClientID %>').value.toLowerCase();
                        }
                    </script>
                </td>
            </tr>
            <tr runat="server" class="FormRow">
                <td class="LeftForm">
                    <Corbis:Localize ID="securityQuestionLabel" runat="server" 
                     Text="<%$ Resources:Accounts, SecurityQuestionLabel %>" />
                </td>
                <td class="RightForm">
                    <Corbis:DropDownList ID="securityQuestion" runat="server" />
                </td>
            </tr>
            <tr runat="server" class="FormRow">
                <td class="LeftForm">
                    <Corbis:Localize ID="answerLabel" runat="server" 
                     Text="<%$ Resources:Accounts, AnswerLabel %>" />
                </td>
                <td class="RightForm">
                    <Corbis:TextBox 
                        ID="securityAnswer" CssClass="textBoxLength" runat="server" 
                        MaxLength="50" validate="required"
                        required_message="<%$ Resources:Accounts, MemberValidationError_PasswordRecoveryAnswerRequired %>"
                    />
                </td>
            </tr>
            <tr class="titleWrapper">
                <td colspan="2" class="titleWrapper">
                    <div class="marginTop"><Corbis:Localize ID="mailingAddressPersonalInfo" runat="server" meta:resourcekey="MailingAddress"></Corbis:Localize></div>
                </td>
            </tr>
            <Corbis:Address ID="mailingAddress" runat="server"
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
                ValidationGroup="ChangePersonalInfoValidationGroup">
            </Corbis:Address>
        </table>
        <div class="profileButtons">
            <Corbis:LinkButton ID="lb" runat="server" OnClick="Save_Click" CssClass="ValidateClickLB displayNone" />
            <Corbis:GlassButton ID="changePersonalInfoSave" runat="server" OnClientClick="return false;" validate="submit" meta:resourceKey="save" CssClass="buttonsSpacing"/>
            <Corbis:GlassButton ButtonStyle="gray" ID="changePersonalInfoCancel" OnClientClick="parent.MochaUI.CloseModal('personalInfoModalPopup');return false;" runat="server" CssClass="buttonsSpacing" meta:resourceKey="cancel"/>
        </div>      
        <div class="displayNone">
            <asp:PlaceHolder ID="validatorContainer" runat="server"></asp:PlaceHolder>
        </div>
    </div>
</div>
<div class="clr"></div>
</asp:Content>
