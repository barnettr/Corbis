using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.Web.UI.Checkout;
using Corbis.Web.Utilities;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Web.UI.ViewInterfaces;
using System.Collections.Generic;
using System.Text;
using Corbis.Image.Contracts.V1;
using Corbis.Web.UI.Presenters;
using Corbis.Web.UI.Pricing.Interfaces;
using Corbis.Web.UI.Presenters.Lightboxes;
using Corbis.Web.UI.Presenters.Pricing;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Web.UI.Presenters.Checkout;
using Corbis.Framework.Globalization;
namespace Corbis.Web.UI.Checkout
{
    public partial class ExpressCheckout_LicenseDetails : CorbisBasePage, IRMPricingExpressCheckout
    {
        private PricingPresenter pricingPresenter;
        private string licenseStartDateText;

        protected override void OnInit(EventArgs e)
        {
            this.RequiresSSL = true;
            base.OnInit(e);
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.ExpressCheckout, "ExpressCheckoutCSS");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            corbisId = Request["CorbisId"];
            GuidHelper.TryParse(Request["ProductUid"], out productUid);
            GuidHelper.TryParse(Request["savedUsageUid"], out savedUsageUid);
            if (!string.IsNullOrEmpty(Request["licenseStartDate"]))
            {
                var licenseStartDtText = Request["licenseStartDate"];
                DateTime dt = new DateTime();
                DateHelper.TryParse(licenseStartDtText, out dt);
                LicenseStartDate = dt;
            }
            pricingPresenter = new PricingPresenter(this);

            System.Collections.Specialized.NameValueCollection valueColl = new System.Collections.Specialized.NameValueCollection();

            if (productUid != Guid.Empty)
            {
                SetResponseHeader();
            }
        }

        private void SetResponseHeader()
        {
            if (pricingPresenter.BuildExpressCheckoutUsage())
            {
                if (this.ShowPriceStatusMessage)
                {
                    Response.AddHeader("validationResult", "CheckoutNotAllowed");
                }
                else if (this.PricedByAE && this.CustomPricedExpired)
                {
                    Response.AddHeader("validationResult", "AEPriceExpired");
                }
                else if (this.PricedByAE)
                {
                    Response.AddHeader("validationResult", "PricedByAE");
                }
                else
                {
                    Response.AddHeader("validationResult", "Success");
                }

                Response.AddHeader("licenseStartDate",
                    (this.LicenseStartDate != DateTime.MinValue) ? this.LicenseStartDate.ToShortDateString() : string.Empty);
            }
            else
            {
                // Response to display an alert for invalid usage.
                if (this.EmptyUsage)
                {
                    Response.AddHeader("validationResult", "NoUsage");
                }
                else
                {
                    Response.AddHeader("validationResult", "Failure");
                }
            }
        }

        public void DisplayLicenseDetail()
        {
            StringBuilder sbAttributeDetails = new StringBuilder();

            foreach (UseTypeAttribute attribute in useTypeWithAttributes.UseTypeAttributes)
            {
                if (!string.IsNullOrEmpty(attribute.DisplayText)
                    && attribute.Values != null && attribute.Values.Count == 1
                    && !string.IsNullOrEmpty(attribute.Values[0].DisplayText)
                    && attribute.DisplayType != AttributeDisplay.None)
                {
                    sbAttributeDetails.Append("<div class=\"QuestionText\">" + attribute.DisplayText + ":</div><div class=\"AnswerText\">" + attribute.Values[0].DisplayText + "</div>");
                }
            }

            StringBuilder sbLicenseDetails = new StringBuilder();
            sbLicenseDetails.Append(UseTypeHelpText);
            sbLicenseDetails.Append(UseCategoryLicenseDetails);
            sbLicenseDetails.Append(UseTypeLicenseDetails);
            sbLicenseDetails.Append(sbAttributeDetails.ToString());
            sbLicenseDetails.Append(licenseStartDateText);
            AllLicenseDetails = sbLicenseDetails.ToString();
        }


        #region IRMPricing Members

        private Guid productUid;
        public Guid ProductUid
        {
            get { return productUid; }
            set { productUid = value; }
        }

        private Guid savedUsageUid;
        public Guid SavedUsageUid
        {
            get { return savedUsageUid; }
            set { savedUsageUid = value; }
        }

        private string corbisId;
        public string CorbisId
        {
            get { return corbisId; }
            set { corbisId = value; }
        }

        #region License Details

        public string UseTypeHelpText
        {
            get
            {
                return
                (UseTypeWithAttributes != null &&
                 !String.IsNullOrEmpty(UseTypeWithAttributes.HelpText))
                 ? "<div class=\"AnswerText\">" + UseTypeWithAttributes.HelpText + "</div>"
                 : String.Empty;
            }
        }

        public string UseCategoryLicenseDetails
        {
            get
            {
                return
                (this.UseCategories != null && this.UseCategories.Count == 1
                && !String.IsNullOrEmpty(this.UseCategories[0].DisplayText))
                ? "<div class=\"licenseDetailsBlockSeperatorTop QuestionText\">" + GetLocalResourceString("useCategoryLicenseDetails.Text") + "</div><div class=\"AnswerText\">" + this.UseCategories[0].DisplayText + "</div>"
                : String.Empty;
            }
        }

        public string UseTypeLicenseDetails
        {
            get
            {
                return
                (this.UseTypeWithAttributes != null
                && !String.IsNullOrEmpty(this.UseTypeWithAttributes.DisplayText))
                ? "<div class=\"QuestionText\">" + GetLocalResourceString("useTypeLicenseDetails.Text") + "</div><div class=\"licenseDetailsBlockSeperatorBottom\">" + this.UseTypeWithAttributes.DisplayText + "</div>"
                : String.Empty;
            }
        }


        private DateTime licenseStartDate;
        public DateTime LicenseStartDate
        {
            get { return licenseStartDate; }
            set
            {
                licenseStartDate = value;
                if (licenseStartDate != DateTime.MinValue)
                {
                    string dateText = licenseStartDate.ToString(Language.CurrentCulture.DateTimeFormat.ShortDatePattern);
                    licenseStartDateText = "<div class=\"QuestionText\">" + GetLocalResourceString("licenseStartDate.Text") + "</div><div id=\"licenseStartDateDiv\" class=\"licenseDetailsBlockSeperatorBottom\">" + dateText + "</div>";
                    
                    if (licenseStartDate < DateTime.Today)
                    {
                        // TODO: date is invalid.
                    }
                }

            }
        }

        public string AllLicenseDetails
        {
            get
            {
                //return "";
                return licenseDetails.InnerHtml;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    licenseDetails.InnerHtml = value;
                }
                else
                {
                    licenseDetails.InnerHtml = GetLocalResourceString("licenseDetailsEmpty.Text");
                }
            }
        }

        #endregion

        private UseType useTypeWithAttributes;
        public UseType UseTypeWithAttributes
        {
            get
            {
                return useTypeWithAttributes;
            }
            set
            {
                useTypeWithAttributes = value;


            }
        }
        List<UseCategory> useCategories;
        public List<UseCategory> UseCategories
        {
            get
            {
                return useCategories;
            }
            set
            {
                useCategories = value;
            }
        }

        private List<UseType> useTypes;
        public List<UseType> UseTypes
        {
            get
            {
                return useTypes;
            }
            set
            {
                useTypes = value;
            }
        }

        public List<KeyValuePair<Guid, string>> SavedUsages
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

        private bool pricedByAE = false;
        public bool PricedByAE
        {
            get
            {
                return pricedByAE;
            }
            set
            {
                pricedByAE = value;
            }
        }

        private bool customPricedExpired = false;
        public bool CustomPricedExpired
        {
            get
            {
                return customPricedExpired;
            }
            set
            {
                customPricedExpired = value;
            }
        }

        private bool showPriceStatusMessage = false;
        public bool ShowPriceStatusMessage
        {
            get
            {
                return showPriceStatusMessage;
            }
            set
            {
                showPriceStatusMessage = value;
            }
        }

        private bool emptyUsage = false;
        public bool EmptyUsage
        {
            get
            {
                return emptyUsage;
            }
            set
            {
                emptyUsage = value;
            }
        }

        #endregion

        #region IRMPricingExpressCheckout Members


        public string EffectivePriceText
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

        public String LightboxId { get; set; }

        #endregion


        #region IView Members

        Corbis.Framework.Logging.ILogging IView.LoggingContext
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

        IList<Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.ValidationDetail> IView.ValidationErrors
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
