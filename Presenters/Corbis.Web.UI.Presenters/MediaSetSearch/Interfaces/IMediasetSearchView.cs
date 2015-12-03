using System.Collections.Generic;
using Corbis.Image.Contracts.V1;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.LightboxCart.Contracts.V1;
namespace Corbis.Web.UI.Presenters.MediaSetSearch
{
    public interface IMediasetSearchView : IView
    {
        List<MediaSet> MediasetList { set; }
       
        // Lightboxes
       // bool ShowAddToLightboxPopup { set; }
        List<Lightbox> LightboxList { set; }
        string ActiveLightbox { set; get; }
        List<LightboxDisplayImage> LightboxItems { get; set; }

        // Zero Search Results
        bool ShowZeroResults { set; }

        

        int ItemsPerPage { get; set; }
        int CurrentPageNumber { get; set; }
        int TotalRecords { get; set; }
        int CurrentPageHitCount { get; set; }
        

        //quick pic
        bool ShowQuickPicTab { get; set; }
        void AdjustStatusForUser();


    }
}
