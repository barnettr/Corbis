using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.UI.ViewInterfaces
{
    public interface IContactUsView : IView
    {
        string FirstName { get; set; }
        string MiddleInitial { get; set; }
        string LastName { get; set; }
        string UserName { get; set; }
        string Email { get; set; }
        string CompanyName { get; set; }
        string Address1 { get; set; }
        string Address2 { get; set; }
        string Address3 { get; set; }
        string City { get; set; }
        string Country { get; set; }
        string Region { get; set; }
        string PostalCode { get; set; }
        string JobTitle { get; set; }
        string Telephone { get; set; }
        string Subject { get; set; }
        string Comments { get; set; }
        string ContactInformationFor { get; set; }
        string ContactInformationForDetails { get; set; }
    }
}
