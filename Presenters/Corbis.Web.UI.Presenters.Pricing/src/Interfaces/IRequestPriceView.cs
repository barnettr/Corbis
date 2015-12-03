using System;
using System.Collections.Generic;
using System.Text;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.CommonSchema.Contracts.V1;

namespace Corbis.Web.UI.Pricing.ViewInterfaces
{
    public interface IRequestPriceView : IView
    {
        string FirstName { get; set; }

        string LastName { get; set; }

        string FirstNameAsian { get; set; }

        string LastNameAsian { get; set; }

        bool NameDefaultVisibility { set; }

        bool NameAsianVisibility { set; }
        
        string Email { get; set; }

        string Phone { get; set; }

        string CountryCode { get; set; }

        string StateCode { get; set; }

        bool StatesEnabled { set; }

        string Comments { get; }

        string SelectOne { get; }

        string Dashes { get; }

        string LightboxName { get; }

        string ContainerName { get; }

        string CorbisId { get; }

        int LightboxId { get; }

        List<LightboxDisplayImage> LightboxItems { get; set; }

        string GetLicenceModelLocalizedText(LicenseModel licenseModel);
    }
}
