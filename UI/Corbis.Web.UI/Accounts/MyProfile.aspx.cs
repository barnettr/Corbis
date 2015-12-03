using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.Content;
using Corbis.Web.Entities;
using Corbis.Web.UI.Accounts.ViewInterfaces;
using Corbis.Web.UI.Controls;
using Corbis.Web.UI.Presenters.Accounts;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Utilities;
using AjaxControlToolkit;

namespace Corbis.Web.UI.Accounts
{
    public partial class MyProfile : CorbisBasePage, IMyProfileView
    {
        private enum QueryString
        {
            Pane
        }

        AccountsPresenter presenter;

        

		protected override void OnInit(EventArgs e)
        {
            this.RequiresSSL = true;
            base.OnInit(e);
            presenter = new AccountsPresenter(this);
            Response.CacheControl = "No-cache";
        }

        private void HookupEventHandlers()
        {
             savePreference.Click += new EventHandler(SavePreference_Click);
             //ancelPreference.Click += new EventHandler(CancelPreference_Click);
         }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HookupEventHandlers();

                LoadLanguageList();
                LoadEmailFormatList();
                LoadPreferredPaymentMethodList();
                SetECommerceVisibiliy();
                SetExpressCheckOut();
                presenter.GetPersonalInfo();
                presenter.PopulateBusinessInformation();
                //presenter.PopulateShippingInformation();
                presenter.GetPaymentInformation();
                presenter.GetPreferences();
            }
            contactCorbis.NavigateUrl = SiteUrls.CustomerService;
        }

        private void SetExpressCheckOut()
        {
            if (Profile.IsECommerceEnabled)
            {
                expressCheckout.SelectedIndex = 0;
                ShowExpressCheckout = true;
            }
            else
            {
                ShowExpressCheckout = false;
            }
        }

        /// <summary>
        /// Sets the visibility on the shipping information pane.  The pane should only be visible when ECommerce is enabled.
        /// </summary>
        
        //TODO: This method with logic needs to be moved to presenter.            
        private void SetECommerceVisibiliy()
        {
            if (!Profile.IsECommerceEnabled)
            {
                paymentPane.Visible = false;
            }
        }

        private void LoadEmailFormatList()
        {
            this.emailFormat.DataSource = GetEnumDisplayValues<EmailFormat>(false);
            this.emailFormat.DataValueField = "Id";
            this.emailFormat.DataTextField = "Text";
            this.emailFormat.DataBind();
        }

        private void LoadLanguageList()
        {
            emailLanguage.DataTextField = "ContentValue";
            emailLanguage.DataValueField = "Key";
            emailLanguage.DataSource = ((LanguageContentProvider)ContentProviderFactory.CreateProvider(ContentItems.Language)).GetLanguages();
            emailLanguage.DataBind();
        }

        private void LoadPreferredPaymentMethodList()
        {
            this.preferredPaymentMethod.DataSource = GetEnumDisplayValues<PaymentMethod>(false, true);
            this.preferredPaymentMethod.DataValueField = "Id";
            this.preferredPaymentMethod.DataTextField = "Text";
            this.preferredPaymentMethod.DataBind();

        }

		#region Display Personal Information

		public string Username
		{
			set { username.Text = Server.HtmlEncode(value); }
		}

        public string DisplayName
		{
			set { displayName.Text = Server.HtmlEncode(value); }
		}

        public bool ShowFuriganaName
        {
            set { this.furiganaNameRow.Visible = value; }
        }

        public string FuriganaDisplayName
        {
            set { furiganaDisplayName.Text = Server.HtmlEncode(value); }
        }

		public string EmailAddress
		{
			set { emailAddress.Text = Server.HtmlEncode(value); }
		}

		public MemberAddress MailingAddress
		{
			set
			{
                mailingAddress.LoadAddressFromMemberAddress(value);
			}
		}

        public PasswordRecoveryQuestionType PasswordRecoveryQuestion
        {
            set { this.securityQuestion.Text = CorbisBasePage.GetEnumDisplayText<PasswordRecoveryQuestionType>(value); }
        }

        public string PasswordRecoveryAnswer
        {
            set { this.securityAnswer.Text = Server.HtmlEncode(value); }
        }
    
		#endregion

		#region DisplayBusinessInformation Implementation

		public string CompanyName
        {
            set { companyName.Text = Server.HtmlEncode(value); }
        }

        public JobTitle JobTitle
        {
            set { jobTitle.Text = GetEnumDisplayText<JobTitle>(value); }
        }

        public MemberAddress BusinessAddress
        {
            set { businessAddress.LoadAddressFromMemberAddress(value); }
        }

        public string Telephone
        {
            set { telephone.Text = Server.HtmlEncode(value); }
		}

		#endregion

		#region Payment and Billing

		public bool DisplayAvailableCreditDiv
        {

            get
            {
                return availableCreditDiv.Visible;
            }

            set
            {
                availableCreditDiv.Visible = value;
            }
        }

        public bool DisplayEmptyCreditDiv
        {

            get
            {
                return emptyCreditDiv.Visible;
            }

            set
            {
                emptyCreditDiv.Visible = value;
            }
        }

        public bool DisplayPaymentPane
        {
            get { return paymentPane.Visible; }
            set { paymentPane.Visible = value; }
        }

        public List<CreditCard> CreditsList
        {
			get { return (List<CreditCard>)creditList.DataSource; }
            set
            {               
                creditList.DataSource = value;
                creditList.DataBind();

				List<KeyValuePair<Guid, String>> preferredCardDataSource = value.ConvertAll<KeyValuePair<Guid, String>>
				(
					new Converter<CreditCard, KeyValuePair<Guid, string>>
					(
						delegate(CreditCard creditCard)
						{
                            return new KeyValuePair<Guid, string>(creditCard.CreditCardUid, String.Format("{0} *{1}", GetResourceString("Resource", creditCard.CreditCardTypeCode + "_card"), creditCard.CardNumberViewable.TrimStart('*')));
						}
					)
				);

                preferredCreditCard.DataValueField = "Key";
                preferredCreditCard.DataTextField = "Value";
				preferredCreditCard.DataSource = preferredCardDataSource;
                preferredCreditCard.DataBind();
            }
        }

        public List<Company> CorporateAccounts
        {
            set
            {
                preferredCorporateAccount.DataValueField = "CompanyId";
                preferredCorporateAccount.DataTextField = "Name";
                preferredCorporateAccount.DataSource = value;
                preferredCorporateAccount.DataBind();
             }
        }


        #endregion

        #region Shipping Information

        

        #endregion

        #region IPreferencesView Members

        public EmailFormat EmailFormat
        {
            get
            {
                EmailFormat returnValue = EmailFormat.Unknown;
                int formatId;
                if (int.TryParse(this.emailFormat.SelectedValue, out formatId) &&
                    Enum.IsDefined(typeof(EmailFormat), formatId))
                {
                    returnValue = (EmailFormat)formatId;
                }
                return returnValue;
            }
            set
            {
                if (value != EmailFormat.Unknown)
                {
                    this.emailFormat.SelectedValue = value.GetHashCode().ToString();
                }
            }
        }

        public string EmailCultureName
        {
            get
            {
                return this.emailLanguage.SelectedValue;
            }
            set
            {
                this.emailLanguage.SelectedValue = value;
            }
        }

        protected void updateBusinessInfoPane_Click(object sender, EventArgs e)
        {
            presenter.PopulateBusinessInformation();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resizeBiz", "CorbisUI.MyProfile.ResizeBusinessInfo()", true);
        }

        protected void updatePaymentInfoPane_Click(object sender, EventArgs e)
        {
            presenter.GetPaymentInformation();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resizePayment", "CorbisUI.MyProfile.ResizePaymentInfo()", true);
        }

        protected void updatePersonalInfoPane_Click(object sender, EventArgs e)
        {
            presenter.GetPersonalInfo();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resizePII", "CorbisUI.MyProfile.ResizePersonalInfo()", true);

        }

        protected void updatePreferencesPane_Click(object sender, EventArgs e)
        {
            LoadLanguageList();
            LoadEmailFormatList();
            LoadPreferredPaymentMethodList();
            presenter.GetPaymentInformation();
            presenter.GetPreferences();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resizePrefs", "CorbisUI.MyProfile.ResizePreferences()", true);
        }

        protected void SavePreference_Click(object sender,EventArgs e)
        {
            presenter.SavePreferences();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "getSuccess", "parent.CorbisUI.MyProfile.OpenSuccessPopup();CorbisUI.MyProfile.refreshPreferencesPane();", true);
        }

        protected void ClosePasswordSuccess_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("{0}?{1}={2}", SiteUrls.MyProfile, QueryString.Pane.ToString(), "personalInfoPane"));
        }

        protected void CancelPreference_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("{0}?{1}={2}", SiteUrls.MyProfile, QueryString.Pane.ToString(), "preferencesPane"));
        }

        public bool SendPromoEmail
        {
            get
            {
                return this.sendPromoEmail.Checked;
            }
            set
            {
                this.sendPromoEmail.Checked = value;
                this.doNotSendPromoEmail.Checked = !value;
            }
        }

        public bool SnailmailPreference
        {
            get
            {
                return this.snailmailPreference.Checked;
            }
            set
            {
                this.snailmailPreference.Checked = value;
            }
        }

       public  bool ShowExpressCheckout
       {
           get
           {
               return expressCheckoutDiv.Visible;
           }
           set
           {
               expressCheckoutDiv.Visible = value;
           }
       }

        public PaymentMethod PreferredPaymentMethod
        {
            get
            {
                PaymentMethod returnValue = PaymentMethod.NotSet;
                int methodId;
                if (int.TryParse(this.preferredPaymentMethod.SelectedValue, out methodId) &&
                    Enum.IsDefined(typeof(PaymentMethod), methodId))
                {
                    returnValue = (PaymentMethod)methodId;
                }
                return returnValue;
            }
            set
            {
                if (value != PaymentMethod.NotSet)
                {
                    this.preferredPaymentMethod.SelectedValue = value.GetHashCode().ToString();
                }
            }
        }

        public Nullable<int> PreferredCorporateAccountId
        {
            get
            {
                int preferredId;
                if (int.TryParse(this.preferredCorporateAccount.SelectedValue, out preferredId))
                {
                    return preferredId;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value.HasValue && value != 0)
                {
                    this.preferredCorporateAccount.SelectedValue = value.Value.ToString();
                }
            }
        }

        public Nullable<Guid> PreferredCreditCardUid
        {
            get
            {
                Guid preferredUid;
                if (GuidHelper.TryParse(this.preferredCreditCard.SelectedValue, out preferredUid))
                {
                    return preferredUid;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value.HasValue && value != Guid.Empty)
                {
                    this.preferredCreditCard.SelectedValue = value.Value.ToString();
                }
            }
        }

        public string PreferredShippingAddress
        {
            get
            {
                return this.preferredShippingAddress.SelectedValue;
            }
            set
            {
                this.preferredShippingAddress.SelectedValue = value;
            }
        }

        public bool IsExpressCheckoutEnabled
        {
            get
            {
                string val = this.expressCheckout.SelectedValue;
                return bool.Parse(val);
            }
            set
            {
                this.expressCheckout.SelectedValue = value.ToString().ToLower();
            }
        }

        public bool ShowPaymentSection
        {
            get
            {
                return this.paymentAndShippingPreferences.Visible;
            }
            set
            {
                this.paymentAndShippingPreferences.Visible = value;
            }
        }

        public bool ShowPreferredPaymentMethod
        {
            get
            {
                return this.preferredPaymentMethodDiv.Visible;
            }
            set
            {
                this.preferredPaymentMethodDiv.Visible = value;
            }
        }

        public bool ShowPreferredCorporateAccount
        {
            get
            {
                return this.preferredCorporateAccountDiv.Visible;
            }
            set
            {
                this.preferredCorporateAccountDiv.Visible = value;
            }
        }

        public bool ShowPreferredCreditCard
        {
            get
            {
                return this.preferredCreditCardDiv.Visible;
            }
            set
            {
                this.preferredCreditCardDiv.Visible = value;
            }
        }

        public bool ShowPreferredShippingAddress
        {
            get
            {
                return this.preferredShippingAddressDiv.Visible;
            }
            set
            {
                this.preferredShippingAddressDiv.Visible = value;
            }
        }

        #endregion

        #region Payment Methods

        private void SetPane(string paneToSet)
        {
            // This is all client side code now...
            // TODO REMOVE
        }

        public void AddNewCreditCard_OnClick(object sender, CommandEventArgs e)
        {
            //this.paymentModal.SetEditMode(e.CommandArgument.ToString());
            //this.paymentModal.CreditCardID = String.Empty;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function() {parent.CorbisUI.MyProfile.OpenEditPaymentInformation()});", true);
            SetPane("paymentPane");
        }

        public void DeleteCreditCardInformation_Click(object sender, CommandEventArgs e)
        {
            //this.paymentModal.CreditCardInformation.EditMode = false;
            //this.paymentModal.SetDeleteMode(e.CommandArgument.ToString());
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function() {parent.CorbisUI.MyProfile.OpenEditPaymentInformation()});", true);
            SetPane("paymentPane");
        }

        public void EditCreditCardInformation_Click(object sender, CommandEventArgs e)
        {
            //this.paymentModal.CreditCardInformation.EditMode = true;
            //this.paymentModal.SetEditMode(e.CommandArgument.ToString());
            //this.paymentModal.LoadCreditCard(e.CommandArgument.ToString());
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function() {parent.CorbisUI.MyProfile.OpenEditPaymentInformation()});", true);
            SetPane("paymentPane");
        }

        #endregion
    }
}
