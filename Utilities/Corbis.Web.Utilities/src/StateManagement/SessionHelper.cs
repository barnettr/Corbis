using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

using Corbis.SessionState.ServiceAgents.V1;
using Corbis.SessionState.Contracts.V1;

namespace Corbis.Web.Utilities.StateManagement
{
    /// <summary>
    /// Class used to persist object in the Session
    /// </summary>
    internal class SessionHelper
    {
        private readonly HttpContextBase _httpContext;
        private readonly HttpSessionStateBase _httpSession;
        private ISessionStateContract _sessionState;
        private const string _applicationName = "Corbis.Web";

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionHelper"/> class.
        /// </summary>
        /// <param name="httpContext">The HTTP context for the session.</param>
        public SessionHelper(HttpContextBase httpContext, HttpSessionStateBase sessionBase)
            : this(httpContext, sessionBase, new SessionStateServiceAgent())
        { }


        /// <summary>
        /// Initializes a new instance of the <see cref="SessionHelper"/> class.
        /// For unit tests ONLY!
        /// </summary>
        /// <param name="hhtpContext">The HHTP context.</param>
        /// <param name="sessionState">SessionState contract</param>
        public SessionHelper(HttpContextBase httpContext, HttpSessionStateBase httpSession, ISessionStateContract sessionState)
        {
            _httpContext = httpContext;
            _httpSession = httpSession;
            _sessionState = sessionState;
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
            
            System.Diagnostics.Debug.WriteLine(string.Format("GetItem: {0},{1},{2},{3}", _applicationName, _httpSession.SessionID,key,_httpSession.Timeout));

            value = default(T);
            bool foundValue = false;
            byte[] valueBytes = _sessionState.GetItem(
                _applicationName,
                _httpSession.SessionID,
                key,
                _httpSession.Timeout);

            if (valueBytes != null)
            {
                try
                {
                    value = (T)SerializationUtility.ToObject(valueBytes);
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

            byte[] valueBytes = SerializationUtility.ToBytes(value);
            _sessionState.UpdateItem(
                _applicationName,
                _httpSession.SessionID,
                key,
                valueBytes,
                _httpSession.Timeout);

        }

        /// <summary>
        /// Deletes the session value.
        /// </summary>
        /// <param name="key">The key.</param>
        public void DeleteValue(string key)
        {
            _sessionState.RemoveItem(
                _applicationName,
                _httpSession.SessionID,
                key);
        }

		/// <summary>
		/// Clears all session values.
		/// </summary>
		public void ClearSession()
		{
			_httpSession.Clear();
		}
    }
}
