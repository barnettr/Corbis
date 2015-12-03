<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="CustomerService.aspx.cs" Inherits="Corbis.Web.UI.CustomerService.CustomerService" MasterPageFile="~/MasterPages/MasterBase.Master" Title="<%$ Resources: windowTitle %>" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="UC" TagName="InstantService" src="../Chat/UserControl/InstantServiceButtonSmall.ascx" %>


<asp:Content ID="customerServiceContent" ContentPlaceHolderID="mainContent" runat="server">
	<div id="CustomerServiceRound" class="rc5px clear">
        <div class="Top">
            <div class="Left"><div class="Right"><div class="Center">&nbsp;</div></div></div>
        </div>
    </div>
    <div id="CustomerServiceContent">
     <div class="CustomerServiceContentDisplay">
        <div class="TitleBar">
            <div class="Title"><Corbis:Localize ID="pageTitle" runat="server" meta:resourcekey="pageTitle" /></div>
		    <div class="ChatDiv" runat="server">
                <UC:InstantService id="instantService1" runat="server" />
            </div>
        </div>
        <div class="LeftAccordion">
            <%-- Corbis Offices starts --%>
            <asp:UpdatePanel ID="OfficesPanel" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
            <div class="PaneHeaderExpanded">
                <div id="officesPaneHeader">
                    <div class="Right">
                        <div class="Center">
                            <Corbis:Localize ID="officesTitle" runat="server" meta:resourcekey="officesTitle" />
                        </div>
                    </div>
                </div>
                <div class="Offices">
                    <div class="PaneContent">
                        <br />
                        <Corbis:Label ID="officeListLabel" runat="server" meta:resourcekey="officeListLabel" />
                        <Corbis:DropDownList ID="officeList" runat="server" OnSelectedIndexChanged="officeList_SelectedIndexChanged" AutoPostBack="true" />
                        <br />
                        <br />
                        <Corbis:Localize ID="officeLocation" runat="server"></Corbis:Localize>
                    </div>
                </div>
            </div>
            </ContentTemplate>            
            </asp:UpdatePanel>
            <%-- Corbis Offices ends--%>
            <div class="PaneContentRound rc5px clear MB_10">
                <div class="Bottom">
                    <div class="Left"><div class="Right"><div class="Center">&nbsp;</div></div></div>
                </div>
            </div>
            <%-- FAQ section Header and Content--%>
            <div class="PaneHeaderExpanded">
                <div id="faqPaneHeader">
                    <div class="Right">
                        <div class="Center">
                            <Corbis:Localize ID="faqTitle" runat="server" meta:resourcekey="faqTitle" />
                        </div>
                    </div>
                </div>
                <%-- FAQ section Content starts--%>
                <div class="Faq">
                    <div class="PaneContent">
                        <div class="Content">

                            <%-- FAQ MyAccount starts--%>
                            <AJAXToolkit:Accordion ID="faqAccordion" CssClass="FAQAccordian" runat="server" AutoSize="None" FadeTransitions="true" 
                            RequireOpenedPane="false" SelectedIndex="-10" >
                            <Panes>
                            <AJAXToolkit:AccordionPane ID="AccordianPane1" runat="server">
                                <Header>
                                    <div class="faqHeader">
                                         <Corbis:Localize ID="myAccountTitle" runat="server" meta:resourcekey="myAccountTitle" />
                                    </div>
                                </Header>
                            </AJAXToolkit:AccordionPane>
                            </Panes>
                             <Panes>		                          
                               <AJAXToolkit:AccordionPane ID="AccordionPane2" runat="server" >
	                            <Header>
		                               <div class="Question">
                                           <a href="javascript:void(0);"><Corbis:Localize ID="Question1" runat="server" meta:resourcekey="Question1" /></a>
                                       </div>
	                            </Header>
	                            <Content>
                                       <div class="Answer">
                                            <Corbis:Localize ID="Answer1" runat="server" meta:resourcekey="Answer1" />
                                       </div>                                                                     					
	                            </Content>			                            
                               </AJAXToolkit:AccordionPane>		                               
                               <AJAXToolkit:AccordionPane ID="AccordionPane3" runat="server" >
	                            <Header>
		                               <div class="Question">
                                            <a href="javascript:void(0);"><Corbis:Localize ID="Question2" runat="server" meta:resourcekey="Question2" /></a>
                                       </div>
	                            </Header>
	                            <Content>
                                       <div class="Answer">
                                             <Corbis:Localize ID="Answer2" runat="server" />
                                       </div>                            					
	                            </Content>
                               </AJAXToolkit:AccordionPane>
                               <AJAXToolkit:AccordionPane ID="AccordionPane4" runat="server" >
	                            <Header>
		                               <div class="Question">
                                            <a href="javascript:void(0);"><Corbis:Localize ID="Question3" runat="server" meta:resourcekey="Question3" /></a>
                                       </div>
	                            </Header>
	                            <Content>
                                       <div class="Answer">
                                            <Corbis:Localize ID="Answer3" runat="server" meta:resourcekey="Answer3" />
                                       </div>                           					
	                            </Content>
                               </AJAXToolkit:AccordionPane>
                               <AJAXToolkit:AccordionPane ID="AccordionPane5" runat="server" >
	                            <Header>
		                               <div class="Question">
                                            <a href="javascript:void(0);"><Corbis:Localize ID="Question4" runat="server" meta:resourcekey="Question4" /></a>
                                        </div>
	                            </Header>
	                            <Content>
                                        <div class="Answer">
                                             <Corbis:Localize ID="Answer4" runat="server"  />
                                        </div>
                                </Content>
                               </AJAXToolkit:AccordionPane>
                               </Panes>
                               <%-- FAQs MyAccount ends--%>
                               <%-- FAQs Lightboxes starts--%>
                              
                              <Panes>
                                   <AJAXToolkit:AccordionPane ID="AccordianPane6" runat="server">
                                       <Header>
                                           <div class="faqHeader">
                                                        <Corbis:Localize ID="Localize1" runat="server" meta:resourcekey="LightboxesTitle" />
                                           </div>
                                       </Header>
                                   </AJAXToolkit:AccordionPane>
                               </Panes>
                               <Panes>                               
                               <AJAXToolkit:AccordionPane ID="AccordionPane7" runat="server" >
	                            <Header>
		                               <div class="Question">
                                           <a href="javascript:void(0);"><Corbis:Localize ID="Question5" runat="server" meta:resourcekey="Question5" /></a>
                                       </div>
	                            </Header>
	                            <Content>
                                       <div class="Answer">
                                            <Corbis:Localize ID="Answer5" runat="server" meta:resourcekey="Answer5" />
                                       </div>                            					
	                            </Content>
                               </AJAXToolkit:AccordionPane>
                               <AJAXToolkit:AccordionPane ID="AccordionPane8" runat="server" >
	                            <Header>
		                               <div class="Question">
                                           <a href="javascript:void(0);"><Corbis:Localize ID="Question6" runat="server" meta:resourcekey="Question6" /></a>
                                       </div>
	                            </Header>
	                            <Content>
                                       <div class="Answer">
                                            <Corbis:Localize ID="Answer6" runat="server" meta:resourcekey="Answer6" />
                                       </div>                          					
	                            </Content>
                               </AJAXToolkit:AccordionPane>
                               <AJAXToolkit:AccordionPane ID="AccordionPane9" runat="server" >
	                            <Header>
		                               <div class="Question">
                                           <a href="javascript:void(0);"><Corbis:Localize ID="Question7" runat="server" meta:resourcekey="Question7" /></a>
                                       </div>
	                            </Header>
	                            <Content>
                                       <div class="Answer">
                                            <Corbis:Localize ID="Answer7" runat="server" />
                                       </div>                            					
	                            </Content>
                               </AJAXToolkit:AccordionPane>
                               <AJAXToolkit:AccordionPane ID="AccordionPane10" runat="server" >
	                            <Header>
		                               <div class="Question">
                                           <a href="javascript:void(0);"><Corbis:Localize ID="Question8" runat="server" meta:resourcekey="Question8" /></a>
                                       </div>
	                            </Header>
	                            <Content>
                                       <div class="Answer">
                                            <Corbis:Localize ID="Answer8" runat="server" meta:resourcekey="Answer8" />
                                       </div>                            					
	                            </Content>
                               </AJAXToolkit:AccordionPane>
                            </Panes>
                            <%-- FAQs Lightboxes end --%>
                            <%-- FAQs Purchasing Images starts--%>
                              <Panes>
                                   <AJAXToolkit:AccordionPane ID="AccordianPane7" runat="server">
                                       <Header>
                                            <div class="faqHeader">
                                              <Corbis:Localize ID="PurchasingImagesTitle" runat="server" meta:resourcekey="PurchasingImagesTitle" />
                                            </div>
                                       </Header>
                                   </AJAXToolkit:AccordionPane>
                               </Panes>
                               <Panes>                               
                               <AJAXToolkit:AccordionPane ID="AccordionPane12" runat="server" >
	                            <Header>
		                               <div class="Question">
                                           <a href="javascript:void(0);"><Corbis:Localize ID="Question9" runat="server" meta:resourcekey="Question9" /></a>
                                       </div>
	                            </Header>
	                            <Content>
                                       <div class="Answer">
                                            <Corbis:Localize ID="Answer9" runat="server" meta:resourcekey="Answer9" />
                                       </div>                            					
	                            </Content>
                               </AJAXToolkit:AccordionPane>
                               <AJAXToolkit:AccordionPane ID="AccordionPane13" runat="server" >
	                            <Header>
		                               <div class="Question">
                                           <a href="javascript:void(0);"><Corbis:Localize ID="Question10" runat="server" meta:resourcekey="Question10" /></a>
                                       </div>
	                            </Header>
	                            <Content>
                                       <div class="Answer">
                                            <Corbis:Localize ID="Answer10" runat="server"  />
                                       </div>                          					
	                            </Content>
                               </AJAXToolkit:AccordionPane>
                               <AJAXToolkit:AccordionPane ID="AccordionPane14" runat="server" >
	                            <Header>
		                               <div class="Question">
                                           <a href="javascript:void(0);"><Corbis:Localize ID="Question11" runat="server" meta:resourcekey="Question11" /></a>
                                       </div>
	                            </Header>
	                            <Content>
                                       <div class="Answer">
                                            <Corbis:Localize ID="Answer11" runat="server" meta:resourcekey="Answer11" />
                                       </div>                            					
	                            </Content>
                               </AJAXToolkit:AccordionPane>
                               <AJAXToolkit:AccordionPane ID="AccordionPane15" runat="server" >
	                            <Header>
		                               <div class="Question">
                                           <a href="javascript:void(0);"><Corbis:Localize ID="Question12" runat="server" meta:resourcekey="Question12" /></a>
                                       </div>
	                            </Header>
	                            <Content>
                                       <div class="Answer">
                                            <Corbis:Localize ID="Answer12" runat="server" meta:resourcekey="Answer12" />
                                       </div>                            					
	                            </Content>
                               </AJAXToolkit:AccordionPane>
                               <AJAXToolkit:AccordionPane ID="AccordionPane16" runat="server" >
	                            <Header>
		                               <div class="Question">
                                           <a href="javascript:void(0);"><Corbis:Localize ID="Question13" runat="server" meta:resourcekey="Question13" /></a>
                                       </div>
	                            </Header>
	                            <Content>
                                       <div class="Answer">
                                            <Corbis:Localize ID="Answer13" runat="server" meta:resourcekey="Answer13" />
                                       </div>                            					
	                            </Content>
                               </AJAXToolkit:AccordionPane>
                               <AJAXToolkit:AccordionPane ID="AccordionPane17" runat="server" >
	                            <Header>
		                               <div class="Question">
                                           <a href="javascript:void(0);"><Corbis:Localize ID="Question14" runat="server" meta:resourcekey="Question14" /></a>
                                       </div>
	                            </Header>
	                            <Content>
                                       <div class="Answer">
                                            <Corbis:Localize ID="Answer14" runat="server" meta:resourcekey="Answer14" />
                                       </div>                            					
	                            </Content>
                               </AJAXToolkit:AccordionPane>
                            </Panes>
                            <%-- FAQ's Purchasing Images ends--%>
                            </AJAXToolkit:Accordion>
                        </div>
                    </div>
                </div>
                <%-- FAQ section ends --%>
            </div>
            <%-- FAQ section Header and Content ends--%>
            <div class="PaneContentRound rc5px clear MB_20">
                <div class="Bottom">
                    <div class="Left"><div class="Right"><div class="Center">&nbsp;</div></div></div>
                </div>
            </div>
        </div>
        <div class="RightAccordion">
            <div class="PaneHeaderExpanded">
                    <div id="myRepPaneHeader" onclick=""><div class="Right"><div class="Center">
                        <Corbis:Localize ID="myRepTitle" runat="server" meta:resourcekey="myRepTitle" />
                    </div></div></div>
                    <div class="MyRep">
                        <div class="PaneContent">
                            <div class="ContactNameLabel"><Corbis:Label ID="contactName" runat="server"/></div>
                            <div class="ContactDetails"><Corbis:Label ID="officeName" runat="server"/><br />
                            <div>
                            <Corbis:Label ID="address1" runat="server" />
                            <Corbis:Label ID="address2" runat="server" />
                            <Corbis:Label ID="address3" runat="server" />
                            </div>
                                <div>
                                    <Corbis:Label ID="city" runat="server" />
                                    <Corbis:Label ID="regionCode" runat="server" />
                                    <Corbis:Label ID="postalCode" runat="server" />
                                </div>
                                <div>
                                    <Corbis:Label ID="country" runat="server" />
                                </div>
                            </div>
                            <div class="ContactDetails"><b><Corbis:Localize ID="phoneNumberLabel" runat="server" meta:resourcekey="phoneNumberLabel" /></b>
                                <Corbis:Label ID="phoneNumber" runat="server" /><br /></div>
                            <div class="ContactDetails"><b><Corbis:Localize ID="faxNumberLabel" runat="server" meta:resourcekey="faxNumberLabel" /></b>
                                <Corbis:Label ID="faxNumber" runat="server" /><br /></div>
                            <div id="emailDiv" runat="server" class="ContactDetails">
                                <b><Corbis:Localize ID="emailAddressLabel" runat="server" meta:resourcekey="emailAddressLabel" /></b>
                                <Corbis:HyperLink ID="emailAddress" runat="server" /><br />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="PaneContentRound rc5px clear MB_10">
                    <div class="Bottom">
                        <div class="Left"><div class="Right"><div class="Center">&nbsp;</div></div></div>
                    </div>
                </div>
                <div class="PaneHeaderExpanded">
                    <div id="getInTouchPaneHeader" onclick=""><div class="Right"><div class="Center">
                        <Corbis:Localize ID="contactUsTitle" runat="server" meta:resourcekey="contactUsTitle" />
                    </div></div></div>
                    <div class="GetInTouch">
                        <div class="PaneContent" id="validationPane">
                            <Corbis:ValidationHub 
                                ID="validationHub" runat="server" IsPopup="false" 
                                SubmitForm="true" ContainerID="CustomerServiceContent"
                                InitScript="initProvinceBehavior();"
                            />
                            <table cellspacing="0" id="formTable">
                            <tr class="FormRow">
                                <td class="FormLeft">
                                     <Corbis:Localize ID="AboutLabel" runat="server" meta:resourceKey="AboutLabel"/>
                                </td>
                                
                                <td class="FormRight">
                                            
                                <Corbis:DropDownList ID="aboutDropDown" runat="server"/>
                               </td>
                            </tr>
                            <tr id="generalFirstName" runat="server" class="FormRow">
                                <td class="FormLeft">
                                     <Corbis:Localize ID="firstnameLabel" runat="server" meta:resourceKey="firstnameLabel"></Corbis:Localize>
                                </td>
                                <td class="FormRight">
                                   <Corbis:TextBox validate="required" meta:resourcekey="firstName" ID="firstName" runat="server"  />
                                </td>
                            </tr>
                            <tr id="generalLastName" runat="server" class="FormRow">
                                <td class="FormLeft">
                                    <Corbis:Localize ID="lastNameLabel" runat="server" meta:resourcekey="lastNameLabel"/>
                                </td>
                                <td class="FormRight">
                                    <Corbis:TextBox validate="required"  meta:resourcekey="lastName" ID="lastName" runat="server" />
                                </td>
                            </tr>
                            <tr id="asianLastName" runat="server" class="FormRow">
                                <td class="FormLeft">
                                    <Corbis:Localize ID="lastNameAsianLabel" runat="server" meta:resourcekey="lastNameLabel"/>
                                </td>
                                <td class="FormRight">
                                    <Corbis:TextBox validate="required" ID="lastNameAsian" meta:resourcekey="lastName" runat="server" />
                                </td>
                            </tr>
                            <tr id="asianFirstName" runat="server" class="FormRow">
                                <td class="FormLeft">
                                     <Corbis:Localize ID="firstnameAsianLabel" runat="server" meta:resourceKey="firstnameLabel"/>
                                </td>
                                <td class="FormRight">
                                   <Corbis:TextBox validate="required" ID="firstNameAsian" meta:resourcekey="firstName" runat="server"  />
                                </td>
                            </tr>
                            <tr class="FormRow">
                                 <td class="FormLeft">
                                    <Corbis:Localize ID="emailLabel" runat="server"  meta:resourcekey="emailLabel"/>
                                </td>
                                <td class="FormRight">
                                    <Corbis:TextBox validate="required;email" meta:resourcekey="email" ID="email" runat="server" /> 
                                </td>
                            </tr>
                            <tr class="FormRow">
                                <td class="FormLeft">
                                     <Corbis:Localize ID="telephoneLabel" runat="server"  meta:resourcekey="telephoneLabel"/>
                                </td>
                                <td class="FormRight">
                                    <Corbis:TextBox validate="required;phone" meta:resourcekey="telephone" ID="telephone" runat="server" />
                                </td>
                            </tr>
                            <tr class="FormRow">
                                <td class="FormLeft"><Corbis:Localize ID="countryLabel" runat="server" meta:resourcekey="countryLabel" /></td>
                                <td class="FormRight">
                                    <Corbis:DropDownList ID="countryList" validate="custom1" custom1=_provinceBehavior.validateCountry(true) meta:resourcekey="country" runat="server" />
                                </td>
                            </tr>
                            <tr class="FormRow">
                                <td class="FormLeft"><Corbis:Localize ID="stateLabel" runat="server" meta:resourcekey="stateLabel" /></td>
                                <td class="FormRight">
                                    <Corbis:DropDownList Enabled="false" validate="custom1" custom1="_provinceBehavior.validateProvince(true)" meta:resourcekey="stateList" ID="stateList" runat="server"/>
                                    <asp:HiddenField ID="provinceName" runat="server" />
                                    <asp:HiddenField ID="provinceCode" runat="server" />
                                    
                                </td>
                            </tr>
                            <tr class="FormRow">
                                <td class="FormLeft clear commentsSpacing">
                                    <Corbis:Localize ID="commentsLabel" runat="server" meta:resourcekey="commentsLabel"/>
                                </td>
                                <td class="FormRight">
                                    <Corbis:TextBox ID="comments" validate="required" name="comments" runat="server" TextMode="MultiLine" style="height: 96px" meta:resourcekey="comments" />
                                </td>
                            </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="alignRight buttonPadding">
                  <!--<span onclick="_formValidation.validateAll()">validate!</span> -->
                    <Corbis:GlassButton ID="save" runat="server" 
                        meta:resourcekey="Save" validate="submit" CssClass="buttonsSpacing" 
                    />
                    <Corbis:LinkButton ID="lb" runat="server" OnClick="Save_Click"  CssClass="ValidateClickLB displayNone" />
                    <Corbis:GlassButton ButtonStyle="Gray" ID="cancel" 
                        CausesValidation="false" meta:resourcekey="Cancel" CssClass="buttonsSpacing"
                        runat="server" OnClientClick="CorbisUI.CustomerService.clearGetInTouchForm(); return false;" 
                    />    
                </div>
                <div class="PaneContentRound rc5px clear MB_10">
                    <div class="Bottom">
                        <div class="Left"><div class="Right"><div class="Center">&nbsp;</div></div></div>
                    </div>
                </div>
        </div>
     </div>
    </div>
    <div id="CustomerServiceRound" class="rc5px clear">
        <div class="Bottom">
            <div class="Left"><div class="Right"><div class="Center">&nbsp;</div></div></div>
        </div>
    </div>
    
<Corbis:ModalPopup ID="ModalPopup1" ContainerID="getThankYou" runat="server" Width="300" meta:resourcekey="getThankYou">
    
    <Corbis:Label ID="getMessage" runat="server" CssClass="getMessage" meta:resourcekey="getMessage"></Corbis:Label>
    <br />
    <Corbis:GlassButton ID="GlassButton1" runat="server" CausesValidation="false" meta:resourcekey="Close" OnClientClick="HideModal('getThankYou');return false;" /> 
    
</Corbis:ModalPopup>
<script>
    var _provinceBehavior;
    function initProvinceBehavior() {
        _provinceBehavior = new CorbisUI.FormUtilities.ProvinceBehavior({
            countriesDropdownId: '<%=countryList.ClientID %>',
            provinceDropdownId: '<%=stateList.ClientID %>',
            provinceNameHdnId: '<%=provinceName.ClientID %>',
            provinceCodeHdnId: '<%=provinceCode.ClientID %>',
            ajaxUrl: '/Common/CommonWebService.asmx/GetStates',
            validationClass: <%=validationHub.ClientInstanceVariableName %>
        });
    }
   
</script>
</asp:Content>
