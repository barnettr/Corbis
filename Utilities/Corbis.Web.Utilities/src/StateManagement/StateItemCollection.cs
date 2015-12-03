using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web;
using System.Runtime.Serialization;
using Corbis.SessionState.Contracts.V1;

namespace Corbis.Web.Utilities.StateManagement
{
    public class StateItemCollection
    {
        #region Private Static Variables
        [ThreadStatic]
        private static HttpContextBase _httpContext;

        [ThreadStatic]
        private static CookieHelper _cookieHelper;

        [ThreadStatic]
        private static ISessionHelper _sessionHelper;

        [ThreadStatic]
        private static AspCacheHelper _aspCacheHelper;

        #endregion Private Static Variables
        
        #region Constructor

        /// <summary>
        /// Constructor for StateItemCollectionBase
        /// </summary>
        /// <param name="httpContext">
        /// Needed for interaction with cookies in the Request/Response stream
        /// </param>
        public StateItemCollection(HttpContext httpContext)
        {
            if (_httpContext == null)
            {
                _httpContext = new System.Web.HttpContextWrapper(httpContext);
                _cookieHelper = new CookieHelper(_httpContext);
                _aspCacheHelper = new AspCacheHelper(_httpContext);
                
                _sessionHelper =
                    //new MultiCallSessionHelper(_httpContext, new System.Web.HttpSessionStateWrapper(httpContext.Session));
                    new SingleCallSessionHelper(
                        _httpContext,
                        new System.Web.HttpSessionStateWrapper(httpContext.Session),
                        _cookieHelper,
                        _aspCacheHelper);
            }
        }

        /// <summary>
        /// Constructor for StateItemCollectionBase
        /// </summary>
        /// <param name="httpContext">
        /// Needed for interaction with cookies in the Request/Response stream
        /// </param>
        public StateItemCollection(HttpContextBase httpContext)
        {
            if (_httpContext == null)
            {
                _httpContext = httpContext;
                _cookieHelper = new CookieHelper(_httpContext);
                _aspCacheHelper = new AspCacheHelper(_httpContext);
                _sessionHelper =
                //new MultiCallSessionHelper(_httpContext, _httpContext.Session);
                new SingleCallSessionHelper(
                    _httpContext,
                    _httpContext.Session,
                    _cookieHelper,
                    _aspCacheHelper);
            }
        }

        /// <summary>
        /// Constructor used for unit testing. Add any new test projects to
        /// </summary>
        /// <remarks>
        /// Add the following line to the AssembyInfo.cs file for any projects using this:
        /// [assembly: InternalsVisibleTo("Corbis.Web.Utilities.Tests")], using the fully 
        /// qualified assembly name
        /// </remarks>
        internal StateItemCollection(HttpContextBase httpContext, ISessionStateContract sessionState)
        {
            _httpContext = httpContext;
            _cookieHelper = new CookieHelper(_httpContext);
            _sessionHelper = new SingleCallSessionHelper(_httpContext, httpContext.Session, sessionState);
            _aspCacheHelper = new AspCacheHelper(_httpContext);
        }

        #endregion Constructor

        #region Public Static Methods
        /// <summary>
        /// Run this on Application_BeginRequest to clear the StateItemCollection from the local thread
        /// </summary>
        public static void BeginRequest()
        {
            _httpContext = null;
        }

        /// <summary>
        /// Run this on Application_EndRequest to clear the StateItemCollection from the local thread
        /// </summary>
        public static void EndRequest()
        {
            if (_sessionHelper != null)
            {
                _sessionHelper.Persist();
            }
            _httpContext = null;
        }
        #endregion Public Static Methods

        #region Public Instance Methods

        /// <summary>
        /// Populates the properties of an object that have the <see cref="StateItemDescAttribute"/> attribute
        /// </summary>
        /// <param name="objectToLoad">The object for which to attempt to load the properties for</param>
        /// <returns>A list of the property names that were not found in state</returns>
        public List<string> PopulateObjectFromState(object objectToLoad)
        {

            if (objectToLoad == null)
            {
                throw new ArgumentNullException("objectToLoad");
            }

            List<string> propertiesNotLoaded = new List<string>();

            PropertyInfo[] objectProperties = objectToLoad.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in objectProperties)
            {
                object[] stateItemAtts = propertyInfo.GetCustomAttributes(typeof(StateItemDescAttribute), true);
                if (stateItemAtts.Length > 0 && stateItemAtts[0] is StateItemDescAttribute)
                {
                    if (!propertyInfo.CanWrite)
                    {
                        throw new ApplicationException(String.Format("Property {0} must be writable!", propertyInfo.Name));
                    }
                    
                    StateItemDescAttribute att = stateItemAtts[0] as StateItemDescAttribute;
                    
                    Type[] genericArgs = new Type[] { propertyInfo.PropertyType };
                    MethodInfo mi = this.GetType().GetMethod("TryGetStateItemValue");
                    MethodInfo genericMi = mi.MakeGenericMethod(genericArgs);
                    object stateItemValue = null;
                    object[] genericMiParams =  new object[] { att.Name, att.Key, att.Store, stateItemValue };
                    bool foundStateItem = (bool) genericMi.Invoke(this, genericMiParams);

                    if (foundStateItem)
                    {
                        stateItemValue = genericMiParams[3];
                        propertyInfo.SetValue(objectToLoad, stateItemValue, null);
                    }
                    else
                    {
                        propertiesNotLoaded.Add(propertyInfo.Name);
                    }
                }
            }
            return propertiesNotLoaded;
        }

        public void SaveObjectToState(object objectToSave)
        {
            if (objectToSave == null)
            {
                throw new ArgumentNullException("objectToSave");
            }

            PropertyInfo[] objectProperties = objectToSave.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in objectProperties)
            {
                object[] stateItemAtts = propertyInfo.GetCustomAttributes(typeof(StateItemDescAttribute), true);
                if (stateItemAtts.Length > 0 && stateItemAtts[0] is StateItemDescAttribute)
                {
                    if (!propertyInfo.CanRead)
                    {
                        throw new ApplicationException(String.Format("Property {0} must be readable!", propertyInfo.Name));
                    }

                    StateItemDescAttribute att = stateItemAtts[0] as StateItemDescAttribute;

                    Type stateItemType = typeof(StateItem<>);
                    Type[] genericArgs = new Type[] { propertyInfo.PropertyType };
                    Type constructedType = stateItemType.MakeGenericType(genericArgs);
                    object[] constructorParams =
                        new object[] { att.Name, att.Key, propertyInfo.GetValue(objectToSave, null), att.Store, att.Duration, att.Ticks };
                    object stateItem = Activator.CreateInstance(constructedType, constructorParams);

                    MethodInfo mi = this.GetType().GetMethod("SetStateItem");
                    MethodInfo genericMi = mi.MakeGenericMethod(genericArgs);
                    object[] genericMiParams = new object[] { stateItem };
                    genericMi.Invoke(this, genericMiParams);

                }
            }
        }

        /// <summary>
        /// Attempts to return the named StateItem from the internal items collection, 
        /// if not found it attempts to load it first.
        /// </summary>
        /// <param name="name">Name of the item.</param>
        /// <param name="key">the item key</param>
        /// <param name="itemStore">The item store.</param>
        /// <returns>
        /// Default Value of <typeparam name="T"></typeparam> If not found
        /// </returns>
        public T GetStateItemValue<T>(string name, string key, StateItemStore itemStore) 
        {
            T value = default(T);
            TryGetStateItemValue<T>(name, key, itemStore, out value);
            return value;
        }

        /// <summary>
        /// Tries the get state item value.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object to try and retreive
        /// </typeparam>
        /// <param name="name">Name of the item.</param>
        /// <param name="key">the item key</param>
        /// <param name="itemStore">The item store.</param>
        /// <param name="value">The value to popolate.</param>
        /// <returns>True if the object is found and is the correct type</returns>
        public bool TryGetStateItemValue<T>(string name, string key, StateItemStore itemStore, out T value)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "name can't be null");
            }            
            
            value = default(T);
            bool foundStateItem = false;
            if (TryLoadStateItem<T>(name, key, itemStore, out value))
            {
                foundStateItem = true;
            }
            return foundStateItem;
        }

        /// <summary>
        /// Loads the specified stateItem from the specified store and adds it to the 
        /// stateItemCollection.
        /// </summary>
        /// <param name="name">Name of the item.</param>
        /// <param name="key">the item key</param>
        /// <param name="itemStore">The item store.</param>
        /// <returns></returns>
        private bool TryLoadStateItem<T>(string name, string key, StateItemStore stateItemStore, out T value) 
        {
            value = default(T);
            string compositeKey = name + "." + key;
            bool foundStateItem = false;

            if ((stateItemStore & StateItemStore.Cookie) == StateItemStore.Cookie)
            {
                if (_cookieHelper.TryGetCookie<T>(name, key, out value ))
                {
                    foundStateItem = true;
                }
            }
            // If we didn't find it in the cookie and there's session persistence look there
            if (!foundStateItem && (stateItemStore & StateItemStore.AspSession) == StateItemStore.AspSession)
            {
                if (_sessionHelper.TryGetValue<T>(compositeKey, out value))
                {
                    foundStateItem = true;
                }
            }
            // If we Still haven't found it, look in the cache
            if (!foundStateItem && (stateItemStore & StateItemStore.AspCache) == StateItemStore.AspCache)
            {
                if (_aspCacheHelper.TryGetValue<T>(compositeKey, out value))
                {
                    foundStateItem = true;
                }
            }

            if (!foundStateItem)
            {
                value = default(T);
            }

            return foundStateItem;
        }
    
        //Stores the supplied value with the named state item
        public void SetStateItem<T>(StateItem<T> stateItem)
        {
            string compositeKey = stateItem.Name + "." + stateItem.Key;
            if ((stateItem.Store & StateItemStore.Cookie) == StateItemStore.Cookie)
            {
                bool persist = stateItem.Duration == StatePersistenceDuration.NeverExpire;
                _cookieHelper.UpdateCookie<T>(
                    stateItem.Name, 
                    stateItem.Key, 
                    stateItem.Value, 
                    stateItem.Duration, 
                    stateItem.Ticks);
            }
            if ((stateItem.Store & StateItemStore.AspSession) == StateItemStore.AspSession)
            {
                _sessionHelper.UpdateValue<T>(compositeKey, stateItem.Value);
            }
            if ((stateItem.Store & StateItemStore.AspCache) == StateItemStore.AspCache)
            {
                _aspCacheHelper.UpdateValue<T>(
                    compositeKey, 
                    stateItem.Value, 
                    stateItem.Duration, 
                    stateItem.Ticks);
            }
        }

        /// <summary>
        /// Deletes the state item.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="key">The key.</param>
        /// <param name="itemStore">The item store.</param>
        public void DeleteStateItem<T>(StateItem<T> stateItem)
        {
            string compositeKey = stateItem.Name + "." + stateItem.Key;
            if ((stateItem.Store & StateItemStore.Cookie) == StateItemStore.Cookie)
            {
                _cookieHelper.DeleteCookie(stateItem.Name, stateItem.Key);
            }
            if ((stateItem.Store & StateItemStore.AspSession) == StateItemStore.AspSession)
            {
                _sessionHelper.DeleteValue(compositeKey);
            }
            if ((stateItem.Store & StateItemStore.AspCache) == StateItemStore.AspCache)
            {
                _aspCacheHelper.DeleteValue(compositeKey);
            }
        }

		public void ClearStateItems(StateItemStore storeType)
		{
			if ((storeType & StateItemStore.Cookie) == StateItemStore.Cookie)
			{
				_cookieHelper.ClearCookies();
			}
			if ((storeType & StateItemStore.AspSession) == StateItemStore.AspSession)
			{
				_sessionHelper.ClearSession();
			}
			if ((storeType & StateItemStore.AspCache) == StateItemStore.AspCache)
			{
				_aspCacheHelper.ClearCache();
			}
        }

        #endregion Public Instance Methods
    }
}
