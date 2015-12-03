using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.Entities;
using Corbis.Web.UI.Presenters.Accounts;
using Corbis.Web.UI.Accounts.ViewInterfaces;

namespace Corbis.Web.UI.Accounts
{
    public partial class EditShippingAddress2 : CorbisBaseUserControl, IEditShippingInformationView
    {
        private AccountsEditMode editMode;
        private MemberAddress shippingAddress;
        private AccountsPresenter presenter;
        public event CommandEventHandler ShippingAddressAdded;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            HookupEventHandlers();
            if (EditMode == AccountsEditMode.Delete)
            {
                modalPopupPanel.Attributes["class"] = "ModalPopupPanelDialog";
            }

            editMode = AccountsEditMode.Create;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            presenter = new AccountsPresenter(this);
            if (!IsPostBack)
            {
                shippingAddressControl.CountryData = presenter.GetCountries();
            }
            this.sameAsBusinessAddr.CheckedChanged += new EventHandler(sameAsBusinessAddr_CheckedChanged);
        }

        public void SetCreateMode()
        {
            //EditMode = AccountsEditMode.Create;
            //presenter.PopulateShippingAddress();
            //editShippingAddressTitle.Text = GetLocalResourceObject("addShippingAddressTitle").ToString();
        }

        public void SetUpdateMode()
        {
            //EditMode = AccountsEditMode.Update;
            //presenter.PopulateShippingAddress();
            //editShippingAddressTitle.Text = GetLocalResourceObject("editShippingAddressTitle").ToString();
        }

        public void SetDeleteMode()
        {
            //EditMode = AccountsEditMode.Delete;
            //presenter.PopulateShippingAddress();
            //editShippingAddressTitle.Text = GetLocalResourceObject("deleteShippingAddressTitle").ToString();
            //modalPopupPanel.Attributes["class"] = "ModalPopupPanelDialog";
        }

        private void HookupEventHandlers()
        {
            editShippingAddressSave.Command += new CommandEventHandler(Save_Click);
            editShippingAddressClose.Click += new ImageClickEventHandler(Close_Click);
            editShippingAddressCancel.Click += new EventHandler(Close_Click);
            shippingAddressControl.CountryDataChange += new EventHandler(ShippingAddressControl_CountryDataChange);
            deleteYes.Command += new CommandEventHandler(DeleteAddress);
            deleteNo.Click += new EventHandler(Close_Click);
        }

        #region Event Handlers

        protected void ShippingAddressControl_CountryDataChange(object sender, EventArgs e)
        {
            shippingAddressControl.RegionData = presenter.GetRegions(shippingAddressControl.Country);
        }

        protected void DeleteAddress(object sender, CommandEventArgs e)
        {
            //presenter.DeleteShippingAddress(new Guid(e.CommandArgument.ToString()));
            //this.Close_Click(sender, e);
        }

        //private void SetShippingAddressFromFormData()
        //{
        //    shippingAddress = new MemberAddress();
        //    shippingAddress.AddressUid = AddressUid;
        //    shippingAddress.Address1 = shippingAddressControl.Address1;
        //    shippingAddress.Address2 = shippingAddressControl.Address2;
        //    shippingAddress.Address3 = shippingAddressControl.Address3;
        //    shippingAddress.AddressType = AddressType.Shipping;
        //    shippingAddress.City = shippingAddressControl.City;
        //    shippingAddress.CompanyName = this.companyName.Text;
        //    shippingAddress.CountryCode = shippingAddressControl.Country;
        //    shippingAddress.IsDefaultForType = this.setAsDefault.Checked;
        //    shippingAddress.Name = this.addressName.Text;
        //    shippingAddress.PhoneNumber = this.phone.Text;
        //    shippingAddress.PostalCode = shippingAddressControl.PostalCode;
        //    shippingAddress.RegionCode = shippingAddressControl.Region;
        //}

        protected void Save_Click(object sender, CommandEventArgs e)
        {
            Guid newAddressGuid = Guid.Empty;
            Page.Validate(editShippingAddressValidationSummary.ValidationGroup);
            if (editShippingAddressValidationSummary.GetErrorMessages() == null)
            {
                BindData(true);
            //    AccountsEditMode accountEditMode = AccountsEditMode.None;
            //    if (Enum.IsDefined(typeof(AccountsEditMode), e.CommandName))
            //    {
            //        accountEditMode = (AccountsEditMode)Enum.Parse(typeof(AccountsEditMode), e.CommandName);
            //  }

                switch (EditMode)
                {
                    case AccountsEditMode.Create:
                        newAddressGuid = presenter.AddShippingAddress();
                        if (newAddressGuid != Guid.Empty)
                        {
                            //need to clean up the ui
                            shippingAddress = new MemberAddress();
                            BindData(false);
                        }
                        break;
                    case AccountsEditMode.Update:
                        shippingAddress.AddressUid = AddressUid;                           
                        //presenter.UpdateShippingAddress();
                        break;
                    default:
                        throw new Exception(string.Format("EditShippingAddress: Save() - Invalid EditMode '{0}'", EditMode));
                }
                if (newAddressGuid != Guid.Empty)
                {
                    this.Close_Click(sender, e);
                    if (ShippingAddressAdded != null)
                    {
                        CommandEventArgs e1 = new CommandEventArgs("ShippingAddressAdded", newAddressGuid);
                        ShippingAddressAdded(this, e1);
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private void BindData(bool toBusinessEntity)
        {
            if (toBusinessEntity)
            {
                shippingAddress = new MemberAddress();
                shippingAddress.AddressUid = Guid.NewGuid();
                shippingAddress.Address1 = shippingAddressControl.Address1;
                shippingAddress.Address2 = shippingAddressControl.Address2;
                shippingAddress.Address3 = shippingAddressControl.Address3;
                shippingAddress.AddressType = AddressType.Shipping;
                shippingAddress.City = shippingAddressControl.City;
                shippingAddress.CompanyName = this.companyName.Text;
                shippingAddress.CountryCode = shippingAddressControl.Country;
                shippingAddress.IsDefaultForType = this.setAsDefault.Checked;
                shippingAddress.Name = this.addressName.Text;
                shippingAddress.PhoneNumber = this.phone.Text;
                shippingAddress.PostalCode = shippingAddressControl.PostalCode;
                shippingAddress.RegionCode = shippingAddressControl.Region;
            }
            else
            {
                shippingAddressControl.Address1 = shippingAddress.Address1;
                shippingAddressControl.Address2 = shippingAddress.Address2;
                shippingAddressControl.Address3 = shippingAddress.Address3;
                //shippingAddress.AddressType = AddressType.Shipping;
                shippingAddressControl.City = shippingAddress.City;
                this.companyName.Text = shippingAddress.CompanyName;
                shippingAddressControl.Country = shippingAddress.CountryCode;
                this.setAsDefault.Checked = shippingAddress.IsDefaultForType;
                this.addressName.Text = shippingAddress.Name;
                this.phone.Text = shippingAddress.PhoneNumber;
                shippingAddressControl.PostalCode = shippingAddress.PostalCode;
                shippingAddressControl.Region = shippingAddress.RegionCode;
            }
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Reset();
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "closeAddAddressModalPopup", "MochaUI.HideModal('editShippingAddressModalPopup');", true);
            //Response.Redirect(string.Format("{0}?{1}={2}", SiteUrls.MyProfile, "Pane", "shippingPane"));
        }

        private void Reset()
        {
            //if (UseType == CreditCardUsageType.SelectFromSavedCards)
            //{
            //    creditCardList.SelectedIndex = 0;
            //    cardNumberDisplayText.Text = string.Empty;
            //    cardTypeDisplayText.Text = string.Empty;

            //}
            //else
            //{
            //    cardNumber.Text = string.Empty;
            //    this.cardTypeList.SelectedIndex = 0;
            //}
            //cardMonth.Text = DateTime.Now.Month.ToString("00");
            //cardYear.Text = DateTime.Now.Year.ToString();
            //cardHolder.Text = string.Empty;

            sameAsBusinessAddr.Checked = false;
            setAsDefault.Checked = false;
            addressName.Text = string.Empty;
            companyName.Text = string.Empty;
            shippingAddressControl.Address1 = string.Empty;
            shippingAddressControl.Address2 = string.Empty;
            shippingAddressControl.Address3 = string.Empty;
            shippingAddressControl.City = string.Empty;
            shippingAddressControl.Country = string.Empty;
            shippingAddressControl.Region = string.Empty;
            shippingAddressControl.PostalCode = string.Empty;
            phone.Text = string.Empty;

        }
        #endregion

        #region IEditShippingInformationView Members

        /// <summary>
        /// Gets or sets the address uid of the shipping address to edit/delete.
        /// </summary>
        /// <value>The address uid.</value>
        public Guid AddressUid
        {
            get
            {
                Guid returnGuid = Guid.Empty;
                try
                {
                    returnGuid = new Guid(this.addressUid.Value);
                }
                catch { }

                return returnGuid;
            }
            set { this.addressUid.Value = value.ToString(); }
        }

        ///// <summary>
        ///// Gets the edit mode.
        ///// </summary>
        ///// <value>The edit mode.</value>
        

        ///// <summary>
        ///// Gets or sets the shipping address to edit and loads the address control accordingly.
        ///// </summary>
        ///// <value>The shipping address.</value>
        public MemberAddress ShippingAddress
        {
            get
            {
                return shippingAddress;
            }
        //    set
        //    {
        //        shippingAddress = value;
        //        shippingAddressControl.LoadAddressFromMemberAddress(shippingAddress);
        //        if (shippingAddress != null)
        //        {
        //            this.setAsDefault.Checked = shippingAddress.IsDefaultForType;
        //            this.addressName.Text = shippingAddress.Name;
        //            this.companyName.Text = shippingAddress.CompanyName;
        //            this.phone.Text = shippingAddress.PhoneNumber;
        //        }
        //        else
        //        {
        //            this.setAsDefault.Checked = false;
        //            this.addressName.Text = String.Empty;
        //            this.companyName.Text = String.Empty;
        //            this.phone.Text = String.Empty;
        //        }
                  
        //    }

            set
            {
                shippingAddress = value;
            }
           
        }
        public AccountsEditMode EditMode
        {
            get { return editMode; }
            set { editMode = value; }
        }  
        /// <summary>
        /// Whether or not to display the delete section on the view
        /// </summary>
        /// <param name="value">
        /// true to display the delete section, false otherwise
        /// </param>
        /// <param name="isDefault">
        /// If true, this is the default shipping address
        /// </param>
        public void DisplayDeleteSection(bool value, bool isDefault, Guid addressUid)
        {
            deleteShippingAddressPanel.Visible = value;
            if (value)
            {
                confirmDeleteDefaultMessage.Visible = isDefault;
                confirmDeleteMessage.Visible = !isDefault;
                deleteYes.CommandArgument = addressUid.ToString();
            }
        }

        /// <summary>
        /// Whether or not to display the create/edit section on the view
        /// </summary>
        /// <param name="value">true to display the create/edit section, false otherwise</param>
        public void DisplayEditSection(bool value, string commandName)
        {
            editShippingAddressPanel.Visible = value;
            if (value)
            {
                editShippingAddressSave.CommandName = commandName;
            }
        }

        /// <summary>
        /// Gets the selected country.
        /// </summary>
        /// <value>The country code.</value>
        public string CountryCode
        {
            get { return shippingAddressControl.Country; }
        }

        ///// <summary>
        ///// Gets or sets the List of Regions to display.
        ///// </summary>
        ///// <value>The regions.</value>
        public List<ContentItem> Regions
        {
            get { return (List<ContentItem>)shippingAddressControl.RegionData; }
            set { shippingAddressControl.RegionData = value; }
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

        //#region IShippingAddress Members

        //public AccountsEditMode EditMode
        //{
        //    get { return editMode; }
        //    set { editMode = value; }
        //}  

        

      

        //MemberAddress IShippingAddress.ShippingAddress
        //{
        //    get
        //    {
        //        return shippingAddress;
        //    }
        //    set
        //    {
        //        shippingAddress = value;
        //    }
        //}

        //#endregion

        protected void sameAsBusinessAddr_CheckedChanged(object sender, EventArgs e)
        {
            //if (this.sameAsBusinessAddr.Checked)
            {
                //grab the business address
                //temporarily use mock data
                //shippingAddress = new MemberAddress();
                //shippingAddress.Address1 = "addr1 1111";
                //shippingAddress.Address2 = "addr2 addr2";
                //shippingAddress.Address3 = "addr3 addr3";
                //shippingAddress.City = "bosttttton";
                //shippingAddress.CompanyName = "my company";
                //shippingAddress.PostalCode = "12345";
                shippingAddress = presenter.GetBillingAddress();
                BindData(false);
            }
        }

        //protected void miketest_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (this.sameAsBusinessAddr.Checked)
        //    {
        //        //grab the business address
        //        //temporarily use mock data
        //        shippingAddress.Address1 = "addr1 1111";
        //        shippingAddress.Address2 = "addr2 addr2";
        //        shippingAddress.Address3 = "addr3 addr3";
        //        shippingAddress.City = "bosttttton";
        //        shippingAddress.CompanyName = "my company";
        //        shippingAddress.PostalCode = "12345";
        //        BindData(false);
        //    }
        //}

        //protected void anothertest_Click(object sender, EventArgs e)
        //{
        //        //grab the business address
        //        //temporarily use mock data
        //    shippingAddress = new MemberAddress();
        //        shippingAddress.Address1 = "addr1 1111";
        //        shippingAddress.Address2 = "addr2 addr2";
        //        shippingAddress.Address3 = "addr3 addr3";
        //        shippingAddress.City = "bosttttton";
        //        shippingAddress.CompanyName = "my company";
        //        shippingAddress.PostalCode = "12345";
        //        BindData(false);
        //}
    }
}
