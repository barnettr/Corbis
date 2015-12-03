using System;
using System.Collections.Generic;
using System.Text;
using Corbis.Web.UI.Presenters.Registration;

namespace Corbis.Web.UI.Registration.ViewInterfaces
{
    public interface ICultureSpecificNameView
    {
        string LastName { get; }
        string FirstName { get; }
        string FuriganaFirstName { get; }
        string FuriganaLastName { get; }
    }
}
