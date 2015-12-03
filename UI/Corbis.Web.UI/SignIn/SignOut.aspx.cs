using System;
using System.Web.Security;
using Corbis.Web.Entities;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.SignIn
{
    public partial class SignOut : CorbisBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ok.Click += delegate { Response.Redirect(SiteUrls.RedirectTop); };
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.SignIn, "SignInCSS");

            if (!Profile.IsAnonymous)
            {
                // sign out
                if (Profile.IsAuthenticated)
                {
                    FormsAuthentication.SignOut();
                }
                ProfileCookie cookie = new ProfileCookie();
                cookie.Invalidate();

                if (Context.Session != null)
                {
                    Context.Session.Clear();
                }
            }
        }
    }
}
