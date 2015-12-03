using System;
using System.Collections.Generic;
using System.Text;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Web.Entities;

namespace Corbis.Web.UI.Pricing.Interfaces
{
    public interface IRSPricing : IView
    {
        string CorbisId { get; set; }
        Dictionary<string, List<RsUseTypeAttributeValue>> RSUseTypeAttributes { get; set; }
        Corbis.Web.Entities.ParentPage ParentPage { get; set; }
        Guid UseCategoryUid { get; set; }
        IPricingHeader PricingHeader { get; }
    }
}
