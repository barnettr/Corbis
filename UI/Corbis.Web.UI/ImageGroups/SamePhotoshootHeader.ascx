﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SamePhotoshootHeader.ascx.cs" Inherits="Corbis.Web.UI.ImageGroups.SamePhotoshootHeader" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register Src="RecentImage.ascx" TagName="RecentImage" TagPrefix="Corbis" %>
<div  class="ALgroupDetailsContainer">
        <div class="darkTopLeftCorner"><img style="" src="../Images/RoundCorner-dark-topleft.gif" style="position: absolute;"/></div>
        <div class="darkBottomLeftCorner"><img src="../Images/RoundCorner-dark-bottomleft.gif"/></div>
        
        <div class="leftBlock">
            <div class="storySetTitlePS"><Corbis:Label ID="samePhotoshootHeaderTitle" runat="server" meta:resourcekey="samePhotoshootHeaderTitle" /></div>
            <div class="idNumberPS"><Corbis:Label ID="idNumberLabel" runat="server" meta:resourcekey="idNumberLabel" /><Corbis:Label ID="id" runat="server" /> (<Corbis:Label ID="imageCount" runat="server" /> <Corbis:Label ID="images" runat="server" meta:resourcekey="images" />)</div>        
            <div>
                <div class="creditLabel"><Corbis:Label ID="PhotographedBy" runat="server" meta:resourcekey="PhotographedBy" />&nbsp;</div>
                <div class="creditContent"><Corbis:Label ID="photographer" runat="server" /></div>
            </div>
        </div>
        <div  class="rightBlockAL">
         
            <div id="PS" class="PS" runat="server" visible="true">
                <Corbis:RecentImage ID="recentImage" runat="server" Visible="true" />
                <div class="darkTopRightCorner"><img src="../Images/RoundCorner-light-topright.gif"/></div>
                <div class="darkBottomRightCorner"><img src="../Images/RoundCorner-light-bottomright.gif"/></div>
            </div>
            <div id="PSButton2" runat="server" visible="true" class="PSButton2">
                <Corbis:GlassButton ID="addalltolightbox" runat="server" Buttonstyle="Orange" ButtonBackground=gray36 meta:resourcekey="MediaSetsButton" />
            </div>
        </div>
        <div class="darkTopRightCorner"><img src="../Images/RoundCorner-dark-topright.gif"/></div>
        <div class="darkBottomRightCorner"><img src="../Images/RoundCorner-dark-bottomright.gif"/></div>
    </div>