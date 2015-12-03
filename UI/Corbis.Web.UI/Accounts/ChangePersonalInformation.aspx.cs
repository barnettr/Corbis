using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using Corbis.Web.UI.Controls;
using Corbis.Framework.Globalization;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.Entities;
using Corbis.Web.UI.Accounts.ViewInterfaces;
using Corbis.Web.UI.Presenters.Accounts;
using Corbis.Web.Validation;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Utilities;
using Corbis.Web.Utilities.StateManagement;
using Languages = Corbis.Framework.Globalization.Language;

namespace Corbis.Web.UI.Accounts
{
    public partial class ChangePersonalInformation : CorbisBasePage, IChangePersonalInformation, IViewPropertyValidator, IValidationHubErrorSetter
    {
        private AccountsPresenter accPresenter;

        protected override void OnInit(EventArgs e)
        {
            this.RequiresSSL = true;
            accPresenter = new AccountsPresenter(this);
            HookupEventHandlers();
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LocalizePage();
            if (!IsPostBack)
            {
                PopulateSecurityQuestionDropDown();
                accPresenter.PopulateChangePersonalInformation();
            }
        }

        /// <summary>
        /// Hooks up event handlers 
        /// </summary>
        public void HookupEventHandlers()
        {
            mailingAddress.CountryDataChange += new EventHandler(MailingAddress_CountryDataChange);
        }


        private void LocalizePage()
        {
            if (Language.CurrentLanguage == Language.Japanese)
            {
                this.furiganaLastNameRow.Visible = true;
                this.furiganaFirstNameRow.Visible = true;
                this.name1Label.Text = Resources.Accounts.JapaneseLastNameLabel;
                this.name1.Attributes["required_message"] = Resources.Accounts.MemberValidationError_LastNameRequired;
                this.name2Label.Text = Resources.Accounts.JapaneseFirstNameLabel;
                this.name2.Attributes["required_message"] = Resources.Accounts.MemberValidationError_FirstNameRequired;
            }
            else if (Language.CurrentLanguage == Language.ChineseSimplified)
            {
                this.furiganaLastNameRow.Visible = false;
                this.furiganaFirstNameRow.Visible = false;
                this.name1Label.Text = Resources.Accounts.LastNameLabel;
                this.name1.Attributes["required_message"] = Resources.Accounts.MemberValidationError_LastNameRequired;
                this.name2Label.Text = Resources.Accounts.FirstNameLabel;
                this.name2.Attributes["required_message"] = Resources.Accounts.MemberValidationError_FirstNameRequired;
            }
            else
            {
                this.furiganaLastNameRow.Visible = false;
                this.furiganaFirstNameRow.Visible = false;
                this.name1Label.Text = Resources.Accounts.FirstNameLabel;
                this.name1.Attributes["required_message"] = Resources.Accounts.MemberValidationError_FirstNameRequired;
                this.name2Label.Text = Resources.Accounts.LastNameLabel;
                this.name2.Attributes["required_message"] = Resources.Accounts.MemberValidationError_LastNameRequired;
            }
        }

        private void PopulateSecurityQuestionDropDown()
        {
            this.securityQuestion.PromptText = Resources.Resource.SelectOne;
            this.securityQuestion.DataSource = GetEnumDisplayValues<PasswordRecoveryQuestionType>(false);
            this.securityQuestion.DataValueField = "Id";
            this.securityQuestion.DataTextField = "Text";
            this.securityQuestion.DataBind();
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            
            
            if (accPresenter.SavePersonalInformation())
            {
                if (Context.Session != null)
                {
                    Context.Session.Clear();
                }

                // unauthenticate the current username
                FormsAuthentication.SignOut();

                // authenticate
                StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                stateItems.SetStateItem<string>(new StateItem<string>(
                    ProfileKeys.Name, 
                    ProfileKeys.UsernameStateItemKey,
                    UserName, 
                    StateItemStore.Cookie, 
                    StatePersistenceDuration.NeverExpire));
                FormsAuthentication.SetAuthCookie(UserName, false);

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "saveClicked", "parent.CorbisUI.MyProfile.refreshPersonalInfoPane();parent.MochaUI.CloseModal('personalInfoModalPopup');parent.CorbisUI.MyProfile.OpenSuccessPopup();", true);
            }
            
        }


        protected void MailingAddress_CountryDataChange(object sender, EventArgs e)
        {
            mailingAddress.RegionData = accPresenter.GetRegions(mailingAddress.Country);
        }

        #region IChangePersonalInformation

        [PropertyControlMapper("username")]
        public string UserName
        {
            get { return username.Text; }
            set { username.Text = value; }
        }

        [PropertyControlMapper("name1")]
        public string Name1
        {
            get { return name1.Text; }
            set { name1.Text = value; }
        }

        [PropertyControlMapper("name2")]
        public string Name2
        {
            get { return name2.Text; }
            set { name2.Text = value; }
        }

        [PropertyControlMapper("furiganaFirstName")]
        public string FuriganaFirstName
        {
            get { return furiganaFirstName.Text; }
            set { furiganaFirstName.Text = value; }
        }

        [PropertyControlMapper("furiganaLastName")]
        public string FuriganaLastName
        {
            get { return furiganaLastName.Text; }
            set { furiganaLastName.Text = value; }
        }

        [PropertyControlMapper("email")]
        public string Email
        {
            get { return email.Text; }
            set { email.Text = value; }
        }

        [PropertyControlMapper("confirmEmail")]
        public string ConfirmEmail
        {
            get { return confirmEmail.Text; }
            set { confirmEmail.Text = value; }
        }

        [PropertyControlMapper("securityQuestion")]
        public PasswordRecoveryQuestionType PasswordRecoveryQuestion
        {
            get
            {
                PasswordRecoveryQuestionType questionType = PasswordRecoveryQuestionType.None;
                int questionId;
                if (int.TryParse(securityQuestion.SelectedValue, out questionId)
                    && Enum.IsDefined(typeof(PasswordRecoveryQuestionType), questionId))
                {
                    questionType = (PasswordRecoveryQuestionType)questionId;
                }
                return questionType;
            }
            set
            {
                securityQuestion.SelectedValue = value.GetHashCode().ToString();
            }
        }

        [PropertyControlMapper("securityAnswer")]
        public string PasswordRecoveryAnswer
        {
            get { return securityAnswer.Text; }
            set { securityAnswer.Text = value; }
        }

        public MemberAddress Address
        {
            set
            {
                MemberAddress address = value;
                mailingAddress.CountryData = accPresenter.GetCountries();
                mailingAddress.LoadAddressFromMemberAddress(address);
                if (address != null)
                {
                    CountryCode = address.CountryCode;
                    mailingAddress.RegionData = accPresenter.GetRegions(address.CountryCode);
                    RegionCode = address.RegionCode;
                }
            }
        }

        /// <summary>
        /// Used to expose the Mailing Address control for validation
        /// </summary>
        /// <value>The mailing address control.</value>
        public Corbis.Web.UI.Controls.Address MailingAddress 
        {
            get { return mailingAddress; }
        }

        [ChildControlPropertyMapper("MailingAddress", "Address1")]
        public string Address1
        {
            get { return mailingAddress.Address1; }
        }

        [ChildControlPropertyMapper("MailingAddress", "Address2")]
        public string Address2
        {
            get { return mailingAddress.Address2; }
        }

        [ChildControlPropertyMapper("MailingAddress", "Address3")]
        public string Address3
        {
            get { return mailingAddress.Address3; }
        }

        [ChildControlPropertyMapper("MailingAddress", "City")]
        public string City
        {
            get { return mailingAddress.City; }
        }

        [ChildControlPropertyMapper("MailingAddress", "Region")]
        public string RegionCode
        {
            get { return mailingAddress.Region; }
            set { mailingAddress.Region = value; }
        }

        [ChildControlPropertyMapper("MailingAddress", "Country")]
        public string CountryCode
        {
            get { return mailingAddress.Country; }
            set { mailingAddress.Country = value; }
        }

        [ChildControlPropertyMapper("MailingAddress", "PostalCode")]
        public string PostalCode
        {
            get { return mailingAddress.PostalCode; }
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
