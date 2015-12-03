<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BetaFeedback.aspx.cs" Inherits="Corbis.Web.UI.BetaFeedback"
    Title="Beta Feedback" MasterPageFile="~/MasterPages/MasterBase.Master" EnableViewState="false" EnableEventValidation="false" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="BetaFeedback" ContentPlaceHolderID="MainContent" runat="server">

    
    <div id="BetaFlag" runat="server" class="BetaFlag">&nbsp;</div>
    <div class="logoWrap">&nbsp;</div>
    
    
    <div class="rounded4 betaFeedbackWrap">
        <div class="rounded4 betaFormLeft">
            <h3 class="betaHeader">
                <Corbis:Label ID="formTitle" runat="server" meta:resourcekey="formTitle" />
            </h3>
            <div class="betaOuterWrap">
            
               <div class="bff">
                    <p>&nbsp;</p>
                    <p>
                        <Corbis:Localize runat="server" ID="welcomeLbl" meta:resourcekey="welcomeLbl" />
                        <Corbis:Label runat="server" CssClass="Error" ID="MessageBox" Visible="false" />
                    </p>
                    <iframe src="BetaIframeFeedbackForm.aspx?usr=<%= myUsername %>&busr=<%= myBetaUsername %>&em=<%= myEmail %>&ref=<%=Request.ServerVariables["HTTP_REFERER"]%>" frameborder="0" scrolling="no" width="100%" height="460px"></iframe>
                </div>           
                <div class="betaDarkHeader">
                    <Corbis:Label runat="server" ID="lblQuestions" meta:resourcekey="lblQuestions" />
                </div>
            </div>
            <div class="clr">&nbsp;</div>
            <a href="javascript:void(0);" class="M_12" onclick="OpenModal('eulaModal')">Corbis Beta Terms & Conditions</a>
            <div class="clr">
            &nbsp;</div>
        </div>
        
        <div class="rounded4 betaFormRight" id="betaFormRight">
            <h3 class="betaDarkHeader">
                <Corbis:Localize runat="server" ID="Localize1" meta:resourcekey="updateTitle" />
            </h3>
            <Corbis:Repeater id="updatesRepeater" runat="server">
              <ItemTemplate><p><%# Eval("Update") %></p></ItemTemplate> 
            </Corbis:Repeater>
            
            <h3 class="betaDarkHeader" style="display:none;">
                <Corbis:Localize runat="server" ID="faqTitle" meta:resourcekey="faqTitle" /></h3>
                <AJAXToolkit:Accordion ID="faqAccordion" CssClass="FAQAccordion" runat="server" AutoSize="None"
                FadeTransitions="true" HeaderCssClass="PaneHeader" HeaderSelectedCssClass="PaneHeaderExpanded"
                RequireOpenedPane="false">
                <HeaderTemplate>
                    <div id="Div1">
                        <div class="Right">
                            <div class="Center">
                                <a href="javascript:void(0);">
                                    <%# DataBinder.Eval(Container.DataItem, "Question") %></a>
                            </div>
                        </div>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Answer") %>
                </ContentTemplate>
            </AJAXToolkit:Accordion>
         
        </div>
        
                <div class="clr">
            &nbsp;</div>
    </div>
         
    <Corbis:ModalPopup ID="eulaModal" Width="600" runat="server" ContainerID="eulaModal">
        <div class="eulaWrap"><Corbis:Label runat="server" ID="lblEulaText" /></div>
    </Corbis:ModalPopup>
</asp:Content>
