using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Globalization;
using System.ServiceModel;
using System.Web.Security;
using Corbis.Common.FaultContracts;
using Corbis.Common.ServiceFactory.Validation;
using Corbis.DisplayText.Contracts.V1;
using Corbis.DisplayText.ServiceAgents.V1;
using Corbis.Framework.Globalization;
using Corbis.Framework.Logging;
using Corbis.Membership.Contracts.V1;
using Corbis.Membership.ServiceAgents.V1;
using Corbis.Web.Authentication;
using Corbis.Web.Content;
using Corbis.Web.Entities;
using Corbis.Web.UI.Accounts.ViewInterfaces;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;

namespace Corbis.Web.UI.Presenters.Accounts
{
    public enum AccountsEditMode
    {
        None,
        Create,
        Update,
        Delete
    }

    public enum CreditCardUsageType
    {
        SelectFromSavedCards,
        CreateNewCard
    }

    public class AccountsPresenter : BasePresenter
    {
        #region Private Fields

		private IView view;
        private IMembershipContract memberServiceAgent;
        private IEditShippingInformationView shippingAddress;
        private ICreditInformationView creditInformation;
		private CountriesContentProvider countriesContentProvider;
		private RegionsContentProvider regionsContentProvider;
        private CreditCardContentProvider creditCardContentProvider;
        private IChangePasswordView changePasswordView;
        private IMemberAssociatesView memberAssociatesView;

        #endregion

        #region Constructors

		public AccountsPresenter(IView view) : this(view, new MembershipServiceAgent()) { }
		public AccountsPresenter(IView view, IMembershipContract service)
        {
            if (view == null)
            {
                throw new ArgumentNullException("accountsView", "AccountsPresenter: AccountsPresenter() - Accounts view cannot be null.");
            }
            if (service == null)
            {
                throw new ArgumentNullException("memberServiceAgent", "AccountsPresenter: AccountsPresenter() - Membership service agent cannot be null.");
            }

            this.view = view;
            this.shippingAddress = this.view as IEditShippingInformationView;
            this.creditInformation = this.view as ICreditInformationView;
            this.changePasswordView = this.view as IChangePasswordView;
            this.memberAssociatesView = this.view as IMemberAssociatesView;
            this.memberServiceAgent = service;
        }

        #endregion

		#region Properties

		private Member memberInfo;
		protected Member MemberInfo
		{
			get
			{
				if (memberInfo == null)
				{
					memberInfo = memberServiceAgent.GetCompleteMemberByUsername(ProfileUserName);
					if (memberInfo == null)
					{
						throw new Exception(string.Format("AccountsPresenter: MemberInfo - Error retrieving information for member '{0}'.", Profile.UserName));
					}
				}

				return memberInfo;
			}
		}

        private string profileUserName;
        public string ProfileUserName
        {
            get
            {
                if (profileUserName == null)
                {
                    profileUserName = Profile.UserName;
                }
                return profileUserName;
            }
            set
            {
                profileUserName = value;
            }
        }

		private static Dictionary<string, string> countryLookup;
		public static Dictionary<string, string> CountryLookup
		{
			get
			{
                if (countryLookup == null)
                {
                    countryLookup = GetCountryCodes();
                }
			    return countryLookup;
			}
		}

		#endregion

        #region Sign In & Personal information

        public void ChangePassword()
        {
            ChangePasswordResult result = ChangePasswordResult.None;
            try
            {
                // Verify the new password is valid.
                ValidatePasswordResult newPasswordValidationResult =
                    MemberPassword.ValidatePassword(changePasswordView.NewPassword);
                switch (newPasswordValidationResult)
                {
                    case ValidatePasswordResult.InvalidPassword:
                        result = ChangePasswordResult.InvalidNewPassword;
                        break;
                    case ValidatePasswordResult.PasswordContainsNonWesternCharacters:
                        result = ChangePasswordResult.NewPasswordContainsNonWesternCharacters;
                        break;
                    case ValidatePasswordResult.Success:
                        string oldPasswordValidationHash =
                            MemberPassword.ComputeHashForUserValidation(changePasswordView.OldPassword);
                        string newPasswordValidationHash =
                            MemberPassword.ComputeHashForUserValidation(changePasswordView.NewPassword);
                        result = memberServiceAgent.ChangePassword(
                            Profile.UserName,
                            oldPasswordValidationHash,
                            newPasswordValidationHash);
                        
                        break;
                    case ValidatePasswordResult.None:
                    default:
                        // N/A, still None
                        break;
                }
            }
            catch { }
            changePasswordView.Result = result;
        }

		public void GetPersonalInfo()
		{
            IMyProfileView personalInformationView = (IMyProfileView)view;
            
            try
			{
               
				personalInformationView.Username = MemberInfo.UserName;
				personalInformationView.EmailAddress = MemberInfo.Email;
                personalInformationView.ShowFuriganaName = false;
                if (Language.CurrentLanguage == Language.Japanese)
                {
                    personalInformationView.DisplayName =
                        MemberInfo.LastName + " " + MemberInfo.FirstName;
                    personalInformationView.FuriganaDisplayName = 
                        MemberInfo.FuriganaLastName + " " + MemberInfo.FuriganaFirstName;
                    personalInformationView.ShowFuriganaName = true;
                }
                else if (Language.CurrentLanguage == Language.ChineseSimplified)
                {
                    personalInformationView.DisplayName =
                        MemberInfo.LastName + " " + MemberInfo.FirstName;
                }
                else
                {
                    personalInformationView.DisplayName =
                        MemberInfo.FirstName + " " + MemberInfo.LastName;
                }
				personalInformationView.MailingAddress = MemberInfo.MailingAddress;
                personalInformationView.PasswordRecoveryQuestion = MemberInfo.PasswordRecoveryQuestion;
                personalInformationView.PasswordRecoveryAnswer = MemberInfo.PasswordRecoveryAnswer;
			}
			catch (Exception ex)
			{
                HandleException(ex, personalInformationView.LoggingContext, string.Format("AccountsPresenter: GetPersonalInfo() - Error retrieving personal information for member '{0}'.", MemberInfo.MemberUid));
				throw;
			}
		}

        public void PopulateChangePersonalInformation()
        {
            IChangePersonalInformation changeView = view as IChangePersonalInformation;
            if (changeView == null)
            {
                throw new ApplicationException("IChangePersonalInformation is null");
            }

            changeView.UserName = MemberInfo.UserName;
            if (Language.CurrentLanguage == Language.Japanese)
            {
                changeView.Name1 = MemberInfo.LastName;
                changeView.Name2 = MemberInfo.FirstName;
                changeView.FuriganaLastName = MemberInfo.FuriganaLastName;
                changeView.FuriganaFirstName = MemberInfo.FuriganaFirstName;
            }
            else if (Language.CurrentLanguage == Language.ChineseSimplified)
            {
                changeView.Name1 = MemberInfo.LastName;
                changeView.Name2 = MemberInfo.FirstName;
            }
            else
            {
                changeView.Name1 = MemberInfo.FirstName;
                changeView.Name2 = MemberInfo.LastName;
            }

            changeView.Email = MemberInfo.Email;
            changeView.ConfirmEmail = MemberInfo.Email;
            changeView.PasswordRecoveryQuestion = MemberInfo.PasswordRecoveryQuestion;
            changeView.PasswordRecoveryAnswer = MemberInfo.PasswordRecoveryAnswer;
            changeView.Address = MemberInfo.MailingAddress;
        }

        public bool SavePersonalInformation()
        {
            IChangePersonalInformation changeView = view as IChangePersonalInformation;
            if (changeView == null)
            {
                throw new ApplicationException("IChangePersonalInformation is null");
            }

            string firstName = changeView.Name1;
            string lastName = changeView.Name2;
            string furiganaFirstName = MemberInfo.FuriganaFirstName;
            string furiganaLastName = MemberInfo.FuriganaLastName;
            if (Language.CurrentLanguage == Language.Japanese) {

                firstName = changeView.Name2;
                lastName = changeView.Name1;
                furiganaFirstName = changeView.FuriganaFirstName;
                furiganaLastName = changeView.FuriganaLastName;
            }
            else if (Language.CurrentLanguage == Language.ChineseSimplified)
            {
                firstName = changeView.Name2;
                lastName = changeView.Name1;
            }

            MemberAddress mailingAddress = memberInfo.MailingAddress;
            if (mailingAddress == null)
            {
                mailingAddress = new MemberAddress();
            }

            mailingAddress.Address1 = changeView.Address1;
            mailingAddress.Address2 = changeView.Address2;
            mailingAddress.Address3 = changeView.Address3;
            mailingAddress.City = changeView.City;
            mailingAddress.CountryCode = changeView.CountryCode;
            mailingAddress.PostalCode = changeView.PostalCode;
            mailingAddress.RegionCode = changeView.RegionCode;
            Dictionary<string, MemberValidationError> validationErrors =
                memberServiceAgent.UpdatePersonalInfo(
                    MemberInfo.MemberUid,
                    changeView.UserName,
                    changeView.Email,
                    firstName,
                    lastName,
                    furiganaFirstName,
                    furiganaLastName,
                    changeView.PasswordRecoveryQuestion,
                    changeView.PasswordRecoveryAnswer,
                    mailingAddress);

            if (validationErrors == null || validationErrors.Count == 0)
            {
                return true;
            }
            else
            {
                bool shownNonAsciiMessage = false;
                List<ViewValidationError<MemberValidationError>> errors =
                    new List<ViewValidationError<MemberValidationError>>(validationErrors.Count);
                foreach (string invalidPropertyName in validationErrors.Keys)
                {
                    // Need to map first & last name errors to the appropriate property on the viw, 
                    // and Remove the leading "Mailing" string from any address errors
                    string viewPropertyName =
                        invalidPropertyName.StartsWith("Mailing")
                        ? invalidPropertyName.Replace("Mailing", "")
                        : invalidPropertyName;
                    if (viewPropertyName == "FirstName")
                    {
                        if (Language.CurrentLanguage == Language.Japanese ||
                            Language.CurrentLanguage == Language.ChineseSimplified)
                        {
                            viewPropertyName = "Name2";
                        }
                        else
                        {
                            viewPropertyName = "Name1";
                        }
                    }
                    else if (viewPropertyName == "LastName")
                    {
                        if (Language.CurrentLanguage == Language.Japanese ||
                            Language.CurrentLanguage == Language.ChineseSimplified)
                        {
                            viewPropertyName = "Name1";
                        }
                        else
                        {
                            viewPropertyName = "Name2";
                        }
                    }
 
                    // Only show the Ascii validation error once
                    bool showInSummary = true;
                    switch (validationErrors[invalidPropertyName])
                    {
                        case MemberValidationError.NonAsciiData:
                            showInSummary = !shownNonAsciiMessage;
                            shownNonAsciiMessage = true;
                            break;
                        default:
                            // Show it
                            break;
                    }
                    errors.Add(new ViewValidationError<MemberValidationError>(
                        viewPropertyName,
                        validationErrors[invalidPropertyName],
                        true,
                        showInSummary));
                }
                SetValidationErrorsOnView<MemberValidationError>(changeView as IViewPropertyValidator, errors);
                return false;
            }
        }


        public MemberAddress GetMailingAddress()
        {
            return MemberInfo.MailingAddress;
        }

        #endregion

        #region MemberAssociates

        public void GetMemberAssociates()
        {
            if (memberAssociatesView == null)
            {
                throw new InvalidOperationException("IMemberAssociatesView is Null");
            }
            List<MemberAssociate> associates = 
                memberServiceAgent.GetMemberAssociates(Profile.UserName);
            SetMemberAssociatesOnView(associates, false);
        }

        public void AddMemberAssociates()
        {
            if (memberAssociatesView == null)
            {
                throw new InvalidOperationException("IMemberAssociatesView is Null");
            }
            List<MemberAssociate> associates = memberServiceAgent.AddMemberAssociates(
                Profile.UserName,
                memberAssociatesView.SelectedAssociateUserNames);
            SetMemberAssociatesOnView(associates, true);
        }

        public void RemoveMemberAssociates()
        {
            if (memberAssociatesView == null)
            {
                throw new InvalidOperationException("IMemberAssociatesView is Null");
            }
            memberServiceAgent.RemoveMemberAssociates(
                Profile.UserName,
                memberAssociatesView.SelectedAssociateUserNames);
        }

        private void SetMemberAssociatesOnView(List<MemberAssociate> associates, bool setAddErrorMessage)
        {
            const string MemberAssociatesDisplayFormat = "{0}, {1} {2}";
            List<MemberAssociateDisplay> results = new List<MemberAssociateDisplay>();

            if (associates != null)
            {
                // Sort associates by username
                associates.Sort(new Comparison<MemberAssociate>(
                    delegate(MemberAssociate x, MemberAssociate y)
                    {
                        return String.Compare(x.UserName, y.UserName, true, Language.CurrentLanguage.SpecificCultureInfo);
                    }));
                // For Chinese and Japanese, Names are displayed Last/First
                // All other cultures are displayed First/Last
                if (Language.CurrentCulture == Language.ChineseSimplified.CultureInfo ||
                    Language.CurrentCulture == Language.Japanese.CultureInfo)
                {
                    foreach (MemberAssociate associate in associates)
                    {
                        MemberAssociateDisplay display = new MemberAssociateDisplay();
                        display.Username = associate.UserName;
                        display.AssociateDisplay = String.Format(
                                MemberAssociatesDisplayFormat,
                                associate.UserName,
                                associate.LastName,
                                associate.FirstName);
                        if (setAddErrorMessage)
                        {
                            SetAddAssociateErrorMessage(associate, display);
                        }
                        results.Add(display);
                    }
                }
                else
                {
                    foreach (MemberAssociate associate in associates)
                    {
                        MemberAssociateDisplay display = new MemberAssociateDisplay();
                        display.Username = associate.UserName;
                        display.AssociateDisplay = String.Format(
                                MemberAssociatesDisplayFormat,
                                associate.UserName,
                                associate.FirstName,
                                associate.LastName);
                        if (setAddErrorMessage)
                        {
                            SetAddAssociateErrorMessage(associate, display);
                        }
                        results.Add(display);
                    }
                }
            }
            memberAssociatesView.Associates = results;

        }

        private void SetAddAssociateErrorMessage(MemberAssociate associate, MemberAssociateDisplay display)
        {
            display.Status = associate.Status;
            if (associate.Status == MemberAssociateStatus.Added)
            {
                display.AddErrorOcurred = false;
                display.ErrorMessage = null;
            }
            else
            {
                // If it's unknown, assume it's an InvalidUser
                if (display.Status == MemberAssociateStatus.Unknown)
                {
                    display.Status = MemberAssociateStatus.InvalidUser;
                }
                display.AddErrorOcurred = true;
            }
        }

        #endregion

        #region Business information

        public MemberAddress GetBillingAddress()
        {
            if (shippingAddress != null)
            {
                shippingAddress.ShippingAddress = MemberInfo.BillingAddress;
            }
            return MemberInfo.BillingAddress;
        }

        public void PopulateBusinessInformation()
        {
            IMyProfileView businessInformationView = (IMyProfileView)view;

            businessInformationView.BusinessAddress = MemberInfo.BillingAddress;
            // Company name and phone data are from Profile, in line with C1 data
            businessInformationView.CompanyName = MemberInfo.CompanyName;
            businessInformationView.JobTitle = MemberInfo.JobTitle;
            businessInformationView.Telephone = MemberInfo.BusinessPhoneNumber;
        }

        public void PopulateEditBusinessInformation()
        {
            IEditBusinessInformationView editView = view as IEditBusinessInformationView;
            if (editView == null)
            {
                throw new ApplicationException("IEditBusinessInformationView is null");
            }

            editView.CompanyName = MemberInfo.CompanyName;
            editView.JobTitle = MemberInfo.JobTitle;
            editView.BusinessPhoneNumber = MemberInfo.BusinessPhoneNumber;
            editView.Address = MemberInfo.BillingAddress;
        }


        public bool SaveBusinessInformation()
        {
            IEditBusinessInformationView editView = view as IEditBusinessInformationView;
            if (editView == null)
            {
                throw new ApplicationException("IEditBusinessInformationView is null");
            }

            MemberAddress billingAddress = new MemberAddress();
            billingAddress.Address1 = editView.Address1;
            billingAddress.Address2 = editView.Address2;
            billingAddress.Address3 = editView.Address3;
            billingAddress.City = editView.City;
            billingAddress.CountryCode = editView.CountryCode;
            billingAddress.PostalCode = editView.PostalCode;
            billingAddress.RegionCode = editView.RegionCode;

            Dictionary<string, MemberValidationError> validationErrors =
                memberServiceAgent.UpdateBusinessInfo(
                    Profile.UserName,
                    editView.CompanyName,
                    editView.JobTitle,
                    editView.BusinessPhoneNumber,
                    billingAddress);
            
            if (validationErrors == null || validationErrors.Count == 0)
            {
                return true;
            }
            else
            {
                bool shownNonAsciiMessage = false;
                List<ViewValidationError<MemberValidationError>> errors =
                    new List<ViewValidationError<MemberValidationError>>(validationErrors.Count);
                foreach (string invalidPropertyName in validationErrors.Keys)
                {
                    // Need to remove the leading "Business" string from any address errors
                    string viewPropertyName = invalidPropertyName;
                    if (invalidPropertyName.StartsWith("Business") &&
                        invalidPropertyName != "BusinessPhoneNumber")
                    {
                        viewPropertyName = invalidPropertyName.Replace("Business", "");
                    }

                    bool showInSummary = true;
                    switch (validationErrors[invalidPropertyName])
                    {
                        case MemberValidationError.NonAsciiData:
                            showInSummary = !shownNonAsciiMessage;
                            shownNonAsciiMessage = true;
                            break;
                        default:
                            // Show it
                            break;
                    }
                    errors.Add(new ViewValidationError<MemberValidationError>(
                        viewPropertyName,
                        validationErrors[invalidPropertyName],
                        true,
                        showInSummary));
                }
                SetValidationErrorsOnView<MemberValidationError>(editView as IViewPropertyValidator, errors);
                return false;
            }
        }

        #endregion

        #region Payment information

        public void GetPaymentInformation()
        {
            IMyProfileView accountDisplayView = (IMyProfileView)view;

            try
            {
                if (GetCreditCards(MemberInfo.CountryCode).Count == 0)
                {
                    accountDisplayView.DisplayPaymentPane = false;
                }
                else
                {
                    if (MemberInfo != null && MemberInfo.CreditCards != null && MemberInfo.CreditCards.Count > 0)
                    {
                        accountDisplayView.CreditsList = MemberInfo.CreditCards;
                        accountDisplayView.DisplayAvailableCreditDiv = true;
                        accountDisplayView.DisplayEmptyCreditDiv = false;
                    }
                    else
                    {
                        accountDisplayView.DisplayAvailableCreditDiv = false;
                        accountDisplayView.DisplayEmptyCreditDiv = true;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, accountDisplayView.LoggingContext, string.Format("AccountsPresenter: GetPaymentInformation() - Error retrieving payment information for member '{0}'.", MemberInfo.MemberUid));
                throw;
            }
        }

        public void DeleteCreditCard(string cardUid)
        {
            try
            {
                Guid card = new Guid(cardUid);
                memberServiceAgent.DeleteCreditCard(card);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    String.Format(
                    "AccountsPresenter: DeleteCreditCard() - Error deleting CreditCard '{0}'.",
                    cardUid), ex);
            }
        }

        public CreditCard GetCreditCard(string creditCardID)
        {
            CreditCard card;

            try
            {
                Guid cardUID = new Guid(creditCardID);
                card = memberServiceAgent.GetCreditCardByUid(cardUID);
                if (card == null)
                    throw new NullReferenceException(string.Format("AccountsPresenter: GetCreditCard() - CreditCard '{0}' not found.", cardUID));
            }
            catch (Exception)
            {
                throw new Exception(String.Format("AccountsPresenter: GetCreditCard() - Error retrieving CreditCard '{0}'.", creditCardID));
            }

            return card;
        }

        public bool AddCreditCard()
        {
            try
            {               
                Dictionary<string, CreditCardValidationError> validationErrors =
                    memberServiceAgent.AddMemberCreditCard(Profile.UserName, creditInformation.CreditCard);
                if (validationErrors == null || validationErrors.Count == 0)
                {
                    return true;
                }
                else
                {
                    List<ViewValidationError<CreditCardValidationError>> errors =
                        new List<ViewValidationError<CreditCardValidationError>>(validationErrors.Count);
                    foreach (string key in validationErrors.Keys)
                    {
                        string viewPropertyName = null;
                        switch (key)
                        {
                            case "NameOnCard":
                                viewPropertyName = "CardHolderName";
                                break;
                            case "CreditCardTypeCode":
                                viewPropertyName = "CardTypes";
                                break;
                            case "CardNumber":
                                viewPropertyName = "CardNumber";
                                break;
                            case "ExpirationDate":
                                viewPropertyName = "CardYear";
                                break;
                            default:
                                // N/A
                                break;
                        }
                        if (!string.IsNullOrEmpty(viewPropertyName))
                        {
                            errors.Add(new ViewValidationError<CreditCardValidationError>(
                                viewPropertyName,
                                validationErrors[key],
                                true,
                                true));
                        }
                    }
                    SetValidationErrorsOnView<CreditCardValidationError>(creditInformation as IViewPropertyValidator, errors);
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("AccountsPresenter: AddCreditCard() - Error adding CreditCard for member '{0}'.", Profile.UserName), ex);
            }
        }

        public bool UpdateCreditCard()
        {
            try
            {
                Dictionary<string, CreditCardValidationError> validationErrors =
                    memberServiceAgent.UpdateCreditCard(Profile.UserName, creditInformation.CreditCard);
                if (validationErrors == null || validationErrors.Count == 0)
                {
                    return true;
                }
                else
                {
                    List<ViewValidationError<CreditCardValidationError>> errors =
                        new List<ViewValidationError<CreditCardValidationError>>(validationErrors.Count);
                    foreach (string key in validationErrors.Keys)
                    {
                        string viewPropertyName = null;
                        switch (key)
                        {
                            case "NameOnCard":
                                viewPropertyName = "CardHolderName";
                                break;
                            case "CreditCardTypeCode":
                                viewPropertyName = "CardTypes";
                                break;
                            case "CardNumber":
                                viewPropertyName = "CardNumber";
                                break;
                            case "ExpirationDate":
                                viewPropertyName = "CardYear";
                                break;
                            default:
                                // N/A
                                break;
                        }
                        if (!string.IsNullOrEmpty(viewPropertyName))
                        {
                            errors.Add(new ViewValidationError<CreditCardValidationError>(
                                viewPropertyName,
                                validationErrors[key],
                                true,
                                true));
                        }
                    }
                    SetValidationErrorsOnView<CreditCardValidationError>(creditInformation as IViewPropertyValidator, errors);
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("AccountsPresenter: UpdateCreditCard() - Error updating CreditCard '{0}'.", creditInformation.CreditCard.CreditCardUid.ToString()), ex);
            }
        }

        public List<ContentItem> GetCardTypes(string countryCode)
        {
            CreditCardContentProvider provider = ContentProviderFactory.CreateProvider<CreditCardContentProvider>();
            return provider.GetCreditCards(Profile.CountryCode);
        }

        #endregion

        #region Preferences

        public void GetPreferences()
        {
            IMyProfileView preferencesDisplayView = (IMyProfileView)view;

            try
            {
                if (MemberInfo != null)
                {
                    preferencesDisplayView.SendPromoEmail = MemberInfo.SendPromoEmails;
                                       
                    preferencesDisplayView.EmailFormat = MemberInfo.EmailFormat;
                    preferencesDisplayView.EmailCultureName = MemberInfo.CultureName;
                    preferencesDisplayView.SnailmailPreference = MemberInfo.SnailmailPreference;
                    preferencesDisplayView.CorporateAccounts = MemberInfo.Bobos;
                    preferencesDisplayView.IsExpressCheckoutEnabled = MemberInfo.IsFastLaneEnabled;
                    
                    preferencesDisplayView.PreferredCreditCardUid = GetDefaultCreditCard(MemberInfo.CreditCards);
                    //preferencesDisplayView.PreferredShippingAddress = GetDefaultShippingAddress(MemberInfo.ShippingAddresses).ToString();
                    preferencesDisplayView.PreferredCorporateAccountId = GetPreferredCorporateAccount(MemberInfo.Bobos);
                    // VISIBILITY
                    // show mailing address if snailmailpreference is true
                    //preferencesDisplayView.ShowMailingAddress = MemberInfo.SnailmailPreference;
                    // do not show preferred credit card list if one or no credit cards available
                    // Set the container for payment to true initially, so we can set the inner parts visible or not
                    //if (MemberInfo.IsECommerceEnabled)
                    //{
                    //    preferencesDisplayView.ShowPaymentSection = true;
                    //}

                    preferencesDisplayView.ShowPreferredCreditCard = false;
                    
                    if (MemberInfo.IsECommerceEnabled)
                    {
                        if ((MemberInfo.CreditCards != null) && (MemberInfo.CreditCards.Count > 0))
                        {
                            if (GetCardTypes(MemberInfo.CountryCode).Count > 0)
                            {
                                preferencesDisplayView.ShowPreferredCreditCard = true;
                            }
                        }
                    }
                    // do not show preferred corporate account if one or no corporate accounts available
                    preferencesDisplayView.ShowPreferredCorporateAccount = MemberInfo.Bobos != null && MemberInfo.Bobos.Count > 0;
                    // only show preferred payment method if user has both credit card and corp acct available

                    if (preferencesDisplayView.ShowPreferredCreditCard && preferencesDisplayView.ShowPreferredCorporateAccount)
                    {
                        preferencesDisplayView.ShowPreferredPaymentMethod = true;
                        preferencesDisplayView.PreferredPaymentMethod = GetPreferredPaymentSetting(MemberInfo.UserPreferences);
                    }
                    else
                    {
                        preferencesDisplayView.ShowPreferredPaymentMethod = false;
                    }
                    
                    
                    preferencesDisplayView.ShowPreferredShippingAddress = false; //  MemberInfo.ShippingAddresses != null && MemberInfo.Bobos != null && MemberInfo.ShippingAddresses.Count > 0;
                    // Do not show payment section if no ecommerce available
                    if (MemberInfo.IsECommerceEnabled)
                    {
                        if (preferencesDisplayView.ShowPreferredCreditCard || preferencesDisplayView.ShowPreferredPaymentMethod
                            || preferencesDisplayView.ShowPreferredCorporateAccount || preferencesDisplayView.ShowExpressCheckout)
                        {
                            preferencesDisplayView.ShowPaymentSection = true;
                        }
                        else
                        {
                            preferencesDisplayView.ShowPaymentSection = false;
                        }
                    }
                    else
                    {
                        preferencesDisplayView.ShowPaymentSection = false;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, preferencesDisplayView.LoggingContext, string.Format("AccountsPresenter: GetPreferences() - Error retrieving preferences for member '{0}'.", MemberInfo.MemberUid));
                throw;
            }
        }

        public void SavePreferences()
        {
            IMyProfileView preferenceView = (IMyProfileView)view;
            if (preferenceView == null)
            {
                throw new ApplicationException("IMyProfileView is Null");
            }

            memberServiceAgent.UpdateMailPreferences(
                Profile.UserName,
                preferenceView.SendPromoEmail,
                preferenceView.EmailCultureName,
                preferenceView.EmailFormat,
                preferenceView.SnailmailPreference,
                preferenceView.PreferredPaymentMethod,
                preferenceView.PreferredCorporateAccountId,
                preferenceView.PreferredCreditCardUid,
                preferenceView.IsExpressCheckoutEnabled);

        }


        private Guid GetDefaultCreditCard(List<CreditCard> creditCards)
        {
            Guid defaultCard = Guid.Empty;
            if (creditCards != null)
            {
                foreach (CreditCard creditCard in creditCards)
                {
                    if (creditCard.IsDefault)
                    {
                        defaultCard = creditCard.CreditCardUid;
                        break;
                    }
                }
            }
            return defaultCard;
        }

        private Guid GetDefaultShippingAddress(List<MemberAddress> shippingAddresses)
        {
            Guid defaultShipping = Guid.Empty;
            if (shippingAddresses != null)
            {
                foreach (MemberAddress shippingAddress in shippingAddresses)
                {
                    if (shippingAddress.IsDefaultForType)
                    {
                        defaultShipping = shippingAddress.AddressUid;
                        break;
                    }
                }
            }
            return defaultShipping;
        }

        private int GetPreferredCorporateAccount(List<Company> bobos)
        {
            int prefBobo = 0;
            if (bobos != null)
            {
                foreach (Company bobo in bobos)
                {
                    if (bobo.IsDefault)
                    {
                        prefBobo = bobo.CompanyId;
                        break;
                    }
                }
            }
            return prefBobo;
        }

        private PaymentMethod GetPreferredPaymentSetting(List<Preference> userPrefs)
        {
            PaymentMethod paymentSetting = PaymentMethod.NotSet;
            if (userPrefs != null)
            {
                foreach (Preference userPref in userPrefs)
                {
                    if (userPref.PreferenceId == (int)UserPreference.PaymentMethod && 
                        Enum.IsDefined(typeof(PaymentMethod), userPref.PreferenceValueId))
                    {
                        paymentSetting = (PaymentMethod)userPref.PreferenceValueId;
                        break;
                    }
                }
            }
            return paymentSetting;
        }

        public bool AddMailingAddress()
        {
            IEditMailingInformationView editMailingInformationView = (IEditMailingInformationView)view;

            if (editMailingInformationView.MailingAddress == null)
            {
                throw new Exception("AccountsPresenter: AddMailingAddress() - MailingAddress cannot be null.");
            }

            try
            {
                memberServiceAgent.AddMemberAddress(MemberInfo.MemberUid, editMailingInformationView.MailingAddress);
            }
            catch (FaultException<UnicodeInMemberDataFault>)
            {
                editMailingInformationView.ShowUnicodeErrorMessage = true;
                return false;
            }
            catch (Exception ex)
            {
                HandleException(ex, editMailingInformationView.LoggingContext, string.Format("AccountsPresenter: AddMailingAddress() - Error adding MailingAddress for member '{0}'.", MemberInfo.MemberUid));
                throw;
            }
            return true;
        }

        public bool UpdateMailingAddress()
        {
            IEditMailingInformationView editMailingInformationView = (IEditMailingInformationView)view;

            if (editMailingInformationView.MailingAddress == null)
            {
                throw new Exception("AccountsPresenter: UpdateMailingAddress() - MailingAddress cannot be null.");
            }

            try
            {
                if (editMailingInformationView.MailingAddress.AddressUid == Guid.Empty && MemberInfo.MailingAddress != null)
                {
                   editMailingInformationView.MailingAddress.AddressUid = MemberInfo.MailingAddress.AddressUid;
                }

                memberServiceAgent.UpdateMemberAddress(MemberInfo.MemberUid, editMailingInformationView.MailingAddress);
            }
            catch (FaultException<UnicodeInMemberDataFault>)
            {
                editMailingInformationView.ShowUnicodeErrorMessage = true;
                return false;
            }
            catch (Exception ex)
            {
                HandleException(ex, editMailingInformationView.LoggingContext, string.Format("AccountsPresenter: UpdateMailingAddress() - Error updating MailingAddress '{0}'", editMailingInformationView.MailingAddress.AddressUid));
                throw;
            }
            return true;
        }

        public void PopulateMailingAddress()
        {
            IEditMailingInformationView mailingInfoView = (IEditMailingInformationView)view;
            if (MemberInfo.MailingAddress != null)
            {
                mailingInfoView.AddressUid = MemberInfo.MailingAddress.AddressUid;
                mailingInfoView.MailingAddress = MemberInfo.MailingAddress;
                mailingInfoView.Regions = GetRegions(MemberInfo.MailingAddress.CountryCode);
            }
        }

        #endregion

        #region Helper Methods

        public List<ContentItem> GetCreditCards(string countryCode)
        {
            if (string.IsNullOrEmpty(countryCode))
            {
                return new List<ContentItem>();
            }

            if (creditCardContentProvider == null)
            {
                creditCardContentProvider = ContentProviderFactory.CreateProvider(ContentItems.CreditCardType) as CreditCardContentProvider;
            }

            return creditCardContentProvider.GetCreditCards(countryCode);
        }

        public virtual List<ContentItem> GetRegions(string countryCode)
        {
            if (String.IsNullOrEmpty(countryCode))
                return new List<ContentItem>();

            if (regionsContentProvider == null)
            {
                regionsContentProvider =
                    ContentProviderFactory.CreateProvider(ContentItems.Region)
                    as RegionsContentProvider;
            }

            return regionsContentProvider.GetRegionsByCountryCode(countryCode);
        }

        public List<ContentItem> GetCountries()
        {
            try
            {
                if (countriesContentProvider == null)
                {
                    countriesContentProvider =
                        ContentProviderFactory.CreateProvider(ContentItems.Country)
                        as CountriesContentProvider;
                }

                return countriesContentProvider.GetCountries();
            }
            catch (Exception ex)
            {
                HandleException(ex, view.LoggingContext, "AccountsPresenter: GetCountries()");
                throw;
            }
        }

        private static Dictionary<string, string> GetCountryCodes()
        {
            Dictionary<string, string> countryDictionary = new Dictionary<string, string>();
            CountriesContentProvider countriesProvider = new CountriesContentProvider();
            List<ContentItem> countries = countriesProvider.GetCountries();

            foreach (ContentItem country in countries)
            {
                countryDictionary.Add(country.Key, country.ContentValue);
            }

            return countryDictionary;
        }

        internal void SetMembershipServiceBehavior(IMembershipContract agent)
        {
            memberServiceAgent = agent;

        }

        internal void SetLoggingBehavior(ILogging log)
        {
            view.LoggingContext = log;
        }

        #endregion

        #region ShippingAddress
        /// <summary>
        /// Updates the shipping address.
        /// </summary>
        public bool UpdateShippingAddress2()
        {
            if (shippingAddress.ShippingAddress != null)
            {
                try
                {
                    memberServiceAgent.UpdateMemberAddress(this.Profile.MemberUid, shippingAddress.ShippingAddress);
                }
                catch (FaultException<UnicodeInMemberDataFault>)
                {
                    shippingAddress.ShowUnicodeErrorMessage = true;
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Adds a new shipping address.
        /// </summary>
        public Guid AddShippingAddress()
        {
            if (shippingAddress.ShippingAddress != null)
            {
                //nahom, please change this function here and return me a guid. 
                try
                {
                    memberServiceAgent.AddMemberAddress(this.Profile.MemberUid, shippingAddress.ShippingAddress);
                    return shippingAddress.ShippingAddress.AddressUid;
                }
                catch (FaultException<UnicodeInMemberDataFault>)
                {
                    shippingAddress.ShowUnicodeErrorMessage = true;
                }
            }
            return Guid.Empty;
        }

        #region Credit Card Information 
         
        public void LoadNewCreditInformation()
        {
            creditInformation.DisplayCardListSection = false;
            creditInformation.DisplayCardNumberDisplayText = false;
            creditInformation.DisplayCardTypeDisplayText = false;
            creditInformation.CardTypes = GetCardTypes(Profile.CountryCode);
        }

        public void LoadSavedCreditInformation()
        {
            creditInformation.CreditCards = memberServiceAgent.GetMemberCreditCards(this.Profile.MemberUid);
            creditInformation.DisplayCardNumber = false;
            creditInformation.DisplayCardTypeList= false;
            creditInformation.DisplayCardNumberDisplayText = true;
            creditInformation.DisplayCardTypeDisplayText = true;

        }

        #endregion



        #endregion
        
    }
}
