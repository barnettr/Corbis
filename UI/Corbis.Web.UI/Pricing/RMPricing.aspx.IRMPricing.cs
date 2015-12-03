using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Corbis.Framework.Globalization;
using Contracts = Corbis.Search.Contracts.V1;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Image.Contracts.V1;
using CorbisControls = Corbis.Web.UI.Controls;
using Corbis.Web.UI.Presenters;
using Corbis.Web.UI.Presenters.Lightboxes;
using Corbis.Web.UI.ViewInterfaces;
using System.Web.UI.HtmlControls;
using Corbis.Web.Utilities;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Web.UI.Pricing.Interfaces;
using Corbis.Web.UI.Presenters.Pricing;
using Corbis.Web.Entities;

using System.Text;
using Corbis.CommonSchema.Contracts.V1;

namespace Corbis.Web.UI.Pricing
{

    public partial class RMPricing : IRMPricing
    {
        protected const string validationGroup = "useTypeAttributesValidationGroup";

        #region IRMPricing Members


        private Corbis.Web.Entities.ParentPage parentPage = Corbis.Web.Entities.ParentPage.Unknown;
        public Corbis.Web.Entities.ParentPage ParentPage
        {
            get { return parentPage; }
            set { parentPage = value; }
        }
        
        string corbisId = string.Empty;
        public string CorbisId
        {
            get { return corbisId; }
            set { corbisId = value; }
        }

        private Guid productUid = Guid.Empty;
        public Guid ProductUid
        {
            get { return productUid; }
            set { productUid = value; }
        }

        public bool CustomPriced
        {
            get { return CustomPricedProductUid.HasValue && CustomPricedProductUid.Value != Guid.Empty; }
        }

        private bool priceNowClicked = false;
        public bool PriceNowClicked
        {
            get
            {
                object vsPriceNowClicked = ViewState["PriceNowClicked"];
                if (vsPriceNowClicked != null)
                {
                    priceNowClicked = (bool)vsPriceNowClicked;
                }
                return priceNowClicked;
            }
            set { ViewState["PriceNowClicked"] = value; }

        }

        private Guid? customPricedProductUid = null;
        public Guid? CustomPricedProductUid
        {
            get
            {
                // Lazy Load
                if (!customPricedProductUid.HasValue)
                {
                    Guid sessionValue;
                    StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                    if (stateItems.TryGetStateItemValue<Guid>(
                        PricingKeys.Name,
                        PricingKeys.CustomPricedProductUid,
                        StateItemStore.AspSession,
                        out sessionValue))
                    {
                        customPricedProductUid = sessionValue;
                    }
                }
                return customPricedProductUid;
            }
            set
            {
                customPricedProductUid = value;

                // Save to Session
                StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                StateItem<Guid> customPricedStateItem = new StateItem<Guid>(
                            PricingKeys.Name,
                            PricingKeys.CustomPricedProductUid,
                            customPricedProductUid.HasValue 
                            ? customPricedProductUid.Value
                            : Guid.Empty,
                            StateItemStore.AspSession,
                            StatePersistenceDuration.Session);
                if (customPricedProductUid.HasValue && customPricedProductUid.Value != Guid.Empty)
                {
                    stateItems.SetStateItem<Guid>(customPricedStateItem);
                }
                else
                {
                    stateItems.DeleteStateItem<Guid>(customPricedStateItem);
                }
            }
        }

        public void ValidateUsage()
        {
            Page.Validate(validationGroup);
            this.ErrorSummaryUpdatePanel.Update();
        }

        private Guid savedUsageUid;
        public Guid SavedUsageUid
        {
            get { return savedUsageUid; }
            set
            {
                savedUsageUid = value;
                if (value != Guid.Empty)
                {
                    savedUsages.SelectedValue = value.ToString();
                }
                else if(savedUsages.DataSource != null)
                {
                    savedUsages.SelectedIndex = 0;
                }
                savedUsageUpdatePanel.Update();
            }
        }

        private Guid selectedUseCategoryUid = Guid.Empty;
        public Guid SelectedUseCategoryUid
        {
            get { return selectedUseCategoryUid; }
            set
            {
                selectedUseCategoryUid = value;
                if (useCategoriesDropDown.Items != null)
                {
                    ListItem categoryItem = useCategoriesDropDown.Items.FindByValue(selectedUseCategoryUid.ToString());
                    if (categoryItem != null)
                    {
                        if (useCategoriesDropDown.SelectedItem != null)
                        {
                            useCategoriesDropDown.SelectedItem.Selected = false;
                        }
                        categoryItem.Selected = true;
                    }
                }
            }
        }

        
        List<UseCategory> useCategories;
        public List<UseCategory> UseCategories
        {
            get
            {
                // If the categories are null, try and derive them from the listitems
                if (useCategories == null && useCategoriesDropDown.Items != null)
                {
                    useCategories = new List<UseCategory>();
                    int displayOrder = 0;
                    foreach (ListItem categoryItem in useCategoriesDropDown.Items)
                    {
                        if (!String.IsNullOrEmpty(categoryItem.Value))
                        {
                            UseCategory category = new UseCategory();
                            category.DisplayOrder = displayOrder;
                            category.DisplayText = categoryItem.Text;
                            category.LicenseModel = LicenseModel.RM;
                            category.OfferingType = OfferingType.Stills;
                            category.Uid = new Guid(categoryItem.Value);
                            useCategories.Add(category);
                            displayOrder++;
                        }
                    }
                }
                return useCategories;
            }
            set
            {
                useCategories = value;
                this.useCategoriesDropDown.DataSource = useCategories;
                this.useCategoriesDropDown.DataValueField = "UID";
                this.useCategoriesDropDown.DataTextField = "DisplayText";
                this.useCategoriesDropDown.DataBind();
                InsertDropdownPromptText(useCategoriesDropDown);
                if (selectedUseCategoryUid != Guid.Empty && useCategoriesDropDown.Items != null)
                {
                    ListItem categoryItem = useCategoriesDropDown.Items.FindByValue(selectedUseCategoryUid.ToString());
                    if (categoryItem != null)
                    {
                        if (useCategoriesDropDown.SelectedItem != null)
                        {
                            useCategoriesDropDown.SelectedItem.Selected = false;
                        }
                        categoryItem.Selected = true;
                    }
                }
            }
        }

        private Guid selectedUseTypeUid = Guid.Empty;
        public Guid SelectedUseTypeUid
        {
            get { return selectedUseTypeUid; }
            set
            {
                selectedUseTypeUid = value;
                if (useTypesDropDown.Items != null)
                {
                    ListItem useTypeItem = useTypesDropDown.Items.FindByValue(selectedUseTypeUid.ToString());
                    if (useTypeItem != null)
                    {
                        if (useTypesDropDown.SelectedItem != null)
                        {
                            useTypesDropDown.SelectedItem.Selected = false;
                        }
                        useTypeItem.Selected = true;
                    }
                }
            }
        }

        private List<UseType> useTypes;
        public List<UseType> UseTypes
        {
            get
            {
                // If the UseTypes are null, try and derive them from the listitems
                if (useTypes == null && useTypesDropDown.Items != null)
                {
                    useTypes = new List<UseType>();
                    int displayOrder = 0;
                    foreach (ListItem useTypeItem in useTypesDropDown.Items)
                    {
                        if (!String.IsNullOrEmpty(useTypeItem.Value))
                        {
                            UseType type = new UseType();
                            type.DisplayOrder = displayOrder;
                            type.DisplayText = useTypeItem.Text;
                            type.HelpText = useTypeItem.Text;
                            type.Uid = new Guid(useTypeItem.Value);
                            useTypes.Add(type);
                            displayOrder++;
                        }
                    }
                }
                return useTypes;
            }
            set
            {
                useTypes = value;
                this.useTypesDropDown.DataSource = value;
                this.useTypesDropDown.DataValueField = "UID";
                this.useTypesDropDown.DataTextField = "DisplayText";
                this.useTypesDropDown.DataBind();
                InsertDropdownPromptText(useTypesDropDown);
                if (selectedUseTypeUid != Guid.Empty && useTypesDropDown.Items != null)
                {
                    ListItem useTypeItem = useTypesDropDown.Items.FindByValue(selectedUseTypeUid.ToString());
                    if (useTypeItem != null)
                    {
                        if (useTypesDropDown.SelectedItem != null)
                        {
                            useTypesDropDown.SelectedItem.Selected = false;
                        }
                        useTypeItem.Selected = true;
                    }
                }
            }
        }

        private List<KeyValuePair<System.Guid, string>> savedUsagesFullText = null;
        public List<KeyValuePair<System.Guid, string>> SavedUsages
        {
            set
            {
                savedUsagesFullText = value;
                if (savedUsagesFullText != null && savedUsagesFullText.Count > 0)
                {
                    Dictionary<System.Guid, string> savedUsagesTruncated = new Dictionary<Guid,string>(savedUsagesFullText.Count);

                    savedUsagesFullText.ForEach(
                        delegate(KeyValuePair<System.Guid, string> favUse)
                        {
                            savedUsagesTruncated.Add(favUse.Key, StringHelper.Truncate(favUse.Value, 30));
                        });

                    savedUsages.Enabled = true;
                    savedUsages.DataSource = savedUsagesTruncated;
                    savedUsages.DataValueField = "Key";
                    savedUsages.DataTextField = "Value";
                    savedUsages.DataBind();
                    InsertDropdownPromptText(savedUsages);
                    favoriteUseContainer.Attributes["class"] = "PD_10";
                }
                else
                {
                    savedUsages.Enabled = false;
                    favoriteUseContainer.Attributes["class"] = "PD_10 disabled";
                }
                this.savedUsageUpdatePanel.Update();
            }
            get
            {
                if (savedUsagesFullText == null)
                {
                    pricingPresenter.GetAllSavedUsages();
                }
                return savedUsagesFullText;
            }
        }
        public string SelectedSavedUsageText
        {
            get
            {
                // Refresh list if it hasn't already been set
                if (savedUsagesFullText == null)
                {
                    pricingPresenter.GetAllSavedUsages();
                }
                string savedUsageText = String.Empty;
                if (savedUsages.SelectedItem != null)
                {
                    Guid selectedUid = Guid.Empty;
                    if (GuidHelper.TryParse(savedUsages.SelectedValue, out selectedUid))
                    {
                        KeyValuePair<Guid, string> selectedFavUse = savedUsagesFullText.Find(
                            delegate(KeyValuePair<Guid, string> favUse)
                            {
                                return favUse.Key == selectedUid;
                            });
                        if (!selectedFavUse.Equals(null))
                        {
                            savedUsageText = selectedFavUse.Value;
                        }
                    }
                }
                return savedUsageText;
            }
        }

        private int lastAttributeIndex = -1;
        public int LastAttributeIndex
        {
            get { return lastAttributeIndex; }
            set { lastAttributeIndex = value; }
        }

        /// <summary>
        /// Index of the attribute that was modified
        /// </summary>
        private int attributeModifiedIndex = -1;
        public int AttributeModifiedIndex
        {
            get { return attributeModifiedIndex; }
            set { attributeModifiedIndex = value; }
        }


        private UseType useTypeWithAttributes;
        public UseType UseTypeWithAttributes
        {
            get
            {
                // Lazy Load
                if (useTypeWithAttributes == null)
                {
                    StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                    useTypeWithAttributes = stateItems.GetStateItemValue<UseType>(
						PricingKeys.Name,
						PricingKeys.RmUseTypeKey,
                        StateItemStore.AspSession);
                }
                return useTypeWithAttributes;
            }
            set
            {
                useTypeWithAttributes = value;

                // Save to Session
                StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                StateItem<UseType> useTypeStateItem = new StateItem<UseType>(
							PricingKeys.Name,
							PricingKeys.RmUseTypeKey,
                            useTypeWithAttributes,
                            StateItemStore.AspSession,
                            StatePersistenceDuration.Session);
                if (useTypeWithAttributes != null)
                {
                    stateItems.SetStateItem<UseType>(useTypeStateItem);
                }
                else
                {
                    stateItems.DeleteStateItem<UseType>(useTypeStateItem);
                }

                if (useTypeWithAttributes != null && useTypeWithAttributes.UseTypeAttributes != null)
                {
                    List<Guid> attributeUids = new List<Guid>();
                    foreach (UseTypeAttribute att in useTypeWithAttributes.UseTypeAttributes)
                    {
                        attributeUids.Add(att.AttributeUid);
                    }
                    this.useTypeAttributesRepeater.DataSource = attributeUids;
                }
                else
                {
                    this.useTypeAttributesRepeater.DataSource = null;
                }
                this.useTypeAttributesRepeater.DataBind();
                this.useCategoryAndTypeUpdatePanel.Update();
                this.ErrorSummaryUpdatePanel.Update();
            }
        }

        string productEffectivePrice = string.Empty;
        public string ProductEffectivePrice
        {
            get
            {
                return productEffectivePrice;
            }
            set
            {
                productEffectivePrice = value;
            }
        }

        public string UsageName
        {
            set { this.usageName.Text = value; }
            get { return this.usageName.Text; }

        }

        public IPricingHeader PricingHeader
        {
            get { return this.pricingHeader; }
        }

        public String EffectivePriceZeroText
        {
            get { return "0.00"; }
        }

        public string EffectivePriceText
        {
            get { return pricingHeader.piPrice.InnerText; }
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
                EffectivePriceLocalized.Value = CurrencyHelper.GetLocalizedCurrency(value);
                string responseScript = string.Format("setRMPriceLabel('{0}','{1}');", value,EffectivePriceLocalized.Value);
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

        public string EffectivePriceCurrencyText
        {
            get { return pricingHeader.piPriceCode.InnerText; }
            set
            {
                pricingHeader.piPriceCode.InnerText = value;
                headerUpdatePanel.Update();
            }
        }

        private PriceStatus effectivePriceStatus;
        public PriceStatus EffectivePriceStatus
        {
            get { return effectivePriceStatus; }
            set { effectivePriceStatus = value; }
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
                    regularContent.Style.Add("display", "none");
                    pricedByAEContent.Style.Add("display", "");
                    // If this is a single product, NOT a saved usage, we update the License Start Date
                    // in the DB when they Save the new License Start Date
                    if (UsageType == RMUsageType.Existing)
                    {
                        pricedByAEstartDateSaveButton.OnClientClick = "updateResult(true, true); return false;";
                    }
                    else
                    {
                        pricedByAEstartDateSaveButton.OnClientClick = "updateResult(true, false); return false;";
                    }
                }
                else
                {
                    regularContent.Style.Add("display", "");
                    pricedByAEContent.Style.Add("display", "none");
                }
                AllUsageUpdatePanel.Update();
                PricedByAEUpdatePanel.Update();
            }
        }

        public string PricedByAEParagraph1ResourceKey
        {
            set { this.pricedByAEParagraph1.Text = GetLocalResourceString(value); }
        }

        public string PricedByAeCalendarFormat
        {
            get { return pricedByAECalendarExtender.Format; }
            set
            {
                pricedByAECalendarExtender.Format = value;
                hdnCultureName.Value = Language.CurrentLanguage.LanguageCode;
                PricedByAEUpdatePanel.Update();
            }
        }

        public bool ShowCustomPriceExpiredMessage
        {
            set
            {
                if (value)
                {
                    AnalyticsData["events"] = AnalyticsEvents.PricingExpired;
                    
                    // Set a timeout for IE if it's not a postback to avoid Operation Aborted exception
                    if (!this.IsPostBack && Request.Browser.Browser == "IE")
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "setpriceMessage", "window.setTimeout('openPriceStatusMessage(\"customPriceExpiredPopup\")', 500);", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "setpriceMessage", "openPriceStatusMessage('customPriceExpiredPopup');", true);
                    }
                    ViewState["CustomPriceExpired"] = true;
                }
            }
        }

        public string CustomPriceExpiredMessageResourceKey
        {
            set
            {
                if (value != null)
                {
                    customPriceExpiredPopup.Message = string.Format(GetLocalResourceString(value),
                          "javascript:parent.CorbisUI.Pricing.ContactUs.OpenRequestForm('" +
                          CorbisId + "', null, 1, '" + ParentPage.ToString() + "');"); 
                    PricedByAEUpdatePanel.Update();
                }
            }
        }

        public string EffectivePriceStatusText
        {
            get { return GetLocalResourceString(effectivePriceStatus.ToString()); }
        }

        public bool ShowPriceStatusMessage
        {
            set
            {
                if (value)
                {
                    // Set a timeout for IE if it's not a postback to avoid Operation Aborted exception
                    if (!this.IsPostBack && Request.Browser.Browser == "IE")
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "setpriceMessage", "window.setTimeout('openPriceStatusMessage(\"priceStatusPopup\")', 500);", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "setpriceMessage", "openPriceStatusMessage('priceStatusPopup');", true);
                    }
                }
            }

        }

        private RMUsageType usageType = RMUsageType.Unknown;
        public RMUsageType UsageType
        {
            get { return usageType; }
            set { usageType = value; }
        }

        public IImageRestrictionsView RestrictionsControl
        {
            get { return this.ImageRestrictions; }
        }

        public bool ShowOr
        {
            set 
            {
                or.Visible = value;
                dottedLine.Visible = value;
                savedUsageUpdatePanel.Update();
            }
            get { return or.Visible; }
        }

        public bool ShowSavedUsageBottomBorder
        {
            set
            {
                selectSavedUsageDiv.Attributes["class"] =
                    selectSavedUsageDiv.Attributes["class"].Replace(" addBottomBorder", "");
                if (value)
                {
                    selectSavedUsageDiv.Attributes["class"] =
                        selectSavedUsageDiv.Attributes["class"] + " addBottomBorder";
                }
                savedUsageUpdatePanel.Update();
            }
        }

        public bool ShowUseCatTypeBottomBorders
        {
            set
            {
                useCategoriesDropDownDiv.Attributes["class"] =
                    useCategoriesDropDownDiv.Attributes["class"].Replace(" addBottomBorder", "");
                useTypesDropDownDiv.Attributes["class"] =
                    useTypesDropDownDiv.Attributes["class"].Replace(" addBottomBorder", "");
                if (value)
                {
                    useCategoriesDropDownDiv.Attributes["class"] =
                        useCategoriesDropDownDiv.Attributes["class"] + " addBottomBorder";
                    useTypesDropDownDiv.Attributes["class"] =
                        useTypesDropDownDiv.Attributes["class"] + " addBottomBorder";
                }
                useCategoryAndTypeUpdatePanel.Update();
            }
        }

        public bool ShowStartFromScratch
        {
            set
            {
                startNewDiv.Visible = value;
                //Fix for part of the Bug-15014
                this.createAnewUseLabel.Visible = value;                
                this.questionsLabel.Visible = value;
                if (value)
                {
                    this.startNewDiv.Style.Add("display", "");
                    this.createAnewUseLabel.Attributes.CssStyle.Add("class", "CollapseContainer");
                    this.createAnewUseLabel.CssClass = this.createAnewUseLabel.CssClass.Replace(" CollapseContainer", "");
                    this.questionsLabel.CssClass = this.questionsLabel.CssClass.Replace(" CollapseContainer", "");
                }
                else
                {
                    this.startNewDiv.Attributes.CssStyle.Add("class", "CollapseContainer"); 
                    //this.startNewUpdatePanelAttributes.CssStyle.Add("class", "CollapseContainer");
                    this.startNewDiv.Style.Add("display", "none");
                    this.createAnewUseLabel.CssClass = this.createAnewUseLabel.CssClass + " CollapseContainer";
                    this.questionsLabel.CssClass = this.questionsLabel.CssClass + " CollapseContainer";
                }
                startNewUpdatePanel.Update();
            }
        }
        public bool ShowStartOver
        {
            set
            {
                if (value)
                {
                    startOverLink.Style.Add("display", "");
                    startOverLinkImage.Style.Add("display", "");
                }

                else
                {
                    startOverLink.Style.Add("display", "none");
                    startOverLinkImage.Style.Add("display", "none");
                }
                startOverUpdatePanel.Update();
            }
        }

        public bool PriceNowButtonEnabled
        {
            get { return PriceNowButton.Enabled; }
            set
            {
                PriceNowButton.Enabled = value;
                if (value)
                {
                    ShowValidationErrorsSummary = true;
                    this.PriceNowButton.ToolTip = "";
                }
                else
                {
                    this.PriceNowButton.ToolTip = GetLocalResourceString("PriceNowButton.ToolTip");
                }
                UsagePricingUpdatePanel.Update();
                if (ParentPage == ParentPage.ExpressCheckout)
                {
                    PriceNowButton.Enabled = false;
                }
            }
        }

        public bool PriceNowButtonVisible
        {
            get { return PriceNowButton.Visible; }
            set
            {
                PriceNowButton.Visible = value;
                if (value)
                {
                    PriceNowButton.Style.Add("display", "");
                }
                else
                {
                    PriceNowButton.Style.Add("display", "none");
                }
                UsagePricingUpdatePanel.Update();
            }
        }

        public bool SaveFavoriteUsageButtonEnabled
        {
            get { return SaveFavoriteUsageButton.Enabled; }
            set
            {
                SaveFavoriteUsageButton.Enabled = value;
                UsagePricingUpdatePanel.Update();
            }
        }

        public bool SaveFavoriteUsageButtonVisible
        {
            get { return SaveFavoriteUsageButton.Visible; }
            set
            {
                SaveFavoriteUsageButton.Visible = value;
                if (value)
                {
                    SaveFavoriteUsageButton.Style.Add("display", "");
                }
                else
                {
                    SaveFavoriteUsageButton.Style.Add("display", "none");
                }
                UsagePricingUpdatePanel.Update();
            }
        }


        public bool ShowSelectUseCategoryQuestion
        {
            get { return selectUseCategory.Visible; }
            set
            {
                selectUseCategory.Visible = value;
                useCategoryAndTypeUpdatePanel.Update();
            }
        }

        public bool ShowSelectUseTypeQuestion
        {
            get { return selectUseType.Visible; }
            set
            {
                selectUseType.Visible = value;
                useCategoryAndTypeUpdatePanel.Update();
            }
        }

        public bool ShowFavoriteUseTitle
        {
            get { return favoriteUseTitle.Visible; }
            set
            {
                favoriteUseTitle.Visible = value;
                licenseDetailsUpdatePanel.Update();
            }
        }

        public bool ShowFavoriteUseValue
        {
            get { return favoriteUseValue.Visible; }
            set
            {
                favoriteUseValue.Visible = value;
                licenseDetailsUpdatePanel.Update();
            }
        }

        public string FavoriteUseValue
        {
            get { return favoriteUseValue.Text; }
            set
            {
                favoriteUseValue.Text = Server.HtmlEncode(value);
                licenseDetailsUpdatePanel.Update();
            }
        }

        public string FavoriteUseTooltip
        {
            get { return favoriteUseValue.ToolTip; }
            set
            {
                favoriteUseValue.ToolTip = value;
                licenseDetailsUpdatePanel.Update();
            }
        }

        public bool ShowFavoriteUseInstructions
        {
            get { return favoriteUseInstructionsLabel.Visible; }
            set
            {
                selectSavedUsageDiv.Visible = value;
                this.selectFavoriteUseLabel.Visible = value;
                this.favoriteUseInstructionsLabel.Visible = value;
                this.savedUsages.Visible = value;
                if (value)
                {
                    this.selectFavoriteUseLabel.Attributes.CssStyle.Add("class", "CollapseContainer");
                    this.selectFavoriteUseLabel.CssClass = this.selectFavoriteUseLabel.CssClass.Replace(" CollapseContainer", "");
                    this.favoriteUseInstructionsLabel.CssClass = this.favoriteUseInstructionsLabel.CssClass.Replace(" CollapseContainer", "");
                }
                else
                {
                    this.selectFavoriteUseLabel.CssClass = this.selectFavoriteUseLabel.CssClass + " CollapseContainer";
                    this.favoriteUseInstructionsLabel.CssClass = this.favoriteUseInstructionsLabel.CssClass + " CollapseContainer";
                }
                savedUsageUpdatePanel.Update();
            }
        }

        private bool showLicenseAlertMessage = false;
        public bool ShowLicenseAlertMessage
        {
            get { return showLicenseAlertMessage; }
            set
            {
                showLicenseAlertMessage = value;
                if (value)
                {
                    licenseAlertDiv.InnerHtml = "<ul><li>" + GetLocalResourceString("LicenseAlertMessage") + "</li></ul>";
                }
                else
                {
                    licenseAlertDiv.InnerHtml = null;
                }
                ErrorSummaryUpdatePanel.Update();
                //this.createANewUseDiv.Visible = true;
            }
        }

        /// <summary>
        /// Keeps a running tally of which Attributes are valid or not between calls
        /// as not all attributes are validated on every call and attributes may also
        /// be validated more than once on a call
        /// </summary>
        Dictionary<int, bool> attributeValidationStatuses;
        public int ValidationErrorsCount
        {
            get
            {
                int count = 0;
                foreach (int key in attributeValidationStatuses.Keys)
                {
                    if (!attributeValidationStatuses[key])
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public bool ShowValidationErrorsSummary
        {
            get { return this.useTypeAttributesValidationSummary.Visible; }
            set
            {
                this.useTypeAttributesValidationSummary.Visible = value;
            }
        }

        public bool ShowUseCategoryErrors
        {
            get { return this.useCategoriesValidator.Visible; }
            set
            {
                if (!attributeValidationStatuses.ContainsKey(-2))
                {
                    attributeValidationStatuses.Add(-2, !value);
                }
                else 
                {
                    attributeValidationStatuses[-2] = !value;
                }
                this.useCategoriesValidator.IsValid = !value;
                this.useCategoriesValidator.Visible = value;
                setErrorClass(useCategoriesDropDownDiv, -2, value);
                useCategoryAndTypeUpdatePanel.Update();
                this.ErrorSummaryUpdatePanel.Update();
            }
        }

        public string UseCategoryErrorsSummaryResourceKey
        {
            get { return this.useCategoriesValidator.ErrorMessage; }
            set
            {
                this.useCategoriesValidator.ErrorMessage = GetLocalResourceString(value);
                useCategoryAndTypeUpdatePanel.Update();
                this.ErrorSummaryUpdatePanel.Update();
            }
        }

        public string UseCategoryErrorResourceKey
        {
            get { return this.useCategoriesValidator.Text; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    this.useCategoriesValidator.Text = GetLocalResourceString(value);
                    this.useCategoriesValidator.Display = ValidatorDisplay.Dynamic;
                }
                else
                {
                    this.useCategoriesValidator.Display = ValidatorDisplay.None;
                }
                useCategoryAndTypeUpdatePanel.Update();
                this.ErrorSummaryUpdatePanel.Update();
            }
        }

        public bool ShowUseTypeErrors
        {
            get { return this.useTypesDropDownValidator.Visible; }
            set
            {
                if (!attributeValidationStatuses.ContainsKey(-1))
                {
                    attributeValidationStatuses.Add(-1, !value);
                }
                else
                {
                    attributeValidationStatuses[-1] = !value;
                }
                this.useTypesDropDownValidator.IsValid = !value;
                this.useTypesDropDownValidator.Visible = value;
                setErrorClass(useTypesDropDownDiv, -1, value);
                useCategoryAndTypeUpdatePanel.Update();
                this.ErrorSummaryUpdatePanel.Update();
            }
        }

        public string UseTypeErrorsSummaryResourceKey
        {
            get { return this.useTypesDropDownValidator.ErrorMessage; }
            set
            {
                this.useTypesDropDownValidator.ErrorMessage = GetLocalResourceString(value);
                useCategoryAndTypeUpdatePanel.Update();
                this.ErrorSummaryUpdatePanel.Update();
            }
        }

        public string UseTypeErrorResourceKey
        {
            get { return this.useTypesDropDownValidator.Text; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.useTypesDropDownValidator.Text = GetLocalResourceString(value);
                    this.useTypesDropDownValidator.Display = ValidatorDisplay.Dynamic;
                }
                else
                {
                    this.useTypesDropDownValidator.Display = ValidatorDisplay.None;
                }
                useCategoryAndTypeUpdatePanel.Update();
                this.ErrorSummaryUpdatePanel.Update();
            }
        }

        public bool CreateNewUseDiv
        {
            set
            {
                this.createANewUseDiv.Visible = value;
            }
            get
            {
                return this.createANewUseDiv.Visible;
            }
        }

        #endregion

        #region Create Controls

        private Label CreateQuestion(string questionText)
        {
            Label question = new Label();
            question.ID = "question";
            question.Text = Server.HtmlEncode(questionText);
            return question;
        }

        /// <summary>
        /// Used to hide the div if a UseTypeAttribute has a displaystyle of None
        /// </summary>
        public void CreateBlankControl()
        {
            useTypeAttributeDiv.Visible = false;
        }

        public void CreateDropdownList(int attributeIndex, string questionText)
        {

            useTypeAttributeDiv.Controls.Add(CreateQuestion(questionText));

            HtmlGenericControl div = new HtmlGenericControl("div");
            Corbis.Web.UI.Controls.DropDownList dd = new Corbis.Web.UI.Controls.DropDownList();
            dd.PromptText = GetLocalResourceString("SelectOne");
            dd.ID = "dropdown";
            dd.AutoPostBack = true;
            dd.Attributes.Add("attributeIndex", attributeIndex.ToString());
            dd.SelectedIndexChanged += AttributeModified;

            AttributeValidator val = new AttributeValidator();
            val.ValidateEmptyText = true;
            val.ValidationGroup = validationGroup;
            val.ServerValidate += ValidateAttribute;
            val.ControlToValidate = dd.ID;
            val.DisplayStyle = AttributeValidatorDisplayStyle.None;

            div.Controls.Add(dd);
            div.Controls.Add(val);
            useTypeAttributeDiv.Controls.Add(div);
        }

        public void CreateGeographyControl(int attributeIndex, string questionText)
        {

            useTypeAttributeDiv.Controls.Add(CreateQuestion(questionText));

            HtmlGenericControl div = new HtmlGenericControl("div");
            Corbis.Web.UI.Controls.GeographySelector geo = new Corbis.Web.UI.Controls.GeographySelector();
            geo.ID = "geography";
            geo.UnknownGeographyText = GetLocalResourceString("UnavailableGeographyText");
            geo.AutoPostBack = true;
            geo.Attributes.Add("attributeIndex", attributeIndex.ToString());
            geo.SelectedValueChanged += AttributeModified;
            
            div.Controls.Add(geo);

            AttributeValidator val = new AttributeValidator();
            val.ValidateEmptyText = true;
            val.ValidationGroup = validationGroup;
            val.ServerValidate += ValidateAttribute;
            val.ControlToValidate = geo.ID;
            val.DisplayStyle = AttributeValidatorDisplayStyle.None;
            div.Controls.Add(val);

            useTypeAttributeDiv.Controls.Add(div);
        }

        public void CreateCalendarControl(int attributeIndex, string questionText)
        {

            useTypeAttributeDiv.Controls.Add(CreateQuestion(questionText));

            HtmlGenericControl div = new HtmlGenericControl("div");

            TextBox calText = new TextBox();
            calText.ID = "calendar";
            calText.MaxLength = 10;
            calText.CssClass = "CalendarText";
            calText.AutoPostBack = true;
            calText.Attributes.Add("attributeIndex", attributeIndex.ToString());
            calText.TextChanged += AttributeModified;
            div.Controls.Add(calText);

            ImageButton imgButton = new ImageButton();
            imgButton.ImageUrl = "~/images/calendar.gif";
            imgButton.Attributes["class"] = "ICN-calendar";
            imgButton.ID = "calendarButton";
            div.Controls.Add(imgButton);

            calendarValidator = new AttributeValidator();
            calendarValidator.ValidateEmptyText = true;
            calendarValidator.ValidationGroup = validationGroup;
            calendarValidator.ServerValidate += ValidateAttribute;
            calendarValidator.ControlToValidate = calText.ID;
            calendarValidator.DisplayStyle = AttributeValidatorDisplayStyle.None;
            calendarValidator.Attributes["class"] = "calendarWarningClass";
            div.Controls.Add(calendarValidator);

            AjaxControlToolkit.CalendarExtender calendar = new AjaxControlToolkit.CalendarExtender();
            calendar.OnClientDateSelectionChanged = "function(sender, e) { sender.hide(); }";
            calendar.PopupPosition = AjaxControlToolkit.CalendarPosition.TopLeft;
            calendar.Format = Language.CurrentCulture.DateTimeFormat.ShortDatePattern;
            calendar.TargetControlID = calText.ID;
            calendar.PopupButtonID = "calendarButton";
            div.Controls.Add(calendar);

            useTypeAttributeDiv.Controls.Add(div);
        }
     
        private IAttributeValidator currentValidator;
        public IAttributeValidator CurrentValidator
        {
            get {  return currentValidator; }
        }

        private AttributeValidator calendarValidator;
        public IAttributeValidator CalendarValidator
        {
            get { return calendarValidator; }
        }

        #endregion

        #region Bind Controls

        public void BindCalendarControl(string dateText)
        {
            TextBox calText = (TextBox)useTypeAttributeDiv.FindControl("calendar");
            {
                if (calText != null)
                {
                    calText.Text = dateText;
                }
            }
            pricedByAEStartDateTextBox.Text = dateText;
            pricedByAEStartDateLbl.Text = dateText;
        }

        public void BindDropdownList(List<UseTypeAttributeValue> values, Guid selectedValue)
        {
            Corbis.Web.UI.Controls.DropDownList dd = (Corbis.Web.UI.Controls.DropDownList)useTypeAttributeDiv.FindControl("dropdown");
            dd.DataSource = values;
            dd.DataTextField = "DisplayText";
            dd.DataValueField = "ValueUid";
            dd.DataBind();
            InsertDropdownPromptText(dd);
            dd.SelectedValue = selectedValue == Guid.Empty ? String.Empty : selectedValue.ToString();
        }

        public void BindGeographyControl(List<UseTypeAttributeValue> values, List<Guid> selectedValues)
        {
            Corbis.Web.UI.Controls.GeographySelector geo = (Corbis.Web.UI.Controls.GeographySelector)useTypeAttributeDiv.FindControl("geography");
            geo.DataSource = values;
            geo.SelectedValueUids = selectedValues == null ? new List<Guid>() : selectedValues;
        }

        private void InsertDropdownPromptText(CorbisControls.DropDownList dd)
        {
            if (!StringHelper.IsNullOrTrimEmpty(dd.PromptText))
            {
                if (dd.Items.Count == 0 || dd.Items[0].Text != dd.PromptText)
                {
                    dd.Items.Insert(0, new ListItem(dd.PromptText, String.Empty));
                }
            }
        }

        #endregion

        #region License Details

        public string UseTypeHelpText
        {
            get
            {
                return 
                    (UseTypeWithAttributes != null && 
                     !String.IsNullOrEmpty(UseTypeWithAttributes.HelpText))
                     ? "<div class=\"AnswerText\">" + UseTypeWithAttributes.HelpText + "</div>"
                     : String.Empty;
            }
        }

        public string UseCategoryLicenseDetails
        {
            get
            {
                return 
                    (this.useCategoriesDropDown.SelectedItem != null
                    && !String.IsNullOrEmpty(this.useCategoriesDropDown.SelectedItem.Text))
                    ? "<div class=\"licenseDetailsBlockSeperatorTop QuestionText\">" + GetLocalResourceString("useCategoryLicenseDetails.Text") + "</div><div class=\"AnswerText\">" + this.useCategoriesDropDown.SelectedItem.Text + "</div>"
                    : String.Empty;
            }
        }

        public string UseTypeLicenseDetails
        {
            get
            {
                return
                    (this.useTypesDropDown.SelectedItem != null
                    && !String.IsNullOrEmpty(this.useTypesDropDown.SelectedItem.Text))
                    ? "<div class=\"QuestionText\">" + GetLocalResourceString("useTypeLicenseDetails.Text") + "</div><div class=\"licenseDetailsBlockSeperatorBottom\">" + this.useTypesDropDown.SelectedItem.Text + "</div>"
                    : String.Empty;
            }
        }


        private DateTime? licenseStartDate;
        
        /// <summary>
        /// Null indicates the usage doesn't have a license start date, 
        /// DateTime.MinValue or before DateTime.Today indicates it's not valid
        /// </summary>
        public DateTime? LicenseStartDate
        {
            get { return licenseStartDate; }
            set
            {
                licenseStartDate = value;
                if (licenseStartDate.HasValue)
                {
                    string dateText = licenseStartDate.Value.ToString(Language.CurrentCulture.DateTimeFormat.ShortDatePattern);
                    pricedByAEStartDateLbl.Text = dateText;
                    pricedByAEStartDateTextBox.Text = dateText;
                    pricedByAEstartDateDiv.Style.Add("display", "");
                    //ScriptManager.RegisterStartupScript(Page, this.GetType(), "populateAEStartDateJSVariable", "window.addEvent('load',function(){ pricedByAEDate = '" +dateText+"'});", true);
                    if (licenseStartDate.Value < DateTime.Today)
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "showStartDateWarning", "window.addEvent('load',function(){ toggleEditMode(true, false);setWarningMode(true); checkStartDateOnManualEdit(); });", true);
                    }
                   
                }
                else
                {
                    pricedByAEstartDateDiv.Style.Add("display", "none");
                }
                PricedByAEUpdatePanel.Update();
            }
        }

        private string attributeLicenseDetails = String.Empty;
        public string AttributeLicenseDetails
        {
            get { return attributeLicenseDetails; }
            set { attributeLicenseDetails = value; }
        }

        public string AllLicenseDetails
        {
            get { return licenseDetails.InnerHtml; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    licenseDetails.InnerHtml = value;
                }
                else
                {
                    licenseDetails.InnerHtml = GetLocalResourceString("licenseDetailsEmpty.Text");
                }
                licenseDetailsUpdatePanel.Update();
            }
        }


        #endregion
    }
}
