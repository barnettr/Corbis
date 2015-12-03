using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Corbis.Web.UI.Presenters.Search;
using Languages = Corbis.Framework.Globalization.Language;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.UI.Presenters.Registration;
using Corbis.Web.UI.Registration.ViewInterfaces;
using Corbis.Web.Content;
using Corbis.Web.Entities;
using Corbis.Web.Utilities;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Web.Validation;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.Registration
{
    public partial class Register : CorbisBasePage, IRegisterView, IViewPropertyValidator
    {
        RegisterPresenter presenter;

        #region Methods

        protected override void OnInit(EventArgs e)
        {
            if (Profile.IsAuthenticated)
            {
                // already logged in
                //Response.Redirect(SiteUrls.Home);
                reDirect();
            }
            
			RequiresSSL = true;
            base.OnInit(e);
            presenter = new RegisterPresenter(this);
            //signIn.NavigateUrl = SiteUrls.Authenticate;
            truste.NavigateUrl = "javascript:CorbisUI.Legal.OpenPolicyIModal();";
            contactCorbis.NavigateUrl = SiteUrls.CustomerService;
           // usernameExistsLabel.ReplaceKey = "$username";
           // emailExistsLabel.ReplaceKey = "$email";
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Registration, "RegisterCSS");
            accept.Text = GetLocalResourceObject("accept").ToString()
                .Replace("$siteUsage", "javascript:CorbisUI.Legal.OpenSAMIModal();")
                .Replace("$privacyPolicy", "javascript:CorbisUI.Legal.OpenPolicyIModal();");
            SetPageCulture();
           
            HookupEventHandlers();

            // check if chinese user
			if (Profile.IsChinaUser)
			{
                advantage1.Text = GetLocalResourceObject("advantageChina1").ToString();
                advantage2.Text = GetLocalResourceObject("advantage2").ToString();
                advantage3.Text = GetLocalResourceObject("advantage3").ToString();
                advantage4.Text = GetLocalResourceObject("advantageChina4").ToString();
                advantage6.Text = GetLocalResourceObject("advantage6").ToString();
                advantage7.Text = GetLocalResourceObject("advantage7").ToString();
            }
            else
            {
                advantage1.Text = GetLocalResourceObject("advantage1").ToString();
                advantage2.Text = GetLocalResourceObject("advantage2").ToString();
                advantage3.Text = GetLocalResourceObject("advantage3").ToString();
                advantage4.Text = GetLocalResourceObject("advantage4").ToString();
                advantage5.Text = GetLocalResourceObject("advantage5").ToString();
                advantage6.Text = GetLocalResourceObject("advantage6").ToString();
                advantage7.Text = GetLocalResourceObject("advantage7").ToString();
            }
          
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                AnalyticsData["events"] = AnalyticsEvents.RegistrationStart;

                PopulateSecurityQuestionDropDown();
                LoadLanguageList();
                LoadEmailFormatList();
                PopulateJobTitleDropdown();

                string redirectUrl =GetQueryString("redirect");

                StateItemCollection stateItems = new StateItemCollection(System.Web.HttpContext.Current);
                stateItems.SetStateItem<string>(new StateItem<string>(
                        RegistrationSessionKeys.Register,
                       RegistrationSessionKeys.RedirectQuery,
                       Server.UrlDecode(redirectUrl),
                       StateItemStore.AspSession));
               
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

        private void PopulateJobTitleDropdown()
        {
            this.jobTitle.PromptText = Resources.Resource.SelectOne;
            this.jobTitle.DataSource = GetEnumDisplayValues<JobTitle>(false);
            this.jobTitle.DataValueField = "Id";
            this.jobTitle.DataTextField = "Text";
            this.jobTitle.DataBind();
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
            language.DataTextField = "ContentValue";
            language.DataValueField = "Key";
            language.DataSource = ((LanguageContentProvider)ContentProviderFactory.CreateProvider(ContentItems.Language)).GetLanguages();
            language.DataBind();
            language.SelectedValue = Languages.CurrentLanguage.LanguageCode;
        }

        private void HookupEventHandlers()
        {
            // register.Click += new EventHandler(Register_Click);
            cancel.Click += new EventHandler(Cancel_Click);
            //acceptValidator.ServerValidate += new ServerValidateEventHandler(AcceptValidator_ServerValidate);
            //closeSuccessButton.Click += new EventHandler(SuccessRegister_Click);
          
        }

        protected void Register_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                bool createSuccess = presenter.CreateUser();
                if (createSuccess)
                {
                    StateItemCollection stateItems = new StateItemCollection(System.Web.HttpContext.Current);
                    string redirectPage = stateItems.GetStateItemValue<string>(RegistrationSessionKeys.Register, RegistrationSessionKeys.RedirectQuery,
                                    StateItemStore.AspSession);
                    string directedManipulatedSearch = stateItems.GetStateItemValue<string>(SearchSessionKeys.DirectlyManipulatedSearch,
                                                                                       null,
                                                                                       StateItemStore.Cookie);
                    
                    if (Context.Session != null)
                    {
                        stateItems.ClearStateItems(StateItemStore.AspSession);
                    }

                    stateItems.SetStateItem<string>(new StateItem<string>(
                      RegistrationSessionKeys.Register,
                      RegistrationSessionKeys.RedirectQuery,
                      redirectPage,
                      StateItemStore.AspSession));

                    //if (!string.IsNullOrEmpty(directedManipulatedSearch))
                    //{
                    //    stateItems.SetStateItem<string>(new StateItem<string>(SearchSessionKeys.SearchQuery,
                    //                                                          SearchSessionKeys.DirectlyManipulatedSearch,
                    //                                                          directedManipulatedSearch,
                    //                                                          StateItemStore.AspSession));
                    //}
                    // sign in
                   
                    string selectedCulture = stateItems.GetStateItemValue<string>(ProfileKeys.Name, ProfileKeys.CultureStateItemKey, StateItemStore.Cookie);
                    stateItems.ClearStateItems(StateItemStore.Cookie);
                    if (!String.IsNullOrEmpty(selectedCulture)) stateItems.SetStateItem<string>(new StateItem<string>(ProfileKeys.Name, ProfileKeys.CultureStateItemKey, selectedCulture, StateItemStore.Cookie, StatePersistenceDuration.NeverExpire));
                    stateItems.SetStateItem<string>(new StateItem<string>(ProfileKeys.Name, ProfileKeys.UsernameStateItemKey, UserName, StateItemStore.Cookie, StatePersistenceDuration.NeverExpire));
                    FormsAuthentication.SetAuthCookie(UserName, false);

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "registerSuccessAnalytics", "LogOmnitureEvent('event3');", true);
                    if (Profile.IsChinaUser) 
                    {
                        if(!BusinessCountryCode.Equals("CN"))
                        {
                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "registerSuccessDiffCountry", "window.addEvent('domready', function() {OpenModal('registerSuccessDiffCountry')})", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "registerSuccess", "window.addEvent('domready', function() {OpenModal('registerSuccess')})", true);
                        }
                    }
                    else if( !Profile.CountryCode.Equals(BusinessCountryCode))
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "registerSuccessDiffCountry", "window.addEvent('domready', function() {OpenModal('registerSuccessDiffCountry')})", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "registerSuccess", "window.addEvent('domready', function() {OpenModal('registerSuccess')})", true);
                    }
                }
                else
                {
                    this.password.Attributes.Add("value", password.Text);
                    this.confirmPassword.Attributes.Add("value", confirmPassword.Text);
                }
            }
            
        }

        private void reDirect()
        {
            StateItemCollection stateItems = new StateItemCollection(System.Web.HttpContext.Current);
            string redirectPage =
            stateItems.GetStateItemValue<string>(RegistrationSessionKeys.Register, RegistrationSessionKeys.RedirectQuery,
                                    StateItemStore.AspSession);
            if (redirectPage != null && !"".Equals(redirectPage))
            {
                if (redirectPage.Contains("SearchResults.aspx"))
                {
                   // string searchQueryString = stateItems.GetStateItemValue<string>(SearchSessionKeys.SearchQuery,
                                                                                   // SearchSessionKeys.PreviousSearch,
                                                                                   // StateItemStore.AspSession);
                    String searchQueryString = stateItems.GetStateItemValue<string>(SearchSessionKeys.DirectlyManipulatedSearch, null, StateItemStore.Cookie);
                    Response.Redirect(string.Format("{0}?{1}", SiteUrls.SearchResults, searchQueryString));
                }
                else if (redirectPage.Contains("Register.aspx"))
                {
                    Response.Redirect(SiteUrls.Home);
                }
                else
                {
                    Response.Redirect(redirectPage);
                }
            }
            else
            {
                Response.Redirect(SiteUrls.Home);
            }
        }
        protected void Cancel_Click(object sender, EventArgs e)
        {
           // string referrer = Request.UrlReferrer == null ? SiteUrls.Home : ( Request.UrlReferrer.AbsolutePath == Request.Url.AbsolutePath ? SiteUrls.Home : Request.UrlReferrer.AbsoluteUri);
            reDirect();
           
        }

        protected void SuccessRegister_Click(object sender, EventArgs e)
        {
            reDirect();
        }

        //protected void AcceptValidator_ServerValidate(object source, ServerValidateEventArgs args)
        //{
        //    if (args == null)
        //        return;

        //    args.IsValid = accept.Checked;
        //}

		protected void SetPageCulture()
		{


            Control ctl;
            Control ct2;
            Control ct3;

            if (Languages.CurrentLanguage == Languages.Japanese)
            {
                ctl = LoadControl("JapaneseCultureName.ascx");
                ct2 = LoadControl("JapaneseAndChineseCultureAddress.ascx");
                ct3 = LoadControl("JapaneseAndChineseCultureBusinessAddress.ascx");
            }
            else if (Languages.CurrentLanguage == Languages.ChineseSimplified)
            {
                ctl = LoadControl("ChineseCultureName.ascx");
                ct2 = LoadControl("JapaneseAndChineseCultureAddress.ascx");
                ct3 = LoadControl("JapaneseAndChineseCultureBusinessAddress.ascx");
            }
            else
            {
                ctl = LoadControl("GeneralCultureName.ascx");
                ct2 = LoadControl("GeneralCultureAddress.ascx");
                ct3 = LoadControl("GeneralCultureBusinessAddress.ascx");

            }
           
           
            this.namePanel.Controls.Add(ctl);
            this.mailingAddressPanel.Controls.Add(ct2);
            this.businessAddressPanel.Controls.Add(ct3);
            this.CultureSpecificNameView = (ICultureSpecificNameView)ctl;
            this.CultureSpecificMailingAddressView = (ICultureSpecificAddressView)ct2;
            this.CultureSpecificBusinessAddressView = (ICultureSpecificAddressView)ct3;
            this.CultureSpecificMailingAddressView.PagePresenter = presenter;
            this.CultureSpecificBusinessAddressView.PagePresenter = presenter;
            string js = "var mailingAddressCtrl = '" + ct2.ClientID + "';var businessAddressCtrl = '" + ct3.ClientID + "';" ;
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "addressControlVariables", js, true);
		}

        public override void SetValidationHubError<T>(
            Control control,
            T errorEnumValue,
            bool showInSummary,
            bool showHilite)
        {
            string errorMessage = GetEnumDisplayText<T>(errorEnumValue, Resources.Accounts.ResourceManager);
            this.vHub.SetError(control, errorMessage, showInSummary, showHilite);
        }
        
        #endregion

		#region Properties

		private ICultureSpecificNameView cultureSpecificNameView;

        public ICultureSpecificNameView CultureSpecificNameView
		{
            get { return cultureSpecificNameView; }
            set { cultureSpecificNameView = value; }
		}

        private ICultureSpecificAddressView cultureSpecificMailingAddressView;

        public ICultureSpecificAddressView CultureSpecificMailingAddressView
        {
            get { return cultureSpecificMailingAddressView; }
            set { cultureSpecificMailingAddressView = value; }
        }

        private ICultureSpecificAddressView cultureSpecificBusinessAddressView;

        public ICultureSpecificAddressView CultureSpecificBusinessAddressView
        {
            get { return cultureSpecificBusinessAddressView; }
            set { cultureSpecificBusinessAddressView = value; }
        }

		#endregion
		
		#region IRegisterView Implementation

        #region Sign In Information

        [PropertyControlMapper("username")]
        public string UserName
        {
            get { return username.Text; }
        }

        [PropertyControlMapper("password")]
        public string Password
        {
            get { return password.Text; }
        }

        [PropertyControlMapper("confirmPassword")]
        public string ConfirmPassword
        {
            get { return confirmPassword.Text; }
        }

        [PropertyControlMapper("securityQuestion")]
        public string SecurityQuestion
        {
            get { return this.securityQuestion.SelectedValue; }
        }

        // TODO: rename answer control to securityAnswer
        [PropertyControlMapper("answer")]
        public string SecurityAnswer
        {
            get
            {
                return this.answer.Text;
            }
        }

        #endregion

        #region Personal Information

        [ChildControlPropertyMapper("CultureSpecificNameView", "FirstName")]
        public string FirstName
        {
            get { return this.CultureSpecificNameView.FirstName; }
        }

        [ChildControlPropertyMapper("CultureSpecificNameView", "FuriganaFirstName")]
        public string FuriganaFirstName
        {
            get { return this.CultureSpecificNameView.FuriganaFirstName; }
        }

        [ChildControlPropertyMapper("CultureSpecificNameView", "LastName")]
        public string LastName
        {
            get { return this.CultureSpecificNameView.LastName; }
        }

        [ChildControlPropertyMapper("CultureSpecificNameView", "FuriganaLastName")]
        public string FuriganaLastName
        {
            get { return this.CultureSpecificNameView.FuriganaLastName; }
        }

        [PropertyControlMapper("email")]
        public string Email
        {
            get { return email.Text; }
        }

        [PropertyControlMapper("confirmEmail")]
        public string ConfirmEmail
        {
            get { return confirmEmail.Text; }
        }

        #endregion

        #region Mailing Address

        [ChildControlPropertyMapper("CultureSpecificMailingAddressView", "Address1")]
        public string MailingAddress1
        {
            get { return this.CultureSpecificMailingAddressView.Address1; }
        }

        [ChildControlPropertyMapper("CultureSpecificMailingAddressView", "Address2")]
        public string MailingAddress2
        {
            get { return this.CultureSpecificMailingAddressView.Address2; }
        }

        [ChildControlPropertyMapper("CultureSpecificMailingAddressView", "Address3")]
        public string MailingAddress3
        {
            get { return this.CultureSpecificMailingAddressView.Address3; }
        }

        [ChildControlPropertyMapper("CultureSpecificMailingAddressView", "City")]
        public string MailingCity
        {
            get { return this.CultureSpecificMailingAddressView.City; }
        }

        [ChildControlPropertyMapper("CultureSpecificMailingAddressView", "Country")]
        public string MailingCountryCode
        {
            get { return this.CultureSpecificMailingAddressView.Country; }
		}

        [ChildControlPropertyMapper("CultureSpecificMailingAddressView", "State")]
        public string MailingRegionCode
        {
            get { return this.hiddenMailingState.Value; }
        }

        [ChildControlPropertyMapper("CultureSpecificMailingAddressView", "Zip")]
        public string MailingPostalCode
        {
            get
            {
                if (this.CultureSpecificMailingAddressView.Country.Equals("HK"))
                {
                    return ".";
                }
                else
                {
                    return this.CultureSpecificMailingAddressView.Zip;
                }
            }
            set { this.CultureSpecificMailingAddressView.Zip = value; }
        }

        [PropertyControlMapper("language")]
        public string Language
        {
            get { return language.SelectedValue; }
        }

        #endregion

        #region Business Information

        [PropertyControlMapper("companyName")]
        public string CompanyName
        {
            get { return this.companyName.Text; }
		}

        [PropertyControlMapper("jobTitle")]
        public string JobTitle
        {
            get { return jobTitle.SelectedValue; }
        }

        // TODO: Rename phone control to businessPhone
        [PropertyControlMapper("phone")]
        public string BusinessPhoneNumber
        {
            get { return phone.Text; }
        }

        #endregion

        #region Business Address

        [ChildControlPropertyMapper("CultureSpecificBusinessAddressView", "Address1")]
        public string BusinessAddress1
        {
            get { return this.CultureSpecificBusinessAddressView.Address1; }
        }

        [ChildControlPropertyMapper("CultureSpecificBusinessAddressView", "Address2")]
        public string BusinessAddress2
        {
            get { return this.CultureSpecificBusinessAddressView.Address2; }
        }

        [ChildControlPropertyMapper("CultureSpecificBusinessAddressView", "Address3")]
        public string BusinessAddress3
        {
            get { return this.CultureSpecificBusinessAddressView.Address3; }
        }

        [ChildControlPropertyMapper("CultureSpecificBusinessAddressView", "City")]
        public string BusinessCity
        {
            get { return this.CultureSpecificBusinessAddressView.City; }
        }

        [ChildControlPropertyMapper("CultureSpecificBusinessAddressView", "Country")]
        public string BusinessCountryCode
        {
            get { return this.CultureSpecificBusinessAddressView.Country; }
        }

        [ChildControlPropertyMapper("CultureSpecificBusinessAddressView", "State")]
        public string BusinessRegionCode
        {
            get { return this.hiddenBusinessState.Value; }
        }

        [ChildControlPropertyMapper("CultureSpecificBusinessAddressView", "Zip")]
        public string BusinessPostalCode
        {
            get { 
                    if(this.CultureSpecificBusinessAddressView.Country.Equals("HK"))
                    {
                        return ".";
                    }else{
                        return this.CultureSpecificBusinessAddressView.Zip;
                    }
                }
            set { this.CultureSpecificBusinessAddressView.Zip = value; }
        }

        #endregion

        #region Preferences

        [PropertyControlMapper("sendEmail")]
        public bool SendEmail
        {
            get { return sendEmail.Checked; }
        }

        [PropertyControlMapper("emailFormat")]
        public string EmailFormat
        {
            get { return emailFormat.SelectedValue; }
        }


        [PropertyControlMapper("sendSnailMail")]
        public bool SendSnailMail
        {
            get { return sendSnailMail.Checked; }
        }

        [PropertyControlMapper("accept")]
        public bool Accept
        {
            get { return accept.Checked; }
        }

        #endregion

        #region Localized Text

        public string NoRegionsText
        {
            get  { return Resources.Resource.Dashes; }
        }

        public string SelectOneText
        {
            get { return Resources.Resource.SelectOne; }
        }

        public string ForeignZipCodeText
        {
            get { return Resources.Resource.Period; }
        }

        #endregion

        // TODO: Remove these and the corresponding Javascript
     


        #endregion
    }
}
