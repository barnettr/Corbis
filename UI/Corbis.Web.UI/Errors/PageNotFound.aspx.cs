using System;
using Corbis.Web.UI.Presenters;
using Corbis.Web.UI.ViewInterfaces;
using System.Web;

namespace Corbis.Web.UI.Errors
{
    /// <summary>
    /// Used as a 404 Error Page as a default
    /// </summary>
    public partial class PageNotFound : CorbisBasePage, IPageNotFoundView
    {
        private const string ERROR_PATH_QUERYSTRING = "aspxerrorpath";

        private PageNotFoundPresenter presenter;


        protected void Page_Load(object sender, EventArgs e)
        {
            presenter = new PageNotFoundPresenter(this);
            CaptureMissingPageInformation();
        }

        /// <summary>
        /// Get the querystring for the error path and log information.
        /// </summary>
        private void CaptureMissingPageInformation()
        {
            string missingPage = this.Request[ERROR_PATH_QUERYSTRING];

            if (!String.IsNullOrEmpty(missingPage))
            {
                presenter.CaptureMissingPage(missingPage);
            }
            else
            {
                presenter.CaptureMissingPage(HttpContext.Current.Request.Url.AbsoluteUri);
            }
        }
    }
}
