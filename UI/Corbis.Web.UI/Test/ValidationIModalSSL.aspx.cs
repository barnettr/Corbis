using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Corbis.Web.UI.Test
{
    public partial class ValidationIModalSSL : Corbis.Web.UI.CorbisBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
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
        }
    }
}
