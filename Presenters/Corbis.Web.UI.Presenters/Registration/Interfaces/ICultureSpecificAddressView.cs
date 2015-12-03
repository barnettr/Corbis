using System;
using System.Collections.Generic;
using System.Text;
using Corbis.Web.UI.Presenters.Registration;

namespace Corbis.Web.UI.Registration.ViewInterfaces
{
    public interface ICultureSpecificAddressView
    {
        RegisterPresenter PagePresenter { get; set; }
        string Address1 { get; }
        string Address2 { get; }
        string Address3 { get; }
        string City { get; }
        string Zip { get; set; }
        string State { get; }
        string Country { get; }
    }
}
