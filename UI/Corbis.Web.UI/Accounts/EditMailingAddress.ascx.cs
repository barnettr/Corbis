using System;
using System.Collections.Generic;
using System.Web.UI;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.Entities;
using Corbis.Web.UI.Presenters.Accounts;
using Corbis.Web.UI.Accounts.ViewInterfaces;

namespace Corbis.Web.UI.Accounts
{
    public partial class EditMailingAddress : CorbisBaseUserControl, IEditMailingInformationView 
    {
        private Guid addressUid;
        private AccountsEditMode editMode;
        private MemberAddress mailingAddress;
        private AccountsPresenter presenter;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            HookupEventHandlers();
        }

        private void HookupEventHandlers()
        {
            sameAsBusiness.CheckedChanged += new EventHandler(SameAsBusiness_CheckedChanged);
            editMailingAddressSave.Click += new EventHandler(Save_Click);
            editMailingAddressCancel.Click += new EventHandler(Close_Click);
            editMailingAddressClose.Click += new ImageClickEventHandler(Close_Click);
            mailingAddressControl.CountryDataChange += new EventHandler(MailingAddressControl_CountryDataChange);
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            presenter = new AccountsPresenter(this);
            if (!IsPostBack)
            {
                mailingAddressControl.CountryData = presenter.GetCountries();
            }
        }

        public void SetCreateMode()
        {
            EditMode = AccountsEditMode.Create;
            editMailingAddressTitle.Visible = false;
        }

        public void SetUpdateMode()
        {
            EditMode = AccountsEditMode.Update;
            addMailingAddressTitle.Visible = false;
            presenter.PopulateMailingAddress();
            mailingAddressControl.Region = mailingAddress.RegionCode;
        }

        #region Event Handlers

        protected void MailingAddressControl_CountryDataChange(object sender, EventArgs e)
        {
            mailingAddressControl.RegionData = presenter.GetRegions(mailingAddressControl.Country);
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            bool success = false;
            Page.Validate(editMailingAddressValidationSummary.ValidationGroup);
            if (editMailingAddressValidationSummary.GetErrorMessages() == null)
            {
                SetMailingAddressFromFormData();
                if (EditMode == AccountsEditMode.Create)
                {
                    mailingAddress.AddressUid = Guid.NewGuid();
                    success = presenter.AddMailingAddress();
                    Profile.SnailmailPreference = true;
                    Profile.Save();
                }
                else
                {
                    success = presenter.UpdateMailingAddress();
                }

                if (success)
                {
                    this.Close_Click(sender, e);
                }
            }
        }

        #endregion

        #region Private Methods

        private void SetMailingAddressFromFormData()
        {
            mailingAddress = new MemberAddress();
            mailingAddress.Address1 = mailingAddressControl.Address1;
            mailingAddress.Address2 = mailingAddressControl.Address2;
            mailingAddress.Address3 = mailingAddressControl.Address3;
            mailingAddress.AddressType = AddressType.Mailing;
            mailingAddress.City = mailingAddressControl.City;          
            mailingAddress.CountryCode = mailingAddressControl.Country;
            mailingAddress.IsDefaultForType = true;            
            mailingAddress.PostalCode = mailingAddressControl.PostalCode;
            mailingAddress.RegionCode = mailingAddressControl.Region;
        }

        private void GetMailingAddressData()
        {
            MemberAddress address = presenter.GetMailingAddress();

            if (address != null)
            {
                mailingAddressControl.Address1 = address.Address1;
                mailingAddressControl.Address2 = address.Address2;
                mailingAddressControl.Address3 = address.Address3;
                mailingAddressControl.City = address.City;
                mailingAddressControl.Country = address.CountryCode;
                mailingAddressControl.PostalCode = address.PostalCode;
                mailingAddressControl.Region = address.RegionCode;
            }
        }

        protected void SameAsBusiness_CheckedChanged(object sender, EventArgs e)
        {
            MemberAddress address = presenter.GetBillingAddress();
            if (sameAsBusiness.Checked && address != null)
            {
                mailingAddressControl.Address1 = address.Address1;
                mailingAddressControl.Address2 = address.Address2;
                mailingAddressControl.Address3 = address.Address3;
                mailingAddressControl.City = address.City;
                mailingAddressControl.Country = address.CountryCode;
                mailingAddressControl.PostalCode = address.PostalCode;
                mailingAddressControl.RegionData = presenter.GetRegions(mailingAddressControl.Country);
                mailingAddressControl.Region = address.RegionCode;
            }
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("{0}?{1}={2}", SiteUrls.MyProfile, "Pane", "preferencesPane"));
        }

        #endregion

        #region IEditmailingInformationView Members

        /// <summary>
        /// Gets or sets the address uid of the mailing address to edit.
        /// </summary>
        /// <value>The address uid.</value>
        public Guid AddressUid
        {
            get { return addressUid; }
            set { addressUid = value; }
        }

        /// <summary>
        /// Gets the edit mode.
        /// </summary>
        /// <value>The edit mode.</value>
        public AccountsEditMode EditMode
        {
            get { return editMode; }
            set { editMode = value; }
        }

        /// <summary>
        /// Gets or sets the mailing address to edit and loads the address control accordingly.
        /// </summary>
        /// <value>The mailing address.</value>
        public MemberAddress MailingAddress
        {
            get
            {
                return mailingAddress;
            }
            set
            {
                mailingAddress = value;
                mailingAddressControl.LoadAddressFromMemberAddress(mailingAddress);
                //if (mailingAddress != null)
                //{
                //    this.addressName.Text = mailingAddress.Name;
                //    this.companyName.Text = mailingAddress.CompanyName;
                //}
            }
        }

        /// <summary>
        /// Gets the selected country.
        /// </summary>
        /// <value>The country code.</value>
        public string CountryCode
        {
            get { return mailingAddressControl.Country; }
        }

        /// <summary>
        /// Gets or sets the List of Regions to display.
        /// </summary>
        /// <value>The regions.</value>
        public List<ContentItem> Regions
        {
            get { return (List<ContentItem>)mailingAddressControl.RegionData; }
            set { mailingAddressControl.RegionData = value; }
        }

        protected global::System.Web.UI.WebControls.CustomValidator nonAsciiValidator;
        public bool ShowUnicodeErrorMessage
        {
            set
            {
                if (value)
                {
                    nonAsciiValidator.ErrorMessage = GetGlobalResourceObject("Resource", "ContainsNonAsciiCharacters").ToString();
                }
                else
                {
                    nonAsciiValidator.ErrorMessage = String.Empty;

                }
                nonAsciiValidator.IsValid = !value;
            }
        }

        #endregion
    }
}
