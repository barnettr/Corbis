using System;


namespace Corbis.Web.UI.ViewInterfaces
{
    public interface IImageGroupHeader : IView
    {
        bool Visible { set; }
        string RecentImageId { get; set; }
        string RecentImageURL { set; }
        string ImageGroupName { get; }
        string ImageGroupId { get; }
        Decimal RecentImageRadio { set; }
    }
}
