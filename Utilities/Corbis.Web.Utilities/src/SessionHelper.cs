using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using Corbis.Web.Entities;
using System.Web.Configuration;
using System.Configuration;

namespace Corbis.Web.Utilities
{

    public enum SessionKeys
    {
        CultureName,
        Theme,
        CurrentCulture
    }

    /// <summary>
    /// Provides some helper methods to store and retrieve values
    /// from Session and Cache
    /// </summary>
    public static class SessionHelper
    {
        #region Session

        /// <summary>
        /// Saves a value to current user's Session
        /// </summary>
        /// <param name="sessionItemName">Strongly type name of the session variable</param>
        /// <param name="sessionItemValue">Session Value</param>
        public static void SaveToSession(SessionKeys sessionItemName, object sessionItemValue)
        {
            //save to session
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Session[sessionItemName.ToString()] = sessionItemValue;
            }
        }

        /// <summary>
        /// Removes a value from current user's Session
        /// </summary>
        /// <param name="sessionItemName">Strongly type name of the session variable</param>
        public static void RemoveFromSession(SessionKeys sessionItemName)
        {
            //remove item from Session
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Session.Remove(sessionItemName.ToString());
            }
        }

        /// <summary>
        /// Retrieves a value from current user's Session
        /// </summary>
        /// <param name="sessionItemName">Strongly type name of the session variable</param>
        /// <returns>Session value which can be a value type or a reference type</returns>
        public static object RetrieveSession(SessionKeys sessionItemName) 
        {
            object result = null;

            if(HttpContext.Current != null)
            {
                result = HttpContext.Current.Session[sessionItemName.ToString()];                
            }

            return result;
        }

        #endregion



    }
}
