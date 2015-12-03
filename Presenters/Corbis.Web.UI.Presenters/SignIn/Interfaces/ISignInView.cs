using System;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.SignIn.ViewInterfaces
{
    public interface ISignInView : IView
    {
        string Username { get; }
        string Password { get; }
        bool LoginUnsuccessful { set; }
        bool PasswordChangeRequired { set; }
        bool UserNameEmpty { set; }
        bool PasswordEmpty { set; }
    }
}
