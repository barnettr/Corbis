using System;
using System.Collections.Generic;
using System.Text;
using Corbis.Image.Contracts.V1;

namespace Corbis.Web.UI.ViewInterfaces
{
    public interface IRFCDHeaderView : IImageGroupHeader 
    {
        string RFCDImageURL { set; }
        string Id { set; }
        string Title { set; }
        string FileSize { set; }
        string Copyright { set; }
        string Price { set; }
        string ImageCount { set; }
        //List<String> RFCDImageFileSizeList { set; }
        bool AddAllImagetoLightBoxVisible { set; get; }
        string  RFCDUid { set; get; }
        bool ShowAddToCartButton { set; }
    }
}
