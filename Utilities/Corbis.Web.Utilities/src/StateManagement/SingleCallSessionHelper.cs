using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

using Corbis.SessionState.ServiceAgents.V1;
using Corbis.SessionState.Contracts.V1;

namespace Corbis.Web.Utilities.StateManagement
{
    /// <summary>
    /// Class used to persist object in the Session.
    /// This class makes a single call to get the session from the database,
    /// Then makes updates in Memory. Method SaveAll() must be called to write the 
    /// data back to the database.
    /// </summary>
    internal class SingleCallSessionHelper : ISessionHelper
    {
        private readonly HttpContextBase _httpContext;
        private readonly HttpSessionStateBase _httpSession;
        private ISessionStateContract _sessionState;
        private readonly CookieHelper _cookieHelper;
        private readonly AspCacheHelper _cacheHelper;
        private const string _applicationName = "Corbis.Web";
        private const string _dataKey = "SessionData";
        private const string _versionKey = "SessionVersionUid";

        [ThreadStatic]
        private static Dictionary<string, object> _data = null;

        [ThreadStatic]
        private static bool _isDirty = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionHelper"/> class.
        /// </summary>
        /// <param name="httpContext">The HTTP context for the session.</param>
        public SingleCallSessionHelper(
            HttpContextBase httpContext,
            HttpSessionStateBase httpSession,
            CookieHelper cookieHelper,
            AspCacheHelper cacheHelper)
        {
            _httpContext = httpContext;
            _httpSession = httpSession;
            _sessionState = new SessionStateServiceAgent();
            _cookieHelper = cookieHelper;
            _cacheHelper = cacheHelper;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SessionHelper"/> class.
        /// For unit tests ONLY!
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="sessionState">SessionState contract</param>
        public SingleCallSessionHelper(
            HttpContextBase httpContext, 
            HttpSessionStateBase httpSession, 
            ISessionStateContract sessionState)
        {
            _httpContext = httpContext;
            _httpSession = httpSession;
            _sessionState = sessionState;
            _cookieHelper = new CookieHelper(_httpContext);
            _cacheHelper = new AspCacheHelper(_httpContext);
        }


        /// <summary>
        /// Gets the session value.
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
                throw new ArgumentNullException("name", "name can't be null");
            }
            
            value = default(T);
            bool foundValue = false;
            if (Data.ContainsKey(key))
            {
                try
                {
                    value = (T)Data[key];
                    foundValue = true;
                }
                catch { }
            }
            return foundValue;
        }

        /// <summary>
        /// Updates the session value. If the value doesn't exist, it is added
        /// </summary>
        /// <typeparam name="T">
        /// The type of object to retrieve
        /// </typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void UpdateValue<T>(string key, T value)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("name", "name can't be null");
            }

            if (Data.ContainsKey(key))
            {
                Data[key] = value;
            }
            else
            {
                Data.Add(key, value);
            }
            _isDirty = true;
        }

        /// <summary>
        /// Deletes the session value.
        /// </summary>
        /// <param name="key">The key.</param>
        public void DeleteValue(string key)
        {
            if (Data.ContainsKey(key))
            {
                Data.Remove(key);
                _isDirty = true;
            }
        }

		/// <summary>
		/// Clears all session values.
		/// </summary>
		public void ClearSession()
		{
            Data = new Dictionary<string, object>();
            _isDirty = true;
		}

        /// <summary>
        /// Persists data to the DB if any changes have been made, and Update the session data and versions as well.
        /// </summary>
        public void Persist()
        {
            if (_isDirty)
            {
                // Update the data in the DB and Cache
                if (_data != null && _data.Count > 0)
                {
                    byte[] valueBytes = SerializationUtility.ToBytes(_data);
                    _sessionState.UpdateItem(
                        _applicationName,
                        _httpSession.SessionID,
                        _dataKey,
                        valueBytes,
                        _httpSession.Timeout);
                }
                else
                {
                    _sessionState.RemoveItem(
                        _applicationName,
                        _httpSession.SessionID,
                        _dataKey);
                }

                Guid versionUid = Guid.NewGuid();
                _cookieHelper.UpdateSessionCookie<Guid>(_versionKey, versionUid);
                // Keep the data in the cache on a 20 minute sliding expiration
                _cacheHelper.UpdateValue<Guid>(
                    _httpSession.SessionID + _versionKey,
                    versionUid, 
                    StatePersistenceDuration.Sliding,
                    12000000000);
                _cacheHelper.UpdateValue<Dictionary<string, object>>(
                    _httpSession.SessionID + _dataKey, 
                    _data,
                    StatePersistenceDuration.Sliding,
                    12000000000);
            }
            // Always reset the dirty flag to false after a save
            _isDirty = false;
            _data = null;
        }

        #region private helper methods


        private Dictionary<string, object> Data
        {
            get
            {
                // if _data is null, check if we've got the correct version in the cache.
                // If so, get it from memory, otherwise make a service call to get it.
                // We need to use the cache instead of session as the Persist method gets called
                // on Application_EndRequest and the Session is no longer available
                if (_data == null)
                {
                    Guid cacheVersionUid = Guid.Empty;
                    Guid cookieVersionUid = Guid.Empty;
                    if (_cacheHelper.TryGetValue<Guid>(_httpSession.SessionID + _versionKey, out cacheVersionUid) &&
                        _cookieHelper.TryGetCookie<Guid>(_versionKey, out cookieVersionUid) &&
                        cookieVersionUid == cacheVersionUid)
                    {
                        // We've got the same version in the cache and cookie,
                        // try and get it out of the cache
                        if (!_cacheHelper.TryGetValue<Dictionary<string, object>>(_httpSession.SessionID + _dataKey, out _data))
                        {
                            // oh well, get it from the service
                            GetDataFromService();
                        }
                    }
                    else
                    {
                        // need to get it from the service
                        GetDataFromService();
                    }
                    // Synchronize the cache and cookie version uids if they're different
                }

                return _data;
            }
            set { _data = value; }
        }

        /// <summary>
        /// Tries to get session value from the service.
        /// </summary>
        /// <typeparam name="T">
        /// The type of object to retrieve
        /// </typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        private void GetDataFromService()
        {
            byte[] valueBytes = _sessionState.GetItem(
                _applicationName,
                _httpSession.SessionID,
                _dataKey,
                _httpSession.Timeout);

            if (valueBytes != null)
            {
                _data = SerializationUtility.ToObject(valueBytes) as Dictionary<string, object>;
            }
            if (_data == null)
            {
                _data = new Dictionary<string, object>();
            }
        }

        /// <summary>
        /// Gets the ASP session id, from Session if it's avaiable, 
        /// or from the cookie if it's not, such as when we call Persist
        /// </summary>
        /// <returns></returns>
        private string GetAspSessionId()
        {
            string sessionId = String.Empty;
            //if (_ht
            return sessionId;
        }

        #endregion

    }
}
