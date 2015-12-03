using System;
using System.Collections.Generic;
using System.Globalization;
using Corbis.DisplayText.Contracts.V1;
using Corbis.Framework.Globalization;
using DisplayTextCountry = Corbis.DisplayText.Contracts.V1.Country;
using Corbis.DisplayText.ServiceAgents.V1;
using Corbis.Web.Entities;

namespace Corbis.Web.Content
{
    public class CountriesContentProvider : DisplayServiceProvider
    {
        public virtual List<ContentItem> GetCountries()
        {
            return GetCultureSpecificCountries(Language.CurrentLanguage.LanguageCode);
        }

        public virtual List<ContentItem> GetCultureSpecificCountries(string culture)
        {
            List<ContentItem> countries = new List<ContentItem>();
            Dictionary<int, DisplayTextEntity> data;

            try
            {
                data = DisplayService.GetAllEntitiesByType(typeof(DisplayTextCountry).FullName, culture);
            }
            catch (Exception ex)
            {
                loggingContext.LogErrorMessage("CountriesContentProvider: GetCountries()", "Error retrieving Countries.", ex);
                throw new ApplicationException("Error Retrieving Countries", ex);
            }

            foreach (KeyValuePair<int, DisplayTextEntity> item in data)
            {
                DisplayTextCountry entity = (DisplayTextCountry)item.Value;
                countries.Add(new ContentItem(entity.CountryCode.Trim(), entity.DisplayText.Trim()));
            }

            return countries;
        }
    }
}
