using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using Corbis.Web.UI.Presenters.SignIn;
using Corbis.Web.Entities;
using Corbis.Web.Utilities;
using Corbis.Web.UI.SignIn.ViewInterfaces;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Web.UI.Presenters.Search;

namespace Corbis.Web.UI.Registration
{
    public partial class SignIn : CorbisBasePage, ISignInView
    {
        private enum QueryString
        {
            username,
            ReturnUrl,
            Reload,
            Execute,
            StandAlone,
            protocol
        }

        SignInPresenter presenter;

        private string parentProtocol;

        private bool usernameMismatch = false;
        private bool countryCodeMismatch = false;

        protected override void OnPreInit(EventArgs e)
        {
            if (GetQueryString(QueryString.StandAlone.ToString()).Equals("true"))
            {
                MasterPageFile = "~/MasterPages/MasterBase.Master";
            }
            base.OnPreInit(e);
        }

        protected override void OnInit(EventArgs e)
        {
             
            RequiresSSL = true;
            base.OnInit(e);
            presenter = new SignInPresenter(this);
            truste.NavigateUrl = SiteUrls.PrivacyPolicy;
            forgotPassword.NavigateUrl = SiteUrls.SignInInformationRequest;
            //validUserPassword.Text = GetLocalResourceObject("validUserPassword").ToString();
                //.Replace("$informationRequest", SiteUrls.SignInInformationRequest)
                //.Replace("$register", SiteUrls.Register);
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.SignIn, "SignInCSS");
            CheckParentProtocol();
            HookupEventHandlers();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitializePage();
            }
        }

        private void HookupEventHandlers()
        {
            validate.Click += new EventHandler(Validate_Click);
          
            register.OnClientClick = "javascript:SecureModalPopupExitAndRedirectToRegister();return false;";
        }

        private void InitializePage()
        {
            string queryStringUsername = GetQueryString(QueryString.username.ToString());
            if (!string.IsNullOrEmpty(queryStringUsername))
            {
                username.Text = queryStringUsername;
            }
            else if (!Profile.IsAnonymous && !string.IsNullOrEmpty(Profile.UserName))
            {
                username.Text = Profile.UserName;
            }
            // This looks bad -- if this is absolutely needed, we will fix later. -TO
            //if (ResolveUrl(GetQueryString(QueryString.ReturnUrl.ToString())).Equals(ResolveUrl(SiteUrls.MyProfile), StringComparison.InvariantCultureIgnoreCase))
            //{
            //    targetPageTitle.Text = GetLocalResourceObject("myAccountPageTitle").ToString(); 
            //    targetWarning.Text = GetLocalResourceObject("mustSignInAccount").ToString();
            //}
        }

        protected void Validate_Click(object sender, EventArgs e)
        {
            if (IsValid && presenter.ValidateUser())
            {
                //AnalyticsData["events"] = AnalyticsEvents.Login;
                //AnalyticsData["eVar14"] = Profile.UserName;
                //AnalyticsData["state"] = Profile.AddressDetail.RegionCode;
                //AnalyticsData["zip"] = Profile.AddressDetail.PostalCode;
                
                this.SignInContent.Visible = false;
                StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                
                
                string directedManipulatedSearch = string.Empty;
                HttpCookie searchCookie = HttpContext.Current.Request.Cookies["directlyManipulatedSearchQuery"];
                if (searchCookie != null)
                {
                    directedManipulatedSearch = HttpUtility.UrlDecode(searchCookie.Value);
                }

                if (Context.Session != null && Profile.UserName != "Anonymous" && Profile.UserName != username.Text)
                {
                   //Context.Session.Clear();
                    stateItems.ClearStateItems(StateItemStore.AspSession);
                    usernameMismatch = true;
                }

                // sign in
                
                string selectedCulture = stateItems.GetStateItemValue<string>(ProfileKeys.Name, ProfileKeys.CultureStateItemKey, StateItemStore.Cookie);
                
                if (Profile.UserName != username.Text)
                {
                    stateItems.ClearStateItems(StateItemStore.Cookie);
                }
                if (!String.IsNullOrEmpty(selectedCulture)) stateItems.SetStateItem<string>(new StateItem<string>(ProfileKeys.Name, ProfileKeys.CultureStateItemKey, selectedCulture, StateItemStore.Cookie, StatePersistenceDuration.NeverExpire));
                stateItems.SetStateItem<string>(new StateItem<string>(ProfileKeys.Name, ProfileKeys.UsernameStateItemKey, username.Text, StateItemStore.Cookie, StatePersistenceDuration.NeverExpire));
                //if(!string.IsNullOrEmpty(directedManipulatedSearch))
                //{
                //    stateItems.SetStateItem<string>(new StateItem<string>(SearchSessionKeys.SearchQuery,
                //                                                          SearchSessionKeys.DirectlyManipulatedSearch,
                //                                                          directedManipulatedSearch,
                //                                                          StateItemStore.AspSession));
                //}
                
                
                FormsAuthentication.SetAuthCookie(username.Text, false);

                CheckCountryCode();

                iFrameHttp.Attributes["src"] = parentProtocol + SiteUrls.IFrameTunnel + "?windowid=secureSignIn&" + GetSuccessAction();
            }
            else 
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "style", "setErrorStyle();", true);
            }
        }

        private void CheckCountryCode()
        {
            // Current country code from profile
            string oldCountry = Profile.CountryCode;
            // Get new country code for user
            Corbis.Web.Authentication.Profile profile = Corbis.Web.Authentication.Profile.Create(username.Text);
            if (oldCountry != profile.CountryCode)
            {
                this.countryCodeMismatch = true;
            }
        }

        private void CheckParentProtocol()
        {
            // if opening URL is HTTPS, open iframe using https protocol
            string js = "var ParentProtocol = '";
            if (GetQueryString(QueryString.protocol.ToString()) == "https")
            {
                js += HttpsUrl + "';";
                parentProtocol = HttpsUrl;
                forgotPassword.NavigateUrl = forgotPassword.NavigateUrl + "?protocol=https";
            }
            else
            {
                js += HttpUrl + "';";
                parentProtocol = HttpUrl;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "parentProtocol", js, true);
        }

   

        private string GetSuccessAction()
        {
            string success = string.Empty;
            if (!string.IsNullOrEmpty(GetQueryString(QueryString.Reload.ToString())))
            {
                // Reload parent page
                success = "action=" + QueryString.Reload.ToString();
            }
            if (!string.IsNullOrEmpty(GetQueryString(QueryString.ReturnUrl.ToString())))
            {
                // Redirect parent page
                success = "action=" + QueryString.ReturnUrl.ToString() + "&actionArg=" + Server.UrlEncode(GetQueryString(QueryString.ReturnUrl.ToString()));
            }
            if (!string.IsNullOrEmpty(GetQueryString(QueryString.Execute.ToString())))
            {
                // Redirect parent page
                success = "action=" + QueryString.Execute.ToString() + "&actionArg=" + Server.UrlEncode(GetQueryString(QueryString.Execute.ToString()));
            }
            if (usernameMismatch || countryCodeMismatch)
            {
                success = "action=" + QueryString.Execute.ToString() + "&actionArg=CorbisUI.Auth.Go('" + Server.UrlEncode(SiteUrls.Home) + "')";
            }
            return success;
        }
      
        #region ISignInView Implementation

        public string Username
        {
            get { return username.Text; }
        }

        public string Password
        {
            get { return password.Text; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        public bool LoginUnsuccessful
        {
            set 
            { 
               //this.loginUnsuccessfulDiv.Visible = value;
                validationHub.SetError(this.username, GetLocalResourceString("validUserPassword.Text"), true, true);
                validationHub.SetError(this.password, string.Empty, true, true);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        public bool PasswordChangeRequired
        {
            set
            {
                if (value)
                {
                    // Sign out
                    FormsAuthentication.SignOut();
                    StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                    stateItems.SetStateItem<string>(new StateItem<string>(ProfileKeys.Name, ProfileKeys.UsernameStateItemKey, username.Text, StateItemStore.Cookie, StatePersistenceDuration.NeverExpire));
                    // Pass the querystring to the change password page
                    Response.Redirect(SiteUrls.SignInChangePassword + "?" + Request.QueryString.ToString() );

                }
            }
        }

        public bool UserNameEmpty
        {
            set
            {
               // this.UserNameEmptyDiv.Visible = value;
                validationHub.SetError(this.username, GetLocalResourceString("usernameRequired.Text"), true, true);
            }
        }

        public bool PasswordEmpty
        {
            set
            {
               // this.PasswordEmptyDiv.Visible = value;
                validationHub.SetError(this.password, GetLocalResourceString("oldPasswordRequired.Text"), true, true);
            }
        }

        #endregion
    }
}
