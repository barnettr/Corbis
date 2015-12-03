using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Corbis.Web.UI.Accounts.ViewInterfaces;
using Corbis.Web.UI.Presenters.Accounts;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;

namespace Corbis.Web.UI.Accounts
{
    public partial class EditGeneral : CorbisBaseUserControl, IEditView
    {
        private EditPresenter presenter;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            country.SelectedIndexChanged += new EventHandler(Country_SelectedIndexChanged);
            presenter = new EditPresenter(this);
            if (!IsPostBack)
            {
                LoadCountries();
                LoadStates();
            }
        }

        private void LoadCountries()
        {
            country.DataTextField = "ContentValue";
            country.DataValueField = "Key";
            country.BindDataWithActions(presenter.GetCountries());
        }

        private void LoadStates()
        {
            state.DataTextField = "ContentValue";
            state.DataValueField = "Key";
            state.BindDataWithActions(presenter.GetStates());
        }

        protected void Country_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStates();
        }

        #region IEditView Implementation

        public bool ShippingNameVisible
        {
            get { return shippingNameDiv.Visible; }
            set { shippingNameDiv.Visible = value; }
        }

        public string ShippingName
        {
            get { return shippingName.Text; }
            set { shippingName.Text = value; }
        }

        public bool UserNameVisible
        {
            get { return userNameDiv.Visible; }
            set { userNameDiv.Visible = value; }
        }

        public string UserName
        {
            get { return userName.Text; }
            set { userName.Text = value; }
        }

        public bool CompanyNameVisible
        {
            get { return companyNameDiv.Visible; }
            set { companyNameDiv.Visible = value; }
        }

        public string CompanyName
        {
            get { return companyName.Text; }
            set { companyName.Text = value; }
        }

        public bool JobTitleVisible
        {
            get { return jobTitleDiv.Visible; }
            set { jobTitleDiv.Visible = value; }
        }

        public string JobTitle
        {
            get { return jobTitle.Text; }
            set { jobTitle.Text = value; }
        }

        public bool NameVisible
        {
            get { return nameDiv.Visible; }
            set { nameDiv.Visible = value; }
        }

        public string FirstName
        {
            get { return firstName.Text; }
            set { firstName.Text = value; }
        }

        public string MiddleInitial
        {
            get { return middleInitial.Text; }
            set { middleInitial.Text = value; }
        }

        public string LastName
        {
            get { return lastName.Text; }
            set { lastName.Text = value; }
        }

        public string FirstNameFurigana
        {
            get { return string.Empty; }
            set { return; }
        }

        public string LastNameFurigana
        {
            get { return string.Empty; }
            set { return; }
        }

        public bool EmailVisible
        {
            get { return emailDiv.Visible; }
            set { emailDiv.Visible = value; }
        }

        public string Email
        {
            get { return email.Text; }
            set { email.Text = value; }
        }

        public bool AddressVisible
        {
            get { return addressDiv.Visible; }
            set { addressDiv.Visible = value; }
        }

        public bool ZipVisible
        {
            set
            {
                this.ZipDiv.Visible = false;
                this.ZipLabelDiv.Visible = false;
            
            }
        }

        public string Address1
        {
            get { return address1.Text; }
            set { address1.Text = value; }
        }

        public string Address2
        {
            get { return address2.Text; }
            set { address2.Text = value; }
        }

        public string Address3
        {
            get { return address3.Text; }
            set { address3.Text = value; }
        }

        public string City
        {
            get { return city.Text; }
            set { city.Text = value; }
        }

        public string Country
        {
            get { return country.SelectedValue; }
            set
            {
                country.SelectedValue = value;
                LoadStates();
            }
        }

        public string State
        {
            get { return state.SelectedValue; }
            set { state.SelectedValue = value; }
        }

        public bool StateEnabled
        {
            get { return state.Enabled; }
            set { state.Enabled = value; }
        }

        public string Zip
        {
            get { return zip.Text; }
            set { zip.Text = value; }
        }

        public bool ZipEnabled
        {
            get { return zip.Enabled; }
            set { zip.Enabled = value; }
        }

        public bool PhoneVisible
        {
            get { return phoneDiv.Visible; }
            set { phoneDiv.Visible = value; }
        }

        public string Phone
        {
            get { return phone.Text; }
            set { phone.Text = value; }
        }

        public string Dashes
        {
            get { return Resources.Resource.Dashes; }
        }

        public string SelectOne
        {
            get { return Resources.Resource.SelectOne; }
        }

        public string Period
        {
            get { return Resources.Resource.Period; }
        }

        public bool StateCountryVisible
        {
            get
            {
                return this.countryStateZip.Visible;
            }
            set
            {
                this.addressDiv.Visible = true;
                
                this.Address1Div.Visible = false;
                this.Address2Div.Visible = false;
                this.Address3Div.Visible = false;

                this.AddressLabel1Div.Visible = false;
                this.AddressLabel2Div.Visible = false;
                this.AddressLabel3Div.Visible = false;

                this.CityLabelDiv.Visible = false;
                this.CityDiv.Visible = false;


                this.countryStateZip.Visible = value;
            }
        }

        #endregion
    }
}