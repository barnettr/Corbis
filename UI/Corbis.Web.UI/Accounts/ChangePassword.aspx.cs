using System;
using System.Web.Security;
using Corbis.Web.Authentication;
using Corbis.Web.UI.Presenters.Accounts;
using Corbis.Web.UI.Accounts.ViewInterfaces;
using Corbis.Web.Utilities;
using Corbis.Membership.Contracts.V1;
using System.Web.UI;

namespace Corbis.Web.UI.Accounts
{
    public partial class ChangePassword : CorbisBasePage, IChangePasswordView
    {
        private enum QueryString
        {
            username
        }
        public string ErrorMessage;

        AccountsPresenter presenter;

        protected override void OnInit(EventArgs e)
        {
            this.RequiresSSL = true;
            base.OnInit(e);
            presenter = new AccountsPresenter(this);
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
                ErrorMessage = GetEnumDisplayText<ChangePasswordResult>(value);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "changeResult", String.Format("alert({0});", ErrorMessage), true);
            }
        }

        #endregion
    }
}
