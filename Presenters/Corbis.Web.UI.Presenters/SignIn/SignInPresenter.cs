using System;
using System.Web.Security;
using Corbis.Membership.Contracts.V1;
using Corbis.Membership.ServiceAgents.V1;
using Corbis.Web.UI.SignIn.ViewInterfaces;

namespace Corbis.Web.UI.Presenters.SignIn
{
    public class SignInPresenter : BasePresenter
    {
        ISignInView signInView;
        IMembershipContract membershipServiceAgent;

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
                signInView.UserNameEmpty = false;
                signInView.PasswordEmpty = false;
                signInView.LoginUnsuccessful = false;
                signInView.PasswordChangeRequired = false;
                if (String.IsNullOrEmpty(signInView.Username))
                {
                    signInView.UserNameEmpty = true;
                }
                if (String.IsNullOrEmpty(signInView.Password))
                {
                    signInView.PasswordEmpty = true;
                }
                // Allow both checks before returning false
                if (String.IsNullOrEmpty(signInView.Username) || String.IsNullOrEmpty(signInView.Username))
                {
                    return false;
                }

                string validationHash = MemberPassword.ComputeHashForUserValidation(signInView.Password);
                ValidateUserStatus status = membershipServiceAgent.ValidateUser(signInView.Username, validationHash);
                switch (status)
                {
                    case ValidateUserStatus.UserValidated:
                        return true;
                    case ValidateUserStatus.PasswordChangeRequired:
                        signInView.PasswordChangeRequired = true;
                        break;
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
                HandleException(ex, signInView.LoggingContext, "SignInPresenter: ValidateUser()");
                throw;
            }

            return false;
        }
    }
}
