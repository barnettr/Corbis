<%@ Page Language="C#" 
AutoEventWireup="true" 
CodeBehind="beta.aspx.cs" 
Inherits="Corbis.Web.UI.beta"  
Title="Welcome to Corbis Beta" MasterPageFile="~/MasterPages/NoGlobalNav.Master" EnableViewState="false" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="BetaLandingPage" ContentPlaceHolderID="MainContent" runat="server">

    <div id="BetaFlag" runat="server" class="BetaFlag">&nbsp;</div>
    <div class="logoWrap">&nbsp;</div>
    <div class="betaContainer">
    <div class="rounded4 betaWrap">
	    <h3 class="betaHeader">Welcome to the Corbis beta site.</h3>
        <div class="betaOuterWrap">
            <div class="clr">&nbsp;</div>
            <div class="betaInnerWrap">
                
                <div class="leftSide">
                <p>All content is confidential and owned by Corbis and our licensors. 
                No portion or element may be copied, retransmitted or shared via any means.</p>
                <div class="eulaTextWrap">
                    <div class="eulaText" id="eulaText">
                          <p><Corbis:Label runat="server" ID="lblEulaTitle" /></p>
                          <Corbis:Label runat="server" ID="lblEulaText" />
                    </div>
                </div>
                <div id="betaAcceptTerms" class="betaAcceptTerms">
                    <asp:Panel ID="pnlNotAccepted" runat="server" CssClass="signInError" Visible="False">
                    <asp:Label ID="lblNotAcceptedText" runat="server" ForeColor="Red" Text="Label"></asp:Label></asp:Panel>
                    <Corbis:ImageCheckbox Checked="false" ID="acceptTerms" runat="server" AutoPostback="false" Text="I agree to the above Terms &amp; Conditions"></Corbis:ImageCheckbox>
                    
                </div>
                
                <asp:Panel ID="pnlFailure" runat="server" CssClass="signInError" Visible="False">
                <asp:Label ID="lblFailureText" runat="server" ForeColor="Red" Text="Label"></asp:Label></asp:Panel>
                <br />
                <Corbis:Label ID="errorChecks" runat="server" />
                <div class="signInBlock">
                    <p class="signInHeading"><Corbis:Label runat="server" ID="lblSignIn" meta:resourcekey="lblSignIn" /></p>
                    <asp:Panel ID="pnlNoUsername" runat="server" CssClass="signInError" Visible="False">
                    <asp:Label ID="lblNoUsernameText" runat="server" ForeColor="Red" Text="Label"></asp:Label></asp:Panel>
                    <label>
                        <Corbis:Label ID="lblUsername" runat="server" Text="Username:" meta:resourcekey="lblUsername" />
                        <asp:TextBox ID="txtUsername" MaxLength="255" runat="server"></asp:TextBox>
                    </label>
                    <br />
                    <asp:Panel ID="pnlNoPassword" runat="server" CssClass="signInError" Visible="False">
                    <asp:Label ID="lblNoPasswordText" runat="server" ForeColor="Red" Text="Label"></asp:Label></asp:Panel>
                    <label>
                        <Corbis:Label ID="lblPassword" runat="server" Text="Password:" meta:resourcekey="lblUPassword" />
                        <asp:TextBox ID="txtPassword" MaxLength="50" runat="server" TextMode="Password"></asp:TextBox>
                    </label>
                </div>
                <br />
                <br />
                </div>
                <div class="colDivider"></div>
                <div class="rightSide">
                    <div class="vertMiddle">
                        <Corbis:Label runat="server" ID="lblQuestions" meta:resourcekey="lblQuestions" />
                    </div>
                </div>
                
            </div>
            <div class="clr">&nbsp;</div>
        </div>
        
        <div style="text-align:center;margin:12px 0 12px;">
            
            <Corbis:GlassButton ID="btn" runat="server" Text="Sign In" OnClick="btnSubmit_Click" />
        </div>
    </div>
    </div>

<script type="text/javascript" language="javascript">
    var cb = $('betaAcceptTerms').getElement('div.imageCheckbox');
    setCheckedState(cb, false);
</script>

</asp:Content>
