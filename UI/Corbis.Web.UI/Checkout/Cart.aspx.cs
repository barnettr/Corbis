using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Search.Contracts.V1;
using Corbis.Web.UI.Cart.ViewInterfaces;
using Corbis.Web.UI.Lightboxes.ViewInterfaces;
using Corbis.Web.Utilities;
using Corbis.Web.UI.Presenters.Checkout;
using Corbis.Web.Utilities.StateManagement;
using System.Web;
using Corbis.Web.Entities;
using Corbis.Web.UI.Presenters.Search;

namespace Corbis.Web.UI.Checkout
{
    public partial class Cart : CorbisBasePage, ICartView 
    {
        CheckoutPresenter checkoutPresenter;
        private StateItemCollection stateItems;


        protected void Page_Load(object sender, EventArgs e)
        {
            AnalyticsData["events"] = AnalyticsEvents.CartStart;
            AnalyticsData["prop3"] = "standard";

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            OutputEnumClientScript<DragStatus>(Page);
            OutputEnumClientScript<PriceStatus>(Page);

            

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.AddScriptToPage(SiteUrls.CartScript, "CartScript");
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Cart, "CartCSS");
			ScriptManager manager = (ScriptManager)Master.FindControl("scriptManager");
			manager.Services.Add(new ServiceReference("~/Checkout/CartScriptService.asmx"));

           

            stateItems = new StateItemCollection(HttpContext.Current);
            String returnToSearchURL = stateItems.GetStateItemValue<string>(SearchSessionKeys.DirectlyManipulatedSearch,null,StateItemStore.Cookie);

            if(!string.IsNullOrEmpty(returnToSearchURL))
            {
                this.returnToSearchButton.Visible = true;
                this.returnToSearchButton.Attributes["onclick"] = string.Format("window.location = '{0}?{1}';", SiteUrls.SearchResults,StringHelper.EncodeToJsString(returnToSearchURL));
            }
            else
                this.returnToSearchButton.Visible = false;


            if (Profile.IsAnonymous)
            {

                string redirectURL = SiteUrls.Home + "?ReturnUrl=" + SiteUrls.Cart;
                Response.Redirect(redirectURL);

                return;
            }

            if (!Profile.IsECommerceEnabled)
            {
                Response.Redirect(SiteUrls.Home);
                return;
            }


        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            checkoutPresenter = new CheckoutPresenter((ICartView)Page);

            if (!IsPostBack)
            {
                checkoutPresenter.GetCartContents();
            }
        }

        public List<CartDisplayImage> PricedImages
        {
            get { return null; }
            set
            {
                this.pricedItems.CartItemList = value;
                if (((List<CartDisplayImage>)this.pricedItems.CartItemList).Count == 0)
                {
                    pricedZoneInstruction.Attributes.Add("class", "instructions");
                    this.tibDeleteAllPriced.IsDisabled = true;

                }
        }
        }
        public List<CartDisplayImage> PriceMultipleImages
        {
            get { return null; }
            set { this.pricingMultipleImages.CartItemList = value; }
        }
        public List<CartDisplayImage> UnPricedImages
        {
            get { return null; }
            set
            {
                this.unPricedItems.CartItemList = value;
                if (((List<CartDisplayImage>)this.unPricedItems.CartItemList).Count == 0)
                {
                    unpricedZoneInstruction.Attributes.Add("class", "instructions");
                    this.tibDeleteAllUnpriced.IsDisabled = true;

                }

            }
        }

        public void UpdateMyCartMsg()
        {
            int count1 = ((List<CartDisplayImage>)pricedItems.CartItemList).Count + ((List<CartDisplayImage>)this.checkoutItems.CartItemList).Count;
            int count2 = ((List<CartDisplayImage>)unPricedItems.CartItemList).Count;
            string cartDetail = (string)GetLocalResourceObject("cartDetail");
            this.myCartDetail.InnerHtml = string.Format(cartDetail, count1 + count2, count1, count2);
        }

		public List<CartDisplayImage> CheckoutImages
        {
			get { return null; }
			set { this.checkoutItems.CartItemList = value; }
        }
        public Int32 CheckoutTotalItemCount 
        {
            get
            {
                return int.Parse(this.checkoutTotalCount.Text);
            }
            set
            {
                this.checkoutTotalCount.Text = value.ToString();
            }
        }
        public string CheckoutTotalItemCost 
        {
            get
            {
                return this.checkoutTotalCost.Text;
            }
            set
            {
                this.checkoutTotalCost.Text = value;
            }
        }
        public String CurrencyCode
        {
            get
            {
                return this.currencyCode.Text.ToString();
            }
            set
            {
                this.currencyCode.Text = value;
            }
        }
    }
}
