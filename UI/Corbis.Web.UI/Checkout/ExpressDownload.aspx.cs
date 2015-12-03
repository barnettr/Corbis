using System;
using System.Collections.Generic;
using System.Web.UI;
using Corbis.Web.Utilities;
using Corbis.WebOrders.Contracts.V1;
using Corbis.CommonSchema.Contracts.V1.Image;
using Corbis.Web.UI.Presenters.Checkout;
using Corbis.Web.UI.Cart.ViewInterfaces;

namespace Corbis.Web.UI.Checkout
{
	public partial class ExpressDownload : CorbisBasePage, IOrderConfirmationView
	{
		private CheckoutPresenter presenter;
        private string parentProtocol;
        private enum QueryString
        {
            username,
            ReturnUrl,
            Reload,
            Execute,
            StandAlone,
            protocol
        }

		protected override void OnInit(EventArgs e)
		{
            base.OnInit(e);
			Page.ClientScript.RegisterClientScriptInclude("OrderScript", SiteUrls.OrderScript);
			HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.ExpressDownload, "ExpressDownloadCSS");
			presenter = new CheckoutPresenter(this);

			ScriptManager manager = ScriptManager.GetCurrent(Page);
			manager.Services.Add(new ServiceReference("~/Checkout/CheckoutService.asmx"));
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

		protected void Page_Load(object sender, EventArgs e)
		{
			contactMessage.Text = String.Format(GetLocalResourceString("contactMessage"), Corbis.Web.UI.SiteUrls.CustomerService);

			//if (Request.UrlReferrer.AbsolutePath.ToLower() != SiteUrls.ExpressCheckout)
			//{
			//    ScriptManager.RegisterStartupScript(this, this.GetType(), "ExpressDownloadError", "CorbisUI.Order.ShowExpressDownloadError();", true);
			//    return;
			//}
            //this.AddScriptToPage(SiteUrls.CorbisScript, "CorbisUI");
			OrderUid = new Guid(Request.QueryString["orderuid"]);
			presenter.GetOrderConfirmationData();
		}

		#region IOrderConfirmationView Members

        public bool IsCoffRMResolution1280 { set { } }
        public string SpecialInstruction { set { } }
        public string DeliverySubject { set { } }

		private Guid _orderUid;
		public Guid OrderUid
		{
			get { return _orderUid; }
			set { _orderUid = value; }
		}

		public List<DownloadableImage> ImagesToDownload
		{
			get
			{
				return null;
			}
			set
			{
				if(value.Count == 1)
				{
					expressImage.Items = value;
				}
				else
				{
					ScriptManager.RegisterStartupScript(this, this.GetType(), "ExpressDownloadError", "CorbisUI.Order.ShowExpressDownloadError();", true);
				}
			}
		}

		public bool OrderError
		{
			set
			{
				if (value)
				{
					ScriptManager.RegisterStartupScript(this, this.GetType(), "ExpressDownloadError", "CorbisUI.Order.ShowExpressDownloadError();", true);
				}
			}
		}

		public string OrderEmail
		{
			set 
			{
				orderEmail.Text = value;
			}
		}

		public string ConfirmationNumber
		{
			get
			{
				return confirmationNumber.Text;
			}
			set
			{
				confirmationNumber.Text = value;
			}
		}

		public string Total
		{
			set
			{
				orderTotal.Text = value;
			}
		}

		public bool PerContract
		{
			set
			{
				if (value)
				{
					string perContractText = GetLocalResourceObject("perContract").ToString();
					orderTotal.Text = perContractText;
				}
			}
		}

		#region Not used properties

		public DateTime OrderDate
		{
			set 
			{ 
				//Not used
			}
		}

		public List<MediaLicenseDetail> LicenseDetails
		{
			set 
			{
				//Not used
			}
		}

		public string CurrencyCode
		{
			get
			{
				return "";
			}
			set
			{
				//Not used
			}
		}

		public string ProjectName
		{
			set 
			{
				//Not used
			}
		}

		public string JobNumber
		{
			set
			{
				//Not used
			}
		}

		public string PoNumber
		{
			set
			{
				//Not used
			}

		}

		public string Licensee
		{
			set
			{
				//Not used
			}

		}

		public string DeliveryEmails
		{
			set
			{
				//Not used
			}
		}

		public Corbis.CommonSchema.Contracts.V1.DeliveryMethod OrderDelivery
		{
			set
			{
				//Not used
			}
		}

		public PaymentMethod OrderPayment
		{
			set
			{
				//Not used
			}
		}

		public Corbis.Membership.Contracts.V1.MemberAddress BillingAddress
		{
			set
			{
				//Not used
			}
		}

		public Corbis.Membership.Contracts.V1.CreditCard CreditCardInformation
		{
			set
			{
				//Not used
			}
		}

		public string CorporateAccountName
		{
			set
			{
				//Not used
			}
		}

		public string SubTotal
		{
			set
			{
				//Not used
			}
		}

		public string PromotionValue
		{
			set
			{
				//Not used
			}
		}

		public string FirstTax
		{
			set
			{
				//Not used
			}
		}

		public string SecondTax
		{
			set
			{
				//Not used
			}
		}

		public string FirstTaxLabel
		{
			set
			{
				//Not used
			}
		}

		public string SecondTaxLabel
		{
			set
			{
				//Not used
			}
		}

		public string Shipping
		{
			set
			{
				//Not used
			}
		}

		#endregion

		#endregion
	}
}
