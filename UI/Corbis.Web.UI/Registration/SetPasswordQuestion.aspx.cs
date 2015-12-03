using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Corbis.Web.UI.Presenters.Accounts;
using Corbis.Web.UI.Accounts.ViewInterfaces;
using Corbis.Membership.Contracts.V1;


namespace Corbis.Web.UI.Registration
{
    public partial class SetPasswordQuestion : CorbisBasePage
    {
        private enum QueryString
        {
            username,
            ReturnUrl,
            Reload,
            Execute,
            protocol
        }
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
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateSecurityQuestionDropDown();
            }
        }

        private void PopulateSecurityQuestionDropDown()
        {
            this.questions.PromptText = Resources.Resource.SelectOne;
            this.questions.DataSource = GetEnumDisplayValues<PasswordRecoveryQuestionType>(false);
            this.questions.DataValueField = "Id";
            this.questions.DataTextField = "Text";
            this.questions.DataBind();
        }

        protected void save_Click(object sender, EventArgs e)
        {
            //presenter.ChangePassword();
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
    }
}
