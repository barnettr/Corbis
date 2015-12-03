using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.UI.ViewInterfaces
{
    public interface IStorySetHeaderView : IImageGroupHeader 
    {
        string Title { set; }
        string Id { set; }
        string ImageCount { set; }
        string Date { set; }
        string Location { set; }
        string Photographer { set; }
    }
}
