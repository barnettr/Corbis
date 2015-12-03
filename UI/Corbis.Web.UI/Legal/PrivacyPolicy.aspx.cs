using System;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.UI.Presenters;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.Legal
{
    public partial class PrivacyPolicy : CorbisBasePage, IPrivacyPolicyView
    {
        public LegalPresenter legalPresenter;

        protected override void OnInit(EventArgs e)
        {
            this.RequiresSSL = null;
            base.OnInit(e);
            legalPresenter = new LegalPresenter(this);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.privacyPolicyText.Text = Resources.Legal.PrivacyPolicyText;
            this.privacyPolicyLabel.Text = Resources.Legal.PrivacyPolicyTitle;
            this.Title = Resources.Legal.PrivacyPolicyTitle;
            legalPresenter.SetTrusteVisibility();
            
            base.AnalyticsData["channel"] = "Legal";
            base.AnalyticsData["pageName"] = "Legal:PrivacyPolicy";
        }

        #region IPrivacyPolicyView Members

        public bool ShowTrustee
        {
            set { trusteDiv.Visible = value; }
        }

        #endregion
    }
}
