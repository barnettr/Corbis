using System;
using System.Collections.Generic;
using Corbis.Image.Contracts.V1;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Web.Entities;
using Corbis.CommonSchema.Contracts.V1;

namespace Corbis.Web.UI.ViewInterfaces
{
    public interface IPostSearchView : ISearchBaseView
    {
        // Lightboxes
        bool ShowAddToLightboxPopup { set; }
        List<Lightbox> LightboxList { set; }
        List<CartDisplayImage> LightboxItems { get; set; }

        // Search Result Header Properties
        int TotalSearchHitCount { set; }
        int CurrentPageHitCount { set; }
        int CurrentPageNumber { set; }
        string SearchResultTitle { set; }
        string SearchResultPhotographerName { set; }
        string SearchResultLocation { set; }

        
        //List<WebProduct> WebProducts { set; }
        List<SearchResultProduct> SearchResultProducts { set; }
        // void ShowClarifications(List<Clarification> clarifications);
        bool ShowQuickPicTab { get; set; }
        void AdjustStatusForUser();

        // Clarification
        bool ShowClarification { get; }
        bool ShowClarificationPopup { get; set; }

        string ClarificationsQueryFlags { get; }

        List<Clarification> Clarifications { get; set; }

        // Zero Search Results
        bool ShowZeroResults { set; }

        ////recent Image URL
        string RecentImageURL { set;}
        Decimal RecentImageRadio { set; }
        //recent Image Id
        string RecentImageId { set; }

        //Search sort options
        SearchSort SearchSortOption { get; set; }
        SearchSort? SearchSortOptionHidden { get; set; }

    }
}
