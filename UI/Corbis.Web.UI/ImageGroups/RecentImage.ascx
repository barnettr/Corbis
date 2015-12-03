<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RecentImage.ascx.cs" Inherits="Corbis.Web.UI.ImageGroups.recentImage" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
 <div id="greyImageContainer" class="greyImageContainer RFCD" runat="server">
        <div id="ImageAndArrowContainer" class="imageAndArrowContainer" style="float:left;" runat="server"><!--[ Bug no. 18361: these styles need to be inline ]-->
            <Corbis:CenteredImageContainer  Size="50" ImageID="image" ID="thumbWrap" IsAbsolute="true" runat="server" /><Corbis:Hyperlink id="arrow" class="arrowContainer" runat="server">&nbsp;</Corbis:Hyperlink> 
        </div>
        <div class="lightTopRightCorner"><img src="../Images/RoundCorner-light-topright.gif"/></div>
        <div class="lightBottomRightCorner"><img src="../Images/RoundCorner-light-bottomright.gif"/></div>
</div>