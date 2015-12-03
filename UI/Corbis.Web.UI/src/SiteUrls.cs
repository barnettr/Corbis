using System;
using System.Web;

namespace Corbis.Web.UI
{
    public static class SiteUrls
    {
        public static readonly string Authenticate = GetAuthenticateUrl();

        public static readonly string BetaLogin = GetUrl("/beta.aspx");
        public static readonly string UnsupportedBrowser = GetUrl("/unsupportedBrowser.aspx");
        public static readonly string ApplyCredit = GetUrl("/Accounts/ApplyCredit.aspx");
        public static readonly string Cart = GetUrl("/Checkout/Cart.aspx");
        public static readonly string CartAbsoluteHttpUrl = GetAbsoluteNonSecureUrl("/Checkout/Cart.aspx");
        //public static readonly string ChangePassword = GetUrl("/Accounts/ChangePassword.aspx");
        public static readonly string ChangePassword = GetUrl("/Accounts/MyProfile.aspx") + "?Pane=personalInfoPane";
        public static readonly string Checkout = GetUrl("/Checkout/MainCheckout.aspx");
        public static readonly string CoffCheckout = GetUrl("/Checkout/MainCheckout.aspx") + "?OrderType=COFF";
        public static readonly string ContactUs = GetUrl("/Corporate/ContactUs.aspx");
        //public static readonly string CustomerService = GetUrl("/CustomerService/CustomerService.aspx");
        public static readonly string CustomerService = GetUrl("/CustomerService/CustomerService.aspx");
        public static readonly string EditBusinessInformation = GetUrl("/Accounts/EditBusinessInformation.aspx");
        public static readonly string EditShippingAddress = GetUrl("/Accounts/EditShippingAddress.aspx");
        public static readonly string EmailLightboxView = GetUrl("/Lightboxes/EmailLightboxView.aspx");
        public static readonly string Employment = "http://www.corbis.com/corporate/Employment/Employment.asp";
        public static readonly string Overview = "http://www.corbis.com/corporate/overview/overview.asp";
        public static readonly string Pressroom = "http://www.corbis.com/corporate/pressroom/default.asp";
        public static readonly string Enlargement = GetUrl("/Enlargement/Enlargement.aspx");
        public static readonly string ExpressCheckout = GetUrl("/Checkout/ExpressCheckout.aspx");
        public static readonly string ExpressDownload = GetAbsoluteNonSecureUrl("/Checkout/ExpressDownload.aspx");
        public static readonly string Home = GetUrl("/Default.aspx");
        public static readonly string IFrameTunnel = GetUrl("/Common/IFrameTunnel.aspx");
        public static readonly string LicenseAgreement = GetUrl("/Legal/LicenseInfo.aspx");
        public static readonly string Lightboxes = GetUrl("/Lightboxes/MyLightboxes.aspx");
        public static readonly string MyProfile = GetUrl("/Accounts/MyProfile.aspx");
        public static readonly string MediaSetSearch = GetUrl("/MediaSetSearch/MediaSetSearch.aspx");
        public static readonly string ImageGroups = GetUrl("/ImageGroups/ImageGroups.aspx");
        public static readonly string MyLightBoxes = GetUrl("/Lightboxes/MyLightboxes.aspx");
        public static readonly string MyLightBoxesAbsoluteHttpUrl = GetAbsoluteNonSecureUrl("/Lightboxes/MyLightboxes.aspx");
        public static readonly string MyOrders = GetUrl("/OrderHistory/OrderHistory.aspx");
        public static readonly string MyUsages = GetUrl("/Lightboxes/MyLightboxes.aspx");
        public static readonly string Orders = GetUrl("/OrderHistory/OrderHistory.aspx");
        public static readonly string OrderHistorySummary = GetUrl("/OrderHistory/OrderHistorySummary.aspx");
        public static readonly string OrderComplete = GetUrl("/Checkout/OrderComplete.aspx");
        public static readonly string CoffOrderComplete = GetUrl("/Checkout/OrderComplete.aspx") + "?OrderType=COFF";
        public static readonly string PageNotFound = GetUrl("/Errors/PageNotFound.aspx");
        public static readonly string PrivacyPolicy = GetUrl("/Legal/PrivacyPolicy.aspx");
        public static readonly string RSPricing = GetUrl("/Pricing/RSPricing.aspx");
        public static readonly string PricingGridControl = GetUrl("/Pricing/PricingGrid.ascx");
        public static readonly string QuickPic = GetUrl("/QuickPic/QuickPic.aspx");
        public static readonly string RMPricing = GetUrl("/Pricing/RMPricing.aspx");
        public static readonly string RFPricing = GetUrl("/Pricing/RFPricing.aspx");
        public static readonly string RfcdCategory = GetUrl("/Rfcd/RfcdCategory.aspx");
        public static readonly string RfcdResults = GetUrl("/imagegroups/imagegroups.aspx");
        public static readonly string Register = GetUrl("/Registration/Register.aspx");
        public static readonly string RegistrationConfirmation = GetUrl("/Registration/RegistrationConfirmation.aspx");
        public static readonly string ResearchRequest = GetUrl("/Corporate/ResearchRequest.aspx");
        public static readonly string RightsClearances = GetUrl("/Rights/RightsClearances.aspx");
        public static readonly string SearchResults = GetUrl("/Search/SearchResults.aspx");
        public static readonly string SignIn = GetUrl("/Registration/SignIn.aspx");
        public static readonly string SignInChangePassword = GetUrl("/Registration/ChangePassword.aspx");
        public static readonly string SignOut = GetUrl("/SignIn/SignOut.aspx");
        public static readonly string SignInInformationRequest = GetUrl("/Registration/SignInInformationRequest.aspx");
        public static readonly string SiteUsageAgreement = GetUrl("/Legal/SiteUsageAgreement.aspx");
        public static readonly string UnexpectedError = GetUrl("/Errors/UnexpectedError.aspx");
        public static readonly string OrderSubmissionError = GetUrl("/Checkout/OrderSubmissionError.aspx");
        public static readonly string ExpressCheckoutOrderSubmissionError = GetAbsoluteNonSecureUrl("/Checkout/ExpressCheckout_OrderSubmissionError.aspx");

        public static readonly string MasterBaseMasterFileLoc = GetUrl("/MasterPages/MasterBase.Master");
        public static readonly string NoGlobalNavMasterFileLoc = GetUrl("/MasterPages/NoGlobalNav.Master");

        /// <summary>
        /// 
        /// </summary>
        public static readonly string WebService_PriceImageService = "~/Pricing/PriceImageService.asmx";

        /// <summary>
        /// Common JavaScript
        /// </summary>
        /// 
        public static readonly string MasterBaseJavascript = GetUrl("/scripts/script.aspx") + "?collection=base";
        // MasterBaseJavascript rolls up the following
        public static readonly string MootoolsScript = GetUrl("/Scripts/mootools-1.2-debug.js");
        public static readonly string Excanvas = GetUrl("/Scripts/excanvas-compressed.js");
        public static readonly string CorbisScript = GetUrl("/Scripts/CorbisUI.js");
        public static readonly string MochaUIScript = GetUrl("/Scripts/mocha-debug.js");
        public static readonly string FValidatorScript = GetUrl("/Scripts/fValidator-debug.js");
        public static readonly string MochaExtensionsScript = GetUrl("/Scripts/mocha-extensions.js");
        public static readonly string ModalPopupScript = GetUrl("/Scripts/ModalPopup.js");
        public static readonly string InstantServiceScript = GetUrl("/Scripts/InstantService.js");
        public static readonly string RoundedCornersScript = GetUrl("/Scripts/rounded.js");
        public static readonly string DropShadowScript = GetUrl("/Scripts/DropShadow.js");
        public static readonly string CommonScript = GetUrl("/Scripts/Common.js");
        public static readonly string ExtendedSearch = GetUrl("/Scripts/ExtendedSearch.js");
        //<script>/scripts/ImageRadio.js</script>
        //<script>/scripts/glassbutton.js</script>
        //<script>/scripts/ImageCheckbox.js</script>
        //<script>/scripts/TextIconButton.js</script>
        // End MasterBaseJavascript

        public static readonly string SearchResultsJavascript = GetUrl("/scripts/script.aspx") + "?collection=searchresults";
        // SearchResultsJavascript rolls up the following
        public static readonly string SearchScript = GetUrl("/Search/Search.js");
        public static readonly string SearchBuddyScript = GetUrl("/Search/SearchBuddy.js");
        public static readonly string AddToCartScript = GetUrl("/Checkout/AddToCart.js");
        // End SearchResultsJavascript

        // Must kill the owl from this one
        public static readonly string ExpressCheckoutScript = GetUrl("/scripts/script.aspx") + "?collection=expresscheckout";

        // Kill off batman
        public static readonly string CheckoutScript = GetUrl("/scripts/script.aspx") + "?collection=checkout";

        public static readonly string IE7Script = GetUrl("/scripts/script.aspx") + "?collection=ie7script";
        public static readonly string LightboxScript = GetUrl("/scripts/script.aspx") + "?collection=lightbox";
        public static readonly string CartScript = GetUrl("/scripts/script.aspx") + "?collection=cart";
        public static readonly string ErrorTrackerScript = GetUrl("/scripts/script.aspx") + "?collection=errortracker";
        public static readonly string FilmstripScript = GetUrl("/scripts/script.aspx") + "?collection=filmstrip";
        // public static readonly string ImageDetailScript = GetUrl("/scripts/script.aspx") + "?collection=imagedetail";
        public static readonly string EnlargementScript = GetUrl("/scripts/script.aspx") + "?collection=enlargement";
        public static readonly string OrderScript = GetUrl("/scripts/script.aspx") + "?collection=order";
        public static readonly string OrderHistoryScript = GetUrl("/scripts/script.aspx") + "?collection=orderhistory";
        public static readonly string ImageScript = GetUrl("/scripts/script.aspx") + "?collection=imagescript";
        public static readonly string QuickPicScript = GetUrl("/scripts/script.aspx") + "?collection=quickpic";
        public static readonly string EmailLightboxScript = GetUrl("/scripts/script.aspx") + "?collection=emaillbx";
        public static readonly string MoveLightboxScript = GetUrl("/scripts/script.aspx") + "?collection=movelbx";

        /// <summary>
        /// Image urls referenced in code.
        /// </summary>
        public static readonly string ItemsPerPage10 = GetUrl("/Images/20UnSelected.GIF");
        public static readonly string ItemsPerPage25 = GetUrl("/Images/20UnSelected.GIF");
        public static readonly string ItemsPerPage50 = GetUrl("/Images/20UnSelected.GIF");
        public static readonly string ItemsPerPage100 = GetUrl("/Images/20UnSelected.GIF");

        public static readonly string RestrictedCopyright128Format = GetUrl("/Images/{0}/copyright_restrict_128.gif");
        public static readonly string RestrictedCopyright170Format = GetUrl("/Images/{0}/copyright_restrict_170.gif");
        public static readonly string RestrictedCopyright256Format = GetUrl("/Images/{0}/copyright_restrict_256.gif");

        public static readonly string ImageNotAvailable256 = GetUrl("/Images/{0}/not_available_256.gif");

        private static string GetUrl(string page)
        {
            return VirtualPathUtility.ToAbsolute(Properties.Settings.Default.WebRoot + page);
        }
        private static string GetAbsoluteNonSecureUrl(string page)
        {
            return Properties.Settings.Default.HttpUrl + page;
        }
        public static string GetAuthenticateUrl()
        {
            return "~/Default.aspx?ReturnUrl=" + HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.PathAndQuery);
        }
    }
}
