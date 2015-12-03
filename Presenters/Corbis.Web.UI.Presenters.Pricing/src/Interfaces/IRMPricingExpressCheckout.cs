using System;
using System.Collections.Generic;
using System.Text;
using Corbis.Image.Contracts.V1;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Entities;

namespace Corbis.Web.UI.Pricing.Interfaces
{
    public interface IRMPricingExpressCheckout : IView
    {
        string CorbisId { get; set; }
        Guid ProductUid { get; set; }
        Guid SavedUsageUid { get; set; }
        bool CustomPricedExpired { get; set; }
        bool EmptyUsage { get; set; }
        List<UseCategory> UseCategories { get; set; }
        List<UseType> UseTypes { get; set; }
        string EffectivePriceText { get; set; }
        string LightboxId { get; set; }
        List<KeyValuePair<System.Guid, string>> SavedUsages { set; get; }
        UseType UseTypeWithAttributes { get; set; }

        bool PricedByAE { get; set; }
        bool ShowPriceStatusMessage { get; set; }
        #region License Details
        string UseTypeHelpText { get; }
        string UseCategoryLicenseDetails { get; }
        DateTime LicenseStartDate { get; set; }
        string UseTypeLicenseDetails { get; }
        string AllLicenseDetails { get; set; }
        #endregion

        void DisplayLicenseDetail();
    }
}
