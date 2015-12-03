using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.Checkout
{
    public partial class ExpressCheckout_OrderSubmissionError : CorbisBasePage
    {
        private enum QueryString
        {
            username,
            ReturnUrl,
            Reload,
            Execute,
            StandAlone,
            protocol
        }
        private string parentProtocol;
        protected override void OnInit(EventArgs e)
        {
            this.RequiresSSL = true;
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.AddScriptToPage(SiteUrls.ExpressCheckoutScript, "ExpressCheckoutScript");
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.ExpressCheckout, "ExpressCheckoutCSS");

			contactButton.OnClientClick = String.Format("window.open('{0}', '_parent');return false;", SiteUrls.CustomerService);
            CheckParentProtocol();
        }
        private void CheckParentProtocol()
        {
            // if opening URL is HTTPS, open iframe using https protocol
            string js = "var ParentProtocol = '";
            if (GetQueryString(QueryString.protocol.ToString()) == "https")
            {
                js += HttpsUrl + "';";
                parentProtocol = HttpsUrl;
            }
            else
            {
                js += HttpUrl + "';";
                parentProtocol = HttpUrl;
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "parentProtocol", js, true);
        }

    }
}
