using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;

namespace Corbis.Web.Utilities
{

    /// <summary>
    /// Class used to get the IP address of the client either from the X-Forwarded-For HttpHeader
    /// or from the UserHostAddress if the X-Forwarded-For HttpHeader doesn't exist
    /// </summary>
    public static class ClientIPHelper
    {
        public const string XForwardedForHeaderName = "X-Forwarded-For";
        public const string MockIpCookieName = "MockIpAddress";

        /// <summary>
        /// Gets the IP address of the client either from the X-Forwarded-For HttpHeader
        /// or from the UserHostAddress if the X-Forwarded-For HttpHeader doesn't exist
        /// </summary>
        /// <returns>
        /// The ClientIp Address
        /// </returns>
        public static String GetClientIpAddress()
        {
            string clientIp = HttpContext.Current.Request.UserHostAddress;
            try
            {
                string mockIp = string.Empty;
                string environment = ConfigurationManager.AppSettings["Environment"];
                if (ConfigurationManager.AppSettings["AllowIpMocking"] == "True" &&
                    !String.IsNullOrEmpty(environment) &&
                    (environment.StartsWith("DEV") ||
                     environment.StartsWith("SQA") ||
                     environment == "STG")
                   )
                {
                    StateManagement.StateItemCollection stateItems = 
                        new StateManagement.StateItemCollection(HttpContext.Current);
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString[MockIpCookieName]))
                    {
                        clientIp = mockIp = HttpContext.Current.Request.QueryString[MockIpCookieName];
                        StateManagement.StateItem<string> mockIpCookie = new StateManagement.StateItem<string>(
                            ClientIPHelper.MockIpCookieName,
                            null,
                            mockIp,
                            StateManagement.StateItemStore.Cookie,
                            StateManagement.StatePersistenceDuration.Session);
                        stateItems.SetStateItem<string>(mockIpCookie);
                    }
                    else if (stateItems.TryGetStateItemValue<string>(
                            MockIpCookieName, null,
                            StateManagement.StateItemStore.Cookie,
                            out mockIp))
                    {
                        clientIp = mockIp;
                    }
                }
                if (String.IsNullOrEmpty(mockIp) && 
                    String.IsNullOrEmpty(HttpContext.Current.Request.Headers[XForwardedForHeaderName]))
                {
                    string[] ips = HttpContext.Current.Request.Headers[XForwardedForHeaderName].Split(new char[] { ',' });
                    clientIp = ips[0];
                }
            }
            catch { }
            return clientIp;
        }
    }
}
