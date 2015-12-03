using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.UI.CustomerService.ViewInterfaces;
using Corbis.Web.UI.Presenters.CustomerService;
using System.Web.Script.Services;
using Corbis.Web.Entities;
using System.Collections.Generic;
using Corbis.Framework.Logging;
using Corbis.Web.Utilities;
using Resources;
using System.Text.RegularExpressions;

namespace Corbis.Web.UI.CustomerService
{
    /// <summary>
    /// Summary description for CustomerServiceWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    [ToolboxItem(false)]
    public class CustomerServiceWebService : Corbis.Web.UI.src.CorbisWebService, ICustomerServiceWebServiceView, ICustomerServiceContactView
    {
        private CustomerServicePresenter presenter;
		private OfficeContactInfo officeContactInfo;

        public CustomerServiceWebService()
        {
            presenter = new CustomerServicePresenter();
            presenter.CustomerServiceWebServiceView = this;
        }

        [WebMethod(true)]
        public List<ContentItem> GetStates(string country)
        {
            List<ContentItem> stateList = null;
            this.Country = country;
            stateList = presenter.GetStatesFromWebService();
            
            return stateList;
        }

		[WebMethod(true)]
		public OfficeContactInfo GetContactCorbisOffice()
		{
			presenter = new CustomerServicePresenter();
			presenter.CustomerServiceContactView = this;
			presenter.GetContactCorbisOffice();
			setNameAndAddress();

			return OfficeContactInfomation;
		}

        private List<ContentItem> stateList = null;
        public List<ContentItem> StateList 
        {
            set { stateList = value; } 
        }

        private ILogging loggingContext;
        public Corbis.Framework.Logging.ILogging LoggingContext
        {
            get
            {
                return loggingContext;
            }
            set
            {
                loggingContext = value;
            }
        }

        public System.Collections.Generic.IList<Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.ValidationDetail> ValidationErrors
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

		private void setNameAndAddress()
		{
			OfficeContactInfomation.ContactName = String.Format(HttpContext.GetLocalResourceObject("~/CustomerService/CustomerServiceWebService.asmx", "nameTemplate").ToString(), ContactFirstName, ContactLastName).Trim();
            OfficeContactInfomation.OfficeAddress = Regex.Replace(String.Format(HttpContext.GetLocalResourceObject("~/CustomerService/CustomerServiceWebService.asmx", "addressTemplate").ToString(), OfficeName, Address1, Address2, Address3, City, RegionCode, PostalCode, Country), @"[\r\n]+", "<br/>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
		}

		private OfficeContactInfo OfficeContactInfomation
		{
			get
			{
				if (officeContactInfo == null)
				{
					officeContactInfo = new OfficeContactInfo();
				}

				return officeContactInfo;
			}
			set 
			{ 
				officeContactInfo = value; 
			}
		}

		#region ICustomerServiceContactView Members

		public string IpAddress
		{
			get { return ClientIPHelper.GetClientIpAddress(); }
		}

		public string OfficeName { get; set; }

        public string Address1 { get; set; }

		public string Address2 { get; set; }

		public string Address3 { get; set; }

		public string City { get; set; }

		public string RegionCode { get; set; }

		public string PostalCode { get; set; }

		public string Country { get; set; }

		public string FaxNumber
		{
			set { OfficeContactInfomation.OfficeFaxNumber = value; }
		}

		public string PhoneNumber
		{
			set { OfficeContactInfomation.OfficePhoneNumber = value; }
		}

		public string EmailAddress
		{
			set { OfficeContactInfomation.EmailAddress = value; }
		}

		public string ContactFirstName { get; set; }

		public string ContactLastName { get; set; }

		public OfficeCountry OfficeCountry
		{
			set 
			{ 
				OfficeContactInfomation.DefaultOfficeInfo = OfficeContactInfomation.UseDefaultOfficeInfo? Offices.ResourceManager.GetString(value.ToString()): "";
				OfficeContactInfomation.IsOtherLocations = value == OfficeCountry.OtherLocations;
			}
		}

		public bool UseDefaultOfficeInfo
		{
			set { OfficeContactInfomation.UseDefaultOfficeInfo = value; }
		}

        public OfficeCountry OfficeCountryCodeSelected
        {
            get { throw new System.NotImplementedException(); }
            set
            {
                throw new System.NotImplementedException();
            }
        }


		#endregion
	}
}
