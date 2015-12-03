using System;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.Accounts.ViewInterfaces
{
    public interface IChangePasswordView : IView
    {
        string OldPassword { get; }
        string NewPassword { get; }
        ChangePasswordResult Result { set; }
    }
}
