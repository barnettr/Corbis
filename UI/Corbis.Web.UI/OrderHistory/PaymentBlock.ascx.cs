using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Corbis.Membership.Contracts.V1;
using PaymentMethod=Corbis.WebOrders.Contracts.V1.PaymentMethod;

namespace Corbis.Web.UI.OrderHistory
{
    public partial class PaymentBlock : System.Web.UI.UserControl
    {
        public bool EnableEdit { get; set; }
        protected override void OnInit(EventArgs e)
        {
            if (!EnableEdit)
            {
                editLink.Visible = false;
            }
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public PaymentMethod OrderPayment
        {
            set
            {

                accountLabel.Visible = (value == PaymentMethod.CorporateAccount);
                cartTypeLabel.Visible = (value == PaymentMethod.CreditCard);
                secondRow.Visible = (value == PaymentMethod.CreditCard);
                thirdRow.Visible = (value == PaymentMethod.CreditCard);
                companyAddressLbl.Visible = (value == PaymentMethod.CorporateAccount);
                billingAddressLbl.Visible = (value == PaymentMethod.CreditCard);
                if (!companyAddressLbl.Visible && !billingAddressLbl.Visible)
                    billingAddressLbl.Visible = true;


            }
        }

        public MemberAddress BillingAddress
        {
            set
            {
                if (value != null)
                {
                    this.billingName.Text = Server.HtmlEncode(value.Name);
                    this.billingAddress123.Text = Server.HtmlEncode(value.Address1);
                    if (!string.IsNullOrEmpty(value.Address2))
                    {
                        this.billingAddress123.Text += @"<br />" + Server.HtmlEncode(value.Address2);
                    }
                    if (!string.IsNullOrEmpty(value.Address3))
                    {
                        this.billingAddress123.Text += @"<br />" + Server.HtmlEncode(value.Address3);
                    }
                    this.billingCity.Text = Server.HtmlEncode(value.City) + ",";
                    this.billingState.Text = Server.HtmlEncode(value.RegionCode);
                    this.billingZip.Text = Server.HtmlEncode(value.PostalCode);
                    this.billingCountry.Text = Server.HtmlEncode(value.CountryCode);
                }

            }
        }

		public string CorporateAccountName
		{
			set
			{
				accountOrCard.Text = Server.HtmlEncode(value);
			}
		}

        public CreditCard CreditCardInformation
        {
            set
            {

                accountLabel.Visible = false;
                cartTypeLabel.Visible = true;
                if (value != null)
                {
                    //accountOrCard.Text = Server.HtmlEncode(value.CreditCardType);
                    accountOrCard.Text = CorbisBasePage.GetResourceString("Resource", value.CreditCardTypeCode + "_card");
                    cardDisplayNumberValue.Text = Server.HtmlEncode(value.CardNumberViewable);
                    cardHolderNameValue.Text = Server.HtmlEncode(value.NameOnCard);
                }
            }
        }
    }
}