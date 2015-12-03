using System;
using System.Collections.Generic;
using System.Text;
using Corbis.Image.Contracts.V1;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Entities;

namespace Corbis.Web.UI.Pricing.Interfaces
{
    public interface IRMPricing : IView
    {
        void ValidateUsage();
        bool IsValid { get; }

        string CorbisId { get; set; }
        Corbis.Web.Entities.ParentPage ParentPage { get; set; }
        Guid ProductUid { get; set; }
        bool CustomPriced { get; }
        bool PriceNowClicked { get; set; }
        Guid? CustomPricedProductUid { get; set; }
        Guid SavedUsageUid { get; set; }

        List<UseCategory> UseCategories { get; set; }
        Guid SelectedUseCategoryUid { get; set; }
        List<UseType> UseTypes { get; set; }
        Guid SelectedUseTypeUid { get; set; }

        List<KeyValuePair<System.Guid, string>> SavedUsages { set; get; }
        String SelectedSavedUsageText { get; }

        int LastAttributeIndex { get; set; }
        /// <summary>
        /// Index of the attribute that was modified
        /// </summary>
        int AttributeModifiedIndex { get; set; }
        UseType UseTypeWithAttributes { get; set; }

        string UsageName { get; set;}
        IPricingHeader PricingHeader { get; }

        string ProductEffectivePrice { get; set; }
        string EffectivePriceZeroText { get; }
        string EffectivePriceText { get; set; }
        string EffectivePriceCurrencyText { get; set; }
        PriceStatus EffectivePriceStatus { get; set; }
        string EffectivePriceStatusText { get; }
        bool ShowPriceStatusMessage { set; }
        bool PricedByAE { get; set; }
        string PricedByAEParagraph1ResourceKey { set; }
        string PricedByAeCalendarFormat { get; set; }
        bool ShowCustomPriceExpiredMessage { set; }
        string CustomPriceExpiredMessageResourceKey { set; }
        
        RMUsageType UsageType { get; set; }
        IImageRestrictionsView RestrictionsControl { get; }

        bool ShowStartFromScratch { set;}
        bool ShowStartOver { set; }
        bool PriceNowButtonEnabled { get; set;}
        bool PriceNowButtonVisible { get; set; }
        bool SaveFavoriteUsageButtonEnabled { get; set; }
        bool SaveFavoriteUsageButtonVisible { get; set; }
        bool ShowSelectUseCategoryQuestion { get; set; }
        bool ShowSelectUseTypeQuestion { get; set; }
        bool ShowFavoriteUseTitle { get; set; }
        bool ShowFavoriteUseValue { get; set; }
        string FavoriteUseValue { get; set; }
        string FavoriteUseTooltip { get; set; }
        bool ShowFavoriteUseInstructions { get; set; }

        bool ShowOr { get; set; }
        bool ShowSavedUsageBottomBorder { set; }
        bool ShowUseCatTypeBottomBorders { set; }
        bool CreateNewUseDiv { get; set; }

        bool ShowLicenseAlertMessage { get; set; }
        bool ShowValidationErrorsSummary { get; set; }
        int ValidationErrorsCount { get; }
        bool ShowUseCategoryErrors { get; set; }
        string UseCategoryErrorsSummaryResourceKey { get; set; }
        string UseCategoryErrorResourceKey { get; set; }

        bool ShowUseTypeErrors { get; set; }
        string UseTypeErrorsSummaryResourceKey { get; set; }
        string UseTypeErrorResourceKey { get; set; }

        void CreateBlankControl();
        void CreateCalendarControl(int attributeIndex, string questionText);
        void BindCalendarControl(string dateText);
        void CreateDropdownList(int attributeIndex, string questionText);
        void BindDropdownList(List<UseTypeAttributeValue> values, Guid selectedValue);
        void CreateGeographyControl(int attributeIndex, string questionText);
        void BindGeographyControl(List<UseTypeAttributeValue> values, List<Guid> selectedValues);
        IAttributeValidator CurrentValidator { get; }
        IAttributeValidator CalendarValidator { get; }

        #region License Details
        string UseTypeHelpText { get; }
        string UseCategoryLicenseDetails { get; }
        DateTime? LicenseStartDate { get; set; }
        string UseTypeLicenseDetails { get; }
        string AttributeLicenseDetails { get; set; }
        string AllLicenseDetails { get; set; }
        #endregion

        // Putting this here temporarily until I can move the logic to the presenter
        void CreateUsage(bool updateLicenseDisplay, bool updateUsageInPresenter);

    }
}
