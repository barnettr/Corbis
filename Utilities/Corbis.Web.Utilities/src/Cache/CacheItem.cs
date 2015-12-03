using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Caching;

namespace Corbis.Web.Utilities
{
    /// <summary>
    /// Represents a single cache item stored in the ASP.NET Cache
    /// </summary>
    public class CacheItem
    {
        #region Cache Item Key Enumeration

        /// <summary>
        /// List of keys of all objects stored in ASP.NET Cache
        /// </summary>
        /// <remarks>
        /// This enumeration is internal as we are not exposing this 
        /// to be used by devs. Rather we want them to create CacheItem object to
        /// supply some settings also.
        /// </remarks>
        internal enum CacheItemKeyEnum
        {
            //NOTE: PLEASE PROVIDE COMMENTS TO ALL KEYS

            LanguageList,
            DropDownMenuList

            
        }

        #endregion

        #region Items

        //NOTE: Represent the list of all objects stored in ASP.NET Cache
        //These static objects are the ones which are going to be used by the Devs to
        //store items in ASP.NET Cache. This provides flexibility in supplying cache settings in
        //one place rather than requiring developers to supply them at different places in application

        //NOTE: PLEASE PROVIDE COMMENTS TO ALL THESE ITEMS
        public static CacheItem LanguageList = new CacheItem(CacheItemKeyEnum.LanguageList, null, CacheItemPriority.Default,
                                                                Cache.NoAbsoluteExpiration, new TimeSpan(24,0,0), null);
        public static CacheItem DropDownMenuList = new CacheItem(CacheItemKeyEnum.DropDownMenuList, null, CacheItemPriority.Default,
                                                                Cache.NoAbsoluteExpiration, new TimeSpan(24, 0, 0), null);
        
        #endregion

        #region Member Variables
        
        private CacheItemKeyEnum _key;
        private CacheDependency _dependency;
        private CacheItemPriority _priority;
        private DateTime _absoluteExpiriation;
        private TimeSpan _slidingExpiration;
        private CacheItemRemovedCallback _itemRemovedCallBack;

        #endregion

        #region Constructor

        /// <summary>
        /// Private constructor as do not want users outside this project to create objects
        /// of this type. We create only static object within this class to enable users
        /// to supply cache settings at one place
        /// </summary>
        /// <param name="key">Key by which we store or retrieve items in Cache</param>
        /// <param name="dependency"><see cref="CacheDependency"/></param>
        /// <param name="priority"><see cref="CacheItemPriority"/></param>
        /// <param name="absoluteExpiriation">The time at which the added object expires and is removed from the cache. 
        /// If you are using sliding expiration, the absoluteExpiration parameter must be NoAbsoluteExpiration.</param>
        /// <param name="slidingExpiration">The interval between the time the added object was last accessed 
        /// and the time at which that object expires. If this value is the equivalent of 20 minutes, the object 
        /// expires and is removed from the cache 20 minutes after it is last accessed. 
        /// If you are using absolute expiration, the slidingExpiration parameter must be NoSlidingExpiration.</param>
        /// <param name="itemRemovedCallBack"><see cref="CacheItemRemovedCallback"/></param>
        private CacheItem(CacheItemKeyEnum key, CacheDependency dependency, CacheItemPriority priority,
                            DateTime absoluteExpiriation, TimeSpan slidingExpiration, 
                            CacheItemRemovedCallback itemRemovedCallBack)
        {
            this.ItemKey = key;
            this.CacheDependency = dependency;
            this.Priority = priority;
            this.AbsoluteExpiration = absoluteExpiriation;
            this.SlidingExpiration = slidingExpiration;
            this.ItemRemovedCallback = itemRemovedCallBack;
        }

        #endregion

        #region Internal Properties

        internal CacheItemKeyEnum ItemKey
        {
            get 
            {
                return _key;
            }
            set 
            {
                _key = value;
            }
        }

        internal CacheDependency CacheDependency
        {
            get
            {
                return _dependency;
            }
            set
            {
                _dependency = value;
            }
        }

        internal CacheItemPriority Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = value;
            }
        }

        internal DateTime AbsoluteExpiration
        {
            get
            {
                return _absoluteExpiriation;
            }
            set
            {
                _absoluteExpiriation = value;   
            }
        }

        internal TimeSpan SlidingExpiration
        {
            get
            {
                return _slidingExpiration;
            }
            set
            {
                _slidingExpiration = value;
            }
        }

        internal CacheItemRemovedCallback ItemRemovedCallback
        {
            get
            {
                return _itemRemovedCallBack;
            }
            set
            {
                _itemRemovedCallBack = value;
            }
        }

        #endregion
    }
}
