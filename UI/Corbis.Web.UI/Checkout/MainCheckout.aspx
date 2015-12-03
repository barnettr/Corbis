<%@ Page Language="C#" AutoEventWireup="true" Codebehind="MainCheckout.aspx.cs" Inherits="Corbis.Web.UI.Checkout.MainCheckout"
    MasterPageFile="~/MasterPages/NoSearchBar.Master" Title="<%$ Resources: windowTitle %>" %>

<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ PreviousPageType VirtualPath="~/Checkout/Cart.aspx" %>
<%@ Register TagPrefix="Corbis" TagName="ItemSummary" Src="~/Checkout/ItemSummary.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="Steps" Src="~/Checkout/Steps.ascx" %>
<%@ Register TagPrefix="Corbis" TagName="ContactCorbis" Src="~/UserControls/ContactControls/ContactCorbis.ascx" %>

<asp:Content ID="checkoutContent" runat="server" ContentPlaceHolderID="mainContent">
<%--
*******************************
* JAVASCRIPT GLOBAL VARS 
*******************************
--%> 
    <script language="javascript" type="text/javascript">
        // have to acutally load this first since some of the other
        // controls are calling this global var to set variables
        CorbisUI.GlobalVars.MainCheckout = {
            // this needs to have a variable passed to it
            // whether there is a valid payment method or not
            invalidPayment: <% if(!this.IsCreditEnable && !this.HasCorporateAccount){ %>true<% }else{ %>false<% } %>
        };
    </script>
<%--
*******************************
* MAIN LAYOUT 
*******************************
--%>
    <div class="checkoutBar">
        <div class="Info">
            <h2>
                <Corbis:WorkflowBlock WorkflowType="COFF" runat="server">
                    <Corbis:Localize ID="pageTitleCOFF" runat="server" meta:resourcekey="pageTitleCOFF" />
                </Corbis:WorkflowBlock>
                <Corbis:WorkflowBlock WorkflowType="CART" runat="server">
                    <Corbis:Localize ID="pageTitle" runat="server" meta:resourcekey="pageTitle" />
                </Corbis:WorkflowBlock>                    
                <span class="orderContains">
                <Corbis:Localize ID="yourOrder" runat="server" meta:resourcekey="yourOrder" />
                <span id="itemCount" runat="server">6</span> 
               <Corbis:Localize ID="items" runat="server" meta:resourcekey="items" />.</span></h2>
            
        </div>
        <div class="totalIndicator">
            <ul>
                <li class="totalLabel">
                    <Corbis:Localize ID="total" runat="server" meta:resourcekey="total" />
                </li>
                <li class="totalAmount">
                    <asp:UpdatePanel ID="totalCostUpdatePanel" runat="server" UpdateMode="Conditional" RenderMode="Inline" >
                        <ContentTemplate>
                            <span id="totalCost" runat="server" ></span>    
                        </ContentTemplate>
                    </asp:UpdatePanel>
                 </li>
            </ul>
        </div>
    </div>
    <div class="contentBar">
        <div id="checkoutStage" class="wrap rounded">
            <div id="checkoutStageWrap">
                <div class="ColumnedLayout TwoColumn_Fixed_LM" id="checkoutDisplayColumns">
		            <div class="TwoColumn_leftWrap">
			            <div class="TwoColumn_mainWrap">
				            <div class="TwoColumn_mainColumn">
					            <Corbis:Steps runat="server" ID="steps" />
				            </div>
				            
			            </div>
			            <div class="TwoColumn_leftColumn">
				            <Corbis:ItemSummary runat="server" ID="itemSummaryBlock" />
			            </div>
		            </div>	
	            </div>
                
                
                <div id="bottomBlock">
                    <ul id="questions">
                        <li class="securityQuestions"><a href="javascript:void(0)" onclick="CorbisUI.Checkout.modals.securityQuestions(this); return false;">
                       <Corbis:Localize ID="securityQ" runat="server" meta:resourcekey="securityQ" />
                        </a></li>
                        <li class="SDquestions"><a href="javascript:void(0)" onclick="CorbisUI.Checkout.modals.shippingDeliveryQuestions(this); return false;">
                       <Corbis:Localize ID="shippingQ" runat="server" meta:resourcekey="shippingQ" /> 
                       </a></li>
                    </ul>
                    <a href="javascript:CorbisUI.Legal.OpenSAMIModal();"  id="TRUSTeLink" runat="server"><img src="/Images/truste.gif" alt="" id="TRUSTe" class="TRUSTe" runat="server" /></a>

                </div>
            </div>
            <asp:UpdatePanel runat="server" ID="importantBlock" UpdateMode="Conditional"  >
                <ContentTemplate >
                    <div class="agessa" runat="server" id="importantSection" visible="false" >
                        <h3><Corbis:Localize ID="importantText" runat="server" meta:resourcekey="importantText" /></h3>
                        <p>
                            <corbis:localize ID="taxError" Visible="false" runat="server" meta:resourcekey="taxErrorMsg"  />
                        </p>
                        <p>
                            <corbis:localize ID="agessa" Visible="false" runat="server" meta:resourcekey="agessaMsg"  />
                        </p>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
 

<%--
**************************************
* SECURITY QUESTIONS MODAL TEMPLATE 
**************************************
--%> 
 
          
      <Corbis:ModalPopup ID="securityQuestionsModal" ContainerID="securityQuestionsModal" Width="500"

        runat="server" meta:resourcekey="securityQuestionsModal">
        
            <p><Corbis:Localize ID="privacyPolicyMessage" runat="server" meta:resourcekey="privacyPolicyMessage" /></p>

            <p><Corbis:Localize ID="privacyPolicyCoveringAreas" runat="server" meta:resourcekey="privacyPolicyCoveringAreas" /></p>

            <ul>
                <li><Corbis:Localize ID="personalInformationLabel" runat="server" meta:resourcekey="personalInformationText" /></li>
                <li><Corbis:Localize ID="paymentInformationLabel" runat="server" meta:resourcekey="paymentInformationText" /></li>
                <li><Corbis:Localize ID="cookiesLabel" runat="server" meta:resourcekey="cookiesText" /></li>
                <li><Corbis:Localize ID="emailLabel" runat="server" meta:resourcekey="emailText" /></li>
            </ul>

            <p><strong><Corbis:Localize ID="personalInformationLabel2" runat="server" meta:resourcekey="personalInformationText" /></strong><br />
                <Corbis:Localize ID="internalPurposesMessage" runat="server" meta:resourcekey="internalPurposesMessage" /></p>

            <ul>
                <li><Corbis:Localize ID="processYourOrderText" runat="server" meta:resourcekey="processYourOrderText" /></li>
                <li><Corbis:Localize ID="maintainUpdateAccount" runat="server" meta:resourcekey="maintainUpdateAccountText" /></li>
                <li><Corbis:Localize ID="sendSpecialOffersLabel" runat="server" meta:resourcekey="sendSpecialOffers" /></li>
            </ul>


            <p><Corbis:Localize ID="corbisPolicyPara1" runat="server" meta:resourcekey="corbisPolicyPara1" /></p>

            <p><Corbis:Localize ID="corbisPolicyPara2" runat="server" meta:resourcekey="corbisPolicyPara2" /></p>

            <p><strong><Corbis:Localize ID="paymentInformationLabel2" runat="server" meta:resourcekey="paymentInformationText" /></strong><br />
            <Corbis:Localize ID="encryptionPolicyText" runat="server" meta:resourcekey="encryptionPolicyText" /></p>

            <p><strong><Corbis:Localize ID="cookiesLabel2" runat="server" meta:resourcekey="cookiesText" /></strong><br />
            <Corbis:Localize ID="cookiesDetailLabel" runat="server" meta:resourcekey="cookiesDetailText" /></p>

            <p><strong><Corbis:Localize ID="emailLabel2" runat="server" meta:resourcekey="emailText" /></strong><br />
            <Corbis:Localize ID="emailNotificationLabel" runat="server" meta:resourcekey="emailNotificationText" />
            <a href="mailto:service@corbis.com">service@corbis.com</a>. </p>
        
    </Corbis:ModalPopup>
    
    
<%--
************************************
* SHIPPING DELIVERY MODAL TEMPLATE 
************************************
--%>
     <Corbis:ModalPopup ID="shippingDeliveryModal" ContainerID="shippingDeliveryModal" Width="500"
        runat="server" meta:resourcekey="shippingDeliveryModal">
        <Corbis:Localize ID="aboutShipping" runat="server" meta:resourcekey="aboutShipping" />
    </Corbis:ModalPopup>

    
    
    <Corbis:ModalPopup ID="sessionTimeout" ContainerID="sessionTimeout" runat="server" Title="<%$ Resources:Resource, sessionTimeoutTitle %>"    CloseScript="handleSessionTimeout();return false;">
        <Corbis:Localize ID="sessionTimeoutMessage" runat="server" Text="<%$ Resources:Resource, sessionTimeoutMessage %>" />
		<Corbis:GlassButton CssClass="closeSuccess"   ID="closeSessionTimeout" runat="server" OnClientClick="handleSessionTimeout();return false;" ButtonStyle="Orange" ButtonBackground="dbdbdb"  meta:resourcekey="closeButton" />
	</Corbis:ModalPopup>
	
	<Corbis:LinkButton ID="sessionTimeoutRedirect" runat="server" OnClick="SessionTimeOut_Click" CssClass="displaynone" ></Corbis:LinkButton>
	<Corbis:ContactCorbis runat="server" />
	
<script>
    function handleSessionTimeout() {
        HideModal('sessionTimeout');
        __doPostBack($('<%=sessionTimeoutRedirect.ClientID %>').id.replace(/_/g, '$'), ''); 
    }
</script>
</asp:Content>

