<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StorySetHeader.ascx.cs" Inherits="Corbis.Web.UI.ImageGroups.StorySetHeader" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register Src="RecentImage.ascx" TagName="RecentImage" TagPrefix="Corbis" %>
<div  class="groupDetailsContainer">
        <div class="darkTopLeftCorner"><img style="" src="../Images/RoundCorner-dark-topleft.gif" style="position: absolute;"/></div>
        <div class="darkBottomLeftCorner"><img src="../Images/RoundCorner-dark-bottomleft.gif"/></div>
        
        
        <div class="leftBlock">
            <div class="storySet"><Corbis:Label ID="storySetHeaderTitle" runat="server" meta:resourcekey="storySetHeaderTitle" /></div>
            <div class="storySetTitle"><Corbis:Label ID="title" runat="server" /></div>
            <div class="idNumber"><Corbis:Label ID="idNumberLabel" runat="server" meta:resourcekey="idNumberLabel" /><Corbis:Label ID="id" runat="server" /> (<Corbis:Label ID="imageCount" runat="server" /> <Corbis:Label ID="images" runat="server" meta:resourcekey="images" />)</div>
            <div class="date" id="dateLocation" runat="server" visible="true"><Corbis:Label ID="date" runat="server" /><Corbis:Label ID="location" runat="server" /> </div>
            <div>
                <div class="creditLabel"><Corbis:Label ID="PhotographedBy" runat="server" meta:resourcekey="PhotographedBy" />&nbsp;</div>
                <div class="creditContent"><Corbis:Label ID="photographer" runat="server" /></div>
            </div>
        </div>
        <div  class="rightBlockOL">
         
            <div id="RFCD" class="RFCD" runat="server" visible="true">
                <Corbis:RecentImage ID="recentImage" runat="server" Visible="true" />
                <div class="lightTopRightCorner"><img src="../Images/RoundCorner-light-topright.gif"/></div>
                <div class="lightBottomRightCorner"><img src="../Images/RoundCorner-light-bottomright.gif"/></div>
            </div>
                <div id="pullDownButton" class="pullDownButtonStory" runat="server" visible="true">
                    <Corbis:GlassButton ID="addalltolightbox" runat="server" CssClass="mediaSetsButton" Buttonstyle="Orange" ButtonBackground=gray36 meta:resourcekey="MediaSetsButton" />
                </div>
            <div class="darkTopRightCorner"><img src="../Images/RoundCorner-dark-topright.gif"/></div>
            <div class="darkBottomRightCorner"><img src="../Images/RoundCorner-dark-bottomright.gif"/></div>
        </div>
        
</div>
<script language="javascript" type="text/javascript">

window.addEvent('domready', function() {
    var mover = new captionMover();
});
    
</script>