using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using System.Web.Configuration;
using System.Collections.Specialized;

namespace Corbis.Web.Utilities.StateManagement
{
    internal class CookieHelper
    {
        /// <summary>
        /// Key used to store the # of ticks in a sliding Expiration Cookie
        /// </summary>
        public static string SlidingExpirationKey = "SlidingExpiration";
        private static string CheckSumCookieName = "CheckSums";
        public static string ASP_SessionCookieName = "ASP.NET_SessionId";
        public static string DirectlyManipulatedSearch = "UserSearchOptions";

        private readonly HttpContextBase _httpContext;

        public CookieHelper(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }


        /// <summary>
        /// Standard function for retrieving the value of a cookie or the sub value of keyed
        /// cookie.
        /// </summary>
        /// <param name="name">The name of the cookie</param>
        /// <param name="key">The name of the cookie key.</param>
        /// <returns>The value indicated by the key, taken from the cookie name passed in.  If no
        /// such cookie exists in the cookies collection an empty string is returned ("")</returns>
        /// <remarks>If key is null then it returns the value of the cookie.</remarks>
        private string GetCookieValue(string name, string key)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "name can't be null");
            }
            
            string returnValue = "";

            HttpCookie cookie = null;
            if (ResponseCookieExists(name))
            {
                cookie = _httpContext.Response.Cookies[name];
            }
            else if (RequestCookieExists(name))
            {
                cookie = _httpContext.Request.Cookies[name];
            }

            if (cookie == null || !ValidateCheckSum(cookie))
            {
                return String.Empty;
            }
            else
            {
                if (key != null)
                {
                    returnValue = cookie.Values[key];
                }
                else
                {
                    returnValue = cookie.Values[0];
                }
                // Check if it's a sliding expiration cookie and update the expiration if it is
                long ticks;
                if (long.TryParse(cookie.Values[SlidingExpirationKey], out ticks))
                {
                    DetermineExpiration(cookie, StatePersistenceDuration.Sliding, ticks);
                }
            }

            if (returnValue == null)
            {
                return String.Empty;
            }
            else
            {
                return DecodeCookie(returnValue);
            }
        }

        /// <summary>
        /// Tries to get a cookie value
        /// </summary>
        /// <typeparam name="T">
        /// the type of object to try and retrieve
        /// </typeparam>
        /// <param name="name">The name of the cookie</param>
        /// <returns>
        /// true if the cookie was found and successfully cast to the proper type
        /// </returns>
        public bool TryGetCookie<T>(string name, out T value)
        {
            return TryGetCookie<T>(name, null, out value);
        }

        /// <summary>
        /// Tries to get a cookie value
        /// </summary>
        /// <typeparam name="T">
        /// the type of object to try and retrieve
        /// </typeparam>
        /// <param name="name">The name of the cookie</param>
        /// <param name="key">The name of the cookie key.</param>
        /// <returns>
        /// true if the cookie was found and successfully cast to the proper type
        /// </returns>
        public bool TryGetCookie<T>(string name, string key, out T value)
        {
            bool gotCookie = false;
            value = default(T);
            string serializedObject = GetCookieValue(name, key);
            if (serializedObject.Length > 0)
            {
                // Found the cookie, try and get the value
                try
                {
                    value = Serializer.DeserializeObject<T>(serializedObject);
                    gotCookie = true;
                }
                catch { }
            }
            return gotCookie;
        }

        /// <summary>
        /// Gets the value of a cookie
        /// </summary>
        /// <param name="name">The name of the cookie</param>
        /// <returns>The de-serialized object found in the cookie or key.  If no
        /// such cookie exists the Default value is returned.</returns>
        public T GetCookie<T>(String name)
        {
            return GetCookie<T>(name, null);
        }

        /// <summary>
        /// Gets the value of a cookie
        /// </summary>
        /// <param name="name">The name of the cookie</param>
        /// <param name="key">The name of the cookie key</param>
        /// <returns>The de-serialized object found in the cookie or key.  If no
        /// such cookie exists the Default value is returned.</returns>
        public T GetCookie<T>(String name, String key)
        {
            T value = default(T);
            // Get the cookie value
            String serializedObject = GetCookieValue(name, key);
            if (!String.IsNullOrEmpty(serializedObject))
            {
                // Cookie was found, de-serialize and return the object
                value = Serializer.DeserializeObject<T>(serializedObject);
            }
            return value;
        }


        private static void SetCookieDomain(HttpCookie cookie)
        {
            System.Web.Configuration.HttpCookiesSection httpCookiesSection =
                (System.Web.Configuration.HttpCookiesSection)WebConfigurationManager.GetSection("system.web/httpCookies");
            cookie.Domain = httpCookiesSection.Domain;
        }

        /// <summary>
        /// Updates a session cookie using the standard domain.
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="name">Name of the cookie.</param>
        /// <param name="cookieValue">The cookie value.</param>
        public void UpdateSessionCookie<T>(string name, T value)
        {
            UpdateCookie<T>(
                name,
                null,
                value,
                StatePersistenceDuration.Session,
                -1);
        }
        
        /// <summary>
        /// Updates a session cookie using the standard domain.
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="name">Name of the cookie.</param>
        /// <param name="key">The cookie key.</param>
        /// <param name="cookieValue">The cookie value.</param>
        public void UpdateSessionCookie<T>(string name, string key, T value)
        {
            UpdateCookie<T>(
                name,
                key,
                value,
                StatePersistenceDuration.Session,
                -1);
        }


        /// <summary>
        /// Updates the cookie.
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="name">Name of the cookie.</param>
        /// <param name="key">The cookie key.</param>
        /// <param name="cookieValue">The cookie value.</param>
        /// <param name="duration">
        /// How long to persist the cookie
        /// </param>
        /// <param name="ticks">
        /// Indicates when to expire the cookie. 
        /// When duration is Sliding, this should be from a <see cref="System.TimeSpan"/>.
        /// When duration is Absolute, this should be from a <see cref="System.DateTime"/>.
        /// Ignored otherwise
        /// </param>
        public void UpdateCookie<T>(
            string name, 
            string key, 
            T value, 
            StatePersistenceDuration duration,
            long ticks)
        {

            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "name can't be null");
            }
            
            HttpCookie cookie;

            if (ResponseCookieExists(name))
            {
                cookie = _httpContext.Response.Cookies[name];
            }
            else if (RequestCookieExists(name))
            {
                cookie = _httpContext.Request.Cookies[name];
                _httpContext.Response.Cookies.Add(cookie);
            }
            else
            {
                cookie = new HttpCookie(name);
                _httpContext.Response.Cookies.Add(cookie);
            }

            string serializedValue = Serializer.SerializeObject(value);
            if (key == null)
            {
                cookie.Value = EncodeCookie(serializedValue);
            }
            else
            {
                cookie.Values[key] = EncodeCookie(serializedValue);
            }

            string culture;
            if (cookie.Name == ProfileKeys.Name && key == ProfileKeys.UsernameStateItemKey &&
                    !TryGetCookie<string>(ProfileKeys.Name, ProfileKeys.CultureStateItemKey, out culture))
            {
                UpdateCookie<String>(
                    ProfileKeys.Name,
                    ProfileKeys.CultureStateItemKey,
                    Corbis.Framework.Globalization.Language.CurrentLanguage.LanguageCode,
                    StatePersistenceDuration.NeverExpire,
                    0);
            }

            SetCookieDomain(cookie);
            DetermineExpiration(cookie, duration, ticks);

            // Don't set the Checksum on the CheckSum cookie or your asking for a stack overflow
            if (name != CheckSumCookieName)
            {
                SetCookieCheckSum(cookie);
            }
        }

        /// <summary>
        /// Deletes an entire cookie, or just a sub-cookie of a multi value cookie.
        /// </summary>
        /// <param name="name">The name of the cookie.</param>
        public void DeleteCookie(string name)
        {
            DeleteCookie(name, null);
        }
 

        /// <summary>
        /// Deletes an entire cookie, or just a sub-cookie of a multi value cookie.
        /// </summary>
        /// <param name="name">The name of the cookie.</param>
        /// <param name="key">The cookie key to delete.</param>
        /// <remarks>If key is null then the entire cookie is deleted.</remarks>
        public void DeleteCookie(string name, string key)
        {
            HttpCookie cookie;

            // If the cookie wasn't sent in the request then it doesn't exist, so
            // sending a Set-Cookie header would be a waste of bandwidth.
            if (!RequestCookieExists(name))
            {
                return;
            }

            if (ResponseCookieExists(name))
            {
                // Get the currently existing response cookie so we don't have two cookies being sent
                cookie = _httpContext.Response.Cookies[name];
            }
            else
            {
                // We could copy the request cookie, but the value no longer has "value" so
                // we'll just create a new object and add it.
                cookie = new HttpCookie(name);
                _httpContext.Response.Cookies.Add(cookie);
            }

            if ((key != null) && (cookie.HasKeys))
            {
                // Remove the value from the cookie. The cookie will be sent back to the client
                // without this key now.
                cookie.Values.Remove(key);
                SetCookieCheckSum(cookie);
            }
            else
            {
                // Expire the whole cookie so the client will remove it.
                DetermineExpiration(
                    cookie, 
                    StatePersistenceDuration.Absolute, 
                    DateTime.Now.AddYears(-5).Ticks);
            }
        }


        /// <summary>
        /// Iterates through the cookie collection and expires them ALL (with the exception of the Bet
        /// Authorization cookie noted below
        /// </summary>
		public void ClearCookies()
		{
			foreach (string cookieName in _httpContext.Request.Cookies.AllKeys)
			{
                //The Beta Site authorization scheme uses a cookie to indicate the user has been authorized to use
                //the beta.  We need to preserve this cookie (if it exists) when a user logs in so they don't
                //get sent back to the beta log in splash page (beta.aspx).  Sure it smells like a hack, but what
                //can you do?
                //Darren Davis 10/16/08

                if (cookieName == BetaSiteAuthentication.BetaAccessCookieName ||
                    cookieName == ClientIPHelper.MockIpCookieName ||
                    cookieName == CheckSumCookieName || cookieName == ASP_SessionCookieName || cookieName == DirectlyManipulatedSearch)
                {
                    continue;
                }

				HttpCookie cookie = new HttpCookie(cookieName);
				_httpContext.Response.Cookies.Add(cookie);
				DetermineExpiration(cookie, StatePersistenceDuration.Absolute, DateTime.Now.AddYears(-5).Ticks);
			}
		}

        /// <summary>
        /// Safe check to see if the cookie already exists in the response cookie
        /// collection.  This type of checking will not result in empty cookies
        /// being inadvertently created (as in the case of checking for the existence 
        /// of Response.Cookies[name] directly.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Boolean ResponseCookieExists(string name)
        {
            return CookieExists(name, _httpContext.Response.Cookies);
        }

        /// <summary>
        /// Safe check to see if the cookie already exists in the response cookie
        /// collection.  This type of checking will not result in empty cookies
        /// being inadvertently created (as in the case of checking for the existence 
        /// of Request.Cookies[name] directly.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Boolean RequestCookieExists(string name)
        {
            return CookieExists(name, _httpContext.Request.Cookies);
        }

        /// <summary>
        /// Utility function used by ResponseCookieExists and RequestCookieExists. It iterates
        /// through a provided cookie collection and looks for a cookie with the provided name.
        /// </summary>
        /// <param name="name">The name of the cookie to look for.</param>
        /// <param name="cookieCollection">The cookie collection to search within.</param>
        /// <returns>True if the cookie exists in the collection.</returns>
        private static Boolean CookieExists(string name, HttpCookieCollection cookieCollection)
        {
            foreach (String checkname in cookieCollection.AllKeys)
            {
                if (checkname == name)
                {
                    return true;
                }
            }
            return false;
        }


        public static string EncodeCookie(string valueIn, Encoding encode)
        {
            return HttpUtility.UrlEncode(valueIn, encode);
        }

        public static string EncodeCookie(string valueIn)
        {
            //We need to replace the "-" character with %2D so it matches
            //the escaped value from the ASP side.
            string returnValue = HttpUtility.UrlEncode(valueIn);
            if (returnValue != null & returnValue != string.Empty)
            {
                returnValue = returnValue.Replace("-", "%2D").Replace("_", "%5F");
            }
            return returnValue;
        }

        public static string DecodeCookie(string valueIn, Encoding encode)
        {
            return HttpUtility.UrlDecode(valueIn, encode);
        }

        public static string DecodeCookie(string valueIn)
        {
            //No need to handle the decode of the "%2D" value.  The UrlDecode
            //method handles 
            return HttpUtility.UrlDecode(valueIn);
        }

        private void DetermineExpiration(HttpCookie cookie, StatePersistenceDuration duration, long ticks)
        {
            DateTime expirationDate = DateTime.MinValue;
            cookie.Values.Remove(SlidingExpirationKey);
            switch (duration)
            {
                case StatePersistenceDuration.Sliding:
                    expirationDate = DateTime.Now.Add(new TimeSpan(ticks));
                    cookie.Values.Add(SlidingExpirationKey, ticks.ToString());
                    break;
                case StatePersistenceDuration.Absolute:
                    expirationDate = new DateTime(ticks);
                    break;
                case StatePersistenceDuration.NeverExpire:
                    expirationDate = DateTime.Now.AddYears(5);
                    break;
                default:
                    // N/A
                    break;
            }
            cookie.Expires = expirationDate;
        }

        private void SetCookieCheckSum(HttpCookie cookie)
        {
            String checkSum = CheckSum.ComputeCheckSum(cookie.Value);
            UpdateCookie<string>(
                CheckSumCookieName,
                cookie.Name,
                checkSum,
                StatePersistenceDuration.NeverExpire,
                0);
        }

        private bool ValidateCheckSum(HttpCookie cookie)
        {
            // Just return true if we're trying to get the CheckSum cookie
            if (cookie.Name == CheckSumCookieName || cookie.Name == DirectlyManipulatedSearch) { return true; }

            bool isValid = false;
            String savedCheckSum;
            if (TryGetCookie<string>(CheckSumCookieName, cookie.Name, out savedCheckSum))
            {
                isValid = CheckSum.ValidateCheckSum(cookie.Value, savedCheckSum);
            }

            return isValid;
        }

    }
}
