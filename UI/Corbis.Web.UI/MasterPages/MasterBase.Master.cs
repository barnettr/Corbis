using System;
using System.Collections.Generic;
using System.Configuration;
using Corbis.Framework.Logging;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.UI.Navigation;
using Corbis.Web.Utilities;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Corbis.Web.Utilities.StateManagement;

namespace Corbis.Web.UI.MasterPages
{
    public partial class Master : CorbisMasterBase
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

            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.MasterBaseStyles, "MasterBaseStyles");

            if (Request.Browser.Browser == "IE" && Request.Browser.MajorVersion < 7)
            {
                HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Ie6Styles, "IeStylesCSS");
            }
            else if (Request.Browser.Browser == "IE" && Request.Browser.MajorVersion == 7)
            {
                HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Ie7Styles, "Ie7StylesCSS");
            }

          
        }

       
    }

}
