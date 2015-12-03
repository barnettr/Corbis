using System;
using System.Web.Security;
using Corbis.Web.Authentication;
using Corbis.Web.UI.Presenters.Accounts;
using Corbis.Web.UI.Accounts.ViewInterfaces;
using Corbis.Web.Utilities;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Membership.Contracts.V1;
using System.Web.UI;
using System.Web;

namespace Corbis.Web.UI.Registration
{
    public partial class ChangePassword : CorbisBasePage, IChangePasswordView
    {
        private enum QueryString
        {
            username,
            ReturnUrl,
            Reload,
            Execute,
            protocol
        }
        public string ErrorMessage;

        private string parentProtocol;

        AccountsPresenter presenter;

        protected override void OnInit(EventArgs e)
        {
            this.RequiresSSL = true;
            base.OnInit(e);
            presenter = new AccountsPresenter(this);
            CheckParentProtocol();

            //HookupEventHandlers();
        }

        

        protected void OldPasswordInvalid_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            if (args == null)
                return;

            //args.IsValid = presenter.ValidatePassword();
        }

        protected void changePasswordSave_Click(object sender, EventArgs e)
        {
            presenter.ChangePassword();
        }

        private void CheckParentProtocol()
        {
            // if opening URL is HTTPS, open iframe using https protocol
            string js = "var ParentProtocol = '";
            if (GetQueryString(QueryString.protocol.ToString()) == "https")
            {
                js += HttpsUrl + "';";
                parentProtocol = HttpsUrl;
            }
            else
            {
                js += HttpUrl + "';";
                parentProtocol = HttpUrl;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "parentProtocol", js, true);
        }

        private void DoSuccess()
        {
            changePasswordContentDiv.Visible = false;
            // sign in
            StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
            string selectedCulture = stateItems.GetStateItemValue<string>(ProfileKeys.Name, ProfileKeys.CultureStateItemKey, StateItemStore.Cookie);
           
            if (!String.IsNullOrEmpty(selectedCulture)) stateItems.SetStateItem<string>(new StateItem<string>(ProfileKeys.Name, ProfileKeys.CultureStateItemKey, selectedCulture, StateItemStore.Cookie, StatePersistenceDuration.NeverExpire));
            stateItems.SetStateItem<string>(new StateItem<string>(ProfileKeys.Name, ProfileKeys.UsernameStateItemKey, Profile.Current.UserName, StateItemStore.Cookie, StatePersistenceDuration.NeverExpire));
            FormsAuthentication.SetAuthCookie(Profile.Current.UserName, false);
            iFrameHttp.Attributes["src"] = parentProtocol + SiteUrls.IFrameTunnel + "?windowid=secureSignIn&" + GetSuccessAction();
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
            return success;
        }

        #region IChangePasswordView Members

        string IChangePasswordView.OldPassword
        {
            get { return oldPassword.Text; }
        }

        string IChangePasswordView.NewPassword
        {
            get { return newPassword.Text; }
        }

        Corbis.Membership.Contracts.V1.ChangePasswordResult IChangePasswordView.Result
        {
            set
            {
                if (value == ChangePasswordResult.Success)
                {
                    DoSuccess();
                }
                else
                {
                    vhub.SetError(oldPassword, GetEnumDisplayText<ChangePasswordResult>(value), true, false);

                }
            }
        }

        #endregion
    }
}
