using System.Collections.Generic;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Membership.Contracts.V1;

namespace Corbis.Web.UI.Accounts.ViewInterfaces
{
    public interface IChangePersonalInformation : IView 
    {
        string UserName { set; get; }

        /// <summary>
        /// First name for all Languages except Japanese and Chinese.
        /// Last name for Japanese and Chinese
        /// </summary>
        string Name1 { set; get; }

        /// <summary>
        /// Last name for all Languages except Japanese and Chinese.
        /// First name for Japanese and Chinese.
        /// </summary>
        string Name2 { get; set; }

        string FuriganaFirstName { get; set;  }
        string FuriganaLastName { get; set; }
        
        
        string Email { get; set; }
        
        string ConfirmEmail { get; set; }

        PasswordRecoveryQuestionType PasswordRecoveryQuestion { get;  set; }

        string PasswordRecoveryAnswer { get;  set; }

        MemberAddress Address { set; }

        string Address1 { get;}

        string Address2 { get;}

        string Address3 { get;}

        string City { get;}

        string CountryCode { get; set; }

        string RegionCode { get; set; }

        string PostalCode { get;}

    }
}