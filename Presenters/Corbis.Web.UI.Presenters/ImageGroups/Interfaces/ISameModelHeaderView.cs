using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.UI.ViewInterfaces
{
    public interface ISameModelHeaderView : IImageGroupHeader  
    {
        string Id { set; }
        string ImageCount { set; }
        string Photographer { set; }
    }
}
