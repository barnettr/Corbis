using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.Web.UI.Presenters.Checkout;
using Corbis.Web.UI.Cart.ViewInterfaces;
using Corbis.WebOrders.Contracts.V1;

namespace Corbis.Web.UI.Checkout
{
    public partial class Steps : Corbis.Web.UI.CorbisBaseUserControl, ISteps
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void quitCheckOut_OnClick(object sender, EventArgs e)
        {
            Response.Redirect(@"~/Checkout/Checkout.aspx");
        }

        #region ISteps members 

        public IProject Project
        {
            get { return this.stepProject; }
        }

        public IDelivery Delivery
        {
            get { return this.stepDelivery; }
        }

        public IPayment Payment
        {
            get { return this.stepPayment; }
        }

        public ISubmit Submit
        {
            get { return this.stepSubmit; }
        }
        #endregion
    }
}