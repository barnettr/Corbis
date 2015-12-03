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
using Corbis.Web.UI.Pricing.Interfaces;
using Corbis.Web.UI.Presenters.Pricing;
using Corbis.Web.Entities;
using Corbis.Web.Utilities.StateManagement;
using Corbis.WebOrders.Contracts.V1;

using System.Text;

namespace Corbis.Web.UI.Pricing
{
    
    public partial class RMPricing : CorbisBasePage
    {

        #region Private Members
        private PricingPresenter pricingPresenter;
        protected HtmlGenericControl useTypeAttributeDiv;
        #endregion

        #region Page Events

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            ScriptManager manager = (ScriptManager)Master.FindControl("scriptManager");
            manager.EnablePageMethods = true;
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //AnalyticsData["events"] = AnalyticsEvents.PricingRMStart;
            //AnalyticsData["prop4"] = "RM";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "registerSuccessAnalytics", "LogOmnitureEvent('event24');", true);

            pricingHeader.StyleSheet = Stylesheets.PricingHeader;
            
            // Get the parent page detail.
            if (!String.IsNullOrEmpty(Request.QueryString["ParentPage"]))
            {
                ParentPage = (Corbis.Web.Entities.ParentPage)Enum.Parse(typeof(Corbis.Web.Entities.ParentPage), Request.QueryString["ParentPage"].ToString());
            }
            
            CorbisId = Request.QueryString["CorbisId"];
            GuidHelper.TryParse(Request.QueryString["ProductUid"], out productUid);
            if (!this.IsPostBack)
            {
                // Set up form for first showing
                ViewState.Clear();
                SetSaveOnCloseWarning(false);
                attributeValidationStatuses = new Dictionary<int, bool>();
                pricingPresenter.InitializeRMPricingForm(false);
                // Need to disable saved usages from express checkout 
                // and put the project info into session
                if (ParentPage == ParentPage.ExpressCheckout)
                {
                    savedUsages.Enabled = false;
                    ProjectInformation projectInfo = new ProjectInformation();
                    projectInfo.JobNumber = Request.QueryString[ExpressCheckoutKeys.JobNumber];
                    projectInfo.Licensee = Request.QueryString[ExpressCheckoutKeys.Licensee];
                    projectInfo.Name = Request.QueryString[ExpressCheckoutKeys.ProjectName];
                    projectInfo.PONumber = Request.QueryString[ExpressCheckoutKeys.PoNumber];
                    StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                    StateItem<ProjectInformation> projectState = new StateItem<ProjectInformation>(
                        ExpressCheckoutKeys.Name,
                        ExpressCheckoutKeys.ProjectInformation,
                        projectInfo,
                        StateItemStore.AspSession);
                    stateItems.SetStateItem<ProjectInformation>(projectState);
                }
            }
            else
            {
                attributeValidationStatuses = (Dictionary<int, bool>)ViewState["attributeValidationStatuses"];
                
                // Huge hack since we can't get a glass button in a Corbis ModalPopup to
                // do a proper postback
                bool customPriceExpired = false;
                if (ViewState["CustomPriceExpired"] != null)
                {
                    customPriceExpired = (bool)ViewState["CustomPriceExpired"];
                    UsageType = RMUsageType.Existing;
                    ViewState["CustomPriceExpired"] = null;
                }
                if (customPriceExpired)
                {
                    pricingPresenter.InitializeRMPricingForm(true);
                }
                else
                {
                    GuidHelper.TryParse(useCategoriesDropDown.SelectedValue, out selectedUseCategoryUid);
                    GuidHelper.TryParse(useTypesDropDown.SelectedValue, out selectedUseTypeUid);
                    GuidHelper.TryParse(savedUsages.SelectedValue, out savedUsageUid);
                }
            }
            // For express checkout, we're hacking this page to only create a new saved usage.  
            // This needs to be outside of the IsPostback check
            if (ParentPage == ParentPage.ExpressCheckout)
            {
                CloseButton.OnClientClick = "parent.CorbisUI.ExpressCheckout.closeCreateNewUsageAndShowExpressCheckout();return false;";
            }
            initSaveFavoriteUsageTip();
        }

        private void initSaveFavoriteUsageTip()
        {
            string rel = string.Empty;
            string details = string.Format(@"
                                    <div class='deEmphasizeNoUsage'>{1}</div>
                                    ", GetLocalResourceObject("SaveFavoriteUseToolTipHeader").ToString(), GetLocalResourceObject("SaveFavoriteUseToolTip").ToString());
//            string details = string.Format(

//                @"<div class='rm-parent-block'>{0}</div>", GetLocalResourceObject("SaveFavoriteUseToolTip").ToString());
            rel = HttpUtility.HtmlEncode(details);
            SaveFavoriteUsageButton.Attributes["rel"] = rel;
            SaveFavoriteUsageButton.Attributes["title"] = GetLocalResourceObject("SaveFavoriteUseToolTipHeader").ToString();
            //this.thumbWrap.Attributes["rel"] = rel;
        }

        protected override void OnInit(EventArgs e)
        {
            pricingPresenter = new PricingPresenter(this);
            HtmlHelper.CreateStylesheetControl(this, Stylesheets.RmPricing, "RMPriceImageCSS");
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Tooltips, "TooltipsCSS");

            this.pricingHeader.cartButton.Click += new EventHandler(addToCart_Click);
            this.pricingHeader.AddToLightBoxButton.AddToNewLightboxHandler += new EventHandler(addToLightboxPopup_AddToNewLightboxHandler);
            this.pricingHeader.AddToLightBoxButton.AddToLightboxHandler += new EventHandler(addToLightboxPopup_AddToLightboxHandler);
            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            ViewState["attributeValidationStatuses"] = attributeValidationStatuses;

            if (!PricedByAE)
            {
                if (ShowLicenseAlertMessage)
                {
                    this.licenseAlertDiv.Style.Add("display", "");
                }
                else
                {
                    this.licenseAlertDiv.Style.Add("display", "none");
                }
                if (ShowValidationErrorsSummary && ValidationErrorsCount > 0)
                {
                    //this.useTypeAttributesValidationSummary.Style.Add("display", "");
                    this.genericErrorMessageDiv.Visible = true;
                    if (pricingHeader.UpdatingCart)
                    {
                        this.pricingHeader.CartButtonEnabled = false;
                    }
                    if (pricingHeader.UpdatingLightbox)
                    {
                        this.pricingHeader.LightboxButtonEnabled = false;
                    }
                    headerUpdatePanel.Update();
                }
                else
                {
                    //this.useTypeAttributesValidationSummary.Style.Add("display", "none");
                    this.genericErrorMessageDiv.Visible = false;
                }

                this.useTypeAttributesValidationSummary.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PricingResizeScript", "resizeDisplay()", true);
            }
            else
            {
               
                    headerUpdatePanel.Update();
 
            }
           
            base.OnPreRender(e);
        }

        protected void savedUsages_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSaveOnCloseWarning(true);
            GuidHelper.TryParse(savedUsages.SelectedValue, out savedUsageUid);
            pricingPresenter.OnSavedUsageChanged();

            
        }

        protected void useCategoriesDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSaveOnCloseWarning(true);
            attributeValidationStatuses = new Dictionary<int, bool>();
            GuidHelper.TryParse(useCategoriesDropDown.SelectedValue, out selectedUseCategoryUid);
            pricingPresenter.OnRmUseCategoryChanged();
        }

        protected void useTypesDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSaveOnCloseWarning(true);
            GuidHelper.TryParse(useTypesDropDown.SelectedValue, out selectedUseTypeUid);
            pricingPresenter.OnRmUseTypeChanged();
        }


        protected void PriceNowButton_Click(object sender, EventArgs e)
        {
            UsageType = RMUsageType.New;
            PriceNowClicked = true;
            pricingPresenter.CalculateRMPrice();
        }

        //TODO: Move logic to presenter
        protected void SaveFinishedUsage_Click(object sender, EventArgs e)
        {

            // Create update usage 
            CreateUsage(true, true);
            if (!pricingPresenter.SavedUsageNameExists(UsageName))
            {
                try
                {
                    Guid savedUsageUid = pricingPresenter.SaveFavoriteUsage();
                    saveFavoriteUsePopUpLabel.Text = (string)GetLocalResourceObject("favorPopupsaved1.Text") + " <b>'" + Server.HtmlEncode(usageName.Text) + "'</b> " + (string)GetLocalResourceObject("favorPopupsaved2.Text");
                    usageName.Visible = false;
                    SaveButton.Visible = false;
                    CancelButton.Visible = false;
                    CloseButton.Visible = true;
                    favorPopupTitle.Text = (string)GetLocalResourceObject("favorPopupSavedTitle.Text");
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "resizemodel", "ResizeModal('saveFavoriteUsePopUp');", true);
                    favoriteUseValue.Text = Server.HtmlEncode(StringHelper.Truncate(UsageName, 14));
                    favoriteUseValue.ToolTip = UsageName;
                    favoriteUseTitle.Visible = true;
                    if (this.ParentPage == ParentPage.ExpressCheckout && 
                        !savedUsageUid.Equals(Guid.Empty))
                    {
                        //Save the usage into Session to be displayed in the Express checkout dialog
                        StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                        stateItems.SetStateItem<Guid>(
                            new StateItem<Guid>(
                                ExpressCheckoutKeys.Name,
                                ExpressCheckoutKeys.NewSavedUsageUid,
                                savedUsageUid,
                                StateItemStore.AspSession,
                                StatePersistenceDuration.Session));
                    }
                }
                catch
                {
                }

            }
            else
            {
                saveFavoriteUsePopUpLabel.Text = "<b>"+(string)GetLocalResourceObject("errorMsgFavorSave.Text")+"</b>";
                favorPopupTitle.Text = (string)GetLocalResourceObject("favorPopupTitleOops.Text");
            }
            saveFavoriteUsePopupUpdatePanel.Update();
        }

        //TODO: Move logic to presenter
        protected void SaveFavoriteUsage_Click(object sender, EventArgs e)
        {            
            usageName.Visible = true;
            SaveButton.Visible = true;
            CancelButton.Visible = true;
            CloseButton.Visible = false;
            usageName.Text = string.Empty;
            favorPopupTitle.Text = (string)GetLocalResourceObject("SaveFavoriteUse.Text");
            saveFavoriteUsePopUpLabel.Text = "<b>" + (string)GetLocalResourceObject("saveFavoriteUsePopUpLabel.Text") +"</b>";
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "openModal", "OpenModal('saveFavoriteUsePopUp');ResizeModal('saveFavoriteUsePopUp');addBackGroundImage();", true);
            saveFavoriteUsePopupUpdatePanel.Update();
           // this.CancelButton.Attributes.Add("class", "GlassButton btnOrangedbdbdb btnGraydbdbdb");

        }
       
        protected void addToCart_Click(object sender, EventArgs e)
        {
            AnalyticsData["events"] = AnalyticsEvents.CartAddTo;
            
            // Create usage without updating license details panel.
            CreateUsage(false, true);

            // Add/Update cart with the new usage.
            pricingPresenter.UpdateCartProducts(Guid.Empty);
			ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseRFPricingModalJS", PricingPresenter.GetPostAddToCartScript(ParentPage, CorbisId, Profile.CartItemsCount), true);
        }

        protected void startOverLink_Click(object sender, EventArgs e)
        {
            // Need to replace the productUid
            string url = Request.RawUrl.Replace(ProductUid.ToString(), Guid.Empty.ToString());
            Response.Redirect(url);
        }

        protected void useTypeAttributesRepeater_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                useTypeAttributeDiv = (HtmlGenericControl)e.Item.FindControl("useTypeAttributeDiv");
                pricingPresenter.CreateRMAttributeControl(e.Item.ItemIndex);
            }
            this.UsagePricingUpdatePanel.Update();
        }

        protected void useTypeAttributesRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                useTypeAttributeDiv = (HtmlGenericControl)e.Item.FindControl("useTypeAttributeDiv");
                pricingPresenter.BindRMAttributeControl(e.Item.ItemIndex);
            }
        }

        protected void AttributeModified(object sender, EventArgs e)
        {
            SetSaveOnCloseWarning(true);
            WebControl controlModified = (WebControl)sender;
            int.TryParse(controlModified.Attributes["attributeIndex"], out attributeModifiedIndex); 
            pricingPresenter.OnAttributeChanged();
        }
        #endregion

        // TODO: Remove whole region after logic is moved to presenter
        #region Get Control Values
        private ListItem GetDropdownItem(RepeaterItem item)
        {
            Corbis.Web.UI.Controls.DropDownList dd = (Corbis.Web.UI.Controls.DropDownList)item.FindControl("dropdown");
            return dd.SelectedItem;
        }

        private string GetGeographyText(RepeaterItem item)
        {
            Corbis.Web.UI.Controls.GeographySelector geo = (Corbis.Web.UI.Controls.GeographySelector)item.FindControl("geography");
            return String.Join(", ", geo.SelectedItemsText.ToArray());
        }

        private List<Guid> GetGeographyValue(RepeaterItem item)
        {
            Corbis.Web.UI.Controls.GeographySelector geo = (Corbis.Web.UI.Controls.GeographySelector)item.FindControl("geography");
            return geo.SelectedValueUids;
        }

        private string GetCalendarValue(RepeaterItem item)
        {
            string dateText;
            // Need to check if it's custom Priced
            if (CustomPriced)
            {
                dateText = pricedByAEStartDateTextBox.Text;
            }
            else
            {
                TextBox calText = (TextBox)item.FindControl("calendar");
                dateText = calText.Text;
            }
            return dateText;
        }
        #endregion

        #region Validation

        protected void ValidateUseCategory(object source, ServerValidateEventArgs args)
        {
            args.IsValid = pricingPresenter.IsValidRMUseCategory();
        }

        protected void ValidateUseType(object source, ServerValidateEventArgs args)
        {
            args.IsValid = pricingPresenter.IsValidRMUseType();
        }

        protected void ValidateAttribute(object source, ServerValidateEventArgs args)
        {
            currentValidator = (IAttributeValidator)source;
            CustomValidator validator = (CustomValidator)source;
            WebControl controlToValidate = (WebControl)validator.FindControl(validator.ControlToValidate);
            useTypeAttributeDiv = (HtmlGenericControl)validator.FindControl("useTypeAttributeDiv");

            int attributeIndex = -1;
            int.TryParse(controlToValidate.Attributes["attributeIndex"], out attributeIndex);
            args.IsValid = pricingPresenter.ValidateAttribute(controlToValidate.ID, attributeIndex, args.Value);
            setErrorClass(useTypeAttributeDiv, attributeIndex, !args.IsValid);
        }

        private void setErrorClass(HtmlGenericControl div, int attributeIndex, bool isError)
        {
            // Last attribute should not have a separator
            // if UseTypeWithAttributes is null, we're checking the UseCategory or UseType
            if (UseTypeWithAttributes != null && UseTypeWithAttributes.UseTypeAttributes != null)
            {
                // We got a valid use type
                this.useCategoriesDropDownDiv.Attributes["class"] = "UseCategoriesDropDownDiv SelectedAttribute";
                this.useTypesDropDownDiv.Attributes["class"] = "UseTypesDropDownDiv addBottomBorder SelectedAttribute";
                if (attributeIndex == LastAttributeIndex)
                {
                    if (div.Attributes["class"].Contains("ErrorField"))
                    {
                        div.Attributes["class"] = "NewUsageSelectorsNoBorder ErrorField";
                    }
                    else
                    {
                        if (isError)
                        {
                            div.Attributes["class"] = "NewUsageSelectorsNoBorder UnselectedAttribute";
                        }
                        else
                        {
                            div.Attributes["class"] = "NewUsageSelectorsNoBorder SelectedAttribute";
                        }
                    }
                }
            }
            else
            {
                this.useCategoriesDropDownDiv.Attributes["class"] = "UseCategoriesDropDownDiv";
                this.useTypesDropDownDiv.Attributes["class"] = "UseTypesDropDownDiv";
            }

            if (isError)
            {
                if (attributeIndex < 0)
                {
                    this.useCategoriesDropDownDiv.Attributes["class"] = "UseCategoriesDropDownDiv";
                    this.useTypesDropDownDiv.Attributes["class"] = "UseTypesDropDownDiv";
                }
                // Only set the error class if we're loading a saved or existing use
                // OR the attribute being modified is in error
                if (UsageType == RMUsageType.Saved
                 || UsageType == RMUsageType.Existing
                 || attributeIndex == attributeModifiedIndex)
                {
                    div.Attributes["class"] = div.Attributes["class"].Replace("SelectedAttribute", "UnselectedAttribute");
                    div.Attributes["class"] = div.Attributes["class"].Replace(" ErrorField", "") + " ErrorField";
                }
                
            }
            // Not in error, clear the error class and set to selected attribute
            else
            {
                div.Attributes["class"] = div.Attributes["class"].Replace("UnselectedAttribute", "SelectedAttribute");
                div.Attributes["class"] = div.Attributes["class"].Replace(" ErrorField", "");
            }
            if (!attributeValidationStatuses.ContainsKey(attributeIndex))
            {
                attributeValidationStatuses.Add(attributeIndex, !div.Attributes["class"].Contains("ErrorField"));
            }
            else
            {
                attributeValidationStatuses[attributeIndex] = !div.Attributes["class"].Contains("ErrorField");
            }
        }
        
        #endregion

        #region Private Helper Methods
        
        
        /// <summary>
        /// Creates the usage.
        /// </summary>
        /// <param name="updateLicenseDisplay">if set to <c>true</c> [update license display].</param>
        public void CreateUsage(bool updateLicenseDisplay, bool updateUsageInPresenter)
        {
            //TODO: Move logic to presenter. 
            //      The idea is to have the presenter generate the 
            //      CompletedUsage and update the license details during all of the Validation events in 
            //      the presenter, which would get run after the presenter calls the Validate method
            //      on the view.
            List<KeyValuePair<Guid, object>> usageAttributeValuePair = new List<KeyValuePair<Guid, object>>();

            StringBuilder sbAttributeDetails = new StringBuilder();

            for (int i = 0; i < useTypeAttributesRepeater.Items.Count; i++)
            {
                KeyValuePair<Guid, object> attributeValuePair;
                Guid attributeUid = Guid.Empty;

                UseTypeAttribute useTypeAttrib = UseTypeWithAttributes.UseTypeAttributes[i];

                string displayText = useTypeAttrib.DisplayText;
                string questionText = useTypeAttrib.QuestionText;
                string answerText = string.Empty;
                attributeUid = useTypeAttrib.AttributeUid;

                switch (useTypeAttrib.DisplayType)
                {
                    case AttributeDisplay.Calendar:
                        try
                        {
                            answerText = GetCalendarValue(useTypeAttributesRepeater.Items[i]);
                            DateTime tmpStartDate = DateTime.MinValue;
                            DateTime.TryParse(
                                     answerText,
                                     Language.CurrentCulture.DateTimeFormat,
                                     DateTimeStyles.None,
                                     out tmpStartDate);
                            LicenseStartDate = tmpStartDate;
                            
                            attributeValuePair = new KeyValuePair<Guid, object>(attributeUid, LicenseStartDate.Value);
                            usageAttributeValuePair.Add(attributeValuePair);
                        }
                        catch (System.FormatException) { }
                        break;
                    case AttributeDisplay.DropdownList:
                        try
                        {
                            ListItem dropdownList = GetDropdownItem(useTypeAttributesRepeater.Items[i]);
                            if (!string.IsNullOrEmpty(dropdownList.Value))
                            {
                                answerText = dropdownList.Text;
                                attributeValuePair = new KeyValuePair<Guid, object>(attributeUid, new Guid(dropdownList.Value));
                                usageAttributeValuePair.Add(attributeValuePair);
                            }
                        }
                        catch (System.FormatException) { }
                        break;
                    case AttributeDisplay.Geography:
                        answerText = GetGeographyText(useTypeAttributesRepeater.Items[i]);
                        foreach (Guid valueUid in GetGeographyValue(useTypeAttributesRepeater.Items[i]))
                        {
                            attributeValuePair = new KeyValuePair<Guid, object>(attributeUid, valueUid);
                            usageAttributeValuePair.Add(attributeValuePair);
                        }
                        break;
                    case AttributeDisplay.None:
                    default:
                        break;
                }

                if (!string.IsNullOrEmpty(displayText) 
                    && !string.IsNullOrEmpty(answerText) 
                    && updateLicenseDisplay == true
                    && useTypeAttrib.DisplayType != AttributeDisplay.None)
                {
                    if (useTypeAttrib.DisplayType == AttributeDisplay.Calendar)
                    {
                        sbAttributeDetails.Append("<div class=\"QuestionText\">" + displayText + ":</div><div class=\"AnswerText\" id=\"licenseStartDateDetails\">" + answerText + "</div>");
                    }
                    else
                    {
                        sbAttributeDetails.Append("<div class=\"QuestionText\">" + displayText + ":</div><div class=\"AnswerText\">" + answerText + "</div>"); 
                    }
                }
            }
            
            if (updateLicenseDisplay)
            {
                StringBuilder sbLicenseDetails = new StringBuilder();
                sbLicenseDetails.Append(UseTypeHelpText);
                sbLicenseDetails.Append(UseCategoryLicenseDetails);
                sbLicenseDetails.Append(UseTypeLicenseDetails);
                sbLicenseDetails.Append(sbAttributeDetails.ToString());
                AllLicenseDetails = sbLicenseDetails.ToString();
            }

            if (updateUsageInPresenter)
            {
                // Update usage in presenter.
                pricingPresenter.UpdateUsageForRM(usageAttributeValuePair);
            }
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
            // Create usage without updating license details panel.
            CreateUsage(false, true);
            pricingPresenter.UpdateLightboxProducts(lightboxId, Guid.Empty);
			(new StateItemCollection(System.Web.HttpContext.Current)).SetStateItem<string>(new StateItem<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, lightboxId.ToString(), StateItemStore.Cookie));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseRFPricingModalJS", PricingPresenter.GetPostAddToLightboxScript(ParentPage, lightboxId, this.CorbisId, lightboxName), true);
        }

        private void SetSaveOnCloseWarning(bool warn)
        {
            if (warn)
            {
                this.XClose.NavigateUrl = "javascript:OpenCloseWarning('confirmClose','" + XClose.ClientID + "');";
            }
            else
            {
                XClose.NavigateUrl = "javascript:PricingModalPopupExit();";
            }
            if (ParentPage == ParentPage.ExpressCheckout)
            {
                XClose.NavigateUrl = "javascript:parent.CorbisUI.ExpressCheckout.closeCreateNewUsageAndShowExpressCheckout();";
            }
        }

        #endregion

        #region Web Methods
        /// <summary>
        /// Changes the start date for a custom priced single image.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="cultureName">Name of the culture.</param>
        /// <returns></returns>
        [WebMethod(true)]
        public static bool changeStartDate(string date, string cultureName)
        {
            // Get the New License Start Date
            if ("zh-CHS".Equals(cultureName))
            {
                cultureName = "zh-CN";
            }
            CultureInfo cultureInfo = new CultureInfo(cultureName);
            DateTime licenseStartDate = DateTime.MinValue;
            if (!DateTime.TryParse(
                 date,
                 cultureInfo.DateTimeFormat,
                 DateTimeStyles.None,
                 out licenseStartDate))
            {
                return false;
            }

            // Get the Custom Product Uid from Session
            StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
            Guid customProductUid = stateItems.GetStateItemValue<Guid>(
                PricingKeys.Name,
                PricingKeys.CustomPricedProductUid,
                StateItemStore.AspSession);
            if (customProductUid != Guid.Empty)
            {
                return PricingPresenter.UpdateCustomProductLicenseStartDate(
                    customProductUid, 
                    licenseStartDate);
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
