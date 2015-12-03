using System;
using System.Collections.Generic;
using Corbis.DisplayText.Contracts.V1;

namespace Corbis.Web.UI.Presenters.Tools.Interfaces
{
    public interface IDisplayTextView
    {
        Dictionary<int, DisplayTextEntity> DisplayTextEntities { set; }
        Dictionary<int, DisplayTextEntity> DisplayCountries { set; }
        Dictionary<int, DisplayTextEntity> DisplaySearchSortOrder { set; }
        Dictionary<int, DisplayTextEntity> DisplayMarketingCollections { set; }
        Dictionary<int, DisplayTextEntity> DisplayArchiveCollections { set; }
        Dictionary<int, DisplayTextEntity> DisplayCategories { set; }
        Dictionary<int, DisplayTextEntity> DisplayNumberOfPeople { set; }
        Dictionary<int, DisplayTextEntity> DisplayPointOfViews { set; }
        Dictionary<int, DisplayTextEntity> DisplayRMImageSize { set; }
        Dictionary<int, DisplayTextEntity> DisplayContentTypes { set; }
        Dictionary<int, DisplayTextEntity> DisplayPrimarySubject { set; }
        Dictionary<int, DisplayTextEntity> DisplayRightOfFirstSale { set; }
        Dictionary<string, string> DisplayOrientation { set; }
        Dictionary<string, string> DisplayMediaTypes { set; }
        Dictionary<string, string> DisplayColorFormats { set; }
        Dictionary<string, string> DisplayExternalAddedDate { set; }
        Dictionary<string, string> DisplayMagazinePublishDate { set; }
        Dictionary<string, string> DisplayInternalAddedDate { set; }
        Dictionary<string, string> DisplayDatePhotographed { set; }
        Dictionary<string, string> DisplayMediaRatings { set; }
    }
}
