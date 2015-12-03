using System;
using Corbis.Web.Entities;

namespace Corbis.Web.Content
{
    public class ContentProviderFactory
    {
        /// <summary>
        /// CreateProvider
        /// </summary>
        /// <param name="ProviderName">This is the type of contentItem to return</param>
        /// <returns>A Type of IContentProvider, that can be used to get data</returns>
        public static IContentProvider CreateProvider(ContentItems contentItems)
        {
            IContentProvider provider = null;

            switch (contentItems)
            {
                case ContentItems.Country:
                    provider = new CountriesContentProvider();
                    break;
                case ContentItems.CreditCardType:
                    provider = new CreditCardContentProvider();
                    break;
                case ContentItems.EmailFormat:
                    provider = new EmailFormatContentProvider();
                    break;
                case ContentItems.Language:
                    provider = new LanguageContentProvider();
                    break;
                case ContentItems.Region:
                    provider = new RegionsContentProvider();
                    break;
                default:
                    throw new ArgumentException(string.Format("ContentProviderFactory: CreateProvider() - ContentProvider '{0}' not found.", contentItems));

            }
            return provider;
        }

        public static T CreateProvider<T>() where T : IContentProvider, new()
        {
            return new T();
        }
    }
}
