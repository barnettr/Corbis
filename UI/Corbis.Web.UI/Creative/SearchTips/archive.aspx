<%@ Page language="c#" Inherits="Corbis.CorbisOne.UI.Template.StandardTemplate"  %>
<%@ Register TagPrefix="CorbisControls" Namespace="Corbis.CorbisOne.UI.Controls" Assembly="Corbis.CorbisOne.UI.ExternalUI" %>

<script runat="server" language="c#">
	private void Page_Load(object sender, System.EventArgs e)
	{
		this.Channel = Corbis.CorbisOne.UI.OmniConstants.Channels.MARKETING;
		this.PageName = "SearchTips";
		
		string lcd = this.StateManager.LanguageCode.ToLower();
		if(lcd == "en-us"){ 
                Server.Transfer("default.aspx");
        }
	}
</script>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 

<html>
  <head id="Head1" runat="server">
	<title>Search Tips</title>
	<script language="javascript" src="/jslibrary/yui/yahoo-min.js"></script>
	<script language="javascript" src="/jslibrary/yui/dom-min.js"></script>
	<script language="javascript" src="/jslibrary/yui/event-min.js"></script>
	<script language="javascript" src="/jslibrary/yui/animation-min.js"></script>
	<script language="javascript" src="/jslibrary/SideScroller.js"></script>
  </head>
  <body onload="initCurrObj()">

<style>
		BODY
		{
			background-color: #262626;
		}
		#ctl05_lblAllRightsReserved { color: #BEBEBE; }
		#ctl05_lblChooseLanguage { color: #BEBEBE; }
		#TemplateBodyHolder { margin-top: -7px; }
		.searchTipsTab {
		height:30px;
		float: left; 
		text-align:center;
		}
		#DivTab1, #DivTab2, #DivTab3, #DivTab4{ cursor:pointer; }
		.extraLink{ cursor:pointer; }
		.extraLink div{ text-decoration:underline; }
		.searchTipsContent {
		width: 617px;
		background-color: #8B8E85;
		color: white;
		min-height: 250px;
		height:250px;
		}
		.latestTipDiv {
		color: #ff0099;
		height: 130px;
		}
		.latestSearchTipDiv
		{
		margin-top:21px; 
		margin-bottom: 21px; 
		background: #ffffff url(images/cloud.gif) no-repeat bottom right; 
		width:617px; 
		}
		.latestSearchTipNav
		{
		color: #999999;
		}
		.latestSearchTipNav:hover
		{
		color: #000000;
		}
		.extraLink 
		{
		height:30px; 
		background-color:#262626;
		margin-bottom:2px;
		text-decoration: none;
		color: #ffffff;
		padding-left: 10px;
		}
		.keywordSearching {
		color: #cccccc;
		}
		.keywordSearchingDiv {
		margin-top: 20px;
		height: 130px;
		color: #ffffff;
		}
		.keywordSearchingDiv a {
		color: white;
		}
		.keywordSearchingNav {
		color: #cccccc;
		}
		.keywordSearchingNav:hover {
		color: #ffffff;
		}
		.creativeKeywordsDiv {
		color: #cccccc;
		}
		.creativeKeywordItem {
			position:relative;
			overflow:hidden;
			float:left; 
			width: 170px;
			margin-right: 10px;
		}
	</style>
	
	    <form id="Form1" method="post" runat="server">
		<script>

		    /* BEGIN remember page state fcns */
		        var creativeKeywordItems = 0;
		        var currTab = 1;
		        var currCat = 1;
		        var currImg = 1;
		        function initCurrObj(){
		            //get existing values, if they exist
		                if( document.cookie.indexOf("searchTips_currTab")!=-1 ) currTab = getCookie("searchTips_currTab");
		                if( document.cookie.indexOf("searchTips_currCat")!=-1 ) currCat = getCookie("searchTips_currCat");
		                if( document.cookie.indexOf("searchTips_currImg")!=-1 ) currImg = getCookie("searchTips_currImg");
		            //set tabs
		                ShowTab(currTab)
		            //set keyword groups
		                if(currCat > 1) document.getElementById("KeywordSearching" + currCat).style.display = "";
		                else document.getElementById("KeywordSearching1").style.display = "";
		            //set image scroller
                        if( currImg > 1 ){
                            for(var i=1; i<currImg; i++){ advanceImg(); }
                            setTimeout('document.getElementById("ImgScrollerTable").style.visibility = "visible"', 750);
                        }
                        else{
                            document.getElementById("ImgScrollerTable").style.visibility = "visible";
                        }
		        }
		        function advanceImg(){
		            ssConfigs['Imagescroll'].isBusy = false;
		            SideScroller.scrollNext(null, 'Imagescroll');
		            ssConfigs['Imagescroll'].isBusy = true;
		        }
		        function setCurrObj( objName, i ){
		            document.cookie = 'searchTips_'+objName+'='+i;
		        }
		        function setCurrImg( dir ){
		            if( dir=='forward' ){
		                currImg++;
		                if( currImg == creativeKeywordItems+1 ) currImg = 1;
		            }
		            else{
		                currImg--;
		                if( currImg <= 0 ) currImg = creativeKeywordItems;
		            }
		            document.cookie = 'searchTips_currImg='+currImg;
		        }
		        function getCookie(c_name){
                    if (document.cookie.length>0){
                        c_start=document.cookie.indexOf(c_name + "=")
                        if (c_start!=-1){ 
                            c_start=c_start + c_name.length+1;
                            c_end=document.cookie.indexOf(";",c_start)
                            if (c_end==-1) c_end=document.cookie.length;
                            return unescape(document.cookie.substring(c_start,c_end));
                        } 
                    }
                    return "";
                } 
		    /* END remember page state fcns */
		    
			function ShowTab(tabNum) {
			    setCurrObj('currTab',tabNum);
			
				// Hide all tabs
				document.getElementById("DivContent1").style.display = "none";
				document.getElementById("DivContent2").style.display = "none";
				document.getElementById("DivContent3").style.display = "none";
				document.getElementById("DivContent4").style.display = "none";
				// Set visibility of selected tab content
				document.getElementById("DivContent" + tabNum).style.display = "";
				
				// Set styles on all tab headers
				document.getElementById("DivTab1").style.height = "30px";
				document.getElementById("DivTab1").style.backgroundColor = "#ffffff";
				document.getElementById("DivTab2").style.height = "30px";
				document.getElementById("DivTab2").style.backgroundColor = "#ffffff";
				document.getElementById("DivTab3").style.height = "30px";
				document.getElementById("DivTab3").style.backgroundColor = "#ffffff";
				document.getElementById("DivTab4").style.height = "30px";
				document.getElementById("DivTab4").style.backgroundColor = "#ffffff";
				
				// Set style of selected tab
				document.getElementById("DivTab" + tabNum).style.height = "32px";
				document.getElementById("DivTab" + tabNum).style.marginBottom = "0px";
				document.getElementById("DivTab" + tabNum).style.backgroundColor = "#8B8E85";
				
			}
			
			var LatestSearchTipNum = 1;
			var LatestSearchTipCount = 0;
			
			function GoLatestSearchTip(tipDir)
			{
				document.getElementById("LatestTip" + LatestSearchTipNum).style.display = "none";
				LatestSearchTipNum = LatestSearchTipNum + tipDir;
				if (document.getElementById("LatestTip" + LatestSearchTipNum) == null) 
				{
					if (LatestSearchTipNum <= 0) { 
						LatestSearchTipNum = LatestSearchTipCount;
					} else {
						LatestSearchTipNum = 1;
					}
				}
				document.getElementById("LatestTip" + LatestSearchTipNum).style.display = "";
			}
			
			function HighlightExtraLink(oDiv, isHighlighted)
			{
				if (isHighlighted) {
					oDiv.style.backgroundColor = "#8b8e85";
				} else {
					oDiv.style.backgroundColor = "#262626";
				}
			}
			
			var KeywordSearchingNum = 1;
			var KeywordSearchingCount = 0;

			function GoKeywordSearching(goDir)
			{
				document.getElementById("KeywordSearching" + KeywordSearchingNum).style.display = "none";
				KeywordSearchingNum = KeywordSearchingNum + goDir;
				if (document.getElementById("KeywordSearching" + KeywordSearchingNum) == null) 
				{
					if (KeywordSearchingNum <= 0) { 
						KeywordSearchingNum = KeywordSearchingCount;
					} else {
						KeywordSearchingNum = 1;
					}
				}
				setCurrObj('currCat',KeywordSearchingNum);
				document.getElementById("KeywordSearching" + currCat).style.display = "none";
				document.getElementById("KeywordSearching" + KeywordSearchingNum).style.display = "";
			}
			
			function ShowDescription(descNum, bShow)
			{
				if (bShow) {
					YAHOO.util.Dom.setStyle("KeywordDescription" + descNum, 'opacity', 0);
					document.getElementById("KeywordDescription" + descNum).style.display = "";
					toOpac = .9;
					easing = YAHOO.util.Easing.easeIn;
				} else {
					toOpac = 0;
					easing = YAHOO.util.Easing.easeOut;
					document.getElementById("KeywordDescription" + descNum).style.display = "none";
				}
				var anim = new YAHOO.util.Anim("KeywordDescription" + descNum, { opacity: { to: toOpac } }, .3, easing);
		        anim.animate();
			}
        </script>
		<CorbisControls:xml id="xmlCorbis" runat="server" contentDirectory="/creative/searchtips/content" DocumentSource="default.xml" TransformSource="archive.xslt" />
    </form>

	<script>
		var imgs = document.body.getElementsByTagName("img");
				
		for (var i = 0; i < imgs.length; i++) {
			if (imgs[i].src == "http://cachens.corbis.com/pro/searchbar_dropshadow.gif") {
				imgs[i].style.display = "none";
			}
		}
		
		ssConfigs['Imagescroll'] = new SideScrollerConfig(1, 170, 10, 500);
		SideScroller.init('Imagescroll');
		
		YAHOO.util.Event.addListener(document.getElementById('forward'), 'mouseover', function(e) { document.getElementById('forward').src = 'images/arrow_right_over.gif';});
		YAHOO.util.Event.addListener(document.getElementById('forward'), 'mouseout', function(e) { document.getElementById('forward').src = 'images/arrow_right.gif';});
		YAHOO.util.Event.addListener(document.getElementById('back'), 'mouseover', function(e) { document.getElementById('back').src = 'images/arrow_left_over.gif';});
		YAHOO.util.Event.addListener(document.getElementById('back'), 'mouseout', function(e) { document.getElementById('back').src = 'images/arrow_left.gif';});
	
	    YAHOO.util.Event.addListener(document.getElementById('forward'), 'click', function(e) { setCurrImg('forward'); });
	    YAHOO.util.Event.addListener(document.getElementById('back'), 'click', function(e) { setCurrImg('back'); });
    </script>
  </body>
</html>