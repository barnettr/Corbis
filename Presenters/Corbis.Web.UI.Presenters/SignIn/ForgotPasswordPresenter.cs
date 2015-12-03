using System;
using System.Web.Security;
using Corbis.Membership.Contracts.V1;
using Corbis.Membership.ServiceAgents.V1;
using Corbis.Web.UI.SignIn.ViewInterfaces;

namespace Corbis.Web.UI.Presenters.SignIn
{
    public class ForgotPasswordPresenter : BasePresenter
    {
        IForgotPasswordView forgotPasswordView;
        IMembershipContract membershipServiceAgent;

        public ForgotPasswordPresenter(IForgotPasswordView forgotPasswordView, MembershipServiceAgent membershipServiceAgent)
        {
            if (forgotPasswordView == null)
            {
                throw new ArgumentNullException("SignInInformationRequestPresenter: SignInInformationRequestPresenter() - View cannot be null.");
            }
            if (membershipServiceAgent == null)
            {
                throw new ArgumentNullException("SignInInformationRequestPresenter: SignInInformationRequestPresenter() - Service agent cannot be null.");
            }

            this.forgotPasswordView = forgotPasswordView;
            this.membershipServiceAgent = membershipServiceAgent;
        }

        public ForgotPasswordPresenter(IForgotPasswordView forgotPasswordView)
            : this(forgotPasswordView, new MembershipServiceAgent())
        {
        }

        public void GetPasswordRecoveryQuestion()
        {
            PasswordRecoveryQuestionType result = membershipServiceAgent.GetPasswordRecoveryQuestionType(
                forgotPasswordView.EmailAddress);

            // Adding check here for DuplicateEmailAddress just to help debugging
            if (result == PasswordRecoveryQuestionType.DuplicateEmailFound)
            {
                result = PasswordRecoveryQuestionType.None;
            }
            forgotPasswordView.PasswordRecoveryQuestion = result;
        }

        public void RequestPassword()
        {
            SendPasswordReminderEmailResult result = membershipServiceAgent.RemindPassword(
                forgotPasswordView.EmailAddress,
                forgotPasswordView.PasswordRecoveryAnswer,
                forgotPasswordView.HomePageUrl,
                forgotPasswordView.CustomerServiceUrl);
            forgotPasswordView.Result = result;
        }
    }
}
