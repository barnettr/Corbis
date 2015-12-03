using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.Entities
{
    /// <summary>
    /// List of available ContentItems.
    /// </summary>
    public enum ContentItems
    {
        ///<summary>
        ///Unknow
        ///</summary>
        Unknown,

        ///<summary>
        ///Country
        ///</summary>
        Country,
        
        ///<summary>
        ///Language
        ///</summary>
        Language,

        ///<summary>
        ///Region
        ///</summary>
        Region,

        ///<summary>
        ///Industry
        ///</summary>
        Industry,

        ///<summary>
        ///OrganizationSize
        ///</summary>
        OrganizationSize,

        ///<summary>
        ///EmailFormat
        ///</summary>
        EmailFormat,

        /// <summary>
        /// DistributionSite
        /// </summary>
        DistributionSite,

        /// <summary>
        /// CreditCardType
        /// </summary>
        CreditCardType,

        /// <summary>
        /// ShippingPriority
        /// </summary>
        ShippingPriority,
    }
}
