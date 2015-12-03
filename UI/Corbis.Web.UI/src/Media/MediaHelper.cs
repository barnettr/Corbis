using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Framework.Globalization;
using Corbis.Web.Entities;

namespace Corbis.Web.UI.Media
{
    public static class MediaHelper
    {
        public static void LocalizeImageNotAvailableLinks(LightboxDisplayImage lightBoxDisplayImage)
        {
                if (lightBoxDisplayImage == null)
                    return;

                if (!lightBoxDisplayImage.IsImageAvailable)
                {
                    if(lightBoxDisplayImage.IsRfcd)
                    {
                        lightBoxDisplayImage.Url128 = string.Format(SiteUrls.LocalizedItemNotAvailable128, Language.CurrentLanguage.LanguageCode);
                        lightBoxDisplayImage.Url256 = string.Format(SiteUrls.LocalizedItemNotAvailable256, Language.CurrentLanguage.LanguageCode);
                    }else
                    {
                        lightBoxDisplayImage.Url128 = string.Format(SiteUrls.LocalizedImageNotAvailable128, Language.CurrentLanguage.LanguageCode);
                        lightBoxDisplayImage.Url256 = string.Format(SiteUrls.LocalizedImageNotAvailable256, Language.CurrentLanguage.LanguageCode);
                    }
                }
         }


        public static void LocalizeImageNotAvailableLinks(SearchResultProduct searchResultProduct)
        {
            if (searchResultProduct == null)
                return;

            if (!searchResultProduct.IsAvailable)
            {
                if (searchResultProduct.IsRFCD)
                {
                    searchResultProduct.Url128 = string.Format(SiteUrls.LocalizedItemNotAvailable128, Language.CurrentLanguage.LanguageCode);
                    searchResultProduct.Url256 = string.Format(SiteUrls.LocalizedItemNotAvailable256, Language.CurrentLanguage.LanguageCode);
                    
                }
                else
                {
                    searchResultProduct.Url128 = string.Format(SiteUrls.LocalizedImageNotAvailable128, Language.CurrentLanguage.LanguageCode);
                    searchResultProduct.Url256 = string.Format(SiteUrls.LocalizedImageNotAvailable256, Language.CurrentLanguage.LanguageCode);
                    searchResultProduct.Url170 = string.Format(SiteUrls.LocalizedImageNotAvailable170, Language.CurrentLanguage.LanguageCode);
                }
            }
        }


        public static void LocalizeImageNotAvailableLinks(CartDisplayImage cartDisplayImage)
        {
            if (cartDisplayImage == null)
                return;

            if (!cartDisplayImage.IsImageAvailable)
            {
                if (cartDisplayImage.IsRfcd)
                {
                    cartDisplayImage.Url128 = string.Format(SiteUrls.LocalizedItemNotAvailable128, Language.CurrentLanguage.LanguageCode);
                }
                else
                {
                    cartDisplayImage.Url128 = string.Format(SiteUrls.LocalizedImageNotAvailable128, Language.CurrentLanguage.LanguageCode);
                }
            }
        }
            
     }
}

