using System;
using Corbis.CommonSchema.Contracts.V1;

namespace Corbis.Web.UI.ViewInterfaces
{
    public interface ISearchBaseView : IView
    {
        // Category Filters
        bool Creative { get; set;}
        bool Editorial { get; set;}
        bool Documentary { get; set;}
        bool Archival { get; set;}
        bool CurrentEvents { get; set;}
        bool FineArt { get; set;}
        bool Entertainment { get; set; }
        bool Outline { get; set;}

        // License Model Filters
        bool RightsManaged { get; set;}
        bool RoyaltyFree { get; set;}

        // Other Filters
        bool NoPeople { get; set;}

        bool Photography { get; set;}
        bool Illustration { get; set;}
        
        bool Color { get; set;}
        bool BlackWhite { get; set;}
        
        bool ModelReleased { get; set;}
        SearchSort SearchSortOption { get; set; }
    }
}
