using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;

namespace Corbis.Web.Utilities.Providers
{
    public class ServiceAgentProviderCollection : ProviderCollection
    {
        public new ServiceAgentProvider this[string name]
        {
            get
            {
                return (ServiceAgentProvider)base[name];
            }
        }

        public override void Add(ProviderBase provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            if (!(provider is ServiceAgentProvider))
                throw new ArgumentException
                    ("Provider must be of type ServiceAgentProvider", "provider");

            base.Add(provider);
        }
    }
}
