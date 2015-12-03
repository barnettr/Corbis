<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="RFCDHeader.ascx.cs" Inherits="Corbis.Web.UI.ImageGroups.RFCDHeader" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register Src="RecentImage.ascx" TagName="RecentImage" TagPrefix="Corbis" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>


<div id="HeaderControlWrapper" runat="server">
    <div class="darkTopLeftCorner"><img alt="" src="../Images/RoundCorner-dark-topleft.gif" /></div>
    <div class="darkBottomLeftCorner"><img alt="" src="../Images/RoundCorner-dark-bottomleft.gif" /></div>
    
    <div class="rfcdImage">
        <asp:Image ID="rfcdImage" Height="90" Width="90" runat="server" />
    </div>
    
    <div class="leftBlockRFCD">
        <div class="mainLabelRFCD"><Corbis:Label ID="rfcdHeaderLabel" runat="server" meta:resourcekey="rfcdHeaderLabel"  /></div>
        <div class="imageTitleRFCD"><asp:Label ID="title" runat="server" /></div>
        <div class="imageNumberRFCD"><Corbis:Label ID="id" runat="server" /> (<Corbis:Label ID="imageCount" runat="server" /> <Corbis:Label ID="images" runat="server" meta:resourcekey="images" />, <span id="royaltyFreeToolTipLink" class="color1"  onmouseover="openRoyaltyFreeToolTipModal();return false;"  onmouseout="return hideRoyaltyFreeToolTipModal();"><Corbis:Label ID="fileSize" runat="server" /></span>)</div>            
        <div class="RFCDCopyright">&copy; <asp:Label ID="copyRight" runat="server" /><asp:Label ID="copyrightText" runat="server" meta:resourcekey="copyrightText" /> </div>
    </div>
    
    <div  class="rightBlockOL">
        <asp:HiddenField ID="rfcdUid" runat="server" />
        
        <div id="RFCD" class="RFCD" runat="server" visible="true">
            <Corbis:RecentImage ID="recentImage" runat="server" Visible="true" />
            <div class="darkTopRightCorner"><img src="../Images/RoundCorner-light-topright.gif"/></div>
            <div class="darkBottomRightCorner"><img src="../Images/RoundCorner-light-bottomright.gif"/></div>
        </div>
        
        <div id="buttonBoxLabel" runat="server" visible="true" class="buttonBoxLabel">
            <asp:Label ID="price" runat="server" />
        </div>
        
        <div id="rfcdRightButtons" class="rfcdRightButtons">
            <asp:UpdatePanel ID="RFCDAddCDtoCartPanel" UpdateMode="conditional" runat="server">
                <ContentTemplate>
                    <div id="RFCDButton1" runat="server" visible="true" class="RFCDButton1" >
                        <Corbis:GlassButton ID="RFCDAddCDtoCart" OnClick="addtoCart_Click" OnClientClick="javascript:CorbisUI.Cart.AddToCartInst().addRFCDToCart(this);return true;" runat="server" CssClass="" Buttonstyle="Orange" ButtonBackground=gray36 meta:resourcekey="RFCDAddCDtoCart" />
                    </div>
                </ContentTemplate>  
            </asp:UpdatePanel>
            
            <div id="RFCDButton2" runat="server" class="RFCDButton2" visible="true">
                <Corbis:GlassButton ID="addCDtolightbox" runat="server" CssClass="" Buttonstyle="Orange" ButtonBackground=gray36 meta:resourcekey="RFCDAddCDtoLBox" OnClientClick="javascript:CorbisUI.Cart.AddToCartInst().addRFCDToLightbox();return true;"/>
            </div>
            
            <div id="rightBlockLink2" runat="server" class="rightBlockLink2" visible="true">
                <Corbis:HyperLink ID="AddAllImages"  runat="server" CssClass="RFCDAddAllImage" meta:resourcekey="RFCDAddAllImage" />
                
            </div>
        </div>
             
        <asp:HiddenField ID="allLinkVisible" runat="server" Value="false" />
        <div class="darkTopRightCorner"><img src="../Images/RoundCorner-dark-topright.gif"/></div>
        <div class="darkBottomRightCorner"><img src="../Images/RoundCorner-dark-bottomright.gif"/></div>
    </div>
    
    <%-- Start Royalty Free File Size Tool Tip repeater --%>
    <div id="rfcdFileSizeToolTip"  style="display:none;">
        <div id="rfcdFileSizeToolTipModal" >
            <div id="rfcdFileSizeToolTipDataContainer" class="rfcdFileSizeToolTipDataContainer">
                <%--<div><span id="rfcdFileSizeToolTipTitle" class="rfcdFileSizeToolTipTitle"><Corbis:Localize ID="rfcdFileSizeToolTipTitleLabel" runat="server" meta:resourcekey="rfcdFileSizeToolTipTitleLabel" /></span></div>--%>
                <asp:Label ID="web" runat="server" meta:resourcekey="web" /><br />
                <asp:Label ID="small" runat="server" meta:resourcekey="small" /><br />
                <asp:Label ID="medium" runat="server" meta:resourcekey="medium" /><br />
                <asp:Label ID="large" runat="server" meta:resourcekey="large" /><br />
                <asp:Label ID="xlarge" runat="server" meta:resourcekey="xlarge" />
                             
                <div class="rfcdFileSizeToolTipNote"><asp:Label runat="server" ID="rfcdFileSizeToolTipNote" meta:resourcekey="rfcdFileSizeToolTipNote" /></div>
            </div>
        </div>
    </div>
    <%-- End Royalty Free File Size Tool Tip repeater --%>
    
</div>


<script language="javascript" type="text/javascript">
   function openRoyaltyFreeToolTipModal()
{
    if ($('rfcdFileSizeToolTipModalWindow'))
    {
        $('rfcdFileSizeToolTipModalWindow').setStyle('display','block');
    } else {
        var el = $('rfcdFileSizeToolTipModal');
	    el.setStyle('display','block');
        var elDimensions = el.getCoordinates();
	    var properties = {
	            title: '<%= Corbis.Web.UI.CorbisBasePage.EncodeToJsString(GetLocalResourceObject("RFCDToolTip").ToString()) %>',
		        collapsible: false,
			    minimizable: false,
		        contentBgColor: '#e8e8e8',
		        headerStartColor: [219,219,219],
		        headerStopColor: [219,219,219],
		        bodyBgColor: [232,232,232],		        
		        useCanvasControls: false,
		        cornerRadius: 4,
		        headerHeight: 32,
		        footerHeight: 4,
			    padding: 0,
			    shadowBlur: 9,
        	    scrollbars: false,
        	    closable: false,
        	    type: 'window',
			    id: el.getProperty('id') + "Window",
		        height: 150,
		        width: 300,
		        x: 540,
		        y: 250,
		        content: '',
		        draggable: false,
		        resizable: true
		    };
		    MochaUI.NewWindowFromDiv(el, properties);
		    ResizeModal('rfcdFileSizeToolTip');
    }
}


function hideRoyaltyFreeToolTipModal()
{
    try{$('rfcdFileSizeToolTipModalWindow').setStyle('display','none');}catch(er){}
}
window.addEvent('domready', function() {
    var RFCDnumber = $('<%=id.ClientID %>');
    if (RFCDnumber != null) {
        $(RFCDnumber).addClass('RFCDnumber');
    }
});
    
</script>

<script language="javascript" type="text/javascript">

    Sys.Net.WebRequestManager.add_invokingRequest(onInvoke);
    Sys.Net.WebRequestManager.add_completedRequest(onComplete);
    function onInvoke(sender , args) {
    }
    function onComplete(sender, args) {
        if(addingRFCD) {
            CorbisUI.Cart.AddToCartInst().displaySuccessDialog();
        }
        addingRFCD = false;
    }

    
    CorbisUI.ImageGroups = {
        AllLinkVisible1: '<%= allLinkVisible.ClientID %>'
    };

    window.addEvent('domready', function() {
        var getAllLinkVisible = $(CorbisUI.ImageGroups.AllLinkVisible1);
        if (getAllLinkVisible.value == 'false' || getAllLinkVisible.value == 'False') {
            try {
                $('SearchColumnedContent').getElement('div .imageAndArrowContainer_AddLink').addClass('imageAndArrowContainer');
                $('SearchColumnedContent').getElement('div .imageAndArrowContainer_AddLink').removeClass('imageAndArrowContainer_AddLink');
            } catch(e) { }
        }
    });  
</script>