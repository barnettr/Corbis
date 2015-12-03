using System;
using System.Collections.Generic;
using System.Configuration;
using Corbis.WebAnalytics.Contracts.V1;
using Corbis.WebAnalytics.ServiceAgents.V1;
using Corbis.Web.Entities;

namespace Corbis.Web.Analytics
{

    /// <summary>
    /// Summary description for CorbisAnalyticsProcessingProvider
    /// </summary>
    public class CorbisAnalyticsProcessingProvider : AnalyticsProcessingProvider
    {
        /// <summary>
        /// CorbisAnalyticsProcessingProvider constructor
        /// </summary>
        public CorbisAnalyticsProcessingProvider()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Process Analytics
        /// </summary>
        public override void ProcessAnalytics(EventType eventType, Dictionary<string, string> eventData)
        {
            // check App settings to see if custom analytics has been turned off. If the setting is not specified, the default is Enabled.
            string enableCorbisAnalytics = ConfigurationManager.AppSettings["EnableCorbisAnalytics"];
            if (!string.IsNullOrEmpty(enableCorbisAnalytics) && string.Compare("false", enableCorbisAnalytics, true) == 0)
                return;

            DateTime eventDateTime = DateTime.Now;
            string userId = "test";
            
            WebAnalyticsServiceAgent webAnalyticsServiceAgent = new WebAnalyticsServiceAgent();
            webAnalyticsServiceAgent.LogEvent(eventType, userId, eventDateTime, eventData);
        }
    }
}