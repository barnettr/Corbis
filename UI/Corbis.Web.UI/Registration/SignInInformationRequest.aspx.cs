using System;
using Corbis.Web.UI.Presenters.SignIn;
using Corbis.Web.UI.SignIn.ViewInterfaces;
using Corbis.Web.Utilities;
using Corbis.Membership.Contracts.V1;
using System.Web.UI;

namespace Corbis.Web.UI.Registration
{
    public partial class SignInInformationRequest : CorbisBasePage
    {
        protected override void OnInit(EventArgs e)
        {
            this.RequiresSSL = true;

            // if opening URL is HTTPS, open iframe using https protocol
            string js = "var ParentProtocol = '";
            if (Request.QueryString["protocol"] == "https")
            {
                js += HttpsUrl + "';";
            }
            else
            {
                js += HttpUrl + "';";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "parentProtocol", js, true);
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.SignIn, "SignInCSS");
            base.OnInit(e);
        }

		protected void Page_Load(object sender, EventArgs e)
		{
			ContactUsLabel.Text = String.Format(GetLocalResourceString("ContactUsLabel"), Corbis.Web.UI.SiteUrls.CustomerService);
		}
    }
}
