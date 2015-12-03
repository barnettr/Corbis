using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;
using Corbis.Framework.Globalization;
using Corbis.Web.Authentication;
using Corbis.Web.UI.Pricing.Interfaces;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.LightboxCart.ServiceAgents.V1;
using Corbis.Search.Contracts.V1;
using Corbis.Image.Contracts.V1;
using Corbis.Image.ServiceAgents.V1;
using Corbis.Web.Entities;
using Corbis.Web.Utilities;
using Corbis.CommonSchema.Contracts.V1;

namespace Corbis.Web.UI.Presenters.Pricing
{
    public partial class PricingPresenter
    {
        #region static constructor

        private static Dictionary<Guid, String> AttributeValidationDictionary;
        static PricingPresenter()
        {
            AttributeValidationDictionary = new Dictionary<Guid, string>();
            AttributeValidationDictionary.Add(new Guid("F93192FF-9C92-4E66-B343-5A5E32145757"), "ProvideValid_ImageSize");
            AttributeValidationDictionary.Add(new Guid("2E47F327-C9C6-4782-83F5-1AD3FEDD8363"), "ProvideValid_Placement");
            AttributeValidationDictionary.Add(new Guid("DFB5C824-2E2B-4BBF-A2F5-4265C615A105"), "ProvideValid_CirculationPrintRun");
            AttributeValidationDictionary.Add(new Guid("2D890F9E-FF50-4AF7-9E88-712B35890A50"), "ProvideValid_Geography");
            AttributeValidationDictionary.Add(new Guid("328954BF-30C8-4DF4-982A-B738110A2FE3"), "ProvideValid_Exposure");
            AttributeValidationDictionary.Add(new Guid("8AA832C0-4FA2-49F1-A81B-BE52C8537943"), "ProvideValid_Industry");
            AttributeValidationDictionary.Add(new Guid("49D4720C-9804-4C80-A666-ADCBB2121291"), "ProvideValid_Duration");
            AttributeValidationDictionary.Add(new Guid("FD83D2F4-7B12-45F7-9651-E5A15521E55C"), "ProvideValid_LicenseStartDate");
        }

        #endregion

        #region Form Initialization methods

        public void InitializeRMPricingForm(bool customPriceExpired)
        {
            
            GetAllSavedUsages();
            rmPricingView.SavedUsageUid = Guid.Empty;
            if (!customPriceExpired)
            {
                rmPricingView.CustomPricedProductUid = null;
            }
            rmPricingView.UseTypes = null;
            rmPricingView.UseTypeWithAttributes = null;
            DisplayImage displayImage = GetDisplayImage();
            ImageRestrictionsPresenter restrictionsPresenter =
                new ImageRestrictionsPresenter(rmPricingView.RestrictionsControl, displayImage);
            restrictionsPresenter.SetRestrictions();
            
            rmPricingView.PricedByAE = false;
            rmPricingView.LicenseStartDate = null;
            DisplayRMCurrencyText(Profile.CurrencyCode);
            rmPricingView.ShowStartOver = false;
            rmPricingView.ShowOr = true;
            rmPricingView.ShowSavedUsageBottomBorder = false;
            rmPricingView.ShowUseCatTypeBottomBorders = false;
            rmPricingView.SaveFavoriteUsageButtonEnabled = false;
            rmPricingView.SaveFavoriteUsageButtonVisible = false;
            rmPricingView.PriceNowButtonEnabled = false;
            rmPricingView.PriceNowButtonVisible = false;
            rmPricingView.ShowSelectUseCategoryQuestion = false;
            rmPricingView.ShowSelectUseTypeQuestion = false;
            rmPricingView.ShowFavoriteUseTitle = false;
            //rmPricingView.ToggleValue = false;
            rmPricingView.ShowFavoriteUseValue = false;
            rmPricingView.FavoriteUseValue = String.Empty;

            if (customPriceExpired)
            {
                rmPricingView.ProductUid = rmPricingView.CustomPricedProductUid.Value;
                InitializeRMProductPricing(true);
            }
            else if (rmPricingView.ProductUid != Guid.Empty)
            {
                rmPricingView.UsageType = RMUsageType.Existing;
                InitializeRMProductPricing(customPriceExpired);
            }
            else
            {
                rmPricingView.UsageType = RMUsageType.New;
                rmPricingView.ShowLicenseAlertMessage = false;
                GetUseCategories();
            }
        }

        private void InitializeRMProductPricing(bool customPriceExpired)
        {
            // It's either saved or existing, so hide the or
            rmPricingView.PriceNowClicked = false;
            rmPricingView.ShowOr = false;
            rmPricingView.ShowFavoriteUseInstructions = rmPricingView.UsageType == RMUsageType.Saved;
            rmPricingView.ShowSavedUsageBottomBorder = true;
            rmPricingView.ShowUseCatTypeBottomBorders = true;
            FolderProduct product = GetProductWithUsage(customPriceExpired);

            if (rmPricingView.PricedByAE)
            {
                rmPricingView.CustomPricedProductUid = rmPricingView.ProductUid;
                if (rmPricingView.UsageType == RMUsageType.Existing)
                {
                    rmPricingView.PricedByAEParagraph1ResourceKey = "pricedByAESingleItem.Text";
                }
                else if (rmPricingView.UsageType == RMUsageType.Saved)
                {
                    rmPricingView.PricedByAEParagraph1ResourceKey = "pricedByAEFavoriteUse.Text";
                }
                else
                {
                    rmPricingView.PricedByAEParagraph1ResourceKey = null;
                }
            }

            // If it's a custom priced product and the custom price hasn't expired,
            // Just display the details, no need to go and get all the Usage attributes,
            // just construct the ones we need. We do this because the custom use may have
            // a UseType, Attribute or Attribute value which is no longer valid, but we 
            // honor it anyway.
            if (rmPricingView.PricedByAE &&
               (product.EffectivePriceStatus & PriceStatus.CustomPriceExpired) == 0)
            {
                rmPricingView.PricedByAeCalendarFormat = Language.CurrentCulture.DateTimeFormat.ShortDatePattern;
                // Set up the UseCatagory
                UseCategory customUseCategory = new UseCategory();
                customUseCategory.Uid = product.Usage.UseCategoryUid;
                customUseCategory.LicenseModel = LicenseModel.RM;
                customUseCategory.DisplayText = product.Usage.UseCategoryDescription;
                rmPricingView.UseCategories = 
                    new List<UseCategory>(new UseCategory[] { customUseCategory });

                // Set up the UseType
                UseType customUseType = new UseType();
                customUseType.Uid = product.Usage.UseTypeUid;
                customUseType.DisplayText = product.Usage.UseTypeDescription;
                customUseType.HelpText = product.Usage.UseTypeHelpText;
                rmPricingView.UseTypes = new List<UseType>(new UseType[] { customUseType });
                rmPricingView.SelectedUseTypeUid = customUseType.Uid;

                // Now add the attributes and values
                customUseType.UseTypeAttributes = new List<UseTypeAttribute>();
                foreach (CompletedUsageAttributeValuePair attValPair in product.Usage.AttributeValuePairs)
                {
                    // See if we've already added the attribute, since Geography can have multiple values
                    UseTypeAttribute att = customUseType.UseTypeAttributes.Find(
                        delegate(UseTypeAttribute useTypeAtt)
                        {
                            return useTypeAtt.AttributeUid == attValPair.UseAttributeUid;
                        });
                    if (att == null)
                    {
                        att = new UseTypeAttribute();
                        att.AttributeUid = attValPair.UseAttributeUid;
                        att.DisplayText = attValPair.UseAttributeDescription;
                        att.QuestionText = attValPair.UseAttributeQuestion;
                        att.DisplayOrder = attValPair.DisplayOrder;
                        att.DisplayType = attValPair.DisplayType;
                        customUseType.UseTypeAttributes.Add(att);
                    }
                    if (attValPair.Value is Guid)
                    {
                        UseTypeAttributeValue attVal = new UseTypeAttributeValue();
                        attVal.AttributeUid = att.AttributeUid;
                        attVal.ValueUid = (Guid)attValPair.Value;
                        attVal.DisplayText = attValPair.ValueDescription;
                        if (att.Values == null)
                        {
                            att.Values = new List<UseTypeAttributeValue>();
                        }
                        att.Values.Add(attVal);
                    }
                }
                rmPricingView.UseTypeWithAttributes = customUseType;
                rmPricingView.PricingHeader.CartButtonEnabled = true;
                rmPricingView.PricingHeader.LightboxButtonEnabled = true;
               
            }
            else
            {
                GetUsageAttributes();
                if (rmPricingView.PricedByAE &&
                    ((product.EffectivePriceStatus & PriceStatus.CustomPriceExpired) == PriceStatus.CustomPriceExpired)
                    )
                {
                    rmPricingView.ShowCustomPriceExpiredMessage = true;
                    if (rmPricingView.UsageType == RMUsageType.Existing)
                    {
                        rmPricingView.CustomPriceExpiredMessageResourceKey = "customPriceExpiredSingleItem.Message";
                    }
                    else if (rmPricingView.UsageType == RMUsageType.Saved)
                    {
                        rmPricingView.CustomPriceExpiredMessageResourceKey = "customPriceExpiredFavoriteUse.Message";
                    }
                    else
                    {
                        rmPricingView.CustomPriceExpiredMessageResourceKey = null;
                    }
                    rmPricingView.ShowStartOver = false;
                }
            }
            rmPricingView.ProductEffectivePrice = product.EffectivePrice;
            rmPricingView.CreateUsage(true, true);
            rmPricingView.ShowValidationErrorsSummary = true;
            rmPricingView.ValidateUsage();

            // Per Michelle, if the usage was set by the AE, 
            // we honor it even if it's no longer valid. 
            // The call to GetProductWithUsage() above sets the PricedByAE flag
            if (rmPricingView.IsValid || isValidExceptForLicenseStartDate || rmPricingView.PricedByAE)
            {
                if (!rmPricingView.PricedByAE)
                {
                    rmPricingView.PriceNowButtonEnabled = true;
                    rmPricingView.SaveFavoriteUsageButtonEnabled = true;
                }
            }
            else
            {
                rmPricingView.PriceNowButtonEnabled = false;
                rmPricingView.SaveFavoriteUsageButtonEnabled = false;
                product.EffectivePriceStatus = PriceStatus.UpdateUse;
            }

            // Make sure LicenseAlert Message isn't shown if it's Custom Priced
            if (rmPricingView.PricedByAE)
            {
                rmPricingView.ShowLicenseAlertMessage = false;
                rmPricingView.ShowValidationErrorsSummary = false;
            }
            DisplayRMPricingInfo(product, customPriceExpired);
        }

        public void SetRMExpressCheckoutPricedByAE()
        {
            IRMPricingExpressCheckout expressCheckoutView = view as IRMPricingExpressCheckout;
            if (expressCheckoutView == null)
            {
                throw new ApplicationException("IExpressCheckoutPricedByAE can't be null");
            }
            string corbisId = expressCheckoutView.CorbisId;
            Guid productUid = expressCheckoutView.ProductUid;
            if (corbisId != null && productUid != Guid.Empty)
            {
                FolderProduct product = lightboxCartService.GetProductByProductUid(
                    Profile.UserName,
                    productUid,
                    Profile.CountryCode,
                    Language.CurrentLanguage.LanguageCode,
                    true,
                    true);
                if (product != null)
                {
                    // Need to Set CustomPrice Expired before PricedByAE
                    // See technical bug 16670
                    rfPricingView.EffectivePriceStatus = product.EffectivePriceStatus;
                    expressCheckoutView.EffectivePriceText = 
                        CurrencyHelper.GetLocalizedCurrency(product.EffectivePrice);
                    expressCheckoutView.CustomPricedExpired =
                        (product.EffectivePriceStatus & PriceStatus.CustomPriceExpired) == PriceStatus.CustomPriceExpired;
                    expressCheckoutView.PricedByAE =
                        (product.EffectivePriceStatus & PriceStatus.PricedByAE) == PriceStatus.PricedByAE;
                    // If it's expired, reset the usage on the product so the price gets recaclulated
                    if (expressCheckoutView.PricedByAE && expressCheckoutView.CustomPricedExpired)
                    {
                        lightboxCartService.UpdateLightboxProducts(
                            Profile.UserName,
                            int.Parse(expressCheckoutView.LightboxId),
                            new List<string>(new string[] { corbisId }),
                            product.Usage,
                            Profile.CountryCode,
                            Language.CurrentLanguage.LanguageCode);
                    }
                }
            }
        }

        private FolderProduct GetProductWithUsage(bool customPriceExpired)
        {
            FolderProduct product = lightboxCartService.GetProductByProductUid(
                Profile.UserName,
                rmPricingView.ProductUid,
                Profile.CountryCode,
                Language.CurrentLanguage.LanguageCode,
                true,
                true);
            
            if (product.Usage != null)
            {
                completedUsage = product.Usage;
                rmPricingView.SelectedUseCategoryUid = completedUsage.UseCategoryUid;
                if (customPriceExpired)
                {
                    rmPricingView.EffectivePriceStatus = PriceStatus.UpdateUse;
                    rmPricingView.PricedByAE = false;
                    rmPricingView.CustomPricedProductUid = null;
                    // Need to update the License Start Date if it doesn't already Exist
                    CompletedUsageAttributeValuePair startDateAvp = null;
                    if (completedUsage.AttributeValuePairs != null) 
                    {
                        startDateAvp = completedUsage.AttributeValuePairs.Find(
                            delegate(CompletedUsageAttributeValuePair avp)
                            {
                                return avp.DisplayType == AttributeDisplay.Calendar;
                            });
                    }
                    if (startDateAvp != null 
                        && 
                       (startDateAvp.Value == null 
                        || 
                        (((DateTime)startDateAvp.Value) == DateTime.MinValue)
                        )
                       )
                    {
                        startDateAvp.Value = DateTime.Today;
                    }
                }
                else
                {
                    rmPricingView.EffectivePriceStatus = product.EffectivePriceStatus;
                    rmPricingView.PricedByAE = ((product.EffectivePriceStatus & PriceStatus.PricedByAE) == PriceStatus.PricedByAE);
                    rmPricingView.CustomPricedProductUid =
                        rmPricingView.PricedByAE
                        ? product.ProductUid
                        : Guid.Empty;
                }

                // If the UseCategories haven't been loaded or we're using a SavedUsage or CustomPrice 
                // created by an AE that hasn't expired and the UseCategory isn't in the List, 
                // we need to reload them as it may only be visible internally
                if (rmPricingView.UseCategories == null
                    || rmPricingView.UseCategories.Count == 0
                    || rmPricingView.CustomPriced)
                {
                    UseCategory selectedUseCat = null;
                    if (rmPricingView.UseCategories != null)
                    {
                        selectedUseCat = rmPricingView.UseCategories.Find(
                            delegate(UseCategory useCat)
                            {
                                return useCat.Uid == completedUsage.UseCategoryUid;
                            });
                    }
                    if (selectedUseCat == null)
                    {
                        GetUseCategories();
                    }
                }


                // We don't need to get the usetypes for a custom priced usage that hasn't expired
                if (!rmPricingView.PricedByAE ||
                   (product.EffectivePriceStatus & PriceStatus.CustomPriceExpired) == PriceStatus.CustomPriceExpired)
                {
                    // Load the UseTypes for this product
                    GetUseTypes();
                    rmPricingView.SelectedUseTypeUid = completedUsage.UseTypeUid;
                }
            }
            return product;
        }


        #endregion

        #region Usage Updates and pricing calculation methods
        /// <summary>
        /// Calculates and displays the RM Price
        /// </summary>
        public void CalculateRMPrice()
        {
            // Initialize control states
            PricedImage imageDetail = null;
            rmPricingView.CreateUsage(true, true);
            rmPricingView.ShowValidationErrorsSummary = true;
            rmPricingView.ValidateUsage();
            // Only calculate and display the price if the PriceNow button has been clicked before
            if (rmPricingView.IsValid && rmPricingView.PriceNowClicked)
            {
                List<String> corbisIds = new List<string>(new string[] { rmPricingView.CorbisId });
                List<PricedImage> pricedImageDetails = lightboxCartService.DeterminePrices(
                    Profile.UserName,
                    corbisIds,
                    completedUsage,
                    Profile.CountryCode,
                    Language.CurrentLanguage.LanguageCode);

                if (pricedImageDetails == null || pricedImageDetails.Count == 0)
                {
                    imageDetail = new PricedImage();
                    imageDetail.EffectivePriceStatus = PriceStatus.Unknown;
                }
                else
                {
                    imageDetail = pricedImageDetails[0];
                }

                // Create a product to display the pricing Information
                FolderProduct product = new FolderProduct();
                product.CurrencyCode = imageDetail.CurrencyCode;
                product.PriceCountryCode = imageDetail.PriceCountryCode;
                product.CustomPriceExpirationDate = DateTime.MaxValue;
                product.DateModified = DateTime.Now;
                product.EffectivePrice = imageDetail.EffectivePrice;
                product.EffectivePriceStatus = imageDetail.EffectivePriceStatus;
                product.LicenseStartDate = 
                    rmPricingView.LicenseStartDate.HasValue 
                    ? rmPricingView.LicenseStartDate.Value 
                    : DateTime.MinValue;
                product.OfferingUid = Guid.Empty;
                product.ProductOrdinal = 0;
                product.ProductTemplateDomainId = 1;
                product.ProductUid = Guid.Empty;
                product.Usage = completedUsage;

                DisplayRMPricingInfo(product, false);
            }

            rmPricingView.PriceNowButtonEnabled = rmPricingView.IsValid;
            rmPricingView.SaveFavoriteUsageButtonEnabled = rmPricingView.IsValid;
            if (rmPricingView.IsValid)
            {
                rmPricingView.PricingHeader.CartButtonEnabled = true;
                rmPricingView.PricingHeader.LightboxButtonEnabled = true;
            }
            else
            {
                if (rmPricingView.PricingHeader.UpdatingCart)
                {
                    rmPricingView.PricingHeader.CartButtonEnabled = false;
                }
                else
                {
                    rmPricingView.PricingHeader.CartButtonEnabled = true;
                }
                if (rmPricingView.PricingHeader.UpdatingLightbox)
                {
                    rmPricingView.PricingHeader.LightboxButtonEnabled = false;
                }
                else
                {
                    rmPricingView.PricingHeader.LightboxButtonEnabled = true;
                }
            }
        }

        /// <summary>
        /// Display RM pricing info.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="customPriceExpired">This flag is only to determine whether or not display price for expired custom pricing text. It comes from viewstate.</param>
        private void DisplayRMPricingInfo(FolderProduct product, bool customPriceExpired)
        {
            // Check if the user is not Ecommerce enabled or there is a country/currency discrepancy
            if ((product.EffectivePriceStatus & PriceStatus.NotEcommerceEnabled) == PriceStatus.NotEcommerceEnabled)
            {
                product.EffectivePriceStatus = PriceStatus.NotEcommerceEnabled;
            }
            else if ((product.EffectivePriceStatus & PriceStatus.CountryOrCurrencyError) == PriceStatus.CountryOrCurrencyError)
            {
                product.EffectivePriceStatus = PriceStatus.CountryOrCurrencyError;
            }
            rmPricingView.EffectivePriceStatus = product.EffectivePriceStatus;
            switch (product.EffectivePriceStatus)
            {
                case PriceStatus.ContactUs:
                case PriceStatus.NotEcommerceEnabled:
                case PriceStatus.ContactOutline:
                    rmPricingView.EffectivePriceText = rmPricingView.EffectivePriceStatusText;
                    DisplayRMCurrencyText(String.Empty);
                    rmPricingView.ShowPriceStatusMessage = true;
                    break;
                case PriceStatus.ContactUs | PriceStatus.DisplayPrice:
                    rmPricingView.EffectivePriceText = product.EffectivePrice;
                    DisplayRMCurrencyText(product.CurrencyCode);
                    rmPricingView.ShowPriceStatusMessage = true;
                    break;
                case PriceStatus.AsPerContract:
                    rmPricingView.EffectivePriceText = rmPricingView.EffectivePriceStatusText;
                    DisplayRMCurrencyText(String.Empty);
                    rmPricingView.ShowPriceStatusMessage = false;
                    break;
                case PriceStatus.PricedByAE:
                case PriceStatus.PricedByAE | PriceStatus.DisplayPrice:
                case PriceStatus.PricedByAE | PriceStatus.DisplayPrice | PriceStatus.CustomPriceExpired:
                    if(!customPriceExpired)
                        rmPricingView.EffectivePriceText = product.EffectivePrice;
                    DisplayRMCurrencyText(product.CurrencyCode);
                    rmPricingView.ShowPriceStatusMessage = false;
                    break;
                case PriceStatus.Ok:
                    rmPricingView.EffectivePriceText = product.EffectivePrice;
                    DisplayRMCurrencyText(product.CurrencyCode);
                    rmPricingView.ShowPriceStatusMessage = false;
                    break;
                case PriceStatus.UpdateUse:
                    if (rmPricingView.UsageType == RMUsageType.Existing
                        || rmPricingView.UsageType == RMUsageType.Saved)
                    {
                        if (!customPriceExpired)
                            rmPricingView.EffectivePriceText = rmPricingView.EffectivePriceZeroText;
                        DisplayRMCurrencyText(Profile.CurrencyCode);
                        rmPricingView.PricingHeader.CartButtonEnabled = false;
                    }
                    else
                    {
                        rmPricingView.EffectivePriceStatus = PriceStatus.ContactUs;
                        rmPricingView.EffectivePriceText = rmPricingView.EffectivePriceStatusText;
                        DisplayRMCurrencyText(String.Empty);
                    }
                    rmPricingView.ShowPriceStatusMessage = rmPricingView.UsageType == RMUsageType.New;
                    break;
                case PriceStatus.CountryOrCurrencyError:
                    rmPricingView.EffectivePriceText = rmPricingView.EffectivePriceZeroText;
                    rmPricingView.PricingHeader.CartButtonEnabled = false;
                    DisplayRMCurrencyText(Profile.CurrencyCode);
                    rmPricingView.ShowPriceStatusMessage = rmPricingView.UsageType == RMUsageType.New;
                    break;
                case PriceStatus.Unknown:
                default:
                    if (rmPricingView.UsageType == RMUsageType.Saved)
                    {
                        rmPricingView.EffectivePriceText = rmPricingView.EffectivePriceZeroText;
                        rmPricingView.PricingHeader.CartButtonEnabled = false;
                    }
                    else
                    {
                        rmPricingView.EffectivePriceStatus = PriceStatus.ContactUs;
                        rmPricingView.EffectivePriceText = rmPricingView.EffectivePriceStatusText;
                    }
                    DisplayRMCurrencyText(Profile.CurrencyCode);
                    rmPricingView.ShowPriceStatusMessage = rmPricingView.UsageType == RMUsageType.New;
                    break;
            }
        }

        private void DisplayRMCurrencyText(string currencyCode)
        {
            rmPricingView.EffectivePriceCurrencyText =
                String.IsNullOrEmpty(currencyCode)
                ? String.Empty
                : "(" + currencyCode + ")";
        }

        /// <summary>
        /// Updates the usage for RM.
        /// </summary>
        /// <param name="usageAttributeValuePair">The usage attribute value pair.</param>
        public void UpdateUsageForRM(List<KeyValuePair<Guid, object>> usageAttributeValuePair)
        {

            completedUsage = new CompletedUsage();
            completedUsage.AttributeValuePairs = new List<CompletedUsageAttributeValuePair>();
            completedUsage.UseCategoryUid = rmPricingView.SelectedUseCategoryUid;
            completedUsage.UseTypeUid = rmPricingView.SelectedUseTypeUid;

            foreach (KeyValuePair<Guid, object> keyValuePair in usageAttributeValuePair)
            {
                CompletedUsageAttributeValuePair avPair = new CompletedUsageAttributeValuePair();
                avPair.UseAttributeUid = keyValuePair.Key;
                avPair.Value = keyValuePair.Value;

                completedUsage.AttributeValuePairs.Add(avPair);
            }
        }

        #endregion

        #region SavedUsage methods

        public void GetAllSavedUsages()
        {
            List<KeyValuePair<System.Guid, string>> savedUsages = new List<KeyValuePair<System.Guid, string>>();

            try
            {
                Guid selectedUid = rmPricingView.SavedUsageUid;
                savedUsages = lightboxCartService.GetAllSavedUsagesByUserName(Profile.UserName);
                rmPricingView.SavedUsages = savedUsages;
                rmPricingView.SavedUsageUid = selectedUid;
            }
            catch (Exception e)
            {
                HandleException(e, rmPricingView.LoggingContext, string.Format("PricingPresenter: GetAllSavedUsages() - Error getting  All saved usages."));
                throw;
            }
        }

        /// <summary>
        /// Saves given Usage for current logged in User
        /// </summary>
        public Guid SaveFavoriteUsage()
        {
            Guid savedUsageUid = Guid.Empty;
            savedUsageUid = lightboxCartService.UpdateSavedUsage(Profile.UserName, completedUsage, rmPricingView.UsageName,
                                                 Profile.CountryCode, Language.CurrentLanguage.LanguageCode);
            rmPricingView.ShowFavoriteUseTitle = true;
            rmPricingView.ShowFavoriteUseValue = true;
            rmPricingView.FavoriteUseValue = StringHelper.Truncate(rmPricingView.UsageName , 14);
            rmPricingView.FavoriteUseTooltip = rmPricingView.UsageName;

            return savedUsageUid;

        }

        /// <summary>
        /// Verifies the name of the duplicate.
        /// </summary>
        /// <param name="usageName">Name of the usage.</param>
        /// <returns></returns>
        public bool SavedUsageNameExists(string usageName)
        {
            usageName = usageName.Trim();
            bool isItemFound = false;
            if (rmPricingView.SavedUsages != null)
            {
                isItemFound = rmPricingView.SavedUsages.Exists(new Predicate<KeyValuePair<Guid,string>>(
                    delegate(KeyValuePair<Guid,string> favUse)
                    {
                        return favUse.Value.Equals(usageName, StringComparison.InvariantCultureIgnoreCase);
                    }));
            }

            return isItemFound;
        }


        #endregion

        #region Usage Category/Type/Attribute Methods
 
        public void GetUseCategories()
        {
            
            rmPricingView.UseCategories = lightboxCartService.GetUseCategories(
                Profile.UserName, 
                OfferingType.Stills, 
                LicenseModel.RM, 
                Profile.CountryCode,
                Language.CurrentLanguage.LanguageCode,
                !rmPricingView.PricedByAE);
        }

        public void OnSavedUsageChanged()
        {
            if (rmPricingView.SavedUsageUid != Guid.Empty)
            {
                rmPricingView.UsageType = RMUsageType.Saved;
                rmPricingView.PriceNowClicked = false;
                rmPricingView.ProductUid = rmPricingView.SavedUsageUid;
                rmPricingView.ShowStartFromScratch = false;
                rmPricingView.ShowOr = false;
                rmPricingView.ShowSavedUsageBottomBorder = true;
                InitializeRMProductPricing(false);
            }
        }

  
        public void OnRmUseCategoryChanged()
        {
            rmPricingView.UsageType = RMUsageType.New;
            rmPricingView.ShowLicenseAlertMessage = false;
            rmPricingView.PriceNowClicked = false;
            rmPricingView.ShowValidationErrorsSummary = true;
            rmPricingView.ShowStartFromScratch = true;
            rmPricingView.ShowOr = true;
            rmPricingView.ShowFavoriteUseInstructions = true;
            rmPricingView.ShowFavoriteUseTitle = false;
            rmPricingView.ShowFavoriteUseValue = false;
            if (IsValidRMUseCategory())
            {
                GetUseTypes();
                //Part of Bug-15014 fix
                rmPricingView.ShowSelectUseCategoryQuestion = false;
                rmPricingView.ShowUseCatTypeBottomBorders = false;
            }
            else
            {
                rmPricingView.UseTypes = null;
                rmPricingView.ShowSelectUseCategoryQuestion = false;
                rmPricingView.ShowSelectUseTypeQuestion = false;
            }
            rmPricingView.UseTypeWithAttributes = null;
            rmPricingView.SaveFavoriteUsageButtonEnabled = false;
            rmPricingView.SaveFavoriteUsageButtonVisible = false;
            rmPricingView.PriceNowButtonEnabled = false;
            rmPricingView.PriceNowButtonVisible = false;
            rmPricingView.EffectivePriceStatus = PriceStatus.Unknown;
            rmPricingView.EffectivePriceText = rmPricingView.EffectivePriceZeroText;
            DisplayRMCurrencyText(Profile.CurrencyCode);
            rmPricingView.SavedUsageUid = Guid.Empty;
            rmPricingView.AllLicenseDetails = string.Empty;
        }

        public void OnRmUseTypeChanged()
        {
            rmPricingView.UsageType = RMUsageType.New;
            rmPricingView.ShowLicenseAlertMessage = false;
            rmPricingView.PriceNowClicked = false;
            rmPricingView.ShowValidationErrorsSummary = true;
            rmPricingView.ShowStartOver = true;
            rmPricingView.ShowStartFromScratch = true;
            rmPricingView.ShowOr = false;
            rmPricingView.ShowFavoriteUseInstructions = false;
            rmPricingView.ShowFavoriteUseTitle = false;
            rmPricingView.ShowFavoriteUseValue = false;
            if (IsValidRMUseType())
            {
                GetUsageAttributes();
                rmPricingView.CreateUsage(true, false);
                rmPricingView.ShowSelectUseTypeQuestion = true;
                rmPricingView.ShowUseCatTypeBottomBorders = true;
                // Don't show validation Errors the first time new UseType attribues are loaded                
                rmPricingView.ShowValidationErrorsSummary = false;
                rmPricingView.ValidateUsage();
                rmPricingView.SaveFavoriteUsageButtonEnabled = rmPricingView.IsValid;
                rmPricingView.PriceNowButtonEnabled = rmPricingView.IsValid;
            }
            else
            {
                GetUseTypes();
                rmPricingView.UseTypeWithAttributes = null;
                rmPricingView.ShowSelectUseTypeQuestion = false;
                rmPricingView.AllLicenseDetails = string.Empty;
                rmPricingView.SaveFavoriteUsageButtonVisible = false;
                rmPricingView.SaveFavoriteUsageButtonEnabled = false;
                rmPricingView.PriceNowButtonVisible = false;
                rmPricingView.PriceNowButtonEnabled = false;
            }
            rmPricingView.SavedUsageUid = Guid.Empty;
            rmPricingView.EffectivePriceStatus = PriceStatus.Unknown;
            rmPricingView.EffectivePriceText = rmPricingView.EffectivePriceZeroText;
            DisplayRMCurrencyText(Profile.CurrencyCode);
        }

        public void OnAttributeChanged()
        {
            rmPricingView.UsageType = RMUsageType.New;
            rmPricingView.ShowLicenseAlertMessage = false;
            CalculateRMPrice();
        }

        public void GetUseTypes()
        {
            List<UseType> useTypes = lightboxCartService.GetUseTypes(
                Profile.UserName, 
                rmPricingView.SelectedUseCategoryUid, 
                Profile.CountryCode,
                Language.CurrentLanguage.LanguageCode,
                !rmPricingView.PricedByAE);
            rmPricingView.UseTypes = useTypes;
        }

        /// <summary>
        /// Gets the usage attributes for the selected UseCategory and UseType
        /// </summary>
        private void GetUsageAttributes()
        {
            bool useCategoryFound = IsValidRMUseCategory();
            bool useTypeFound = IsValidRMUseType();
            // Fix for part of the Bug-15014
            if (rmPricingView.UsageType == RMUsageType.Existing || rmPricingView.UsageType == RMUsageType.New)
            {
                rmPricingView.CreateNewUseDiv = true;
            }

            if (useCategoryFound && useTypeFound)
            {
                GetUseTypeAttributes();

                rmPricingView.ShowStartOver = true;
                rmPricingView.ShowSelectUseTypeQuestion = true;
                rmPricingView.ShowSelectUseCategoryQuestion = true;
                rmPricingView.SaveFavoriteUsageButtonVisible = true;
                if (rmPricingView.UsageType == RMUsageType.Saved)
                {
                    rmPricingView.ShowFavoriteUseTitle = true;
                    rmPricingView.ShowFavoriteUseValue = true;
                    rmPricingView.FavoriteUseValue = StringHelper.Truncate(rmPricingView.SelectedSavedUsageText, 12);
                    rmPricingView.FavoriteUseTooltip = rmPricingView.SelectedSavedUsageText;
                }
                rmPricingView.PriceNowButtonVisible = true;
            }
            else
            {
                rmPricingView.UseTypeWithAttributes = null;
            }
        }

        private Guid GetRMAttributeValue(Guid attributeUid)
        {
            Guid attributeValueUid = Guid.Empty;

            if (completedUsage != null && completedUsage.AttributeValuePairs != null)
            {
                foreach (CompletedUsageAttributeValuePair pair in completedUsage.AttributeValuePairs)
                {
                    if (pair.UseAttributeUid == attributeUid)
                    {
                        attributeValueUid = (Guid)pair.Value;
                        break;
                    }
                }
            }

            return attributeValueUid;
        }

        private List<Guid> GetGeographyAttributeValue(Guid attributeUid)
        {
            List<Guid> attributeValues = new List<Guid>();
            
            if (completedUsage != null && completedUsage.AttributeValuePairs != null)
            {
                foreach (CompletedUsageAttributeValuePair pair in completedUsage.AttributeValuePairs)
                {
                    if (pair.UseAttributeUid == attributeUid)
                    {
                        attributeValues.Add((Guid)pair.Value);
                    }
                }
            }

            return attributeValues;

        }

        private String GetLicenseStartDateText(Guid attributeUid)
        {
            DateTime dateToDisplay = DateTime.MinValue;
            switch (rmPricingView.UsageType)
            {
                case RMUsageType.Existing:
                    if (completedUsage != null && completedUsage.AttributeValuePairs != null)
                    {
                        CompletedUsageAttributeValuePair startDateAttValPair =
                            completedUsage.AttributeValuePairs.Find(
                            delegate(CompletedUsageAttributeValuePair attValPair)
                            {
                                return attValPair.UseAttributeUid == attributeUid;
                            });
                        if (startDateAttValPair != null)
                        {
                            try
                            {
                                dateToDisplay = (DateTime)startDateAttValPair.Value;
                            }
                            catch { }
                        }
                    }
                    // If it's a custom priced product without a License Start Date,
                    // set the date to today
                    if (rmPricingView.CustomPriced && dateToDisplay == DateTime.MinValue)
                    {
                        dateToDisplay = DateTime.Today;
                    }
                    break;
                case RMUsageType.Saved:
                case RMUsageType.New:
                    dateToDisplay = DateTime.Today;
                    break;
                case RMUsageType.Unknown:
                default:
                    // return String.Empty
                    break;
            }
            if (dateToDisplay != DateTime.MinValue)
            {
                return dateToDisplay.ToString(Language.CurrentCulture.DateTimeFormat.ShortDatePattern);
            }
            else
            {
                return String.Empty;
            }
        }

        public void GetUseTypeAttributes()
        {
            // Need a try/catch block to make sure the selected UseType is still valid
            try
            {
                UseType useTypeWithAttributes =
                    lightboxCartService.GetUseTypeAttributes(
                    Profile.UserName,
                    rmPricingView.SelectedUseCategoryUid,
                    rmPricingView.SelectedUseTypeUid,
                    Profile.CountryCode,
                    Language.CurrentLanguage.LanguageCode,
                    !rmPricingView.PricedByAE);
                rmPricingView.UseTypeWithAttributes = useTypeWithAttributes;
            }
            catch
            {
                rmPricingView.UseTypeWithAttributes = null;
            }
        }

        #endregion

        #region Control Creation 

        /// <summary>
        /// Calls methods necessary to create the appropriate control for a UseTypeAttribute
        /// </summary>
        /// <param name="attributeIndex">
        /// The index of the Attribute in the UseTypeWithAttributes property of the view
        /// </param>
        public void CreateRMAttributeControl(int attributeIndex)
        {
            int lastAttributeIndex = rmPricingView.LastAttributeIndex;
            rmPricingView.LastAttributeIndex = rmPricingView.LastAttributeIndex = 
                Math.Max(rmPricingView.LastAttributeIndex, attributeIndex);
            UseTypeAttribute attribute = rmPricingView.UseTypeWithAttributes.UseTypeAttributes[attributeIndex];
            if (attribute == null)
            {
                attribute = new UseTypeAttribute();
                attribute.DisplayType = AttributeDisplay.None;
            }
            switch (attribute.DisplayType)
            {
                case AttributeDisplay.Calendar:
                    rmPricingView.CreateCalendarControl(attributeIndex, attribute.QuestionText);
                    
                    break;
                case AttributeDisplay.DropdownList:
                    rmPricingView.CreateDropdownList(attributeIndex, attribute.QuestionText);
                    break;
                case AttributeDisplay.Geography:
                    rmPricingView.CreateGeographyControl(attributeIndex, attribute.QuestionText);
                    break;
                default:
                    rmPricingView.CreateBlankControl();
                    rmPricingView.LastAttributeIndex = lastAttributeIndex;
                    break;
            }
        }


        public void BindRMAttributeControl(int attributeIndex)
        {
            UseTypeAttribute attribute = rmPricingView.UseTypeWithAttributes.UseTypeAttributes[attributeIndex];
            if (attribute == null)
            {
                attribute = new UseTypeAttribute();
                attribute.DisplayType = AttributeDisplay.None;
            }
            switch (attribute.DisplayType)
            {
                case AttributeDisplay.Calendar:
                    rmPricingView.BindCalendarControl(GetLicenseStartDateText(attribute.AttributeUid));
                    break;
                case AttributeDisplay.DropdownList:
                    //TODO: Get actual value
                    rmPricingView.BindDropdownList(attribute.Values, GetRMAttributeValue(attribute.AttributeUid));
                    break;
                case AttributeDisplay.Geography:
                    //TODO: Get actual value
                    rmPricingView.BindGeographyControl(attribute.Values, GetGeographyAttributeValue(attribute.AttributeUid));
                    break;
                default:
                    // N/A
                    break;
            }

        }

        #endregion

        #region Control Validation methods

        bool? isValidRMUseCategory = null;
        public bool IsValidRMUseCategory()
        {
            // Check if we've already validated it
            if (isValidRMUseCategory.HasValue)
            {
                return isValidRMUseCategory.Value;
            }

            isValidRMUseCategory = false;
            UseCategory selectedCategory = rmPricingView.UseCategories.Find(
                delegate(UseCategory useCat)
                {
                    return useCat.Uid == rmPricingView.SelectedUseCategoryUid;
                });
            if (selectedCategory == null)
            {
                switch (rmPricingView.UsageType)
                {
                    case RMUsageType.Saved:
                    case RMUsageType.Existing:
                        rmPricingView.ShowLicenseAlertMessage = true;
                        rmPricingView.ShowValidationErrorsSummary = false;
                        break;
                    default:
                        rmPricingView.UseCategoryErrorsSummaryResourceKey = "useCategoryError.Text";
                        break;
                }
                rmPricingView.ShowUseCategoryErrors = true;
            }
            else
            {
                rmPricingView.ShowUseCategoryErrors = false;
                isValidRMUseCategory = true;
            }
            rmPricingView.UseCategoryErrorResourceKey = null;
            if (!isValidRMUseCategory.Value)
            {
                isValidExceptForLicenseStartDate = false;
            }
            return isValidRMUseCategory.Value;
        }

        bool? isValidRMUseType = null;
        public bool IsValidRMUseType()
        {
            // Check if we've already validated the useType
            if (isValidRMUseType.HasValue)
            {
                return isValidRMUseType.Value;
            }

            isValidRMUseType = false;
            UseType selecteduseType = rmPricingView.UseTypes.Find(
                delegate(UseType useType)
                {
                    return useType.Uid == rmPricingView.SelectedUseTypeUid;
                });
            if (selecteduseType == null)
            {
                switch (rmPricingView.UsageType)
                {
                    case RMUsageType.Saved:
                    case RMUsageType.Existing:
                        rmPricingView.ShowLicenseAlertMessage = true;
                        rmPricingView.ShowValidationErrorsSummary = false;
                        break;
                    default:
                        rmPricingView.UseTypeErrorsSummaryResourceKey = "useTypeError.Text";
                        break;
                }
                rmPricingView.ShowUseTypeErrors = true;
            }
            else
            {
                rmPricingView.ShowUseTypeErrors = false;
                isValidRMUseType = true;
            }
            rmPricingView.UseTypeErrorResourceKey = null;
            if (!isValidRMUseType.Value)
            {
                isValidExceptForLicenseStartDate = false;
            }
            return isValidRMUseType.Value;
        }

        private bool isValidExceptForLicenseStartDate = true;
        public bool ValidateAttribute(string controlId, int attributeIndex, string value)
        {
            bool isValid = false;
            UseTypeAttribute attribute = rmPricingView.UseTypeWithAttributes.UseTypeAttributes[attributeIndex];

            switch (controlId)
            {
                case "calendar":
                    isValid = IsValidLicenseStartDate(attribute, value);
                    break;
                case "geography":
                    isValid = IsValidGeography(attribute, attributeIndex, value);
                    if (!isValid)
                    {
                        isValidExceptForLicenseStartDate = false;
                    }
                    break;
                case "dropdown":
                    isValid = IsValidAttributeDropDown(attribute, attributeIndex, value);
                    if (!isValid)
                    {
                        isValidExceptForLicenseStartDate = false;
                    }
                    break;
                default:
                    // N/A
                    break;
            }
            return isValid;
        }

        bool? isValidLicenseStartDate = null;
        private bool IsValidLicenseStartDate(UseTypeAttribute attribute, string dateText)
        {
            // Check if we've aalready validated it
            if (isValidLicenseStartDate.HasValue)
            {
                return isValidLicenseStartDate.Value;
            }
            
            IAttributeValidator validator = rmPricingView.CurrentValidator;
            
            DateTime tmpStartDate = DateTime.MinValue;
            isValidLicenseStartDate = DateTime.TryParse(
                dateText,
                Language.CurrentCulture.DateTimeFormat,
                DateTimeStyles.None,
                out tmpStartDate);

            if (!isValidLicenseStartDate.Value)
            {
                validator.ErrorTextResourceKey = "InvalidDateError";
                validator.ErrorSummaryTextResourceKey =
                    AttributeValidationDictionary.ContainsKey(attribute.AttributeUid)
                    ? AttributeValidationDictionary[attribute.AttributeUid]
                    : null;
            }
            else
            {
                if (tmpStartDate < DateTime.Today)
                {
                    isValidLicenseStartDate = false;
                    validator.ErrorTextResourceKey = "DateBeforeTodayError";
                    validator.ErrorSummaryTextResourceKey =
                        AttributeValidationDictionary.ContainsKey(attribute.AttributeUid)
                        ? AttributeValidationDictionary[attribute.AttributeUid]
                        : null;
                }
            }

            if (isValidLicenseStartDate.Value)
            {
                validator.ErrorTextResourceKey = null;
                validator.ErrorSummaryTextResourceKey = null;
                validator.DisplayStyle = AttributeValidatorDisplayStyle.None;
            }
            else
            {
                validator.DisplayStyle = AttributeValidatorDisplayStyle.Dynamic;
            }
            
            // If we're validating, the usage has a licensestartdate
            rmPricingView.LicenseStartDate = tmpStartDate;

            return isValidLicenseStartDate.Value;
        }

        private bool IsValidAttributeDropDown(
            UseTypeAttribute attribute, 
            int attributeIndex, 
            string selectedValue)
        {
            IAttributeValidator validator = rmPricingView.CurrentValidator;

            bool isValid = false;

            Guid attValUid = Guid.Empty;
            isValid = GuidHelper.TryParse(selectedValue, out attValUid);
            UseTypeAttributeValue selectedAttVal = null;
            if (isValid && attribute.Values != null)
            {
                selectedAttVal = attribute.Values.Find(
                    delegate(UseTypeAttributeValue attVal)
                    {
                        return attVal.ValueUid == attValUid;
                    });
            }

            if (isValid)
            {
                validator.ErrorSummaryTextResourceKey = null;
                validator.DisplayStyle = AttributeValidatorDisplayStyle.None;
            }
            else
            {
                switch (rmPricingView.UsageType)
                {
                    case RMUsageType.Existing:
                    case RMUsageType.Saved:
                        // Case where an image was added to a lightbox with an incomplete usage
                        if (rmPricingView.UsageType == RMUsageType.Existing 
                            && String.IsNullOrEmpty(rmPricingView.ProductEffectivePrice))
                        {
                            validator.ErrorSummaryTextResourceKey =
                                AttributeValidationDictionary.ContainsKey(attribute.AttributeUid)
                                ? AttributeValidationDictionary[attribute.AttributeUid]
                                : null;

                        }
                        // Case where license details have changed for a Favorite Use
                        // or since an image was last priced
                        else
                        {
                            validator.ErrorSummaryTextResourceKey =
                                AttributeValidationDictionary.ContainsKey(attribute.AttributeUid)
                                ? AttributeValidationDictionary[attribute.AttributeUid]
                                : null;
                            rmPricingView.ShowLicenseAlertMessage = true;
                            rmPricingView.ShowValidationErrorsSummary = false;
                        }                       
                        break;
                    case RMUsageType.New:
                    case RMUsageType.Unknown:
                    default:
                        // Only set the error summary text if it's the attribute that was modified
                        if (attributeIndex == rmPricingView.AttributeModifiedIndex)
                        {
                            validator.ErrorSummaryTextResourceKey =
                                AttributeValidationDictionary.ContainsKey(attribute.AttributeUid)
                                ? AttributeValidationDictionary[attribute.AttributeUid]
                                : null;
                        }
                        break;
                }
            }

            validator.ErrorTextResourceKey = null;
            validator.DisplayStyle = AttributeValidatorDisplayStyle.None;
            return isValid;
        }


        private bool IsValidGeography(
            UseTypeAttribute attribute,
            int attributeIndex,
            string selectedValues)
        {
            bool isValid = true;
            IAttributeValidator validator = rmPricingView.CurrentValidator;
            List<Guid> selectedUids = new List<Guid>();
            try
            {
                string[] uids = selectedValues.Split(new char[] { ',' });
                foreach (string uidString in uids)
                {
                    Guid uid = new Guid(uidString);
                    selectedUids.Add(uid);
                }
            }
            catch
            {
                isValid = false;
            }

            if (isValid)
            {
                foreach (Guid selectedUid in selectedUids)
                {
                    UseTypeAttributeValue selectedAttVal = attribute.Values.Find(
                        delegate(UseTypeAttributeValue attVal)
                        {
                            return attVal.ValueUid == selectedUid;
                        });
                    if (selectedAttVal == null)
                    {
                        isValid = false;
                        break;
                    }
                }
            }

            if (isValid)
            {
                validator.ErrorTextResourceKey = null;
                validator.ErrorSummaryTextResourceKey = null;
                validator.DisplayStyle = AttributeValidatorDisplayStyle.None;
            }
            else
            {
                switch (rmPricingView.UsageType)
                {
                    case RMUsageType.Existing:
                    case RMUsageType.Saved:
                        // Case where an image was added to a lightbox with an incomplete usage
                        if (rmPricingView.UsageType == RMUsageType.Existing
                            && String.IsNullOrEmpty(rmPricingView.ProductEffectivePrice))
                        {
                            validator.ErrorSummaryTextResourceKey =
                                AttributeValidationDictionary.ContainsKey(attribute.AttributeUid)
                                ? AttributeValidationDictionary[attribute.AttributeUid]
                                : null;
                        }
                        // Case where license details have changed for a Favorite Use
                        // or since an image was last priced
                        else
                        {
                            validator.ErrorSummaryTextResourceKey =
                                AttributeValidationDictionary.ContainsKey(attribute.AttributeUid)
                                ? AttributeValidationDictionary[attribute.AttributeUid]
                                : null;
                            rmPricingView.ShowLicenseAlertMessage = true;
                            rmPricingView.ShowValidationErrorsSummary = false;
                        }                       
                        break;
                    case RMUsageType.New:
                    case RMUsageType.Unknown:
                    default:
                        // Only set the error summary text if it's the attribute that was modified
                        if (attributeIndex == rmPricingView.AttributeModifiedIndex)
                        {
                            validator.ErrorSummaryTextResourceKey =
                                AttributeValidationDictionary.ContainsKey(attribute.AttributeUid)
                                ? AttributeValidationDictionary[attribute.AttributeUid]
                                : null;
                        }
                        break;
                }
                validator.ErrorTextResourceKey = null;
                validator.DisplayStyle = AttributeValidatorDisplayStyle.Dynamic;
            }
            return isValid;
        }

        #endregion

        #region Express Checkout methods
        public void GetAllExpressCheckoutSavedUsages()
        {
            List<KeyValuePair<System.Guid, string>> savedUsages = new List<KeyValuePair<System.Guid, string>>();

            try
            {
                Guid selectedUid = rmPricingExpressCheckoutView.SavedUsageUid;
                savedUsages = lightboxCartService.GetAllSavedUsagesByUserName(Profile.UserName);
                rmPricingExpressCheckoutView.SavedUsages = savedUsages;
                rmPricingExpressCheckoutView.SavedUsageUid = selectedUid;
            }
            catch (Exception e)
            {
                HandleException(e, rmPricingExpressCheckoutView.LoggingContext, string.Format("PricingPresenter: GetAllExpressCheckoutSavedUsages() - Error getting  All saved usages."));
                throw;
            }
        }

        public bool BuildExpressCheckoutUsage()
        {
            bool isValid = true;

            // Check if we need to update the product with Saved Usage Details
            if (rmPricingExpressCheckoutView.SavedUsageUid != Guid.Empty)
            {
                lightboxCartService.ExpressCheckoutUpdateRM(
                    Profile.UserName,
                    rmPricingExpressCheckoutView.CorbisId,
                    rmPricingExpressCheckoutView.SavedUsageUid,
                    Profile.CountryCode,
                    Language.CurrentLanguage.LanguageCode);
            }
            FolderProduct product = lightboxCartService.GetProductByProductUid(
                Profile.UserName,
                rmPricingExpressCheckoutView.ProductUid,
                Profile.CountryCode,
                Language.CurrentLanguage.LanguageCode,
                true,
                true);

            SetExpressCheckoutPricingStatus(product.EffectivePriceStatus);

            if (product != null && product.Usage != null)
            {
                if (product.Usage.UseCategoryUid == Guid.Empty || product.Usage.UseTypeUid == Guid.Empty)
                {
                    rmPricingExpressCheckoutView.EmptyUsage = true;
                    return false;
                }

                // Set up the UseCatagory
                Guid selectedUseCategoryUid = Guid.Empty;
                UseCategory customUseCategory = new UseCategory();
                customUseCategory.Uid = product.Usage.UseCategoryUid;
                customUseCategory.LicenseModel = LicenseModel.RM;
                customUseCategory.DisplayText = product.Usage.UseCategoryDescription;

                if (!rmPricingExpressCheckoutView.PricedByAE)
                    GetExpressCheckoutUseCategories();

                if (rmPricingExpressCheckoutView.PricedByAE || rmPricingExpressCheckoutView.UseCategories.Exists(new Predicate<UseCategory>(
                    delegate(UseCategory category)
                    {
                        return category.Uid == product.Usage.UseCategoryUid;
                    })))
                {
                    rmPricingExpressCheckoutView.UseCategories =
                        new List<UseCategory>(new UseCategory[] { customUseCategory });
                    selectedUseCategoryUid = customUseCategory.Uid;
                }
                else
                {
                    return false;
                }

                // Set up the UseType
                Guid selectedUseTypeUid = Guid.Empty;
                UseType customUseType = new UseType();
                customUseType.Uid = product.Usage.UseTypeUid;
                customUseType.DisplayText = product.Usage.UseTypeDescription;
                customUseType.HelpText = product.Usage.UseTypeHelpText;

                if (!rmPricingExpressCheckoutView.PricedByAE)
                    GetExpressCheckoutUseTypes(selectedUseCategoryUid);

                if (rmPricingExpressCheckoutView.PricedByAE || rmPricingExpressCheckoutView.UseTypes.Exists(new Predicate<UseType>(
                    delegate(UseType useType)
                    {
                        return useType.Uid == product.Usage.UseTypeUid;
                    })))
                {
                    rmPricingExpressCheckoutView.UseTypes = new List<UseType>(new UseType[] { customUseType });
                    selectedUseTypeUid = customUseType.Uid;
                }
                else
                {
                    return false;
                }

                if (!rmPricingExpressCheckoutView.PricedByAE)
                    GetExpressCheckoutUseTypeAttributes(selectedUseCategoryUid , selectedUseTypeUid);

                // Now add the attributes and values
                customUseType.UseTypeAttributes = new List<UseTypeAttribute>();
                foreach (CompletedUsageAttributeValuePair attValPair in product.Usage.AttributeValuePairs)
                {
                    if (attValPair.DisplayType == AttributeDisplay.None)
                        continue;

                    if (!rmPricingExpressCheckoutView.PricedByAE && !ValidateExpressCheckoutAttributes(attValPair))
                    {
                        isValid = false;
                        break;
                    }

                    // See if we've already added the attribute, since Geography can have multiple values
                    UseTypeAttribute att = customUseType.UseTypeAttributes.Find(
                        delegate(UseTypeAttribute useTypeAtt)
                        {
                            return useTypeAtt.AttributeUid == attValPair.UseAttributeUid;
                        });
                    if (att == null)
                    {
                        att = new UseTypeAttribute();
                        att.AttributeUid = attValPair.UseAttributeUid;
                        att.DisplayText = attValPair.UseAttributeDescription;
                        att.QuestionText = attValPair.UseAttributeQuestion;
                        att.DisplayOrder = attValPair.DisplayOrder;
                        att.DisplayType = attValPair.DisplayType;
                        customUseType.UseTypeAttributes.Add(att);
                    }
                    if (attValPair.Value is Guid)
                    {
                        UseTypeAttributeValue attVal = new UseTypeAttributeValue();
                        attVal.AttributeUid = att.AttributeUid;
                        attVal.ValueUid = (Guid)attValPair.Value;
                        attVal.DisplayText = attValPair.ValueDescription;
                        if (att.Values == null)
                        {
                            att.Values = new List<UseTypeAttributeValue>();
                        }
                        att.Values.Add(attVal);
                    }
                    else if (attValPair.DisplayType == AttributeDisplay.Calendar && rmPricingExpressCheckoutView.LicenseStartDate == DateTime.MinValue)
                    {
                        rmPricingExpressCheckoutView.LicenseStartDate = (DateTime)attValPair.Value;
                    }
                }

                if (isValid)
                {
                    rmPricingExpressCheckoutView.UseTypeWithAttributes = customUseType;
                    rmPricingExpressCheckoutView.DisplayLicenseDetail();
                }
            }

            return isValid;
        }

        private bool ValidateExpressCheckoutAttributes(CompletedUsageAttributeValuePair attValPair)
        {
            bool isValid = true;

            // Check if it is valid attribute
            UseTypeAttribute validUseTypeAttribute = rmPricingExpressCheckoutView.UseTypeWithAttributes.UseTypeAttributes.Find(
                delegate(UseTypeAttribute attrib)
                {
                    return attrib.AttributeUid == attValPair.UseAttributeUid;
                });

            if (validUseTypeAttribute != null)
            {
                // Check if it is a valid attribute value.
                UseTypeAttributeValue validUseTypeValue = (validUseTypeAttribute.Values != null) ?
                    validUseTypeAttribute.Values.Find(
                    delegate(UseTypeAttributeValue val)
                    {
                        if (attValPair.Value is Guid)
                            return val.ValueUid == (Guid)attValPair.Value;
                        else
                            return true;
                    }) : new UseTypeAttributeValue();

                if (validUseTypeValue == null)
                {
                    isValid = false;
                }
            }
            else
            {
                isValid = false;
            }

            return isValid;
        }

        private void SetExpressCheckoutPricingStatus(PriceStatus status)
        {
            // Check if the user is not Ecommerce enabled or there is a country/currency discrepancy
            if ((status & PriceStatus.NotEcommerceEnabled) == PriceStatus.NotEcommerceEnabled)
            {
                status = PriceStatus.NotEcommerceEnabled;
            }
            else if ((status & PriceStatus.CountryOrCurrencyError) == PriceStatus.CountryOrCurrencyError)
            {
                status = PriceStatus.CountryOrCurrencyError;
            }
            
            rmPricingExpressCheckoutView.PricedByAE = ((status & PriceStatus.PricedByAE) == PriceStatus.PricedByAE);
            rmPricingExpressCheckoutView.CustomPricedExpired = ((status &
                (PriceStatus.PricedByAE | PriceStatus.CustomPriceExpired)) == (PriceStatus.PricedByAE | PriceStatus.CustomPriceExpired));

            switch (status)
            {
                case PriceStatus.ContactUs:
                case PriceStatus.NotEcommerceEnabled:
                case PriceStatus.ContactOutline:
                case PriceStatus.ContactUs | PriceStatus.DisplayPrice:
                case PriceStatus.CountryOrCurrencyError:
                case PriceStatus.UpdateUse:
                case PriceStatus.Unknown:
                    rmPricingExpressCheckoutView.ShowPriceStatusMessage = true;
                    break;
                default:
                    rmPricingExpressCheckoutView.ShowPriceStatusMessage = false;
                    break;
            }
        }

        public void GetExpressCheckoutUseCategories()
        {
            rmPricingExpressCheckoutView.UseCategories = lightboxCartService.GetUseCategories(
                Profile.UserName,
                OfferingType.Stills,
                LicenseModel.RM,
                Profile.CountryCode,
                Language.CurrentLanguage.LanguageCode,
                !rmPricingExpressCheckoutView.PricedByAE);
        }

        public void GetExpressCheckoutUseTypes(Guid selectedUseCategoryUid)
        {
            List<UseType> useTypes = lightboxCartService.GetUseTypes(
                Profile.UserName,
                selectedUseCategoryUid,
                Profile.CountryCode,
                Language.CurrentLanguage.LanguageCode,
                !rmPricingExpressCheckoutView.PricedByAE);
            rmPricingExpressCheckoutView.UseTypes = useTypes;
        }

        public void GetExpressCheckoutUseTypeAttributes(Guid selectedUseCategoryUid, Guid selectedUseTypeUid)
        {
            // Need a try/catch block to make sure the selected UseType is still valid
            try
            {
                UseType useTypeWithAttributes =
                    lightboxCartService.GetUseTypeAttributes(
                    Profile.UserName,
                    selectedUseCategoryUid,
                    selectedUseTypeUid,
                    Profile.CountryCode,
                    Language.CurrentLanguage.LanguageCode,
                    !rmPricingExpressCheckoutView.PricedByAE);
                rmPricingExpressCheckoutView.UseTypeWithAttributes = useTypeWithAttributes;
            }
            catch
            {
                rmPricingExpressCheckoutView.UseTypeWithAttributes = null;
            }
        }

        #endregion
    }
}
