using System;
using Corbis.Membership.ServiceAgents.V1;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.UI.Presenters.Tools.Interfaces;


namespace Corbis.Web.UI.Presenters.Tools
{
    public class SignInPresenter : BasePresenter
    {
        ISignInView signInView;
        IMembershipContract membershipServiceAgent;
        Member member;

        public SignInPresenter(ISignInView signInView, MembershipServiceAgent membershipServiceAgent)
        {
            if (signInView == null)
            {
                throw new ArgumentNullException("SignInPresenter: SignInPresenter() - View cannot be null.");
            }
            if (membershipServiceAgent == null)
            {
                throw new ArgumentNullException("SignInPresenter: SignInPresenter() - Service agent cannot be null.");
            }
            this.signInView = signInView;
            this.membershipServiceAgent = membershipServiceAgent;
        }

        public SignInPresenter(ISignInView signInView)
            : this(signInView, new MembershipServiceAgent())
        {
        }

        public bool ValidateUser()
        {
            try
            {
                ValidateUserStatus status = membershipServiceAgent.ValidateUser(signInView.Username, signInView.Password);

                switch (status)
                {
                    case ValidateUserStatus.UserValidated:
                        return true;
                    case ValidateUserStatus.InvalidUserName:
                    case ValidateUserStatus.InvalidPassword:
                    case ValidateUserStatus.None:
                    default:
                        signInView.LoginUnsuccessful = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, null, "SignInPresenter: ValidateUser()");
                throw;
            }

            return false;
        }

        public string UserCountry
        {
            get
            {
                string userCountry = string.Empty;

                if (this.member == null) this.member = membershipServiceAgent.GetMemberByUsername(signInView.Username);
                if (this.member != null) userCountry = this.member.CountryCode;

                return userCountry;
            }
        }

        public string UserLanguage
        {
            get
            {
                string userLanguage = string.Empty;

                if (this.member == null) this.member = membershipServiceAgent.GetMemberByUsername(signInView.Username);
                if (this.member != null) userLanguage = this.member.CultureName;

                return userLanguage;
            }
        }
    }
}

