<html>
<head id="Head1" runat="server">
    <title>Corbis Beta Site Bug Report Form</title>
    <link rel="stylesheet" href="/Stylesheets/helpers/Resets.css" type="text/css" />
    <link rel="stylesheet" href="/Stylesheets/MasterBase.css" type="text/css" />
    <link rel="Stylesheet" href="/Stylesheets/controls/GlassButton.css" type="text/css" />
    <link rel="Stylesheet" href="/Stylesheets/Beta.css" type="text/css" />
    <script type="text/javascript" src="/Scripts/mootools-1.2-debug.js"></script>
    <script type="text/javascript" language="javascript" src="/Scripts/rounded.js"></script>
    <script type="text/javascript" src="/Scripts/common.js"></script>
    <script type="text/javascript" src="/Scripts/formcheck.js"></script>
    <script type="text/javascript">  
		
       

        //Set up browser auto-fill 
        var BrowserDetect = {
	    init: function () {
		    this.browser = this.searchString(this.dataBrowser) || "An unknown browser";
		    this.version = this.searchVersion(navigator.userAgent)
			    || this.searchVersion(navigator.appVersion)
			    || "an unknown version";
		    this.OS = this.searchString(this.dataOS) || "an unknown OS";
		    
	    },
	    searchString: function (data) {
	    
		    for (var i=0;i<data.length;i++)	{
			    var dataString = data[i].string;
			    var dataProp = data[i].prop;
			    this.versionSearchString = data[i].versionSearch || data[i].identity;
			    if (dataString) {
				    if (dataString.indexOf(data[i].subString) != -1)

					    return data[i].identity;
			    }
			    else if (dataProp)
				    return data[i].identity;
		    }
	    },
	    searchVersion: function (dataString) {
		    var index = dataString.indexOf(this.versionSearchString);
		    if (index == -1) return;
		    return parseFloat(dataString.substring(index+this.versionSearchString.length+1));
	    },
	    dataBrowser: [
		    {
			    string: navigator.userAgent,
			    subString: "Chrome",
			    identity: "Chrome"
		    },
		    { 	string: navigator.userAgent,
			    subString: "OmniWeb",
			    versionSearch: "OmniWeb/",
			    identity: "OmniWeb"
		    },
		    {
			    string: navigator.vendor,
			    subString: "Apple",
			    identity: "Safari",
			    versionSearch: "Version"
		    },
		    {
			    prop: window.opera,
			    identity: "Opera"
		    },
		    {
			    string: navigator.vendor,
			    subString: "iCab",
			    identity: "iCab"
		    },
		    {
			    string: navigator.vendor,
			    subString: "KDE",
			    identity: "Konqueror"
		    },
		    {
			    string: navigator.userAgent,
			    subString: "Firefox",
			    identity: "Firefox",
			    versionSearch: "Firefox"
		    },
		    {
			    string: navigator.vendor,
			    subString: "Camino",
			    identity: "Camino"
		    },
		    {		// for newer Netscapes (6+)
			    string: navigator.userAgent,
			    subString: "Netscape",
			    identity: "Netscape"
		    },
		    {
			    string: navigator.userAgent,
			    subString: "MSIE",
			    identity: "Explorer",
			    versionSearch: "MSIE"
		    },
		    {
			    string: navigator.userAgent,
			    subString: "Gecko",
			    identity: "Mozilla",
			    versionSearch: "rv"
		    },
		    { 		// for older Netscapes (4-)
			    string: navigator.userAgent,
			    subString: "Mozilla",
			    identity: "Netscape",
			    versionSearch: "Mozilla"
		    }
	    ],
	    dataOS : [
		    
		    {
			    string: navigator.userAgent,
			    subString: "NT 5.1",
			    identity: "WindowsXP",
			    versionSearch: "NT 5.1"
		    },
		    {
			    string: navigator.userAgent,
			    subString: "NT 6.0",
			    identity: "WindowsVista",
			    versionSearch: "NT 6.0"
		    },
		    {
			    string: navigator.platform,
			    subString: "Mac",
			    identity: "Mac"
		    },
		    {
			    string: navigator.platform,
			    subString: "Linux",
			    identity: "Linux"
		    },
		    {
			    string: navigator.platform,
			    subString: "Win",
			    identity: "WindowsXP"
		    }
	    ]

    };
    BrowserDetect.init();




    window.addEvent('domready', function() {
			    if(Browser.Engine.trident){
			        var myCheck = new FormCheck('formular', {
			            display:{
			                indicateErrors: 1,
			                listErrorsAtTop: false,
			                scrollToFirst:false,
				            errorsLocation :1,
			                showErrors: 1,
			                addClassErrorToField: 2,
			                tipsOffsetY: 56,
			                tipsOffsetX: 0,
			                listErrorsAtTop: false,
			                keepFocusOnError: 0,
			                checkValueIfEmpty: 1
                        },
                        tipsClass: 'Error BetaTips',
                        errorClass: 'Error BetaTips',
                        fieldErrorClass: 'Error'
                        
			        });
			    }else{
			        var myCheck = new FormCheck('formular', {
			            display:{
			                indicateErrors: 1,
			                listErrorsAtTop: false,
			                scrollToFirst:false,
				            errorsLocation :1,
			                showErrors: 1,
			                addClassErrorToField: 2,
			                tipsOffsetY: 36,
			                tipsOffsetX: 0,
			                listErrorsAtTop: false,
			                keepFocusOnError: 0,
			                checkValueIfEmpty: 1
                        },
                        tipsClass: 'Error BetaTips',
                        errorClass: 'Error BetaTips',
                        fieldErrorClass: 'Error'
                        
			        });
			    }
    			
                var screenRes = screen.width+'x'+screen.height;
                $('screen_resolution').value = screenRes;
                
                usrOS = BrowserDetect.OS;
                var myOSEle = $('cf1'); //Get OS select box
               
                usrBrowser = BrowserDetect.browser;
                if(usrBrowser == "Explorer" || usrBrowser == "Firefox" || usrBrowser == "Safari") usrBrowser = usrBrowser + BrowserDetect.version;
                var myBrowserEle = $('cf0'); //Get Browser select box
                
                try {
                    //loop through OS and see if it matches BrowserDetect
                    for(i=0;i<myOSEle.length;i++){
                        var tmpValue = myOSEle[i].value;
                        
                        if(tmpValue == usrOS){
                        
                                //MSIE Select implementation is diff than others
                                if(Browser.Engine.trident){
                                    myOSEle[i].selectedIndex = i;
                                    myOSEle[i].selected = true;
                                }else{
                                    myOSEle[i].selected = true;
                                }
                        }
                        
                    }
                    //loop through Browsers and see if it matches BrowserDetect
                    for(i=0;i<myBrowserEle.length;i++){
                        var tmpValue = myBrowserEle[i].value;
                        if(tmpValue == usrBrowser){
                                //MSIE Select implementation is diff than others
                                if(Browser.Engine.trident){
                                    myBrowserEle.selectedIndex = i;
                                    myBrowserEle[i].selected = true;
                                }else{
                                    myBrowserEle[i].selected = true;
                                }
                                
                        }
                        
                    }
                   
                }
                catch(ex) {
                    
                    
                }
                                
               
            });
		    
		   
		    function trimEmail(stringToTrim) {
	            $('cf2').value = $('cf2').value.replace(/^\s+|\s+$/g,"");
            }


	</script>
	
</head>
<body style="background-color: #f8f8f8 !important; margin: 12px 12px -12px 12px !important;">
    <div class="betaOuterWrap" style="width:500px;">
    
        <form  id="formular" class="formular" target="_top" method="post" enctype="multipart/form-data" 
          action="<%= ConfigurationManager.AppSettings("BetaFeedbackActionAddress") %>/post.aspx?action=api-openbug&license=NkJGNEIzRDQ5QjIxNDAwNEExMEQwRTFDQkI0QzU1NDc=&project=15426&area=68880&openby=23494&assignto=23494&priority=19407&version=13866&type=18362&category=10751&phase=10075">
            <input name="success_url" type="hidden" value="http://<%= Request.ServerVariables("HTTP_HOST") %>/CustomerService/BetaThankYou.aspx" />
            <input name="fail_url" type="hidden" value="" />
            <input name="cf7" id="screen_resolution" type="hidden" value="" />
            <input name="cf8" id="user_name" type="hidden" value="corbisUser:<%=Request.QueryString("usr")%> betaUser:<%=Request.QueryString("busr")%>" />
            <input name="cf9" id="referring_page" type="hidden" value="<%=Request.QueryString("ref")%>" />
            <div class="bff">
            
            <label>Email:</label>
            <input type="text" maxlength="128" tabindex="1" id="cf2" name="cf2" onchange="trimEmail(); return false;" class="validate['required','email'] text-input" value="<%=Request.QueryString("em")%>" />
            
            <label>Your full name:</label>
            <input type="text" maxlength="128" tabindex="2" id="cf3" name="cf3" class="validate['required'] text-input" />
            
            <label>Issue:</label>
            <select name="title" class="validate['required'] select"  tabindex="3">
                <option>Choose one</option>
                <option value="search">Search</option>
                <option value="searchresults">Search results</option>
                <option value="pricing">Pricing</option>
                <option value="cart">Cart</option>
                <option value="checkout">Checkout</option>
                <option value="lightbox">Lightbox</option>
                <option value="myaccount">My account</option>
                <option value="quickpic">Quickpic</option>
                <option value="other">Other</option>
                <option value="notsure">Not sure</option>
            </select>
            
            
            
            
            
            <label>Description:<em>Please be as specific as possible so we may address the problem.</em></label>

            <textarea rows="10" cols="50" id="body" name="body" class="validate['wordcount[500]']" tabindex="4"></textarea>

            <label>Type of comment:</label>
            <select name="cf4" class="validate['required'] select" tabindex="5">
                <option>Choose one</option>
                <option value="Suggestion">Suggestion</option>
                <option value="Appears broken">Appears broken</option>
                <option value="Works well">Works well</option>
                <option value="Other">Other</option>
            </select>
            
            <label>Operating System:</label>
            <select name="cf1" id="cf1" class="validate['required'] select" tabindex="6">
                <option>Choose one</option>
                <option value="Mac">Mac OSX</option>
                <option value="WindowsXP">Windows XP</option>
                <option value="WindowsServer2003">Windows Server 2003</option>
                <option value="WindowsVista">Windows Vista</option>
                <option value="Other">Other</option>
                
            </select>
            
            <label>Browser:</label>
            <select name="cf0" id="cf0" class="validate['required'] select" tabindex="7">
                <option>Choose one</option>
                <option value="Safari1">Safari 1</option>
                <option value="Safari2">Safari 2</option>
                <option value="Safari3">Safari 3</option>
                <option value="Safari4">Safari 4</option>
                <option value="Firefox1">Firefox 1</option>
                <option value="Firefox2">Firefox 2</option>
                <option value="Firefox3">Firefox 3</option>
                <option value="Explorer8">Internet Explorer 8</option>
                <option value="Explorer7">Internet Explorer 7</option>
                <option value="Explorer6">Internet Explorer 6</option>
                <option value="Chrome">Google Chrome</option>
                <option value="Opera">Opera</option>
                <option value="Other">Other</option>
            </select>
            
            <label>Screenshot or supporting document upload:</label>
            <input name="fileupload" type="file" tabindex="8" />
            <br clear="all" />
            
            <div class="clr">&nbsp;</div>
            
            <div class="betaSubmitWrap GlassButton btnOrangedbdbdb validate['submit']">
                <span class="Right"><input type="submit" tabindex="11" name="submit_" value="Submit" class="betaSubmit Center" /></span>
            </div>

          </div>
        </form>
    </div>
</body>
</html>
