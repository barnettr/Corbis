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
using Corbis.Web.UI.Pricing.Interfaces;
using Corbis.Web.UI.Presenters.Pricing;
using Corbis.Web.UI.Presenters.Lightboxes;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Web.Utilities;
using Corbis.Web.Utilities.StateManagement;

namespace Corbis.Web.UI.Pricing
{
    public partial class PricingHeader : CorbisBaseUserControl, IPricingHeader
    {

        PricingPresenter pricingPresenter;
        public Corbis.Web.UI.Controls.GlassButton cartButton;
        public Corbis.Web.UI.Controls.GlassButton lightboxButton;
        public System.Web.UI.HtmlControls.HtmlGenericControl piPrice;
        public System.Web.UI.HtmlControls.HtmlGenericControl piPriceCode;

        protected void Page_Load(object sender, EventArgs e)
        {
            pricingPresenter = new PricingPresenter(this);

            if (!String.IsNullOrEmpty(Request.QueryString["ParentPage"]))
            {
                ParentPage = (Corbis.Web.Entities.ParentPage)Enum.Parse(typeof(Corbis.Web.Entities.ParentPage), Request.QueryString["ParentPage"].ToString());
            }

            CorbisId = Request.QueryString["CorbisId"];
            pricingPresenter.GetHeaderDetails();
            pricingPresenter.SetLightboxAndCartButtonState();
            if (!IsPostBack)
            {
                piPrice.InnerHtml = CurrencyHelper.GetLocalizedCurrency(piPrice.InnerHtml);
            }

            if (Page.GetType().BaseType == typeof(RFPricingPage))
            {
                this.lightboxButton.OnClientClick = "CorbisUI.Pricing.RF.ShowAddToLightboxModal('" + OfferingUid.ToString() + "');return false;";
            }
            else
            {
                this.lightboxButton.OnClientClick = "ShowAddToLightboxModal('" + OfferingUid.ToString() + "', null, false);return false;";
            }
        }

        public string StyleSheet
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    HtmlHelper.CreateStylesheetControl(this.Page, value, "PricingHeaderCSS");
                }
            }
        }

        #region IPricingHeader Members

        string corbisId = string.Empty;
        [StateItemDesc("PricingHeader", "CorbisId", StateItemStore.AspSession, StatePersistenceDuration.Session)]
        public string CorbisId
        {
            get { return corbisId; }
            set { corbisId = value; }
        }

        public Guid offeringUid = Guid.Empty;
        public Guid OfferingUid
        {
            get { return offeringUid; }
            set { offeringUid = value; }
        }

        public string ImageThumbnail
        {
            set { imageThumbnail.ImageUrl = value.Replace("http://cachens.corbis.com/", "/Common/GetImage.aspx?sz=90&im="); }
        }

        public bool IsThumbPortrait
        {
            set 
            {
                if (value) imageThumbnail.Height = 90;
                else imageThumbnail.Width = 90;
            }
        }

        public Corbis.CommonSchema.Contracts.V1.Category ImageTitle
        {
            set
            {
                imageTitle.Text = CorbisBasePage.GetEnumDisplayText<Corbis.CommonSchema.Contracts.V1.Category>(value); ;
            }
        }
        public string ImageId
        {
            set { imageId.Text = value; }
        }

        public string PricingTier
        {
            set { pricingTier.Text = value; }
        }

        public string LicenseModel
        {
            set { pricingTier.CssClass = value + "_color"; 
                  licenseModal.Text =  GetGlobalResourceObject("Resource",value + "Text") as string;
                  licenseModal.CssClass = value + "_color"; 
                }
        }

        public bool CartButtonEnabled
        {
            get { return this.cartButton.Enabled; }
            set { this.cartButton.Enabled = value; }
        }

        public bool UpdatingCart
        {
            get
            {
                bool updating = false;
                bool.TryParse(hidUpdatingCart.Value, out updating);
                return updating;
            }
            set
            {
                if (value)
                {
                    cartButton.Text = (string) GetLocalResourceObject("updateCart.Text");
                }
                else 
                {
                    cartButton.Text = (string)GetLocalResourceObject("addToCart.Text");
                }
                hidUpdatingCart.Value = value.ToString();
            }
        }


        public bool LightboxButtonEnabled
        {
            get { return this.lightboxButton.Enabled; }
            set { lightboxButton.Enabled = value; }
        }

        public bool UpdatingLightbox
        {
            get
            {
                bool updating = false;
                bool.TryParse(hidUpdatingLightbox.Value, out updating);
                return updating;
            }
            set
            {
                if (value)
                {
                    lightboxButton.Text = (string)GetLocalResourceObject("updateLightbox.Text");
                }
                else
                {
                    lightboxButton.Text = (string)GetLocalResourceObject("addToLightbox.Text");
                }
                hidUpdatingLightbox.Value = value.ToString();
            }
        }


        public string CurrencyCode
        {
            set { piPriceCode.InnerText = value; }
        }

        private Corbis.Web.Entities.ParentPage parentPage = Corbis.Web.Entities.ParentPage.Unknown;
        public Corbis.Web.Entities.ParentPage ParentPage
        {
            get { return parentPage; }
            set { parentPage = value; }
        }

		public Corbis.Web.UI.Lightboxes.AddToLightbox AddToLightBoxButton
        {
            get
            {
                return this.addToLightboxPopup;
            }
        }

        public bool LicenseDetailsVisible
        {
            get { return licenseAlertDiv.Visible; }
            set { licenseAlertDiv.Visible = value; }
        }

        public bool imageContainerVisible
        {
            get { return imageContainer.Visible; }
            set { imageContainer.Visible = value; }
        }

        #endregion
      
    }
}