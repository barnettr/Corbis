using System;
using System.Collections.Generic;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Entities;

namespace Corbis.Web.UI.CustomerService.ViewInterfaces
{
	public interface ICustomerServiceView : ICustomerServiceOfficeView
	{
        string FirstName{ set; get;}
        string LastName { set; get;}
        string Email { set; get;}
        string State { set; get;}
        string Comments{ get;}
        string Subject{ get;}
        string Dashes{ get;}
        bool StateEnabled { get; set;}
        string SelectOne { get; }
        string CountryCodeSelected { get; set; }
        string StateCodeSelected { get; set; }
        string CountryNameSelected { get; }
        string StateNameSelected { get; }
        string Telephone { get; set; }
        string Browser { get; }
        string BrowserVersion { get; }
        string Plateform { get; }
        string PageUrl { get; }
        List<ContentItem> CountryList { set; }
        List<ContentItem> StateList { set; }
	}

	public interface ICustomerServiceContactView : ICustomerServiceOfficeView
	{
		bool UseDefaultOfficeInfo { set; }
		OfficeCountry OfficeCountry { set; }
	}

	public interface ICustomerServiceOfficeView : IView
	{
        string OfficeName { set; }
        string IpAddress { get; }
		string Address1 { set; }
		string Address2 { set; }
		string Address3 { set; }
		string City { set; }
		string RegionCode { set; }
		string Country { set; }
		string PostalCode { set; }
		string FaxNumber { set; }
		string PhoneNumber { set; }
		string EmailAddress { set; }
		string ContactFirstName { set; }
		string ContactLastName { set; }
        OfficeCountry OfficeCountryCodeSelected { get; set; }
	}
}
