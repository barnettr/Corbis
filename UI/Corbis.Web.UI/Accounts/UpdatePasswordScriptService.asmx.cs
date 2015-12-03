using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.UI.Presenters.Accounts;
using Corbis.Web.UI.Accounts.ViewInterfaces;

namespace Corbis.Web.UI.Accounts
{
    /// <summary>
    /// Summary description for UpdatePassword
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class UpdatePasswordScriptService : Corbis.Web.UI.src.CorbisWebService
    {

        [WebMethod()]
        public string UpdatePassword(string oldPassword, string newPassword)
        {
            ChangePasswordView view = new ChangePasswordView(oldPassword, newPassword);
            AccountsPresenter presenter = new AccountsPresenter(view);
            presenter.ChangePassword();
            return CorbisBasePage.GetEnumDisplayText<ChangePasswordResult>(view.Result);
        }

        private class ChangePasswordView : IChangePasswordView
        {
            #region member Variables

            private string _oldPassword;
            private string _newPassword;
            private ChangePasswordResult _result;

            #endregion

            #region constructor

            public ChangePasswordView(string oldPassword, string newPassword)
            {
                _oldPassword = oldPassword;
                _newPassword = newPassword;
            }

            #endregion

            #region IChangePasswordView Members

            public string OldPassword
            {
                get { return _oldPassword; }
            }

            public string NewPassword
            {
                get { return _newPassword; }
            }

            public ChangePasswordResult Result
            {
                get { return _result; }
                set { _result = value; }
            }

            #endregion

            #region IView Members

            public Corbis.Framework.Logging.ILogging LoggingContext
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public IList<Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.ValidationDetail> ValidationErrors
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            #endregion
        }
    }
}
