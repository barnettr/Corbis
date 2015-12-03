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
using Corbis.Web.Authentication;

namespace Corbis.Web.UI
{
    public partial class BetaBackToProSiteTracker : CorbisBasePage
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.SetAnalytics("LND:CustomerService:Beta:BackToOldProSite");

            ClientScript.RegisterStartupScript(typeof(Page), "goBackToPro", "<script type=\"text/javascript\">var moveOn = function(){ window.location='http://pro.corbis.com'; }; moveOn.delay(3000);</script>");
        }
    }
}
