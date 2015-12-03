using System;
using System.Collections.Generic;
using Corbis.Framework.Logging;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.UI.Navigation;
using Corbis.Web.Utilities;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;

namespace Corbis.Web.UI.MasterPages
{
    
    public partial class NoSearchBar : CorbisMasterBase
    {
        public GlobalNav GlobalNav
        {
            get { return globalNav; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Request.Browser.Browser == "IE" && Request.Browser.MajorVersion < 7)
            {   // BUGBUG: this is IE 6 and below - why is it named IE7? (ron)
                this.AddScriptToPage(SiteUrls.IE7Script, "IE7JS");
            }

            this.AddScriptToPage(SiteUrls.MasterBaseJavascript, "MBScript");

            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.NoSearchStyles, "NoSearchStyles");

            if (Request.Browser.Browser == "IE" && Request.Browser.MajorVersion < 7)
            {
                HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Ie6Styles, "IeStylesCSS");
            }
            else if (Request.Browser.Browser == "IE" && Request.Browser.MajorVersion == 7)
            {
                HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Ie7Styles, "Ie7StylesCSS");
            }

            Response.CacheControl = "no-cache";
            Response.AddHeader("Pragma", "no-cache");
            Response.Expires = -1;
			Response.Cache.SetNoStore();
        }
    }
}
