using System;
using System.Collections.Generic;
using Corbis.Web.UI.ViewInterfaces;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;

namespace Corbis.Web.UI.Registration.ViewInterfaces
{
    public interface IRegisterView : IView
    {

        #region SignIn Information

        string UserName { get; }
        string Password { get; }
        string ConfirmPassword { get; }
        string SecurityQuestion { get; }
        string SecurityAnswer { get; }

        #endregion

        #region Personal Information

        string FirstName { get; }
        string FuriganaFirstName { get; }
        string LastName { get; }
        string FuriganaLastName { get; }
        string Email { get; }
        string ConfirmEmail { get; }

        #endregion

        #region Mailing Address

        string MailingAddress1 { get; }
        string MailingAddress2 { get; }
        string MailingAddress3 { get; }
        string MailingCity { get; }
        string MailingCountryCode { get; }
        string MailingRegionCode { get; }
        string MailingPostalCode { get; set; }
        string Language { get; }

        #endregion

        #region Business Information

        string CompanyName { get; }
        string JobTitle { get; }
        string BusinessPhoneNumber { get; }

        #endregion

        #region Business Address

        string BusinessAddress1 { get; }
        string BusinessAddress2 { get; }
        string BusinessAddress3 { get; }
        string BusinessCity { get; }
        string BusinessCountryCode { get; }
        string BusinessRegionCode { get; }
        string BusinessPostalCode { get; set; }

        #endregion

        #region Preferences

        bool SendEmail { get; }
        string EmailFormat { get; }
        bool SendSnailMail { get; }
        bool Accept { get; }

        #endregion

        #region Localized Text

        string NoRegionsText { get; }
        string SelectOneText { get; }
        string ForeignZipCodeText { get; }

        #endregion
    }
}
