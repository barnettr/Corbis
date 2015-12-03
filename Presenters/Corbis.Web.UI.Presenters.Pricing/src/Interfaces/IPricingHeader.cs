using System;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.Pricing.Interfaces
{
    public interface IPricingHeader : IView
    {
        string CorbisId { get; set; }
        Guid OfferingUid { get; set; }
        string ImageThumbnail { set; }
        bool IsThumbPortrait { set; }
        Corbis.CommonSchema.Contracts.V1.Category ImageTitle { set; }
        string ImageId { set; }
        string PricingTier { set; }
        string LicenseModel { set; }
        bool CartButtonEnabled { get; set; }
        bool UpdatingCart { get; set; }
        bool LightboxButtonEnabled { get; set; }
        bool UpdatingLightbox { get; set; }
        bool LicenseDetailsVisible { get; set; }
        bool imageContainerVisible { get; set;}
        string CurrencyCode { set; }
        Corbis.Web.Entities.ParentPage ParentPage { get; set; }
    }
}
