using System;
using System.Collections.Generic;
using System.Text;
using Corbis.Framework.Logging;

namespace Corbis.Web.Utilities.Providers
{
    public class StandardServiceAgentProvider : ServiceAgentProvider
    {


        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config", "Configuration Section for the provider is null");

            if (String.IsNullOrEmpty(name))
                name = "StandardServiceAgentProvider";

            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description",
                    "Default ServiceAgentProvider");
            }

            base.Initialize(name, config);
        }

        public override T CreateServiceAgent<T>()
        {
            List<string> categories = new List<string>();

            ILogging log = new LoggingContext(categories);

            return CreateServiceAgent<T>(log);
        }

        public override T CreateServiceAgent<T>(ILogging logger)
        {
            object o = typeof(T);
            if (!logger.Categories.Contains("ServiceAgents"))
            {
                logger.Categories.Add("ServiceAgents");
            }
            logger.LogInformationMessage("Standard Service Agent Created", "Service Agent : " + o.ToString());
            return new T();
        }


    }
}
