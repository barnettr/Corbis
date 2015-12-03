using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.UI.Properties;
using Corbis.Web.UI.Presenters.SignIn;
using Corbis.Web.UI.SignIn.ViewInterfaces;
using Corbis.Web.UI.src;

namespace Corbis.Web.UI.Registration
{
    /// <summary>
    /// Summary description for ForgotPassword
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class ForgotPassword : CorbisWebService
    {
        #region Public Methods
        [WebMethod(true)]
        public string GetSecurityQuestion(string email, string answer) // Ron: I added answer param here too as an unused to help the client script just pass the same params each time
        {
            ForgotView view = new ForgotView();
            view.EmailAddress = email;
            ForgotPasswordPresenter presenter = new ForgotPasswordPresenter(view);
            presenter.GetPasswordRecoveryQuestion();
            return CorbisBasePage.GetEnumDisplayText<PasswordRecoveryQuestionType>(view.PasswordRecoveryQuestion);
        }

        [WebMethod(true)]
        public string SubmitAnswer(string email, string answer)
        {
            ForgotView view = new ForgotView();
            view.EmailAddress = email;
            view.PasswordRecoveryAnswer = answer;
            ForgotPasswordPresenter presenter = new ForgotPasswordPresenter(view);
            presenter.RequestPassword();
            return view.Result.ToString();
        }
        #endregion 


        private class ForgotView : IForgotPasswordView
        {
            #region IForgotPasswordView Members

            public string EmailAddress { get; set;  }

            public PasswordRecoveryQuestionType PasswordRecoveryQuestion { get;  set; }

            public string PasswordRecoveryAnswer { get; set; }

            public SendPasswordReminderEmailResult Result { get;  set; }

            public string HomePageUrl
            {
                get { return Settings.Default.HttpUrl + SiteUrls.Home; }
            }

            public string CustomerServiceUrl
            {
                get { return Settings.Default.HttpUrl + SiteUrls.CustomerService; }
            }
            #endregion

            #region IView
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
