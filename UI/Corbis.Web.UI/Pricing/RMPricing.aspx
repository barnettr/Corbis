<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RMPricing.aspx.cs" Inherits="Corbis.Web.UI.Pricing.RMPricing" MasterPageFile="/MasterPages/ModalPopup.Master"  %>
<%@ Register TagName="PricingHeader" TagPrefix="Corbis" Src="PricingHeader.ascx" %>
<%@ Register TagName="Restrictions" TagPrefix="IR" Src="../src/Image/Restrictions.ascx"  %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register Assembly="Corbis.Web.UI.Controls" Namespace="Corbis.Web.UI.Controls" TagPrefix="Corbis" %>
<%@ Register TagPrefix="UC" TagName="InstantService" src="../Chat/UserControl/InstantServiceButtonSmall.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="ContactCorbis" Src="~/UserControls/ContactControls/ContactCorbis.ascx" %>

<asp:Content ID="rfPricingContent" ContentPlaceHolderID="mainContent" runat="server">
    
    <script language="javascript" src="/Scripts/GeographySelector.js" type="text/javascript"></script>
    <script language="javascript" src="/Scripts/Pricing.js" type="text/javascript"></script> 
    <!-- start -->
    <div class="OuterContainer ">
        <div class="TitleBar">
            <div class="PriceImageTitle">
                <Corbis:Label ID="pageTitle" CssClass="Title" meta:resourcekey="pageTitle" runat="server" />
                <span class="SubTitle"><Corbis:Label ID="pageSubTitle" meta:resourcekey="pageSubTitle" runat="server" /></span>
            </div>
            <div class="CloseButton">
                <asp:UpdatePanel ID="xCloseUpdatePAnel" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:HyperLink ID="XClose" runat="server"><img Class="Close" src="/Images/iconClose.gif" /></asp:HyperLink>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
             <div class="PrintDiv" meta:resourcekey="printDiv" runat="server" visible="false">
                <asp:HyperLink runat="server" NavigateUrl="#" ID="printImgHyperlink" CssClass="PrintIconImg" meta:resourcekey="printImgHyperlink" />
            </div>
            
            <div class="ChatDiv" meta:resourcekey="chatDiv" runat="server">
                <UC:InstantService id="instantService1" runat="server" />
            </div>
        </div>
        <input type="hidden" name="EffectivePriceLocalized" id="EffectivePriceLocalized" value="24.000,00" runat="server" />
        <asp:UpdatePanel ID="headerUpdatePanel" EnableViewState="true" runat="server" UpdateMode="conditional">
           <ContentTemplate>
             <Corbis:PricingHeader id="pricingHeader" runat="server" />              
           </ContentTemplate>
           <Triggers>
                <asp:PostBackTrigger ControlID="pricingHeader" />
                <asp:AsyncPostBackTrigger ControlID="PriceNowButton" EventName="Click" />
           </Triggers>
        </asp:UpdatePanel>
        
       
      
        <div id="pricing" class="RMPricing">            
            <div class="CustomPricing rounded4" internal="true">
                <asp:UpdatePanel ID="startOverUpdatePanel" UpdateMode="conditional" runat="server">
                    <ContentTemplate>
                        <div class="StartOver">
                        <Corbis:ImageButton ID="startOverLinkImage" OnClick="startOverLink_Click" OnClientClick="LogOmnitureEvent('event26');" CausesValidation="false" runat="server" ImageUrl="/images/resetdbdbdb.gif" />
                            <Corbis:LinkButton ID="startOverLink" meta:resourcekey="startOverLink" OnClientClick="LogOmnitureEvent('event26');" CausesValidation="false" OnClick="startOverLink_Click" runat="server" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="Title">
                        <div class="headerFont"><asp:Label runat="server" ID="customPricingHeader" meta:resourcekey="customPricingHeader" /></div>
                        <div><asp:LinkButton runat="server" CausesValidation="false" ID="learnMoreHyperLink" OnClientClick="OpenLearnMore(this);return false;" meta:resourcekey="learnMoreHyperLink" /></div>
                </div>
                
                <asp:UpdatePanel ID="AllUsageUpdatePanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <asp:UpdatePanel ID="ErrorSummaryUpdatePanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
						<div id="ErrorContainer" class="Error">
							<div id="genericErrorMessageDiv" runat="server" class="Error" visible="false">
							    <div  class="ValidationSummary">
                                    <ul>
                                        <li>
                                            <Corbis:Localize ID="GenericErrorMessage" runat="server" meta:resourcekey="genericErrorMessage" />
                                        </li>
                                    </ul>
                                </div>
							</div>
							<Corbis:ValidationGroupSummary ID="useTypeAttributesValidationSummary"  runat="server" CssClass="Error" ValidationGroup="useTypeAttributesValidationGroup" />
							<div id="licenseAlertDiv" runat="server"></div>
						</div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Panel runat="server" CssClass="Content" ID="regularContent" >
                    <asp:UpdatePanel ID="savedUsageUpdatePanel" UpdateMode="conditional" runat="server">        
                        <ContentTemplate>
                            <div class="SelectSavedUsage widthController" id="selectSavedUsageDiv" runat="server">
                                <div id="favoriteUseContainer" runat="server">
                                    <h3>
                                        <asp:Label 
                                            runat="server" ID="selectFavoriteUseLabel" 
                                            meta:resourcekey="selectFavoriteUseLabel" 
                                        />
                                    </h3>
                                    <div>
                                        <asp:Label 
                                            runat="server" ID="favoriteUseInstructionsLabel" 
                                            meta:resourcekey="favoriteUseInstructionsLabel"
                                        />
                                    </div>
                                    <div>
                                        <Corbis:Dropdownlist onChange="LogOmnitureEvent('event23'); setIsSavedUsage();" id="savedUsages" meta:resourcekey="useFavoriteUse" 
                                        Width="240" AutoPostBack="true"  OnSelectedIndexChanged="savedUsages_SelectedIndexChanged" runat="server"
                                        />
                                        
                                    </div>
                                </div>
                            </div>   
                            <div class="dottedLine widthController" id="dottedLine" runat="server"></div> 
                            <div style="background-color: #e8e8e8;"><asp:Label CssClass="or" runat="server" ID="or" meta:resourcekey="or" /></div>  
                        </ContentTemplate>
                    </asp:UpdatePanel>
                     
                    

                    <div class="StartNewUsage widthController">
                        <asp:UpdatePanel ID="startNewUpdatePanel" runat="server" UpdateMode="conditional" >
                            <ContentTemplate>                                                     
                                    <div  id="startNewDiv" runat="server" visible="true">
                                    <h3><asp:Label CssClass="PT_10 PL_10" runat="server" ID="createAnewUseLabel" meta:resourcekey="createAnewUseLabel" Visible="true"/></h3>
                                    <div><asp:Label CssClass="PL_10" runat="server" ID="questionsLabel" meta:resourcekey="questionsLabel" Visible="true" /></div>
                                    </div>                            
                                    <div id="createANewUseDiv" class="createANewUseDiv" visible="false" runat="server"></div> 
                            </ContentTemplate>                     
                       </asp:UpdatePanel>               
                            <asp:UpdatePanel ID="useCategoryAndTypeUpdatePanel" EnableViewState="true" runat="server" UpdateMode="conditional" >
                                <ContentTemplate>
                                    <div id="allUsageDiv" runat="server">
                                        <div class="UseCategoriesDropDownDiv" id="useCategoriesDropDownDiv" runat="server">
                                            <asp:Label runat="server" ID="selectUseCategory" meta:resourcekey="selectUseCategory" />
                                            <Corbis:Dropdownlist id="useCategoriesDropDown" meta:resourcekey="useCategoriesDropDown" OnSelectedIndexChanged="useCategoriesDropDown_SelectedIndexChanged" autopostback="true" runat="server"></Corbis:Dropdownlist>
                                            <asp:CustomValidator ID="useCategoriesValidator" runat="server" Display="Dynamic" ValidateEmptyText="true" ControlToValidate="useCategoriesDropDown" ValidationGroup="useTypeAttributesValidationGroup" OnServerValidate="ValidateUseCategory"  />
                                        </div>
                                        <div class="UseTypesDropDownDiv" id="useTypesDropDownDiv" runat="server">
                                        <asp:Label runat="server" ID="selectUseType" meta:resourcekey="selectType" />
                                            <Corbis:Dropdownlist id="useTypesDropDown" meta:resourcekey="useTypesDropDown" OnSelectedIndexChanged="useTypesDropDown_SelectedIndexChanged" autopostback="true" runat="server"></Corbis:Dropdownlist>
                                            <asp:CustomValidator ID="useTypesDropDownValidator" runat="server" Display="Dynamic" ValidateEmptyText="true" ControlToValidate="useTypesDropDown" ValidationGroup="useTypeAttributesValidationGroup" OnServerValidate="ValidateUseType" />
                                        </div>
                                        <div class="UseTypeAttributeDropDowns">
                                            <asp:Repeater ID="useTypeAttributesRepeater" EnableViewState="true" OnItemCreated="useTypeAttributesRepeater_ItemCreated" OnItemDataBound="useTypeAttributesRepeater_ItemDataBound" runat="server">
                                                <ItemTemplate>
                                                    <div id="useTypeAttributeDiv" class="NewUsageSelectors UnselectedAttribute" runat="server">
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="saveFavoriteUsePopUp" style="width: 300px; display:none;">
                           <asp:UpdatePanel ID="saveFavoriteUsePopupUpdatePanel" runat="server" UpdateMode="Conditional" >
                            <ContentTemplate>                            
                            <div class="ModalPopupPanelDialog">
                                    <div class="ModalTitleBar">
                                       <asp:ImageButton id="imageClosebutton" runat="server"  src="../Images/iconClose.gif" OnClientClick="javascript:HideModal('saveFavoriteUsePopUp');removeBackGroundImage();return false;"  type="image" Cssclass="Close" style="border-width: 0px;" />
                                       <asp:Label ID="favorPopupTitle" class="Title"  runat="server" meta:resourcekey="favorPopupTitle" />
                                    </div>      
                                    <div class="ModalPopupContent">                            
                                    <asp:Label ID="saveFavoriteUsePopUpLabel" CssClass="saveFavoriteUsePopUpLabel" runat="server" meta:resourcekey="saveFavoriteUsePopUpLabel" />
                                    <corbis:textbox width="200" MaxLength="50"  runat="server" id="usageName" />
                                    <div class="FormButtons">
                                        <Corbis:GlassButton id="CancelButton" ButtonStyle="Gray" CausesValidation="false"   runat="server"  OnClientClick="javascript:HideModal('saveFavoriteUsePopUp');removeBackGroundImage();return false;"  meta:resourcekey="cancelSave" />
                                        <Corbis:GlassButton id="SaveButton" CausesValidation="true"  OnClientClick="return validateUsageName();" OnClick="SaveFinishedUsage_Click" runat="server" meta:resourcekey="saveFavoriteUseButton" />
                                        <Corbis:GlassButton id="CloseButton" CausesValidation="true" Visible="false" OnClientClick="javascript:MochaUI.HideModal('saveFavoriteUsePopUp');removeBackGroundImage();return false;"  runat="server" meta:resourcekey="close" />
                                    </div>
                                  </div>  
                                  </div>
                             </ContentTemplate>
                          </asp:UpdatePanel>
      	                </div>
      	                
                        <!--end-->
                        
                        <div class="Clr MT_10"></div>
                        
                        <asp:UpdatePanel ID="UsagePricingUpdatePanel" EnableViewState="true" runat="server" UpdateMode="Conditional">
                         <ContentTemplate>
                           <div class="ButtonRight" ID="SaveFavDiv" runat="server">
                           <Corbis:GlassButton id="SaveFavoriteUsageButton" CssClass="PushLeft" ButtonStyle="Outline" CausesValidation="true" runat="server"  OnClick="SaveFavoriteUsage_Click"  meta:resourcekey="SaveFavoriteUse">
                           </Corbis:GlassButton>
                           <Corbis:GlassButton id="PriceNowButton" CausesValidation="true" OnClick="PriceNowButton_Click" runat="server" meta:resourcekey="PriceNowButton" class="thumbWrap"/></div>
                         </ContentTemplate>
                        
                        </asp:UpdatePanel>
                </asp:Panel>
                </ContentTemplate>
                </asp:UpdatePanel>
                
                <asp:UpdatePanel ID="PricedByAEUpdatePanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" CssClass="Content AE" ID="pricedByAEContent">
                            <div id="aeOutLine">
                                <div id="aeOutLineContents">
                                    <h3>
                                        <Corbis:Localize runat="server"  meta:resourcekey="pricedByAETitle" />
                                    </h3>
                                    <p>
                                        <Corbis:Localize runat="server" ID="pricedByAEParagraph1" />
                                    </p>
                                    <p>
                                        <br />
                                        <Corbis:Localize ID="pricedByAEReviewLicense" runat="server" meta:resourcekey="pricedByAEReviewLicense" />
                                    </p>
                                    <div id="pricedByAEstartDateDiv" runat="server" >
                                        <div id="errorBlock" class="WarningMode MT_5" style="visibility:hidden;">
                                            <img alt="error" src="../Images/iconError.png" / class="errorImage">
                                            <Corbis:Label runat="server" meta:resourcekey="pricedByAEStartDateError" />
                                        </div>
                                        <div id="startDateMain" class="">
                                            <span id="priceByAETitle">
                                            <Corbis:Localize ID="pricedByAEStartDateTitle" runat="server" meta:resourcekey="pricedByAEStartDateTitle" />
                                            </span>
                                            <asp:Label runat="server" ID="pricedByAEStartDateLbl" CssClass="startDateDefaultMode"></asp:Label>
                                            <a onclick="toggleEditMode(true);return false;" class="startDateDefaultMode">
                                            <Corbis:Localize ID="pricedByAEStartDateEditAnchor" runat="server" meta:resourcekey="pricedByAEStartDateEditAnchor" />
                                            </a>
                                            <asp:TextBox runat="server" CssClass="fValidate['date'] startDateEditMode" onblur="checkStartDateOnManualEdit();" ID="pricedByAEStartDateTextBox" style="display:none;"  />
                                            <asp:ImageButton runat="server" CssClass="startDateEditMode" ImageUrl="~/Images/calendar.gif" ID="pricedByAEstartDatePicker" style="display:none;"/>
                                        </div>
                                        <AJAXToolkit:CalendarExtender ID="pricedByAECalendarExtender" runat="server" TargetControlID="pricedByAEStartDateTextBox" CssClass="aeCalendar" Format="MM/dd/yyyy" 
                                                OnClientDateSelectionChanged="checkStartDate" PopupButtonID="pricedByAEstartDatePicker" BehaviorID="Calendar1" PopupPosition="TopLeft" />

                                        <div id="startDateButtons">
                                            <Corbis:GlassButton ID="pricedByAEstartDateSaveButton" CssClass="startDateEditMode" ButtonBackground="gray36" runat="server"  meta:resourcekey="pricedByAEstartDateSaveButton" />
                                            <Corbis:GlassButton ID="pricedByAEstartDateCancelButton" CssClass="startDateEditMode" ButtonBackground="gray36" ButtonStyle="Gray" runat="server" meta:resourcekey="pricedByAEstartDateCancelButton" OnClientClick="updateResult(false);return false;" />
                                        </div>
                                    </div>
                                    <input type="hidden" id="hdnCultureName" runat="server" />
                                </div>
                            </div>
                            <Corbis:ModalPopup ID="customPriceExpiredPopup" ContainerID="customPriceExpiredPopup"  runat="server" Width="300" meta:resourcekey="customPriceExpiredPopup" CloseScript="PricingModalPopupExit();return false;">  
                                <Corbis:GlassButton ID="customPriceExpiredPriceNowBtn" runat="server" CausesValidation="true" meta:resourcekey="customPriceExpiredPriceNowBtn" />                    
  	                        </Corbis:ModalPopup>                            
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>                
                
            </div>
            <div id="licenseDetailsDiv" class="LicenseDetails rounded" internal="true">
               
               <asp:UpdatePanel ID="licenseDetailsUpdatePanel" runat="server" UpdateMode="Conditional" >
                 <ContentTemplate>                    
                    <div class="Title">
                        <div class="headerFont"><asp:Label runat="server" ID="licenseDetailsHeader" meta:resourcekey="licenseDetailsHeader" /></div>
                        <asp:Label runat="server"  ID="favoriteUseTitle" meta:resourcekey="FavoriteUseTitle" />
                        <asp:Label runat="server" ID="favoriteUseValue" CssClass="font--color"/>
                    </div>                
                </ContentTemplate>
                </asp:UpdatePanel>
                
                <asp:Panel runat="server" CssClass="Content" ID="regularContentDetail">
                <asp:UpdatePanel ID="detailsUpdatePanel" runat="server" >
                    <ContentTemplate>
                        <div id="licenseDetails" runat="server"><Corbis:Label id="licenseDetailsEmpty" runat="server" meta:resourcekey="licenseDetailsEmpty" /></div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="PriceNowButton" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="savedUsages" EventName="SelectedIndexChanged" />
                        
                    </Triggers>
                </asp:UpdatePanel>
                </asp:Panel>
            </div>

    </div>
    <div class="PricingBottom">
            <div class="ExclusiveRights">
                <a id="rmPricingContactLink" href="javascript:void(0);" class="textIconButton" onclick="javascript:CorbisUI.ContactCorbis.ShowContactCorbisModal(this)"><Corbis:Localize runat="server"  ID="exclusiveRightsHyperLink" meta:resourcekey="exclusiveRightsHyperLink" /></a>
            </div>
            <div class="ViewRestrictions">
                <Corbis:TextIconButton runat="server" Icon="yellowAlert" OnClientClick="OpenRestrictions(this)" meta:resourcekey="lnkViewRestrictions" />
            </div>
        </div>   
       
    </div>
    
    <asp:UpdatePanel ChildrenAsTriggers="true" runat="server">
	    <ContentTemplate>
			<Corbis:ModalPopup ContainerID="confirmClose"  runat="server" Width="300" meta:resourcekey="confirmClose">  
				<Corbis:GlassButton ID="cancelClose" runat="server" cssClass="closeWarning" CausesValidation="false" meta:resourcekey="cancelClose"  OnClientClick="HideModal('confirmClose');return false;"  />
				<Corbis:GlassButton ID="continueClose" runat="server" cssClass="btnGraydbdbdb closeWarning" ButtonStyle="Gray"  CausesValidation="false" meta:resourcekey="continueClose" OnClientClick="PricingModalPopupExit();return false;" />
  			</Corbis:ModalPopup>
		</ContentTemplate>
    </asp:UpdatePanel>
              	
  	<Corbis:ModalPopup ContainerID="learnMorePopup" runat="server" Width="350" meta:resourcekey="learnMorePopup">  
  	</Corbis:ModalPopup>
        
    <Corbis:ModalPopup ContainerID="restrictionsPopup" runat="server" Width="550" meta:resourcekey="restrictionsPopup">  
        <IR:restrictions id="ImageRestrictions" ShowHeader="false" runat="server" />
    </Corbis:ModalPopup>
    
    <Corbis:ModalPopup ContainerID="priceStatusPopup" Width="350" runat="server" meta:resourcekey="priceStatusPopup">
        
        <Corbis:GlassButton ID="priceStatusCloseButton" runat="server" meta:resourcekey="priceStatusCloseButton" OnClientClick="HideModal('priceStatusPopup');return false;" />
    </Corbis:ModalPopup>
	<Corbis:ContactCorbis runat="server" />
	
<%--    <div class="PricingFooter">
        <asp:Label runat="server" ID="lblPricingFooterDisclaimer" meta:resourcekey="lblPricingFooterDisclaimer" />       
    </div>--%>

    <script type="text/javascript" language="javascript">
        CorbisUI.Pricing.RM.vars.SaveFavoriteUsageButtonID = '<%= SaveFavoriteUsageButton.ClientID %>';
        
//    
//   Begin Script
//   Originally Built By: v-nicks 10/31/08
//   This code vertically centers two HTML elements within a page.
//   Search Keyword(ctrl+F): verticalMiddle
//
        localizedValue = $('<%=EffectivePriceLocalized.ClientID %>');
        window.addEvent('load', function() {
            CorbisUI.Pricing.RM.RegisterToolTips();
        });
        window.addEvent('domready', function() {

            var verticalMiddleElements = new Hash({
                //Add multiple elelments to center.
                //innerElementToVerticallyAlign1: 'outerContainingElement1',
                //innerElementToVerticallyAlign2: 'outerContainingElement2',
                //innerElementToVerticallyAlign3: 'outerContainingElement3',
                //innerElementToVerticallyAlign4: 'outerContainingElement4',
                aeOutLineContents: 'aeOutLine'
                
            });

        
            
            verticalMiddleElements.each(function(innerElement, outerElement) {
                verticalMiddleGlobal(innerElement, outerElement);
            });

            var usages = $('<%= savedUsages.ClientID %>');

            var categories = $('<%= useCategoriesDropDown.ClientID %>');
            try {
                if (usages == null || usages.disabled == true) {
                    categories.focus();
                } else {
                    usages.focus();
                }
            } catch (Error) {
                setTimeout("if($('<%=printImgHyperlink.ClientID %>')) $('<%=printImgHyperlink.ClientID %>').focus();", 100);
            }
        }); 
//
//   End Script
//   Originally Built By: Nick Stark 10/31/08
//   This code vertically centers two HTML elements within a page
//
        
        var priceLabel = $('<%= pricingHeader.piPrice.ClientID %>');
        var priceCode = $('<%= pricingHeader.piPriceCode.ClientID %>');
        
        function OpenLearnMore(element)
        {
            new CorbisUI.Popup('learnMorePopup', { 
                showModalBackground: false,
                centerOverElement: element,
                closeOnLoseFocus: true,
                positionVert: 'bottom', 
                positionHoriz: 'right'
            });  
        }
        
        function OpenRestrictions(element)
        {
            new CorbisUI.Popup('restrictionsPopup', { 
                showModalBackground: false,
                centerOverElement: element,
                closeOnLoseFocus: true,
                positionVert: 'top', 
                positionHoriz: 'right'
            });  
        }

        function setRMPriceLabel(amount,localeValue) {        
            //Price code: Eg: USD,JPY
            var priceCode = $('<%= pricingHeader.piPriceCode.ClientID %>');
            //Price Label : Eg: 0.00,510.00,Per your contract
            var priceLabel = $('<%= pricingHeader.piPrice.ClientID %>');
            localizedValue.value=localeValue;
            var speed = 5;
            var steps = 95;
            if ( isNaN(amount)) 
            {
                 var currentPrice = priceLabel.innerHTML;
                 priceLabel.innerHTML = amount;
                 {
                    priceCode.style.display='none';
                 }
            }
            else if (amount == "0.00")
            {
                 var currentPrice = priceLabel.innerHTML;
                 priceLabel.innerHTML = amount.toInt().localeFormat('C');
                 if (isNaN(amount))
                 {
                    priceCode.style.display='display-block';
                 }
            }            
            else
            {
                 if (isNaN(parseFloat(priceLabel.innerHTML)))
                 {
                    priceLabel.innerHTML = amount.toInt().localeFormat('C');
                 }
                 var currentPrice = parseFloat(priceLabel.innerHTML);
                 var gotoPrice = parseFloat(amount);
                  for (var i=0; i <steps; i++)
                    {
                        value = currentPrice + (((gotoPrice - currentPrice) / steps) * i);
                        window.setTimeout("setLabelValue(" + value + ")", i * speed);
                    }
                    priceCode.style.display='inline-block';
                    window.setTimeout("setLabelValue(" + gotoPrice + ")", steps * speed);
            }
           // getLocalizedPayment();
        }
        function getLocalizedPayment()
        {        
         var priceLabel = $('<%= pricingHeader.piPrice.ClientID %>');
         priceLabel.innerHTML = localizedValue.value;
        }
        
        function setLabelValue(value)
        {      
        
           var priceLabel = $('<%= pricingHeader.piPrice.ClientID %>');
            priceLabel.innerHTML = value.localeFormat('C');            
            window.setTimeout("getLocalizedPayment()",500);
        }
      
        function openPriceStatusMessage(which, link)
        {
            new CorbisUI.Popup(which, {
                showModalBackground: false,
                closeOnLoseFocus: true,
                centerOverElement: link,
                positionVert: 'middle', 
                positionHoriz: 'right'
            });    
        }
        function doSaveFav()
        {
            var href = $('SaveFavDiv').getElement('a').getProperty('href').replace(/javascript:/, '');
            //console.log(href);
            eval(href);
            HideModal('confirmClose');
            return false;
        }
        function testCanSave(link)
        {
            if ($('SaveFavDiv').getElement('a') == null || $('SaveFavDiv').getElement('div').hasClass('DisabledGlassButton'))
                PricingModalPopupExit();
            else
                OpenCloseWarning('confirmClose',link);
        }
        
        function validateUsageName() {
            var usage = $('<%=usageName.ClientID %>').value;
            if(usage == null || usage == "" || (usage!=null && usage.trim() == ""))
            {
                return false;
            }else{
                LogOmnitureEvent('event25');
                return true;
            }
            
        }
        
        function resizeDisplay()
        {
			var errorSummary = $('ErrorContainer');
			var regularContent = $('<%= regularContent.ClientID %>');
			var regularContentDetail = $('<%= regularContentDetail.ClientID %>');
			var usageContent = regularContent.getElement('div.StartNewUsage');

			var errorSummaryHeight = 0;
			if (errorSummary) errorSummaryHeight = errorSummary.getStyle('height').toInt();
			
			regularContent.setStyle('height', regularContentDetail.getStyle('height').toInt() - errorSummaryHeight);


}
// Remove this workaround after replacing calendar control
//http://connect.microsoft.com/VisualStudio/feedback/Workaround.aspx?FeedbackID=277646
Sys.CultureInfo.prototype._getAbbrMonthIndex = function(value) {
    if (!this._upperAbbrMonths) {
        this._upperAbbrMonths = this._toUpperArray(this.dateTimeFormat.AbbreviatedMonthNames);
    }
    return Array.indexOf(this._upperAbbrMonths, this._toUpper(value));
}
//});

    function addBackGroundImage() {
        if ($(parent.document).getElement('#pricingWindow_overlay')) {
            $(parent.document).getElement('#pricingWindow_overlay').setStyles({
            'background-image': 'url(../Images/RMpricingBackground.png)',
                'background-repeat': 'no-repeat'
            });
        }
    }

    function removeBackGroundImage() {
        if ($(parent.document).getElement('#pricingWindow_overlay')) {
            $(parent.document).getElement('#pricingWindow_overlay').setStyles({
                'background-image': 'none'
            });
        }
    }
    </script>

    <script type="text/javascript" language="javascript">
		<%// This AJAX stuff does not play well in the js file %>
		Sys.Net.WebRequestManager.add_completedRequest(onComplete);

		function onComplete(sender, args)
		{
            CorbisUI.Pricing.RM.vars.SaveFavoriteUsageButtonID = '<%= SaveFavoriteUsageButton.ClientID %>';
            setTimeout("CorbisUI.Pricing.RM.RegisterToolTips()", 2000);		
        }    
    </script>
    <!-- end -->
</asp:Content>
