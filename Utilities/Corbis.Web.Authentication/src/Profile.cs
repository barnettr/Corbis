using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using Corbis.Framework.Globalization;
using Corbis.Framework.IpToCountry;
using Corbis.Framework.Logging;
using Corbis.Membership.Contracts.V1;
using Corbis.Membership.ServiceAgents.V1;
using Corbis.Web.Entities;
using Corbis.Web.Utilities;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.Web.Utilities.StateManagement;

namespace Corbis.Web.Authentication
{
    public class Profile : ProfileBase
    {
        #region Fields

        private static ILogging loggingContext;
        private IMembershipContract membershipServiceAgent;
        private CorbisMembershipProvider membershipProvider;

        [ThreadStatic]
        private static Profile _currentThreadProfile;

        [ThreadStatic]
        private static bool? _isChinaUser = null;

        private Dictionary<string, object> _memberDataSource = new Dictionary<string, object>();

        #endregion

        #region Properties

        public override object this[string propertyName]
        {
            get
            {
                try
                {
                    return base[propertyName];
                }
                catch (SettingsPropertyNotFoundException)
                {
                    return null;
                }
            }
            set
            {
                try
                {
                    base[propertyName] = value;

                    if (Properties[propertyName] != null && ((bool)Properties[propertyName].Attributes["AllowAnonymous"]) ||
                        MemberDataSource.ContainsKey(propertyName))
                    {
                        MemberDataSource[propertyName] = value;
                    }
                }
                catch (SettingsPropertyNotFoundException) { }
            }
        }

        internal Dictionary<string, object> MemberDataSource
        {
            get
            {
                return _memberDataSource;
            }
            set
            {
                _memberDataSource = value;
            }
        }

        public static Profile Current
        {
            get
            {
                if (_currentThreadProfile == null)
                {
                    _isChinaUser = null;
                    _currentThreadProfile = GetCurrentThreadProfile();
                    _currentThreadProfile.IsChinaUser  = CheckChinaUser();
                    if(_currentThreadProfile.IsChinaUser)
                    {
                        _currentThreadProfile.IsECommerceEnabled = false;
                    }
                }
                return _currentThreadProfile;
            }
            set { _currentThreadProfile = value; }
        }

        private static Profile GetCurrentThreadProfile()
        {
            Profile profile;
            string username = string.Empty;

            try
            {
                // anonymous
				if (HttpContext.Current.Profile.IsAnonymous)
				{
					StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
					username = stateItems.GetStateItemValue<string>(ProfileKeys.Name, ProfileKeys.UsernameStateItemKey, StateItemStore.Cookie);
				}
				else
                {
                    // get current username
                    username = HttpContext.Current.Profile.UserName;
                }

                // create partially-authenticated profile
                if (!StringHelper.IsNullOrTrimEmpty(username) &&
                    !HttpContext.Current.Request.IsAuthenticated)
                {
                    profile = Create(username);
                    profile.Context["IsAnonymous"] = false;
                    profile.Context["IsAuthenticated"] = false;
                    return profile;
                }
            }
            catch (Exception ex)
            {
                loggingContext.LogErrorMessage("CorbisProfileBase: Current", string.Format("Error getting current Profile for user '{0}.'", username), ex);
            }

            // get current anonoymous or fully-authenticated profile
            profile = HttpContext.Current.Profile as Profile;
            profile.Context["IsAnonymous"] = HttpContext.Current.Profile.IsAnonymous;
            profile.Context["IsAuthenticated"] = HttpContext.Current.Request.IsAuthenticated;

            return profile as Profile;
        }

        public virtual new string UserName
        {
            get
            {
                if (IsAnonymous)
                {
                    return "Anonymous";
                }
                else
                {
                    return base.UserName;
                }
            }
        }

        public virtual new bool IsAnonymous
        {
            get
            {
                return base.IsAnonymous;
            }
        }

        public bool IsAuthenticated
        {
            get { return Context["IsAuthenticated"] is bool ? (bool)Context["IsAuthenticated"] : false; }
        }

        #endregion

        #region Constructors

        public Profile(IMembershipContract membershipServiceAgent, CorbisMembershipProvider membershipProvider)
        {
            loggingContext = new LoggingContext(new string[] { "ProfileBase" });
            this.membershipProvider = membershipProvider;
            this.membershipServiceAgent = membershipServiceAgent;
        }

        public Profile()
            : this(new MembershipServiceAgent(), new CorbisMembershipProvider())
        {
        }

        #endregion

        #region Member Properties

        public MemberAddress AddressDetail
        {
            get { return (MemberAddress)this["AddressDetail"]; }
            set { this["AddressDetail"] = value; }
        }

        public int CartItemsCount
        {
            get { return (int)this["CartItemsCount"]; }
            set { this["CartItemsCount"] = value; }
        }

		public string CompanyType
		{
			get { return (string)this["CompanyType"]; }
			set { this["CompanyType"] = value; }
		}

        public ContractType ContractType
        {
            get { return (ContractType)this["ContractType"]; }
            set { this["ContractType"] = value; }
        }

        public string CompanyName
		{
			get { return (string)this["CompanyName"]; }
			set { this["CompanyName"] = value; }
		}

		public string CountryCode
        {
            get { return (string)this["CountryCode"]; }
            set { this["CountryCode"] = value; }
        }

        public string CurrencyCode
        {
            get { return (string)this["CurrencyCode"]; }
            set { this["CurrencyCode"] = value; }
        }

        [SettingsAllowAnonymous(true)]
        public string CultureName
        {
            get { return (string)this["CultureName"]; }
            set { this["CultureName"] = value; }
        }

        public string Email
        {
            get { return (string)this["Email"]; }
            set { this["Email"] = value; }
        }

        public EmailFormat EmailFormat
        {
            get { return (EmailFormat)this["EmailFormat"]; }
            set { this["EmailFormat"] = value; }
        }

        public string FirstName
        {
            get { return (string)this["FirstName"]; }
            set { this["FirstName"] = value; }
        }

        public string FuriganaFirstName
        {
            get { return (string)this["FuriganaFirstName"]; }
            set { this["FuriganaFirstName"] = value; }
        }

        public string FuriganaLastName
        {
            get { return (string)this["FuriganaLastName"]; }
            set { this["FuriganaLastName"] = value; }
        }

        [SettingsAllowAnonymous(true)]
        public bool IsECommerceEnabled
        {
            get { return ((bool)this["IsECommerceEnabled"] && 
                 (_isChinaUser.HasValue && !_isChinaUser.Value)); }
            set { this["IsECommerceEnabled"] = value; }
        }

        public bool HasRFCDPrice
        {
            get { return (bool)this["HasRFCDPrice"]; }
            set { this["HasRFCDPrice"] = value; }
        }

        public JobTitle JobTitle
        {
            get { return (JobTitle) this["JobTitle"]; }
            set { this["JobTitle"] = value; }
        }

        public string LastName
        {
            get { return (string)this["LastName"]; }
            set { this["LastName"] = value; }
        }

        public Guid MemberUid
        {
            get { return (Guid)this["MemberUid"]; }
            set { this["MemberUid"] = value; }
        }

        public string MiddleName
        {
            get { return (string)this["MiddleName"]; }
            set { this["MiddleName"] = value; }
        }

        public bool PasswordChangeRequired
        {
            get { return (bool)this["PasswordChangeRequired"]; }
            set { this["PasswordChangeRequired"] = value; }
        }

		public string PhoneNumber
		{
			get { return (string)this["PhoneNumber"]; }
			set { this["PhoneNumber"] = value; }
		}

		public string BusinessPhoneNumber
		{
			get { return (string)this["BusinessPhoneNumber"]; }
			set { this["BusinessPhoneNumber"] = value; }
		}

		public List<Role> Roles
        {
            get { return (List<Corbis.Membership.Contracts.V1.Role>)this["Roles"]; }
            set { this["Roles"] = value; }
        }

        public bool SendPromoEmails
        {
            get { return (bool)this["SendPromoEmails"]; }
            set { this["SendPromoEmails"] = value; }
        }

        public bool SnailmailPreference
        {
            get { return (bool)this["SnailmailPreference"]; }
            set { this["SnailmailPreference"] = value; }
        }

        public List<Preference> UserPreferences
        {
            get { return (List<Preference>)this["UserPreferences"]; }
            set { this["UserPreferences"] = value; }
        }

		public bool IsQuickPicEnabled
		{
			get { return (bool)this["IsQuickPicEnabled"]; }
			set { this["IsQuickPicEnabled"] = value; }
		}

		public bool IsFastLaneEnabled
		{
			get { return (bool)this["IsFastLaneEnabled"]; }
			set { this["IsFastLaneEnabled"] = value; }
		}

		public QuickPicFlags QuickPicType
        {
            get { return (QuickPicFlags)this["QuickPicType"]; }
            set { this["QuickPicType"] = value; }
        }

        public List<Permission> Permissions
        {
            get { return (List<Permission>)this["Permissions"]; }
            set { this["Permissions"] = value; }
        }

        [SettingsAllowAnonymous(true)]
        public bool IsChinaUser
        {
            get { return (bool) this["IsChinaUser"]; }
            set 
            { 
                
                this["IsChinaUser"] = value; 
            }
        }


        public bool CanSeeOutline
        {
            get { return Permissions.Contains(Permission.HasPermissionSearchOutline); }
        }
        #endregion

        #region Methods

        public static new Profile Create(string username)
        {
            try
            {
                Profile profile = ProfileBase.Create(username) as Profile;
                profile.Context["IsAnonymous"] = false;
                return profile;
            }
            catch (Exception ex)
            {
                loggingContext.LogErrorMessage("CorbisProfileBase: Create()", string.Format("Unable to create Profile for user '{0}.'", username), ex);
            }
            return null;
        }

        public static new Profile Create(string username, bool isAuthenticated)
        {
            throw new Exception("CorbisProfileBase: Create() - The method or operation is not implemented.");
        }

        public void Refresh()
        {
            MemberDataSource.Clear();
        }

        internal static bool CheckChinaUser()
        {
            if (!_isChinaUser.HasValue)
            {
                _isChinaUser = (ChineseURL() || IsChineseIpAddress() || CheckChinaBillingAddress());
            }
            return _isChinaUser.Value;
        }

        private static bool ChineseURL()
        {
            bool isChineseUser = false;
            string host = HttpContext.Current.Request.Url.Host;
            string chinaHostName = ConfigurationManager.AppSettings["CnHttpHost"];
            string chinaSecureHostName = ConfigurationManager.AppSettings["CnHttpsHost"];

            if (string.IsNullOrEmpty(chinaHostName) || string.IsNullOrEmpty(chinaSecureHostName))
            {
                throw new  ConfigurationErrorsException("China host name not configured");
            }

            if(host.Equals(chinaHostName,StringComparison.InvariantCultureIgnoreCase) || host.Equals(chinaSecureHostName, StringComparison.InvariantCultureIgnoreCase))
            {
                isChineseUser = true;
            }
            return isChineseUser;
        }

        private static bool IsChineseIpAddress()
        {
            IpToCountryLookup countryLookup = new IpToCountryLookup();
            string CountryCode = countryLookup.GetCountry(ClientIPHelper.GetClientIpAddress());
            return (CountryCode.ToLower().Equals("cn"));
        }

        private static bool CheckChinaBillingAddress()
        {
            return Current.CountryCode.ToLower().Equals("cn");
        }

        #endregion
    }
}
