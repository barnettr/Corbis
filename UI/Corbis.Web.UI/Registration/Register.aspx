<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Register.aspx.cs" Inherits="Corbis.Web.UI.Registration.Register" MasterPageFile="~/MasterPages/MasterBase.Master" Title="<%$ Resources: windowTitle %>" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<%@ Register Assembly="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet" Namespace="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet" TagPrefix="Microsoft" %>

<asp:Content ID="registerContent" ContentPlaceHolderID="mainContent" runat="server">
    
    <div id="RegistrationRound" class="rc5px clear">
        <div class="Top">
            <div class="Left"><div class="Right"><div class="Center">&nbsp;</div></div></div>
        </div>
    </div>
    <div id="mainRegistrationContent">
    <div style="padding:2px;">
     <div class="LeftAccordion">
    <div id="RegisterContent" >
          <div class="PaneHeaderExpanded">
                <div id="officesPaneHeader">
                    <div class="Right">
                        <div class="Center">
                           <div class="title"><Corbis:Localize ID="pageTitle"  runat="server" meta:resourcekey="pageTitle" /></div>
                        </div>
                    </div>
                </div>
         </div>
        
        <div id="RegisterContentUserInput" >
        <div class="captionClass" ><Corbis:Localize ID="caption" runat="server" meta:resourcekey="caption" /></div>
        
        <div class="PaneContent" id="validationPane">
            
            <Corbis:ValidationHub 
                ID="vHub" runat="server" IsPopup="false" ContainerID="validationPane"
                IsIModal="false" SubmitForm="true" 
            />
            
            <div class="subHeadings"><Corbis:Localize ID="SignInCaption" runat="server" meta:resourcekey="SignInCaption" /></div>
      
         <table cellspacing="0" class="container">
          <tr class="FormRow" runat="server"  enableviewstate="false">
            <td class="FormLeft">
             <Corbis:Localize ID="usernameLabel" runat="server" Text="<%$ Resources:Accounts, UserNameLabel %>" />
            </td>
            <td class="FormRight">
            <Corbis:TextBox class="item" validate="required" ID="username"  required_message="<%$ Resources:Accounts, MemberValidationError_UserNameRequired %>" runat="server"/>
            </td>
          </tr>
          
          <tr class="FormRow" runat="server" enableviewstate="false">
            <td class="FormLeft">
              <Corbis:Localize ID="passwordLabel" runat="server" Text="<%$ Resources:Accounts, PasswordLabel %>" />
            </td>
            <td class="FormRight">
             <Corbis:TextBox MaxLength="20" class="item" validate="custom1;required" custom1="validatePassword();" ID="password" TextMode="Password"  required_message="<%$ Resources:Accounts, MemberValidationError_PasswordRequired %>" custom1_message="<%$ Resources:Accounts, MemberValidationError_PasswordCustom1 %>" runat="server"/>
                <img alt="" src="/Images/spacer.gif" title='<Corbis:Localize ID="PasswordHelpTitle" runat="server" meta:resourcekey="PasswordHelpTitle"/>'
                                                        rel='<Corbis:Localize ID="PasswordHelpInfo" runat="server" meta:resourcekey="PasswordHelpInfo"/>'
                                                        class="thumbWrap expressWrap" />
                
            </td>
          </tr>
         
          <tr class="FormRow" runat="server" enableviewstate="false">
            <td class="FormLeft">
             <Corbis:Localize ID="confirmPasswordLabel" runat="server" Text="<%$ Resources:Accounts, ConfirmPasswordLabel %>" />
            </td>
            <td class="FormRight">
             <Corbis:TextBox MaxLength="20" class="item" validate="custom1;required" custom1="passwordsShouldMatch()" ID="confirmPassword" TextMode="Password" runat="server" required_message="<%$ Resources:Accounts, MemberValidationError_PasswordConfirmRequired %>" custom1_message="<%$ Resources:Accounts, MemberValidationError_PasswordConfirmCustom1 %>" AllowPaste="false"></Corbis:TextBox>
            </td>
          </tr>
          
          <tr class="FormRow" runat="server">
            <td class="FormLeft">
             <Corbis:Localize ID="securityQuestionLabel" runat="server" Text="<%$ Resources:Accounts, SecurityQuestionLabel %>" />
            </td>
            <td class="FormRight">
            <Corbis:DropDownList  class="item" validate="custom1" custom1="validateSecurityQuestion()" ID="securityQuestion" EntityType="SecurityQuestion" custom1_message="<%$ Resources:Accounts, MemberValidationError_PasswordRecoveryQuestionRequired %>" runat="server" />
             <img alt="" src="/Images/spacer.gif" title='<Corbis:Localize ID="securityQuestionHelpTitle" runat="server" meta:resourcekey="securityQuestionHelpTitle"/>'
                                                        rel='<Corbis:Localize ID="securityQuestionHelpLabel" runat="server" meta:resourcekey="securityQuestionHelpLabel"/>'
                                                        class="thumbWrap expressWrap" />
            </td>
          </tr>
          
            <tr class="FormRow" runat="server">
            <td class="FormLeft">
             <Corbis:Localize ID="answerLabel" runat="server" Text="<%$ Resources:Accounts, AnswerLabel %>" />
            </td>
            <td class="FormRight">
            <Corbis:TextBox  class="item" validate="required" ID="answer" runat="server"  required_message="<%$ Resources:Accounts, MemberValidationError_PasswordRecoveryAnswerRequired %>" ></Corbis:TextBox>
            </td>
          </tr>
          </table>
         
          <div class="subHeadings"><Corbis:Localize ID="personalProfile" runat="server" meta:resourcekey="personalProfile" /></div>
          
          <asp:Panel runat="server" ID="namePanel"/>
          <table cellspacing="0"  class="container">
           <tr class="FormRow" runat="server" enableviewstate="false">
            <td class="FormLeft">
              <Corbis:Localize ID="emailLabel"  runat="server" Text="<%$ Resources:Accounts, EmailLabel %>" />
            </td>
            <td class="FormRight">
            <Corbis:TextBox class="item" validate="required;email" ID="email" email_message="<%$ Resources:Accounts, MemberValidationError_InvalidEmailAddress %>" required_message="<%$ Resources:Accounts, MemberValidationError_EmailAddressRequired %>" runat="server"/>
            </td>
          </tr>
          
           <tr class="FormRow" runat="server" enableviewstate="false">
            <td class="FormLeft">
              <Corbis:Localize ID="confirmEmailLabel" runat="server" Text="<%$ Resources:Accounts, ConfirmEmailLabel %>" />
            </td>
            <td class="FormRight">
            <Corbis:TextBox class="item" validate="custom1;required" custom1="emailsShouldMatch()" ID="confirmEmail" runat="server" custom1_message="<%$ Resources:Accounts, MemberValidationError_ConfirmEmailMatch %>" required_message="<%$ Resources:Accounts, MemberValidationError_ConfirmEmailRequired %>" AllowPaste="false"></Corbis:TextBox>
            </td>
          </tr>
          </table>
          
            <div class="subHeadings"><Corbis:Localize ID="mailingAddress" runat="server" meta:resourcekey="mailingAddress" /></div>
            <asp:Panel runat="server" ID="mailingAddressPanel" />
             <table cellspacing="0" class="container">
           <tr class="FormRow" runat="server">
            <td class="FormLeft">
             <Corbis:Localize ID="languageLabel" runat="server" Text="<%$ Resources:Accounts, LanguageLabel %>" />
            </td>
            <td class="FormRight">
             <Corbis:DropDownList class="item" ID="language" runat="server" meta:resourcekey="language" />
            </td>
          </tr>
          </table>
           <div class="subHeadings"><Corbis:Localize ID="businessInformation" runat="server" meta:resourcekey="businessInformation" /></div>
           
          <table class="container">
          
           <tr class="FormRow" runat="server">
            <td class="FormLeft">
              <Corbis:Localize ID="companyNameLabel" runat="server" Text="<%$ Resources:Accounts, CompanyLabel %>" />
            </td>
            <td class="FormRight">
             <Corbis:TextBox class="item" validate="required" required_message="<%$ Resources:Accounts, MemberValidationError_CompanyNameRequired %>" ID="companyName" runat="server"></Corbis:TextBox>
            </td>
          </tr>
          
           <tr class="FormRow" runat="server">
            <td class="FormLeft">
              <Corbis:Localize ID="jobTitleLabel" runat="server" Text="<%$ Resources:Accounts, JobTitleLabel %>" />
            </td>
            <td class="FormRight">
              <Corbis:DropDownList class="item" validate="custom1" custom1="validateJobTitle()" ID="jobTitle" custom1_message="<%$ Resources:Accounts, MemberValidationError_JobTitleRequired %>" runat="server">
                </Corbis:DropDownList>
            </td>
          </tr>
          
            <tr class="FormRow" runat="server">
            <td class="FormLeft">
              <Corbis:Localize ID="phoneLabel" runat="server" Text="<%$ Resources:Accounts, PhoneLabel %>" />
            </td>
            <td class="FormRight">
             <Corbis:TextBox class="item" validate="required" required_message="<%$ Resources:Accounts, MemberValidationError_BusinessPhoneRequired %>" ID="phone" runat="server" />
            </td>
          </tr>
          
          
        
               
               
         </table>
         <div class="subHeadings"><Corbis:Localize ID="businessAddress" runat="server" meta:resourcekey="businessAddress" /></div>
        <div class="FormRow" style="padding-left:10px;">
                   <Corbis:ImageCheckbox ID="sameAsMailingAddress" OnClientChanged="javascript:CopyMailingToBusinessAddress();" meta:resourcekey="sameAsMailingAddress"  runat="server" />
        </div>
           
         <asp:Panel runat="server" ID="businessAddressPanel" />
       <div class="subHeadings"><Corbis:Localize ID="promoComm" runat="server" meta:resourcekey="promoComm" /></div>
       
       
       
    
   
        <div class="FormRow"><div class="FormRight" style="width:550px">
            <Corbis:ImageCheckbox ID="sendEmail" Checked="true" meta:resourcekey="sendEmail"  runat="server" />
            </div>
            </div>
         
         
         <table cellspacing="0" class="container">
         <tr class="FormRow" runat="server">
            <td class="FormLeft indentLeft">
              <Corbis:Localize ID="emailFormatLabel" runat="server" meta:resourcekey="emailFormatLabel" />
            </td>
            <td class="FormRight indentLeft">
             <Corbis:DropDownList ID="emailFormat" runat="server" meta:resourcekey="emailFormat" />
            </td>
          </tr>
          </table>
      

        <div class="FormRow"><div class="FormRight" style="width:550px">
            <Corbis:ImageCheckbox ID="sendSnailMail" Checked="true" meta:resourcekey="sendSnailMail"  runat="server" />
        </div></div>
        
        
        <div class="FormRow" style="height:40px">
            
        
            <div class="FormRight" style="width:420px;float:left;">
                <Corbis:ImageCheckbox ID="accept" OnClientChanged="javascript:enableDisableRegisterButton();" validate="true" meta:resourcekey="accept"  runat="server" />
            </div>
            
            <div style="margin-left:440px">
            <Corbis:HyperLink ID="truste"  class="TRUSTe" Localize="true" ImageUrl="/Images/en-US/truste.gif"  runat="server" />
            </div>
            
        </div>
         <div class="FormRow"><div class="FormRight">
            
             <Corbis:GlassButton ID="register"  runat="server" 
                            meta:resourcekey="register"  OnClientClick="javascript:submitRegistrationForm(); return false;"  CssClass="buttonsSpacing" 
                        />
                        <Corbis:LinkButton ID="lb" runat="server" OnClick="Register_Click"  CssClass="ValidateClickLB displayNone" />
            <Corbis:GlassButton ButtonStyle="Gray" ID="cancel" 
                CausesValidation="false" meta:resourcekey="cancel" CssClass="buttonsSpacing"
                runat="server" 
            />    
        </div>
    </div>
    </div>
        
    </div>
         <div class="PaneContentRound rc5px clear MB_20">
                <div class="Bottom">
                    <div class="Left"><div class="Right"><div class="Center">&nbsp;</div></div></div>
                </div>
         </div>
       
    </div>
    </div>
    <div class="RightAccordion">
    <div id="whyRegister">
        <div class="PaneHeaderExpanded">
                <div id="Div1">
                    <div class="Right">
                        <div class="Center">
                          <div class="whyRegisterTitle"><Corbis:Localize ID="whyRegisterTitle"  runat="server" meta:resourcekey="whyRegisterTitle" />
                          </div>   
                        </div>
                    </div>
                </div>
         </div>
         
        
  
        <div class="whyRegisterContent">
         <div style="padding:5px;">
         <div class="bold"><Corbis:Localize ID="whyRegisterMainContent"  runat="server" meta:resourcekey="whyRegisterMainContent" /> </div>
         <div class="spacer">&nbsp;</div>
          <div class="bold"><Corbis:Localize ID="whyRegisterMainSubContent"  runat="server" meta:resourcekey="whyRegisterMainSubContent" /> </div>
          <ul class="advantage">
          <li><Corbis:Localize ID="advantage1"  runat="server" /></li>
           <li><Corbis:Localize ID="advantage2"  runat="server" /></li>
            <li><Corbis:Localize ID="advantage3"  runat="server" /></li>
           <li><Corbis:Localize ID="advantage4"  runat="server" /></li>
            <li><Corbis:Localize ID="advantage5"  runat="server" /></li>
           <li><Corbis:Localize ID="advantage6"  runat="server" /></li>
           <li><Corbis:Localize ID="advantage7"  runat="server"  /></li>
         </ul>
          </div>
        </div>
        
         <div class="PaneContentRound rc5px clear MB_20 ">
                <div class="Bottom">
                    <div class="Left "><div class="Right "><div class="Center ">&nbsp;</div></div></div>
                </div>
         </div>
    </div>
    </div>
    </div>
    </div>
    
      <div id="RegistrationRoundBottom" class="rc5px clear">
        <div class="Bottom">
            <div class="Left"><div class="Right"><div class="Center">&nbsp;</div></div></div>
        </div>
    </div>
   

  <asp:HiddenField ID="hiddenMailingState" Value="" runat="server" />
  <asp:HiddenField ID="hiddenBusinessState" Value="" runat="server" />
  
 
 
  <Corbis:ModalPopup ID="registerSuccess" Width="350" ContainerID="registerSuccess" runat="server"  meta:resourcekey="successTitle"  CloseScript="HideModal('registerSuccess');javascript:__doPostBack('','');">
        <div style="padding-top:22px">
        <Corbis:Localize ID="successMessage" runat="server" meta:resourcekey="successMessage" />
		<Corbis:GlassButton CssClass="closeSuccess"   ID="closeSuccessButton" runat="server" OnClientClick="javascript:HideModal('registerSuccess');return true;" ButtonStyle="Orange" ButtonBackground="dbdbdb"  meta:resourcekey="closeButton" />
		</div>
	</Corbis:ModalPopup>
	
	<Corbis:ModalPopup ID="registerSuccessDiffCountry" Width="350" ContainerID="registerSuccessDiffCountry" runat="server"  meta:resourcekey="successTitle" CloseScript="HideModal('registerSuccessDiffCountry');javascript:__doPostBack('','');">
       <div style="padding-top:22px">
        <Corbis:Localize ID="successMessageDiff" runat="server" meta:resourcekey="successMessage" />
       
        <div style="padding-top:10px;">
        <Corbis:Image ID="alertImage" ImageUrl="/Images/alertYellow.gif" runat="server"/>
        <Corbis:Localize ID="warningDifferentCountry" runat="server" meta:resourcekey="warningDifferentCountry" />
        <Corbis:HyperLink ID="contactCorbis" runat="server"  meta:resourcekey="contactCorbis" /> 
        <Corbis:Localize ID="warningDifferentCountryEnd" runat="server" meta:resourcekey="warningDifferentCountryEnd" />
        </div>
        </div>
		<Corbis:GlassButton CssClass="closeSuccess"  ID="closeSuccessDiffCountry" runat="server" OnClientClick="javascript:HideModal('registerSuccessDiffCountry');return true;" ButtonStyle="Orange" ButtonBackground="dbdbdb" meta:resourcekey="closeButton" />
		
	</Corbis:ModalPopup>
<script>
    var _mailingAddressCountries, _mailingAddressStates, _mailingAddressZip, _businessAddressCountries, _businessAddressStates, _businessAddressZip, _securityQuestionList, _jobTitleList;
    var selectOneText = '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString((string)HttpContext.GetGlobalResourceObject("Resource", "SelectOne")) %>';
    function initialize() {
    
        _mailingAddressCountries = $(mailingAddressCtrl + '_country');
        _mailingAddressStates = $(mailingAddressCtrl + '_state');
        _mailingAddressZip = $(mailingAddressCtrl + '_zip');

        _businessAddressCountries = $(businessAddressCtrl + '_country');
        _businessAddressStates = $(businessAddressCtrl + '_state');
        _businessAddressZip = $(businessAddressCtrl + '_zip');

        _securityQuestionList = $('<%=securityQuestion.ClientID %>');
        _jobTitleList = $('<%=jobTitle.ClientID %>');
        
        _businessAddressCountries.addEvent('change', handleBusinessAddressCountryChange);


        _mailingAddressCountries.addEvent('change', handleMailingAddressCountryChange);

        handleCountryChange(_mailingAddressCountries, _mailingAddressStates, _mailingAddressZip, false, $('<%=hiddenMailingState.ClientID %>').value, true);
        handleCountryChange(_businessAddressCountries, _businessAddressStates, _businessAddressZip, false, $('<%=hiddenBusinessState.ClientID %>').value, true);

        initializeOnBlurEvents();
        RegisterToolTips();
        enableDisableRegisterButton();
        //handleServerFaults();
    }

    function submitRegistrationForm() {

        _Validation.validateAll();
        if (!_Validation.form.isValid) {
            window.scroll(0, 0);
        }
        
        return false;
    }

   
    function handleCountryChange(countryList, statesList,zipTextBox,shouldUpdateZip, state, shouldUpdateState) {
        var countrySelected = countryList.value;

        if (countrySelected == 'HK') {
            zipTextBox.value = '. ';
            zipTextBox.setProperty('disabled', 'disabled');
        } else {
            if (shouldUpdateZip) {
                zipTextBox.value = '';
                zipTextBox.removeProperty('disabled');
            }
        }
        var mailingeq = new Request.HTML({
            method: 'post',
            url: 'RegistrationService.asmx/GetStates',
            data: { country: countrySelected },
            onRequest: function() {

            },
            onComplete: function(responseTree, responseElements, responseHtml) {

                statesList.empty();
                var Data = parseXMLDoc(responseHtml);
                if (Data && Data.getElementsByTagName('ContentItem').length > 0) {

                    statesList.removeProperty('disabled');
                    var topOpt = new Element('option');
                    topOpt.set('value', '');
                    topOpt.set('text', selectOneText);
                    topOpt.inject(statesList);
                    
                    var ContentItemArray = Data.getElementsByTagName('ContentItem');
                    var ContentItemObject;
                    var KeyObject;
                    var ValueObject;
                    for (var i = 0; i < ContentItemArray.length; i++) {
                        ContentItemObject = ContentItemArray[i];
                        KeyObject = ContentItemObject.getElementsByTagName('Key')[0].childNodes[0].nodeValue;
                        ValueObject = ContentItemObject.getElementsByTagName('ContentValue')[0].childNodes[0].nodeValue;
                        var opt = new Element('option', { value: KeyObject });
                        opt.set('text', ValueObject);
                        opt.inject(statesList);
                    }



                    if (shouldUpdateState) {
                        statesList.value = state;
                    }
                }
                else {
                    statesList.empty();
                    var topOpt = new Element('option');
                    topOpt.set('text', ' - - - - - - - - - - - - - - - - - - - - - - - - -');
                    topOpt.set('value', '');
                    topOpt.inject(statesList);
                    statesList.setProperty('disabled', 'disabled');
                }
            },
            onFailure: function() {

            }
        }).send();
   
    }

    function enableDisableRegisterButton() {

       
        if (getCheckedState('<%=accept.ClientID %>')) {
            $('<%= register.ClientID %>').setStyle('opacity',1);
            $('<%= register.ClientID %>' + '_GlassButton').removeProperty('disabled');
        } else {
            $('<%= register.ClientID %>').setStyle('opacity',0.6);
            $('<%= register.ClientID %>' + '_GlassButton').setProperty('disabled','disabled');
        }
    }
    function initializeOnBlurEvents() {
        $(businessAddressCtrl + '_address1').addEvent('focus', onFocusHandler);
        $(businessAddressCtrl + '_address2').addEvent('focus', onFocusHandler);
        $(businessAddressCtrl + '_address3').addEvent('focus', onFocusHandler);
        $(businessAddressCtrl + '_city').addEvent('focus', onFocusHandler);
        $(businessAddressCtrl + '_country').addEvent('focus', onFocusHandler);
        $(businessAddressCtrl + '_state').addEvent('focus', onFocusHandler);
        $(businessAddressCtrl + '_zip').addEvent('focus', onFocusHandler);
    }
    function onFocusHandler() {
       
        if (getCheckedState('<%=sameAsMailingAddress.ClientID %>')) {
            toggleCheckedState($('<%=sameAsMailingAddress.ClientID %>'));
        }
        
    }
    
    function handleBusinessAddressCountryChange() {
        handleCountryChange(_businessAddressCountries, _businessAddressStates, _businessAddressZip,true, $('<%=hiddenBusinessState.ClientID %>').value,false);
    }

    function handleMailingAddressCountryChange() {
        handleCountryChange(_mailingAddressCountries, _mailingAddressStates, _mailingAddressZip,true, $('<%=hiddenMailingState.ClientID %>').value, false);
    }

    function passwordsShouldMatch() {
        if ($('<%=confirmPassword.ClientID %>').value == '') {
            return true;
        }
        return $('<%=password.ClientID %>').value == $('<%=confirmPassword.ClientID %>').value;
    }

    function emailsShouldMatch() {
        if ($('<%=confirmEmail.ClientID %>').value == '') {
            return true;
        }
        return $('<%=email.ClientID %>').value.toLowerCase() == $('<%=confirmEmail.ClientID %>').value.toLowerCase();
    }

    function validateMailingZip() {
        return validateZip(_mailingAddressCountries, _mailingAddressZip);
     }

     function validateZip(countryList,zipTextBox) {
         var zipCode = zipTextBox.value;
         var country = countryList.value;
         if (zipCode == '') {
             _Validation.highlightRow(true, zipTextBox);
             return false;
         } else {
         if (country == 'US') {
             var result = _Validation.options.regexp.digit.test(zipCode);
                _Validation.highlightRow(!result, zipTextBox);
                 return result;  
             } else {
                 return true;
             }
         }

     }

     function validateBusinessZip() {
         return validateZip(_businessAddressCountries, _businessAddressZip);
     }


     
    function validateMailingCountry() {
        var expression = _mailingAddressCountries.selectedIndex != 0;
        _Validation.highlightRow(!expression, _mailingAddressCountries);
        return expression;
    }

    function validateBusinessCountry() {
        var expression = _businessAddressCountries.selectedIndex != 0;
        _Validation.highlightRow(!expression, _businessAddressCountries);
        return expression;
    }

    function validatePassword() {
        var pass = $('<%=password.ClientID %>').value;
        if (pass == '') {
            return true;
        }
        var RegularExpression1 = new RegExp("[0-9]");
        var RegularExpression2 = new RegExp("[a-z]|[A-Z]");
        var result = (pass.length >=8 && pass.length<=20 && RegularExpression1.test(pass) && RegularExpression2.test(pass));
        _Validation.highlightRow(!result, $('<%=password.ClientID %>'));
        return result;
    }
    function validateMailingState() {
        $('<%=hiddenMailingState.ClientID %>').value = _mailingAddressStates.getElements('option')[_mailingAddressStates.selectedIndex].value;

        var mailingCountry = _mailingAddressCountries.value;
        var mailingStateIndex = _mailingAddressStates.selectedIndex;
        if (mailingCountry == 'US' || mailingCountry == 'CA' || mailingCountry == 'AU' || mailingCountry == 'GB') {
            var result = (mailingStateIndex == 0);
                _Validation.highlightRow(result, _mailingAddressStates);
                return !result;

        } else {
        return true;
        }
    }

    function validateBusinessState() {
        $('<%=hiddenBusinessState.ClientID %>').value = _businessAddressStates.getElements('option')[_businessAddressStates.selectedIndex].value;
        
        var businessCountry = _businessAddressCountries.value;
        var businessStateIndex = _businessAddressStates.selectedIndex;
        if (businessCountry == 'US' || businessCountry == 'CA' || businessCountry == 'AU' || businessCountry == 'GB') {
            var result = (businessStateIndex == 0);
            _Validation.highlightRow(result, _businessAddressStates);
            return !result;
        } else {
            return true;
        }
    }

    function validateSecurityQuestion() {
        var result = (_securityQuestionList.selectedIndex == 0);
        _Validation.highlightRow(result, _securityQuestionList);
        return !result;
    }

    function validateJobTitle() {
        var result = (_jobTitleList.selectedIndex == 0);
        _Validation.highlightRow(result, _jobTitleList);
        return !result;
    }
    function CopyMailingToBusinessAddress() {

        if (getCheckedState('<%=sameAsMailingAddress.ClientID %>')) {
            $(businessAddressCtrl + '_address1').value = $(mailingAddressCtrl + '_address1').value;

            if ($find('address2MailingWatermark').get_Text() == '') {
                $find('address2BusinessWatermark').set_Text('');
                $find('address2BusinessWatermark').set_WatermarkText($find('address2BusinessWatermark').get_WatermarkText());
            } else {
                $find('address2BusinessWatermark').set_Text($find('address2MailingWatermark').get_Text());
            }

            if ($find('address3MailingWatermark').get_Text() == '') {
                $find('address3BusinessWatermark').set_Text('');
                $find('address3BusinessWatermark').set_WatermarkText($find('address3BusinessWatermark').get_WatermarkText());
            } else {
                $find('address3BusinessWatermark').set_Text($find('address3MailingWatermark').get_Text());
            }

            $(businessAddressCtrl + '_city').value = $(mailingAddressCtrl + '_city').value;
            $(businessAddressCtrl + '_country').value = $(mailingAddressCtrl + '_country').value;
            handleCountryChange($(businessAddressCtrl + '_country'), $(businessAddressCtrl + '_state'), $(businessAddressCtrl + '_zip'), true, $(mailingAddressCtrl + '_state').value, true);
            $(businessAddressCtrl + '_zip').value = $(mailingAddressCtrl + '_zip').value;
        } 

    }

  

    function RegisterToolTips() {
        $('aspnetForm').getAllNext('.TIP-license-details').destroy();
        var tipShowDelay = 500;
        var tipHideDelay = 100;
        var tipShowMethod = "in";
        var tipHideMethod = "out";
        if (Browser.Engine.trident) {
            tipShowDelay = 0;
            tipHideDelay = 0;
            tipShowMethod = "show";
            tipHideMethod = "hide";
        }
        new Tips('.thumbWrap', {
            showDelay: tipShowDelay,
            hideDelay: tipHideDelay,
            offsets: { x: 0, y: -120 },
            className: 'TIP-license-details mochaContent',
            onHide: function(tip) {
                tip.fade(tipHideMethod);
            },
            onShow: function(tip) {
                tip.fade(tipShowMethod);
            }
        });
    }

   
    window.addEvent('load', initialize);
   
</script>	 
</asp:Content>
