<%@ Page Language="C#" 
AutoEventWireup="true" 
CodeBehind="unsupportedBrowser.aspx.cs" 
Inherits="Corbis.Web.UI.unsupportedBrowser"  
Title="<%$ Resources: windowTitle %>" MasterPageFile="~/MasterPages/BadBrowser.Master" EnableViewState="false" %>
<%@ Import Namespace="Corbis.Framework.Globalization"%>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
	Namespace="System.Web.UI" TagPrefix="aspx" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>





<asp:Content ID="UnsupporterBrowserPage" ContentPlaceHolderID="MainContent" runat="server">

    <div id="BetaFlag" runat="server" class="BetaFlag">&nbsp;</div>
    <table border="1" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td class="SorryBadBrowser" align="center">
                <div style="width: 100%;" align="center">
                    <div id="browserName" runat="server" class="hdn"></div>
                    <div id="browserVersion" runat="server" class="hdn"></div>
                    <div id="browserData" runat="server" class="hdn"></div>
                    <table border="1" cellpadding="0" cellspacing="0" width="600">
                        <tr>
                            <td class="logoRow"><img src="/Images/corbis-logo.gif" /></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="optimalExperience"><Corbis:Localize ID="optimalParagraph" runat="server" meta:resourcekey="optimalParagraph" /></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="browserList">
                                <ul>
                                    <li><a href="http://www.microsoft.com/windows/downloads/ie/getitnow.mspx"><Corbis:Localize ID="IEStringText" runat="server" meta:resourcekey="IEString" /></a></li>
                                    <li><a href="http://www.mozilla.com/en-US/firefox/personal.html"><Corbis:Localize ID="FirefoxStringText" runat="server" meta:resourcekey="FirefoxString" /></a></li>
                                    <li><a href="http://www.apple.com/safari/download/"><Corbis:Localize ID="SafariStringText" runat="server" meta:resourcekey="SafariString" /></a></li>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="upgradeLater"><Corbis:Localize ID="upgradeLaterNotice" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="installAssistance"><Corbis:Localize ID="upgradeAssistanceParagraph" runat="server" meta:resourcekey="upgradeAssistanceParagraph" /></td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    

</asp:Content>
