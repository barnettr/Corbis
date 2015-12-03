using System;
using System.Collections.Generic;
using System.Text;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Image.Contracts.V1;
using Corbis.Membership.Contracts.V1;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.RFCD.Contracts.V1;

namespace Corbis.Web.Entities
{
    public class CheckoutProduct
    {
        #region Private Variables

        private string _url128;
        private string _corbisID;
        private Guid _productuid;
        private string _effectivePrice;
        private string _priceTier;
        private string _countryCode;
        private string _currencyCode;
        private decimal _aspectRatio;
        private LicenseModel _licenceModel;
        private bool _isRFCD;
        #region RM Licence Details
        private string useCatagoryText;
        private string useTypeText;
        private Dictionary<string, string> usageAttributes;
        #endregion
        private RfLicenseDetail _rfLicenceDetails;
        private RFCDEntity rfcdEntity;
        private OfferingType offeringType;
       
        
        #endregion

        #region Public Properties
        public string URL128
        {
            get
            {
                return _url128;
            }
            set
            {
                _url128 = value;
            }
        }
        public decimal AspectRatio
        {
            get
            {
                return _aspectRatio;
            }
            set
            {
                _aspectRatio = value;
            }
        }
        public String CorbisID
        {
            get
            {
                return _corbisID;
            }

            set
            {

                _corbisID = value;
            }
        }
        public Guid ProductUid
        {
            get
            {
                return _productuid;
            }

            set
            {
                _productuid = value;
            }
        }
        public string EffectivePrice
        {
            get
            {
                return _effectivePrice;
            }
            set
            {
                _effectivePrice = value;
            }
        }        
        public String PriceTier
        {
            get
            {
                return _priceTier;
            }

            set
            {
                _priceTier = value;
            }
        }
        public String CountryCode
        {
            get
            {
                return _countryCode;
            }
            set
            {
                _countryCode = value;
            }
        }
        public String CurrencyCode
        {
            get 
            {
                return _currencyCode;
            }

            set
            {

                _currencyCode = value;
            }
        }
        public LicenseModel LicenseModel
        {
            get
            {
                return _licenceModel;
            }

            set
            {
                _licenceModel = value;

            }
        }
        public string UseCatagoryText
        {
            get
            {
                return useCatagoryText;
            }

            set
            {
                useCatagoryText = value;
            }
        }

        public string UseTypeText
        {
            get
            {
                return useTypeText;
            }

            set
            {
                useTypeText = value;
            }
        }
        public Dictionary<string, string> UsageAttributes
        {
            get
            {
                return usageAttributes;
            }

            set
            {

                usageAttributes = value;
            }
        }

        public OfferingType OfferingType
        {
            get
            {
                return offeringType;
            }
            set
            {
                offeringType = value;
            }
        }
      
        public RfLicenseDetail RFLicenseDetail
        {
            get
            {
                return _rfLicenceDetails;
            }

            set
            {

               _rfLicenceDetails = value;
            }

        }

        public bool IsRFCD
        {
            get { return _isRFCD; }
            set { _isRFCD = value; }
        }
        public RFCDEntity RfcdEntity
        {
            get
            {
                return rfcdEntity;
            }
            set
            {
                rfcdEntity = value;
            }
        }


        #endregion 
    }
    public enum CreditCardType
    {
        Unknown,
        New,
        Saved
    }
}
