using System;
using System.Collections.Generic;
using System.Globalization;
using Corbis.DisplayText.Contracts.V1;
using Corbis.DisplayText.ServiceAgents.V1;
using Corbis.Framework.Globalization;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.Entities;

namespace Corbis.Web.Content
{
    public class EmailFormatContentProvider : DisplayServiceProvider
    {
        public virtual List<ContentItem> GetEmailFormats()
        {
            List<ContentItem> emailFormats = new List<ContentItem>();
			Dictionary<int, DisplayTextEntity> data;

			try
			{
                data = DisplayService.GetAllEntitiesByType(typeof(Corbis.DisplayText.Contracts.V1.EmailFormat).FullName, Language.CurrentLanguage.LanguageCode);
			}
			catch (Exception ex)
			{
				loggingContext.LogErrorMessage("CountriesContentProvider: GetCountries", "Unable to retrieve EmailFormats from the service.", ex);
				return emailFormats;
			}

			foreach (KeyValuePair<int, DisplayTextEntity> item in data)
			{
				DisplayTextEntity entity = (DisplayTextEntity)item.Value;
				emailFormats.Add(new ContentItem(entity.DisplayText.Trim(), entity.DisplayText.Trim()));
			}

            return emailFormats;
        }
    }
}
