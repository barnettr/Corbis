using System;
using System.Collections.Generic;
using Corbis.Web.Entities;
using Corbis.LightboxCart.Contracts.V1;

namespace Corbis.Web.UI.ViewInterfaces
{
    public interface IImageGroupsView : IView 
    {
        List<SearchResultProduct> SearchResultProducts { set; }
        
        // Lightboxes
        bool ShowAddToLightboxPopup { set; }
        List<Lightbox> LightboxList { set; }
        string ActiveLightbox { set; }
        List<CartDisplayImage> LightboxItems { get; set; }

        // Zero Search Results
        bool ShowZeroResults { set; }

        string ImageGroupName { get; set; }
        string ImageGroupId { get; set; }
        bool ShowCaptionButtonAndText { set; }
        string CaptionHeader { get; set; }
        string CaptionText { set;}

        int ItemsPerPage { get; set; }
        int CurrentPageNumber { get; set; }
        int TotalRecords { get; set; }
        int CurrentPageHitCount { get; set; }

        Corbis.Web.UI.Presenters.ImageGroups.ImageGroupsPresenter Presenter { get; }
        
        //quick pic
        bool ShowQuickPicTab { get; set; }
        void AdjustStatusForUser();


        //recent Image URL
        //string RecentImageURL { set; }
        Decimal RecentImageRadio { set; }
        //recent Image Id
        //string RecentImageId { set; }
    }
}
