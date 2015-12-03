using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.Threading;
using System.Web.UI;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.UI.CustomerService.ViewInterfaces;
using Corbis.Web.UI.Presenters.CustomerService;
using Resources;
using Corbis.Web.Utilities;
using Corbis.Web.Entities;
using Languages = Corbis.Framework.Globalization.Language;

namespace Corbis.Web.UI.CustomerService
{
    public partial class CustomerService : CorbisBasePage, ICustomerServiceView
    {
        private Member member;
        private CustomerServicePresenter presenter;
        private string state;


        protected override void OnInit(EventArgs e)
        {
            RequiresSSL = false;
            base.OnInit(e);
            presenter = new CustomerServicePresenter();
            presenter.CustomerServiceView = this;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            contactName.Text = GetLocalResourceObject("contactName").ToString();
            
           
            
            if (!IsPostBack)
            {
                presenter.GetRepresentativeOffice();
                presenter.PopulateGetInTouchForm();
                if (!string.IsNullOrEmpty(stateList.SelectedValue))
                {
                    provinceCode.Value = stateList.SelectedValue;
                }

                PopulateOfficeDropdown();
                DisplayOfficeLocation();
                PopulateAboutDropdown();
            }

			if (Languages.CurrentLanguage == Languages.Japanese || Languages.CurrentLanguage == Languages.ChineseSimplified)
            {
                asianFirstName.Visible = true;
                asianLastName.Visible = true;
                generalFirstName.Visible = false;
                generalLastName.Visible = false;
            }
            else
            {
                asianFirstName.Visible = false;
                asianLastName.Visible = false;
                generalFirstName.Visible = true;
                generalLastName.Visible = true;
            }
            if (Profile.IsChinaUser)
            {
                Answer2.Text = GetLocalResourceObject("AnswerChina2").ToString();
                Answer5.Text = GetLocalResourceObject("AnswerChina5").ToString();
                Answer7.Text = GetLocalResourceObject("AnswerChina7").ToString();
                Answer10.Text = GetLocalResourceObject("AnswerChina10").ToString();
                AccordionPane4.Visible = false;
                AccordionPane5.Visible = false;
            }
            else
            {
                Answer2.Text = GetLocalResourceObject("Answer2").ToString();
                Answer5.Text = GetLocalResourceObject("Answer5").ToString();
                Answer7.Text = GetLocalResourceObject("Answer7").ToString();
                Answer10.Text = GetLocalResourceObject("Answer10").ToString();
                AccordionPane4.Visible = true;
                AccordionPane5.Visible = true;

            }
        }

        private void PopulateAboutDropdown()
        {
            this.aboutDropDown.DataSource = GetEnumDisplayValues<AboutCustomerFeedback>();
            this.aboutDropDown.DataValueField = "Id";
            this.aboutDropDown.DataTextField = "Text";
            this.aboutDropDown.DataBind();
        }

        private void PopulateOfficeDropdown()
        {
            List<DisplayValue<OfficeCountry>> listOfOffices = GetEnumDisplayValues<OfficeCountry>();
            DisplayValue<OfficeCountry> usaCountry,canadaCountry;
            if (!(OfficeCountry.UnitedStates.Equals(OfficeCountryCodeSelected) || OfficeCountry.Canada.Equals(OfficeCountryCodeSelected)))
            {
                usaCountry = listOfOffices.Find(new Predicate<DisplayValue<OfficeCountry>>(delegate(DisplayValue<OfficeCountry> item) { return item.Id == OfficeCountry.UnitedStates.GetHashCode(); }));
                canadaCountry = listOfOffices.Find(new Predicate<DisplayValue<OfficeCountry>>(delegate(DisplayValue<OfficeCountry> item) { return item.Id == OfficeCountry.Canada.GetHashCode(); }));
                usaCountry.Ordinal = 0;
                canadaCountry.Ordinal = 0;
                SortAndRemoveNullOrdinals(listOfOffices);
            }
            this.officeList.DataSource = listOfOffices;
            this.officeList.DataValueField = "Id";
            this.officeList.DataTextField = "Text";
            this.officeList.DataBind();
            this.officeList.SelectedValue = Convert.ToString(OfficeCountryCodeSelected.GetHashCode());
        }

        protected void officeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayOfficeLocation();
        }

        #region ICustomerServiceView implementation

        public Member DumbMember
        {
            get { return member; }
            set { member = value; }
        }

        public string IpAddress
        {
            get { return ClientIPHelper.GetClientIpAddress(); }
        }

        public string OfficeName
        {
            set { officeName.Text = value; }
        }

        public string Address1
        {
            set { address1.Text = value; }
        }

        public string Address2
        {
            set 
            { 
                if (!string.IsNullOrEmpty(value)) address2.Text = string.Concat(value, "<br />"); 
            }
        }

        public string Address3
        {
            set 
            { 
                if (!string.IsNullOrEmpty(value)) address3.Text = string.Concat(value, "<br />"); 
            }
        }

        public string City
        {
            set { city.Text = value; }
        }

        public string RegionCode
        {
            set 
            { 
                if (!string.IsNullOrEmpty(value)) regionCode.Text = string.Concat(", ", value); 
            }
        }

        public string Country
        {
            set { country.Text = value; }
        }

        public string PostalCode
        {
            set { postalCode.Text = value; }
        }

        public string FaxNumber
        {
            set { faxNumber.Text = value; }
        }

        public string PhoneNumber
        {
            set { phoneNumber.Text = value; }
        }

        public string EmailAddress
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    emailAddress.NavigateUrl = string.Concat("mailto:", value);
                    emailAddress.Text = value;
                }
                else
                {
                    emailDiv.Visible = false;
                }
            }
        }

        public string ContactFirstName
        {
            set 
            { 
                contactName.Text = contactName.Text.Replace("$firstName", value).Trim(); 
            }
        }

        public string ContactLastName
        {
            set 
            { 
                contactName.Text = contactName.Text.Replace("$lastName", value).Trim(); 
            }
        }

        public string FirstName
        {
            get
            {
                if (Languages.CurrentLanguage == Languages.Japanese || Languages.CurrentLanguage == Languages.ChineseSimplified)
                {
                    return firstNameAsian.Text;
                }
                else
                {
                    return firstName.Text;
                }
            }
            set
            {
                if (Languages.CurrentLanguage == Languages.Japanese || Languages.CurrentLanguage == Languages.ChineseSimplified)
                {
                    firstNameAsian.Text = value;
                }
                else
                {
                    firstName.Text = value;
                }
            }
        }

        public string LastName
        {
            get
            {
                if (Languages.CurrentLanguage == Languages.Japanese || Languages.CurrentLanguage == Languages.ChineseSimplified)
                {
                    return lastNameAsian.Text;
                }
                else
                {
                    return lastName.Text;
                }
            }
            set
            {
                if (Languages.CurrentLanguage == Languages.Japanese || Languages.CurrentLanguage == Languages.ChineseSimplified)
                {
                    lastNameAsian.Text = value;
                }
                else
                {
                    lastName.Text = value;
                }
            }
        }

        public string Email
        {
            get { return email.Text; }
            set { email.Text = value; }
        }

        public string State
        {
            get { return state; }
            set { state = value; }
        }

        public string Comments
        {
            get { return comments.Text; }
        }

        public string Subject
        {
            get { return aboutDropDown.SelectedItem.ToString(); }
        }

        public string Dashes
        {
            get { return Resource.Dashes; }
        }

        public bool StateEnabled
        {
            get { return stateList.Enabled; }
            set { stateList.Enabled = value; }
        }

        public string SelectOne
        {
            get { return Resource.SelectOne; }
        }

        public List<ContentItem> StateList
        {
            set
            {
                stateList.DataTextField = "ContentValue";
                stateList.DataValueField = "Key";
                stateList.BindDataWithActions(value);
            }
        }
        
        public List<ContentItem> CountryList
        {
            set
            {
                countryList.DataTextField = "ContentValue";
                countryList.DataValueField = "Key";
                countryList.BindDataWithActions(value);
            }
        }

        public string CountrySelected
        {
            get { return countryList.SelectedValue; }
            set
            {
                countryList.SelectedValue = value;
            }
        }

     

        string ICustomerServiceView.Telephone
        {
            get { return telephone.Text; }
            set { telephone.Text = value; }
        }

        public string CountryCodeSelected
        {
            get { return countryList.SelectedValue; }
            set
            {
                countryList.SelectedValue = value;
            }
        }
        private OfficeCountry _OfficeCountryCodeSelected;
        public OfficeCountry OfficeCountryCodeSelected
        {
            get { return _OfficeCountryCodeSelected; }
            set
            {
                _OfficeCountryCodeSelected = value;
            }
        }

        public string StateCodeSelected
        {
            get { return provinceCode.Value; }
            set { stateList.SelectedValue = value; }
        }

        public string CountryNameSelected
        {
            get { return countryList.SelectedItem.Text; }
        }

        public string StateNameSelected
        {
            get { return provinceName.Value; }
        }

        public string Browser
        {
            get { return Page.Request.Browser.Browser; }
        }

        public string BrowserVersion
        {
            get { return Page.Request.Browser.Version; }
        }

        public string Plateform
        {
            get { return Page.Request.Browser.Platform; }
        }

        public string PageUrl
        {
            get { return Page.Request.Url.AbsoluteUri; }
        }

        #endregion

        #region private methods

        

        protected void Save_Click(object sender, EventArgs e)
        {
            presenter.SendCustomerFeedbackEmail();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "clearGetInTouchForm", "CorbisUI.QueueManager.domReady.addItem('clearGetInTouchForm',function(){CorbisUI.CustomerService.clearGetInTouchForm();}); ", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenThankYouPopup", "CorbisUI.QueueManager.domReady.addItem('openThankYouPopup',function(){CorbisUI.CustomerService.OpenThankYouPopup();});", true);
        }

        private void DisplayOfficeLocation()
        {
            OfficeCountry officeCountrySelected = (OfficeCountry)int.Parse(officeList.SelectedValue);
            string officeInfo = Offices.ResourceManager.GetString(officeCountrySelected.ToString());

            officeLocation.Text = officeInfo;
        }

        #endregion

    }
}
