<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BetaBackToProSiteTracker.aspx.cs" Inherits="Corbis.Web.UI.BetaBackToProSiteTracker"
    Title="Beta Feedback" MasterPageFile="~/MasterPages/MasterBase.Master" EnableViewState="false" EnableEventValidation="false" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="BetaFeedback" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="/Scripts/mootools-1.2.js"></script>
    <style type="text/css">
            /* $$$ ====[ DIMMERS  ]*/
            #redirectProcessingIndicator
            { 
            	
	            left: 0;
	            background-color:transparent;
                overflow: hidden;
                position: absolute;
                right: 0;
                top:180px;
                bottom:0px;
                height:450px;
                z-index:1;
                width: 100%;
                
                
            }
            #redirectProcessingIndicator .mask
            {
	            left: 0;
                opacity: 0.66;
                filter:alpha(opacity = 66);
                overflow: hidden;
                position: fixed;
                right: 0;
                top:180px;
                height:500px;
                bottom:-1000px;
                z-index:1;
                width: 100%;
                background-color: black;
                background: url(/Images/BG_dimmer.png) repeat 0 0;
            }
            #redirectProcessingContents
            {
	            background:transparent;
                color: #1A1A1A;
                margin-top: 200px;
                padding: 10px 5px 5px;
                text-align: center;
                width: 100%;
                min-width: 1008px;
                position: fixed;
                top:15%;left:10%;
                width: auto !important;
                z-index: 505;
                font-weight:bold;

            }
            #redirectConfirmationMsgDiv
            {
                color: #FFFFFF;
                margin-top:18px;
            }

            #redirectProcessingContents h1
            {
                font-size:20px;
                color: #4385C0; 
                height: 50px;
                line-height: 36px;
                margin-left: 10px;
                padding-top: 5px;
            }
            #redirectProcessingContents h1 span.small
            {
                display:block;
                margin-top:-15px;
                font-size:12px;
            }
    </style>
    <div id="redirectProcessingIndicator">
        <div class="mask">
        </div>
        <div id="redirectProcessingContents">
            <img border="0" alt="" src="/images/ajax-loader-dark-transparent.gif" />
            <br />
            <h1>
                <Corbis:Label ID="pleaseStandBy" runat="server" meta:resourcekey="pleaseStandBy" />
                <span class="small"><Corbis:Label ID="weAreRedirecting" runat="server" meta:resourcekey="weAreRedirecting" /></span>
            </h1>
            <div id="redirectConfirmationMsgDiv">
                <Corbis:Label ID="thisMayTake" runat="server" meta:resourcekey="thisMayTake" />
            </div>
        </div>
    </div>
    <div style="min-height:400px;">&nbsp;</div>
</asp:Content>
