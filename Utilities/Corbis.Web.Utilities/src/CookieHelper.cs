using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Security;
using Corbis.Framework.Logging;
using Corbis.Web.Entities;

namespace Corbis.Web.Utilities
{
    /// <summary>
    /// Contains helper methods to read and write cookies from 
    /// current Http Context
    /// </summary>
    public static class CookieHelper
    {
        private static MethodInfo encodeMethod;
        private static MethodInfo decodeMethod;
        private static ILogging loggingContext;
        private static byte[] buffer;

        private static byte[] Buffer
        {
            get { return buffer; }
            set { buffer = value; }
        }

        private static ILogging LoggingContext
        {
            get
            {
                if (loggingContext == null)
                {
                    loggingContext = new LoggingContext(new string[] { "CookieHelper" });
                }
                return loggingContext;
            }
        }

        /// <summary>
        /// Setup for System.Web.Security.CookieProtectionHelper methods
        /// </summary>
        static CookieHelper()
        {
            Assembly systemWeb = typeof(HttpContext).Assembly;
            if (systemWeb == null)
            {
                throw new InvalidOperationException("CookieHelper: CookieHelper() - Unable to get assembly System.Web.");
            }

            Type cookieProtectionHelper = systemWeb.GetType("System.Web.Security.CookieProtectionHelper");
            if (cookieProtectionHelper == null)
            {
                throw new InvalidOperationException("CookieHelper: CookieHelper() - Unable to get type System.Web.Security.CookieProtectionHelper.");
            }

            encodeMethod = cookieProtectionHelper.GetMethod("Encode", BindingFlags.NonPublic | BindingFlags.Static);
            decodeMethod = cookieProtectionHelper.GetMethod("Decode", BindingFlags.NonPublic | BindingFlags.Static);

            if (encodeMethod == null || decodeMethod == null)
            {
                throw new InvalidOperationException("CookieHelper: CookieHelper() - Unable to get the methods to invoke.");
            }
        }

        public static string Decode(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    return text;
                }
                byte[] buffer = (byte[])decodeMethod.Invoke(null, new object[] { CookieProtection.All, text });
                return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            }
            catch
            {
                LoggingContext.LogWarningMessage("CookieHelper: Decode() - Unable to decode cookie.");
                return text;
            }
        }

        public static string Encode(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    return text;
                }
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                return (string)encodeMethod.Invoke(null, new object[] { CookieProtection.All, buffer, buffer.Length });
            }
            catch
            {
                LoggingContext.LogWarningMessage("CookieHelper: Encode() - Unable to encode cookie.");
                return text;
            }
        }

        /// <summary>
        /// Decode the specified cookie's value.
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static HttpCookie Decode(HttpCookie cookie)
        {
            if (cookie == null || StringHelper.IsNullOrTrimEmpty(cookie.Value))
            {
                return cookie;
            }

            try
            {
                HttpCookie decodedCookie = new HttpCookie(cookie.Name);
                foreach (string key in cookie.Values.AllKeys)
                {
                    decodedCookie[Decode(key)] = Decode(cookie[key]);
                }

                return decodedCookie;
            }
            catch (Exception ex)
            {
                LoggingContext.LogErrorMessage("CookieHelper: Decode() - An unknown exception has occurred.", ex);
                return cookie;
            }
        }

        /// <summary>
        /// Encode the specified cookie's value.
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static HttpCookie Encode(HttpCookie cookie)
        {
            if (cookie == null || StringHelper.IsNullOrTrimEmpty(cookie.Value))
            {
                return cookie;
            }

            try
            {
                HttpCookie encodedCookie = new HttpCookie(cookie.Name);
                foreach (string key in cookie.Values.AllKeys)
                {
                    encodedCookie[Encode(key)] = Encode(cookie[key]);
                }

                return encodedCookie;
            }
            catch (Exception ex)
            {
                LoggingContext.LogErrorMessage("CookieHelper: Encode() - An unknown exception has occurred.", ex);
                return cookie;
            }
        }

        /// <summary>
        /// Gets the HttpCookie corresponding to the supplied name
        /// </summary>
        /// <param name="cookieName">Cookie Name</param>
        /// <returns>HttpCookie if cookie present, else null</returns>
        public static HttpCookie Get(string name)
        {
            if (StringHelper.IsNullOrTrimEmpty(name))
            {
                throw new ArgumentNullException("CookieHelper: Get() - Cookie name cannot be empty.");
            }

            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(name);
            return Decode(cookie);
        }

        /// <summary>
        /// Gets the cookie value
        /// </summary>
        /// <param name="cookieName">Cookie Name</param>
        /// <returns></returns>
        public static string GetValue(string name)
        {
            string value = string.Empty;

            if (StringHelper.IsNullOrTrimEmpty(name))
            {
                throw new ArgumentNullException("CookieHelper: GetValue() - Cookie name cannot be empty.");
            }

            HttpCookie cookie = Get(name);
            if (cookie != null)
            {
                value = cookie.Value;
            }

            return value;
        }

        /// <summary>
        /// Gets the cookie value for the supplied cookie name and name/value pair key
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="contentKey"></param>
        /// <returns></returns>
        public static string GetValue(string name, string key)
        {
            string value = string.Empty;

            if (StringHelper.IsNullOrTrimEmpty(name))
            {
                throw new ArgumentNullException("CookieHelper: GetValue() - Cookie name cannot be empty.");
            }

            if (StringHelper.IsNullOrTrimEmpty(key))
            {
                throw new ArgumentNullException("CookieHelper: GetValue() - Content key cannot be empty.");
            }

            HttpCookie cookie = Get(name);
            if (cookie != null)
            {
                value = cookie.Values[key];
            }

            return value;
        }

        /// <summary>
        /// Sets a cookie (creates a new one if one is not present) into the current
        /// HttpContext's Response stream
        /// </summary>
        /// <param name="cookie">HttpCookie</param>
        public static void Set(HttpCookie cookie)
        {
            if (cookie == null)
            {
                throw new ArgumentNullException("CookieHelper: Set() - Cookie cannot be null.");
            }

            HttpCookie encodedCookie = Encode(cookie);
            HttpContext.Current.Response.Cookies.Add(encodedCookie);
        }

        /// <summary>
        /// Sets a cookie (creates a new one if one is not present) into the current
        /// HttpContext's Response stream
        /// </summary>
        /// <param name="cookieName">Name of the cookie</param>
        /// <param name="cookieValue">Cookie Value</param>
        /// <param name="domain">Domain of cookie</param>
        public static void SetValue(string name, string value, string domain)
        {
            if (StringHelper.IsNullOrTrimEmpty(name))
            {
                throw new ArgumentNullException("CookieHelper: SetValue() - Cookie name cannot be empty.");
            }

            HttpCookie cookie = Get(name);
            if (cookie == null)
            {
                cookie = new HttpCookie(name);
            }

            cookie.Domain = domain;
            cookie.Expires = DateTime.MaxValue;
            cookie.Value = value;

            Set(cookie);
        }

        /// <summary>
        /// Sets a cookie (creates a new one if one is not present) into the current
        /// HttpContext's Response stream
        /// </summary>
        /// <param name="cookieName">Name of the cookie</param>
        /// <param name="cookieKey">Cookie Key</param>
        /// <param name="cookieValue">Cookie Value</param>
        /// <param name="domain">Domain of cookie</param>
        public static void SetValue(string name, string key, string value, string domain)
        {
            if (StringHelper.IsNullOrTrimEmpty(name))
            {
                throw new ArgumentNullException("CookieHelper: SetValue() - Cookie name cannot be empty.");
            }

            HttpCookie cookie = Get(name);
            if (cookie == null)
            {
                cookie = new HttpCookie(name);
            }

            cookie.Domain = domain;
            cookie.Expires = DateTime.MaxValue;
            cookie[key] = value;

            Set(cookie);
        }

        /// <summary>
        /// Sets a cookie (creates a new one if one is not present) into the current
        /// HttpContext's Response stream
        /// </summary>
        /// <param name="cookieName">Name of the cookie</param>
        /// <param name="values">List of Name/Value pairs to be stored in the cookie</param>
        /// <param name="domain">Domain of cookie</param>
        public static void SetValues(string name, NameValueCollection values, string domain)
        {
            if (StringHelper.IsNullOrTrimEmpty(name))
            {
                throw new ArgumentNullException("CookieHelper: SetValues() - Cookie name cannot be empty.");
            }

            HttpCookie cookie = Get(name);
            if (cookie == null)
            {
                cookie = new HttpCookie(name);
            }

            for (int i = 0; i < values.Count; ++i)
            {
                cookie[values.GetKey(i)] = values.Get(i);
            }

            cookie.Domain = domain;
            cookie.Expires = DateTime.MaxValue;

            Set(cookie);
        }

        /// <summary>
        /// Invalidates the cookie
        /// </summary>
        /// <param name="cookieName">Cookie Name</param>
        /// <param name="domain">Domain of cookie</param>
        public static void Invalidate(string name, string domain)
        {
            if (StringHelper.IsNullOrTrimEmpty(name))
            {
                throw new ArgumentNullException("CookieHelper: Invalidate() - Cookie name cannot be empty.");
            }

            HttpCookie cookie = Get(name);
            if (cookie != null)
            {
                // save the current culture name
                string languageCode = cookie[Cookies.Profile.KEY_LANGUAGE_CODE];
                cookie.Value = string.Empty;
                if (!StringHelper.IsNullOrTrimEmpty(languageCode))
                {
                    cookie[Cookies.Profile.KEY_LANGUAGE_CODE] = languageCode;
                }
                cookie.Domain = domain;
                cookie.Expires = DateTime.MinValue;

                Set(cookie);
            }
        }
    }
}
