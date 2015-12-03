using System;
using System.Collections.Generic;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.Web.Entities;
using Corbis.MarketingCollection.Contracts.V3;

namespace Corbis.Web.UI.ViewInterfaces
{
    public interface ISearchView : ISearchBaseView
    {
        // Keyword Search box
        string KeywordSearch { get; set; }

        //Advanced Search
        string DateCreated { get; set; }
        bool DaysChecked { get; set;}
        string Days { get; set; }
        bool DateRangeChecked{ get; set;}
        string BeginDate { get; set; }
        string EndDate { get; set; }
        string Location { get; set; }
        string Photographer { get; set; }
        string Provider { get; set; }
        bool HorizontalCheckbox { get; set; }
        bool PanoramaCheckbox { get; set; }
        bool VerticalCheckbox { get; set; }
        string HorizontalLabelText { get; set; }
        string PanoramaLabelText { get; set; }
        string VerticalLabelText { get; set; }
        string PointOfView { get; set; }
        string NumberOfPeople { get; set; }
        string ImmediateAvailability { get; set; }
        string ImageNumbers { get; set; }
		object MarketingCollection { get; set; }
        List<string> SelectedMarketingCollection { get; set; }
        string PremiumCollectionsCountSummary { get; set; }
        string PremiumCollectionsTotalSummary { get; set; }
        string StandardCollectionsCountSummary { get; set; }
        string StandardCollectionsTotalSummary { get; set; }
        string ValueCollectionsCountSummary { get; set; }
        string ValueCollectionsTotalSummary { get; set; }
        string SuperValueCollectionsCountSummary { get; set; }
        string SuperValueCollectionsTotalSummary { get; set; }
        bool ShowPremiumCollectionsSummary { set; }
        bool ShowStandardCollectionsSummary { set; }
        bool ShowValueCollectionsSummary { set; }
        bool ShowSuperValueCollectionsSummary { set; }
        bool RemoveMoreSearchOptions { get; set; }
        bool ShowOptionsAppliedStyle { get; set; }
        string OrientationSummary { get;  set; }
        List<string> GetSelectedMarketingCollections(MarketingCollectionGroupType marketingCollectionGroupType);
        int CountMarketingCollections(MarketingCollectionGroupType marketingCollectionGroupType);
        MoreSearchOptions MoreSearchOptionsSettings { get; set; }
        SearchSort SearchSortOption { get; set; }
        
        //Return to previous search
        bool ShowReturnToPreviousSearch { set; }
        string PreviousSearchURL { set; }
        bool IsMSOVisible { get; }
    }
}
