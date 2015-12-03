<%-- KEVINH: LEAVE VIEWSTATE ENABLED ON THIS PAGE --%>
<%@ Page Language="C#" AutoEventWireup="True" Codebehind="MyProfile.aspx.cs" Inherits="Corbis.Web.UI.Accounts.MyProfile" 
    MasterPageFile="~/MasterPages/AccountsMaster.Master" Title="<%$ Resources: WindowTitle %>" EnableEventValidation="false"
    EnableViewState="true" %>     
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="AJAXExtentions" Namespace="AjaxControlToolkit.WCSFExtensions" Assembly="AjaxControlToolkit.WCSFExtensions" %>
<%@ Register TagPrefix="Corbis" TagName="ChangeSuccess" Src="~/Accounts/ChangeSuccess.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="RoundCorners" Src="~/Accounts/RoundCorners.ascx" %>

<asp:Content ID="myProfileContent" ContentPlaceHolderID="accountsContent" runat="server">
   
	<div id="ProfileContent">
		<div class="TitleBar">
			<span class="Title"><Corbis:Localize ID="pageTitle" runat="server" meta:resourcekey="pageTitle"></Corbis:Localize></span>
		</div>

	    <%-- SIGN IN AND PERSONAL INFORMATION --%>

        <Corbis:ModalPopup ID="ModalPopup2" meta:resourcekey="passwordSuccessModal" ContainerID="passwordSuccessDiv" runat="server">
            <Corbis:GlassButton ID="pwdSuccessClose" OnClientClick="HideModal('passwordSuccessDiv');return false;" runat="server" meta:resourcekey="Close" />
        </Corbis:ModalPopup>

        <div id="personalInfoPaneHeader" class="PaneHeader"><div class="Right"><div class="Center" onclick="CorbisUI.MyProfile.getPaneDiv(0);">
            <asp:Literal ID="personalTitle" runat="server" meta:resourcekey="personalTitle" />
        </div></div></div>
        
       <div class="PaneContent" id="personalInfoPaneContent">
            <asp:Button ID="updatePersonalInfoPane" runat="server" CausesValidation="false" OnClick="updatePersonalInfoPane_Click" CssClass="hdn" />
            <asp:UpdatePanel ID="updatePersonalInfoPaneUpdatePanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="updatePersonalInfoPane" />
                </Triggers>
                <ContentTemplate>
                    <div class="PaneContentLead"><Corbis:Localize ID="personalInfoMessageLabel" runat="server" meta:resourcekey="onfileMessageLabel" /><Corbis:Hyperlink ID="editPersonalInformationLink" NavigateUrl="javascript:CorbisUI.MyProfile.OpenMyPersonalInformation();" runat="server" CssClass="Edit" meta:resourcekey="editPersonalInformation" /></div>
                    <table cellpadding="1" cellspacing="0">
                        <tr class="FormRow">
                            <td class="FormLeftProfile"><Corbis:Localize ID="usernameLabel" runat="server" meta:resourcekey="usernameLabel" /></td>
	                        <td class="FormRight"><corbis:label ID="username" runat="server" /></td>
	                    </tr>
	                    <tr class="FormRow">
	                        <td class="FormLeftProfile"><Corbis:Localize ID="displayNameLabel" runat="server" meta:resourcekey="displayNameLabel" /></td>
	                        <td class="FormRight">
	                            <corbis:label ID="displayName" runat="server" />
	                        </td>
	                    </tr>
	                    <tr class="FormRow" id="furiganaNameRow" runat="server">
	                        <td class="FormLeftProfile"><Corbis:Localize ID="furiganaNameLabel" runat="server" meta:resourcekey="furiganaNameLabel" /></td>
	                        <td class="FormRight">
	                            <corbis:label ID="furiganaDisplayName" runat="server" />
	                        </td>
	                    </tr>	                    
	                    <tr class="FormRow">
	                        <td class="FormLeftProfile"><Corbis:Localize ID="emailAddressLabel" runat="server" meta:resourcekey="emailAddressLabel" /></td>
	                        <td class="FormRight"><corbis:label ID="emailAddress" runat="server" /></td>
	                    </tr>
	                    <tr class="FormRow">
	                        <td class="FormLeftProfile"><Corbis:Localize ID="mailingAddressLabel" runat="server" meta:resourcekey="mailingAddressLabel" /></td>
	                        <td class="FormRight"><div class="FillAddress"><Corbis:Address ID="mailingAddress" runat="server" DisplayOnly="true" /></div></td>
	                    </tr>
	                </table>
	                <div class="PaneContentLead"><Corbis:Localize ID="securityMessageLabel" runat="server" meta:resourcekey="securityMessageLabel" /></div>
	                <table cellpadding="1" cellspacing="0">
	                    <tr class="FormRow">
                            <td class="FormLeftProfile"><Corbis:Localize ID="securityQuestionLabel" runat="server" meta:resourcekey="securityQuestionLabel" /></td>
	                        <td class="FormRight"><corbis:label ID="securityQuestion" runat="server" /></td>
	                    </tr>
	                    <tr class="FormRow">
	                        <td class="FormLeftProfile"><Corbis:Localize ID="answerLabel" runat="server" meta:resourcekey="answerLabel" /></td>
	                        <td class="FormRight"><corbis:label ID="securityAnswer" runat="server" /></td>
	                    </tr>
	                </table>
    	        </ContentTemplate>
	        </asp:UpdatePanel>
            <div class="IWant">
                <Corbis:HyperLink ID="changePasswordLink" NavigateUrl="javascript:CorbisUI.MyProfile.changePassword()" runat="server" meta:resourcekey="changePasswordModal" />
            </div>
        </div>
        
        <div id="personalInfoPaneDiv" class="PaneContentRound rc5px clear MB_5">
            <div class="Bottom">
                <div class="Left"><div class="Right"><div class="Center">&nbsp;</div></div></div>
            </div>
        </div>

	    <%-- BUSINESS INFORMATION --%>

        <div id="businessInfoPaneHeader" class="PaneHeader"><div class="Right"><div class="Center" onclick="CorbisUI.MyProfile.getPaneDiv(1);">
            <asp:Literal ID="businesssInfoTitle" runat="server" meta:resourcekey="businesssInfoTitle" />
        </div></div></div>

        <div class="PaneContent" id="businessInfoPaneContent">
            <asp:Button ID="updateBusinessInfoPane" runat="server" CausesValidation="false" OnClick="updateBusinessInfoPane_Click" CssClass="hdn" />
            <asp:UpdatePanel ID="businessInfoPane" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="updateBusinessInfoPane" />
                </Triggers>
                <ContentTemplate>
                    <div class="PaneContentLead"><Corbis:Localize ID="businesssInfoMessageLabel" runat="server" meta:resourcekey="onfileMessageLabel" /><Corbis:HyperLink ID="editBusinessInformationLink" NavigateURL="javascript:CorbisUI.MyProfile.OpenEditBusinessInformation();" runat="server" CssClass="Edit" meta:resourcekey="edit" /></div>
                    <table cellpadding="1" cellspacing="0" class="tableBottomMargin">
                        <tr class="FormRow">
                            <td class="FormLeftProfile"><Corbis:Localize ID="companyNameLabel" runat="server" meta:resourcekey="companyNameLabel"></Corbis:Localize></td>
                            <td class="FormRight"><Corbis:Label ID="companyName" runat="server"></Corbis:Label></td>
                        </tr>
                        <tr class="FormRow">
                            <td class="FormLeftProfile"><Corbis:Localize ID="jobTitleLabel" runat="server" meta:resourcekey="jobTitleLabel"></Corbis:Localize></td>
                            <td class="FormRight"><Corbis:Label ID="jobTitle" runat="server"></Corbis:Label></td>
                        </tr>
                        <tr class="FormRow">
                            <td class="FormLeftProfile"><Corbis:Localize ID="telephoneLabel" runat="server" meta:resourcekey="telephoneLabel"></Corbis:Localize></td>
                            <td class="FormRight"><corbis:label ID="telephone" runat="server"></corbis:label></td>
                        </tr>
                        <tr class="FormRow">
                            <td class="FormLeftProfile"><Corbis:Localize ID="streetAddressLabel" runat="server" meta:resourcekey="streetAddressLabel"></Corbis:Localize></td>
                            <td class="FormRight">
                                <div class="FillAddress">
                                    <Corbis:Address ID="businessAddress" runat="server" DisplayOnly="true"
                                        Address1Caption="<%$ Resources: Address1Caption %>"
                                        Address2Caption="<%$ Resources: Address2Caption %>"
                                        Address3Caption="<%$ Resources: Address3Caption %>"
                                        CityCaption="<%$ Resources: CityCaption %>"
                                        CountryCaption="<%$ Resources: CountryCaption %>"
                                        RegionCaption="<%$ Resources: RegionCaption %>"
                                        PostalCodeCaption="<%$ Resources: PostalCodeCaption %>"
                                        RowCssClass="FormRow"
                                        LabelsCssClass="FormLeftProfile"
                                        FormFieldsCssClass="FormRight"/>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
            
        <div id="businessInfoPaneDiv" class="PaneContentRound rc5px clear MB_5">
            <div class="Bottom">
                <div class="Left"><div class="Right"><div class="Center">&nbsp;</div></div></div>
            </div>
        </div>

	    <%-- PAYMENT INFORMATION --%>

        <div id="paymentPane" class="paymentPaneDiv" runat="server">
            <div id="paymentPaneHeader" class="PaneHeader"><div class="Right"><div class="Center"  onclick="CorbisUI.MyProfile.getPaneDiv(2);">
                <asp:Literal ID="paymentInfoHeadTitle" runat="server" meta:resourcekey="paymentInfoHeadTitle" />
            </div></div></div>
            <div class="PaneContent">
                <asp:Button ID="updatePaymentInfoPane" runat="server" CausesValidation="false" OnClick="updatePaymentInfoPane_Click" CssClass="hdn" />

                <asp:UpdatePanel ID="paymentPanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="updatePaymentInfoPane" />
                    </Triggers>
                    <ContentTemplate>
                        <Corbis:LinkButton ID="hiddenAddPaymentAddressLink" CssClass="displayNone" runat="server" meta:resourcekey="addShippingAddress" CausesValidation="false"></Corbis:LinkButton>                                
                        <Corbis:LinkButton ID="dummyPaymentTarget" cssclass="displayNone" runat="server" meta:resourcekey="addLink" CausesValidation="false"></Corbis:LinkButton>
                        <div id="availableCreditDiv" runat="server" class="PaneContentLead">
                            <Corbis:Localize ID="paymentInfoTitle" runat="server" meta:resourcekey="shippingSubheader"></Corbis:Localize>
                        </div>
                        <asp:Repeater ID="creditList" EnableViewState="false" runat="server">
                            <ItemTemplate>
                                <div id="ShippingInfoDetail" class='<%# (bool)DataBinder.Eval(Container.DataItem,"IsDefault") ? "DefaultAddress" : "" %>'>
                                    <div class="Title">
                                        <span class="floatLeft">
                                            <Corbis:Label ID="cardLabel" runat="server" CssClass="bold" Text='<%# (Eval("CardNumberViewable").ToString().Length > 5) ? 
                                            (GetResourceString("Resource",Eval("CreditCardTypeCode").ToString() + "_card") + " " + Eval("CardNumberViewable").ToString().Substring(Eval("CardNumberViewable").ToString().Length - 5)) :  
                                             GetResourceString("Resource",Eval("CreditCardTypeCode").ToString() + "_card") %>'></Corbis:Label>
                                            <Corbis:Localize ID="preferredLabel" runat="server" meta:resourcekey="Preferred" Visible='<%# (bool)DataBinder.Eval(Container.DataItem,"IsDefault") %>'></Corbis:Localize>
                                        </span>
                                        <span class="floatRight">
                                            <Corbis:Hyperlink ID="edit" runat="server" CssClass="Edit" meta:resourcekey="editLink" 
                                                NavigateUrl='<%# "javascript:CorbisUI.MyProfile.OpenEditPaymentInformation(\u0027" + Eval("CreditCardUid") + "\u0027);" %>'
                                                /><span class="Delete"><Corbis:ImageButton ID="delete" runat="server" OnClientClick='<%# "CorbisUI.MyProfile.OpenEditPaymentInformation(\u0027" + Eval("CreditCardUid") + "\u0027, \u0027delete\u0027);" %>' CausesValidation="false" ImageUrl="/Images/iconDelete.gif"></Corbis:ImageButton></span>
                                        </span>
                                    </div>
                                    <div class="Detail">
                                        <div class="clear"></div>
                                        <table cellpadding="1" cellspacing="0">
                                            <tr class="FormRow">
                                                <td class="FormLeftProfile"><Corbis:Localize ID="expirationDateTitle" runat="server" meta:resourcekey="expirationDateTitle"></Corbis:Localize></td>
                                                <td class="FormRightPayment"><Corbis:Label ID="expirationDateResult" runat="server" Text='<%#Eval("ExpirationDate")%>' ></Corbis:Label></td>
                                            </tr>
                                            <tr class="FormRow">
                                                <td class="FormLeftProfile"><Corbis:Localize ID="nameOfCardholderTitle" runat="server" meta:resourcekey="nameofCardholderTitle"></Corbis:Localize></td>
                                                <td class="FormRightPayment"><Corbis:Label ID="nameOfCardholderResult" runat="server" Text='<%#(Eval("NameOnCard")==null)? "" : Server.HtmlEncode(Eval("NameOnCard").ToString())%>' ></Corbis:Label></td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                           </ItemTemplate>
                        </asp:Repeater>
                        <div id="emptyCreditDiv" runat="server" class="Empty">
                            <div class="FormRow"><div class="FormLeftProfile alignLeft weightNormal">
                                <Corbis:Localize ID="emptyCredit" runat="server" meta:resourceKey="emptyCreditResult"></Corbis:Localize>
                            </div></div>
                        </div>
                        <div class="IWantShipping" id="IWantShipping">
                            <Corbis:Hyperlink ID="linkAddNewCreditCard" runat="server" meta:resourcekey="addLink" NavigateUrl="javascript:CorbisUI.MyProfile.OpenEditPaymentInformation();" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div id="paymentPaneDiv" class="PaneContentRound rc5px clear MB_5">
                <div class="Bottom">
                    <div class="Left"><div class="Right"><div class="Center">&nbsp;</div></div></div>
                </div>
            </div>
        </div>
        
        <%-- PREFERENCES --%>
    
        <div id="preferencesPaneHeader" class="PaneHeader"><div class="Right"><div class="Center" onclick="CorbisUI.MyProfile.getPaneDiv(3);">
            <asp:Literal ID="preferencesHeader" runat="server" meta:resourcekey="preferencesHeader"/>
        </div></div></div>

        <div class="PaneContent">
            <div id="changeSuccessDiv" class="hdn">
                <Corbis:ChangeSuccess ID="changeSuccess" runat="server"></Corbis:ChangeSuccess>
            </div>

            <asp:Button ID="updatePreferencesPane" runat="server" CausesValidation="false" OnClick="updatePreferencesPane_Click" CssClass="hdn" />
            <asp:UpdatePanel id="PreferencesUpdatePanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional"  >
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="updatePreferencesPane" />
                </Triggers>
                <ContentTemplate>
                    <div id="PreferencesDetail">
                        <div class="PaneContentLead"><asp:Literal ID="emailPreferencesHeader" runat="server" meta:resourcekey="emailPreferencesHeader"/></div>
                        <div class="EmailPreferences">
                            <div class="EmailOptInControls">
                                
                                <div><Corbis:RadioButton ID="sendPromoEmail" runat="server"  GroupName="Email" meta:resourcekey="sendPromoEmail" /></div>
                                 <div><Corbis:RadioButton ID="doNotSendPromoEmail" runat="server" GroupName="Email" meta:resourcekey="doNotSendPromoEmail" /></div>
                            </div>
                        </div>
                        <table cellpadding="1" cellspacing="0" style="width:100%;">
	                        <tr class="FormRow">
                                <td class="FormLeftProfile"><Corbis:Localize ID="emailLanguageLabel" runat="server" meta:resourcekey="emailLanguageLabel" /></td>
	                            <td class="FormRightEmailTop">
	                                <div class="paddingBottom6"><Corbis:DropDownList ID="emailLanguage" runat="server"></Corbis:DropDownList></div>
	                            </td>
	                        </tr>
	                        <tr class="FormRow">
	                            <td class="FormLeftProfile"><Corbis:Localize ID="emailFormatLabel" runat="server" meta:resourcekey="emailFormatLabel" /></td>
	                            <td class="FormRightEmail">
	                                <div class="paddingBottom6"><Corbis:DropDownList ID="emailFormat" runat="server"></Corbis:DropDownList></div>
	                            </td>
	                        </tr>
	                    </table>
                        <div class="PaneContentLead"><asp:Literal ID="directMailPreferencesHeaderText" runat="server" meta:resourcekey="directMailPreferencesHeader" /></div>
                        <div class="DirectMailPreferences">
                            <div class="DirectMailCheckbox">
                                <div class="imgContainer"><Corbis:ImageCheckbox ID="snailmailPreference" CssClass="prefCheckbox" runat="server" meta:resourcekey="snailMailPreferenceText"></Corbis:ImageCheckbox></div>
                            </div>
                        </div>
                        <div id="paymentAndShippingPreferences" runat="server">
                            <div class="PaneContentLead"><asp:Literal ID="paymentAndShippingPreferencesHeader" runat="server" meta:resourcekey="paymentAndShippingHeader"/></div>
                            <div class="PaymentAndShippingPreferences">
                                <div class="Preferred">
                                    <div class="Payment">
                                        <table cellpadding="5" cellspacing="0" border="0" style="width:110%;">
                                            <tr id="preferredPaymentMethodDiv" runat="server">
                                                <td style="border:solid 1px #dbdbdb;" class="alignRightPreference"><asp:Literal ID="preferredPaymentMethodText" runat="server" meta:resourcekey="preferredPaymentMethod"/></td>
                                                <td style="border:solid 1px #dbdbdb;" class="PaymentRight"><Corbis:DropDownList ID="preferredPaymentMethod" runat="server" /></td>
                                            </tr>
                                            <tr id="preferredCorporateAccountDiv" runat="server">
                                                <td style="border:solid 1px #dbdbdb;" class="alignRightPreference"><asp:Literal ID="preferredCorporateAccountText" runat="server" meta:resourcekey="preferredCorporateAccount"/></td>
                                                <td style="border:solid 1px #dbdbdb;" class="PaymentRight"><Corbis:DropDownList ID="preferredCorporateAccount" runat="server" /></td>
                                            </tr>
                                            <tr id="preferredCreditCardDiv" runat="server">
                                                <td style="border:solid 1px #dbdbdb;" class="alignRightPreference"><asp:Literal ID="preferredCreditCardText" runat="server" meta:resourcekey="preferredCreditCard"/></td>
                                                <td style="border:solid 1px #dbdbdb; border-right:solid 1px #e8e8e8;" class="PaymentRight"><Corbis:DropDownList ID="preferredCreditCard" runat="server" /></td>
                                            </tr>
                                            <tr id="expressCheckoutDiv" runat="server">
                                                <td style="border:solid 1px #dbdbdb;" class="alignRightPreference"><asp:Literal ID="expressCheckoutText" runat="server" meta:resourcekey="expressCheckout" /></td>
                                                <td style="border:solid 1px #dbdbdb; border-right:solid 1px #e8e8e8;" class="PaymentRight">
                                                    <Corbis:DropDownList ID="expressCheckout" runat="server">
                                                        <asp:ListItem meta:resourcekey="expressCheckoutOn" Value="true" />
                                                        <asp:ListItem meta:resourcekey="expressCheckoutOff" Value="false" />
                                                    </Corbis:DropDownList>
                                                    <img src="/Images/spacer.gif" runat="server" class="thumbWrap expressWrap" meta:resourcekey="ExpressCheckOuttooltip"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="preferredShippingAddressDiv" runat="server" class="Shipping shipBold">
                                        <asp:Literal ID="preferredShippingAddressText" runat="server" meta:resourcekey="preferredShippingAddress"/>
                                        <Corbis:DropDownList ID="preferredShippingAddress" runat="server" />
                                    </div>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </div>
                        <div class="PreferencesButtons">
                            <Corbis:GlassButton 
                                ID="savePreference" runat="server" CausesValidation="true" 
                                meta:resourcekey="save" ValidationGroup="PaymentInfo"
                                OnClick="SavePreference_Click"
                            />
                            <%--<Corbis:GlassButton 
                                ID="cancelPreference" runat="server" CausesValidation="false" 
                                ButtonStyle="gray" meta:resourcekey="cancel"
                            />--%>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="preferencesPaneDiv" class="PaneContentRound rc5px clear">
            <div class="Bottom">
                <div class="Left"><div class="Right"><div class="Center">&nbsp;</div></div></div>
            </div>
        </div>
		
		<div id="personalInfoModalPopup" runat="server" class="hdn">
        </div>
        
        <div id="editPaymentInfoModalPopup" runat="server" class="hdn">
        </div>
        
        <div id="editMailingAddress" runat="server" class="hdn">
        </div>
        
        <Corbis:ModalPopup ID="ModalPopup1" ContainerID="getSuccess" runat="server" Width="300" meta:resourcekey="getSuccess">
            <Corbis:Label ID="getMessage" runat="server" CssClass="getMessage" meta:resourcekey="getSuccessMessage"></Corbis:Label>
            <br />
            <Corbis:GlassButton ID="GlassButton1" runat="server" CausesValidation="false" meta:resourcekey="Close" OnClientClick="HideModal('getSuccess');return false;" /> 
        </Corbis:ModalPopup>
        
        <Corbis:ModalPopup ID="registerSuccessDiffCountry" Width="350" ContainerID="registerSuccessDiffCountry" runat="server"  meta:resourcekey="successTitle" CloseScript="HideModal('registerSuccessDiffCountry');">
       <div style="padding-top:22px">
        <Corbis:Localize ID="successMessageDiff" runat="server" meta:resourcekey="successMessage" />
       
        <div style="padding-top:10px;">
        <Corbis:Image ID="alertImage" ImageUrl="/Images/alertYellow.gif" runat="server"/>
        <Corbis:Localize ID="warningDifferentCountry" runat="server" meta:resourcekey="warningDifferentCountry" />
        <Corbis:HyperLink ID="contactCorbis" runat="server"  meta:resourcekey="contactCorbis" /> 
        <Corbis:Localize ID="warningDifferentCountryEnd" runat="server" meta:resourcekey="warningDifferentCountryEnd" />
        </div>
        </div>
		<Corbis:GlassButton CssClass="closeSuccess"  ID="closeSuccessDiffCountry" runat="server" OnClientClick="javascript:HideModal('registerSuccessDiffCountry');return false;" ButtonStyle="Orange" ButtonBackground="dbdbdb" meta:resourcekey="closeButton" />
		
	</Corbis:ModalPopup>
        
	</div>
	<script type="text/javascript">
	    CorbisUI.MyProfile.vars.updateBusinessInfoPaneButton = $('<%=updateBusinessInfoPane.ClientID %>');
	    CorbisUI.MyProfile.vars.updatePersonalInfoPaneButton = $('<%=updatePersonalInfoPane.ClientID %>');
	    CorbisUI.MyProfile.vars.updatePreferencesPaneButton = $('<%=updatePreferencesPane.ClientID %>');
	    CorbisUI.MyProfile.vars.updatePaymentPaneButton = $('<%=updatePaymentInfoPane.ClientID %>');

	    var myAccordian;

	    window.addEvent('domready', function() {
	        //create our Accordion instance
	        myAccordion = new Accordion($('ProfileContent'), 'div.PaneHeader', 'div.PaneContent', {
	            opacity: false,
	            display: CorbisUI.MyProfile.GetPane()
	        });
	        CorbisUI.MyProfile.getPaneDiv(CorbisUI.MyProfile.GetPane());
	    });

	    window.addEvent('load', function() {
	        CorbisUI.MyProfile.registerTooltips(false);
	    });
	</script>
</asp:Content>
