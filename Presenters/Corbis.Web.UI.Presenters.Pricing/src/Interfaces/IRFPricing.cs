using System;
using System.Collections.Generic;
using System.Text;
using Corbis.Image.Contracts.V1;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.Pricing.Interfaces
{
    public interface IRFPricing : IView
    {
        string CorbisId { get; set; }
        Guid ProductUid { get; set; }
        RfPriceList PriceList { get; set; }
        Corbis.Web.Entities.ParentPage ParentPage { get; set; }
        Guid UseCategoryUid { get; set; }
        Guid UseTypeUid { get; set; }
        void AddToCompletedUsageAVPairList(Guid useAttributeUid, object value, bool clearListBeforeAdd);
        IPricingHeader PricingHeader { get; }
        PriceStatus EffectivePriceStatus { get; set; }
        string EffectivePriceText { get; set; }
        IImageRestrictionsView RestrictionsControl { get;}
        Guid AttributeValueUid { get; set;}
        bool PricedByAE { get; set; }
        bool CustomPriced { get; set; }
        bool ValidLicense { get; set; }
        string DimensionText { set; }
        string FileSizeText { set; }
        ImageSize ImageFileSize { set;}
        bool ShowPriceStatusMessageForRF { set; }
    }
}
