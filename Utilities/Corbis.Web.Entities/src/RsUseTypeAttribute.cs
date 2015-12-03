using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.Entities
{
    /// <summary>
    /// Custom class used to bind the RS Pricing UseType attribute data to the UI grid.
    /// </summary>
    public class RsUseTypeAttributeValue
    {
        #region Private Members

        private string corbisId;

        private Guid useCategoryUid;

        private Guid useTypeUid;

        private Guid attributeUid;

        private string displayText;

        private string currencyCode;

        private string description;

        private Guid valueUid;

        private string effectivePrice;

        private bool requiresAeNegotiation;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the corbis id.
        /// </summary>
        /// <value>The corbis id.</value>
        public string CorbisId
        {
            get { return corbisId; }
            set { corbisId = value; }
        }

        /// <summary>
        /// Gets or sets the use category uid.
        /// </summary>
        /// <value>The use category uid.</value>
        public Guid UseCategoryUid
        {
            get { return useCategoryUid; }
            set { useCategoryUid = value; }
        }

        /// <summary>
        /// Gets or sets the use type uid.
        /// </summary>
        /// <value>The use type uid.</value>
        public Guid UseTypeUid
        {
            get { return useTypeUid; }
            set { useTypeUid = value; }
        }

        /// <summary>
        /// Gets or sets the attribute uid.
        /// </summary>
        /// <value>The attribute uid.</value>
        public Guid AttributeUid
        {
            get { return attributeUid; }
            set { attributeUid = value; }
        }

        /// <summary>
        /// Gets or sets the display text.
        /// </summary>
        /// <value>The display text.</value>
        public string DisplayText
        {
            get { return displayText; }
            set { displayText = value; }
        }

        /// <summary>
        /// Gets or sets the currency code.
        /// </summary>
        /// <value>The currency code.</value>
        public string CurrencyCode
        {
            get { return currencyCode; }
            set { currencyCode = value; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Gets or sets the value uid.
        /// </summary>
        /// <value>The value uid.</value>
        public Guid ValueUid
        {
            get { return valueUid; }
            set { valueUid = value; }
        }

        /// <summary>
        /// Gets or sets the effective price.
        /// </summary>
        /// <value>The effective price.</value>
        public string EffectivePrice
        {
            get { return effectivePrice; }
            set { effectivePrice = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [requires ae negotiation].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [requires ae negotiation]; otherwise, <c>false</c>.
        /// </value>
        public bool RequiresAeNegotiation
        {
            get { return requiresAeNegotiation; }
            set { requiresAeNegotiation = value; }
        }
        #endregion

        #region Constructor
        public RsUseTypeAttributeValue()
        {
        }
        #endregion
    }
}
