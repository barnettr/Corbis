using System;
using System.Collections.Generic;
using System.Web.UI;
using Corbis.DisplayText.Contracts.V1;
using Languages = Corbis.Framework.Globalization.Language;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.UI.Controls;
using Corbis.Web.Entities;
using Corbis.Web.UI.Accounts.ViewInterfaces;
using Corbis.Web.UI.Presenters.Accounts;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Corbis.Web.Utilities;
using Corbis.Web.Validation;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Framework.IpToCountry;
using System.Configuration;
using System.Web;

namespace Corbis.Web.UI.Accounts
{
    public partial class EditBusinessInformation : CorbisBasePage, IEditBusinessInformationView, IViewPropertyValidator, IValidationHubErrorSetter
    {
        AccountsPresenter presenter;

        private enum QueryString
        {
            Pane
        }

        private static string BUSINESSADDRESSCOUNTRYCODE = "BusinessAddressCountryCode";

        protected override void OnInit(EventArgs e)
        {
            this.RequiresSSL = true;
            base.OnInit(e);
            presenter = new AccountsPresenter(this);
            HookupEventHandlers();
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Accounts, "Accounts");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateJobTitleDropdown();
                presenter.PopulateEditBusinessInformation();
            }
        }

        private void HookupEventHandlers()
        {
            businessAddress.CountryDataChange += new EventHandler(BusinessAddress_CountryDataChange);
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            if (presenter.SaveBusinessInformation())
            {
                if (CheckChinaIPAddress() || ChineseURL())
                {
                    if (!businessAddress.Country.ToUpper().Equals("CN"))
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "saveClicked", "parent.CorbisUI.MyProfile.refreshBusinessInfoPane();parent.MochaUI.CloseModal('editBusinessInformation');parent.CorbisUI.MyProfile.OpenSuccessChinaPopup();", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "saveClicked", "parent.CorbisUI.MyProfile.refreshBusinessInfoPane();parent.MochaUI.CloseModal('editBusinessInformation');parent.CorbisUI.MyProfile.OpenSuccessPopup();", true);
                    }
                }
                else if (ViewState[BUSINESSADDRESSCOUNTRYCODE].ToString() == this.CountryCode)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "saveClicked", "parent.CorbisUI.MyProfile.refreshBusinessInfoPane();parent.MochaUI.CloseModal('editBusinessInformation');parent.CorbisUI.MyProfile.OpenSuccessPopup();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "saveClicked", "parent.window.location = '" + SiteUrls.MyProfile + "?Pane=businessInfoPane'", true);
                }
            }
        }

        protected void BusinessAddress_CountryDataChange(object sender, EventArgs e)
        {
            businessAddress.RegionData = presenter.GetRegions(businessAddress.Country);
        }

        public bool CheckChinaIPAddress()
        {
            IpToCountryLookup countryLookup = new IpToCountryLookup();
            string CountryCode = countryLookup.GetCountry(ClientIPHelper.GetClientIpAddress());
            return (CountryCode.ToLower().Equals("cn"));
        }

        public static bool ChineseURL()
        {
            bool isChineseUser = false;
            string host = HttpContext.Current.Request.Url.Host;
            string chinaHostName = ConfigurationManager.AppSettings["CnHttpHost"];
            string chinaSecureHostName = ConfigurationManager.AppSettings["CnHttpsHost"];

            if (string.IsNullOrEmpty(chinaHostName) || string.IsNullOrEmpty(chinaSecureHostName))
            {
                throw new ConfigurationErrorsException("China host name not configured");
            }

            if (host.Equals(chinaHostName, StringComparison.InvariantCultureIgnoreCase) || host.Equals(chinaSecureHostName, StringComparison.InvariantCultureIgnoreCase))
            {
                isChineseUser = true;
            }
            return isChineseUser;
        }

        private void PopulateJobTitleDropdown()
        {
            this.jobTitle.PromptText = Resources.Resource.SelectOne;
            this.jobTitle.DataSource = GetEnumDisplayValues<JobTitle>(false);
            this.jobTitle.DataValueField = "Id";
            this.jobTitle.DataTextField = "Text";
            this.jobTitle.DataBind();
        }

        #region IEditBusinessInformation Implementation

        [PropertyControlMapper("companyName")]
        public string CompanyName
        {
            get { return companyName.Text; }
            set { companyName.Text = value; }
        }

        [PropertyControlMapper("jobTitle")]
        public JobTitle JobTitle
        {
            get
            {
                JobTitle jobTitleType = JobTitle.Unknown;
                int jobTitleId;
                if (int.TryParse(jobTitle.SelectedValue, out jobTitleId)
                    && Enum.IsDefined(typeof(JobTitle), jobTitleId))
                {
                    jobTitleType = (JobTitle)jobTitleId;
                }
                return jobTitleType;
            }
            set
            {
                jobTitle.SelectedValue = value.GetHashCode().ToString();
            }
        }

        [PropertyControlMapper("phone")]
        public string BusinessPhoneNumber
        {
            get { return phone.Text; }
            set { phone.Text = value; }
        }


        public MemberAddress Address
        {
            set
            {
                MemberAddress address = value;
                businessAddress.CountryData = presenter.GetCountries();
                businessAddress.LoadAddressFromMemberAddress(address);
                if (address != null)
                {
                    CountryCode = address.CountryCode;
                    ViewState[BUSINESSADDRESSCOUNTRYCODE] = CountryCode;
                    businessAddress.RegionData = presenter.GetRegions(address.CountryCode);
                    RegionCode = address.RegionCode;

                }
            }
        }

        /// <summary>
        /// Used to expose the Billing Address control for validation
        /// </summary>
        /// <value>The mailing address control.</value>
        public Corbis.Web.UI.Controls.Address BusinessAddress
        {
            get { return businessAddress; }
        }

        [ChildControlPropertyMapper("BusinessAddress", "Address1")]
        public string Address1
        {
            get { return businessAddress.Address1; }
        }

        [ChildControlPropertyMapper("BusinessAddress", "Address2")]
        public string Address2
        {
            get { return businessAddress.Address2; }
        }

        [ChildControlPropertyMapper("BusinessAddress", "Address3")]
        public string Address3
        {
            get { return businessAddress.Address3; }
        }

        [ChildControlPropertyMapper("BusinessAddress", "City")]
        public string City
        {
            get { return businessAddress.City; }
        }

        [ChildControlPropertyMapper("BusinessAddress", "Region")]
        public string RegionCode
        {
            get { return businessAddress.Region; }
            set { businessAddress.Region = value; }
        }

        [ChildControlPropertyMapper("BusinessAddress", "Country")]
        public string CountryCode
        {
            get { return businessAddress.Country; }
            set { businessAddress.Country = value; }
        }

        [ChildControlPropertyMapper("BusinessAddress", "PostalCode")]
        public string PostalCode
        {
            get { return businessAddress.PostalCode; }
        }


        #endregion

        #region Validation Error Overrides

        public override void SetValidationHubError<T>(Control control, T errorEnumValue, bool showInSummary, bool showHilite)
        {
            string errorMessage = GetEnumDisplayText<T>(errorEnumValue, Resources.Accounts.ResourceManager);
            this.vHub.SetError(control, errorMessage, showInSummary, showHilite);
        }
        #endregion

    }
}
