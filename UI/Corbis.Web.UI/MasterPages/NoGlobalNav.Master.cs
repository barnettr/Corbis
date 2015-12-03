using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.MasterPages
{
    public partial class NoGlobalNav : CorbisMasterBase
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Request.Browser.Browser == "IE" && Request.Browser.MajorVersion < 7)
            {
                this.AddScriptToPage(SiteUrls.IE7Script, "IE7JS");
            }
            this.AddScriptToPage(SiteUrls.MasterBaseJavascript, "MBScript");

            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.MasterBaseStyles, "MasterBaseStyles");

            if (Request.Browser.Browser == "IE")
            {
                HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.IeStyles, "IeStyles");
			    if (Request.Browser.MajorVersion == 7)
			    {
				    HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Ie7Styles, "Ie7StylesCSS");
			    }
                else if (Request.Browser.MajorVersion < 7)
			    {
				    HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Ie6Styles, "IeStylesCSS");
			    }
           }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetCopyrightText();
        }
        public bool ShowFooter
        {
            set
            {
                this.footerDiv.Visible = value;
            }
        }
        private void SetCopyrightText()
        {
            this.footer.Text = CopyrightHelper.CopyrightText;
        }
    }
}
