using System;
using System.Collections.Generic;
using Corbis.Image.Contracts.V1;
using Corbis.CommonSchema.Contracts.V1;


namespace Corbis.Web.UI.ViewInterfaces
{
    public interface IImageRestrictionsView
    {
        Corbis.Framework.Globalization.Language LanguageName { set; }

        bool ShowPricingIcon { set; }
        string PricingIconUrl { set; }

        bool ShowDomesticEmbargoDate { set; }
        string DomesticEmbargoDate { set; }
        
        bool ShowInternationalEmbargoDate { set; }
        string InternationalEmbargoDate { set; }

        ModelRelease ModelRelease { set; }
        string PropertyReleaseText { set; }

        List<Restriction> RestrictionsDataSource { set; }
    }
}
