using System;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.MasterPages
{
    public partial class ModalPopup : CorbisMasterBase
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

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

        }
    }
}
