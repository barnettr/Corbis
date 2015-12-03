using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.Framework.Globalization;
using Corbis.Web.Utilities;
using System.Xml.Linq;
using Corbis.Web.Entities;
using Corbis.Web.UI.Presenters.OrderHistory;
using Corbis.WebOrders.Contracts.V1;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Web.UI.OrderHistory.Interfaces;

namespace Corbis.Web.UI.OrderHistory
{
    public partial class OrderHistorySummary : CorbisBasePage, IOrderSummaryView
    {
        private OrderHistoryPresenter presenter;
		protected bool isOdd; 

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			
			Page.ClientScript.RegisterClientScriptInclude("OrderHistoryJs", SiteUrls.OrderHistoryScript);
			Page.ClientScript.RegisterClientScriptInclude("ImageJs", SiteUrls.ImageScript);
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.OrderHistorySummary, "OrderHistorySummaryCSS");
            presenter = new OrderHistoryPresenter(this);            
            ScriptManager manager = ScriptManager.GetCurrent(Page);
            this.AddScriptToPage(SiteUrls.AddToCartScript, "AddToCartScript");
            manager.Services.Add(new ServiceReference("~/Checkout/CartScriptService.asmx"));
            manager.Services.Add(new ServiceReference("~/Checkout/CheckoutService.asmx"));

            manager.Services.Add(new ServiceReference("~/OrderHistory/OrderHistoryScriptService.asmx"));
		}


		protected void Page_Load(object sender, EventArgs e)
		{
            AnalyticsData["events"] = AnalyticsEvents.OrderHistorySummaryLoad;

            // display TRUSTe?
			if (Profile.CountryCode != UScountryCode)
			{
				this.TRUSTeLink.Visible = false;
			}
            GuidHelper.TryParse(Request.QueryString[OrderKeys.OrderUid], out _orderUid);
            if (!IsPostBack)
            {
                StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
			}
            presenter.GetOrderHistorySummaryDetails();
			contactUsButton.OnClientClick = String.Format("window.location='{0}';return false;", SiteUrls.CustomerService);
			contactMessage.Text = String.Format(GetLocalResourceString("contactMessage"), Corbis.Web.UI.SiteUrls.CustomerService);
		}
        protected void Page_Error(Object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            if (ex != null)
            {
                error.Visible = true;
                block1.Visible = false;
                block2.Visible = false;
                block4.Visible = false;

            }
        }

        public bool ShowError
        {
            set
            {
                this.error.Visible = value;
                block1.Visible = !value;
                block2.Visible = !value;
                block4.Visible = !value;
               
            }
        }
		protected void imageDetailRepeater_ItemDataBound(Object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				LicenseDetail licenseDetailControl;
				MediaLicenseDetail licenseDetail = e.Item.DataItem as MediaLicenseDetail;

				if (e.Item.ItemType == ListItemType.Item)
				{
					licenseDetailControl = e.Item.FindControl("oddLicenseDetail") as LicenseDetail;
					isOdd = true;
				}
				else
				{
					licenseDetailControl = e.Item.FindControl("evenLicenseDetail") as LicenseDetail;
					isOdd = false;
				}
				licenseDetailControl.CurrencyCode = this.CurrencyCode;
				licenseDetailControl.LicenseDetailInfo = licenseDetail;
			}
		}


		#region IOrderConfirmationView
		
		public string OrderEmail
		{
			set
			{
                orderEmail.Text = String.Format(GetLocalResourceObject("orderEmail").ToString(), value);
			}
		}

		private string _orderNumber;
		public string OrderNumber
		{
			get { return _orderNumber; }
			set
			{
				_orderNumber = value;
				//confirmationNumber.Text = String.Format(GetLocalResourceObject("cornfirmationNumber").ToString(), value);
				orderSummary.Text = String.Format(GetLocalResourceObject("orderSummary").ToString(), Server.HtmlEncode(value));
			}
		}

		private Guid _orderUid;
		public Guid OrderUid
		{
			get {return _orderUid; }
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
				//Download images
				TotalImageCount = 0;
				value.ForEach(new Action<DownloadableImage>(delegate(DownloadableImage image) 
				{
					if (image.OfferingType == Corbis.CommonSchema.Contracts.V1.OfferingType.RFCD) 
					{
						TotalImageCount += image.RfcdImageCount;
					} 
					else 
					{
						TotalImageCount++; 
					} 
				}));

				if (value.Count > 1)
				{
					downloadItemCount.Text = String.Format(GetLocalResourceObject("downloadItemsCount").ToString(), value.Count.ToString());
					orderTotalItems.Text = String.Format(GetLocalResourceObject("orderTotalItems").ToString(), value.Count.ToString());
				}
				else
				{
					downloadItemCount.Text = String.Format(GetLocalResourceObject("downloadItemCount").ToString(), value.Count.ToString());
					orderTotalItems.Text = String.Format(GetLocalResourceObject("orderTotalItem").ToString(), value.Count.ToString());
				}

				orderItems.Items = value;
			}
		}

		public List<MediaLicenseDetail> LicenseDetails
		{
			set
			{
				imageDetailRepeater.DataSource = value;
				imageDetailRepeater.DataBind();
			}
		}

        public DateTime OrderDate
        {
            set
            {
                orderDateLabel.Text = String.Format(GetLocalResourceObject("orderDateLabel").ToString(), DateHelper.GetLocalizedLongDateWithoutWeekday(value));
            }
        }

		private string currencyCode;
		public string CurrencyCode
		{
			get { return currencyCode;  }
			set { currencyCode = value; }
		}

		#region Project block properties

		public string ProjectName
		{
			set { projectBlock.ProjectName = value; }
		}

        public string JobNumber
        {
			set { projectBlock.JobNumber = value; }
        }

        public string PoNumber
        {
			set { projectBlock.PoNumber = value; }
        }

        public string Licensee
        {
			set { projectBlock.Licensee = value; }
		}

		#endregion

		#region Delivery block properties

		public string DeliveryEmails
		{
			set { deliveryBlock.DeliveryEmails = value; }
		}

		public DeliveryMethod OrderDelivery
		{
			set { deliveryBlock.OrderDelivery = value; }
		}
		
		#endregion

		#region Payment block properties
		
		public Corbis.WebOrders.Contracts.V1.PaymentMethod OrderPayment
        {
            set
            {
				paymentBlock.OrderPayment = value;
			}
		}

        public MemberAddress BillingAddress
        {
            set
            {
				paymentBlock.BillingAddress = value;
			}
		}

        public CreditCard CreditCardInformation
        {
			set
			{
				paymentBlock.CreditCardInformation = value;
			}
		}

		public string CorporateAccountName
		{
			set
			{
				paymentBlock.CorporateAccountName = value;
			}
		}

		#endregion

		#region Totals properties

		public bool PerContract
		{
			set 
			{
				if (value)
				{
					string perContractText = GetLocalResourceObject("perContract").ToString();
					subtotalValueLabel.Text = perContractText;
					promotionAppliedValue.Text = perContractText;
					taxValue.Text = GetLocalResourceObject("toBedetermined").ToString();
					shippingValue.Text = perContractText;
				}
			}
		}

		public string SubTotal
		{
			set { subtotalValueLabel.Text = Server.HtmlEncode(value); }
		}

		public string PromotionValue
		{
			set { promotionAppliedValue.Text = Server.HtmlEncode(value); }
		}

        public List<OrderFee> Fees
        {
            set
            {
                foreach (OrderFee fee in value)
                {
                    DisplayValue<FeeType> feeDisplayValue = 
                        CorbisBasePage.GetEnumDisplayValue<FeeType>(fee.Type, true);
                    fee.DisplayOrder = 
                        feeDisplayValue.Ordinal.HasValue 
                        ? feeDisplayValue.Ordinal.Value
                        : 0;
                    fee.DisplayText = feeDisplayValue.Text;
                }
                value.Sort(delegate(OrderFee x, OrderFee y)
                {
                    return x.DisplayOrder.CompareTo(y.DisplayOrder);
                });
                feeDetailsRepeater.DataSource = value;
                feeDetailsRepeater.DataBind();
            }
        }

		public string Tax
		{
			set { taxValue.Text = Server.HtmlEncode(value); }
		}

		public string Shipping
		{
			set { shippingValue.Text = Server.HtmlEncode(value); }
		}

		public string Total
		{
			set 
            {
                totalValue.Text = Server.HtmlEncode(value);
            }
		}
	
		#endregion

		#endregion

		private int _totalImageCount;
		public int TotalImageCount
		{
			get { return _totalImageCount; }
			set { _totalImageCount = value; }
		}

	
    }
}
