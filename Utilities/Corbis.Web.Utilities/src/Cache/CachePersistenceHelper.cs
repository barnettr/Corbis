using System;
using System.Web;

namespace Corbis.Web.Utilities
{
    /// <summary>
    /// Contains helper methods to read and write data into Cache
    /// </summary>
    public static class CachePersistenceHelper
    {
        /// <summary>
        /// Saves the supplied item to the Cache
        /// </summary>
        /// <param name="cacheItemKey">Cache Item Key</param>
        /// <param name="cacheKeyValue">Cache Item Value</param>
        public static void SaveToCache(CacheItem cacheItem, object cacheItemValue)
        {
            CheckCurrentHttpContext("SaveToCache");

            HttpContext.Current.Cache.Insert(cacheItem.ItemKey.ToString(), cacheItemValue,
                                cacheItem.CacheDependency, cacheItem.AbsoluteExpiration,
                                cacheItem.SlidingExpiration, cacheItem.Priority,
                               cacheItem.ItemRemovedCallback);
        }

        /// <summary>
        /// Removes an item from the Cache
        /// </summary>
        /// <param name="cacheItemKey">Cache Item Key</param>
        public static void RemoveFromCache(CacheItem cacheItem)
        {
            CheckCurrentHttpContext("RemoveFromCache");

            //get the item key
            string itemKey = cacheItem.ItemKey.ToString();

            if (HttpContext.Current.Cache[itemKey] != null)
            {
                HttpContext.Current.Cache.Remove(itemKey);
            }
        }

        /// <summary>
        /// Retrieves an item from the Cache
        /// </summary>
        /// <param name="cacheItemKey">Cache Item Key</param>
        /// <returns>Cache Item Value</returns>
        public static object RetrieveFromCache(CacheItem cacheItem)
        {
            CheckCurrentHttpContext("RetrieveFromCache");

            return HttpContext.Current.Cache[cacheItem.ItemKey.ToString()];
        }

        /// <summary>
        /// Helper to check if HttpContext is present in the current context.
        /// If not present then we throw an exception.
        /// </summary>
        private static void CheckCurrentHttpContext(string operationName)
        {
            if (HttpContext.Current == null)
            {
                throw new ArgumentNullException((string.Format("HttpContext was null while trying to perform operation '{0}' in CachePersistenceHelper", 
                                    operationName)));
            }
        }
    }
}
