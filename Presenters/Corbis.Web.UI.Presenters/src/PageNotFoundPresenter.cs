using System;
using System.Collections.Generic;
using System.Text;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Framework.Logging;

namespace Corbis.Web.UI.Presenters
{
    public class PageNotFoundPresenter
    {
        private const string MISSINGPAGE_SUBJECT = "404:MissingPage";

        private IPageNotFoundView view;

        public IPageNotFoundView View
        {
            get
            {
                return view;
            }
        }

        
        public PageNotFoundPresenter(IPageNotFoundView pageView)
        {
            if (pageView == null)
            {
                throw new ArgumentNullException(
                                    "PageNotFoundPresenter:Constructor",
                                    "parameter pageView is null.");
            }

            view = pageView;

        }

        public void CaptureMissingPage(string pageInfo)
        {
            if (String.IsNullOrEmpty(pageInfo))
            {
                throw new ArgumentOutOfRangeException(
                                    "PageNotFoundPresenter:CaptureMissingPage: " +
                                    "pageInfo must not be null or empty");
            }

            if (view.LoggingContext == null)
            {
                view.LoggingContext = new LoggingContext(new List<string>());
            }
            view.LoggingContext.LogInformationMessage(
                    MISSINGPAGE_SUBJECT, String.Format("Missing Page Name: {0}", pageInfo));
        }
    }
}
