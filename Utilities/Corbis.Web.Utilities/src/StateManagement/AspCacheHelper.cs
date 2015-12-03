using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Corbis.Web.Utilities.StateManagement
{
    /// <summary>
    /// Class used to persist object in the ASP Cache
    /// </summary>
    internal class AspCacheHelper
    {
        private readonly HttpContextBase _httpContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionHelper"/> class.
        /// </summary>
        /// <param name="httpContext">The HTTP context for the session.</param>
        public AspCacheHelper(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        /// <summary>
        /// Gets the cache value.
        /// </summary>
        /// <typeparam name="T">
        /// The type of object to retrieve
        /// </typeparam>
        /// <param name="key">The key.</param>
        /// <returns>
        /// The value
        /// </returns>
        public T GetValue<T>(string key)
        {
            T retVal = default(T);
            TryGetValue<T>(key, out retVal);
            return retVal;
        }

        /// <summary>
        /// Tries to get session value.
        /// </summary>
        /// <typeparam name="T">
        /// The type of object to retrieve
        /// </typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if the value was succesfully retrieved, false otherwise</returns>
        public bool TryGetValue<T>(string key, out T value)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", "Key can't be null");
            }

            value = default(T);
            bool foundValue = false;
            if (_httpContext.Cache != null)
            {
                try
                {
                    value = (T)_httpContext.Cache[key];
                    foundValue = true;
                }
                catch { }
            }
            return foundValue;
        }

        /// <summary>
        /// Updates the session cache valuevalue. If the value doesn't exist, it is added.
        /// The value never expires
        /// </summary>
        /// <typeparam name="T">
        /// The type of object to retrieve
        /// </typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void UpdateValue<T>(string key, T value)
        {
            UpdateValue<T>(key, value, StatePersistenceDuration.NeverExpire, -1);
        }

        /// <summary>
        /// Updates the cache value. If the value doesn't exist, it is added
        /// </summary>
        /// <typeparam name="T">
        /// The type of object to retrieve
        /// </typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="duration">
        /// How long to cache the object. 
        /// Must be Sliding, Absolute or NeverExpire 
        /// </param>
        /// <param name="ticks">
        /// Indicates when to expire the cookie. 
        /// When duration is Sliding, this should be from a <see cref="System.TimeSpan"/>.
        /// When duration is Absolute, this should be from a <see cref="System.DateTime"/>.
        /// Ignored otherwise
        /// </param>
        public void UpdateValue<T>(
            string key, 
            T value, 
            StatePersistenceDuration duration, 
            long ticks)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", "Key can't be null");
            }

            if (!(duration == StatePersistenceDuration.Sliding ||
                  duration == StatePersistenceDuration.Absolute ||
                  duration == StatePersistenceDuration.NeverExpire))
            {
                throw new ArgumentException("duration is InvalidCastException", "duration");
            }

            switch (duration)
            {
                case StatePersistenceDuration.Sliding:
                    _httpContext.Cache.Insert(
                        key, 
                        value, 
                        null, 
                        System.Web.Caching.Cache.NoAbsoluteExpiration, 
                        new TimeSpan(ticks));
                    break;
                case StatePersistenceDuration.Absolute:
                    _httpContext.Cache.Insert(
                        key,
                        value,
                        null,
                        new DateTime(ticks),
                        System.Web.Caching.Cache.NoSlidingExpiration);
                    break;
                default:
                    _httpContext.Cache.Insert(key, value);
                    break;
            }
        }

        /// <summary>
        /// Deletes the session value.
        /// </summary>
        /// <param name="key">The key.</param>
        public void DeleteValue(string key)
        {
            _httpContext.Cache.Remove(key);
        }

		public void ClearCache()
		{
			foreach (DictionaryEntry item in _httpContext.Cache)
			{
				DeleteValue(item.Key.ToString());
			}
		}
    }
}
