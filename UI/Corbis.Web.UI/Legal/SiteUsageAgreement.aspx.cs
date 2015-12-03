using System;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.Legal
{
    public partial class SiteUsageAgreement : CorbisBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.agreement.Text = Resources.Legal.SiteUsageAgreementText;
            this.pageTitle.Text = Resources.Legal.SiteUsageAgreementTitle;
            this.Title = Resources.Legal.SiteUsageAgreementTitle;

            AnalyticsData["channel"] = "Legal";
            AnalyticsData["pageName"] = "Legal:SiteUsage";
        }

        protected override void OnInit(EventArgs e)
        {
            this.RequiresSSL = null;
            base.OnInit(e);
        }
    }
}
