using System;
using System.Collections.Generic;
using System.ServiceModel;
using Corbis.Web.UI.Presenters;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.UI.Registration.ViewInterfaces;
using Corbis.Web.Utilities;
using Corbis.Membership.Contracts.V1;
using Corbis.Membership.ServiceAgents.V1;
using ListItemConfigSection = Corbis.Web.Utilities.CustomConfigurationSettings;
using Corbis.Web.Content;
using Corbis.Common.ServiceFactory;
using Corbis.Common.FaultContracts;
using Corbis.Web.Entities;
using Corbis.Web.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Corbis.DisplayText.Contracts.V1;
using Corbis.DisplayText.ServiceAgents.V1;
using System.Globalization;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.LightboxCart.ServiceAgents.V1;

namespace Corbis.Web.UI.Presenters.Registration
{
    public class RegisterPresenter : BasePresenter
    {
        IRegisterView registerView;
        IMembershipContract membershipServiceAgent;
        ILightboxCartContract lightboxCartServiceAgent;
        RegionsContentProvider regions;
        CountriesContentProvider countries;

        public RegisterPresenter(IRegisterView registerView, IMembershipContract membershipServiceAgent, ILightboxCartContract lightboxCartServiceAgent)
        {
            if (registerView == null)
            {
                throw new ArgumentNullException("RegisterPresenter: RegisterPresenter() - View cannot be null.");
            }
            if (membershipServiceAgent == null)
            {
                throw new ArgumentNullException("RegisterPresenter: RegisterPresenter() - Service agent cannot be null.");
            }

            this.registerView = registerView;
            this.membershipServiceAgent = membershipServiceAgent;
            this.lightboxCartServiceAgent = lightboxCartServiceAgent;
            regions = ContentProviderFactory.CreateProvider(ContentItems.Region) as RegionsContentProvider;
            countries = ContentProviderFactory.CreateProvider(ContentItems.Country) as CountriesContentProvider;
        }

        public RegisterPresenter(IRegisterView registerView)
            : this(registerView, new MembershipServiceAgent(), new LightboxCartServiceAgent())
        {
        }

        public RegisterPresenter()
        {
             regions = ContentProviderFactory.CreateProvider(ContentItems.Region) as RegionsContentProvider;
             countries = ContentProviderFactory.CreateProvider(ContentItems.Country) as CountriesContentProvider;
        }

        public List<ContentItem> GetEmailFormatList()
        {
            List<ContentItem> items = new List<ContentItem>();
            items.Add(new ContentItem(Convert.ToString(Corbis.Membership.Contracts.V1.EmailFormat.Html),"HTML"));
            items.Add(new ContentItem(Convert.ToString(Corbis.Membership.Contracts.V1.EmailFormat.Text), "TEXT"));
            return items;
        }


        // TODO : RajulV - This code needs to be removed. 
        // All Script webservices are suppose to call Common presenter "GetStates" method. 
        public List<ContentItem> GetRegions(string countryCode)
        {
            try
            {
                List<ContentItem> results = new List<ContentItem>();
                results = regions.GetRegionsByCountryCode(countryCode);
                return results;
            }
            catch (Exception ex)
            {
                //HandleException(ex, CustomerServiceView.LoggingContext, "EditPresenter: GetRegions()");
                throw;
            }
        }


        public void SetRegionContent(RegionsContentProvider data)
        {
            regions = data;
        }

        public bool CreateUser()
        {
            
            try
            {
                Member member = new Member();

                MemberAddress billingAddress = new MemberAddress();
                billingAddress.Address1 = registerView.BusinessAddress1;
                billingAddress.Address2 = registerView.BusinessAddress2;
                billingAddress.Address3 = registerView.BusinessAddress3;
                billingAddress.AddressType = AddressType.Billing;
                billingAddress.PhoneNumber = member.BusinessPhoneNumber = registerView.BusinessPhoneNumber;
                billingAddress.AddressUid = new Guid();
                billingAddress.Name = registerView.FirstName;
                billingAddress.City = registerView.BusinessCity;
                billingAddress.CompanyName = member.CompanyName = registerView.CompanyName;
                billingAddress.CountryCode = member.CountryCode = registerView.BusinessCountryCode.Trim();
                billingAddress.PostalCode = registerView.BusinessPostalCode;
                billingAddress.RegionCode = registerView.BusinessRegionCode.Trim();
                member.BillingAddress = billingAddress;
                member.IsFastLaneEnabled = lightboxCartServiceAgent.IsECommerceEnableByCountry(billingAddress.CountryCode);

                MemberAddress mailingAddress = new MemberAddress();
                mailingAddress.Address1 = registerView.MailingAddress1;
                mailingAddress.Address2 = registerView.MailingAddress2;
                mailingAddress.Address3 = registerView.MailingAddress3;
                mailingAddress.AddressType = AddressType.Mailing;
                mailingAddress.PhoneNumber = registerView.BusinessPhoneNumber;
                mailingAddress.AddressUid = new Guid();
                mailingAddress.Name = registerView.FirstName;
                mailingAddress.City = registerView.MailingCity;
                mailingAddress.CompanyName = registerView.CompanyName;
                mailingAddress.CountryCode = registerView.MailingCountryCode.Trim();
                mailingAddress.PostalCode = registerView.MailingPostalCode;
                mailingAddress.RegionCode = registerView.MailingRegionCode.Trim();
                member.MailingAddress = mailingAddress;

                member.FirstName = registerView.FirstName;
                member.LastName = registerView.LastName;
                member.FuriganaFirstName = registerView.FuriganaFirstName;
                member.FuriganaLastName = registerView.FuriganaLastName;
                member.Email = registerView.Email;
                member.UserName = registerView.UserName;
                int jobTitleId;
                if (int.TryParse(registerView.JobTitle, out jobTitleId)
                    && Enum.IsDefined(typeof(JobTitle), jobTitleId))
                {
                    member.JobTitle = (JobTitle)jobTitleId;
                }
                member.CultureName = registerView.Language;
                int emailFormatId;
                if (int.TryParse(registerView.EmailFormat, out emailFormatId) &&
                    Enum.IsDefined(typeof(Corbis.Membership.Contracts.V1.EmailFormat), emailFormatId))
                {
                    member.EmailFormat = (Corbis.Membership.Contracts.V1.EmailFormat)emailFormatId;
                }
                int securityQuestionId;
                if (int.TryParse(registerView.SecurityQuestion, out securityQuestionId)
                    && Enum.IsDefined(typeof(PasswordRecoveryQuestionType), securityQuestionId))
                {
                    member.PasswordRecoveryQuestion = (PasswordRecoveryQuestionType)securityQuestionId;
                }
                member.PasswordRecoveryAnswer = registerView.SecurityAnswer;
    
                member.SendPromoEmails = registerView.SendEmail;
                member.SnailmailPreference = registerView.SendSnailMail;

                // Verify the new password is valid. We need to do this before calling the service
                ValidatePasswordResult passwordValidationResult =
                    MemberPassword.ValidatePassword(registerView.Password);

                if (passwordValidationResult != ValidatePasswordResult.Success)
                {
                    List<ViewValidationError<MemberValidationError>> errors = new List<ViewValidationError<MemberValidationError>>();
                    ViewValidationError<MemberValidationError> error =
                        new ViewValidationError<MemberValidationError>(
                            "Password", 
                            MemberValidationError.Unknown,
                            true,
                            true);
                    switch (passwordValidationResult)
                    {
                        case ValidatePasswordResult.InvalidPassword:
                            error.InvalidReason = MemberValidationError.InvalidPassword;
                            break;
                        case ValidatePasswordResult.PasswordContainsNonWesternCharacters:
                            error.InvalidReason = MemberValidationError.NonAsciiData;
                            break;
                        default:
                            // N/A
                            break;
                    }
                    errors.Add(error);
                    SetValidationErrorsOnView<MemberValidationError>(registerView as IViewPropertyValidator, errors);
                    return false;
                }

                // Compute the password hash
                string passwordValidationHash = 
                    MemberPassword.ComputeHashForUserValidation(registerView.Password);
                Dictionary<string, MemberValidationError> validationErrors = 
                    membershipServiceAgent.CreateMember(member, passwordValidationHash);
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
                            invalidPropertyName,
                            validationErrors[invalidPropertyName],
                            true,
                            showInSummary));
                    }
                    SetValidationErrorsOnView<MemberValidationError>(registerView as IViewPropertyValidator, errors);
                    return false;
                }

            }
            catch (FaultException<ValidationFault> vfex)
            {
                ValidationFault validationfault = vfex.Detail;
                registerView.ValidationErrors = validationfault.Details;

                return false;
            }
            catch (FaultException<CorbisFault> cfex)
            {
                CorbisFault corbisFault = cfex.Detail;
                registerView.LoggingContext.LogErrorMessage("RegisterPresenter: CreateUser()", corbisFault.ClientMessage, cfex);
                throw;
            }
            catch (Exception ex)
            {
                registerView.LoggingContext.LogErrorMessage("RegisterPresenter: CreaterUser()", ex);
                throw;
            }
        }

        public List<ContentItem> GetCountries()
        {
            try
            {
                List<ContentItem> list = countries.GetCountries();
                list.Insert(0, new ContentItem(String.Empty, registerView.SelectOneText));
                return list;
            }
            catch (Exception ex)
            {
                HandleException(ex, registerView.LoggingContext, "RegisterPresenter: GetCountries()");
                throw;
            }
        }

      
    }

    public static class RegistrationSessionKeys
    {
        public const string Register = "Register";
        public const string RedirectQuery = "RedirectQuery";
    }
}
