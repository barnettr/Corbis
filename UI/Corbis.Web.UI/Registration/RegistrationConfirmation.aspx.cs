using System;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.Registration
{
    public partial class RegistrationConfirmation : CorbisBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ok.Click += new EventHandler(ok_Click);
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Registration, "RegisterCSS");
        }

        void ok_Click(object sender, EventArgs e)
        {
            string referrer = Request.UrlReferrer == null ? SiteUrls.Home : (Request.UrlReferrer.AbsolutePath == Request.Url.AbsolutePath ? SiteUrls.Home : Request.UrlReferrer.AbsoluteUri);
            Response.Redirect(referrer);
        }
    }
}
