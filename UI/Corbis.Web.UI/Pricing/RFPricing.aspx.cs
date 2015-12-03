using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.Image.Contracts.V1;
using Corbis.Web.UI.Presenters;
using Corbis.Web.UI.Pricing.Interfaces;
using Corbis.Web.UI.Presenters.Lightboxes;
using Corbis.Web.UI.Presenters.Pricing;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Web.Utilities;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Web.UI.ViewInterfaces;
using System.Globalization;
using Corbis.Framework.Globalization;
using Corbis.Web.Entities;

namespace Corbis.Web.UI.Pricing
{
    public partial class RFPricingPage : CorbisBasePage, IRFPricing
    {

        PricingPresenter pricingPresenter;
            

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.RfPricing, "RfPricingCSS");
            pricingPresenter = new PricingPresenter(this);
            pricingHeader.StyleSheet = Stylesheets.PricingHeaderRF;
            this.pricingHeader.cartButton.Click += new EventHandler(addToCart_Click);
            this.pricingHeader.AddToLightBoxButton.AddToNewLightboxHandler += new EventHandler(addToLightboxPopup_AddToNewLightboxHandler);
            this.pricingHeader.AddToLightBoxButton.AddToLightboxHandler += new EventHandler(addToLightboxPopup_AddToLightboxHandler);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AnalyticsData["events"] = AnalyticsEvents.PricingStart;
            AnalyticsData["prop4"] = "RF";
            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "perContractJsVariable", "var perContractDisplayText = '" + EncodeToJsString((string)GetLocalResourceObject("AsPerContract")) +"';", true);
            if (!String.IsNullOrEmpty(Request.QueryString["arg"]))
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "showDialog", String.Format("window.addEvent('load', function(){0})", " { " + Request.QueryString["arg"] + " }" ), true);
                hidAttributeValueUID.Value = Request.QueryString["hidattributeValue"]; 
            }
            if (!String.IsNullOrEmpty(Request.QueryString["ParentPage"]))
            {
                ParentPage = (Corbis.Web.Entities.ParentPage)Enum.Parse(typeof(Corbis.Web.Entities.ParentPage), Request.QueryString["ParentPage"].ToString());
            }

            CorbisId = Request.QueryString["CorbisId"];
            GuidHelper.TryParse(Request.QueryString["ProductUid"], out productUid);

            StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
            if (!this.IsPostBack)
            {
                pricingHeader.imageContainerVisible = false;
                customPricedDisplay.Visible = false;
                pricingPresenter.InitializeRFPricingForm((string)GetLocalResourceObject("ContactUs"), (string)GetLocalResourceObject("AsPerContract"));
                stateItems.SaveObjectToState(this);
            }
            else
            {
                stateItems.PopulateObjectFromState(this);
            }
        }

        protected void addToCart_Click(object sender, EventArgs e)
        {
            AnalyticsData["events"] = AnalyticsEvents.CartAddTo;
            
            Guid attributeUid = string.IsNullOrEmpty(this.hidAttributeUID.Value) ? Guid.Empty : new Guid(this.hidAttributeUID.Value);
            Guid attributeValueUid = string.IsNullOrEmpty(this.hidAttributeValueUID.Value) ? Guid.Empty : new Guid(this.hidAttributeValueUID.Value);
            if (attributeUid != Guid.Empty && attributeValueUid != Guid.Empty)
            {
                pricingPresenter.AddToCompletedAVPairList(attributeUid,
                    new Guid(this.hidAttributeValueUID.Value), true);
                pricingPresenter.UpdateCartProducts(Guid.Empty);
            }
            else
            {
                pricingPresenter.AddImageToCart();
            }
			ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseRFPricingModalJS", PricingPresenter.GetPostAddToCartScript(ParentPage, CorbisId, Profile.CartItemsCount), true);
        }

        public void RFPricing_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PricedUseTypeAttributeValue attribute = (PricedUseTypeAttributeValue)e.Item.DataItem;
                ((Label)e.Item.FindControl("lblDisplayText")).Text =
                    GetKeyedEnumDisplayText(attribute.ImageSize, "RF");

                Label uncommpressed = (Label)e.Item.FindControl("UncompressedFileSize");
                uncommpressed.Text = string.Format("({0})", uncommpressed.Text);

                Label pixelWidth = (Label)e.Item.FindControl("pixelWidth");
                pixelWidth.Text = string.Format("{0}px ", pixelWidth.Text);

                Label pixelHeight = (Label)e.Item.FindControl("pixelHeight");
                pixelHeight.Text = string.Format(" {0}px x", pixelHeight.Text);

                Label imageWidth = (Label)e.Item.FindControl("imageWidth");
                Label imageHeight = (Label)e.Item.FindControl("imageHeight");

                Label lblPriceText = (Label)e.Item.FindControl("lblPriceText");
                HtmlInputHidden effectivePrice = (HtmlInputHidden)e.Item.FindControl("EffectivePrice");
                HtmlInputHidden EffectivePriceLocalized = (HtmlInputHidden)e.Item.FindControl("EffectivePriceLocalized");                 
                HtmlInputHidden currecyCode = (HtmlInputHidden)e.Item.FindControl("currecyCode");
               
                // till javascript string parse with culture implemented;
                if(effectivePrice!=null && !string.IsNullOrEmpty(effectivePrice.Value))
                {
                    effectivePrice.Value = CurrencyHelper.GetLocalizedCurrencyInvariant(effectivePrice.Value);
                }
                           
                if(currecyCode!=null && !string.IsNullOrEmpty(currecyCode.Value))
                {
                    lblPriceText.Text = string.Format("{0} {1}", CurrencyHelper.GetLocalizedCurrency(effectivePrice.Value), currecyCode.Value);

                }

                if (Profile.CountryCode.Equals("US", StringComparison.InvariantCultureIgnoreCase))
                {
                    imageWidth.Text = string.Format("{0}in @ ", imageWidth.Text);
                    imageHeight.Text = string.Format("{0}in x", imageHeight.Text);
                }
                else
                {
                    imageWidth.Text = string.Format("{0}cm @ ", imageWidth.Text);
                    imageHeight.Text = string.Format("{0}cm x", imageHeight.Text);
                }

                Label resolution = (Label)e.Item.FindControl("resolution");
                resolution.Text = string.Format("{0}ppi", resolution.Text);

                HtmlInputHidden valueUid = (HtmlInputHidden)e.Item.FindControl("ValueUID");             

                HtmlGenericControl rfPricingRepeaterDataRow = (HtmlGenericControl)e.Item.FindControl("RFPricingRepeaterDataRow");
                HtmlGenericControl rfPricingRowLeft = (HtmlGenericControl)e.Item.FindControl("RFPricingRowLeft");
                HtmlGenericControl rfPricingRowRight = (HtmlGenericControl)e.Item.FindControl("RFPricingRowRight");
            }
        }

        #region IRFPricing Members
        string corbisId = string.Empty;
        [StateItemDesc("RFPricingPage", "CorbisId", StateItemStore.AspSession, StatePersistenceDuration.Session)]
        public string CorbisId
        {
            get { return corbisId; }
            set { corbisId = value; }
        }

        public Corbis.LightboxCart.Contracts.V1.RfPriceList PriceList
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                List<PricedUseTypeAttributeValue> sourceList = ((RfPriceList)value).PricedAttributeValues;

                if (sourceList.Count > 0)
                {
                    rpRFPricing.DataSource = sourceList;
                    rpRFPricing.DataBind();

                    rpRFPricingLabel.Visible = false;
                }
                else
                {
                    rpRFPricing.Visible = false;
                }
            }
        }

        private bool pricedByAE = false;
        public bool PricedByAE
        {
            get { return pricedByAE; }
            set
            {
                pricedByAE = value;
                if (value)
                {
                    if ((EffectivePriceStatus & PriceStatus.CustomPriceExpired) == PriceStatus.CustomPriceExpired)
                    {
                        rpRFPricing.Visible = true;
                        customPricedDisplay.Visible = false;

                        mainStatement.Text = string.Format(GetLocalResourceString("mainStatement.Text"),
                          "javascript:parent.CorbisUI.Pricing.ContactUs.OpenRequestForm('" +
                          CorbisId + "', null, 1, '" + ParentPage.ToString() + "');");

                        // Display Cutom Price Expired Model pop up.
                        if (!this.IsPostBack && Request.Browser.Browser == "IE")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "window.setTimeout('CorbisUI.Pricing.RF.OpenCustomPriceExpired(\"customPriceExpired\")', 500);", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "CorbisUI.Pricing.RF.OpenCustomPriceExpired();", true);
                        }
                    }
                    else
                    {
                        // Display Priced by AE atert message & Licensing details on UI. 
                        customPricedDisplay.Visible = true;
                        rpRFPricing.Visible = false;
                        hidAttributeValueUID.Value = string.Empty;
                        lblPricingFooterDisclaimerStrong.Visible = false;
                        
                        if (!ValidLicense)
                        {
                            // Priced by AE and license details are expired.
                            // Show the message label for license expired Priced by AE image.
                            fileSizeWrapper.Visible = false;
                            dimensionSizeWrapper.Visible = false;
                            priceFooter.Visible = false;
                            
                            PricingFooter.Visible = true;
                            pricingHeader.LicenseDetailsVisible = false;
                            pricingHeader.imageContainerVisible = false;
                            btnContactAE.OnClientClick = "javascript:CorbisUI.Pricing.ContactUs.OpenContactCorbis('" +
                                                  CorbisId + "' , null, 1, '" + ParentPage.ToString() + "');return false;";
                            itemNotAvailable.Visible = true;
                        }
                    }
                }
                else
                {
                    // Hide Priced by AE atert message on UI.
                    customPricedDisplay.Visible = false;
                    rpRFPricing.Visible = true;
                    PricingBottom.Visible = false;
                    PricingBottomAE.Visible = true;
                    lblPricingFooterDisclaimerStrong.Visible = true;
                }
            }
        }

        private PriceStatus effectivePriceStatus;
        public PriceStatus EffectivePriceStatus
        {
            get { return effectivePriceStatus; }
            set { effectivePriceStatus = value; }
        }

        private string effectivePriceText;
        public string EffectivePriceText
        {
            get { return effectivePriceText; }
            set 
            {
                if (value == "0.00")
                {
                    AnalyticsData["events"] = AnalyticsEvents.PricingFailure;
                }
                else
                {
                    AnalyticsData["events"] = AnalyticsEvents.PricingFinish;
                }
                effectivePriceText = value;
                string responseScript = string.Format("CorbisUI.Pricing.RF.setPriceLabel('{0}'); ", value);
                // If it's not a postback, register it as a startup script, otherwise
                // just do a client script block, 2 reasons:
                // 1) If it's not a postback, setPriceLabel won't have been loaded yet if we just 
                //    register a ClientScriptBlock
                // 2) If we register a StartupScript on a postback, Safari doesn't pick it up.
                if (!this.IsPostBack)
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "setPrice", responseScript, true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "setPrice", responseScript, true);
                }
            }
        }

        private bool customPriced = false;
        public bool CustomPriced
        {
            get 
            {
                // Lazy Load
                if (customPriced == false)
                {
                    bool sessionValue;
                    StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                    if (stateItems.TryGetStateItemValue<bool>(
                        PricingKeys.Name,
                        PricingKeys.CustomPriced,
                        StateItemStore.AspSession,
                        out sessionValue))
                    {
                        customPriced = sessionValue;
                    }
                }

                return customPriced;
            }
            set 
            { 
                customPriced = value;

                // Save to Session
                StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                StateItem<bool> customPricedStateItem = new StateItem<bool>(
                            PricingKeys.Name,
                            PricingKeys.CustomPriced,
                            customPriced,
                            StateItemStore.AspSession,
                            StatePersistenceDuration.Session);
                if (customPriced)
                {
                    stateItems.SetStateItem<bool>(customPricedStateItem);
                }
                else
                {
                    stateItems.DeleteStateItem<bool>(customPricedStateItem);
                }
            }
        }
        public bool ShowPriceStatusMessageForRF
        {
            set
            {
                if (value)
                {
                    // Set a timeout for IE if it's not a postback to avoid Operation Aborted exception
                    if (!this.IsPostBack && Request.Browser.Browser == "IE")
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "setpriceMessage", "window.setTimeout('OpenModal(\"priceStatusPopup\")', 500);", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "setpriceMessage", "OpenModal('priceStatusPopup');", true);
                    }
                }
            }

        }
        private Guid useCatgoryUid;
        [StateItemDesc("RFPricingPage", "UseCategoryUid", StateItemStore.AspSession, StatePersistenceDuration.Session)]
        public Guid UseCategoryUid
        {
            get { return useCatgoryUid; }
            set
            {
                useCatgoryUid = value;
            }
        }

        private Guid useTypeUid;
        [StateItemDesc("RFPricingPage", "UseTypeUid", StateItemStore.AspSession, StatePersistenceDuration.Session)]
        public Guid UseTypeUid
        {
            get { return useTypeUid; }
            set { useTypeUid = value; }
        }

        private Guid productUid = Guid.Empty;
        public Guid ProductUid
        {
            get { return productUid; }
            set { productUid = value; }
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

        public List<CompletedUsageAttributeValuePair> CompletedUsageAVPairList
        {
            set { throw new Exception("The method or operation is not implemented."); }
        }

        public IImageRestrictionsView RestrictionsControl
        {
            get { return this.ImageRestrictions; }
        }

        private Guid valueGuid = Guid.Empty;
        public Guid AttributeValueUid
        {
            get { return valueGuid; }
            set 
            { 
                valueGuid = value;
                hidAttributeValueUID.Value = valueGuid.ToString();
            }
        }

        private bool validLicense = true;
        public bool ValidLicense
        {
            get { return validLicense; }
            set 
            { 
                validLicense = value;

                // Check for invalid license and Enable Licensing Alert message.
                pricingHeader.LicenseDetailsVisible = !validLicense;
                pricingHeader.imageContainerVisible = !validLicense;
            }
        }

        public string FileSizeText
        {
            set { fileSizeValue.Text = value; }
        }

        public string DimensionText
        {
            set { dimensionsValue.Text = value; }
        }

        public ImageSize ImageFileSize
        {
            set { fileSizeValue.Text = CorbisBasePage.GetKeyedEnumDisplayText(value, "RF") + fileSizeValue.Text; }
        }

        public void AddToCompletedUsageAVPairList(Guid useAttributeUid, object value, bool clearListBeforeAdd)
        {
            pricingPresenter.AddToCompletedAVPairList(useAttributeUid, value, clearListBeforeAdd);
        }

        #endregion

        #region AddToLightbox Handlers

        protected void addToLightboxPopup_AddToNewLightboxHandler(object sender, EventArgs e)
        {
			LightboxesPresenter lightBoxPresenter = new LightboxesPresenter((IView)Page);
            int newLightboxId = lightBoxPresenter.CreateLightbox(Profile.UserName, this.pricingHeader.AddToLightBoxButton.LightboxName);
            AddLightboxProduct(newLightboxId, this.pricingHeader.AddToLightBoxButton.LightboxName);
        }

        protected void addToLightboxPopup_AddToLightboxHandler(object sender, EventArgs e)
        {
			AddLightboxProduct(this.pricingHeader.AddToLightBoxButton.LightboxId, string.Empty);
        }

        #endregion

        #region private methods

        private void AddLightboxProduct(int lightboxId, string lightBoxName)
        {
            Guid attributeUid = string.IsNullOrEmpty(hidAttributeUID.Value) ? Guid.Empty : new Guid(hidAttributeUID.Value);
            if (!string.IsNullOrEmpty(hidAttributeValueUID.Value))
            {
                Guid attrubuteValueUid = new Guid(hidAttributeValueUID.Value);
                pricingPresenter.AddToCompletedAVPairList(attributeUid, attrubuteValueUid, true);
            }
            pricingPresenter.UpdateLightboxProducts(lightboxId, Guid.Empty);
			(new StateItemCollection(System.Web.HttpContext.Current)).SetStateItem<string>(new StateItem<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, lightboxId.ToString(), StateItemStore.Cookie));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseRFPricingModalJS", PricingPresenter.GetPostAddToLightboxScript(ParentPage, lightboxId, CorbisId, lightBoxName), true); ;
        }

		#endregion
        
    }
}
