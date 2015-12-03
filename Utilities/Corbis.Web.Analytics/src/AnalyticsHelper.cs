using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;
using System.Configuration.Provider;
using Corbis.WebAnalytics.Contracts.V1;
using Corbis.Web.Entities;



namespace Corbis.Web.Analytics
{

    /// <summary>
    /// Helper class for Web Analytics
    /// </summary>
    public static class AnalyticsHelper
    {


        /// <summary>
        /// Logs a web analytics event.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="page">The page to which the Omniture Javascript will be written.</param>
        /// <param name="eventData">The event data associated with the event in a query string format.</param>
        public static void LogWebAnalyticsEvent(AnalyticsLoggingLocation analyticsLoggingLocation, EventType eventType, Dictionary<string, string> eventData)
        {
            //We don't want web analytics to throw any errors.
            try
            {
                OmnitureAnalyticsProcessingProvider omnitureAnalyticsProcessingProvider = new OmnitureAnalyticsProcessingProvider();
                CorbisAnalyticsProcessingProvider corbisAnalyticsProcessingProvider = new CorbisAnalyticsProcessingProvider();

                switch (analyticsLoggingLocation)
                {
                    case AnalyticsLoggingLocation.Unknown:
                        break;
                    case AnalyticsLoggingLocation.OmnitureDataWarehouse:
                        omnitureAnalyticsProcessingProvider.ProcessAnalytics(eventType, eventData);
                        break;
                    case AnalyticsLoggingLocation.CorbisDataWarehouse:
                        corbisAnalyticsProcessingProvider.ProcessAnalytics(eventType, eventData);
                        break;
                    case AnalyticsLoggingLocation.All:
                        omnitureAnalyticsProcessingProvider.ProcessAnalytics(eventType, eventData);
                        corbisAnalyticsProcessingProvider.ProcessAnalytics(eventType, eventData);
                        break;
                    default:
                        break;
                }
            }
            catch
            {
            }
        }
    }

}