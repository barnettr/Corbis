using System;
using System.Web;
using System.Text;
using Corbis.Framework.Globalization;
using Corbis.Framework.Logging;

namespace Corbis.Web.Utilities
{
    /// <summary>
    /// Contains helper methods related to localization of data
    /// on the Web UI.
    /// </summary>
    public static class LocalizationHelper
    {
        private static LoggingContext loggingContext;

        public static ILogging LoggingContext
        {
            get
            {
                if (loggingContext == null)
                {
                    loggingContext = new LoggingContext(new string[] { "LocalizationHelper" });
                }
                return loggingContext;
            }
        }

        /// <summary>
        /// Helper method to get the locazlied value of a given resource key. This
        /// method calls the Localization web service to get the localized values.
        /// </summary>
        /// <param name="resourceCategory">Resource Category under which we can find the resource key</param>
        /// <param name="resourceKey">Resource Key for getting its associated localized value</param>
        /// <returns>Localized value, if resource found, Empty otherwise</returns>
        /// <remarks>Since this method tries to communicate with the Localization web service, 
        /// there are chances of exceptions occurring due to several factors. Incase an exception is thrown
        /// we return a empty string.
        /// </remarks>
        public static string GetLocalizedValue(string resourceCategory, string resourceKey)
        {
            string localizedValue = string.Empty;

            try
            {
                #region Validate Parameters

                if (StringHelper.IsNullOrTrimEmpty(resourceCategory) || 
                            StringHelper.IsNullOrTrimEmpty(resourceKey))
                {
                    //TODO: WRITE A HELPER METHOD TO COLLECT LIST OF ALL PARAMETERS (FOR A METHOD ACCEPTING
                    // MULTIPLE PARAMETERS) THAT ARE NULL AND THEN THROW THE EXCEPTION.
                    throw new ArgumentNullException(resourceCategory); 
                }

                #endregion

                //get the localized value from the localization service
                object localizedObject = HttpContext.GetGlobalResourceObject(resourceCategory, resourceKey,
                                           Language.CurrentCulture);

                if (localizedValue != null)
                {
                    localizedValue = localizedObject.ToString();
                }

            }
            catch(Exception ex)
            {
                #region Build Error Message
		 
	            StringBuilder sbErrorMessage = new StringBuilder();
                sbErrorMessage.AppendLine("Error occurred while trying to retrieve localized values for the following input parameters:");
                sbErrorMessage.Append("Resource Category: ");
                sbErrorMessage.AppendLine(resourceCategory);
                sbErrorMessage.Append("Resource Key: ");
                sbErrorMessage.AppendLine(resourceKey);

                #endregion

                //log the message now
                LoggingContext.LogErrorMessage(sbErrorMessage.ToString(),
                                ex.ToString());
            }

            return localizedValue;
        }


    }
}
