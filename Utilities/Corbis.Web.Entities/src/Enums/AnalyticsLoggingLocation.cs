using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.Entities
{
    /// <summary>
    /// List of available AnalyticsLoggingLocation.
    /// </summary>
    [Flags]
    public enum AnalyticsLoggingLocation
    {
        ///<summary>
        ///Unknow
        ///</summary>
        Unknown,

        ///<summary>
        ///OmnitureDataWarehouse
        ///</summary>
        OmnitureDataWarehouse,

        ///<summary>
        ///CorbisDataWarehouse
        ///</summary>
        CorbisDataWarehouse,

        ///<summary>
        ///All
        ///</summary>
        All,

    }
}
