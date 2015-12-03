using System;
using System.Web;
using System.Web.Security;
using Corbis.Web.Authentication;
using Corbis.Web.UI.Presenters.Accounts;
using Corbis.Web.UI.Accounts.ViewInterfaces;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.Accounts
{
    public partial class ChangePasswordModal : CorbisBasePage, IChangePasswordView
    {
        private enum QueryString
        {
            username
        }

        AccountsPresenter presenter;

        protected override void OnInit(EventArgs e)
        {
            if (Profile.IsAnonymous && string.IsNullOrEmpty(Username))
            {
                Response.Redirect(SiteUrls.MyAccount);
            }
            base.OnInit(e);
            presenter = new AccountsPresenter(this);
            HookupEventHandlers();
        }

        private void HookupEventHandlers()
        {
            oldPasswordInvalid.ServerValidate += new System.Web.UI.WebControls.ServerValidateEventHandler(OldPasswordInvalid_ServerValidate);
            save.Click += new EventHandler(Update_Click);
        }

        protected void OldPasswordInvalid_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            if (args == null)
                return;

            args.IsValid = presenter.ValidatePassword();
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            if (IsValid &&
                presenter.ChangePassword())
            {
                whileChanging.Visible = false;
                changesuccess.Visible = true;
            }
        }

        #region IChangePassword Implementation

        public string NewPassword
        {
            get { return newPassword.Text; }
        }

        public string OldPassword
        {
            get { return oldPassword.Text; }
        }

        public string Username
        {
            get { return GetQueryString(QueryString.username.ToString()); }
        }

        #endregion
    }
}
