using System;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Membership.Contracts.V1;

namespace Corbis.Web.UI.SignIn.ViewInterfaces
{
    public interface IForgotPasswordView : IView
    {
        string EmailAddress { get; }
        PasswordRecoveryQuestionType PasswordRecoveryQuestion { set; }
        String PasswordRecoveryAnswer { get; }
        SendPasswordReminderEmailResult Result { set; }
        string HomePageUrl { get; }
        string CustomerServiceUrl { get; }
    }
}
