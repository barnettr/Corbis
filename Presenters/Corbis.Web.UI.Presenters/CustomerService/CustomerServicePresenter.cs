using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using Corbis.Framework.Globalization;
using Corbis.Web.Authentication;
using Corbis.Web.Content;
using Corbis.Web.Entities;
using Corbis.Web.UI.CustomerService.ViewInterfaces;
using Corbis.Membership.ServiceAgents.V1;
using Corbis.Membership.Contracts.V1;
using Corbis.Office.Contracts.V1;
using Corbis.Office.ServiceAgents.V1;
using Corbis.Framework.IpToCountry;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Corbis.CommonSchema.Contracts.V1;

namespace Corbis.Web.UI.Presenters.CustomerService
{
	public class CustomerServicePresenter : BasePresenter
	{

        private RegionsContentProvider regionsContentProvider;
        private CountriesContentProvider countriesContentProvider;

        public CustomerServicePresenter()
        {
            
        }

        private IRequestImageResearch requestImageResearchView;
        public IRequestImageResearch RequestImageResearchView
        {
            get
            {
                return requestImageResearchView;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("View object must not be null");
                }
                requestImageResearchView = value;
            }
        }

        private ICustomerServiceWebServiceView customerServiceWebServiceView;
        public ICustomerServiceWebServiceView CustomerServiceWebServiceView
        {
            get
            {
                return customerServiceWebServiceView;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("View object must not be null");
                }
                customerServiceWebServiceView = value;
            }
        }

		private ICustomerServiceView customerServiceView;
		public ICustomerServiceView CustomerServiceView
		{
			get
			{
				return customerServiceView;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("View object must not be null");
				}
				customerServiceView = value;
			}
		}

		private ICustomerServiceContactView customerServiceContactView;
		public ICustomerServiceContactView CustomerServiceContactView
		{
			get
			{
				return customerServiceContactView;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("View object must not be null");
				}
				customerServiceContactView = value;
			}
		}
        
        private IOfficeContract officeAgent;
        public IOfficeContract OfficeAgent 
        { 
            get
            {
                if(officeAgent == null)
                {
                    officeAgent = new OfficeServiceAgent();
                }
                return officeAgent;

            }
            set
            {
                officeAgent = value;
            }
        }

        private IMembershipContract membershipAgent;
        public IMembershipContract MembershipAgent 
        { 
            get
            {
                if (membershipAgent == null)
                {
                    membershipAgent = new MembershipServiceAgent();
                }
                return membershipAgent;

            }
            set
            {
                membershipAgent = value;
            }
        }

        public void SubmitResearchRequest()
        {
            Corbis.Email.Contracts.V1.ResearchRequestDetail researchRequestDetail = new Corbis.Email.Contracts.V1.ResearchRequestDetail();
            
            researchRequestDetail.Description = RequestImageResearchView.ImageDescription;
            researchRequestDetail.ProjectDeadline = RequestImageResearchView.ProjectDeadline;
            researchRequestDetail.ModelRelease = (ModelReleaseForResearch)Int32.Parse(RequestImageResearchView.ModelRelease);
            researchRequestDetail.ProjectClient = RequestImageResearchView.ProjectClient;
            researchRequestDetail.POJobProject = RequestImageResearchView.JobNumber;
            researchRequestDetail.NatureOfYourBusiness = (NatureOfYourBusiness)Int32.Parse(RequestImageResearchView.NatureOfBusiness);
            researchRequestDetail.Other = RequestImageResearchView.OtherDescription;

            MembershipAgent.SendResearchRequest(researchRequestDetail, Profile.UserName);
        }

        public void GetRepresentativeOffice()
		{
			string countryCode = "";
			string regionCode = "";
			OfficeContact memberContact = null;

			//If user is not anonymous and we have a member uid
			if (!(bool)Profile.Context["IsAnonymous"] && Profile.MemberUid != Guid.Empty)
			{
				//Check that the address is of the right type and populated
				if (Profile.AddressDetail == null || Profile.AddressDetail.AddressType != AddressType.Billing || String.IsNullOrEmpty(Profile.AddressDetail.CountryCode))
				{
					//get the right type of member address
					List<MemberAddress> memberAddresses = MembershipAgent.GetMemberAddresses(Profile.MemberUid, AddressType.Billing);

					if (memberAddresses != null && memberAddresses.Count > 0)
					{
						countryCode = memberAddresses[0].CountryCode;
						regionCode = memberAddresses[0].RegionCode;
					}
				}
				else
				{
					countryCode = Profile.AddressDetail.CountryCode;
					regionCode = Profile.AddressDetail.RegionCode;
				}

				//Get the member office contact
				memberContact = MembershipAgent.GetMemberOfficeContact(Profile.MemberUid);
			}

			//if we still don't have a good country code, then just get by ip address.
			if (String.IsNullOrEmpty(countryCode))
			{
				IpToCountryLookup countryLookup = new IpToCountryLookup();
                if (CustomerServiceView != null)
                {
                    countryCode = countryLookup.GetCountry(CustomerServiceView.IpAddress);
                }
			}

            ContentItem billingCountry = this.GetCountriesForCulture("en-US").Find(new Predicate<ContentItem>(delegate(ContentItem item) { return item.Key == countryCode; }));
            OfficeCountry billingOfficeCountry = Corbis.Web.Entities.OfficeCountry.OtherLocations;
            
            if(billingCountry!=null)
            {
                try
                {
                    billingOfficeCountry = (OfficeCountry)Enum.Parse(typeof(OfficeCountry), billingCountry.ContentValue.Replace(" ", ""));
                }
                catch { }
            }
            CustomerServiceView.OfficeCountryCodeSelected = billingOfficeCountry;
            

			Office.Contracts.V1.Office office = OfficeAgent.GetOffice(countryCode, regionCode, OfficeContactType.RepresentativeOffice);
            ContentItem OfficeCountry = this.GetCountriesForCulture("en-US").Find(new Predicate<ContentItem>(delegate(ContentItem item) { return item.Key == office.CountryCode; }));

			if (office != null)
			{
				CustomerServiceView.OfficeName = office.OfficeName;
				CustomerServiceView.Address1 = office.Address1;
				CustomerServiceView.Address2 = office.Address2;
				CustomerServiceView.Address3 = office.Address3;
				CustomerServiceView.City = office.City;
				CustomerServiceView.RegionCode = office.RegionCode;
                CustomerServiceView.Country = OfficeCountry !=null ? OfficeCountry.ContentValue :office.CountryCode;
				CustomerServiceView.PostalCode = office.PostalCode;
				CustomerServiceView.FaxNumber = office.FaxNumber;
				CustomerServiceView.PhoneNumber = office.PhoneNumber;

                if (memberContact != null)
                {
                    if (!String.IsNullOrEmpty(memberContact.EmailAddress))
                    {
                        CustomerServiceView.EmailAddress = memberContact.EmailAddress;
                    }
                    CustomerServiceView.ContactFirstName = memberContact.FirstName;
                    CustomerServiceView.ContactLastName = memberContact.Lastname;
                }
                else
                {
                    CustomerServiceView.EmailAddress = office.EmailAddress;
                    CustomerServiceView.ContactFirstName = String.Empty;
                    CustomerServiceView.ContactLastName = String.Empty;
                }
			}
			else
			{
				throw new ApplicationException(String.Format("Unable to retrieve representative office information for country:{0} and region:{1}", countryCode, regionCode));
			}
		}

		public void GetContactCorbisOffice()
		{
			//If user is not anonymous and we have a member uid
			if (Profile.IsAnonymous || Profile.MemberUid == Guid.Empty)
			{
				CustomerServiceContactView.UseDefaultOfficeInfo = true;
                OfficeCountry officeCountry = OfficeCountry.OtherLocations;
				IpToCountryLookup countryLookup = new IpToCountryLookup();
				
				if (CustomerServiceContactView != null)
				{
					string countryCode = countryLookup.GetCountry(CustomerServiceContactView.IpAddress);
                    ContentItem country = this.GetCountriesForCulture("en-US").Find(new Predicate<ContentItem>(delegate(ContentItem item) { return item.Key == countryCode; }));

					if (country != null)
					{
						try
						{
							officeCountry = (OfficeCountry)Enum.Parse(typeof(OfficeCountry), country.ContentValue.Replace(" ", ""));
						}
						catch {}
					}
				}

				CustomerServiceContactView.OfficeCountry = officeCountry;
			}
			else
			{
				CustomerServiceContactView.UseDefaultOfficeInfo = false;
				string countryCode = "";
				string regionCode = "";

				//Check that the address is of the right type and populated
				if (Profile.AddressDetail == null || Profile.AddressDetail.AddressType != AddressType.Billing || String.IsNullOrEmpty(Profile.AddressDetail.CountryCode))
				{
					//get the right type of member address
					List<MemberAddress> memberAddresses = MembershipAgent.GetMemberAddresses(Profile.MemberUid, AddressType.Billing);

					if (memberAddresses != null && memberAddresses.Count > 0)
					{
						countryCode = memberAddresses[0].CountryCode;
						regionCode = memberAddresses[0].RegionCode;
					}
				}
				else
				{
					countryCode = Profile.AddressDetail.CountryCode;
					regionCode = Profile.AddressDetail.RegionCode;
				}

				//Get the member office contact
				OfficeContact memberOfficeContact = MembershipAgent.GetMemberOfficeContact(Profile.MemberUid);
               
				//if we still don't have a good country code, then just get by ip address.
				if (String.IsNullOrEmpty(countryCode))
				{
					IpToCountryLookup countryLookup = new IpToCountryLookup();
					if (CustomerServiceContactView != null)
					{
						countryCode = countryLookup.GetCountry(CustomerServiceContactView.IpAddress);
					}
				}

				Office.Contracts.V1.Office office = OfficeAgent.GetOffice(countryCode, regionCode, OfficeContactType.RepresentativeOffice);
               

				if (office != null)
				{
                    CountriesContentProvider countryContent = ContentProviderFactory.CreateProvider(ContentItems.Country) as CountriesContentProvider;
                    List<ContentItem> countries = countryContent.GetCultureSpecificCountries(Profile.CultureName);

                    CustomerServiceContactView.OfficeName = office.OfficeName;
					CustomerServiceContactView.Address1 = office.Address1;
					CustomerServiceContactView.Address2 = office.Address2;
					CustomerServiceContactView.Address3 = office.Address3;
					CustomerServiceContactView.City = office.City;
					CustomerServiceContactView.RegionCode = office.RegionCode;

                    ContentItem countryItem = countries.Find(new Predicate<ContentItem>(delegate(ContentItem item) { return office.CountryCode == item.Key; }));
                    CustomerServiceContactView.Country = (countryItem==null? office.CountryCode: countryItem.ContentValue);
					CustomerServiceContactView.PostalCode = office.PostalCode;
					CustomerServiceContactView.FaxNumber = office.FaxNumber;
					CustomerServiceContactView.PhoneNumber = office.PhoneNumber;

					if (memberOfficeContact != null)
					{
						if (!String.IsNullOrEmpty(memberOfficeContact.EmailAddress))
						{
							CustomerServiceContactView.EmailAddress = memberOfficeContact.EmailAddress;
						}
						CustomerServiceContactView.ContactFirstName = memberOfficeContact.FirstName;
						CustomerServiceContactView.ContactLastName = memberOfficeContact.Lastname;
					}
					else
					{
						CustomerServiceContactView.EmailAddress = office.EmailAddress;
						CustomerServiceContactView.ContactFirstName = String.Empty;
						CustomerServiceContactView.ContactLastName = String.Empty;
					}
				}
				else
				{
					throw new ApplicationException(String.Format("Unable to retrieve representative office information for country:{0} and region:{1}", countryCode, regionCode));
				}
			}
		}

        public string GetRepresentativeOfficePhone()
        {
            string countryCode = "";
            string regionCode = "";

            //If user is not anonymous and we have a member uid
            if (!Profile.IsAnonymous && Profile.MemberUid != Guid.Empty)
            {
                //Check that the address is of the right type and populated
                if (Profile.AddressDetail == null || Profile.AddressDetail.AddressType != AddressType.Billing || String.IsNullOrEmpty(Profile.AddressDetail.CountryCode))
                {
                    //get the right type of member address
                    List<MemberAddress> memberAddresses = MembershipAgent.GetMemberAddresses(Profile.MemberUid, AddressType.Billing);

                    if (memberAddresses != null && memberAddresses.Count > 0)
                    {
                        countryCode = memberAddresses[0].CountryCode;
                        regionCode = memberAddresses[0].RegionCode;
                    }
                }
                else
                {
                    countryCode = Profile.AddressDetail.CountryCode;
                    regionCode = Profile.AddressDetail.RegionCode;
                }
            }

            //if we still don't have a good country code, then just get by ip address.
            if (String.IsNullOrEmpty(countryCode))
            {
                IpToCountryLookup countryLookup = new IpToCountryLookup();
                if (CustomerServiceView != null)
                {
                    countryCode = countryLookup.GetCountry(CustomerServiceView.IpAddress);
                }
            }

            Office.Contracts.V1.Office office = OfficeAgent.GetOffice(countryCode, regionCode, OfficeContactType.RepresentativeOffice);

            if (office != null)
            {
                return office.PhoneNumber;
            }
            else
            {
                throw new ApplicationException(String.Format("Unable to retrieve representative office phone information for country:{0} and region:{1}", countryCode, regionCode));
            }
        }
   

        public void PopulateGetInTouchForm()
        {
            if (!Profile.IsAnonymous)
            {
                CustomerServiceView.FirstName = Profile.FirstName;
                CustomerServiceView.LastName = Profile.LastName;
                CustomerServiceView.Email = Profile.Email;
                CustomerServiceView.Telephone = Profile.BusinessPhoneNumber;
                CustomerServiceView.CountryList = this.GetCountries();
                CustomerServiceView.CountryCodeSelected = Profile.CountryCode;
                CustomerServiceView.StateList = this.GetStates();
                if(Profile.AddressDetail != null)
                {
                    CustomerServiceView.StateCodeSelected = Profile.AddressDetail.RegionCode;
                }
            }
            else
            {
                CustomerServiceView.CountryList = this.GetCountries();
            }
	    }
 

        public void SendCustomerFeedbackEmail()
        {
            try
            {
                Corbis.Email.Contracts.V1.CustomerServiceDetail emailDetail = new Corbis.Email.Contracts.V1.CustomerServiceDetail();
                emailDetail.About = CustomerServiceView.Subject;

                if (!string.IsNullOrEmpty(CustomerServiceView.CountryCodeSelected))
                {
                    emailDetail.CountryCode = CustomerServiceView.CountryCodeSelected;
                    emailDetail.CountryName = CustomerServiceView.CountryNameSelected;
                }

                if (!string.IsNullOrEmpty(CustomerServiceView.StateCodeSelected))
                {
                    emailDetail.RegionCode = CustomerServiceView.StateCodeSelected;
                    emailDetail.RegionName = CustomerServiceView.StateNameSelected;
                }

                emailDetail.FirstName = CustomerServiceView.FirstName;
                emailDetail.LastName = CustomerServiceView.LastName;
                emailDetail.Email = CustomerServiceView.Email;
                emailDetail.Phone = CustomerServiceView.Telephone;
                emailDetail.Comments = CustomerServiceView.Comments;

                emailDetail.Page = CustomerServiceView.PageUrl;
                emailDetail.Browser = CustomerServiceView.Browser;
                emailDetail.BrowserVersion = CustomerServiceView.BrowserVersion;
                emailDetail.Plateform = CustomerServiceView.Plateform;

                MembershipAgent.SendCustomerServiceEmail(Profile.UserName, emailDetail, Language.CurrentCulture.Name);
            }
            catch (Exception ex)
            {
                HandleException(ex, CustomerServiceView.LoggingContext, "EditPresenter: SendCustomerFeedbackEmail()");
                throw;
            }
        }

        public List<ContentItem> GetStatesFromWebService()
        {
            try
            {
                List<ContentItem> results = new List<ContentItem>();

                string countryCode = CustomerServiceWebServiceView.Country;

                regionsContentProvider = ContentProviderFactory.CreateProvider(ContentItems.Region) as RegionsContentProvider;
                results = regionsContentProvider.GetRegionsByCountryCode(countryCode);

                return results;
            }
            catch (Exception ex)
            {
                HandleException(ex, CustomerServiceView.LoggingContext, "EditPresenter: GetRegions()");
                throw;
            }
        }
        
        public List<ContentItem> GetStates()
        {
            try
            {
                List<ContentItem> results = new List<ContentItem>();

                string countryCode = CustomerServiceView.CountryCodeSelected;

                if (!string.IsNullOrEmpty(countryCode))
                {
                    regionsContentProvider = ContentProviderFactory.CreateProvider(ContentItems.Region) as RegionsContentProvider;
                    results = regionsContentProvider.GetRegionsByCountryCode(countryCode);
                    if (results.Count == 0)
                    {
                        results.Insert(0, new ContentItem(String.Empty, CustomerServiceView.Dashes));
                        CustomerServiceView.StateEnabled = false;
                    }
                    else
                    {
                        results.Insert(0, new ContentItem(String.Empty, CustomerServiceView.SelectOne));
                        CustomerServiceView.StateEnabled = true;
                    }
                    
                }
                else
                {
                    results.Insert(0, new ContentItem(String.Empty, CustomerServiceView.Dashes));
                    CustomerServiceView.StateEnabled = false;
                }

                return results;
            }
            catch (Exception ex)
            {
                HandleException(ex, CustomerServiceView.LoggingContext, "EditPresenter: GetRegions()");
                throw;
            }
        }

        public List<ContentItem> GetCountries()
        {
            try
            {
                countriesContentProvider = ContentProviderFactory.CreateProvider(ContentItems.Country) as CountriesContentProvider;
                return countriesContentProvider.GetCountries();
            }
            catch (Exception ex)
            {
                HandleException(ex, CustomerServiceView.LoggingContext, "CustomerServicePresenter: GetCountries()");
                throw;
            }
        }

        public List<ContentItem> GetCountriesForCulture(string culture)
        {
            try
            {
                countriesContentProvider = ContentProviderFactory.CreateProvider(ContentItems.Country) as CountriesContentProvider;
                return countriesContentProvider.GetCultureSpecificCountries(culture);
            }
            catch (Exception ex)
            {
                HandleException(ex, CustomerServiceView.LoggingContext, "CustomerServicePresenter: GetCountriesInEnglish()");
                throw;
            }
        }
	}
}
