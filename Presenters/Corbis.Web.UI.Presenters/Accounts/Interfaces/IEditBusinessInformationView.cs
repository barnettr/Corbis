using System;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.Accounts.ViewInterfaces
{
    public interface IEditBusinessInformationView : IView
    {
        string CompanyName { get; set; }
        JobTitle JobTitle { get; set; }
        string BusinessPhoneNumber { get; set; }
        MemberAddress Address { set; }
        string Address1 { get; }
        string Address2 { get; }
        string Address3 { get; }
        string City { get; }
        string CountryCode { get; set; }
        string RegionCode { get; set; }
        string PostalCode { get; }

    }
}
