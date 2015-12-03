using System;
using System.Collections.Generic;
using System.Web.UI;
using Corbis.DisplayText.Contracts.V1;
using Languages = Corbis.Framework.Globalization.Language;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.Entities;
using Corbis.Web.UI.Accounts.ViewInterfaces;
using Corbis.Web.UI.Presenters.Accounts;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;

namespace Corbis.Web.UI.Accounts
{
    public partial class EditBusinessInformation : CorbisBaseUserControl, IEditBusinessInformationView
    {
        private enum QueryString
        {
            Pane
        }

        AccountsPresenter presenter;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            presenter = new AccountsPresenter(this);
            HookupEventHandlers();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBusinessInformation();
            }
        }

        private void HookupEventHandlers()
        {
            businessAddress.CountryDataChange += new EventHandler(BusinessAddress_CountryDataChange);
            editBusinessInformationSave.Click += new EventHandler(Save_Click);
            editBusinessInformationCancel.Click += new EventHandler(Cancel_Click);
            editBusinessInformationClose.Click += new ImageClickEventHandler(Cancel_Click); 
        }

        public void LoadBusinessInformation()
        {
            businessAddress.CountryData = presenter.GetCountries();
            MemberAddress address = presenter.GetBillingAddress();
            if (address != null)
            {
                businessAddress.RegionData = presenter.GetRegions(address.CountryCode);
                businessAddress.LoadAddressFromMemberAddress(address);
            }
            presenter.LoadBusinessInformation();
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            Page.Validate(editBusinessInformationValidationSummary.ValidationGroup);
            if (editBusinessInformationValidationSummary.GetErrorMessages() == null &&
                presenter.SaveBusinessInformation())
            {
                Response.Redirect(string.Format("{0}?{1}={2}", SiteUrls.MyProfile, QueryString.Pane.ToString(), "businessInfoPane"));
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("{0}?{1}={2}", SiteUrls.MyProfile, QueryString.Pane.ToString(), "businessInfoPane"));
        }

        protected void BusinessAddress_CountryDataChange(object sender, EventArgs e)
        {
            businessAddress.RegionData = presenter.GetRegions(businessAddress.Country);
        }

        #region IEditBusinessInformation Implementation

        public MemberAddress BillingAddress
        {
            get { return businessAddress.ExportAddress(); }
            set { businessAddress.LoadAddressFromMemberAddress(value); }
        }

        public string CompanyName
        {
            get { return companyName.Text; }
            set { companyName.Text = value; }
        }

        public string JobTitle
        {
            get { return jobTitle.Text; }
            set { jobTitle.Text = value; }
        }

        public string Phone
        {
            get { return phone.Text; }
            set { phone.Text = value; }
        }

        public string Address1
        {
            get { return businessAddress.Address1; }
            set { businessAddress.Address1 = value; }
        }

        public string Address2
        {
            get { return businessAddress.Address2; }
            set { businessAddress.Address2 = value; }
        }

        public string Address3
        {
            get { return businessAddress.Address3; }
            set { businessAddress.Address3 = value; }
        }

        public string City
        {
            get { return businessAddress.City; }
            set { businessAddress.City = value; }
        }

        public string Country
        {
            get { return businessAddress.Country; }
            set { businessAddress.Country = value; }
        }

        public string Region
        {
            get { return businessAddress.Region; }
            set { businessAddress.Region = value; }
        }

        public string PostalCode
        {
            get { return businessAddress.PostalCode; }
            set { businessAddress.PostalCode = value; }
        }

        protected global::System.Web.UI.WebControls.CustomValidator nonAsciiValidator;
        public string ShowUnicodeErrorMessageResourceKey
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    nonAsciiValidator.ErrorMessage = GetGlobalResourceObject("Resource", value).ToString();
                    nonAsciiValidator.IsValid = false;
                }
                else
                {
                    nonAsciiValidator.ErrorMessage = String.Empty;
                    nonAsciiValidator.IsValid = true;
                }
            }
        }

        #endregion
    }
}
