using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.UI.ViewInterfaces
{
    public interface IOutlineHeaderView : IImageGroupHeader  
    {
        string ImageCount { set; }
        string Photographer { set; }
        string DatePublished { set; }
        string CreditLine { set; }
        List<string> FeaturedCelebrities { set; }
    }
}
