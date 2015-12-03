using System;
using Corbis.Web.UI.Presenters;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.Legal
{
	public partial class LicenseInfo : CorbisBasePage, ILicenseContentView
	{
		protected override void OnLoad(EventArgs e)
		{
			LegalPresenter licenseTermsPresenter = new LegalPresenter(this);
			Response.Redirect(licenseTermsPresenter.GetLicenseURL());
		}

        protected override void OnInit(EventArgs e)
        {
            this.RequiresSSL = null;
            base.OnInit(e);
        }
	}
}
