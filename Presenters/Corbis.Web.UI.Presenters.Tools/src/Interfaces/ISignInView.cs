using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.UI.Presenters.Tools.Interfaces
{
    public interface ISignInView
    {
        bool LoginUnsuccessful { set; }
        string Password { get; }
        string Username { get; }
    }
}

