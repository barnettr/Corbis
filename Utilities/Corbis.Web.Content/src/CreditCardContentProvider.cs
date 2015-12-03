using System;
using System.Collections.Generic;
using System.Globalization;
using Corbis.DisplayText.Contracts.V1;
using Corbis.Framework.Globalization;
using DisplayTextCreditCardType = Corbis.DisplayText.Contracts.V1.CreditCardType;
using Corbis.DisplayText.ServiceAgents.V1;
using Corbis.Web.Entities;

namespace Corbis.Web.Content
{
    public class CreditCardContentProvider : DisplayServiceProvider
    {
        public List<ContentItem> GetCreditCards(string countryCode)
        {
            List<ContentItem> creditCards = new List<ContentItem>();
            Dictionary<int, DisplayTextEntity> data;

            try
            {
                data = DisplayService.GetAllCreditCardTypesByCountryCode(countryCode, Language.CurrentLanguage.LanguageCode);
            }
            catch (Exception ex)
            {
                loggingContext.LogErrorMessage("CreditCardProvider: GetAvailableCreditCards", String.Format("Error retrieving CreditCards for CountryCode '{0}'.", countryCode), ex);
                return creditCards;
            }

            foreach (KeyValuePair<int, DisplayTextEntity> item in data)
            {
                DisplayTextCreditCardType entity = item.Value as DisplayTextCreditCardType;
                creditCards.Add(new ContentItem(entity.CreditCardCode.Trim(), entity.DisplayText.Trim()));
            }

            return creditCards;
        }
    }
}
