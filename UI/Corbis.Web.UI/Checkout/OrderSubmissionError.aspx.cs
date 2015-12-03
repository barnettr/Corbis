using System;
using System.Web;
using System.Web.UI;
using Corbis.Web.Entities;
//using Corbis.Web.UI.Cart.ViewInterfaces;
using Corbis.Web.Utilities;
//using Corbis.Web.UI.Presenters.Checkout;
using Corbis.Membership.Contracts.V1;
using Corbis.CommonSchema.Contracts.V1;
//using Corbis.WebOrders.Contracts.V1;
//using WebOrderContracts = Corbis.WebOrders.Contracts.V1;
//using ServiceReference = System.Web.UI.ServiceReference;
using Corbis.Web.Utilities.StateManagement;

namespace Corbis.Web.UI.Checkout
{
    public partial class OrderSubmissionError : CorbisBasePage
    {

        protected override void OnInit(EventArgs e)
        {

            base.OnInit(e);
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Checkout, "CheckoutCSS");

        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
			btnContactCorbis.OnClientClick = String.Format("window.location='{0}';return false;", SiteUrls.CustomerService);
        }
    }
}
