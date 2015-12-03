using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.Web.UI.Pricing.Interfaces;

namespace Corbis.Web.UI.Pricing
{
    public class AttributeValidator : CustomValidator, IAttributeValidator
    {

        #region private members

        private string _errorTextResourceKey = null;
        private string _errorSummaryTextResourceKey = null;

        #endregion

        #region IAttributeValidator Members

        public string ErrorTextResourceKey
        {
            get { return _errorTextResourceKey; }
            set
            {
                _errorTextResourceKey = value;
                if (string.IsNullOrEmpty(value))
                {
                    base.Text = null;
                }
            }
        }

        public string ErrorSummaryTextResourceKey
        {
            get { return _errorSummaryTextResourceKey; }
            set
            {
                _errorSummaryTextResourceKey = value;
                if (string.IsNullOrEmpty(value))
                {
                    base.ErrorMessage = null;
                }
            }
        }

        public AttributeValidatorDisplayStyle DisplayStyle
        {
            set
            {
                switch (value)
                {
                    case AttributeValidatorDisplayStyle.Dynamic:
                        base.Display = ValidatorDisplay.Dynamic;
                        break;
                    case AttributeValidatorDisplayStyle.Static:
                        base.Display = ValidatorDisplay.Static;
                        break;
                    case AttributeValidatorDisplayStyle.None:
                    default:
                        base.Display = ValidatorDisplay.None;
                        break;
                }
            }
        }

        #endregion

        #region Overrides

        protected override void OnPreRender(EventArgs e)
        {
            if (!string.IsNullOrEmpty(_errorTextResourceKey))
            {
                base.Text = GetLocalizedError(_errorTextResourceKey);
            }
            if (String.IsNullOrEmpty(base.Text))
            {
                base.Display = ValidatorDisplay.None;
            }
            if (!string.IsNullOrEmpty(_errorSummaryTextResourceKey))
            {
                base.ErrorMessage = GetLocalizedError(_errorSummaryTextResourceKey);
            }
            base.OnPreRender(e);
        }

        #endregion

        #region private methods 

        private string GetLocalizedError(string resourceKey)
        {
            CorbisBasePage basePage = (CorbisBasePage)Page;
            string errorText = String.Empty;
            if (!string.IsNullOrEmpty(resourceKey))
            {
                errorText = basePage.GetLocalResourceString(resourceKey);
            }
            return errorText;
        }

        #endregion
    }
}
