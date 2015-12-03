using System;
using System.Web;
using System.Web.Security;
using Corbis.Web.UI.Presenters.SignIn;
using Corbis.Web.Entities;
using Corbis.Web.Utilities;
using Corbis.Web.UI.SignIn.ViewInterfaces;
using Corbis.Web.Utilities.StateManagement;

namespace Corbis.Web.UI.Test
{
    public partial class SignIn : CorbisBasePage, ISignInView
    {
        private enum QueryString
        {
            username,
            ReturnUrl
        }

        SignInPresenter presenter;

        protected override void OnInit(EventArgs e)
        {
            if (Profile.IsAuthenticated)
            {
                string returnScript = "<iframe src='" + HttpUrl + "/Test/RefreshSignIn.aspx?cmd=alreadyLoggedIn' style='display:none' />";
                ClientScript.RegisterStartupScript(this.GetType(), "redirectUserSignedIn", returnScript);
            }

            RequiresSSL = true;
            base.OnInit(e);
            presenter = new SignInPresenter(this);
            truste.NavigateUrl = SiteUrls.PrivacyPolicy;
            forgotPassword.NavigateUrl = SiteUrls.SignInInformationRequest;
            tryAgain.Text = GetLocalResourceObject("tryAgain").ToString()
                .Replace("$informationRequest", SiteUrls.SignInInformationRequest)
                .Replace("$register", SiteUrls.Register);
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.SignIn, "SignInCSS");
            HookupEventHandlers();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AnalyticsData["channel"] = "Account Management";
            AnalyticsData["pageName"] = "Sign-In";

            if (!IsPostBack)
            {
                InitializePage();
            }
        }

        private void HookupEventHandlers()
        {
            validate.Click += new EventHandler(Validate_Click);
            register.Click += delegate { Response.Redirect(SiteUrls.Register); };
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

            if (ResolveUrl(GetQueryString(QueryString.ReturnUrl.ToString())).Equals(ResolveUrl(SiteUrls.MyProfile), StringComparison.InvariantCultureIgnoreCase))
            {
                targetPageTitle.Text = GetLocalResourceObject("myAccountPageTitle").ToString();
                targetWarning.Text = GetLocalResourceObject("mustSignInAccount").ToString();
            }
        }

        protected void Validate_Click(object sender, EventArgs e)
        {
            if (IsValid && presenter.ValidateUser())
            {
                if (Context.Session != null)
                {
                    Context.Session.Clear();
                }

                // sign in
                FormsAuthentication.SetAuthCookie(username.Text, false);
				StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
				stateItems.SetStateItem<string>(new StateItem<string>(ProfileKeys.Name, ProfileKeys.UsernameStateItemKey, username.Text, StateItemStore.Cookie, StatePersistenceDuration.NeverExpire));
                string returnUrl = GetQueryString(QueryString.ReturnUrl.ToString());
                returnUrl = (String.IsNullOrEmpty(returnUrl) ? SiteUrls.Home : returnUrl);
                string returnScript = "<iframe src='" + HttpUrl + "/Test/RefreshSignIn.aspx?cmd=" + returnUrl + "' style='display:none' />";
                ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", returnScript);
            }
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
            set { this.loginUnsuccessfulDiv.Visible = value; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        public bool PasswordChangeRequired
        {
            set { }
        }

        public bool UserNameEmpty
        {
            set
            {

            }
        }
        public bool PasswordEmpty
        {
            set
            {

            }
        }

        #endregion
    }
}
