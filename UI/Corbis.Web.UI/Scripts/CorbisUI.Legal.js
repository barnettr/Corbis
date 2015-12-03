/****************************************************
    Corbis UI Legal
***************************************************/

// TODO: the name 'legal' could be seen as a misnomer because I am using the class to affect the modal in "Request Image Research".
CorbisUI.Legal = {
    vars: {
        SAM: null,
        policy: null,
        base: null,
        printBase: null,
        printContent: null,
        cleanPrintContent: null,
        WinPrint: null,
        g_firefox: null,
        imprint: null,
        allLinks: null
    },

    initialize: function() {
        this.vars.SAM = '/Legal/SiteUsageAgreement.aspx';
        this.vars.policy = '/Legal/PrivacyPolicy.aspx';
        this.vars.RIR = '/CustomerService/RequestImageResearch.aspx';
        this.vars.base = $('getAcrobat');
        this.vars.printBase = $('aspnetForm');
        this.vars.printContent = this.vars.printBase.getElement('.contentWrapper');
        this.vars.g_firefox = document.getElementById && !document.all;
        this.vars.imprint = '/Legal/Imprint.aspx';
        //this.vars.allLinks = this.vars.printContent.innerHTML.replace(/[^<]*(<a href="([^"]+)">([^<]+)<\/a\>)/g;
    },

    OpenSAMIModal: function() {
        this.initialize();
        OpenNewIModal(this.vars.SAM, 600, 600, 'SiteUsageAgreementModal');
    },

    OpenPolicyIModal: function() {
        this.initialize();
        OpenNewIModal(this.vars.policy, 600, 600, 'PrivacyPolicyModal');
    },

    OpenImprintIModal: function() {
        this.initialize();
        OpenNewIModal(this.vars.imprint, 600, 600, 'ImprintModal');
    },

    OpenRIRModal: function() {
        this.initialize();
        var actionArg = "OpenNewIModal('" + this.vars.RIR + "', 500, 450, 'RequestImageResearchModal')";
        CorbisUI.Auth.Check(1, CorbisUI.Auth.ActionTypes.Execute, actionArg);
    },

    HideSiteUsageAgreementModal: function() {
        parent.MochaUI.CloseModal('SiteUsageAgreementModal');
    },

    HidePrivacyPolicyModal: function() {
        parent.MochaUI.CloseModal('PrivacyPolicyModal');
    },

    HideImprintModal: function() {
        parent.MochaUI.CloseModal('ImprintModal');
    },

    CloseAgreementOpenPolicy: function() {
        this.HideSiteUsageAgreementModal();
        this.OpenPolicyIModal();
    },
    HideRequestImageResearchModal: function() {
        parent.MochaUI.CloseModal('RequestImageResearchModal');
    },

    OpenAcrobatReaderPopup: function(link) {
        new CorbisUI.Popup('getAcrobat', {
            showModalBackground: false,
            centerOverElement: link,
            closeOnLoseFocus: true,
            positionVert: 'top',
            positionHoriz: 'right'
        });
        this.initialize();
        this.vars.base.getElement('.FormButtons').setStyle('margin-top', 5);
    },

    CallPrint: function() {
        this.initialize();

        this.vars.WinPrint = window.open('', '', 'top=0, left=0, height=700, width=800, status=no, menubar=no, resizable=yes, scrollbars=yes, toolbar=no, location=no, directories=no');
        this.vars.WinPrint.document.open();
        this.vars.WinPrint.document.write('<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"><html><head><title>');
        this.vars.WinPrint.document.write(CorbisUI.GlobalVars.SiteUsageAgreement.text.windowTitleMsg);
        this.vars.WinPrint.document.write('</title><link href="/Stylesheets/MasterBase.css" rel="stylesheet" type="text/css" media="all" /></head>');
        this.vars.WinPrint.document.write('<body id="siteUsageAgreement" class="printVersion">');
        this.vars.WinPrint.document.write('<img id="Logo" src="../Images/corbis-logo_sm.gif" alt="');
        this.vars.WinPrint.document.write(CorbisUI.GlobalVars.SiteUsageAgreement.text.altTextMsg);
        this.vars.WinPrint.document.write('" />');
        //Bug fix 17900 begins
        //this.vars.WinPrint.document.write('<h1>Site Usage Agreement</h1>');
        this.vars.WinPrint.document.write('<h1>');
        this.vars.WinPrint.document.write(CorbisUI.GlobalVars.SiteUsageAgreement.text.siteUsageAgreementHeadingText);
        this.vars.WinPrint.document.write('</h1>');
        //Bug fix 17900 ends
        
        this.vars.WinPrint.document.write('<div id="clearMe"></div>');
        if (this.vars.g_firefox) {
            this.vars.WinPrint.document.write(this.vars.printContent.innerHTML
            .replace('<a href="mailto:service@corbis.com">', "")
            .replace(/<a [^>]*>([^<]*)<\/a>/gi, "<u>$1</u>"));
        }
        else {
            this.vars.WinPrint.document.write(this.vars.printContent.innerHTML
            .replace('<A href="mailto:service@corbis.com">service@corbis.com</A>', "service@corbis.com")
            .replace(/<a [^>]*>([^<]*)<\/a>/gi, "<u>&dollar;1</u>"));
        }

        this.vars.WinPrint.document.write('<p id="copyright">');
        this.vars.WinPrint.document.write(CorbisUI.GlobalVars.SiteUsageAgreement.text.copyrightMsg);
        this.vars.WinPrint.document.write('</p></body></html>');

        this.vars.WinPrint.document.close();
        this.vars.WinPrint.focus();
        this.vars.WinPrint.print();
        //this.vars.WinPrint.close();
    },

    //TODO: This is written following the other examples of Printer Friendly Version for Privacy Policy and Site Usage Agreement
    //but all the print versions for Privacy Policy,Site Usage Agreement and Imprint needs to be rewritten.
    CallImprint: function() {
        this.initialize();

        this.vars.WinPrint = window.open('', '', 'top=0, left=0, height=700, width=800, status=no, menubar=no, resizable=yes, scrollbars=yes, toolbar=no, location=no, directories=no');
        this.vars.WinPrint.document.open();
        this.vars.WinPrint.document.write('<html><head><title>');
        this.vars.WinPrint.document.write(CorbisUI.GlobalVars.Imprint.text.WindowTitleText);
        this.vars.WinPrint.document.write('</title><style type="text/css" media="all">'
        + 'body{margin:10px 10px 10px 10px;height:100%;color:#000000;background-color:#FFFFFF;font:normal 11px "Tahoma", Verdana, Tahoma, Arial, sans-serif; line-height:16px;}'
        + 'p{margin:0px 0px 1.2em;}'
        + 'a{color:#4385C0;text-decoration:none;}'
        + 'a:visited{color:#4385C0;text-decoration:none;}'
        + 'a:hover, a.context_link:hover, a.link_light:hover{color:#4385C0;text-decoration:underline;}'
        + '</style></head>'
        );
        this.vars.WinPrint.document.write('<body>');
        this.vars.WinPrint.document.write('<img id="Logo" src="../Images/corbis-logo_sm.gif" align="absmiddle" alt="');
        this.vars.WinPrint.document.write(CorbisUI.GlobalVars.Imprint.text.WindowTitleText);
        this.vars.WinPrint.document.write('" />');
        this.vars.WinPrint.document.write('<div style="font-size:18px;font-family:Trebuchet MS;color:#666666;margin-left:20px;margin-top:25px;display:inline-block;">');
        this.vars.WinPrint.document.write(CorbisUI.GlobalVars.Imprint.text.WindowTitleText);
        this.vars.WinPrint.document.write('</div>');
        this.vars.WinPrint.document.write('<div style="clear:both;margin-top:10px;" />');
        this.vars.WinPrint.document.write('<br />');
        if (this.vars.g_firefox) {
            this.vars.WinPrint.document.write(this.vars.printContent.innerHTML
            .replace('<img src="../Images/en-US/clickseal.gif" style="border-width: 0px;">', "")
            .replace(/<a [^>]*>([^<]*)<\/a>/gi, "<u>$1</u>"));
        }
        else {
            this.vars.WinPrint.document.write(this.vars.printContent.innerHTML
            .replace('<IMG style="BORDER-TOP-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-RIGHT-WIDTH: 0px" src="../Images/en-US/clickseal.gif">', "")
            .replace(/<a [^>]*>([^<]*)<\/a>/gi, "<u>$1</u>"));
        }
        this.vars.WinPrint.document.write(CorbisUI.GlobalVars.Imprint.text.copyrightMsg);
        this.vars.WinPrint.document.write('</body></html>');
        this.vars.WinPrint.document.close();
        this.vars.WinPrint.focus();
        this.vars.WinPrint.print();
        //this.vars.WinPrint.close();
    },

    CallPrintPolicy: function() {
        this.initialize();

        this.vars.WinPrint = window.open('', '', 'top=0, left=0, height=700, width=800, status=no, menubar=no, resizable=yes, scrollbars=yes, toolbar=no, location=no, directories=no');
        this.vars.WinPrint.document.open();
        this.vars.WinPrint.document.write('<html><head><title>');
        this.vars.WinPrint.document.write(CorbisUI.GlobalVars.PrivacyPolicy.text.privacyTitleMsg);
        this.vars.WinPrint.document.write('</title><style type="text/css" media="all">'
        + 'body{margin:10px 10px 10px 10px;height:100%;color:#000000;background-color:#FFFFFF;font:normal 11px "Tahoma", Verdana, Tahoma, Arial, sans-serif; line-height:16px;}'
        + 'p{margin:0px 0px 1.2em;}'
        + '.trustE {display:none;} '
        + 'a{color:#4385C0;text-decoration:none;}'
        + 'a:visited{color:#4385C0;text-decoration:none;}'
        + 'a:hover, a.context_link:hover, a.link_light:hover{color:#4385C0;text-decoration:underline;}'
        + '</style></head>'
        );
        this.vars.WinPrint.document.write('<body>');
        this.vars.WinPrint.document.write('<img id="Logo" src="../Images/corbis-logo_sm.gif" align="absmiddle" style="float:left;background-repeat:no-repeat;height:28px; padding-top:13px; width:100px;" alt="');
        this.vars.WinPrint.document.write(CorbisUI.GlobalVars.PrivacyPolicy.text.altTextMsg);
        this.vars.WinPrint.document.write('" />');
        this.vars.WinPrint.document.write('<div style="font-size:18px;font-family:Trebuchet MS;color:#666666;margin-left:20px;margin-top:25px;display:inline-block;">');
        this.vars.WinPrint.document.write(CorbisUI.GlobalVars.PrivacyPolicy.text.privacrPolicyTitle);
        this.vars.WinPrint.document.write('</div>');
        this.vars.WinPrint.document.write('<div style="clear:both;margin-top:10px;" />');
        this.vars.WinPrint.document.write('<br />');
        if (this.vars.g_firefox) {
            this.vars.WinPrint.document.write(this.vars.printContent.innerHTML
            .replace('<img src="../Images/en-US/clickseal.gif" style="border-width: 0px;">', "")
            .replace(/<a [^>]*>([^<]*)<\/a>/gi, "<u>$1</u>"));
        }
        else {
            this.vars.WinPrint.document.write(this.vars.printContent.innerHTML

            .replace('<IMG style="BORDER-TOP-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-RIGHT-WIDTH: 0px" src="../Images/en-US/clickseal.gif">', "")
            .replace(/<a [^>]*>([^<]*)<\/a>/gi, "<u>$1</u>"));
        }
        this.vars.WinPrint.document.write('<p style="font-size:10px; font-weight:bold;">');
        this.vars.WinPrint.document.write(CorbisUI.GlobalVars.PrivacyPolicy.text.copyrightMsg);
        this.vars.WinPrint.document.write('</p></body></html>');
        this.vars.WinPrint.document.close();
        this.vars.WinPrint.focus();
        this.vars.WinPrint.print();
        //this.vars.WinPrint.close();
    }
}