using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using Corbis.Framework.Logging;

namespace Corbis.Web.Utilities.Providers
{
    public abstract class ServiceAgentProvider : ProviderBase
    {
        /// <summary>
        /// By Default this should just return the service agent that has been requested in the generic
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public abstract T CreateServiceAgent<T>() where T : new();

        public abstract T CreateServiceAgent<T>(ILogging logger) where T : new();




    }
}
