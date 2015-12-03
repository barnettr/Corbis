using System;
using System.Collections.Generic;
using System.Text;
using Corbis.Web.Entities;

namespace Corbis.Web.UI.ViewInterfaces
{
    public interface IQuickPicView
    {
        List<QuickPicItem> QuickPicList { get; set;}

    }
}
