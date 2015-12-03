using System;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.Pricing.Interfaces
{
    public interface IPriceImageLink : IView
    {
        bool IsAnonymous { get; }
        bool ShowDownloadingProhibited { get; set; }
        bool ShowPricingLink { get; set; }
        String PricingAltText { get; set; }
        String PricingNavigateUrl { get; set;}
        Corbis.Web.Entities.ParentPage ParentPage { get; set; }
        String RMRawUrl { get; }
        String RFRawUrl { get; }
        String RSRawUrl { get; }
        String CustomerServiceUrl { get; }
        int RFPricingPageWidth { get; }
        int RFPricingPageHeight { get; }
        int RMPricingPageWidth { get; }
        int RMPricingPageHeight { get; }
        int RSPricingPageWidth { get; }
        int RSPricingPageHeight { get; }
    }
}
