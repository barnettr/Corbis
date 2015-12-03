<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApplyCredit.aspx.cs" Inherits="Corbis.Web.UI.Accounts.ApplyCredit"  MasterPageFile="~/MasterPages/AccountsMaster.Master" Title="<%$ Resources: windowTitle %>" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>

<asp:Content ID="applyCreditContent" ContentPlaceHolderID="accountsContent" runat="server">
    <div>
       <table width="75%">
			<tbody><tr>
			<td>
			<span class="RegularText"><asp:Literal ID="applyCreditTopHeader" runat="server" meta:resourcekey="applyCreditTopHeader" />
			</td>
			</tr>
          </tbody></table>
          </div>
          <br />
    <div>
                    
                    
    <table cellpadding="5">
          <tbody><tr>
			<td class="RegularLink">
			    <Corbis:HyperLink runat="server" ChildWindow="false" text="<%$ Resources: creditApplicationUnitedStates %>"  NavigateUrl = "../corporateAccountApplication/Corbis_Credit_App_US.doc" />  <span class="RegularText"><span><asp:Literal ID="Literal21"  runat="server" Text=" <%$ Resources: creditApplicationMSWordVersion %>" /></span></span>
			</td>
			</tr>
			<tr>
			<td class="RegularLink">
			
			<Corbis:Hyperlink ID="creditApplicationUnitedStatesPDF" runat="server" ChildWindow="false" NavigateUrl="../corporateAccountApplication/Corbis_Credit_App_US.pdf" Text="<%$ Resources: creditApplicationUnitedStates %>" />  <span class="RegularText"><span><asp:Literal ID="Literal2" runat="server" Text=" <%$ Resources: creditApplicationPDFVersion  %>" /></span></span>
			</td>
			</tr>
			<tr>
			<td> </td>
			</tr>
			<tr>
			<td class="RegularLink">
			<Corbis:HyperLink runat="server" ChildWindow="false" text="<%$ Resources: creditApplicationUnitedKingdom%>"  NavigateUrl = "../corporateAccountApplication/Corbis_Credit_App_UK.doc" />  <span class="RegularText"><span><asp:Literal ID="Literal3"  runat="server" Text=" <%$ Resources: creditApplicationMSWordVersion  %>" /></span></span>
			</td>
			</tr>
			<tr>
			<td class="RegularLink">
			<Corbis:Hyperlink runat="server" ChildWindow="false" NavigateUrl="../corporateAccountApplication/Corbis_Credit_App_UK.pdf" Text="<%$ Resources: creditApplicationUnitedKingdom %>" />  <span class="RegularText"><span><asp:Literal ID="Literal4"  runat="server" Text=" <%$ Resources: creditApplicationPDFVersion  %>" /></span></span>
			</td>
			</tr>
			<tr>
			<td> </td>
			</tr>
			<tr>
			<td class="RegularLink">
			<Corbis:HyperLink runat="server" ChildWindow="false" text="<%$ Resources: creditApplicationGermany %>"  NavigateUrl = "../corporateAccountApplication/Corbis_Credit_App_DE.doc" />  <span class="RegularText"><span><asp:Literal ID="Literal5"  runat="server" Text=" <%$ Resources: creditApplicationMSWordVersion  %>" /></span></span>
			</td>
			</tr>
			<tr>
			<td class="RegularLink">
			<Corbis:Hyperlink runat="server" ChildWindow="false" NavigateUrl="../corporateAccountApplication/Corbis_Credit_App_DE.pdf" Text="<%$ Resources: creditApplicationGermany %>" />  <span class="RegularText"><span><asp:Literal ID="Literal6"  runat="server" Text=" <%$ Resources: creditApplicationPDFVersion  %>" /></span></span>
			</td>
			</tr>
			<tr>
			<td> </td>
			</tr>
			<tr>
			<td class="RegularLink">
			<Corbis:HyperLink runat="server" ChildWindow="false" text="<%$ Resources: creditApplicationFrance %>"  NavigateUrl = "../corporateAccountApplication/Corbis_Credit_App_FR.doc" />  <span class="RegularText"><span><asp:Literal ID="Literal7"  runat="server" Text=" <%$ Resources: creditApplicationMSWordVersion  %>" /></span></span>
				</td>		
			</tr>
			<tr>
			<td class="RegularLink">
			<Corbis:Hyperlink runat="server" ChildWindow="false" NavigateUrl="../corporateAccountApplication/Corbis_Credit_App_FR.pdf" Text="<%$ Resources: creditApplicationFrance%>" />  <span class="RegularText"><span><asp:Literal ID="Literal8"  runat="server" Text=" <%$ Resources: creditApplicationPDFVersion  %>" /></span></span>
			</td>
			</tr>
			<tr>
			<td> </td>
			</tr>
			<tr>
			<td class="RegularLink">
			<Corbis:HyperLink runat="server" ChildWindow="false" text="<%$ Resources: creditApplicationPoland %>"  NavigateUrl = "../corporateAccountApplication/Corbis_Credit_App_PL.doc" />  <span class="RegularText"><span><asp:Literal ID="Literal9"  runat="server" Text=" <%$ Resources: creditApplicationMSWordVersion  %>" /></span></span>
				</td>		
			</tr>
			<tr>
			<td class="RegularLink">
			<Corbis:Hyperlink runat="server" ChildWindow="false" NavigateUrl="../corporateAccountApplication/Corbis_Credit_App_PL.pdf" Text="<%$ Resources: creditApplicationPoland %>" />  <span class="RegularText"><span><asp:Literal ID="Literal10"  runat="server" Text=" <%$ Resources: creditApplicationPDFVersion  %>" /></span></span>
			</td>
			</tr>
			<tr>
			<td> </td>
			</tr>
			<tr>
			<td class="RegularLink">
			<Corbis:HyperLink runat="server" ChildWindow="false" text="<%$ Resources: creditApplicationItaly %>"  NavigateUrl = "../corporateAccountApplication/Corbis_Credit_App_IT.doc" />  <span class="RegularText"><span><asp:Literal ID="Literal11"  runat="server" Text=" <%$ Resources: creditApplicationMSWordVersion  %>" /></span></span>
				</td>		
			</tr>
			<tr>
			<td class="RegularLink">
			<Corbis:Hyperlink runat="server" ChildWindow="false" NavigateUrl="../corporateAccountApplication/Corbis_Credit_App_IT.pdf" Text="<%$ Resources: creditApplicationItaly %>" />  <span class="RegularText"><span><asp:Literal ID="Literal12"  runat="server" Text=" <%$ Resources: creditApplicationPDFVersion  %>" /></span></span>
			</td>
			</tr>
			<tr>
			<td> </td>
			</tr>
			<tr>
			<td class="RegularLink">
			<Corbis:HyperLink runat="server" ChildWindow="false" text="<%$ Resources: creditApplicationNetherlands %>"  NavigateUrl = "../corporateAccountApplication/Corbis_Credit_App_NL.doc" />  <span class="RegularText"><span><asp:Literal ID="Literal13"  runat="server" Text=" <%$ Resources: creditApplicationMSWordVersion  %>" /></span></span>
				</td>		
			</tr>
			<tr>
			<td class="RegularLink">
			<Corbis:Hyperlink runat="server" ChildWindow="false" NavigateUrl="../corporateAccountApplication/Corbis_Credit_App_NL.pdf" Text="<%$ Resources: creditApplicationNetherlands %>" />  <span class="RegularText"><span><asp:Literal ID="Literal14"  runat="server" Text=" <%$ Resources: creditApplicationPDFVersion  %>" /></span></span>
			</td>
			</tr>
			<tr>
			<td> </td>
			</tr>
			<tr>
			<td class="RegularLink">
			<Corbis:HyperLink runat="server" ChildWindow="false" text="<%$ Resources: creditApplicationBelgium %>"  NavigateUrl = "../corporateAccountApplication/Credit_Application_Corbis_Belgium_BVBA.doc" />  <span class="RegularText"><span><asp:Literal ID="Literal16"  runat="server" Text=" <%$ Resources: creditApplicationMSWordVersion  %>" /></span></span>
				</td>		
			</tr>
			<tr>
			<td class="RegularLink">
			<Corbis:Hyperlink runat="server" ChildWindow="false" NavigateUrl="../corporateAccountApplication/Credit_Application_Corbis_Belgium_BVBA.pdf" Text="<%$ Resources: creditApplicationBelgium %>" />  <span class="RegularText"><span><asp:Literal ID="Literal17"  runat="server" Text=" <%$ Resources: creditApplicationPDFVersion  %>" /></span></span>
			</td>
			</tr>
			<tr>
			<td> </td>
			</tr>
			<tr>
			<td class="RegularLink">
			<Corbis:HyperLink runat="server" ChildWindow="false" text="<%$ Resources: creditApplicationAustria %>"  NavigateUrl = "../corporateAccountApplication/Credit_Application_Corbis_GmbH_Zweigniederlassung_Wien.doc" />  <span class="RegularText"><span><asp:Literal ID="Literal18"  runat="server" Text=" <%$ Resources: creditApplicationMSWordVersion  %>" /></span></span>
				</td>		
			</tr>
			<tr>
			<td class="RegularLink">
			<Corbis:Hyperlink runat="server" ChildWindow="false" NavigateUrl="../corporateAccountApplication/Credit_Application_Corbis_GmbH_Zweigniederlassung_Wien.pdf" Text="<%$ Resources: creditApplicationAustria %>" />  <span class="RegularText"><span><asp:Literal ID="Literal19"  runat="server" Text=" <%$ Resources: creditApplicationPDFVersion  %>" /></span></span>
			</td>
			</tr>
			
			<tr>
			<td> </td>
			</tr>
			<tr>
			<td class="RegularLink">
			<Corbis:HyperLink runat="server" ChildWindow="false" text="<%$ Resources: creditApplicationAustralia %>"  NavigateUrl = "../corporateAccountApplication/Corbis_Credit_App_AU.doc" />  <span class="RegularText"><span id="Label4"><asp:Literal ID="Literal20"  runat="server" Text=" <%$ Resources: creditApplicationMSWordVersion  %>" /></span></span>
				</td>		
			</tr>
			<tr>
			<td class="RegularLink">
			<Corbis:Hyperlink runat="server" ChildWindow="false" resizable="true" NavigateUrl="../corporateAccountApplication/Corbis_Credit_App_AU.pdf" Text="<%$ Resources: creditApplicationAustralia %>" />  <span class="RegularText"><span id="Label8"><asp:Literal ID="Literal22"  runat="server" Text=" <%$ Resources: creditApplicationPDFVersion  %>" /></span></span>
			</td>
			</tr>
			<tr>
			<td> </td>
			</tr>		
			<tr>
			<td> </td>
			</tr>
          </tbody></table>
    </div>
</asp:Content>
