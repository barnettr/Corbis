using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Corbis.Web.UI;
using Corbis.Web.UI.Navigation;
//using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Utilities;
using Corbis.Web.Entities;
using Corbis.Web.Utilities.StateManagement;

using System.Configuration;


namespace Corbis.Web.UI
{
    public partial class unsupportedBrowser : CorbisBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            Response.CacheControl = "no-cache";
            Response.AddHeader("Pragma", "no-cache");
            Response.Expires = -1;
			Response.Cache.SetNoStore();

            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Beta, "Beta");

            //home.NavigateUrl = SiteUrls.Home;

            browserName.InnerText = Request.Browser.Browser.ToString();
            browserVersion.InnerText = Request.Browser.MajorVersion.ToString();

            System.Web.HttpBrowserCapabilities browser = Request.Browser;
            string s = "Browser Capabilities\n"
                + "Type = " + browser.Type + "\n"
                + "Name = " + browser.Browser + "\n"
                + "Version = " + browser.Version + "\n"
                + "Major Version = " + browser.MajorVersion + "\n"
                + "Minor Version = " + browser.MinorVersion + "\n"
                + "Platform = " + browser.Platform + "\n"
                + "Is Beta = " + browser.Beta + "\n"
                + "Is Crawler = " + browser.Crawler + "\n"
                + "Is AOL = " + browser.AOL + "\n"
                + "Is Win16 = " + browser.Win16 + "\n"
                + "Is Win32 = " + browser.Win32 + "\n"
                + "Supports Frames = " + browser.Frames + "\n"
                + "Supports Tables = " + browser.Tables + "\n"
                + "Supports Cookies = " + browser.Cookies + "\n"
                + "Supports VBScript = " + browser.VBScript + "\n"
                + "Supports JavaScript = " +
                    browser.EcmaScriptVersion.ToString() + "\n"
                + "Supports Java Applets = " + browser.JavaApplets + "\n"
                + "Supports ActiveX Controls = " + browser.ActiveXControls
                      + "\n"
                + "AGENT = " + Request.UserAgent.ToString()
                      + "\n";
            

            String UAtest = Request.UserAgent.ToLower();
            if(UAtest.Contains("safari")){
                int sfStart = UAtest.LastIndexOf("safari/") + 7;
                string sfVersion = UAtest.Substring(sfStart).Trim().Substring(0, 1);
                s += "SAFARI AGENT = " + UAtest + "\n"
                    + "SAFARI VERSION = " + sfVersion;
            }

            browserData.InnerText = s;
            //Request.UserAgent.

            base.OnInit(e);
            
            
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            // If we are in Beta mode on any other server than the Dev server,
            // hide the real content and show the Beta content.
            string betaMode = ConfigurationManager.AppSettings["InBetaMode"];
            string environment = ConfigurationManager.AppSettings["Environment"];
            if (!string.IsNullOrEmpty(betaMode) && betaMode.ToLower() == "true"
                && (string.IsNullOrEmpty(environment) || environment.ToLower() != "dev"))
            {
                BetaFlag.Visible = true;
            }

            String redirectUrl = ConfigurationManager.AppSettings["UnsupportedBrowserRedirect"];
            upgradeLaterNotice.Text = String.Format((string)GetLocalResourceObject("upgradeLaterParagraph.Text"), redirectUrl);

            //String redirectUrl = ConfigurationManager.AppSettings["UnsupportedBrowserRedirect"];
            //String redirectScript = "setTimeout(\"window.location = '"+redirectUrl+"'\",10000)";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "SetPageFocus", redirectScript, true);



        }

    }
}
