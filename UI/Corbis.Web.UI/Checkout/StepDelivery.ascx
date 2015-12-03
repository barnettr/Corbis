<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StepDelivery.ascx.cs" Inherits="Corbis.Web.UI.Checkout.StepDelivery" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<%--
*******************************
* JAVASCRIPT GLOBAL VARS 
*******************************
--%> 
<script type="text/javascript">

    // check if MainCheckout object has a Tabs definition
    if (typeof (CorbisUI.GlobalVars.MainCheckout.Tabs) == 'undefined') {
        CorbisUI.GlobalVars.MainCheckout.Tabs = {};
    }

    // since CorbisUI.GlobalVars.MainCheckout.Tabs
    // is set in the top of MainCheckout.aspx
    // we need to extend the object

    $extend(CorbisUI.GlobalVars.MainCheckout.Tabs, {
        nonRfcdTabs: '<%= optionsBlock.ClientID %>',
        rfcdTabs: '<%= optionsBlockRFCD.ClientID %>'
    });

    // see Checkout.js DomReady load for button initialization
    
    
    CorbisUI.Checkout.DeliveryOptions = {
        DeliveryMethod:'Download',
        selectedDeliveryMethod:'<%= optionsBlockSelected.ClientID %>'
    };

   function setDeliveryMethod(method) {
       $('<%= optionsBlockSelected.ClientID %>').setProperty('value', method);
       CorbisUI.Checkout.DeliveryOptions.DeliveryMethod = method;
   }
   
   
</script>

<%--
*******************************
* PANE LAYOUT 
*******************************
--%> 

<div class="paneLayout" id="deliveryPaneLayout">
    <h4>                            
        <Corbis:Localize ID="howDeliver" runat="server" meta:resourcekey="howDeliver" />
    </h4>
    <asp:Panel CssClass="optionsBlock" ID="optionsBlock" runat="server" Visible="false">
	    <a href="javascript:void(0)" class="deliveryOptionsDownload selected" runat="server" id="deliveryOptionsDownload"
		    visible="false">
		    <span>
			    <Corbis:Localize ID="downloadThis1" runat="server" meta:resourcekey="downloadThis" />
		    </span>
	    </a>
	    <a href="javascript:void(0)" onclick="DeliveryPane('FTP');return false;" class="deliveryOptionsFTP" runat="server" id="deliveryOptionsFTP" visible="false">
		    <span>
			    <Corbis:Localize ID="FTPThis1" runat="server" meta:resourcekey="FTPThis" />
		    </span>
	    </a>
	    <a href="javascript:void(0)" onclick="CorbisUI.Checkout.modals.shippingPriorityQuestions(this); return false;"
	    class="deliveryOptionsCD" runat="server" id="deliveryOptionsCD" visible="false">
		    <span>
			    <Corbis:Localize ID="CDThis1" runat="server" meta:resourcekey="CDThis" />
		    </span>
	    </a>
	    <a href="javascript:void(0)" class="deliveryOptionsEmail" runat="server" id="deliveryOptionsEmail" visible="false">
		    <span>
			    <Corbis:Localize ID="deliveryEmail1" runat="server" meta:resourcekey="emailThis" />
		    </span>
	    </a>
	    <asp:HiddenField ID="optionsBlockSelected" runat="server" EnableViewState="true" />
	    <div class="clr">
	    </div>
	    
    </asp:Panel>

    <div id="deliveryPhysical_Error" class="Error displayNone">
        <div>
            <strong>
                <Corbis:Localize ID="NoAddressesError" runat="server" meta:resourcekey="NoAddressesError" />
            </strong>
        </div>
    </div>

    <asp:Panel CssClass="optionsBlock" ID="optionsBlockRFCD" runat="server" Visible="false">
	    <a href="javascript:void(0)" class="deliveryOptionsDownload selected" runat="server" id="deliveryOptionsDownloadRFCD" visible="false">
		    <span>
			    <Corbis:Localize ID="downloadThis2" runat="server" meta:resourcekey="downloadThis" />
		    </span>
	    </a>
	    <a href="javascript:void(0)" class="deliveryOptionsFTP" runat="server" id="deliveryOptionsFTPRFCD" visible="false">
		    <span>
			    <Corbis:Localize ID="FTPThis2" runat="server" meta:resourcekey="FTPThis" />
		    </span>
	    </a>
	    <a href="javascript:void(0)" onclick="CorbisUI.Checkout.modals.shippingPriorityQuestions(this); return false;" class="deliveryOptionsCD" runat="server" id="deliveryOptionsCDRFCD" visible="false">
		    <span>
			    <Corbis:Localize ID="CDThis2" runat="server" meta:resourcekey="CDThis" />
		    </span>
	    </a>
	    <a href="javascript:void(0)" class="deliveryOptionsEmail" runat="server" id="deliveryOptionsEmailRFCD" visible="false">
		    <span>
			    <Corbis:Localize ID="emailThis2" runat="server" meta:resourcekey="emailThis" />
		    </span>
	    </a>
	    <asp:HiddenField ID="optionsBlockSelectedRFCD" runat="server" EnableViewState="true" />
	    <div class="clr">
	    </div>
    </asp:Panel>
    
    <div class="deliveryChargesDiv" runat="server" id="deliveryCharges" visible="false">
        <Corbis:Localize ID="deliveryCharges1" runat="server" meta:resourcekey="deliveryCharges" />
    </div>
    <hr />
    
    <div id="emailDeliverySection">
        <h4>
            <Corbis:Localize ID="pleaseGiveEmail" runat="server" meta:resourcekey="pleaseGiveEmail" />
        </h4>
        <Corbis:Localize ID="weWillSend" runat="server" meta:resourcekey="WeWillSend" />
        <br />
        <Corbis:TextBox
            ID="deliveryEmails" runat="server" MaxLength="255" 
            validate="required;email-m" meta:resourcekey="deliveryEmails"
        />
        <Corbis:ValidationHub 
            id="deliveryValidationHub" runat="server" UniqueName="Delivery"
            IsPopup="false" SuccessScript="checkoutTabs.show(2, true);"
            ContainerID="emailDeliverySection" AutoInit="false"
        />
        <div><Corbis:Localize ID="semicolon" runat="server" meta:resourcekey="semicolon" /></div>
    </div>
    <Corbis:WorkflowBlock WorkflowType="COFF" runat="server">
        <hr />
        <div id="preferencesSpecialInstructionsSection">
            <h4>
                <Corbis:Localize ID="Localize7" runat="server" meta:resourcekey="pleaseIndicateImagePreferences" />
            </h4>
            <div id="preferencesGroup">
                <div class="rmResolutionPreferenceCheckBox">
                    <span class="LabelClass">&nbsp;</span>
                    <span class="ValueClass">
                        <Corbis:CheckBox Checked="false" id="rmResolutionPreference" runat="server" meta:resourcekey="rmResolutionPreference" />
                    </span>
                </div>
                <div class="subjectDiv">
                    <span class="LabelClass">
                            <Corbis:Localize ID="Localize10" runat="server" meta:resourcekey="subjectSection" />
                    </span>
                    <span class="ValueClass">
                            <asp:TextBox
                                ID="deliverySubject" runat="server" MaxLength="100" meta:resourcekey="subjectSectionText"  CssClass="thingValue" />
                    </span>
                </div>
                <div class="instructionsDiv">
                    <span class="LabelClass">
                            <Corbis:Localize ID="Localize9" runat="server" meta:resourcekey="specialInstructionsSection" />
                    </span>
                    <span class="ValueClassTextArea">
                        <Corbis:TextArea ID="deliverySpecialInstructions" runat="server" TextMode="MultiLine" MaxLength="150" meta:resourcekey="specialInstructionsSectionText"  CssClass="thingValue" />
                    </span>
                </div>
            </div>        
        </div>
        <AJAXToolkit:TextBoxWatermarkExtender ID="deliverySubjectWatermark" runat="server" TargetControlID="deliverySubject"
            WatermarkText="<%$ Resources: Resource, Optional %>" WatermarkCssClass="Optional" />
        <AJAXToolkit:TextBoxWatermarkExtender ID="deliverySpecialInstructionsWatermark" runat="server" TargetControlID="deliverySpecialInstructions"
            WatermarkText="<%$ Resources: Resource, Optional %>" WatermarkCssClass="Optional" />
        
    </Corbis:WorkflowBlock>
</div>

<%--
************************************
* BUTTON BAR 
************************************
--%>

<div class="buttonBar ">
    <!-- "Want to quit checkout?" modal -->
<Corbis:ModalPopup ContainerID="quitCheckout" runat="server" meta:resourcekey="areYouSure">
    <Corbis:GlassButton runat="server" ButtonStyle="Gray" CausesValidation="false" ID="continueCheckout" meta:resourcekey="continue" OnClientClick="HideModal('quitCheckout');return false;" />  
    <Corbis:GlassButton runat="server" CausesValidation="false" ID="quitCheckout" meta:resourcekey="quit" OnClientClick="GoToCartByFlow(true);return false;" />  
</Corbis:ModalPopup>

    <a href="javascript:OpenModal('quitCheckout'); ResizeModal('quitCheckout');"><Corbis:Localize ID="quit" runat="server" meta:resourcekey="quit" /></a>

    <ul class="buttonPositioner">
        <li class="previous">
            <Corbis:GlassButton ID="btnDeliveryPrevious" OnClientClick="checkoutTabs.show(0);UpdateBasedOnCurrentTabIndex({tabIndex:0});return false;" runat="server" CssClass="backStepButton" ButtonStyle="Gray" meta:resourcekey="previousStep"></Corbis:GlassButton>
        </li>
        <li class="next">
            <Corbis:GlassButton
                ID="btnDeliveryNext" OnClientClick="_DeliveryValidation.validateAll();return false;" runat="server" 
                CssClass="nextStepButton" ButtonStyle="Orange" meta:resourcekey="nextStep"
            />
        </li>

    </ul>
</div>

<%--
************************************
* SHIPPING PRIORITY MODAL TEMPLATE 
************************************
--%> 
<Corbis:ModalPopup ID="shippingPriorityModal" ContainerID="shippingPriorityModal" Width="340" runat="server" meta:resourcekey="shippingPriorityModal" CloseScript="MochaUI.HideModal('{0}');return false;">
    <Corbis:DropDownList ID="shippingPriorityCtrl" class="selectOne" runat="server"></Corbis:DropDownList>
       
    <div class="subTitle">
            <Corbis:Localize ID="shipTo" runat="server" meta:resourcekey="shipTo"/>
        <a href="javascript:void(0)" onClick="CorbisUI.Checkout.modals.addNewShippingAddress(); return false;">
            <Corbis:Localize ID="newAddress" runat="server" meta:resourcekey="newAddress"/>
        </a>
<%--
<a href="#" onClick="CorbisUI.Checkout.modals.addShippingAddress('shippingPriorityModalWindow'); return false;">
     <Corbis:Localize ID="newAddress" runat="server" meta:resourcekey="newAddress"/>
</a>
--%>
        
    </div>
        <asp:UpdatePanel ID="shippingSelectPopup" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false" >
        <ContentTemplate>

        <div class="addressItems">
            <asp:Repeater ID="addressList" runat="server" OnItemDataBound="addressList_ItemDataBound" >
            <ItemTemplate>
              <div class="addressItem">
                       <div class="addressItemPadding">
                <asp:RadioButton runat="server" ID="btnKey" GroupName="addressGroup" uid='<%# DataBinder.Eval(Container.DataItem,"AddressUid")%>'  />
                <ul>
                    <li>
                            <span><%# (string)DataBinder.Eval(Container.DataItem, "Name") + (((bool)DataBinder.Eval(Container.DataItem, "IsDefaultForType")) ? " (preferred)" : "") %></span>
                    </li>
                     <li>
                            <span><%# (string)DataBinder.Eval(Container.DataItem, "companyName") %></span>
                    </li>
                     <li>
                           <span> <%# DataBinder.Eval(Container.DataItem,"Address1")%></span>
                    </li>
                    <li>
                            <span><%# DataBinder.Eval(Container.DataItem,"Address2")%></span>
                    </li>
                     <li>
                            <span><%# DataBinder.Eval(Container.DataItem,"Address3")%></span>
                    </li>
                   <li>
                            <span><%# DataBinder.Eval(Container.DataItem,"City") + ","%></span>

                            <span><%# DataBinder.Eval(Container.DataItem, "RegionCode")%></span>

                           <span> <%# DataBinder.Eval(Container.DataItem,"PostalCode")%></span>
                    </li>
                    <li>
                            <span><%# DataBinder.Eval(Container.DataItem,"CountryCode")%></span>
                    </li>
                 </ul>
            </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <script>
            var _shippingAddressCount = <%=addressList.Items.Count %>;
        </script>
</div>
<p class="ML_10" ><Corbis:Localize ID="noSavedAddress" runat="server" meta:resourcekey="noSavedAddress" /></p>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="Informative Error">
    <h5>Shipping policy</h5>
    All items for the same order are shipped in one package to one address. The above address will be used for ALL items in your order. 
    </div>
    <Corbis:GlassButton ID="btnOK" OnClientClick="MochaUI.HideModal('shippingPriorityModal');Addresses_Validate(_shippingAddressCount);return false;" runat="server" Text="OK"  />   
</Corbis:ModalPopup>

<%--
**************************************
* EMAIL CONFIRMATION MODAL TEMPLATE 
**************************************
--%>

<Corbis:ModalPopup ID="emailConfirmationLayer" ContainerID="emailConfirmationLayer" Width="340" runat="server" meta:resourcekey="emailConfirmationLayer">
    <Corbis:Localize ID="emailConfirmationModalInputLabel" runat="server" meta:resourcekey="emailConfirmationModalInputLabel" />
    <input type="text" />
    <div>&nbsp;</div>
    <Corbis:GlassButton 
        ID="btnEmailConfirm" meta:resourcekey="btnSave"
        OnClientClick="MochaUI.CloseModal('emailConfirmationLayer');return false;"
        runat="server" CssClass="" ButtonStyle="Gray"
    />
    <Corbis:GlassButton 
        ID="btnEmailCancel" meta:resourcekey="btnCancel"
        OnClientClick="MochaUI.CloseModal('emailConfirmationLayer');return false;"
        runat="server" CssClass="" ButtonStyle="Orange"
    />
</Corbis:ModalPopup>

<%--
***************************************
* ADD SHIPPING LAYER 2 MODAL TEMPLATE 
***************************************
--%>

<corbis:ModalPopup 
    ID="addShippingLayer3" Title="miketest" 
    ContainerID="addShippingLayer2" Width="390" runat="server" 
    meta:resourcekey="addShippingLayer"
    >
</corbis:ModalPopup>


<%--
************************************
* ADD SHIPPING LAYER MODAL TEMPLATE 
************************************
--%>

<Corbis:ModalPopup ID="addShippingLayer" ContainerID="addShippingLayer" Width="390" CloseScript="MochaUI.HideModal('addShippingLayer');return false;" runat="server" meta:resourcekey="addShippingLayer">
    <div class="FormRow">
        <div class="FormLeft">
            <Corbis:Localize ID="sameAsBusinessAddress" runat="server" meta:resourcekey="sameAsBusinessAddress" />
        </div>
        <div class="FormRight">&nbsp;</div>
    </div>
    <div class="FormRow">
        <div class="FormLeft">
            <Corbis:Localize ID="preferredAddress" runat="server" meta:resourcekey="preferredAddress" />
        </div>
        <div class="FormRight">&nbsp;</div>
    </div>
    <br clear="all" />
    <div class="FormRow">
        <div class="FormLeft">
            <Corbis:Localize ID="nameLabel" runat="server" meta:resourcekey="nameLabel" />
        </div>
        <div class="FormRight">
            <Corbis:TextBox ID="nameTextBox" runat="server"/>
        </div>
    </div>
    <div class="FormRow">
        <div class="FormLeft">
            <Corbis:Localize ID="companyNameLabel" runat="server" meta:resourcekey="companyNameLabel" />
        </div>
        <div class="FormRight">
            <Corbis:TextBox ID="companyNameTextBox" runat="server" meta:resourcekey="companyNameTextBox"/>
        </div>
    </div>
    <div class="FormRow">
        <div class="FormLeft">
            <Corbis:Localize ID="streetAddress1Label" runat="server" meta:resourcekey="streetAddress1Label" />
        </div>
        <div class="FormRight">
            <Corbis:TextBox ID="streetAddress1TextBox" runat="server"  meta:resourcekey="streetAddress1TextBox"/>
        </div>
    </div>
    <div class="FormRow">
        <div class="FormLeft">
            <Corbis:Localize ID="streetAddress2Label" runat="server" meta:resourcekey="streetAddress2Label" />
        </div>
        <div class="FormRight">
            <Corbis:TextBox ID="streetAddress2TextBox" runat="server" meta:resourcekey="streetAddress2TextBox"/>
        </div>
    </div>
    <div class="FormRow">
        <div class="FormLeft">
            <Corbis:Localize ID="streetAddress3Label" runat="server" meta:resourcekey="streetAddress3Label" />
        </div>
        <div class="FormRight">
            <Corbis:TextBox ID="streetAddress3TextBox" runat="server" meta:resourcekey="streetAddress3TextBox"/>
        </div>
    </div>
    <div class="FormRow">
        <div class="FormLeft">
            <Corbis:Localize ID="cityLabel" runat="server" meta:resourcekey="cityLabel" />
        </div>
        <div class="FormRight">
            <Corbis:TextBox ID="cityTextBox" runat="server" meta:resourcekey="cityTextBox"/>
        </div>
    </div>
    <div class="FormRow">
        <div class="FormLeft">
            <Corbis:Localize ID="countryRegionLabel" runat="server" meta:resourcekey="countryRegionLabel" />
        </div>
        <div class="FormRight">
            <select class="selectOne">
               <option><Corbis:Localize ID="Localize1" runat="server" meta:resourcekey="ship1"/></option>
               <option><Corbis:Localize ID="Localize2" runat="server" meta:resourcekey="ship2"/></option>
               <option><Corbis:Localize ID="Localize3" runat="server" meta:resourcekey="ship3"/></option>
            </select> 
        </div>
    </div>
    <div class="FormRow">
        <div class="FormLeft">
            <Corbis:Localize ID="stateProvinceLabel" runat="server" meta:resourcekey="stateProvinceLabel" />
        </div>
        <div class="FormRight">
            <select class="selectOne">
                <option><Corbis:Localize ID="Localize4" runat="server" meta:resourcekey="ship1"/></option>
                <option><Corbis:Localize ID="Localize5" runat="server" meta:resourcekey="ship2"/></option>
                <option><Corbis:Localize ID="Localize6" runat="server" meta:resourcekey="ship3"/></option>
            </select> 
		</div>
    </div>
    <div class="FormRow">
        <div class="FormLeft">
            <Corbis:Localize ID="zipPostalCodeLabel" runat="server" meta:resourcekey="zipPostalCodeLabel" />
        </div>
        <div class="FormRight">
            <Corbis:TextBox ID="zipPostalCodeTextBox" runat="server"/>
        </div>
    </div>
    <div class="FormRow">
        <div class="FormLeft">
            <Corbis:Localize ID="phoneLabel" runat="server" meta:resourcekey="phoneLabel" />
        </div>
        <div class="FormRight">
            <Corbis:TextBox ID="phoneTextBox" runat="server" meta:resourcekey="phoneTextBox"/>
        </div>
    </div>
    <br clear="all" />
    <div>&nbsp;</div>
    <Corbis:GlassButton 
        ID="GlassButton1" meta:resourcekey="btnSave"
        OnClientClick="MochaUI.HideModal('addShippingLayer');return false;"
        runat="server" CssClass="" ButtonStyle="Gray"
    />
    <Corbis:GlassButton 
        ID="GlassButton2" meta:resourcekey="btnCancel"
        OnClientClick="MochaUI.HideModal('addShippingLayer');return false;"
        runat="server" CssClass="" ButtonStyle="Orange"
    />
</Corbis:ModalPopup>



