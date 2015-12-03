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
using Corbis.Image.Contracts.V1;
using Corbis.Web.UI.Controls;
using Corbis.Web.UI.Lightboxes.ViewInterfaces;
using Corbis.Web.UI.Presenters.Checkout;
using Corbis.Web.UI.Cart.ViewInterfaces;
using Corbis.Web.Utilities;
using Corbis.Search.Contracts.V1;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Web.Entities;
using Corbis.Web.UI.Pricing.Interfaces;
using Corbis.Web.UI.Presenters.Pricing;

namespace Corbis.Web.UI.Checkout
{
    public partial class CartItems : Corbis.Web.UI.CorbisBaseUserControl, IPriceImageLink
    {
        CheckoutPresenter checkoutPresenter;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Page is ICartView)
            {
                checkoutPresenter = new CheckoutPresenter((ICartView)Page);
            }
            else
            {
                checkoutPresenter = null;
            }
            if (!RenderInList)
            {
                noItemMessage.Text = (string)GetLocalResourceObject((Zone == CartContainerEnum.Priced) ? "noItemMsgPriced" : "noItemMsgUnpriced");
            }
            ShowDownloadingProhibited = Profile.IsChinaUser;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
            }

        }

        //private bool _isPriced;
        //public bool IsPriced
        //{
        //    get { return _isPriced; }
        //    set { _isPriced = value; }
        //}

        private CartContainerEnum _zone;
        public CartContainerEnum Zone
        {
            get { return _zone; }
            set { _zone = value; }
        }   


		private bool _renderInList;
		public bool RenderInList
		{
			get { return _renderInList; }
			set { _renderInList = value; }
		}


        public object CartItemList
        {
            get { return cartItemsID.DataSource; }
            set
            {
                cartItemsID.DataSource = value;
                cartItemsID.DataBind();
                if (cartItemsID.Items.Count == 0)
                {
                    instructionId.Visible = true;
                }
            }
        }
        
        private Corbis.Web.Entities.ParentPage parentPage = Corbis.Web.Entities.ParentPage.Unknown;
        public Corbis.Web.Entities.ParentPage ParentPage
        {
            get { return parentPage; }
            set { parentPage = value; }
        }

        //TODO: This logic all belongs in the presenter
        protected void cartItemsID_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int HEIGHT = 128;

            if (Zone == CartContainerEnum.PriceMultiple)
            {
                HEIGHT = 90;
            }
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CartDisplayImage displayImage = (CartDisplayImage)e.Item.DataItem;
                Guid productUid = displayImage.ProductUid;
                string imageid = displayImage.CorbisId;

                PricingPresenter.InitializePriceImageLink(this, displayImage);
                
                System.Web.UI.WebControls.Label licenseDetailsTag = (System.Web.UI.WebControls.Label)e.Item.FindControl("licenseDetails");
                if ((displayImage.EffectivePriceStatus & PriceStatus.ContactOutline) == PriceStatus.ContactOutline
                    || (displayImage.EffectivePriceStatus == PriceStatus.Unknown && displayImage.IsOutLine))
                {
                    // For "Contact Outline" the LicenseDetails tab opens Request price/Contact us form
                    licenseDetailsTag.Attributes.Add("onclick", "javascript:CorbisUI.Pricing.ContactUs.OpenRequestForm('" + displayImage.CorbisId + "', null, 1, '" + ParentPage.ToString() + "');return false;");
                }
                else
                {
                    licenseDetailsTag.Attributes.Add("onclick", PricingNavigateUrl);
                }
                

                HtmlGenericControl priceTag = e.Item.FindControl("priceTag") as HtmlGenericControl;
                //priceTag.Visible = ShowPricingLink;
                if ((displayImage.EffectivePriceStatus & PriceStatus.ContactUs) == PriceStatus.ContactUs 
                    || (displayImage.EffectivePriceStatus & PriceStatus.ContactOutline) == PriceStatus.ContactOutline
                    || (displayImage.EffectivePriceStatus == PriceStatus.Unknown && displayImage.IsOutLine))
                {
                    // Opens  Request price/Contact us form
                    priceTag.Attributes.Add("onclick", "javascript:CorbisUI.Pricing.ContactUs.OpenRequestForm('" + displayImage.CorbisId + "', null, 1, '" + ParentPage.ToString() + "');return false;");
                }
                else
                {
                    priceTag.Attributes.Add("onclick", PricingNavigateUrl);
                }

                priceTag.InnerHtml = CheckoutPresenter.GetPricingDisplay(displayImage, Zone, CorbisBasePage.GetEnumDisplayTexts<PriceStatus>(), Resources.Resource.priceNow);
                if (displayImage.IsRfcd)
                {
                    priceTag.Attributes["class"] += " RFCDPriceTag";
                }
                //priceTag.Attributes.Add("onclick", "javascript:CorbisUI.Pricing.ContactUs.OpenRequestForm();");
                
                Decimal ratio = displayImage.AspectRatio;
                System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)e.Item.FindControl((Zone == CartContainerEnum.PriceMultiple) ? "cartThumb2" : "cartThumb");
                System.Web.UI.WebControls.Image imgNoAvailable = (System.Web.UI.WebControls.Image)e.Item.FindControl("imageNotAvailable");
                imgNoAvailable.ImageUrl = displayImage.Url128;
                img.ImageUrl = displayImage.Url128;
                System.Web.UI.WebControls.Label license = (System.Web.UI.WebControls.Label)e.Item.FindControl("licenseDetails");
                license.Text = (string)GetLocalResourceObject("licenseDetails");
                if (ratio > 1)
                {
                    int height = (HEIGHT - (int)Math.Round(HEIGHT / ratio)) >> 1; 
                    img.Style.Add("margin-top", height.ToString() + "px");
                }
                if (Zone == CartContainerEnum.PriceMultiple )
                {
                    img.Style.Add((ratio > 1)? "width":"height", "90px"); 
                }

                System.Web.UI.WebControls.HyperLink imgD = (System.Web.UI.WebControls.HyperLink)e.Item.FindControl("imgDetail");
                if (!displayImage.IsRfcd)
                {
                    imgD.Text = (string)GetLocalResourceObject("imageDetails");
                    imgD.Attributes["onclick"] =
                        string.Format(
							@"EnlargeImagePopUp('../Enlargement/Enlargement.aspx?id={0}&puid={1}&caller=cart');return false;",
                            imageid, productUid);
                }
                else
                {
                    imgD.Text = (string)GetLocalResourceObject("RFCDDetail");
                    imgD.Attributes["onclick"] = string.Format(@"window.location.href = '../imagegroups/imagegroups.aspx?typ={0}&id={1}'", (int)ImageMediaSetType.RFCD, displayImage.CorbisId);
                    System.Web.UI.WebControls.Label imgDetail = (System.Web.UI.WebControls.Label)e.Item.FindControl("licenseDetails");
                    imgDetail.Visible = false;
                }
                if (displayImage.IsImageAvailable)
                {
                    System.Web.UI.WebControls.Image imageControl = (System.Web.UI.WebControls.Image) e.Item.FindControl("imageNotAvailable");
                    imageControl.Visible = false;
                }

                HoverButton btn = (HoverButton)e.Item.FindControl("btnClose");
                btn.OnClientClick = string.Format("modalDeleteAlert(this, \'{0}\');return false;", Zone.ToString());

                HtmlGenericControl pricedByAE =(HtmlGenericControl)(e.Item.FindControl("pricedByAE"));
                pricedByAE.Visible = ((displayImage.EffectivePriceStatus & PriceStatus.PricedByAE) == PriceStatus.PricedByAE);
            }
        }

        protected void closeBtn_Command(object sender, CommandEventArgs e)
        {
            string corbisId = (string) e.CommandArgument;
            //delete this item
            //cartPresenter.
        }

        protected void infoTabBtn_Command(object sender, CommandEventArgs e)
        {
            string corbisId = (string)e.CommandArgument;
            //show info for this item
            //cartPresenter.
        }

        #region IPriceImageLink Members

        private bool _showPricingLink;
        public bool ShowPricingLink
        {
            get { return _showPricingLink; }
            set { _showPricingLink = value; }
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