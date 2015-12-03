using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.UI.Accounts.ViewInterfaces;
using Corbis.Web.Utilities;
using Corbis.Web.Utilities.StateManagement;

namespace Corbis.Web.UI.Checkout
{
    public partial class CreditCardAddEdit : CorbisBasePage
    {
        private string ccUidString;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.RequiresSSL = true;
            if (!this.Profile.IsAuthenticated)
            {
                Response.Redirect(SiteUrls.Home);
            }
            ccUidString = Request.QueryString["ccUid"];
            try
            {
                Guid ccUid = new Guid(ccUidString);
                pageTitle1.Visible = false;
                pageTitle2.Visible = true;
                cardCreator.Visible = false;
                cardSelector.Visible = true;
                if (!IsPostBack)
                {
                    cardSelector.CreditCards = CreditCards;
                    cardSelector.SelectCreditCard(ccUid);
                   
                }
            }
            catch (Exception)
            {
                ccUidString = string.Empty;
            }
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.ExpressCheckout, "ExpressCheckoutCSS");
            cardSelector.CreditCardAdded += cardSelector_CreditCardSelected;
          

        }
        private List<CreditCard> creditCards;
        public List<CreditCard> CreditCards
        {
            get
            {           
                // Lazy Load
                if (creditCards == null)
                {
                    StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                    creditCards = stateItems.GetStateItemValue<List<CreditCard>>(
                        "ExpressCheckout",
                        "CreditCards",
                        StateItemStore.AspSession);



                }
                return creditCards;            
            }
        }

        protected void cardCreator_CreditCardAdded(object sender, CommandEventArgs e)
        {
            CreditCard card = e.CommandArgument as CreditCard;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "RefreshPaymentMethods", "parent.CorbisUI.ExpressCheckout.RefreshPaymentMethods('" + card.CreditCardUid + "')", true);
            // Close the page w/javascript, run ajax update on credit card list, select the correct one on update
            // parent.close, parent.updatepaymentmethod w/parameter of new cc

            //CreditCard = card;
            //if (card != null)
            //{
            //    foreach (ListItem item in paymentMethodControl.SelectPaymentMethod.Items)
            //    {
            //        if (item.Value == card.CreditCardUid.ToString())
            //        {
            //            item.Attributes["Selected"] = "selected";
            //            break;
            //        }
            //    }


            //}
         }

        protected void cardSelector_CreditCardSelected(object sender, CommandEventArgs e)
        {
        }

        protected void cardCreator_CancelClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "cancelClick", "parent.CloseModal('creditCardAddEdit');", true);
        }
        protected void cardSelector_CancelClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "cancelClick", "parent.CloseModal('creditCardAddEdit');", true);
        }
    }
}
