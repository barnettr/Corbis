using System;
using System.Collections.Generic;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Membership.Contracts.V1;

namespace Corbis.Web.UI.Accounts.ViewInterfaces
{
    public interface IMyProfileView : IView
    {
        #region Sign In & Personal information

        string Username { set; }
		string DisplayName { set; }
        string FuriganaDisplayName { set; }
		string EmailAddress { set; }
        MemberAddress MailingAddress { set; }
        PasswordRecoveryQuestionType PasswordRecoveryQuestion { set; }
        string PasswordRecoveryAnswer { set; }
        
        #endregion

        #region Business information
        
        string CompanyName { set; }
        JobTitle JobTitle { set; }
        string Telephone { set; }
        MemberAddress BusinessAddress { set; }

        #endregion

        #region Payment Information

        List<CreditCard> CreditsList { get; set;}
        bool DisplayPaymentPane { get; set; }
        bool DisplayAvailableCreditDiv { get; set;}
        bool DisplayEmptyCreditDiv { get;set; }

        #endregion

        #region Preferences

        bool SendPromoEmail { get; set; }
        string EmailCultureName { get; set; }
        EmailFormat EmailFormat { get;  set; }
        bool SnailmailPreference { get; set; }
        PaymentMethod PreferredPaymentMethod { get; set; }
        List<Company> CorporateAccounts { set; }
        Nullable<int> PreferredCorporateAccountId { get; set; }
        Nullable<Guid> PreferredCreditCardUid { get; set; }
        bool IsExpressCheckoutEnabled { get; set; }

        #endregion

        #region Display 

        bool ShowFuriganaName { set; }
        bool ShowPaymentSection { get; set; }
        bool ShowPreferredPaymentMethod { get; set; }
        bool ShowPreferredCorporateAccount { get; set; }
        bool ShowPreferredCreditCard { get; set; }
        bool ShowPreferredShippingAddress { get;set; }
        bool ShowExpressCheckout { get; set; }

        #endregion
    }
}
