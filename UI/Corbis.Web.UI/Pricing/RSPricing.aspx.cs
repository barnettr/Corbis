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
using Corbis.Web.UI.Presenters.Lightboxes;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Web.UI.Presenters.Pricing;
using Corbis.Web.Authentication;
using Corbis.Web.Entities;
using System.Collections.Generic;
using Corbis.Web.Utilities;
using System.Text;

namespace Corbis.Web.UI.Pricing
{
    public partial class RSPricing : CorbisBasePage, IRSPricing
    {
        #region Private members
        private PricingPresenter pricingPresenter;
        private Dictionary<string, List<RsUseTypeAttributeValue>> rsUseTypeAttributes = null;
        #endregion

        #region Page Events
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get the parent page detail.
            if (!String.IsNullOrEmpty(Request.QueryString["ParentPage"]))
            {
                ParentPage = (Corbis.Web.Entities.ParentPage)Enum.Parse(typeof(Corbis.Web.Entities.ParentPage), Request.QueryString["ParentPage"].ToString());
            }

            StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
            if (!this.IsPostBack)
            {
                // Get the corbis Id from querystring.
                CorbisId = Request.QueryString["CorbisId"];

                // Call presenter to get the RS pricelists and update the view.
                pricingPresenter.GetRSPriceList(
                    (string)GetLocalResourceObject("ContactUs"), 
                    pricingPresenter.Profile.UserName,
                    pricingPresenter.Profile.CountryCode,
                    pricingPresenter.Profile.IsECommerceEnabled);
                stateItems.SaveObjectToState(this);
            }
            else
            {
                // Populate object from session state.
                stateItems.PopulateObjectFromState(this);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            pricingPresenter = new PricingPresenter(this);
            this.pricingHeader.cartButton.Click += new EventHandler(addToCart_Click);
            this.pricingHeader.AddToLightBoxButton.AddToNewLightboxHandler += new EventHandler(addToLightboxPopup_AddToNewLightboxHandler);
            this.pricingHeader.AddToLightBoxButton.AddToLightboxHandler += new EventHandler(addToLightboxPopup_AddToLightboxHandler);
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.RfPricing, "RfPricingCSS");
        }

        /// <summary>
        /// Handles the Click event of the addToCart control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void addToCart_Click(object sender, EventArgs e)
        {
            // Update Attribute value pair list.
            pricingPresenter.AddToCompletedAVPairList(new Guid(this.hidAttributeUID.Value),
                new Guid(this.hidAttributeValueUID.Value), true);

            // Add/Update cart with the new usage.
            pricingPresenter.UpdateCartProducts(new Guid(this.hidUseTypeUID.Value));
			ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseRFPricingModalJS", PricingPresenter.GetPostAddToCartScript(ParentPage, CorbisId, Profile.CartItemsCount), true);
		}
        #endregion

        #region IRSPricing Members
        string corbisId = string.Empty;
        [StateItemDesc("RSPricingPage", "CorbisId", StateItemStore.AspSession, StatePersistenceDuration.Session)]
        public string CorbisId
        {
            get { return corbisId; }
            set { corbisId = value; }
        }

        /// <summary>
        /// Gets or sets the RS use type attributes.
        /// </summary>
        /// <value>The RS use type attributes.</value>
        public Dictionary<string, List<RsUseTypeAttributeValue>> RSUseTypeAttributes
        {
            get
            {
                return rsUseTypeAttributes;
            }
            set
            {
                rsUseTypeAttributes = (Dictionary<string, List<RsUseTypeAttributeValue>>)value;
                if (rsUseTypeAttributes.Values.Count > 0)
                {
                    rpRSPricingLabel.Visible = false;
                    foreach (string key in rsUseTypeAttributes.Keys)
                    {
                        // Add a grid dynamically to the pop up.
                        PricingGrid pricingGrid = (PricingGrid)LoadControl(SiteUrls.PricingGridControl);
                        pricingGrid.RSUseTypeAttributeValue = (List<RsUseTypeAttributeValue>)rsUseTypeAttributes[key];
                        pricingGrid.PricingGridHeader = key;
                        PricegridHolder.Controls.Add(pricingGrid);
                    }
                }
                else
                {
                    rpRSPricingLabel.Visible = true;
                }
            }
        }

        private Guid useCatgoryUid;
        [StateItemDesc("RSPricingPage", "UseCategoryUid", StateItemStore.AspSession, StatePersistenceDuration.Session)]
        public Guid UseCategoryUid
        {
            get { return useCatgoryUid; }
            set
            {
                useCatgoryUid = value;
            }
        }

        public IPricingHeader PricingHeader
        {
            get { return this.pricingHeader; }
        }

        private Corbis.Web.Entities.ParentPage parentPage = Corbis.Web.Entities.ParentPage.Unknown;
        public Corbis.Web.Entities.ParentPage ParentPage
        {
            get { return parentPage; }
            set { parentPage = value; }
        }
        #endregion

        #region AddToLightbox Handlers

        protected void addToLightboxPopup_AddToNewLightboxHandler(object sender, EventArgs e)
        {
            LightboxesPresenter lightBoxPresenter = new LightboxesPresenter(this.pricingHeader.AddToLightBoxButton);
			int newLightboxId = lightBoxPresenter.CreateLightbox(Profile.UserName, this.pricingHeader.AddToLightBoxButton.LightboxName);
            AddLightboxProduct(newLightboxId, this.pricingHeader.AddToLightBoxButton.LightboxName);
        }

        protected void addToLightboxPopup_AddToLightboxHandler(object sender, EventArgs e)
        {
			AddLightboxProduct(this.pricingHeader.AddToLightBoxButton.LightboxId, string.Empty);
        }

        #endregion

        #region private methods

        private void AddLightboxProduct(int lightboxId, string lightboxName)
        {
            pricingPresenter.AddToCompletedAVPairList(new Guid(this.hidAttributeUID.Value),
                new Guid(this.hidAttributeValueUID.Value), true);
            pricingPresenter.UpdateLightboxProducts(lightboxId, new Guid(this.hidUseTypeUID.Value));
			(new StateItemCollection(System.Web.HttpContext.Current)).SetStateItem<string>(new StateItem<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, lightboxId.ToString(), StateItemStore.Cookie));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseRFPricingModalJS", PricingPresenter.GetPostAddToLightboxScript(ParentPage, lightboxId, this.CorbisId, lightboxName), true);
		}

		#endregion
    }
}
