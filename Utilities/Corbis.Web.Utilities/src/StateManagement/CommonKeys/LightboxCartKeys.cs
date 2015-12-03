using System;

namespace Corbis.Web.Utilities.StateManagement
{
    public static class LightboxCartKeys
    {
        public static string Name = "LightboxCart";
        public static string LightboxIdKey = "lightboxId";
		public static string SortTypeKey = "sortType";
        public static string CartProductUids = "cartProductUids";
        public static string CheckoutReadyItems = "CheckoutReadyItems";
        public static string LightboxPreviewKey = "LightboxPreview";
        public static string LightboxPageNumber = "LightboxPageNumber";
    }

    public static class LightboxCOFFKeys
    {
        public static string Name = "LightboxCOFF";
        public static string COFFValidationResults = "coffValidationResults";
        public static string COFFProductUids = "coffProductUids";
        public static string COFFCheckoutItems = "coffCheckoutItems";
    }
}
