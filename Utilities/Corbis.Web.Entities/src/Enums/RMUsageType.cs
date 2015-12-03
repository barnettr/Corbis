using System;

namespace Corbis.Web.Entities
{
    public enum RMUsageType
    {
        /// <summary>
        /// Unknown Usage Type
        /// </summary>
        Unknown,
        /// <summary>
        /// Saved Usage
        /// </summary>
        Saved,
        /// <summary>
        /// Usage already set on Product
        /// </summary>
        Existing,
        /// <summary>
        /// NEw Usage
        /// </summary>
        New
    }

}
