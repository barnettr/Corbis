using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Corbis.Framework.Globalization;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Authentication;

namespace Corbis.Web.UI.Presenters
{
   public class LegalPresenter:BasePresenter
   {
       #region Member variables
       public string License_URL = "/Content/LicenseInfo/Certified_EULA_{0}.pdf";
       private IView view;
       private IPrivacyPolicyView privacyPolicyView;
       private List<string> licensedCountryCodes = null;
       #endregion

       #region Constructors
       /// <summary>
       /// Initializes a new instance of the <see cref="LicenseTermsPresenter"/> class.
       /// </summary>
       public LegalPresenter()
       {
       }

       public LegalPresenter(IView view)
       {
           if (view == null)
           {
               throw new ArgumentNullException("LegalPresenter: LegalPresenter() - Legal view cannot be null.");
           } 
           this.view = view;
           privacyPolicyView = view as IPrivacyPolicyView;
       }
       #endregion

       #region properties
       /// <summary>
       /// Gets the licensed country codes.
       /// </summary>
       /// <value>The licensed country codes.</value>
       public List<string> LicensedCountryCodes
       {
           get 
           {
               if (licensedCountryCodes == null)
               {
                   licensedCountryCodes = new List<string>();

                   // Add hard coded licensed country codes to the list.
                   licensedCountryCodes.Add("AT"); //Austria
                   licensedCountryCodes.Add("AU"); // Australia
                   licensedCountryCodes.Add("BE"); // Belgium
                   licensedCountryCodes.Add("CA"); // Canada
                   licensedCountryCodes.Add("DE"); // Germany
                   licensedCountryCodes.Add("ES"); // Spain
                   licensedCountryCodes.Add("FR"); // France
                   licensedCountryCodes.Add("GB"); // Great Britan
                   licensedCountryCodes.Add("HK"); // Hong Kong
                   licensedCountryCodes.Add("IT"); // Italy
                   licensedCountryCodes.Add("JP"); // Japan
                   licensedCountryCodes.Add("MY"); // Malaysia
                   licensedCountryCodes.Add("NL"); // Netherlands
                   licensedCountryCodes.Add("PL"); // Poland
                   licensedCountryCodes.Add("SG"); // Singapore
                   licensedCountryCodes.Add("US"); // United States
               }

               return licensedCountryCodes; 
           }
       }
       #endregion

       #region Public Methods
       /// <summary>
       /// 
       /// 
       /// if( IsAnonymous) 
       /// Profile.IsAnonymous ==true
       /// displaly english
       /// 
       ///  Authenticated     
       /// (Profile.IsAuthenticated == true) 
      
           //partail Authenticated
           ///(Profile.IsAuthenticated==false && Profile.IsAnonymous==false)
          
       ///else if (partial Authenticated or Authenticated)
       /// 
       /// Step 1 check the Billing Address with the 16 country list
       /// Step 1.1 if true when you click download pdf display the 
       ///          billing address country License content
       ///      1.2 if billing Address null or not match with the 16 countries
       ///          display the english version
       ///      1.3  if billing country ==16 country list with more than 1 file
       ///             (call service function GetLicenseCode(countryCode,culture))
       /// 
       ///         1.3.1 if Billing country== canada and language ==english united states or UK then
       ///                display EULA_CA
       ///         1.3.2 if Billing country== canada and language ==Francais then
       ///                display EULA_CA_French
       ///         1.3.3 if Billing country== canada and language ==other than the top  then
       ///                display EULA_CA
       /// 
       ///         1.3.4 if Billing country== japan and language ==english united states or UK then
       ///                display EULA_JP_Engish
       ///         1.3.5 if Billing country== canada and language =japansis then
       ///                display EULA_JP
       ///         1.3.6 if Billing country== canada and language ==other than the top  then
       ///                display EULA_JP
       /// 
       /// step 2. if Coun
       /// </summary>
       /// <param name="selectedCountryCode"></param>
       /// <returns></returns>
       public string GetLicenseURL()
       {
        if (Profile.IsChinaUser)
           {
               return string.Format(License_URL, "CN");
           }
           // Partial Authenticated profiles
           else if (Profile.IsAuthenticated == true || Profile.IsAuthenticated == false && Profile.IsAnonymous == false)
           {
               return BillingAddressInCountryList(Profile.CountryCode);
           }          
           else
           {
               // For other users return file path for US.
               return string.Format(License_URL, "US");
           }
       }

       public void SetTrusteVisibility()
       {
           
           {
           }
           if (Language.CurrentLanguage == Language.EnglishUS)
           {
               privacyPolicyView.ShowTrustee = true;
           }
       }
       
       #endregion

       #region Private Methods
       /// <summary>
       /// Billings the address in country list.
       /// </summary>
       /// <param name="billingCountryCode">The billing country code.</param>
       /// <returns></returns>
       private string BillingAddressInCountryList(string billingCountryCode)
       {
           // Declare variable to hold the return value.
           string licenseUrl = string.Empty;

           // Get the current culture information.
           string currentCulture = Language.CurrentLanguage.LanguageCode;

           // Check if countrycode exists in the known list.
           if (LicensedCountryCodes.Contains(billingCountryCode))
           {
               if (billingCountryCode == "CA" && currentCulture == "fr-FR")
               {
                   licenseUrl = string.Format(License_URL, "CA_French");
               }
               else if (billingCountryCode == "JP" && currentCulture != "ja-JP")
               {
                   licenseUrl = string.Format(License_URL, "JP_English");
               }
               else if(billingCountryCode == "ES"){
                   licenseUrl = string.Format(License_URL, "US");
               }
               else
               {
                   licenseUrl = string.Format(License_URL, billingCountryCode);
               }
           }
           else
           {
               // Use US english path for unknown country codes.
               licenseUrl = string.Format(License_URL, "US");
           }

           // Return the link path.
           return licenseUrl;
       }
       #endregion
   }

}
