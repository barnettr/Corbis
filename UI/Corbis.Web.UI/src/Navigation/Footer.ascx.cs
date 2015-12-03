using System;
using Corbis.Web.UI.Presenters;
using Corbis.Framework.Globalization;
using Corbis.Web.Authentication;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Utilities;


namespace Corbis.Web.UI.Navigation
{
    public partial class Footer : CorbisBaseUserControl,IFooterView
    {
        private LegalPresenter legalPresenter;
        private FooterPresenter footerPresenter;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            legalPresenter = new LegalPresenter(this);
            footerPresenter = new FooterPresenter();
            footerPresenter.FooterView = this;

            //rightsClearances.NavigateUrl = SiteUrls.RightsClearances;
            aboutCorbis.NavigateUrl = SiteUrls.Overview;
            pressroom.NavigateUrl = SiteUrls.Pressroom;
            employment.NavigateUrl = SiteUrls.Employment;
            customerService.NavigateUrl = SiteUrls.CustomerService;
            LocalizeCorporateUrls();
            //siteUsageAgreement.NavigateUrl = SiteUrls.SiteUsageAgreement;
            //privacyPolicyLink.NavigateUrl = SiteUrls.PrivacyPolicy;
            //licenseInformation.NavigateUrl = legalPresenter.GetLicenseURL();
            licenseInformation.NavigateUrl = SiteUrls.LicenseAgreement;

            this.SetCopyrightText();
            footerPresenter.SetFooterVisibility();


        }

        private void LocalizeCorporateUrls()
        {
            //French and German sites have different corporate urls. Bug 17897
           // Get the current culture information.
           string currentCulture = Language.CurrentLanguage.LanguageCode;

           if (Language.CurrentLanguage == Language.French || Language.CurrentLanguage == Language.German)
           {
               string lcdCode = "lcd=" + currentCulture;
               aboutCorbis.NavigateUrl = AppendQP(SiteUrls.Overview,"lcd" , currentCulture);
               pressroom.NavigateUrl = AppendQP(SiteUrls.Pressroom ,"lcd" , currentCulture);
               employment.NavigateUrl = AppendQP(SiteUrls.Employment , "lcd" , currentCulture);
           }
        }

        private string AppendQP(string url, string qpName, string qpValue)
        {
            if (url != null)
            {
                bool qps = true;
                if (url.IndexOf("?") == -1)
                {
                    url += "?";
                    qps = false;
                }
                if (qps)
                    url += "&";
                url += qpName + "=" + qpValue;
            }
            return url;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (this.Page.MasterPageFile == SiteUrls.NoGlobalNavMasterFileLoc)
            {
                LargeFooter.Attributes.Add("style", "display: none;");
            }
            else
            {
                SmallFooter.Attributes.Add("style", "display: none;");
            }            
        }

        private void SetCopyrightText()
        {
            this.copyright.Text = CopyrightHelper.CopyrightText;
            this.copyright2.Text = this.copyright.Text;
        }

        public bool ImprintVisibility
        {
            get { return this.imprint.Visible; }
            set { 
                this.imprint.Visible = value;
                if (!this.imprint.Visible)
                {
                    this.imprintDiv.Attributes.Add("style", "display: none;");
                }
                else
                {
                    this.imprintDiv.Attributes.Add("style", "display: block;");
                }
            }            
        }

    }
}