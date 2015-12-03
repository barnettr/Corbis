using System;
using System.Collections.Generic;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.UI.Presenters;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;

namespace Corbis.Web.UI.Accounts.ViewInterfaces
{
    public interface IEditView : IView
    {
        bool ShippingNameVisible { get; set; }
        string ShippingName { get; set; }
        bool UserNameVisible { get; set; }
        string UserName { get; set; }
        bool CompanyNameVisible { get; set; }
        string CompanyName { get; set; }
        bool JobTitleVisible { get; set; }
        string JobTitle { get; set; }
        bool NameVisible { get; set; }
        string FirstName { get; set; }
        string MiddleInitial { get; set; }
        string LastName { get; set; }
        string FirstNameFurigana { get; set; }
        string LastNameFurigana { get; set; }
        bool EmailVisible { get; set; }
        string Email { get; set; }
        bool AddressVisible { get; set; }
        string Address1 { get; set; }
        string Address2 { get; set; }
        string Address3 { get; set; }
        string City { get; set; }
        string Country { get; set; }
        string State { get; set; }
        bool StateEnabled { get; set; }
        string Zip { get; set; }
        bool ZipEnabled { get; set; }
        bool PhoneVisible { get; set; }
        string Phone { get; set; }
        string Dashes { get; }
        string SelectOne { get; }
        string Period { get; }
        bool StateCountryVisible { get; set;}
        bool ZipVisible { set;}
    }
}
