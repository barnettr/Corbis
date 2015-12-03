using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Web.Entities;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.CustomerService.ViewInterfaces
{
    public interface ICustomerServiceWebServiceView : IView
    {
        string Country { get; set; }
        List<ContentItem> StateList { set; }
    }
}
