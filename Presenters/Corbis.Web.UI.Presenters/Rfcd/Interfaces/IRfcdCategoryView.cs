using System;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.RFCD.Contracts.V1;
using System.Collections.Generic;


namespace Corbis.Web.UI.Presenters.Rfcd.ViewInterfaces
{
    public interface IRfcdCategoryView : IView
    {
        Guid CategoryUID { get; set; }
        string PreviousLanguageCode { get; set; }

        string CategoryTitle { get; set; }
        List<RFCDEntity> RFCDsByFirstLetterOrCategory { set; }
        string RFCDCategories { set; }
    }
}
