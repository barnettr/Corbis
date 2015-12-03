using System;
using System.Collections.Generic;
using Corbis.RFCD.Contracts.V1;
using Corbis.Search.Contracts.V1;
using Corbis.Web.Entities;

namespace Corbis.Web.UI.ViewInterfaces
{
    public interface IRFCDResultsView : IView
    {
        string CdName { set; }
        string NumberOfImages { set; }
        string RfcdID { set; }
        string RfcdImage { set; }
        string ImagePrice { set; }
        string ImageSize { set; }
        string RfcdID2 { set; }
        string NumberOfImages2 { set; }
        string Copyright { set; }
        string RfcdText { set; }
        List<RFCDEntity> InterestedRepeater { set;}
        List<RfcdDisplayImage> WebProductList { set;}
        // List<SearchResultProduct> SearchResultProducts { set; }
        string VolumeNumber { get; }
        Guid VolumeGuid { get; set;}
    }
}
