<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AlbumHeader.ascx.cs" Inherits="Corbis.Web.UI.ImageGroups.AlbumHeader" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register Src="RecentImage.ascx" TagName="RecentImage" TagPrefix="Corbis" %>
<div  class="ALgroupDetailsContainer">
        <div class="darkTopLeftCorner"><img style="" src="../Images/RoundCorner-dark-topleft.gif" style="position: absolute;"/></div>
        <div class="darkBottomLeftCorner"><img src="../Images/RoundCorner-dark-bottomleft.gif"/></div>
        
        
        <div class="leftBlockAL">
            <div class="storySetAL">ALBUM</div>
            <div class="storySetTitleAlbum"><Corbis:Label ID="title" runat="server" /></div>
            <div class="idNumber"><Corbis:Label ID="idNumberLabel" runat="server" meta:resourcekey="idNumberLabel" /><Corbis:Label ID="id" runat="server" /> (<Corbis:Label ID="imageCount" runat="server" /> <Corbis:Label ID="images" runat="server" meta:resourcekey="images" />)</div>
        </div>
        
        <div  class="rightBlockAL">
            <div id="PS" class="PS" runat="server" Visible="true">
                <Corbis:RecentImage ID="recentImage" runat="server" Visible="true" />
                <div class="lightTopRightCorner"><img src="../Images/RoundCorner-light-topright.gif"/></div>
                <div class="lightBottomRightCorner"><img src="../Images/RoundCorner-light-bottomright.gif"/></div>
            </div>
            <div id="ALButton" class="ALButton" runat="server" visible="true">
                <Corbis:GlassButton ID="addalltolightbox" runat="server" Buttonstyle="Orange" ButtonBackground=gray36 meta:resourcekey="MediaSetsButton" />
            </div>
        </div>
        <div class="darkTopRightCorner"><img src="../Images/RoundCorner-dark-topright.gif"/></div>
        <div class="darkBottomRightCorner"><img src="../Images/RoundCorner-dark-bottomright.gif"/></div>
    </div>