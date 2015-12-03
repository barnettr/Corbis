using System;
using System.Collections.Generic;
using System.Globalization;
using Corbis.DisplayText.Contracts.V1;
using Corbis.Framework.Globalization;
using DisplayTextRegion = Corbis.DisplayText.Contracts.V1.Region;
using Corbis.DisplayText.ServiceAgents.V1;
using Corbis.Web.Entities;

namespace Corbis.Web.Content
{
    public class RegionsContentProvider : DisplayServiceProvider
    {
        public virtual List<ContentItem> GetRegionsByCountryCode(string countryCode)
        {
            List<ContentItem> regions = new List<ContentItem>();
            Dictionary<int, DisplayTextEntity> data;
            
            try
            {
                data = DisplayService.GetAllRegionsByCountryCode(countryCode, Language.CurrentLanguage.LanguageCode);
            }
            catch (Exception ex)
            {
                string message = String.Format("Error retrieving Regions for CountryCode '{0}'.", countryCode);
                loggingContext.LogErrorMessage("GetRegionsByCountryCode: GetRegionsByCountry()", message, ex);
                throw new ApplicationException(message, ex);
            }

            foreach (KeyValuePair<int, DisplayTextEntity> item in data)
            {
                DisplayTextRegion entity = item.Value as DisplayTextRegion;
                regions.Add(new ContentItem(entity.RegionCode.Trim(), entity.DisplayText.Trim()));
            }

            return regions;
        }
    }
}
