using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.Entities
{
    public static class AnalyticsEvents
    {
        public const string ProductViewed = "prodView";
        public const string CartStart = "scOpen";
        public const string CartAddTo = "scAdd";
        public const string CheckoutStart = "scCheckout";
        public const string CheckoutFinish = "purchase";
        public const string PageView = "event1";
        public const string RegistrationStart = "event2";
        public const string RegistrationFinish = "event3";
        public const string Login = "event4";
        public const string Search = "event5";
        public const string SearchRefine = "event6";
        public const string LightboxCreate = "event7";
        public const string LightboxEmail = "event8";
        public const string LightboxSend = "event9";
        public const string LightboxReceive = "event10";
        public const string LightboxCopy = "event11";
        public const string LightboxAddTo = "event12";
        public const string LightboxMove = "event13";
        public const string CartAddUnpriced = "event14";
        public const string QuickPicAddTo = "event15";
        public const string PricingStart = "event16";
        public const string PricingFinish = "event17";
        public const string PricingFailure = "event18";
        public const string DownloadReDownload = "event19";
        public const string CheckoutAddTo = "event20";
        public const string PricingExpired = "event21";
        public const string ExpressCheckoutStart = "event22";
        public const string PricingRMSelectFavoriteUse = "event23";
        public const string PricingRMStart = "event24";
        public const string PricingRMSaveFavoriteUse = "event25";
        public const string PricingRMStartOver = "event26";
        public const string EnlargementPrint = "event29";
        public const string EnlargementViewDimensionsLoad = "event30";
        public const string EnlargementLocationSearch = "event31";
        public const string EnlargementPhotographerSearch = "event32";
        public const string EnlargementCaptionDisclaimerLoad = "event33";
        public const string EnlargementKeywordLoad = "event34";
        public const string EnlargementRelatedImagesLoad = "event35";
        public const string EnlargementDetailsLoad = "event36";
        public const string OrderHistorySummaryLoad = "event39";
    }
}
