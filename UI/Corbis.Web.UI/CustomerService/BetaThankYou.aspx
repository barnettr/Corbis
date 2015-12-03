<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BetaThankYou.aspx.cs" Inherits="Corbis.Web.UI.BetaThankYou"
    Title="Beta Feedback Thank You" MasterPageFile="~/MasterPages/NoGlobalNav.Master" EnableViewState="false" %>

<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="BetaFeedback" ContentPlaceHolderID="MainContent" runat="server">
    <div id="BetaFlag" runat="server" class="BetaFlag">&nbsp;</div>
    <div class="logoWrap">&nbsp;</div>
    <div class="rounded4  betaThanksYouWrap">
        <div class=" betaThanksInnerWrap">
            <h3>Thanks for your feedback!</h3>
            <p><Corbis:Label runat="server" ID="lblThanksMessage" meta:resourcekey="lblThanksMessage" /></p>
            <div class="clr">&nbsp;</div>
            <div class="clr">&nbsp;</div>
            <div class="clr">&nbsp;</div>
            <div class="clr">&nbsp;</div>
            <div class="clr">&nbsp;</div>
            <div class="clr">&nbsp;</div>
            <div class="buttonHolder">
            <Corbis:GlassButton runat="server" ID="returnToBeta" OnClientClick="returnBetaHome();return false;" Text="Return to Corbis beta site" />
                <div class="clr">&nbsp;</div>
                <div class="clr">&nbsp;</div>
                <Corbis:GlassButton runat="server" ButtonStyle="Gray" OnClientClick="proSiteTrackEvent();return false;" ID="goToProSite" Text="Go to old Corbis site" />
                <div class="clr">&nbsp;</div>
                <div class="clr">&nbsp;</div>
                <div class="clr">&nbsp;</div>
                <div class="clr">&nbsp;</div>
            </div>
            <div class="betaThanksBottom"><Corbis:Label runat="server" ID="lblQuestions" meta:resourcekey="lblQuestions" /></div>
            
        </div>
        
    </div>
    <script type="text/javascript">
    function returnBetaHome(){
        window.location="/";
    }
    function proSiteTrackEvent(){
		//Call Omniture tracking per search
		s.pageName="LND:Home:Beta:BackToCorbisPro"
        s.channel=""
        s.pageType=""
        s.prop1=""
        /* E-commerce Variables */
        s.campaign=""
        s.state=""
        s.zip=""
        s.events=""
        s.products=""
        s.purchaseID=""
        s.eVar1="<%= myUsername %>"
        s.eVar2="<%= myEmail %>"
        
        //confirm(s_account + s.pageName + s.server);
        
        try{
		    return s_gi(s_account, s.pageName, s.server);
		}catch(ex){
    	    //alert(ex);
    	}finally{
    		window.location="/CustomerService/BetaBackToProSiteTracker.aspx";
        }
    	
     }
     
     </script>
</asp:Content>
