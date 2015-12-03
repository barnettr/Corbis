using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.RFCD.Contracts.V1;
using Corbis.Web.UI.Controls;
using Corbis.Web.UI.MasterPages;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Authentication;
using System.Collections.Generic;
using System.Globalization;
using Corbis.Search.Contracts.V1;
using Corbis.LightboxCart.Contracts.V1;
using AjaxControlToolkit;
using Corbis.Web.UI.Presenters.Search;
using Corbis.Web.UI.Presenters.Pricing;
using Corbis.Web.UI.Pricing.Interfaces;
using Corbis.Web.Entities;
using LinkButton = System.Web.UI.WebControls.LinkButton;

namespace Corbis.Web.UI.Rfcd
{
    public partial class RfcdProducts : CorbisBaseUserControl, IPriceImageLink
    {
        const int tooltipImgHeight = 256;
        private const int MAXIUMCAPTIONLENGTH = 200;
        protected System.Web.UI.HtmlControls.HtmlAnchor priceImageLink;
        protected System.Web.UI.HtmlControls.HtmlImage pricingImage;
        protected Corbis.Web.UI.Controls.CenteredImageContainer thumbWrap;
        protected System.Web.UI.WebControls.Label searchCategory;
        protected System.Web.UI.WebControls.Label marketingCollection;
        protected System.Web.UI.WebControls.Label localizedLicenseModel;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ScriptManager manager = ScriptManager.GetCurrent(Page);
            manager.Services.Add(new ServiceReference("~/Search/SearchScriptService.asmx"));
            ShowDownloadingProhibited = Profile.IsChinaUser;
        }

        public object WebProductList
        {
            set
            {
                results.DataSource = value;
                results.DataBind();
            }
            get
            {
                return results.DataSource;
            }
        }

        //TOSO: This logic belongs in the presenter
        protected void Result_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || 
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RfcdDisplayImage currentItem = e.Item.DataItem as RfcdDisplayImage;
                
                if (currentItem != null)
                {
                    thumbWrap.ImgUrl = currentItem.Url128;
                    thumbWrap.Size = 128;
                    thumbWrap.Ratio = currentItem.AspectRatio;
                    thumbWrap.AltText = String.Format("{0} - {1}", currentItem.CorbisId, currentItem.Title);
                    searchCategory.Visible = false;
                    marketingCollection.Visible = false;
                
                    HtmlContainerControl productBlock = (HtmlContainerControl)e.Item.FindControl("productBlock");
                    productBlock.Attributes["class"] += string.Format(@" {0}", currentItem.LicenseModel);
                }
            }
        }

        protected void EnlargeImageClick(object sender, CommandEventArgs e)
        {
            Response.Redirect(string.Format("{0}?id={1}&{2}", SiteUrls.Enlargement, e.CommandArgument, Request.QueryString));
        }

        protected void MyLightboxClick(object sender, CommandEventArgs e)
        {
            Response.Redirect(string.Format("{0}?id={1}", SiteUrls.Lightboxes, e.CommandArgument));
        }

        protected void AddToCartClick(object sender, CommandEventArgs e)
        {
            LinkButton addTocart = (LinkButton)sender;
            int tempCartCount = Profile.Current.CartItemsCount;
            SearchPresenter searchPresenter = new SearchPresenter(this);
            searchPresenter.AddToCart(new Guid(e.CommandArgument.ToString()));
            if (tempCartCount != Profile.Current.CartItemsCount)
            {
                ShowAddCartMessage(addTocart, true);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "showAddCartSuccessModal", String.Format("confirmItemAdded('{0}');", addTocart.ClientID), true);
            }
        }

        protected void GoToCart_Click(object sender, CommandEventArgs e)
        {

            LinkButton continueShopping = (LinkButton)sender;
            ShowAddCartMessage(continueShopping, false);

            Response.Redirect(string.Format("{0}?id={1}", SiteUrls.Cart, e.CommandArgument));
        }

        protected void ContinueShopping_Click(object sender, CommandEventArgs e)
        {
            LinkButton continueShopping = (LinkButton)sender;
            ShowAddCartMessage(continueShopping, false);

        }

        private void ShowAddCartMessage(LinkButton addToCartButton, bool status)
        {
            for (int itemIndex = 0; itemIndex < results.Items.Count; itemIndex++)
            {
                LinkButton button = (LinkButton)results.Items[itemIndex].FindControl(addToCartButton.ID);

                if (button.CommandArgument == addToCartButton.CommandArgument)
                {

                    HtmlGenericControl addtoCartDiv = (HtmlGenericControl)results.Items[itemIndex].FindControl("addCartMessageDiv");
                    addtoCartDiv.Visible = status;
                    ((NoSearchBar)Page.Master).GlobalNav.UpdateCartCount();
                    break;
                }
            }
        }

        #region IPriceImageLink Members

        private Corbis.Web.Entities.ParentPage parentPage = Corbis.Web.Entities.ParentPage.Unknown;
        public Corbis.Web.Entities.ParentPage ParentPage
        {
            get { return parentPage; }
            set { parentPage = value; }
        }

        public bool ShowPricingLink
        {
            get { return priceImageLink.Visible; }
            set { priceImageLink.Visible = value; }
        }

        private string pricingAltText;
        public string PricingAltText
        {
            get
            {
                return pricingAltText;
            }
            set
            {
                pricingAltText = (String)GetLocalResourceObject(value);
                this.pricingImage.Alt = pricingAltText;
                this.pricingImage.Attributes.Add("title", pricingAltText);
            }
        }

        private string pricingNavigateUrl;
        public string PricingNavigateUrl
        {
            get
            {
                return pricingNavigateUrl;
            }
            set
            {
                pricingNavigateUrl = value;
                priceImageLink.Attributes.Add("onclick", value);
            }
        }

        public string RMRawUrl
        {
            get { return SiteUrls.RMPricing; }
        }

        public string RFRawUrl
        {
            get { return SiteUrls.RFPricing; }
        }

        public string RSRawUrl
        {
            get { return SiteUrls.RSPricing; }
        }

        public string CustomerServiceUrl
        {
            get { return "javascript:location.href='" + SiteUrls.CustomerService + "';"; }
        }

        public int RFPricingPageWidth
        {
            get { return 640; }
        }

        public int RFPricingPageHeight
        {
            get { return 480; }
        }

        public int RMPricingPageWidth
        {
            get { return 700; }
        }

        public int RMPricingPageHeight
        {
            get { return 545; }
        }

        public int RSPricingPageWidth
        {
            get { return 640; }
        }

        public int RSPricingPageHeight
        {
            get { return 480; }
        }

		public bool IsAnonymous
		{
			get { return Profile.IsAnonymous; }
		}
        #endregion

        #region IPriceImageLink Members


        public bool ShowDownloadingProhibited
        {
            get
            {
                return false;
            }
            set
            { }
        }

        #endregion
    }
}
