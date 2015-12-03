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
using Corbis.Web.UI.Presenters.Checkout;
using Corbis.WebOrders.Contracts.V1;
using Corbis.Web.UI.Cart.ViewInterfaces;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Web.Entities;

namespace Corbis.Web.UI.Checkout
{
	public partial class OrderComplete : CorbisBasePage, IOrderConfirmationView
	{
		private CheckoutPresenter presenter;
		protected bool isOdd; 

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			
			Page.ClientScript.RegisterClientScriptInclude("OrderJs", SiteUrls.OrderScript);
			Page.ClientScript.RegisterClientScriptInclude("ImageJs", SiteUrls.ImageScript);
			HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Checkout, "CheckoutCSS");
			presenter = new CheckoutPresenter(this);

			ScriptManager manager = ScriptManager.GetCurrent(Page);
			manager.Services.Add(new ServiceReference("~/Checkout/CheckoutService.asmx"));
		}


		protected void Page_Load(object sender, EventArgs e)
		{
            AnalyticsData["events"] = AnalyticsEvents.CheckoutFinish;

            contactMessage.Text = String.Format(GetLocalResourceString("contactMessage"), Corbis.Web.UI.SiteUrls.CustomerService);

			// display TRUSTe?
			if (Profile.CountryCode != UScountryCode)
			{
				this.TRUSTeLink.Visible = false;
			}

            StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
            OrderUid = stateItems.GetStateItemValue<Guid>(
                OrderKeys.Name,
                OrderKeys.OrderUid,
                StateItemStore.AspSession);

#if DEBUG
            if (OrderUid == default(Guid))
            {
                //backdoor for test purpose?
				OrderUid = new Guid(Request.QueryString["orderuid"]);
			}
#endif
            //if there's no orderUid, then let's just kick user to orderhistory page and let them find it themselves.
            if (OrderUid == Guid.Empty)
            {
                Response.Redirect(SiteUrls.MyOrders);
            }

			presenter.GetOrderConfirmationData();
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

		private string _confirmationNumber;
		public string ConfirmationNumber
		{
			get { return _confirmationNumber; }
			set
			{
                if (AnalyticsData.ContainsKey("products"))
                {
                    AnalyticsData["products"] += (";" + value);
                }
                else
                {
                    AnalyticsData["products"] = value;
                }
                
                _confirmationNumber = value;
				confirmationNumber.Text = String.Format(GetLocalResourceObject("cornfirmationNumber").ToString(), value);
                if (WorkflowHelper.IsCOFFWorkflow(Request))
                {
                    orderSummary.Text = String.Format(GetLocalResourceObject("coffOrderSummary").ToString(), value);
                }
                else
                {
                    orderSummary.Text = String.Format(GetLocalResourceObject("orderSummary").ToString(), value);
                }
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
                    if (AnalyticsData.ContainsKey("products"))
                    {
                        AnalyticsData["products"] += (";" + image.CorbisId);
                    }
                    else
                    {
                        AnalyticsData["products"] = image.CorbisId;
                    }

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

		public bool OrderError
		{
			set
			{
				if (value)
				{
					Response.Redirect(SiteUrls.OrderSubmissionError);
				}
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
                    ((HtmlGenericControl)totalCost).InnerText = totalValue.Text = perContractText;
				}
			}
		}

		public string SubTotal
		{
			set { subtotalValueLabel.Text = value; }
		}

		public string PromotionValue
		{
			set { promotionAppliedValue.Text = value; }
		}

		public string FirstTax
		{
			set { taxValue.Text = value; }
		}

		public string SecondTax
		{
			set 
			{ 
				secondTaxValue.Text = value;
				secondTax.Visible = true;
			}
		}

		public string FirstTaxLabel
		{
			set { taxLabel.Text = value; }
		}

		public string SecondTaxLabel
		{
			set{ secondTaxlabel.Text = value; }
		}

	
		public string Shipping
		{
			set { shippingValue.Text = value; }
		}

		public string Total
		{
			set 
            {
                if (AnalyticsData.ContainsKey("products"))
                {
                    AnalyticsData["products"] += (";" + value);
                }
                else
                {
                    AnalyticsData["products"] = value;
                }
                ((HtmlGenericControl)totalCost).InnerText = totalValue.Text = value;
            }
		}
	
		#endregion

		#endregion

		//todo:  This property is getting set too many times.  It is initially getting set to 0, which should never be the case on this page.  If the user is trying to buy 1 image, the property is getting set twice.
        private int _totalImageCount;
		public int TotalImageCount
		{
			get { return _totalImageCount; }
			set 
            {
                if (AnalyticsData.ContainsKey("products"))
                {
                    AnalyticsData["products"] += (";" + value.ToString());
                }
                else
                {
                    AnalyticsData["products"] = value.ToString();
                }
                _totalImageCount = value; 
            }
		}


        #region IOrderConfirmationView Members


        public bool IsCoffRMResolution1280
        {
            set
            {
                //deliveryBlock.IsCoffRMResolution1280 = value;
            }
        }

        public string SpecialInstruction
        {
            set
            {
                deliveryBlock.DeliverySpecialInstructions = value;
            }
        }

        public string DeliverySubject
        {
            set
            {
                deliveryBlock.DeliverySubject = value;
            }
        }
        #endregion
    }
}
