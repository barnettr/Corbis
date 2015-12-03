using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.WebOrders.Contracts.V1;

namespace Corbis.Web.UI.ViewInterfaces
{
    public interface IQuickPicDownloadView
    {
        List<DownloadableQuickPicImage> DownloadableQuickPicImages { get; set; }
    }
}
