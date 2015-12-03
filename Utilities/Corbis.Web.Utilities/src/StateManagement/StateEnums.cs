using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.Utilities.StateManagement
{
    [Flags]
    public enum StateItemStore
    {
        /// <summary>
        /// Not very Handy
        /// </summary>
        None = 0,
        /// <summary>
        /// Item will be stored in a cookie in the standard domain
        /// </summary>
        Cookie = 1,
        /// <summary>
        /// Item will be stored in the Asp Session
        /// </summary>
        AspSession = 2,
        /// <summary>
        /// Item will be stored in the Asp Cache
        /// </summary>
        AspCache = 4
    }

    public enum StatePersistenceDuration
    {
        /// <summary>
        /// Expire right away, not very handy
        /// </summary>
        None = 0,
        /// <summary>
        /// Store for the users session, applies only to <see cref="StateItemStore.Cookie"/>
        /// </summary>
        Session = 1,
        /// <summary>
        /// Use a sliding expiration. 
        /// Set the Ticks on the StateItem to the # of ticks
        /// from a <see cref="System.Timespan"/>
        /// </summary>
        Sliding = 2,
        /// <summary>
        /// Expire at a specified Time. 
        /// Set the Ticks on the StateItem to the # of tics from a <see cref="System.DateTime"/>
        /// </summary>
        Absolute = 3,
        /// <summary>
        /// The item should never expire. Not valid for <see cref="StateItemStore.AspSession"/>
        /// </summary>
        NeverExpire = 4
    }	
}
