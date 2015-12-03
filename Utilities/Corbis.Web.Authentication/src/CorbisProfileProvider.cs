using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Profile;
using Corbis.Framework.Logging;
using Corbis.Membership.Contracts.V1;
using Corbis.Membership.ServiceAgents.V1;
using Corbis.Web.Utilities;
using Corbis.Framework.IpToCountry;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.LightboxCart.ServiceAgents.V1;

namespace Corbis.Web.Authentication
{
    public class CorbisProfileProvider : ProfileProvider
    {
        #region Fields

        private static ILogging loggingContext;
        private IMembershipContract membershipServiceAgent;
        private CorbisMembershipProvider membershipProvider;
        private ILightboxCartContract lightboxCartServiceAgent;

        #endregion


        #region Constructors

        public CorbisProfileProvider(IMembershipContract membershipServiceAgent, CorbisMembershipProvider membershipProvider, ILightboxCartContract lightboxCartServiceAgent)
        {
            loggingContext = new LoggingContext(new string[] { "ProfileProvider" });
            this.membershipProvider = membershipProvider;
            this.membershipServiceAgent = membershipServiceAgent;
            this.lightboxCartServiceAgent = lightboxCartServiceAgent;
        }

        public CorbisProfileProvider()
            : this(new MembershipServiceAgent(), new CorbisMembershipProvider(), new LightboxCartServiceAgent())
        {
        }

        #endregion

        #region ProviderBase Overrides

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ConfigurationErrorsException("CorbisProfileProvider: Initialize() - Profile configuration doesn't exist.");
            }

            ApplicationName = config["applicationName"];
            if (StringHelper.IsNullOrTrimEmpty(ApplicationName))
            {
                ApplicationName = "Corbis.Web";
            }

            base.Initialize(name, config);

            if (config.Count > 0)
            {
                if (!StringHelper.IsNullOrTrimEmpty(config.GetKey(0)))
                {
                    throw new ProviderException(string.Format("CorbisProfileProvider: Initialize() - Unrecognized attribute '{0}.'", config.GetKey(0)));
                }
            }
        }

        #endregion

        #region SettingsProvider Overrides

        private string applicationName;
        public override string ApplicationName
        {
            get { return applicationName; }
            set { applicationName = value; }
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            SettingsPropertyValueCollection settings = new SettingsPropertyValueCollection();

            if (collection.Count == 0)
            {
                return settings;
            }

            string userName = (string)context["UserName"];

            if (!string.IsNullOrEmpty(userName))
            {
                Profile profile = new Profile();
                if (profile.MemberDataSource == null || profile.MemberDataSource.Count == 0)
                {
                    profile.MemberDataSource = GetMemberDataSource(context);
                }

                SettingsPropertyValue setting;

                foreach (SettingsProperty property in collection)
                {
                    if (property.SerializeAs == SettingsSerializeAs.ProviderSpecific)
                    {
                        if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(String))
                        {
                            property.SerializeAs = SettingsSerializeAs.String;
                        }
                        else
                        {
                            property.SerializeAs = SettingsSerializeAs.Xml;
                        }
                    }

                    setting = new SettingsPropertyValue(property);

                    if (profile.MemberDataSource.ContainsKey(setting.Name))
                    {
                        setting.PropertyValue = profile.MemberDataSource[setting.Name];
                        setting.Deserialized = true;
                        setting.IsDirty = false;
                    }

                    settings.Add(setting);
                }
            }

            return settings;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            string userName = (string)context["UserName"];
            bool isAnonymous = context["IsAnonymous"] == null ? true : (bool)context["IsAnonymous"];
            string allowAnonymous = "AllowAnonymous";

            if (string.IsNullOrEmpty(userName) || collection.Count == 0)
            {
                return;
            }

            if (HttpContext.Current.Session == null)
            {
                return;
            }

            if (!HasDirtyProperty(collection))
            {
                return;
            }
            
            Profile profile = new Profile();
            foreach (SettingsPropertyValue setting in collection)
            {
                if (isAnonymous && !(bool)setting.Property.Attributes[allowAnonymous])
                {
                    continue;
                }

                if (!setting.IsDirty || !profile.MemberDataSource.ContainsKey(setting.Name))
                {
                    continue;
                }

                profile.MemberDataSource[setting.Name] = setting.PropertyValue;
            }
        }

        #endregion

        #region ProfileProvider Overrides

        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new Exception("CorbisProfileProvider: DeleteInactiveProfiles() - The method or operation is not implemented.");
        }

        public override int DeleteProfiles(string[] usernames)
        {
            throw new Exception("CorbisProfileProvider: DeleteProfiles() - The method or operation is not implemented.");
        }

        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            throw new Exception("CorbisProfileProvider: DeleteProfiles() - The method or operation is not implemented.");
        }

        public override ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("CorbisProfileProvider: FindInactiveProfilesByUserName() - The method or operation is not implemented.");
        }

        public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("CorbisProfileProvider: FindProfilesByUserName() - The method or operation is not implemented.");
        }

        public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("CorbisProfileProvider: GetAllInactiveProfiles() - The method or operation is not implemented.");
        }

        public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("CorbisProfileProvider: GetAllProfiles() - The method or operation is not implemented.");
        }

        public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new Exception("CorbisProfileProvider: GetNumberOfInactiveProfiles() - The method or operation is not implemented.");
        }

        #endregion

        #region Private Methods

        private Dictionary<string, object> GetMemberDataSource(SettingsContext context)
        {
            string userName = (string)context["UserName"];
            bool isAnonymous = context["IsAnonymous"] == null ? true : (bool)context["IsAnonymous"];

            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("CorbisProfileProvider: GetDataSource() - Username is null or empty.");
            }

            try
            {
                Dictionary<string, object> dataSource = new Dictionary<string, object>();

                if (isAnonymous)
                {
                    String countryCode = GetCountryCodeFromIPAddress(ClientIPHelper.GetClientIpAddress());
                    if(countryCode == null)
                    {
                        countryCode = "US";
                    }
                    dataSource["CountryCode"] = countryCode;
                    dataSource["IsECommerceEnabled"] = lightboxCartServiceAgent.IsECommerceEnableByCountry(countryCode);
                    return dataSource;
                }
                else
                {
                    // get Member properties
                    Member member = (membershipProvider.GetUser(userName, true) as CorbisMembershipUser).Member;
                    dataSource["AddressDetail"] = member.BillingAddress;
                    dataSource["CartItemsCount"] = member.CartItemsCount;
					dataSource["CompanyType"] = member.CompanyType;
					dataSource["CompanyName"] = member.CompanyName;
                    dataSource["ContractType"] = member.ContractType;
					dataSource["CountryCode"] = member.CountryCode;
                    dataSource["CultureName"] = member.CultureName;
                    dataSource["Email"] = member.Email;
                    dataSource["EmailFormat"] = member.EmailFormat;
                    dataSource["FirstName"] = member.FirstName;
                    dataSource["FuriganaFirstName"] = member.FuriganaFirstName;
                    dataSource["FuriganaLastName"] = member.FuriganaLastName;
                    //dataSource["HasRFCDPrice"] = member.HasRFCDPrice;
                    dataSource["IsECommerceEnabled"] = member.IsECommerceEnabled;
                    dataSource["JobTitle"] = member.JobTitle;
                    dataSource["LastName"] = member.LastName;
                    dataSource["MemberUid"] = member.MemberUid;
                    dataSource["MiddleName"] = member.MiddleName;
                    dataSource["PasswordChangeRequired"] = member.PasswordChangeRequired;
					dataSource["PhoneNumber"] = member.PhoneNumber;
					dataSource["BusinessPhoneNumber"] = member.BusinessPhoneNumber;
					dataSource["SendPromoEmails"] = member.SendPromoEmails;
                    dataSource["SnailmailPreference"] = member.SnailmailPreference;
                    dataSource["UserPreferences"] = member.UserPreferences;
                    dataSource["CurrencyCode"] = member.CurrencyCode;
                    dataSource["IsQuickPicEnabled"] = member.IsQuickPicEnabled;
					dataSource["IsFastLaneEnabled"] = member.IsFastLaneEnabled;
                    dataSource["QuickPicType"] = member.QuickPicType;
                    dataSource["Permissions"] = member.Permissions;

                    return dataSource;
                }
            }
            catch (Exception ex)
            {
                loggingContext.LogErrorMessage("CorbisProfileProvider: GetDataSource()", string.Format("Unable to get profile properties for user '{0}.'", userName), ex);
                throw;
            }
        }

        private string GetCountryCodeFromIPAddress(String ipAddress)
        {
            IpToCountryLookup countryLookup = new IpToCountryLookup();
            return countryLookup.GetCountry(ipAddress);
        }

        //private string GetCurrencyCodeFromCountryCode(String countryCode)
        //{ 
            
        //}

        private static bool HasDirtyProperty(SettingsPropertyValueCollection collection)
        {
            foreach (SettingsPropertyValue setting in collection)
            {
                if (setting.IsDirty)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
