using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.Framework.Globalization;
using Corbis.Web.Utilities;
using Corbis.Web.UI.Presenters.Checkout;
using Corbis.Web.UI.Cart.ViewInterfaces;
using Corbis.CommonSchema.Contracts.V1;

using Corbis.WebOrders.Contracts.V1;

namespace Corbis.Web.UI.Checkout
{
    public partial class StepProject : Corbis.Web.UI.CorbisBaseUserControl, IProject, IValidateEncoding
    {

        #region Control Events

        protected override void OnInit(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                base.OnInit(e);
                string projectName = "";
                if (Language.CurrentLanguage != Language.Japanese && Language.CurrentLanguage != Language.ChineseSimplified)
                {
                    projectName = (string)GetLocalResourceObject("project") + " ";
                }
                projectName += DateHelper.GetLocalizedDate(DateTime.Now);
                projectField.Text = projectName;
                projectField.Attributes.Add("onblur", "if(this.value=='') this.value = '" + projectName + "';");
            }
        }

        protected void GBNext_OnClick(object sender, EventArgs e)
        {
            //Licensee = this.licenseeField.Text.Trim();
            //ProjectName = this.projectField.Text.Trim();
            //Page.Validate(projectValSummary.ValidationGroup);
            //string[] msgs = projectValSummary.GetErrorMessages();
            //if (msgs == null)
            //{
            //    CheckoutPresenter presenter = ((MainCheckout)Page).CheckoutPresenter;
            //    if (presenter.ValidateProjectEncoding(this))
            //    {
            //        ScriptManager.RegisterStartupScript(Page, this.GetType(), "projectTabNext", "checkoutTabs.show(1,true);", true);
            //    }
            //}
        }
        #endregion

        #region IProject Members

        public string Name
        {
            get
            {
                return this.projectField.Text;
            }
            set
            {
                this.projectField.Text = value;
            }
        }

        public string JobNumber
        {
            get
            {
                return this.jobField.Text;
            }
            set
            {
                this.jobField.Text = value;
            }
        }

        public string PONumber
        {
            get
            {
                return this.poField.Text;
            }
            set
            {
                this.poField.Text = value;
            }
        }

        public string Licensee
        {
            get
            {
                return this.licenseeField.Text;
            }
            set
            {
                this.licenseeField.Text = value;
            }
        }

        #endregion



        #region IValidateEncoding Members

        protected global::System.Web.UI.WebControls.CustomValidator nonAsciiValidator;
        public bool ShowUnicodeErrorMessage
        {
            set
            {
                if (value)
                {
                    nonAsciiValidator.ErrorMessage = GetGlobalResourceObject("Resource", "ContainsNonAsciiCharacters").ToString();
                }
                else
                {
                    nonAsciiValidator.ErrorMessage = String.Empty;

                }
                nonAsciiValidator.IsValid = !value;
            }
        }


        #endregion
    }
}
