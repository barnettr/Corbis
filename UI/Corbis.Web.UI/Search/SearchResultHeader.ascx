<%@ Control Language="C#" AutoEventWireup="true" Codebehind="SearchResultHeader.ascx.cs"
    Inherits="Corbis.Web.UI.Search.SearchResultHeader" %>
<%@ Register TagPrefix="AJAXToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="AJAXExtentions" Namespace="AjaxControlToolkit.WCSFExtensions"
    Assembly="AjaxControlToolkit.WCSFExtensions" %>
<%@ Register TagPrefix="Corbis" Namespace="Corbis.Web.UI.Controls" Assembly="Corbis.Web.UI.Controls" %>
<%@ Register TagPrefix="Corbis" TagName="Pager" Src="~/src/Pager/Pager.ascx" %>
<%@ Register Src="../ImageGroups/RecentImage.ascx" TagName="RecentImage" TagPrefix="Corbis" %>
<%@ Register TagPrefix="Corbis" TagName="SortBlock" Src="SortBlock.ascx" %>
<div>
    <% if (ShowHeader)
       { %>
       
    <div id="header1" class="rounded4">
        
        <div id="header1ResultTitle" class="header1ResultTitle">
            <Corbis:Localize ID="resultTitle" runat="server"></Corbis:Localize>
        </div>
        <div id="recentImage" class="recentImageDisplay" runat="server" visible="false">
            <Corbis:RecentImage ID="recentImageSelected" runat="server" visible="false"/>
        </div>
        <Corbis:SortBlock runat="server" ID="sortBlock"  OnGenericCommand="sortBlock_Sort" />
        <div class="clr extraSmallClear" ></div>
             
    </div>
    <% } %>
    <div id="header2" style="margin:0px;">
    <div id="header2Div" runat="server">
        <Corbis:Label runat="server" ID="indexInfo" CssClass="floatLeft labelInSearchHeader" />
        <div id="imagePaging">
            <Corbis:Pager ID="searchResultPager" runat="server" OnPageCommand="PageChanged" 
                PrevCssClass="PrevButton" NextCssClass="NextButton" 
                PrevDisabledCssClass="PrevButtonDisabled" NextDisabledCssClass="NextButtonDisabled"
                PageNumberCssClass="NavPageNumber" PrefixLabelCssClass="PagerLabelPreFix" PostfixLabelCssClass="PagerLabelPostFix" EnableViewState="true" />
        </div>
        <div id="headerlist" runat="server">
            <span class="floatLeft seperaterInSearchHeader"></span>
            <Corbis:LinkButtonList ID="itemsPerPageList" runat="server" CssClass="itemsPerPageList floatLeft"
                OnControlCommand="PageSizeChanged" Spacing="10px" />
            <span class="floatLeft seperaterInSearchHeader"></span>
            <Corbis:Label ID="Label1" runat="server" meta:resourcekey="preview" CssClass="floatLeft labelInSearchHeader"></Corbis:Label>
            <Corbis:LinkButtonList ID="previewList" runat="server" CssClass="itemsPerPageList floatLeft"
                Spacing="10px" ClientScriptFunction="setPreviewPref" />
            <span class="floatLeft seperaterInSearchHeader"></span>
            <div id="optionDiv" runat="server">
            <div id="optionsContainer" style="float: left; cursor: pointer;" onclick="OpenDisplayOptions(); return false;">
                <Corbis:Label ID="displayOptions" runat="server" meta:resourcekey="displayOptions"
                    CssClass="floatLeft labelInSearchHeader"></Corbis:Label>
                <div class="arrowMeDown" id="arrowMeDown" runat="server">
                </div>
            </div>
            </div>
            <div id="optionsContainerClose" style="float: left; cursor: pointer;
                display: none;" onclick="hideDisplayOptionsWindow(); return false;">
                <Corbis:Label ID="displayOptionsClose" runat="server" meta:resourcekey="displayOptions"
                    CssClass="floatLeft labelInSearchHeader"></Corbis:Label>
                <div class="arrowMeDownClose">
                </div>
            </div>
        </div>
        <div class="clr" > </div>
        </div>
    </div>
    <%--MORE DISPLAY OPTIONS MODAL POPUP--%>
    <div id="moreDisplayOptions" style="width:205px;height:48px;display:none;position:absolute;top:0px;left:0px; border:solid 1px #1a1a1a;">
        <%--<div id="thumbContentContainer" class="thumbContentContainer">
            <Corbis:Label ID="thumbnailLayout" runat="server" CssClass="thumbNail" meta:resourcekey="thumbnailLayout" />
            
            <div id="thumbnailOnlyContainer">
                <div id="thumbnailOnly" class="thumbnailOnly"></div> 
                <Corbis:Label ID="thumbOnlyText" runat="server" CssClass="thumbNailDark" meta:resourcekey="thumbOnlyText" />
            </div>
            
            <div class="clr" > </div>
            
            <div id="thumbnailPlusLabelsContainer">
                <div id="thumbnailPlusLabels" class="thumbnailPlusLabels"></div> 
                <Corbis:Label ID="thumbnailPlusLabelsText" runat="server" CssClass="thumbNailWhite" meta:resourcekey="thumbnailPlusLabelsText" />
            </div>
            
            <div class="clr" > </div>
            
            <div id="thumbnailPlusDetailssContainer">
                <div id="thumbnailPlusDetails" class="thumbnailPlusDetails"></div> 
                <Corbis:Label ID="thumbnailPlusDetailsText" runat="server" CssClass="thumbNailDetails" meta:resourcekey="thumbnailPlusDetailsText" />
            </div>
        
        </div>--%>
        <asp:HiddenField ID="showTermsClarification"  runat="server" />            
       <%-- <div id="seperatorLine" class="seperatorLine"></div>--%>      
        <div id="lowerThumbContentContainer" class="thumbContentContainer">
            <Corbis:Label ID="termClarifications" runat="server" CssClass="thumbNail" meta:resourcekey="termClarifications"/>
            
            <div id="warningContainer" style="margin:0;">
                <span id="onWarning" class="onWarning"></span>
                <Corbis:LinkButton ID="clarificationOnLink" runat="server" CssClass="warningOn disabled"   OnClientClick="javascript:enableClarification();return false; "  meta:resourcekey="warningOn"  />
                <span id="offWarning" class="offWarning"></span>
                <Corbis:LinkButton ID="clarificationOffLink" runat="server" CssClass="warningOff"  OnClientClick="javascript:disableClarification();return false;" meta:resourcekey="warningOff"  />                
            </div>
        
        </div>
        
<%--        <div id="infoTop" class="infoTop" title="Tool Tip Messaging-TBD"></div>
        <div id="infoBottom" class="infoBottom"></div>--%>
    
    </div>
    <%--END MORE DISPLAY OPTIONS MODAL POPUP--%>
    
</div>




<script type="text/javascript" language="javascript">
if(!CorbisUI.QueueManager.has('searchDomReady')){
    CorbisUI.QueueManager.addQueue('searchDomReady', { canRerun: true, delay: true });
}
CorbisUI.QueueManager.searchDomReady.addItem('headerSetup', function(){
    
    var temp = $('<%=recentImage.ClientID %>');
    if(temp){
    
        var verticalMiddleElements = new Hash({
            'header1ResultTitle': 'header1'
        });


        verticalMiddleElements.each(function(innerElement, outerElement){
            verticalMiddleGlobal(innerElement, outerElement);
            
        });
    }
    $(document.body).addEvent('click',function(e)
    {
     if($('sortOptionsMenuDiv')!= null)
     {
        $('sortOptionsMenuDiv').setStyle('display', 'none');
     }
    });
});

var _isSearchFlyoutMousedown = false;

var showTermsClarification =$('<%=showTermsClarification.ClientID%>');
//CorbisUI.Search.displayOptionsClarifications(true);

function defaultClarifications()
{
 var showch= $(showTermsClarification);
    if(showch.value =='' ||showch.value == "True")
    {
        enableClarification();
    }
    else
    {
        disableClarification();
    }
}
// Calling DOM ready event only if it is part of Search Results page
if (window.location.href.toLowerCase().contains('searchresults.aspx') || window.location.href.toLowerCase().contains('imagegroups.aspx'))
  {
    //window.addEvent('domready', defaultClarifications);
    CorbisUI.QueueManager.searchDomReady.addItem('defaultClarifications',defaultClarifications);
  }

// Display Options Properties for 
// search and lightbox "Display Options" button
var DOP = {
		    title: '',
	        collapsible: false,
		    minimizable: false,
	        contentBgColor: '#929292',
	        headerStartColor: [74,74,74],
	        headerStopColor: [65,65,65],
	        bodyBgColor: [54,54,54],		        
	        useCanvasControls: false,
	        cornerRadius: 0,
	        headerHeight: 0,
	        footerHeight: 0,
		    padding: 0,
    	    scrollbars: false,
    	    closable: false,
    	    type: 'window',
	        x: 590,
	        y: 258,
	        content: '',
	        draggable: false,
	        resizable: false
	    };
	    
var extendDOP = {
	        
	    }
	    
var ImageGroupsDisplayOptions = false; 	    
if (window.location.href.toLowerCase().contains('imagegroups.aspx')) {
    ImageGroupsDisplayOptions = true;
}
if (ImageGroupsDisplayOptions) {
    DOP.x = 537;
    DOP.y = 358;
}	    
// if the Display Options is coming
// from the lightbox page
if (typeof (LightboxDOP) != 'undefined') {
    DOP.x = 537;
    DOP.y = 333;
}

function OpenDisplayOptions()
{   
    if ($('moreDisplayOptionsWindow'))
    {
        $('optionsContainer').setStyle('display', 'none');
	    $('optionsContainerClose').setStyle('display', 'block');
        $('moreDisplayOptionsWindow').setStyle('display','block');
    } else {
        var el = $('moreDisplayOptions');
	    el.setStyle('display','block');
        var elDimensions = el.getCoordinates();
        var ExtendProps =
        {
            id: el.getProperty('id') + "Window",
		    height: elDimensions.height,
		    width: elDimensions.width
        }
	    var properties = $extend(DOP,ExtendProps);
	    
		MochaUI.NewWindowFromDiv(el, properties);
		$('optionsContainer').setStyle('display', 'none');
		$('optionsContainerClose').setStyle('display', 'block');
        $(document.body).addEvent('mousedown',detectDisplayOptionsclick.bindWithEvent($('moreDisplayOptionsWindow')));

    }
}

function cleanupAllPopups()
{
    if($('sortOptionsMenuDiv')!= null)
    {
        $('sortOptionsMenuDiv').setStyle('display', 'none');
    }
}

function detectDisplayOptionsclick(ev){
    var MSOcor = this.getCoordinates();
    if(
        ((ev.page.y < MSOcor.top) ||(ev.page.y > (MSOcor.top + MSOcor.height)))
        ||
        (ev.page.x < MSOcor.left) || (ev.page.x >(MSOcor.left + MSOcor.width))
    ){
        (function(){hideDisplayOptionsWindow()}).delay(10);
    }
}

function hideDisplayOptionsWindow()
{
    if (!_isSearchFlyoutMousedown)
        try{
            $('optionsContainer').setStyle('display', 'block');
	        $('optionsContainerClose').setStyle('display', 'none');
            $('moreDisplayOptionsWindow').setStyle('display','none');
        } catch(e) { }
    _isSearchFlyoutMousedown = false;
}

function closeMochaModal()
{
    $('optionsContainer').setStyle('display', 'block');
	$('optionsContainerClose').setStyle('display', 'none');
	$('moreDisplayOptionsWindow').setStyle('display','none');
}

//var showclarificationstatus = true;

function setClarificationCookie(clarificationOn)
{
    var currentSetting = $(showTermsClarification).value.toLowerCase();

    // only save setting if the value is changing from the current setting.   
    if (currentSetting == 'false' && clarificationOn)
    {
        Corbis.Web.UI.Search.SearchScriptService.displayOptionsClarifications(true, clarificationsSuccess, null);
    }
    if ((currentSetting == 'true' || currentSetting == '') && !clarificationOn)
    {
        Corbis.Web.UI.Search.SearchScriptService.displayOptionsClarifications(false, clarificationsSuccess, null);
    }
}

function disableClarification()
{ 
    // Toggling On/Off buttons
    var divWarningContainer = $('warningContainer');

    if (window.location.href.toLowerCase().contains('searchresults.aspx') || window.location.href.toLowerCase().contains('imagegroups.aspx')) {          
        divWarningContainer.getElement('.offWarning').setStyle('background-position', '0 -15px');
        divWarningContainer.getElement('a.warningOff').setStyle('color', '#FFFFFF');
        divWarningContainer.getElement('.onWarning').setStyle('background-position', '0 0');
        divWarningContainer.getElement('a.warningOn').setStyle('color', '#000000');

        if(divWarningContainer.getElement('a.warningOn').hasClass('disabled')) {
            divWarningContainer.getElement('a.warningOn').removeClass('disabled');
            divWarningContainer.getElement('a.warningOff').addClass('disabled');        
        }

        // update setting if needed (cookie must be set by server because of hash)
        setClarificationCookie(false);
        hideDisplayOptionsWindow();
     }
}
     
function enableClarification()
{
    // Toggling On/Off buttons
    var divWarningContainer = $('warningContainer'); 

    if (window.location.href.toLowerCase().contains('searchresults.aspx') || window.location.href.toLowerCase().contains('imagegroups.aspx')) {          
        divWarningContainer.getElement('.onWarning').setStyle('background-position', '0 -15px');
        divWarningContainer.getElement('a.warningOn').setStyle('color', '#FFFFFF');        
        divWarningContainer.getElement('.offWarning').setStyle('background-position', '0 0');
        divWarningContainer.getElement('a.warningOff').setStyle('color', '#000000');

        if(divWarningContainer.getElement('a.warningOff').hasClass('disabled')) {
            divWarningContainer.getElement('a.warningOff').removeClass('disabled');                
            divWarningContainer.getElement('a.warningOn').addClass('disabled');
        } 

        // update setting if needed (cookie must be set by server because of hash)
        setClarificationCookie(true);
        hideDisplayOptionsWindow();
    } 
}
    
function clarificationsSuccess(results, context, methodName)
{
    $(showTermsClarification).value = results;
}

function termsClarificationOffModal()
{
    // Setting the cookie by calling web method
    //Corbis.Web.UI.Search.SearchScriptService.displayOptionsClarifications(false, clarificationsSuccess, null); 
    MochaUI.CloseModal('ambiguousModal');
    // Setting or enabling Off button (hightlighted to red)
    disableClarification();
    return false;
}

function setPreviewPref(previewValue)
{   
    var previewOn = $('ctl00_mainContent_searchResultHeader_previewList_previewOn');
    var previewOff = $('ctl00_mainContent_searchResultHeader_previewList_previewOff');
    
    if (previewValue == '0')
    {
        previewOn.addClass('previewOnSelected');
        previewOn.removeClass('previewOn');
        previewOn.set('disabled', true);
        previewOff.addClass('previewOff');
        previewOff.removeClass('previewOffSelected');
        previewOff.set('disabled', false);
    }
    else
    {
        previewOn.addClass('previewOn');
        previewOn.removeClass('previewOnSelected');
        previewOn.set('disabled', false);
        previewOff.addClass('previewOffSelected');
        previewOff.removeClass('previewOff');
        previewOff.set('disabled', true);
    }
    
    Corbis.Web.UI.<%= Page is Corbis.Web.UI.Lightboxes.MyLightboxes? "Lightboxes.LightboxScriptService": "Search.SearchScriptService" %>.SavePreviewPreference(previewValue, null, null);
    
    return false;
}

window.addEvent('domready', function() {
    if ($('header1')) {
        try {
            $('header2').setStyles({    
                borderBottom: '1px solid #333333',
                float: 'left',
                height: 26,
                lineHeight: 26,
                marginTop: 3,
                paddingBottom: 5,
                verticalAlign: 'middle',
                width: '100%'
            });
        } catch(e) { }
    }
    if ($('ctl00_mainContent_HeaderPanel')) {
        try{
            $('header2').setStyles({    
                borderBottom: '1px solid #333333',
                float: 'left',
                height: 26,
                lineHeight: 26,
                marginTop: 3,
                paddingBottom: 5,
                verticalAlign: 'middle',
                width: '100%'
            });
        } catch(e) { }
    }
});
    

</script>
