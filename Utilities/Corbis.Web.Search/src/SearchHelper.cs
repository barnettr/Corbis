using System;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Configuration;

namespace Corbis.Web.Search
{
    /// <summary>
    /// Helper class for Search
    /// </summary>
    public static class SearchHelper
    {
        static SearchHelper()
        {
            Initialize();
        }

        private static void Initialize()
        {
            SearchSection searchSection = (SearchSection)ConfigurationManager.GetSection("search");
            Providers = new ProviderCollection();
            ProvidersHelper.InstantiateProviders(searchSection.Providers, Providers, typeof(SearchProvider));
            DefaultProvider = (SearchProvider)Providers[searchSection.DefaultProvider];
        }

        private static SearchProvider defaultProvider;
        /// <summary>
        /// Default search provider
        /// </summary>
        public static SearchProvider DefaultProvider
        {
            get { return defaultProvider; }
            set { defaultProvider = value; }
        }

        private static ProviderCollection providers;
        /// <summary>
        /// Search provider collection
        /// </summary>
        public static ProviderCollection Providers
        {
            get { return providers; }
            set { providers = value; }
        }
    }
}