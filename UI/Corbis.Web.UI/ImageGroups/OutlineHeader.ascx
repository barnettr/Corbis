<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="OutlineHeader.ascx.cs" Inherits="Corbis.Web.UI.ImageGroups.OutlineHeader" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register Src="RecentImage.ascx" TagName="RecentImage" TagPrefix="Corbis" %>

<div class="groupDetailsContainer">
        <div class="darkTopLeftCorner"><img style="" src="../Images/RoundCorner-dark-topleft.gif" style="position: absolute;"/></div>
        <div class="darkBottomLeftCorner"><img src="../Images/RoundCorner-dark-bottomleft.gif"/></div>
        
        <div id="leftBlockOL" class="leftBlockOL" Visible="true" runat="server">
            <div id="mainLabelOL" class="mainLabelOL" Visible="true" runat="server"><img src="../Images/outline-logo.gif" /> &nbsp;&nbsp;<Corbis:Label ID="imageCount" runat="server"  Visible="true"/></div>
            <div id="photographedByOL" class="photographedBy" Visible="true" runat="server"><Corbis:Label ID="PhotographedBy" runat="server" meta:resourcekey="PhotographedBy" /> <Corbis:Label ID="photographer" runat="server" /></div>
            <div id="datePublishedOL" class="datePublished" Visible="true" runat="server"><Corbis:Label ID="DatePublishedTxt" runat="server" meta:resourcekey="DatePublished" /> <Corbis:Label ID="datePublished" runat="server" /></div>
            <div id="byItalicsOL" class="byItalics" Visible="true" runat="server"><Corbis:Label ID="creditline" runat="server"></Corbis:Label></div>
            <div id="OL_leftButton" class="OL_leftButton" runat="server" visible="true">
                <Corbis:GlassButton ID="addalltolightbox" runat="server" Buttonstyle="Orange" ButtonBackground=gray36 meta:resourcekey="button" />
                <%-- View Slideshow to be built in a later iteration --%>
                <%--<a href="#"><div id="ViewSlideShowDiv" class="viewSlideShow"></div><Corbis:Label ID="SlideShow" runat="server" CssClass="slideShow" meta:resourcekey="SlideShow" /></a>--%>
            </div> 

        </div>
                   
        <div id="rightBlockOL" class="rightBlockOL" visible="true" runat="server">
        
            <div id="RFCD" class="RFCD" runat="server" visible="true">
                <Corbis:RecentImage ID="recentImage" runat="server" Visible="true" />
                <div class="darkTopRightCorner"><img src="../Images/RoundCorner-light-topright.gif"/></div>
                <div class="darkBottomRightCorner"><img src="../Images/RoundCorner-light-bottomright.gif"/></div>
            </div> 
            
            <div id="scrollBoxOL" runat="server" visible="true" class="scrollBoxOL">
                <div class="scrollBoxOLContent">
                    <asp:Label ID="Label1" CssClass="scrollBoxOLTitle"  runat="server" meta:resourcekey="feature" ></asp:Label><br />
                    <!-- <div ID="ltCelebrities" CssClass="scrollBoxOLContent" runat="server" Width="198" Height="88" EnableViewState="false" AutoPostBack="false" onclick="javascript:setTimeout('__doPostBack(\'ctl00$mainContent$OutlineHeader$ltCelebrities\',\'\')', 0);"></div> -->
                    <Corbis:Repeater ID="featuringRepeater" runat="server" OnItemDataBound="featuringRepeater_ItemDataBound">
                        <ItemTemplate>
                            <div class="featuringItem">
                                <a id="featuringLink" runat="server"><Corbis:Label id="featuringText" runat="server" /></a>
                            </div>
                        </ItemTemplate>
                    </Corbis:Repeater>
                </div>
            </div>
                        
            <div class="darkTopRightCorner"><img src="../Images/RoundCorner-dark-topright.gif"/></div>
            <div class="darkBottomRightCorner"><img src="../Images/RoundCorner-dark-bottomright.gif"/></div>
        
        </div>
        
        
</div>

<script language="javascript">
    window.onload = function()
    {
        document.getElementsByName("__EVENTTARGET")[0].value = "";
             
    }
    //window.attachEvent("onload", cleanup); 
    function resizeOutlineHeader()
    {
      $(document.body).getElement('.groupDetailsContainer').setStyle('height', '60px');
    } 
</script>