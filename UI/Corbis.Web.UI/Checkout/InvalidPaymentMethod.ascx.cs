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


namespace Corbis.Web.UI.Checkout
{
    public partial class InvalidPaymentMethod : Corbis.Web.UI.CorbisBaseUserControl
    {
        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
			btnContactCorbis.OnClientClick = String.Format("window.open('{0}', '_parent');return false;", SiteUrls.CustomerService);
		}

        protected void btnApplyCredit_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Accounts/ApplyCredit.aspx");
        }
        #endregion
    }
}