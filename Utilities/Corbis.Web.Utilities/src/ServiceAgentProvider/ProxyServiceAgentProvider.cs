using System;
using System.Collections.Generic;
using System.Text;
using Corbis.Framework.Logging;
using Castle.Core.Interceptor;

namespace Corbis.Web.Utilities.Providers
{
    public class ProxyServiceAgentProvider : ServiceAgentProvider
    {
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config", "Configuration Section for the provider is null");

            if (String.IsNullOrEmpty(name))
                name = "ProxyServiceAgentProvider";

            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description",
                    "ProxyServiceAgentProvider for the generation of a dynamic proxy");
            }

            base.Initialize(name, config);
        }

        public override T CreateServiceAgent<T>()
        {
            List<string> categories = new List<string>();
            ILogging log = new LoggingContext(categories);

            return CreateServiceAgent<T>(log);
        }

        public override T CreateServiceAgent<T>(Corbis.Framework.Logging.ILogging logger)
        {
            if (!logger.Categories.Contains("ServiceAgents"))
            {
                logger.Categories.Add("ServiceAgents");
            }


            Castle.DynamicProxy.ProxyGenerator generator = new Castle.DynamicProxy.ProxyGenerator();
            IInterceptor[] interceptors = new IInterceptor[] { new DetailsInterceptor(logger) };
            T serviceProxy = generator.CreateClassProxy<T>(interceptors);

            object o = typeof(T);
            logger.LogInformationMessage("Proxy Service Agent Created", "Service Agent Name: " + o.ToString());

            return serviceProxy;

        }
    }
}