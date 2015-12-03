<%@ Page language="c#" Inherits="Corbis.CorbisOne.UI.Template.StandardTemplate"  %>
<%@ Register TagPrefix="CorbisControls" Namespace="Corbis.CorbisOne.UI.Controls" Assembly="Corbis.CorbisOne.UI.ExternalUI" %>
<%@ Register TagPrefix="CorbisControls" TagName="footer" Src="/UserControls/Template/FooterControl.ascx" %>
<script runat="server" language="c#">
	string lcd;
	private void Page_Load(object sender, System.EventArgs e)
	{
	
	    this.Channel = Corbis.CorbisOne.UI.OmniConstants.Channels.MARKETING;
		this.PageName = "SearchTips";
		
		this.Title = this.GetLocalizedValue("/creative/searchtips/content", "default.xml", "PageTitle");
		
        HtmlMeta hm = new HtmlMeta();
		hm.Name = "TITLE";
        hm.Content = this.GetLocalizedValue("/creative/searchtips/content", "default.xml", "PageTitle");
        this.Header.Controls.AddAt(0, hm);

        hm = new HtmlMeta();
        hm.Name = "KEYWORDS";
        hm.Content = this.GetLocalizedValue("/creative/searchtips/content", "default.xml", "MetaKeywords");
        this.Header.Controls.AddAt(0, hm);

        hm = new HtmlMeta();
        hm.Name = "DESCRIPTION";
        hm.Content = this.GetLocalizedValue("/creative/searchtips/content", "default.xml", "MetaDescription");
        this.Header.Controls.AddAt(0, hm);
        
        
		
 
        lcd = this.StateManager.LanguageCode.ToLower();
             
        
        
        System.Xml.Xsl.XsltArgumentList t = new System.Xml.Xsl.XsltArgumentList();
            string cssFile;
            string scriptFile;
            
            string userAgent = Request.UserAgent;
            
            if(this.IsIE() && userAgent.IndexOf("MSIE 7.0") == -1){
                cssFile = "<link rel=\"stylesheet\" type=\"text/css\" href=\"/creative/searchtips/css/searchtips_ie.css\" />";
                cssFile = cssFile + "<link rel=\"stylesheet\" type=\"text/css\" href=\"/creative/searchtips/css/accordion-menu.css\" />";
            }else{
                cssFile = "<link rel=\"stylesheet\" type=\"text/css\" href=\"/creative/searchtips/css/searchtips_ns.css\" />";
                cssFile = cssFile + "<link rel=\"stylesheet\" type=\"text/css\" href=\"/creative/searchtips/css/accordion-menu.css\" />";
            }
            
                       
            t.AddParam("css", "", cssFile);
            t.AddParam("lcd", "", lcd);

        this.xmlCorbis.TransformArgumentList = t;
	     }       
 
	
</script>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 


<html>
<head id="Head1" runat="server">
<title>Search Tips</title>
</head>
<script type="text/javascript">
<!--
//This code prevents that horrible 'flashiing' that IE6 does to its background images
try {
  document.execCommand("BackgroundImageCache", false, true);
} catch(err) {}
// -->
</script>
<body>
<form id="Form1" method="post" runat="server">

  <CorbisControls:xml id="xmlCorbis" runat="server" contentDirectory="/creative/searchtips/content" DocumentSource="default.xml" TransformSource="default.xslt" />

</form>
<% if(Request.UserAgent.IndexOf("MSIE 7.0") != -1){ %>
<style type="text/css">
		div#layout_cleft_col{
			width: 604px;
		}
		div#layout_right_col{
			margin-left:1px;
			width:271px;
			border:1px solid #656565;
		}
		div.extraLinksDiv{
			margin:0px 0 0 96px;
			padding:0;
			overflow:visible;
			height:32px;
			width:604px;			
		}
		
		a.extraLink{
		    cursor:pointer;
		    color: #656565;
		    float:left;
		    font-size:11px;
		    font-weight:bold; 
		    display:block;
		    background-color:#bada55;
		    height:11px;
		    margin:0;
		    padding: 8px;
		    text-decoration: underline;
		    width:299px;
		    text-align:left;
		}
		
		div.boutiqueLink{
		    float:right;
		    margin:-32px 10px 0 0;
		    width:283px;
		    display:inline;
		    background-color:#bada55;
		    color:#656565;
		    padding: 6px;
		    font-size:12px;
		    text-align:left;
		}
		div.boutiqueLink a.extraLink{
		    float:right;
		    margin:0px;
		    padding:6px 0 6px 0;
		    width:98%;
		    text-align:right;
		}
		div.whiteCenterBox{
			width:590px;
		}
		p.tabDesc{
		    display:block;
		    height:65px;
		    width:95%;
		    overflow:visible;
		    padding:5px;
		    margin: 20px 0px 0px 26px;
		    font-size:12px;
		}
</style>
<% } 
if(lcd == "fr-fr" || lcd == "es-es" || lcd == "it-it" || lcd == "nl-nl" || lcd == "de-de"){ %>
<style type="text/css">
	a.extraLink{
		font-size:9px;
	}
</style>
<%
}
if(lcd == "it-it"){ %>
	<style type="text/css">
		div.boutiqueLink{
			height:59px;
		}
	</style>
     
<%
}
if(lcd == "pl-pl" || lcd == "pt-br"){ %>
	<style type="text/css">
		a.extraLink{
			height:39px;
			font-size:10px;
		}
	</style>
<% }
if((lcd == "pl-pl" || lcd == "pt-br") && Request.UserAgent.IndexOf("MSIE 7.0") != -1){ %>
	<style type="text/css">
		div.boutiqueLink{
			margin:-40px 10px 0 0;
		}
	</style>     
<%
}
if(lcd == "ja-jp"){ %>
	<style type="text/css">
		#TemplateBodyHolder *{
			font-family: "MS Gothic", "MS P Gothic", sans-serif;
		}
	</style>
<% } 
if(lcd == "zh-chs" || lcd == "ja-jp"){ %>
	<style type="text/css">

		a.extraLink{
			font-size:12px;

		}
		dt.a-m-t{
			height:13px;
			font-size:12px;
		}
		dt.a-m-t b{
			font-weight:normal;
			font-size:14px;
		}
		p.tabDesc{
			line-height:17px;
			margin-top:3px;
			margin-bottom: 3px;
			height:78px;
		}
	</style>
     
<%
}

%>
<script>
        <% if(lcd != "en-us" && lcd != "en-gb"){ %>
            var tabCount = 4;
        <% }else{ %>
            var tabCount = 6;
        <% } %>
		var imgs = document.body.getElementsByTagName("img");
				
		for (var i = 0; i < imgs.length; i++) {
			if (imgs[i].src == "http://cachens.corbis.com/pro/searchbar_dropshadow.gif") {
				imgs[i].style.display = "none";
			}
		}
	    
	    
	    //Set up options for expanding keyword menu
        var oOptions=
            {	dependent:true,
                seconds:0.6,
	            easeOut:true,
			  easeIn:true,
	            openedIds:[]
            }
            var setting = new AccordionMenu.setting('accordion-menu',oOptions);
		

		
		
		YAHOO.util.Event.addListener($('forward'), 'mouseover', function(e) { $('forward').src = 'images/arrow_right_over.gif';});
		YAHOO.util.Event.addListener($('forward'), 'mouseout', function(e) { $('forward').src = 'images/arrow_right.gif';});
		YAHOO.util.Event.addListener($('back'), 'mouseover', function(e) { $('back').src = 'images/arrow_left_over.gif';});
		YAHOO.util.Event.addListener($('back'), 'mouseout', function(e) { $('back').src = 'images/arrow_left.gif';});
	
	    YAHOO.util.Event.addListener($('forward'), 'click', function(e) { setCurrImg('forward'); });
	    YAHOO.util.Event.addListener($('back'), 'click', function(e) { setCurrImg('back'); });
    </script>
</body>
</html>
