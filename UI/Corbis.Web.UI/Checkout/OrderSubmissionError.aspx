<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/NoSearchBar.Master" 
    AutoEventWireup="true" CodeBehind="OrderSubmissionError.aspx.cs" 
    Inherits="Corbis.Web.UI.Checkout.OrderSubmissionError" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="UC" TagName="InstantService" src="../Chat/UserControl/InstantServiceButtonSmall.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="server">

    <div class="checkoutBar">
        <div class="Info">
            <h2>
                <Corbis:Localize ID="pageTitle" runat="server" meta:resourcekey="pageTitle" /> 
            </h2>
        </div>
    </div>
    <div class="contentBar">
        <div id="checkoutStage" class="wrap rounded">
            <div id="OrderError">
                 
		     <div  class="ChatDiv" meta:resourcekey="chatDiv" runat="server">
			     <UC:InstantService id="instantService1" runat="server" />
		    </div>
                <h2 class="Error_ICN"><Corbis:Localize ID="errorTitle" runat="server" meta:resourcekey="errorTitle" /></h2>
                <p><Corbis:Localize ID="notProcessedString" runat="server" meta:resourcekey="notProcessedString" /></p>
                <p><Corbis:Localize ID="apology" runat="server" meta:resourcekey="apology" /></p>
                <br /><br />
                <Corbis:GlassButton ID="btnContactCorbis" runat="server" 
                ButtonStyle="Orange" ButtonBackground="gray36"
                meta:resourcekey="btnContactCorbis" />  
            </div>
        </div>
    </div>
</asp:Content>
