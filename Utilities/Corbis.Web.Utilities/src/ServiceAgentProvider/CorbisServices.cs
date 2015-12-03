using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web.Configuration;
using System.Configuration.Provider;

namespace Corbis.Web.Utilities.Providers
{
    public class CorbisServices
    {
        private static ServiceAgentProvider provider = null;
        private static ServiceAgentProviderCollection providers = null;
        private static object lockSync = new object();

        public ServiceAgentProvider Provider
        {
            get
            {
                return provider;
            }
        }

        public ServiceAgentProviderCollection Providers
        {
            get
            {
                return providers;
            }
        }

        public static T CreateServiceAgent<T>() where T : new()
        {
            LoadProviders();

            return provider.CreateServiceAgent<T>();

        }

        private static void LoadProviders()
        {
            // Avoid claiming lock if providers are already loaded
            if (provider == null)
            {
                lock (lockSync)
                {
                    // Do this again to make sure _provider is still null
                    if (provider == null)
                    {
                        // Get a reference to the <imageService> section
                        ServiceAgentProviderSection section = (ServiceAgentProviderSection)
                            WebConfigurationManager.GetSection
                            ("serviceAgentProviders");

                        // Load registered providers and point _provider
                        // to the default provider
                        providers = new ServiceAgentProviderCollection();
                        ProvidersHelper.InstantiateProviders
                            (section.Providers, providers,
                            typeof(ServiceAgentProvider));

                        provider = providers[section.DefaultProvider];

                        if (provider == null)
                            throw new ProviderException
                                ("Unable to load default ServiceAgentProvider");
                    }
                }
            }

        }

    }
}
