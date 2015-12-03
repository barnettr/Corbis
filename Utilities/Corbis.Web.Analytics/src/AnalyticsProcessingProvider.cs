using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Provider;
using Corbis.WebAnalytics.Contracts.V1;
using Corbis.Web.Entities;

namespace Corbis.Web.Analytics
{
    /// <summary>
    /// Summary description for AnalyticsProcessingProvider
    /// </summary>
    public abstract class AnalyticsProcessingProvider
    {
        /// <summary>
        /// AnalyticsProcessingProvider constructor
        /// </summary>
        public AnalyticsProcessingProvider()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// ProcessAnalytics
        /// </summary>
        public abstract void ProcessAnalytics(EventType eventType, Dictionary<string, string> eventData);
    }
}