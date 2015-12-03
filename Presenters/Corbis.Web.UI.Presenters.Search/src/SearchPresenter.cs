using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.Framework.Globalization;
using Corbis.Framework.Logging;
using Corbis.Image.Contracts.V1;
using Corbis.Image.ServiceAgents.V1;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.LightboxCart.ServiceAgents.V1;
using Corbis.MarketingCollection.Contracts.V3;
using Corbis.MarketingCollection.ServiceAgents.V3;
using Corbis.Membership.Contracts.V1;
using Corbis.Search.Contracts.V1;
using Corbis.Web.Entities;
using Corbis.Web.UI.Controls;
using Corbis.Web.UI.Presenters.QuickPic;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Utilities.StateManagement;
using System.Web.UI;


namespace Corbis.Web.UI.Presenters.Search
{
    public class SearchPresenter : BasePresenter
    {
        #region Constants

        // should match Corbis.Search.BusinessActions.V1.Constants.GetClarification.MaxKeywordCountLimit
        private const int MAX_KEYWORD_COUNT_LIMIT = 2;
        private const string BEGIN_DATE = "mm/dd/yyyy";
        private const string END_DATE_FORMAT = "MM/dd/yyyy";
        public const string ChinaCountryCode = "CN";

        #endregion

        #region Private members

        private IImageContract _imageAgent;
        private ILightboxCartContract _lightboxCartAgent;
        private IMarketingCollectionContract _marketingCollectionAgent;
        private IPostSearchView postSearchView;

        private StateItemCollection stateItems;
        private IView view;
        private char _initializedSharedFiltersChangedWithClarification;
        private bool _sharedFiltersChangedWithClarification;
        

        #endregion

        #region Constructors

        public SearchPresenter(IView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException("SearchPresenter: SearchPresenter() - Search view cannot be null.");
            }
            // two flags used to handle SharedFiltersChangedWithClarification.
            _initializedSharedFiltersChangedWithClarification = 'N';
            _sharedFiltersChangedWithClarification = false;
            this.view = view;
            postSearchView = view as IPostSearchView;
            stateItems = new StateItemCollection(HttpContext);
        }

        #endregion

        #region Properties

        public IPostSearchView PostSearchView
        {
            set
            {
                if (postSearchView == null)
                {
                    postSearchView = value;
                }
            }
        }

        public IImageContract ImageAgent
        {
            get { return _imageAgent ?? new ImageServiceAgent(); }
            set { _imageAgent = value; }
        }

        public ILightboxCartContract LightboxCartAgent
        {
            get { return _lightboxCartAgent ?? new LightboxCartServiceAgent(); }
            set { _lightboxCartAgent = value; }
        }

        public IMarketingCollectionContract MarketingCollectionAgent
        {
            get { return _marketingCollectionAgent ?? new MarketingCollectionServiceAgent(); }
            set { _marketingCollectionAgent = value; }
        }
        /// <summary>
        /// Returns True if SharedFilters are Changed After a Clarification is done.
        /// </summary>
        //todo: bug # 16531 Change method to return void.  Set a bool property on the view instead 
        public bool SharedFiltersChangedWithClarification
        {
            get
            {
                if (_initializedSharedFiltersChangedWithClarification == 'N')
                {
                    _sharedFiltersChangedWithClarification = AreSharedFiltersChangedWithClarification();
                    _initializedSharedFiltersChangedWithClarification = 'Y';
                }
                return _sharedFiltersChangedWithClarification;
            }
        }
        #endregion

        /// <summary>
        /// Populates the selected and total counts for the More Search Options layer
        /// </summary>
        /// <returns></returns>
        public void PopulateOptionsAppliedWithCollectionsData()
        {
            if (view is ISearchView)
            {
                ISearchView searchView = (ISearchView)view;

                searchView.PremiumCollectionsCountSummary = (searchView.GetSelectedMarketingCollections(MarketingCollectionGroupType.Premium)).Count.ToString();
                searchView.PremiumCollectionsTotalSummary = searchView.CountMarketingCollections(MarketingCollectionGroupType.Premium).ToString();

                if (searchView.PremiumCollectionsCountSummary != searchView.PremiumCollectionsTotalSummary)
                {
                    searchView.ShowPremiumCollectionsSummary = true;
                }
                searchView.StandardCollectionsCountSummary = (searchView.GetSelectedMarketingCollections(MarketingCollectionGroupType.Standard)).Count.ToString();
                searchView.StandardCollectionsTotalSummary = searchView.CountMarketingCollections(MarketingCollectionGroupType.Standard).ToString();

                if (searchView.StandardCollectionsCountSummary != searchView.StandardCollectionsTotalSummary)
                {
                    searchView.ShowStandardCollectionsSummary = true;
                }

                searchView.ValueCollectionsCountSummary = (searchView.GetSelectedMarketingCollections(MarketingCollectionGroupType.Value)).Count.ToString();
                searchView.ValueCollectionsTotalSummary = searchView.CountMarketingCollections(MarketingCollectionGroupType.Value).ToString();

                if (searchView.ValueCollectionsCountSummary != searchView.ValueCollectionsTotalSummary)
                {
                    searchView.ShowValueCollectionsSummary = true;
                }

                searchView.SuperValueCollectionsCountSummary = (searchView.GetSelectedMarketingCollections(MarketingCollectionGroupType.SuperValue)).Count.ToString();
                searchView.SuperValueCollectionsTotalSummary = searchView.CountMarketingCollections(MarketingCollectionGroupType.SuperValue).ToString();

                if (searchView.SuperValueCollectionsCountSummary != searchView.SuperValueCollectionsTotalSummary)
                {
                    searchView.ShowSuperValueCollectionsSummary = true;
                }
            }
        }

        /// <summary>
        /// Get the value to set in the QueryString and Saves to Session.
        /// </summary>
        /// <returns></returns>
        //todo: bug # 16532 Remove the return value from the following method.  Set the value on a view property.
        public string BuildQuery()
        {
            string query = BuildSearchQueryOnly();
            // Check if there are any Clarifications when Shared Filters are 
            // changed for the Same Keyword
            if (SharedFiltersChangedWithClarification)
            {
                query = BuildQueryWithClarificationsQueryFlagsForChangedSharedFilters(query);
            }
            //Guid CalculateFilterDifferences(List<SearchFilter>  query)
            List<SearchFilter> searchFilters = BuildSearchFilters(ConvertStringToNameValueCollection(query));
            Guid SearchGuid = CalculateFilterDifferences(searchFilters);
            StateItem<Guid> searchIdentiFier = new StateItem<Guid>(SearchSessionKeys.SearchQuery, SearchSessionKeys.PreviousSearchUid, SearchGuid, StateItemStore.AspSession);
            stateItems.SetStateItem<Guid>(searchIdentiFier);

            SaveQueryToSession(query);
            return query;
        }

        /// <summary>
        /// Get the value to set in the QueryString.
        /// </summary>
        /// <returns></returns>
        private string BuildSearchQueryOnly()
        {
            string query = string.Empty;

            if (view is ISearchView)
            {
                ISearchView searchView = (ISearchView) view;

                query += BuildSearchQueryFromSearchView();
                query += BuildSearchQueryFromSharedFilters();

                if (postSearchView != null && !string.IsNullOrEmpty(postSearchView.ClarificationsQueryFlags))
                {
                    query = AppendClarificationParameterToQueryString(query, postSearchView.ClarificationsQueryFlags, false, true);
                }


            }

            if (query.Length > 0)
            {
                query = query.ToString().Remove(query.ToString().Length - 1);
            }

            return query;
        }

        /// <summary>
        /// Appends Clarifications Parameter to Query String
        /// </summary>
        /// <param name="query">Query String to which the Clarifications are appended</param>
        /// <param name="clarificationsQueryFlags">Selected Clarification Flags</param>
        /// <param name="withPrefixAmpersand">Adds an Ampersand before the Clarifications parameter</param>
        /// <param name="withPostfixAmpersand">Adds an Ampersand after the Clarifications parameter</param>
        /// <returns></returns>
        private string AppendClarificationParameterToQueryString(string query, string clarificationsQueryFlags, bool withPrefixAmpersand, bool withPostfixAmpersand)
        {
            if (!string.IsNullOrEmpty(clarificationsQueryFlags))
            {
                if (withPrefixAmpersand)
                {
                    query += string.Concat("&");
                }

                query += string.Concat(SearchFilterKeys.ClarificationSelection, "=", clarificationsQueryFlags);
                
                if (withPostfixAmpersand)
                {
                    query += string.Concat("&");
                }
            }
            return query;
        }

        /// <summary>
        /// Saves Query to Session for "DirectlyManipulatedSearch" and "PreviousSearch" under "SearchQuery"
        /// </summary>
        /// <param name="query"></param>
        private void SaveQueryToSession(String query)
        {
            if (HasDirectlyManipulatedKeyword(ConvertSearchQueryToNameValueCollection(query), true))
            {
                //StateItem<string> direcltyManipulatedQueryString = new StateItem<string>(SearchSessionKeys.DirectlyManipulatedSearch, null, query, StateItemStore.Cookie);
                //stateItems.SetStateItem(direcltyManipulatedQueryString);
                
            }
            StateItem<string> searchQueryString = new StateItem<string>(SearchSessionKeys.SearchQuery, SearchSessionKeys.PreviousSearch, query, StateItemStore.AspSession);
            stateItems.SetStateItem(searchQueryString);
        }

        //todo: bug # 16533 Move the input parameter for the following method to the view.  Access them off the view
        //todo: bug # 16533 Remove the return value from the following method.  Set the value on a view property.
        public string BuildQueryStringForPagingAndSort(int index, string clarificationsQueryFlags, NameValueCollection queryCollection)
        {
            string query = string.Empty;
            query += BuildQueryStringForPagingAndSortOnly(index, queryCollection);
            query += BuildQueryStringForPagingClarification(query, clarificationsQueryFlags, queryCollection);
            SaveQueryToSession(query);
            return query;  
        }

        private string GetClarificationsSelectedQueryStringForPagingOrSortChange(string currentSearchQuery)
        {
            // Get Previous Search Query
            string previousSearchQuery =
                stateItems.GetStateItemValue<string>(SearchSessionKeys.SearchQuery, SearchSessionKeys.PreviousSearch,
                                          StateItemStore.AspSession);
            // Compare currentSearchquery and previousSearchQuery
            // if there is no cl-Clarification term abort the compare.
            // 1) remove the cl-Clarification term and then compare
            // 2) if there is cl-Clarification term and Paging or Sort are changed
            // then append cl-Clarification and send it back.
            NameValueCollection previousQueryCollection = new NameValueCollection();
            NameValueCollection currentQueryCollection = new NameValueCollection();
            string clarificationQueryFlags = string.Empty;

            // Check if both previousSearchQuery string and CurrentSearchQuery string are not empty.
            if (!string.IsNullOrEmpty(previousSearchQuery) && !string.IsNullOrEmpty(currentSearchQuery))
            {
                previousQueryCollection = ConvertSearchQueryToNameValueCollection(previousSearchQuery);
                currentQueryCollection = ConvertSearchQueryToNameValueCollection(currentSearchQuery);
                // Check if there is a ClarificationsQueryFlags in the Previous Search Query String
                // Check if both Previous and Current Search Keywords are the same.
                // Check if only the Paging or Sortare changed.
                if (HasClarificationsQueryFlags(previousQueryCollection)
                    && KeywordsAreIdentical(previousQueryCollection, currentQueryCollection)
                    && PagingOrSortChanged(previousQueryCollection, currentQueryCollection)
                    )
                {
                    // Get ClarificationQueryFlags from Previous Query String.
                    clarificationQueryFlags = GetClarificationsQueryStringFromQuery(previousQueryCollection);
                }
            }
            return clarificationQueryFlags;
        }

        private bool PagingOrSortChanged(NameValueCollection previousQueryCollection, NameValueCollection currentQueryCollection)
        {
            bool pagingOrSortChanged = false;
            bool pagingChanged = false;
            bool sortChanged = false;

            // Check if Paging
            if (!(previousQueryCollection[SearchFilterKeys.Paging] == currentQueryCollection[SearchFilterKeys.Paging]))
            {
                pagingChanged = true;
            }
            // Check if Sort Changed
            if (!(previousQueryCollection[SearchFilterKeys.Sort] == currentQueryCollection[SearchFilterKeys.Sort]))
            {
                sortChanged = true;
            }

            if (pagingChanged || sortChanged)
            {
                pagingOrSortChanged = true;
            }

            return pagingOrSortChanged;
        }

        private string BuildQueryStringForPagingClarification(string currentSearchQuery, string clarificationsQueryFlags, NameValueCollection queryCollection)
        {
            string query = string.Empty;

            bool clarificationKeyFound = false;

            foreach (string key in queryCollection.Keys)
            {
                switch (key)
                {
                    case SearchFilterKeys.ClarificationSelection:
                        // Add Clarification Parameter to Query String when NoClarifications are Selected 
                        // for a term with clarifications
                        query += string.Concat("&" , key.ToString(), "=", queryCollection[key].ToString());
                        //GetNoClarificationsSelectedQueryString(clarificationsQueryFlags, queryCollection);
                        clarificationKeyFound = true;
                        break;
                }
                if (clarificationKeyFound)
                {
                    break;
                }
            }


            if (!clarificationKeyFound)
            {
                if (!string.IsNullOrEmpty(clarificationsQueryFlags))
                {
                    query += GetNoClarificationsSelectedQueryString(clarificationsQueryFlags, queryCollection);
                }
                else
                {
                    query += GetClarificationsSelectedQueryStringForPagingOrSortChange(currentSearchQuery);
                }
            }

            return query; 
        }

        private string BuildQueryStringForPagingAndSortOnly(int index, NameValueCollection queryCollection)
        {
            string query = string.Empty;
            bool sortKeyFound = false;
            bool pageKeyFound = false;
            foreach (string key in queryCollection.Keys)
            {
                switch (key)
                {
                    case SearchFilterKeys.ClarificationSelection:
                        // Dont add the clarifications in here there is a seperate method to handle 
                        // Clarifications.
                        break;
                    case SearchFilterKeys.Sort:
                        query += string.Concat(SearchFilterKeys.Sort.ToString(), "=", (int)postSearchView.SearchSortOption, "&");
                        sortKeyFound = true;
                        break;
                    case SearchFilterKeys.Paging:
                        query += string.Concat(SearchFilterKeys.Paging.ToString(), "=", index.ToString(), "&");
                        pageKeyFound = true;
                        break;
                    default:
                        query += string.Concat(key.ToString(), "=", queryCollection[key].ToString(), "&");
                        break;
                }
            }

            if (!sortKeyFound)
            {
                query += string.Concat(SearchFilterKeys.Sort.ToString(), "=", (int)postSearchView.SearchSortOption, "&");
            }

            if (!pageKeyFound)
            {
                query += string.Concat(SearchFilterKeys.Paging.ToString(), "=", index.ToString(), "&");
            }

            if (query.Length > 0)
            {
                query = query.ToString().Remove(query.ToString().Length - 1);
            }

            return query; 
        }

        /// <summary>
        /// Gets Clarifications QueryString parameter When 1) it is not present and 2) None of the Clarifications are Selected.
        /// </summary>
        /// <param name="clarificationsQueryFlags">Selected Clarifications Flags</param>
        /// <param name="currentUrl">Actual URL of the Request.</param>
        /// <returns></returns>
        
        //todo:  change the following to a private method
        public string GetNoClarificationsSelectedQueryString(string clarificationsQueryFlags, NameValueCollection queryCollection)
        {
            string query = string.Empty;
            // Add the clarifications flag when the Clarifications Parameter is not present
            if (!HasClarificationsQueryFlags(queryCollection))
            {
                // check if None of the Clarifications is selected and then add clarifications flag.
                if (!AtleastOneClarificationSelected(clarificationsQueryFlags))
                {
                    query = AppendClarificationParameterToQueryString(query, clarificationsQueryFlags, true, false);
                }
            }

            return query;
        }

        private string BuildSearchQueryFromSharedFilters()
        {
            string query = string.Empty;
            ISearchBaseView baseView = postSearchView ?? view as ISearchBaseView;
            if (baseView != null)
            {
                //this is crazy
                //if(Profile.Permissions.Contains(Permission.HasPermissionSearchOutline))
                //{
                //    query += string.Concat(SearchFilterKeys.Sort, "=", (int)SearchSort.DatePublished, "&");
                //}
                //else if (postSearchView == null)
                //{
                //    query += string.Concat(SearchFilterKeys.Sort, "=", (int)SearchSort.Relevancy , "&");
                //}
                //if(baseView != null)
                //{
                //    query += string.Concat(SearchFilterKeys.Sort, "=", (int)baseView.SearchSortOption, "&");
                //}

                //Category
                string filterValueString = GetFilterQueryValue<Category>(GetCategoryFilterList(baseView));
                if (filterValueString != string.Empty)
                {
                    query += string.Concat(SearchFilterKeys.Category, "=", filterValueString, "&");
                }

                //LicenseModels
                filterValueString = GetFilterQueryValue<LicenseModel>(GetLicenseModelFilterList(baseView));
                if (filterValueString != string.Empty)
                {
                    query += string.Concat(SearchFilterKeys.LicenseModel, "=", filterValueString, "&");
                }

                //MediaType
                filterValueString = GetFilterQueryValue<MediaType>(GetMediaTypesFilterList(baseView));
                if (filterValueString != string.Empty)
                {
                    query += string.Concat(SearchFilterKeys.MediaType, "=", filterValueString, "&");
                }

                //ColorFormats
                filterValueString = GetFilterQueryValue<ColorFormat>(GetColorFormatFilterList(baseView));
                if (filterValueString != string.Empty)
                {
                    query += string.Concat(SearchFilterKeys.ColorFormat, "=", filterValueString, "&");
                }

                // NumberOfPeople
                filterValueString = GetFilterQueryValue<NumberOfPeople>(GetNumberOfPeopleFilterList(baseView));
                if (filterValueString != string.Empty)
                {
                    query += string.Concat(SearchFilterKeys.NumberOfPeople, "=", filterValueString, "&");
                }

                // Model Released
                if (baseView.ModelReleased)
                {
                    query += string.Concat(SearchFilterKeys.ModelReleased, "=1&");
                }
            }
            return query;
        }

        //Fix for Bug# 17439
        private string ProcessClarifications(string previousSearchKeyword)
        {
            string previousSearchQuery =
                stateItems.GetStateItemValue<string>(SearchSessionKeys.SearchQuery, SearchSessionKeys.PreviousSearch,
                                          StateItemStore.AspSession);

            NameValueCollection previousSearchQueryCollection = ConvertStringToNameValueCollection(previousSearchQuery);
            if ((previousSearchQueryCollection[SearchFilterKeys.Keywords] == previousSearchKeyword) && (!string.IsNullOrEmpty(previousSearchQueryCollection[SearchFilterKeys.ClarificationSelection])))
            {
                return
                    (string.Concat(SearchFilterKeys.ClarificationSelection, "=",
                                   previousSearchQueryCollection[SearchFilterKeys.ClarificationSelection], "&"));
            }
            return null;
        }

        private string BuildSearchQueryFromSearchView()
        {
            string query = string.Empty;

            if (view is ISearchView)
            {
                ISearchView searchView = (ISearchView) view;

                bool bool1 = searchView.DaysChecked;
                bool bool2 = searchView.DateRangeChecked;


                if (!String.IsNullOrEmpty(searchView.KeywordSearch))
                {
                    query += string.Concat(SearchFilterKeys.Keywords, "=", HttpUtility.UrlEncode(searchView.KeywordSearch), "&");
                    query += ProcessClarifications(HttpUtility.UrlEncode(searchView.KeywordSearch));
                }
                
               
              
                if (!searchView.RemoveMoreSearchOptions)
                {
                    if (!String.IsNullOrEmpty(searchView.DateCreated))
                    {
                        query += string.Concat(SearchFilterKeys.DatePhotographedCreated, "=", HttpUtility.UrlEncode(searchView.DateCreated), "&");
                    }

                    if (searchView.DaysChecked && !String.IsNullOrEmpty(searchView.Days))
                    {
                        query += string.Concat(SearchFilterKeys.ImageMadeAvailableDays, "=", HttpUtility.UrlEncode(searchView.Days), "&");
                    }

                    List<string> dateList = GetDatesFilterList(searchView);
                    if (searchView.DateRangeChecked && dateList != null && dateList.Count > 0)
                    {
                        query += string.Concat(SearchFilterKeys.ImageMadeAvailableDate, "=", HttpUtility.UrlEncode(HttpUtility.HtmlDecode(dateList[0])), ",", HttpUtility.UrlEncode(HttpUtility.HtmlDecode(dateList[1])), "&");
                    }

                    if (!String.IsNullOrEmpty(searchView.Location))
                    {
                        query += string.Concat(SearchFilterKeys.Location, "=", HttpUtility.UrlEncode(HttpUtility.HtmlDecode(searchView.Location)), "&");
                    }

                    if (!String.IsNullOrEmpty(searchView.Photographer))
                    {
                        query += string.Concat(SearchFilterKeys.Photographer, "=", HttpUtility.UrlEncode(HttpUtility.HtmlDecode(searchView.Photographer)), "&");
                    }

                    if (!String.IsNullOrEmpty(searchView.Provider))
                    {
                        query += string.Concat(SearchFilterKeys.Provider, "=", HttpUtility.UrlEncode(HttpUtility.HtmlDecode(searchView.Provider)), "&");
                    }

                    string query1 = GetFilterQueryValue<Orientation>(GetOrientationFilterList(searchView));
                    if (!string.IsNullOrEmpty(query1))
                    {
                        query += string.Concat(SearchFilterKeys.Orientation, "=", query1, "&");
                    }

                    //todo: bug # 16534 Change the next line to us the enum value instead of "0".
                    if (searchView.PointOfView != "0")
                    {
                        query += string.Concat(SearchFilterKeys.PointofView, "=", searchView.PointOfView, "&");
                    }

                    if (searchView.NumberOfPeople != ((int)NumberOfPeople.WithPeople).ToString())
                    {
                        if (postSearchView == null || !postSearchView.NoPeople)
                        {
                            query += string.Concat(SearchFilterKeys.NumberOfPeople, "=", searchView.NumberOfPeople, "&");
                        }
                    }

                    //todo: bug # 16534 Change the next line to us the enum value instead of "1".
                    if (searchView.ImmediateAvailability != "1")
                    {
                        query += string.Concat(SearchFilterKeys.ImmediateAvailability, "=", searchView.ImmediateAvailability, "&");
                    }

                    if (!String.IsNullOrEmpty(searchView.ImageNumbers))
                    {
                        Regex rx = new Regex(@"[^A-Za-z0-9\-]+");
                        string imageNumners = rx.Replace(searchView.ImageNumbers, " ");

                        query += string.Concat(SearchFilterKeys.ImageNumbers, "=", HttpUtility.UrlEncode(HttpUtility.HtmlDecode(searchView.ImageNumbers)), "&");
                    }

                    List<string> seletedMarketingList = GetSelectedMarketingCollection(searchView);
                    if (seletedMarketingList != null && seletedMarketingList.Count > 0)
                    {
                        string[] marketingCollectionQuery = GetSelectedMarketingCollection(searchView).ToArray();
                        string queryValue = string.Join(",", marketingCollectionQuery);
                        query += string.Concat(SearchFilterKeys.MarketingCollection, "=", queryValue, "&");
                    }
                }

            }

            return query;
        }

        public string LoadDefaultSearchFilters()
        {
            return stateItems.GetStateItemValue<string>(SearchKeys.Name, SearchKeys.DefaultSearchCookieKey, StateItemStore.Cookie);
        }

        public void SaveDefaultSearchFilters(NameValueCollection queryString)
        {
            ISearchBaseView baseView = postSearchView ?? view as ISearchBaseView;

            if (baseView != null)
            {
                string defaultQuery = string.Empty;

                string filterValueString = queryString[SearchFilterKeys.Category];// GetFilterQueryValue<Category>(GetCategoryFilterList(baseView));
                if (!string.IsNullOrEmpty(filterValueString))
                {
                    defaultQuery += string.Concat(SearchFilterKeys.Category, "=", filterValueString, "&");
                }

                filterValueString = queryString[SearchFilterKeys.LicenseModel];// GetFilterQueryValue<LicenseModel>(GetLicenseModelFilterList(baseView));
                if (!string.IsNullOrEmpty(filterValueString))
                {
                    defaultQuery += string.Concat(SearchFilterKeys.LicenseModel, "=", filterValueString, "&");
                }

                stateItems.SetStateItem<string>(new StateItem<string>(
                                                    SearchKeys.Name,
                                                    SearchKeys.DefaultSearchCookieKey,
                                                    defaultQuery.TrimEnd('&'),
                                                    StateItemStore.Cookie,
                                                    StatePersistenceDuration.NeverExpire));
            }
        }

        public void PopulateMSODatesAvailable()
        {
            ISearchView searchView = view as ISearchView;

            searchView.BeginDate = DateTime.Now.AddYears(-1).ToShortDateString();
            searchView.EndDate = DateTime.Now.ToShortDateString();

        }

        //public void SetSearchSortOptions()
        //{
        //    ISearchView searchView = view as ISearchView;
        //    if(searchView != null)
        //    {
        //        searchView.SearchSortOption = 
        //    }
        //}

        /// <summary>
        /// Gets ClarificationsQueryFlags when SharedFilters are Changed
        /// </summary>
        /// <param name="previousSearchQuery"></param>
        /// <param name="currentSearcyQuery"></param>
        private string GetClarificationsQueryFlagsForChangedSharedFilters(string previousSearchQuery, string currentSearcyQuery)
        {
            NameValueCollection previousQueryCollection = new NameValueCollection();
            NameValueCollection currentQueryCollection = new NameValueCollection();
            string clarificationQueryFlags = string.Empty;

            // Check if both previousSearchQuery string and CurrentSearchQuery string are not empty.
            if (!string.IsNullOrEmpty(previousSearchQuery) && !string.IsNullOrEmpty(currentSearcyQuery))
            {
                previousQueryCollection = ConvertSearchQueryToNameValueCollection(previousSearchQuery);
                currentQueryCollection = ConvertSearchQueryToNameValueCollection(currentSearcyQuery);
                // Check if there is a ClarificationsQueryFlags in the Previous Search Query String
                // Check if both Previous and Current Search Keywords are the same.
                // Check if only the Shared Filters are changed.
                if (HasClarificationsQueryFlags(previousQueryCollection)
                    && KeywordsAreIdentical(previousQueryCollection, currentQueryCollection)
                    && AnySharedFiltersChanged(previousQueryCollection, currentQueryCollection)
                    )
                {
                    // Get ClarificationQueryFlags from Previous Query String.
                    clarificationQueryFlags = GetClarificationsQueryStringFromQuery(previousQueryCollection);
                }
            }
            return clarificationQueryFlags;
        }

        /// <summary>
        /// Checks if Any Shared Filters are Changed For the Same SearchKeywords Which was 
        /// Clarified that has Clarifications 
        /// </summary>
        /// <returns></returns>
        private bool AreSharedFiltersChangedWithClarification()
        {
            bool sharedFiltersChangedWithClarification = false;
            string currentSearchQuery = BuildSearchQueryOnly();
            string previousSearchQuery =
                stateItems.GetStateItemValue<string>(SearchSessionKeys.SearchQuery, SearchSessionKeys.PreviousSearch,
                              StateItemStore.AspSession);
                       NameValueCollection previousQueryCollection = new NameValueCollection();
            NameValueCollection currentQueryCollection = new NameValueCollection();

            // Check if both previousSearchQuery string and CurrentSearchQuery string are not empty.
            if (!string.IsNullOrEmpty(previousSearchQuery) && !string.IsNullOrEmpty(currentSearchQuery))
            {
                previousQueryCollection = ConvertSearchQueryToNameValueCollection(previousSearchQuery);
                currentQueryCollection = ConvertSearchQueryToNameValueCollection(currentSearchQuery);
                if (AnySharedFiltersChanged(previousQueryCollection, currentQueryCollection) &&
                    HasClarificationsQueryFlags(previousQueryCollection) && 
                    KeywordsAreIdentical(previousQueryCollection, currentQueryCollection)
                    )
                {
                    sharedFiltersChangedWithClarification = true;
                }
            }
            return sharedFiltersChangedWithClarification;
        }

        /// <summary>
        /// Builds the Query String when Shared Filters are changed after Clarification.
        /// </summary>
        /// <param name="currentSearchQuery">Current Search Query</param>
        /// <returns></returns>
        private string BuildQueryWithClarificationsQueryFlagsForChangedSharedFilters(string currentSearchQuery)
        {
            // Get Previous Search Query
            string previousSearchQuery =
                stateItems.GetStateItemValue<string>(SearchSessionKeys.SearchQuery, SearchSessionKeys.PreviousSearch,
                                          StateItemStore.AspSession);
            // Compare currentSearchquery and previousSearchQuery
            // if there is no cl-Clarification term abort the compare.
            // 1) remove the cl-Clarification term and then compare
            // 2) if there is cl-Clarification term and only shared Filters are changed
            // then append cl-Clarification and send it back.
            string queryForClarifications = GetClarificationsQueryFlagsForChangedSharedFilters(previousSearchQuery, currentSearchQuery);
            string query = currentSearchQuery + queryForClarifications;
            return query;
        }

        /// <summary>
        /// Retrieves ClarificationQueryFlags from Query String
        /// </summary>
        /// <param name="previousQueryCollection">Previous Search Query String Key/Value Collection</param>
        /// <returns></returns>
        private string GetClarificationsQueryStringFromQuery(NameValueCollection previousQueryCollection)
        {
            string clarificationsQueryFlags = string.Empty;
            string clarificationValue = previousQueryCollection[SearchFilterKeys.ClarificationSelection];
            clarificationsQueryFlags = string.Concat("&", SearchFilterKeys.ClarificationSelection, "=", clarificationValue);
            return clarificationsQueryFlags;
        }

        /// <summary>
        /// Checks if any Shared Filters are changed.
        /// </summary>
        /// <param name="previousQueryCollection">Previous Search Query String Key/Value Collection</param>
        /// <param name="currentQueryCollection">Current Search Query String Key/Value Collection</param>
        /// <returns></returns>
        private bool AnySharedFiltersChanged(NameValueCollection previousQueryCollection, NameValueCollection currentQueryCollection)
        {
            bool sharedFilterChanged = false;
            
            //// Category
            if (!(previousQueryCollection[SearchFilterKeys.Category] == currentQueryCollection[SearchFilterKeys.Category]))
            {
                sharedFilterChanged = true;
            }

            //// LicenseModels
            //SearchFilterKeys.LicenseModel;
            if (!sharedFilterChanged  && !(previousQueryCollection[SearchFilterKeys.LicenseModel] == currentQueryCollection[SearchFilterKeys.LicenseModel]))
            {
                sharedFilterChanged = true;
            }
            //// MediaType
            //SearchFilterKeys.MediaType;
            if (!sharedFilterChanged && !(previousQueryCollection[SearchFilterKeys.MediaType] == currentQueryCollection[SearchFilterKeys.MediaType]))
            {
                sharedFilterChanged = true;
            }
            ////ColorFormats
            //SearchFilterKeys.ColorFormat;
            if (!sharedFilterChanged && !(previousQueryCollection[SearchFilterKeys.ColorFormat] == currentQueryCollection[SearchFilterKeys.ColorFormat]))
            {
                sharedFilterChanged = true;
            }
            //// NumberOfPeople
            //SearchFilterKeys.NumberOfPeople;
            if (!sharedFilterChanged && !(previousQueryCollection[SearchFilterKeys.NumberOfPeople] == currentQueryCollection[SearchFilterKeys.NumberOfPeople]))
            {
                sharedFilterChanged = true;
            }
            //// Model Released
            //SearchFilterKeys.ModelReleased;
            if (!sharedFilterChanged && !(previousQueryCollection[SearchFilterKeys.ModelReleased] == currentQueryCollection[SearchFilterKeys.ModelReleased]))
            {
                sharedFilterChanged = true;
            }

            return sharedFilterChanged;
        }

        /// <summary>
        /// Checks if Search Keywords are identical between previous Post(Page Submit) and Current Post(Page Submit)
        /// </summary>
        /// <param name="previousSearchQueryCollection">Previous Search Query String Key/Value Collection</param>
        /// <param name="currentSearchQueryCollection">Current Search Query String Key/Value Collection</param>
        /// <returns></returns>
        private bool KeywordsAreIdentical(NameValueCollection previousSearchQueryCollection, NameValueCollection currentSearchQueryCollection)
        {
            bool areKeywordsSame = false;
            string previousKeyword = GetSearchKeywordFromQueryString(previousSearchQueryCollection);
            string currentKeyword = GetSearchKeywordFromQueryString(currentSearchQueryCollection);
            if (previousKeyword == currentKeyword)
            {
                areKeywordsSame = true;
            }
            return areKeywordsSame;
        }

        /// <summary>
        /// Retrives the SearchKeyword from Query string
        /// </summary>
        /// <param name="queryCollection">Query String</param>
        /// <returns></returns>
        private string GetSearchKeywordFromQueryString(NameValueCollection queryCollection)
        {
            string searchKeyword = string.Empty;
            foreach (string key in queryCollection.Keys)
            {
                if (key == SearchFilterKeys.Keywords)
                {
                    searchKeyword = queryCollection[SearchFilterKeys.Keywords];
                    break;
                }
            }
            return searchKeyword;
        }

        /// <summary>
        /// Checks if ClarificationQueryFlags are present in a Query String
        /// </summary>
        /// <param name="previousQueryCollection">Query String</param>
        /// <returns></returns>
        private static bool HasClarificationsQueryFlags(NameValueCollection queryCollection)
        {
            bool hasClarificationsQueryFlags = false;
            foreach (string key in queryCollection.Keys)
            {
                switch (key)
                {
                    case SearchFilterKeys.ClarificationSelection:
                        hasClarificationsQueryFlags = true;
                        break;
                    default:
                        hasClarificationsQueryFlags = false;
                        break;
                }
                if (hasClarificationsQueryFlags)
                {
                    break;
                }
            }
            return hasClarificationsQueryFlags;
        }

        private static NameValueCollection ConvertSearchQueryToNameValueCollection(string searchQuery)
        {
            NameValueCollection queryCollection = new NameValueCollection();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                foreach (string param in searchQuery.Split('&'))
                {
                    if (!string.IsNullOrEmpty(param) && param.IndexOf('=') > 0)
                    {
                        string key = param.Substring(0, param.IndexOf("="));
                        string value = param.Substring(param.IndexOf("=") + 1);
                        queryCollection.Add(key, value);
                    }
                }
            }
            return queryCollection;
        }

        private static Dictionary<string, QuickPicItem> GetImagesInQuickPick()
        {
            Dictionary<string, QuickPicItem> imageCollection = null;
            QuickPicPresenter quickPicPresenter = new QuickPicPresenter();


            List<QuickPicItem> imagesInQuickPick = quickPicPresenter.QuickPicList;

            if (imagesInQuickPick != null)
            {
                imageCollection = new Dictionary<string, QuickPicItem>();
                foreach (QuickPicItem item in imagesInQuickPick)
                {
                    imageCollection.Add(item.CorbisID, item);
                }
            }
            return imageCollection;
        }

        private List<SearchResultProduct> ConvertSearchResults(
            ImageSearchResults results,
            Dictionary<string, QuickPicItem> quickPickImages, 
            int lightboxId)
        {
            return results.SearchDisplayImages.ConvertAll<SearchResultProduct>(
                new Converter<SearchDisplayImage, SearchResultProduct>(
                    delegate(SearchDisplayImage displayImage)
                        {
                            return new SearchResultProduct(
                                displayImage,
                                Profile.IsQuickPicEnabled ? Profile.QuickPicType : QuickPicFlags.None,
                                Profile.Permissions,
                                Profile.IsFastLaneEnabled,
                                lightboxId,
                                CheckCorbisIdInQuickPickList(displayImage.CorbisId, quickPickImages),
                                false
                                );
                        }));
        }

        /// <summary>
        /// Adds an offering to cart
        /// </summary>
        /// <param name="offeringUid"></param>
        //todo: bug # 16537 Remove offeringUid as an input parameter to this method.  Get the value off of the view.
        //todo: bug # 16537 Remove the return value from the following method.  Set a value on the view.
        public bool AddToCart(Guid offeringUid)
        {
            try
            {
                if (!Profile.IsAnonymous)
                {
                    //TODO:- have AddOfferingToCart to return an Int for CartItemCount instead of void.
                    this.LightboxCartAgent.AddOfferingToCart(Profile.MemberUid, offeringUid,
                                                             Language.CurrentLanguage.LanguageCode,
                                                             Profile.CountryCode);
                    Profile.CartItemsCount = this.LightboxCartAgent.GetCartCount(Profile.MemberUid);
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                HandleException(e, view.LoggingContext, "AddItemToCart");
                throw;
            }
        }

        public bool AddToCart(Guid offeringUid, Guid productUid)
        {
            bool result = false;
            try
            {
                if (!Profile.IsAnonymous)
                {
                    FolderProduct product = this.LightboxCartAgent.GetProductByProductUid(
                        Profile.UserName,
                        productUid,
                        Profile.CountryCode,
                        Language.CurrentLanguage.LanguageCode,
                        true,
                        false);

                    if (product.Usage == null || product.Usage.UseCategoryUid == Guid.Empty ||
                        product.Usage.AttributeValuePairs == null || product.Usage.AttributeValuePairs.Count == 0)
                    {
                        // if there is no usage add offering to cart.
                        result = AddToCart(offeringUid);
                    }
                    else
                    {
                        List<string> errors = this.LightboxCartAgent.UpdateCartProducts(
                            Profile.UserName,
                            new List<string>(new string[] { product.CorbisID }),
                            product.Usage,
                            Profile.CountryCode,
                            Language.CurrentLanguage.LanguageCode);
                        if (errors == null || errors.Count == 0)
                        {
                            Profile.CartItemsCount = this.LightboxCartAgent.GetCartCount(Profile.MemberUid);
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                HandleException(e, view.LoggingContext, "AddItemToCart");
                throw;
            }
        }

        /// <summary>
        /// Adds an offering to cart
        /// </summary>
        /// <param name="offeringUid"></param>
        //todo: bug # 16539 Remove the input parameters from this method.  Get the values off the view.
        public void DeleteImageFromLightbox(int lightboxId, Guid productUid)
        {
            try
            {
                LightboxCartAgent.DeleteProductUidFromLightBox(lightboxId, productUid, Profile.UserName);
            }
            catch (Exception e)
            {
                HandleException(e, view.LoggingContext, "DeleteImageFromLightbox");
                throw;
            }
        }

        /// <summary>
        /// Shows or hides Search Buddy tabs based on business logic. 
        /// Only Quickpic is currently implemented
        /// </summary>
        public void SetSearchBuddyTabs()
        {
            if (postSearchView != null)
            {
                postSearchView.ShowQuickPicTab = Profile.IsQuickPicEnabled;
                postSearchView.AdjustStatusForUser();
            }
        }

        //todo: bug # 16540 Remove the input parameters from the following method.  Get the values from the view.
        //todo: bug # 16541 Remove some of the nesting in the following method
        public void GetImageSearchResults(NameValueCollection queryString, int itemsPerPage, int pageNumber)
        {
            if (postSearchView != null)
            {
                int startPosition = (itemsPerPage*(pageNumber - 1)) + 1;
                string currentKeyword = GetSearchKeywordFromQueryString(queryString);
                ClientSearchResults searchResult =
                    Search(queryString, itemsPerPage, startPosition, Profile.UserName, Profile.CountryCode,
                                Language.CurrentLanguage.LanguageCode);
                
                if (searchResult != null)
                {

                    postSearchView.SearchResultProducts = searchResult.SearchResultProducts;
                    
                    if ((searchResult.SearchResultProducts != null) && (searchResult.SearchResultProducts.Count != 0))
                    {
                        postSearchView.TotalSearchHitCount = searchResult.TotalRecords;
                        postSearchView.CurrentPageNumber = pageNumber;
                        postSearchView.CurrentPageHitCount = searchResult.SearchResultProducts.Count;
                    }

                    if (searchResult.Clarifications != null && searchResult.Clarifications.Count > 0)
                    {
                        SetClarificationsToSession(searchResult.Clarifications,currentKeyword);
                        postSearchView.Clarifications = searchResult.Clarifications;
                        if (postSearchView.ShowClarification && !HasOnlyCurrentEventsSelected() && (pageNumber == 0 || pageNumber == 1))
                        {
                            // Check if NONE of the Clarifications are selected.
                            // Dont show the popup if the user cancels the popup
                            // ie., the user has not selected any clarifications when the popup was shown earlier.
                            if (HasClarificationAndNoneSelected(queryString))
                            {
                                postSearchView.ShowClarificationPopup = false;
                            }
                            else
                            {
                                postSearchView.ShowClarificationPopup = true;
                            }
                        }
                        else
                        {
                            postSearchView.ShowClarificationPopup = false;
                        }
                    }
                    else
                    {
                        postSearchView.ShowClarificationPopup = false;
                    }

                    if(!string.IsNullOrEmpty(queryString[SearchFilterKeys.ClarificationSelection]))
                    {
                        string clarificationOptions = queryString[SearchFilterKeys.ClarificationSelection];
                        string keywordFilter = queryString[SearchFilterKeys.Keywords];
                        List<Clarification> clarificationsFromSession = new List<Clarification>();

                        if (!string.IsNullOrEmpty(clarificationOptions))
                        {
                            clarificationsFromSession = GetClarificationsFromSession();
                            if (clarificationsFromSession == null || clarificationsFromSession.Count == 0)
                            {
                                // Get Previous Keyword with Clarification and its Clarifications
                                string previousClarifiedKeyword = GetPreviousClarifiedKeywordFromSession();
                                List<Clarification> previousClarifications = GetPreviousClarificationsFromSession();
                                if (previousClarifiedKeyword == currentKeyword)
                                {
                                    clarificationsFromSession = previousClarifications;
                                }
                            }
                        }

                        List<Clarification> selectedClarifications = GetSelectedClarifications(clarificationsFromSession, clarificationOptions);

                        if (postSearchView != null)
                        {
                            string title = GetClarificationSearchTitle(selectedClarifications, new SearchTextFilter(keywordFilter) , clarificationOptions);
                            if (!string.IsNullOrEmpty(title))
                            {
                                postSearchView.SearchResultTitle = title;
                            }
                        }

                    }
                }
                if(!string.IsNullOrEmpty(queryString.Get("ri")))
                {
                    string recentImageId = queryString.Get("ri");
                    if(!string.IsNullOrEmpty(recentImageId))
                    {
                        DisplayImage image = ImageAgent.GetDisplayImage(recentImageId,
                                               Language.CurrentLanguage.LanguageCode,
                                               Profile.IsAnonymous);
                        postSearchView.RecentImageId = recentImageId;
                        postSearchView.RecentImageURL = image.Url128;
                        postSearchView.RecentImageRadio = image.AspectRatio;
                    }
                    else
                    {
                        postSearchView.RecentImageId = string.Empty;
                        postSearchView.RecentImageURL = string.Empty;
                    }
                }

                if(!string.IsNullOrEmpty(queryString.Get(SearchFilterKeys.Sort)))
                {
                    string searchSortOptin = queryString.Get(SearchFilterKeys.Sort);
                    int searchSort;
                    SearchSort searchSortEnum;
                    if(int.TryParse(searchSortOptin, out searchSort))
                    {
                        searchSortEnum =
                            (SearchSort)
                            (Enum.IsDefined(typeof (SearchSort), searchSort)
                                 ? Enum.Parse(typeof (SearchSort), searchSortOptin)
                                 : SearchSort.Relevancy);
                    }
                    else
                    {
                        searchSortEnum = SearchSort.Relevancy;
                    }
                    
                    postSearchView.SearchSortOption = searchSortEnum;
                }
            }
        }

        private bool HasClarificationAndNoneSelected(NameValueCollection queryString)
        {
            string clarificationValue = string.Empty;
            bool hasClarificationAndNoneSelected = false;
            if (HasClarificationsQueryFlags(queryString))
            {
                clarificationValue = queryString[SearchFilterKeys.ClarificationSelection];
                hasClarificationAndNoneSelected = !AtleastOneClarificationSelected(clarificationValue);
            }
            return hasClarificationAndNoneSelected;
        }

        private void SetClarificationsToSession(List<Clarification> clarifications, string currentKeyword)
        {
            // Save the Current KeywordWithClarification and Clarifications into Session
            stateItems.SetStateItem(new StateItem<string>(SearchKeys.Name, SearchKeys.KeywordWithClarifications, currentKeyword, StateItemStore.AspSession));
            stateItems.SetStateItem(new StateItem<List<Clarification>>(SearchKeys.Name, SearchKeys.Clarifications, clarifications, StateItemStore.AspSession));
        }

        public void PopulateSearchFlyoutAndSearchBuddyFilterState()
        {
            string defaultQuery = LoadDefaultSearchFilters();

            if (!string.IsNullOrEmpty(defaultQuery))
            {
                NameValueCollection queryString = ConvertSearchQueryToNameValueCollection(defaultQuery);
                PopulateSearchFlyoutAndSearchBuddyFilterState(queryString);
            } 
        }

        private void SetControlStateInMSO(MoreSearchOptions moreSearchOptions)
        {
            ISearchView searchView = view as ISearchView;
            
            if (moreSearchOptions != null)
            {
                if (!string.IsNullOrEmpty(moreSearchOptions.DateCreated))
                {
                    SetDateCreatedFilter(moreSearchOptions.DateCreated, searchView);
                }
                if (!string.IsNullOrEmpty(moreSearchOptions.Days))
                {
                    SetImageMadeAvailableFilter(moreSearchOptions.Days, searchView);
                }
                if (!string.IsNullOrEmpty(moreSearchOptions.BeginDate))
                {
                    SetDateRangeFilter(moreSearchOptions, searchView);
                }
                if (!string.IsNullOrEmpty(moreSearchOptions.Location))
                {
                    SetLocationFilter(moreSearchOptions.Location, searchView);
                }
                if (!string.IsNullOrEmpty(moreSearchOptions.Photographer))
                {
                    SetPhotographerFilter(moreSearchOptions.Photographer, searchView);
                }
                if (!string.IsNullOrEmpty(moreSearchOptions.Provider))
                {
                    SetProviderFilter(moreSearchOptions.Provider, searchView);
                }
                SetOrientationFilter(moreSearchOptions, searchView);
                if (!string.IsNullOrEmpty(moreSearchOptions.PointOfView))
                {
                    SetPointOfViewFilter(moreSearchOptions.PointOfView, searchView);
                }
                if (!string.IsNullOrEmpty(moreSearchOptions.ImmediateAvailability))
                {
                    SetImmediateAvailabilityFilter(moreSearchOptions.ImmediateAvailability, searchView);
                }
                if (!string.IsNullOrEmpty(moreSearchOptions.ImageNumbers))
                {
                    SetImageNumbersFilter(moreSearchOptions.ImageNumbers, searchView);
                }
                if (moreSearchOptions.SelectedMarketingCollection != null && moreSearchOptions.SelectedMarketingCollection.Count > 0)
                {
                    SetMarketingCollectionFilter(moreSearchOptions.SelectedMarketingCollection, searchView);
                }
            }
        }

        //todo: bug # 16542 Remove the input parameter from the following method.  Retrieve the value from the View.
        //todo: bug # 16542 Rename the following method, since it sets MSO as well
        public void PopulateSearchFlyoutAndSearchBuddyFilterState(NameValueCollection queryString)
        {
            ISearchBaseView baseView = postSearchView ?? view as ISearchBaseView;
            ISearchView searchView = view as ISearchView;

            if (baseView != null && searchView != null)
            {
                foreach (string key in queryString.Keys)
                {
                    switch (key)
                    {
                            // Filters shared between views
                        case SearchFilterKeys.Category:
                            SetCategoryFilters(ConvertFilterValuesToList<Category>(queryString[key]), baseView);
                            break;
                        case SearchFilterKeys.LicenseModel:
                            SetLicenseModelFilters(ConvertFilterValuesToList<LicenseModel>(queryString[key]), baseView);
                            break;
                        case SearchFilterKeys.ColorFormat:
                            SetColorFormatFilters(ConvertFilterValuesToList<ColorFormat>(queryString[key]), baseView);
                            break;
                        case SearchFilterKeys.MediaType:
                            SetMediaTypeFilters(ConvertFilterValuesToList<MediaType>(queryString[key]), baseView);
                            break;
                        case SearchFilterKeys.NumberOfPeople:
                            if (queryString[key] == ((int)NumberOfPeople.WithoutPeople).ToString())
                            {
                                SetNumberOfPeopleFilters(ConvertFilterValuesToList<NumberOfPeople>(queryString[key]), baseView);
                            }
                            else
                            {
                                SetNumberOfPeopleFilters(ConvertFilterValuesToList<NumberOfPeople>(queryString[key]), searchView);
                            }
                            break;
                        case SearchFilterKeys.ModelReleased:
                            SetModelReleasedFilter(queryString[key], baseView);
                            break;


                        case SearchFilterKeys.Sort:
                            SearchSort sortOption;
                            int sortOrdinal;
                            int.TryParse(queryString[key], out sortOrdinal);
                            sortOption = Enum.IsDefined(typeof(SearchSort), sortOrdinal)? (SearchSort)sortOrdinal : SearchSort.Relevancy;
                            SetSearchSortOption(sortOption, baseView);
                            break;
                        case SearchFilterKeys.Photographer:
                            if(null != postSearchView)
                            {
                                postSearchView.SearchResultPhotographerName = queryString[key];
                            }
                            break;
                        case SearchFilterKeys.Location:
                            if(null != postSearchView)
                            {
                                postSearchView.SearchResultLocation = queryString[key];
                            }
                            break;
                        // Search View Filters
                        case SearchFilterKeys.Keywords:
                            SetKeywordsFilter(queryString[key], searchView);
                            if (postSearchView != null)
                            {
                                postSearchView.SearchResultTitle = queryString[key];
                            }
                            break;

                    }
                }
                SetControlStateInMSO(ConvertQuerystringToMSO(queryString));
            }
        }

        public void LoadSearchPreferences()
        {
            List<string> propertiesNotFound = stateItems.PopulateObjectFromState(view);
            ResetSearchBuddyFilter(view as ISearchBaseView, postSearchView as ISearchBaseView);
        }

        public void SaveSearchPreferences()
        {
            ISearchView searchView = view as ISearchView;
            ResetSearchBuddyFilter(postSearchView as ISearchBaseView, searchView);
            stateItems.SaveObjectToState(view);
            if (searchView != null)
            {
                stateItems.SetStateItem(new StateItem<MoreSearchOptions>(SearchKeys.Name, SearchKeys.MoreSearchOptions, searchView.MoreSearchOptionsSettings, StateItemStore.AspSession));
            }
        }

        public void DeleteMoreSearchOptions()
        {
            stateItems.DeleteStateItem<MoreSearchOptions>(new StateItem<MoreSearchOptions>(SearchKeys.Name, SearchKeys.MoreSearchOptions, null, StateItemStore.AspSession));

            stateItems.DeleteStateItem<string>(new StateItem<string>(SearchSessionKeys.SearchQuery, SearchSessionKeys.PreviousSearch, null, StateItemStore.AspSession));
        
        }

        private MoreSearchOptions GetMoreSearchOptionsFromSession()
        {
            MoreSearchOptions msoString = stateItems.GetStateItemValue<MoreSearchOptions>(SearchKeys.Name, SearchKeys.MoreSearchOptions, StateItemStore.AspSession);
            return msoString;
        }

        public void PopulateMoreSearchOptionsCollectionsControls()
        {
            ((ISearchView) view).MarketingCollection =
                MarketingCollectionAgent.GatherMarketingCollectionsByDisplayGroup(
                    Language.CurrentLanguage.LanguageCode);
        }

        //todo: bug # 16543 Remove the input parameter from the following method.  Retrieve the value from the View.
        //todo: bug # 16543 Remove the return value from the following method.  Set a property on the View instead.
        public bool HasDirectlyManipulatedKeyword(NameValueCollection queryString, bool isEncoded)
        {
            bool hasDirectlyManipulatedKeyword = false;
            string directlyManipulatedKeyword = HttpUtility.UrlDecode(GetDirecltyManipulatedKeyword());
            string keywordFromQueryString = isEncoded == true ? HttpUtility.UrlDecode(GetSearchKeywordFromQueryString(queryString)) : GetSearchKeywordFromQueryString(queryString);
            if (directlyManipulatedKeyword == keywordFromQueryString)
            {
                hasDirectlyManipulatedKeyword = true;
            }
            return hasDirectlyManipulatedKeyword;
        }
        /// <summary>
        /// Returns true if atleast one Clarification is selected, returns False if None is Selected,
        /// For a keyword/s with Clarifications.
        /// </summary>
        /// <param name="clarificationsQueryFlags">Selected Clarifications Flags</param>
        /// <returns></returns>
        //todo: bug # 16544 Remove the input parameter from the following method.  Retrieve the value from the View.
        //todo: bug # 16544 Remove the return value from the following method.  Set a property on the View instead.
        public bool AtleastOneClarificationSelected(string clarificationsQueryFlags)
        {
            bool atleastOneClarificationSelected = false;
            if (!string.IsNullOrEmpty(clarificationsQueryFlags))
            {
                clarificationsQueryFlags = clarificationsQueryFlags.Replace("0", "");
                if (!string.IsNullOrEmpty(clarificationsQueryFlags.TrimEnd(new char[] { '0' }).Trim()))
                {
                    atleastOneClarificationSelected = true;
                }
            }
            return atleastOneClarificationSelected;
        }

        private static void ResetSearchBuddyFilter(ISearchBaseView readView, ISearchBaseView writeView)
        {
            if (readView != null && writeView != null)
            {
                StateItemCollection stateItemsCollection = new StateItemCollection(System.Web.HttpContext.Current);
                string searchString = stateItemsCollection.GetStateItemValue<string>(SearchSessionKeys.DirectlyManipulatedSearch, null,
                                                               StateItemStore.Cookie);
                if(!string.IsNullOrEmpty(searchString))
                {
                    NameValueCollection searchFilters = ConvertSearchQueryToNameValueCollection(searchString);
                    string licenseModel = searchFilters[SearchFilterKeys.LicenseModel];
                    if(!string.IsNullOrEmpty(licenseModel))
                    {
                        List<LicenseModel> licenseModelFilters = ConvertFilterValuesToList<LicenseModel>(licenseModel);
                        SetLicenseModelFilters(licenseModelFilters, writeView);
                    }
                    string categoryString = searchFilters[SearchFilterKeys.Category];
                    if (!string.IsNullOrEmpty(categoryString))
                    {
                        List<Category> categoriesList = ConvertFilterValuesToList<Category>(categoryString);
                        SetCategoryFilters(categoriesList, writeView);
                    }
                }
            }
        }

        #region Lightbox Operations

        //todo: change the following method to Private
        public List<Lightbox> GetLightboxList(string userName)
        {
            try
            {
                string lightboxSortType = string.Empty;
                if (
                    stateItems.GetStateItemValue<string>(LightboxCartKeys.Name, LightboxCartKeys.SortTypeKey,
                                                         StateItemStore.Cookie) == null)
                {
                    lightboxSortType = "date";
                }
                else
                {
                    lightboxSortType =
                        stateItems.GetStateItemValue<string>(LightboxCartKeys.Name, LightboxCartKeys.SortTypeKey,
                                                             StateItemStore.Cookie).ToString();
                }

                if (lightboxSortType != "name")
                {
                    return LightboxCartAgent.GetLightboxTreeFlattenedByUserName(userName, LightboxTreeSort.Date, false);
                }
                else
                {
                    return LightboxCartAgent.GetLightboxTreeByUserName(userName, LightboxTreeSort.Name);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, view.LoggingContext,
                                string.Format(
                                    "LightboxesPresenter: GetLightboxTreeByFlattenedByUsername() - Error getting lightbox tree for member '{0}'.",
                                    userName));
            }

            return null;
        }

        public void LoadLightBoxData()
        {
            if (postSearchView != null)
            {
                if (Profile.IsAnonymous)
                {
                    postSearchView.ShowAddToLightboxPopup = false;
                }
                else
                {
                    List<Lightbox> lightboxes = GetLightboxList(Profile.UserName);
                    if (lightboxes != null && lightboxes.Count > 0)
                    {
                        postSearchView.LightboxList = lightboxes;
                    }
                }
            }
        }

        //todo: bug # 16604 Remove the input parameter from the following method.  Retrieve the value from the View.
        //todo: bug # 16604 Remove the return value from the following method.  Set a property on the View instead.
        public List<LightboxDisplayImage> LoadLightboxDetails(int lightboxId, bool isQuickpickSort, int maxImageCount, out bool isOverMax)
        {
            isOverMax = false;
            List<LightboxDisplayImage> displayImages = LightboxCartAgent.GetLightboxProductDetails(lightboxId, Language.CurrentLanguage.LanguageCode, 
                Profile.CountryCode, 1, maxImageCount, isQuickpickSort, Profile.UserName);
            
            int totalImageCount = LightboxCartAgent.GetLightboxProductCount(lightboxId);
            if (maxImageCount < totalImageCount)
            {
                isOverMax = true;
            }

            return displayImages;
        }

        //todo: bug # 16605 Remove the return value from the following method.  Set a property on the View instead.
        public List<CartDisplayImage> GetCartItems()
        {
            if(!Profile.IsAnonymous)
            {
                return LightboxCartAgent.GetCartContent(
                Profile.UserName,
                Language.CurrentLanguage.LanguageCode,
                Profile.CountryCode);
            }
            return  new List<CartDisplayImage>();
        }
        
        #endregion

        #region Search Filters

        //todo: bug # 16609 Remove the following class from this file.  Have it in it's own .cs file.
        public static class SearchFilterKeys
        {
            public const string ImageMadeAvailableDate = "bd";
            public const string Category = "cat";
            public const string ColorFormat = "cf";
            public const string DatePhotographedCreated = "dr";
            public const string ImageMadeAvailableDays = "ma";
            public const string ImageNumbers = "in";
            public const string ImmediateAvailability = "ia";
            public const string Keywords = "q";
            public const string LicenseModel = "lic";
            public const string Location = "lc";
            public const string MediaType = "mt";
            public const string ModelReleased = "mr";
            public const string NumberOfPeople = "np";
            public const string Orientation = "or";
            public const string Photographer = "pg";
            public const string PointofView = "pv";
            public const string Provider = "pr";
            public const string MarketingCollection = "mrc";
            public const string ClarificationSelection = "cl";
            public const string Sort = "sort";
            public const string Paging = "p";
            public const string QueryLink = "qlnk";
        }

        private static List<T> ConvertFilterValuesToList<T>(string value)
        {
            List<T> filterValueList = new List<T>();
            string[] filterValues = value.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < filterValues.Length; ++i)
            {
                //using try catch block instead of Enum.Isdefined so it'll work for both enum values and enum string
                try
                {
                    T filterValue = (T) Enum.Parse(typeof (T), filterValues[i]);
                    if (Enum.IsDefined(typeof (T), filterValue))
                    {
                        filterValueList.Add(filterValue);
                    }
                }
                catch
                {
                }
            }
            return filterValueList;
        }

        private static string GetFilterQueryValue<T>(List<T> selectedFilters)
        {
            string queryValue = "";

            if (selectedFilters != null)
            {
                foreach (T filter in selectedFilters)
                {
                    queryValue += "," + filter.GetHashCode();
                }
            }

            return queryValue.TrimStart(',');
        }

        #region Set Filter control values

        private static void SetKeywordsFilter(string keywords, ISearchView view)
        {
            view.KeywordSearch = keywords;
            
        }

        private static void SetSearchSortOption(SearchSort sortOption, ISearchBaseView view)
        {
            view.SearchSortOption = sortOption;
        }

        private static void SetCategoryFilters(List<Category> categories, ISearchBaseView view)
        {
            view.Archival = false;
            view.FineArt = false;
            view.Creative = false;
            view.Documentary = false;
            view.CurrentEvents = false;
            view.Entertainment = false;
            view.Outline = false;
            
            view.Editorial = false;

            foreach (Category value in categories)
            {
                switch (value)
                {
                    case Category.Archival:
                        view.Archival = true;
                        break;
                    case Category.FineArt:
                        view.FineArt = true;
                        break;
                    case Category.Creative:
                        view.Creative = true;
                        break;
                    case Category.Documentary:
                        view.Documentary = true;
                        break;
                    case Category.CurrentEvents:
                        view.CurrentEvents = true;
                        break;
                    case Category.Entertainment:
                        view.Entertainment = true;
                        break;
                    case Category.Outline:
                        view.Outline = true;
                        break;
                }
            }

            // turn on editorial checkbox if any of the following are checked
            view.Editorial = (view.Documentary || view.Archival ||
                              view.CurrentEvents || view.FineArt ||
                              view.Entertainment || view.Outline);
        }

        private static void SetLicenseModelFilters(List<LicenseModel> licenceModels, ISearchBaseView view)
        {
            view.RoyaltyFree = false;
            view.RightsManaged = false;

            foreach (LicenseModel value in licenceModels)
            {
                switch (value)
                {
                    case LicenseModel.RM:
                        view.RightsManaged = true;
                        break;
                    case LicenseModel.RF:
                        view.RoyaltyFree = true;
                        break;
                }
            }
        }

        private static void SetNumberOfPeopleFilters(List<NumberOfPeople> numberOfPeoples, ISearchBaseView view)
        {
            view.NoPeople = false;
            ISearchView searchView = view as ISearchView;

            foreach (NumberOfPeople value in numberOfPeoples)
            {
                switch (value)
                {
                    case NumberOfPeople.GroupsOrCrowds:
                        searchView.NumberOfPeople = ((int)NumberOfPeople.GroupsOrCrowds).ToString();
                        searchView.ShowOptionsAppliedStyle = true;
                        break;
                    case NumberOfPeople.OnePersonOnly:
                        searchView.NumberOfPeople = ((int)NumberOfPeople.OnePersonOnly).ToString();
                        searchView.ShowOptionsAppliedStyle = true;
                        break;
                    case NumberOfPeople.ThreeToFivePeopleOnly:
                        searchView.NumberOfPeople = ((int)NumberOfPeople.ThreeToFivePeopleOnly).ToString();
                        searchView.ShowOptionsAppliedStyle = true;
                        break;
                    case NumberOfPeople.TwoPeopleOnly:
                        searchView.NumberOfPeople = ((int)NumberOfPeople.TwoPeopleOnly).ToString();
                        searchView.ShowOptionsAppliedStyle = true;
                        break;
                    case NumberOfPeople.WithPeople:
                        searchView.NumberOfPeople = ((int)NumberOfPeople.WithPeople).ToString();
                        searchView.ShowOptionsAppliedStyle = true;
                        break;
                    case NumberOfPeople.WithoutPeople:
                        view.NoPeople = true;
                        break;

                }
            }
        }

        private static void SetMediaTypeFilters(List<MediaType> mediaTypes, ISearchBaseView view)
        {
            view.Illustration = false;
            view.Photography = false;

            foreach (MediaType value in mediaTypes)
            {
                switch (value)
                {
                    case MediaType.Illustration:
                        view.Illustration = true;
                        break;
                    case MediaType.Photography:
                        view.Photography = true;
                        break;
                }
            }
        }

        private static void SetColorFormatFilters(List<ColorFormat> LicenseModels, ISearchBaseView view)
        {
            view.Color = false;
            view.BlackWhite = false;

            foreach (ColorFormat value in LicenseModels)
            {
                switch (value)
                {
                    case ColorFormat.Color:
                        view.Color = true;
                        break;
                    case ColorFormat.BlackAndWhite:
                        view.BlackWhite = true;
                        break;
                }
            }
        }

        private static void SetModelReleasedFilter(string modelReleasedOnly, ISearchBaseView view)
        {
            view.ModelReleased = modelReleasedOnly == "2";
        }

        #region More Search Options

        private static void SetDateCreatedFilter(string dateCreatedValue, ISearchView searchView)
        {
            searchView.DateCreated = dateCreatedValue;
            if (!string.IsNullOrEmpty(dateCreatedValue))
            {
                searchView.ShowOptionsAppliedStyle = true;
            }
        }

        private static void SetImageMadeAvailableFilter(string daysValue, ISearchView searchView)
        {
            searchView.DaysChecked = true;
            searchView.DateRangeChecked = false;
            searchView.Days = daysValue;
            if (!string.IsNullOrEmpty(daysValue))
            {
                searchView.ShowOptionsAppliedStyle = true;
            }
        }

        private static void SetDateRangeFilter(MoreSearchOptions options, ISearchView searchView)
        {
            if (options.BeginDate != BEGIN_DATE && options.DateRangeChecked) {
                searchView.ShowOptionsAppliedStyle = true;
                searchView.DateRangeChecked = true;
                searchView.DaysChecked = false;
                searchView.BeginDate = options.BeginDate;
                searchView.EndDate = options.EndDate;
            }
        }

        private static void SetLocationFilter(string locationValue, ISearchView searchView)
        {
            searchView.Location = locationValue;
            if (!string.IsNullOrEmpty(locationValue))
            searchView.ShowOptionsAppliedStyle = true;
        }

        private static void SetPhotographerFilter(string photographerValue, ISearchView searchView)
        {
            searchView.Photographer = photographerValue;
            if (!string.IsNullOrEmpty(photographerValue))
            {
                searchView.ShowOptionsAppliedStyle = true;
            }
        }

        private static void SetProviderFilter(string providerValue, ISearchView searchView)
        {
            searchView.Provider = providerValue;
            if (!string.IsNullOrEmpty(providerValue))
            {
                searchView.ShowOptionsAppliedStyle = true;
            }
        }

        private static void SetOrientationFilter(MoreSearchOptions moreSearchOptions, ISearchView searchView)
        {
            searchView.HorizontalCheckbox = false;
            searchView.VerticalCheckbox = false;
            searchView.PanoramaCheckbox = false;
            string[] orientationlist = new string[3];
            int count = 0;

            if (moreSearchOptions.HorizontalCheckbox)
            {
                searchView.HorizontalCheckbox = true;
                searchView.HorizontalLabelText = "horizontalLabel.Text";
                orientationlist[count] = searchView.HorizontalLabelText;
                count++;
            }
            if (moreSearchOptions.VerticalCheckbox)
            {
                searchView.VerticalCheckbox = true;
                searchView.VerticalLabelText = "verticalLabel.Text";
                orientationlist[count] = searchView.VerticalLabelText;
                count++;
            }
            if (moreSearchOptions.PanoramaCheckbox)
            {
                searchView.PanoramaCheckbox = true;
                searchView.PanoramaLabelText = "panoramaLabel.Text";
                orientationlist[count] = searchView.PanoramaLabelText;
                count++;
            }

            if (!(searchView.HorizontalCheckbox &&
                  searchView.VerticalCheckbox &&
                  searchView.PanoramaCheckbox) &&
                !(!searchView.HorizontalCheckbox &&
                  !searchView.VerticalCheckbox &&
                  !searchView.PanoramaCheckbox))
            {
                searchView.ShowOptionsAppliedStyle = true;
            }

            Int32 len = count;
            //if (len == 3)
            //    searchView.OrientationSummary = orientationlist[0] + ", " + orientationlist[1] + " & " + orientationlist[2];
            if (len == 2)
                searchView.OrientationSummary = orientationlist[0] + " & " + orientationlist[1];
            else if (len == 1)
                searchView.OrientationSummary = orientationlist[0];
        }

        private static void SetPointOfViewFilter(string pointOfViewValue, ISearchView searchView)
        {
            int povNumber;
            int.TryParse(pointOfViewValue, out povNumber);
            PointOfView pointOfView = PointOfView.All;

            if (Enum.IsDefined(typeof(PointOfView), povNumber))
            {
                pointOfView = (PointOfView) povNumber;
            }

            searchView.PointOfView = ((int)pointOfView).ToString();
            if (pointOfView != PointOfView.All)
            {
                searchView.ShowOptionsAppliedStyle = true;
            }
        }

        private static void SetImmediateAvailabilityFilter(string value, ISearchView searchView)
        {
            int sizeNumber;
            int.TryParse(value, out sizeNumber);

            ImageAvailability imageAvail = ImageAvailability.AllResolutions;

            if (Enum.IsDefined(typeof(ImageAvailability), sizeNumber))
            {
                imageAvail = (ImageAvailability)sizeNumber;
            }

            searchView.ImmediateAvailability = ((int)imageAvail).ToString();
            if (imageAvail != ImageAvailability.AllResolutions)
            {
                searchView.ShowOptionsAppliedStyle = true;
            }
        }

        private static void SetImageNumbersFilter(string value, ISearchView searchView)
        {
            searchView.ImageNumbers = value;
            if (!string.IsNullOrEmpty(value))
            {
                searchView.ShowOptionsAppliedStyle = true;
            }
        }

        private static void SetMarketingCollectionFilter(List<string> marketingCollectionList, ISearchView searchView)
        {
            if (marketingCollectionList != null && marketingCollectionList.Count > 0)
            {
                searchView.SelectedMarketingCollection = marketingCollectionList;
                searchView.ShowOptionsAppliedStyle = true;
            }
            else
            {
                searchView.SelectedMarketingCollection = null;
            }
        }
        
        #endregion

        #endregion

        #region Get Filter Control Values

        private List<Category> GetCategoryFilterList(ISearchBaseView view)
        {
            if (view.Archival && view.FineArt && view.Creative &&
                view.Documentary && view.CurrentEvents && view.Entertainment && view.Outline)
            {
                // all categories are slected. return an empty list
                return new List<Category>();
            }
            List<Category> selectedCategory = new List<Category>();

            if (view.Archival) selectedCategory.Add(Category.Archival);
            if (view.FineArt) selectedCategory.Add(Category.FineArt);
            if (view.Creative) selectedCategory.Add(Category.Creative);
            if (view.Documentary) selectedCategory.Add(Category.Documentary);
            if (view.CurrentEvents) selectedCategory.Add(Category.CurrentEvents);
            if (view.Entertainment) selectedCategory.Add(Category.Entertainment);
            if (view.Outline && Profile.Permissions.Contains(Permission.HasPermissionSearchOutline)) selectedCategory.Add(Category.Outline);
            return selectedCategory;
        }

        private static List<LicenseModel> GetLicenseModelFilterList(ISearchBaseView view)
        {
            if (view.RightsManaged && view.RoyaltyFree)
            {
                // all are selected, return an empty list
                return new List<LicenseModel>();
            }
            List<LicenseModel> selectedLicenseModel = new List<LicenseModel>();
            if (view.RoyaltyFree) selectedLicenseModel.Add(LicenseModel.RF);
            if (view.RightsManaged) selectedLicenseModel.Add(LicenseModel.RM);
            return selectedLicenseModel;
        }

        private static List<NumberOfPeople> GetNumberOfPeopleFilterList(ISearchBaseView view)
        {
            List<NumberOfPeople> selectedNumberOfPeoples = new List<NumberOfPeople>();
            if (view.NoPeople)
            {
                selectedNumberOfPeoples.Add(NumberOfPeople.WithoutPeople);
            }
            return selectedNumberOfPeoples;
        }

        private static List<MediaType> GetMediaTypesFilterList(ISearchBaseView view)
        {
            if (view.Illustration && view.Photography)
            {
                // all are selected, return an empty list
                return new List<MediaType>();
            }
            List<MediaType> selectedMediaType = new List<MediaType>();
            if (view.Illustration) selectedMediaType.Add(MediaType.Illustration);
            if (view.Photography) selectedMediaType.Add(MediaType.Photography);
            return selectedMediaType;
        }

        private static List<ColorFormat> GetColorFormatFilterList(ISearchBaseView view)
        {
            if (view.Color && view.BlackWhite)
            {
                // all are selected, return an empty list
                return new List<ColorFormat>();
            }
            List<ColorFormat> selectedColorFormats = new List<ColorFormat>();
            if (view.Color) selectedColorFormats.Add(ColorFormat.Color);
            if (view.BlackWhite) selectedColorFormats.Add(ColorFormat.BlackAndWhite);
            return selectedColorFormats;
        }

        private static List<string> GetDatesFilterList(ISearchView view)
        {
            List<String> dateList = new List<string>();
            if (!String.IsNullOrEmpty(view.BeginDate) && !String.IsNullOrEmpty(view.EndDate) && (view.BeginDate != BEGIN_DATE))
            {
                dateList.Add(view.BeginDate);
                dateList.Add(view.EndDate);
            }
            return dateList;
        }

        private static List<Orientation> GetOrientationFilterList(ISearchView view)
        {

           
            if (view.PanoramaCheckbox && view.HorizontalCheckbox && view.VerticalCheckbox)
            {
                // all orientation are selected return an empty list
                return new List<Orientation>();
            }

            List<Orientation> selectedOrientations = new List<Orientation>();
            if(view.PanoramaCheckbox) selectedOrientations.Add(Orientation.Panorama);
            if(view.HorizontalCheckbox) selectedOrientations.Add(Orientation.Horizontal);
            if(view.VerticalCheckbox) selectedOrientations.Add(Orientation.Vertical);

            return selectedOrientations;
        }

        private static List<string> GetSelectedMarketingCollection(ISearchView view)
        {
            return view.SelectedMarketingCollection;
        }

        #endregion

        #endregion

        //todo: bug # 16610 Remove the input parameter from the following method.  Retrieve the value from the View.
        //todo: bug # 16610 Remove the return value from the following method.  Set a property on the View instead.
        public ClientSearchResults Search(NameValueCollection query, int itemsPerPage, int startPosition,
                                               string username, string countryCode, string cultureName)
        {
            ClientSearchResults clientSearchResults = new ClientSearchResults();
            try
            {
                SearchRequest request = CreateSearchRequest(query, itemsPerPage, startPosition, cultureName);
                
                ImageSearchResults results = new ImageSearchResults();
                try
                {
                    if (Profile.IsChinaUser)
                    {
                        results = ImageAgent.Search(request, ChinaCountryCode , username);
                    }
                    else
                    {
                        results = ImageAgent.Search(request, Profile.CountryCode, username);
                    }
                }
                catch(Exception ex)
                {
                    LoggingHelper.Log(ex);
                }
                finally
                {
                    if(results.SearchDisplayImages == null )
                    {
                        results.SearchDisplayImages = new List<SearchDisplayImage>();
                    }
                }
                if (this.postSearchView != null && results.TotalRecords == 0)
                {
                    this.postSearchView.ShowZeroResults = true;
                }
                
                if (results.SearchDisplayImages != null )//&& results.SearchDisplayImages.Count > 0)
                {
                    results.SearchDisplayImages.ForEach(
                        delegate(SearchDisplayImage img)
                            {
                                if (img.Title != null && img.Title.Contains(@"'"))
                                {
                                    img.Title = img.Title.Replace(@"'", @"&rsquo;");
                                }
                            }
                        );
                    results.SearchDisplayImages.ForEach(
                        delegate(SearchDisplayImage img)
                        {
                            if (img.Title != null && (img.Title.Contains(@"<") || img.Title.Contains(@">")))
                            {
                                img.Title = img.Title.Replace(@"<", @"&lt;");
                                img.Title = img.Title.Replace(@">", @"&gt;");
                            }
                             
                        }
                        );
                    Dictionary<string, QuickPicItem> quickPickImages = GetImagesInQuickPick();

                    int lightboxId = ReadActiveLightboxId();
                    List<SearchResultProduct> resultProducts = ConvertSearchResults(results, quickPickImages, lightboxId);
                    clientSearchResults = new ClientSearchResults(
                        results.Clarifications, 
                        results.FilterCounts, 
                        results.TotalRecords,
                        resultProducts);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, null, "SearchPresenter: Search()");
                throw;
            }
            SetSearchFilters();
            PopulateSearchFlyoutAndSearchBuddyFilterState(query);
            return clientSearchResults;
        }

        private void SetSearchFilters()
        {
            if(this.postSearchView != null)
            {
                this.postSearchView.Creative = true;
                this.postSearchView.Editorial = true;
                this.postSearchView.FineArt = true;
                this.postSearchView.Documentary = true;
                this.postSearchView.Archival = true;
                this.postSearchView.CurrentEvents = true;
                this.postSearchView.Entertainment = true;
                if(Profile.CanSeeOutline)
                    this.postSearchView.Outline = true;
                this.postSearchView.RightsManaged = true;
                this.postSearchView.RoyaltyFree = true;

            }
        }

        private Boolean CheckCorbisIdInQuickPickList(String corbisId, Dictionary<string, QuickPicItem> quickPickImages)
        {
            Boolean isInQuickPick = false;
            if (quickPickImages != null && quickPickImages.Count > 0)
            {
                if (quickPickImages.ContainsKey(corbisId))
                {
                    QuickPicItem item = quickPickImages[corbisId];
                    if (item != null)
                    {
                        isInQuickPick = true;
                    }
                }
            }
            return isInQuickPick;
        }

        private Dictionary<string, ImageInLightboxCart> GetImagesInLightboxCart(string username,
                                                                                ImageSearchResults results,
                                                                                int lightboxId)
        {
            List<string> corbisIds = GetCorbisIdsFromSearchResults(results);
            Dictionary<string, ImageInLightboxCart> imageCollection = null;

            if (corbisIds != null && corbisIds.Count > 0)
            {
                List<ImageInLightboxCart> imagesInLightboxCart =
                    LightboxCartAgent.GetLightBoxCartStatusByCorbisIds(username, corbisIds, lightboxId);

                imageCollection = new Dictionary<string, ImageInLightboxCart>();
                foreach (ImageInLightboxCart item in imagesInLightboxCart)
                {
                    imageCollection.Add(item.CorbisId, item);
                }
            }
            return imageCollection;
        }

        private static List<string> GetCorbisIdsFromSearchResults(ImageSearchResults results)
        {
            List<string> corbisIds = null;
            if (results.SearchDisplayImages != null)
            {
                corbisIds = results.SearchDisplayImages.ConvertAll<string>(
                    delegate(SearchDisplayImage displayImage) { return displayImage.CorbisId; });
            }
            return corbisIds;
        }

        private SearchRequest CreateSearchRequest(NameValueCollection query, int itemsPerPage, int startPosition,
                                                         string cultureName)
        {
            SearchRequest request = new SearchRequest();
            List<SearchFilter> searchFilters = BuildSearchFilters(query);
            request.SearchRequestMetaData = new SearchRequestHeader();
            request.SearchRequestMetaData.ClientIp = Corbis.Web.Utilities.ClientIPHelper.GetClientIpAddress();
            request.SearchRequestMetaData.SearchRequestUid = CalculateFilterDifferences(searchFilters);
            request.SearchRequestMetaData.CurrentLightboxId = ReadActiveLightboxId();
            
            string previousQueryString = stateItems.GetStateItemValue<string>(SearchSessionKeys.SearchQuery, SearchSessionKeys.PreviousSearch, StateItemStore.AspSession);
            if(previousQueryString != null && !previousQueryString.ToLower().Equals(query))
            {
                StateItem<string> searchQuery = new StateItem<string>(SearchSessionKeys.SearchQuery, SearchSessionKeys.PreviousSearch,query.ToString(),StateItemStore.AspSession);
                stateItems.SetStateItem(searchQuery);
            }

            request.CultureName = cultureName;
            request.ItemsPerPage = itemsPerPage;
            request.StartPosition = startPosition;
            ArrayOfSearchFilter arrayofSearchFilter = new ArrayOfSearchFilter();
            arrayofSearchFilter.AddRange(searchFilters);
            request.SearchFilters = arrayofSearchFilter;
            request.SortOrder = ProcessSortFilter(query);

            return request;
        }

        private Guid CalculateFilterDifferences(List<SearchFilter>  query)
        {
            string previousQueryString = stateItems.GetStateItemValue<string>(SearchSessionKeys.SearchQuery, SearchSessionKeys.PreviousSearch, StateItemStore.AspSession);
            NameValueCollection previousSearchFilter = ConvertStringToNameValueCollection(previousQueryString);
            List<SearchFilter> previousSearchFilters = BuildSearchFilters(previousSearchFilter);
            
            bool isThisANewSearch = false;
            Guid searchUid;

             Guid previousSearchUid = stateItems.GetStateItemValue<Guid>(SearchSessionKeys.SearchQuery, SearchSessionKeys.PreviousSearchUid, StateItemStore.AspSession);
            if(previousSearchUid == Guid.Empty || previousSearchUid == null)
            {
                isThisANewSearch = true;
            }
            else
            {
                if (previousSearchFilters != null && (previousSearchFilters.Count != query.Count))
                {
                    isThisANewSearch = true;
                }
                else if(previousSearchFilters != null)
                {
                    //Counts are equal, now make sure that filters inside filter ( categoryCont,marketing collection etc hasnt changed at all
                    foreach (SearchFilter filter in previousSearchFilters)
                    {
                        var requestedFilter = FindMyFilter(ref query,  filter);
                       if(CompareSearchFilter(requestedFilter, filter))
                       {
                           continue;
                       }
                       else
                       {
                           isThisANewSearch = true;
                           break;
                       }
                    }
                }
                else if(previousSearchFilters == null)
                {
                    isThisANewSearch = true;
                }
            }




            if (isThisANewSearch)
            {
                searchUid = Guid.NewGuid();
            }
            else
            {
                searchUid = previousSearchUid;
            }
            StateItem<Guid> searchIdentiFier = new StateItem<Guid>(SearchSessionKeys.SearchQuery, SearchSessionKeys.PreviousSearchUid, searchUid, StateItemStore.AspSession);
            stateItems.SetStateItem<Guid>(searchIdentiFier);
            return searchUid;
        }

        private bool CompareSearchFilter(SearchFilter oldFilter, SearchFilter newFilter)
        {
            if(oldFilter != null && newFilter != null)
                return oldFilter.CompareTo(newFilter);
            else
            {
                return false;
            }
        }

        private SearchFilter FindMyFilter(ref List<SearchFilter> searchFilters,  SearchFilter searchFilter)
        {
            SearchFilter requestedSearchFilter = searchFilters.Find(new Predicate<SearchFilter>(delegate(SearchFilter filter)
                                                               {
                                                                   return filter.Id == searchFilter.Id;
                                                               }));
            return requestedSearchFilter;
        }

        private NameValueCollection ConvertStringToNameValueCollection(string query)
        {
            NameValueCollection myValues = new NameValueCollection();
            if (!string.IsNullOrEmpty(query))
            {
                string[] aryStrings = query.Split(new char[] {'&'});
                string[] nameAndValue;
                foreach (string s in aryStrings)
                {
                    if (s.IndexOf('=') > -1)
                    {
                        nameAndValue = s.Split(new char[] {'='});
                        myValues.Add(nameAndValue[0], nameAndValue[1]);
                    }
                }
            }
            return myValues;
        }

        private string ProcessSortFilter(NameValueCollection query)
        {

            string sortParameter = stateItems.GetStateItemValue<string>(SearchKeys.Name, SearchKeys.SortOptionCookieKey, StateItemStore.Cookie);
            int sortInt;
            if(!string.IsNullOrEmpty(sortParameter) && int.TryParse(sortParameter, out sortInt))
            {
                SearchSort searchSort = (SearchSort)(Enum.IsDefined(typeof (SearchSort), int.Parse(sortParameter) )
                                            ? Enum.Parse(typeof (SearchSort), sortParameter.ToString())
                                            : SearchSort.Relevancy);
                //validate that user is not cheating by directly entering the sort = 4 :))))))
                ISearchView searchView = view as ISearchView;
                bool isOutlineSelected = false;
                if(searchView != null )
                {
                    isOutlineSelected = searchView.Outline;
                }
                else if (! string.IsNullOrEmpty(query[SearchFilterKeys.Category]))
                {
                    CategoriesFilter filter = new CategoriesFilter( ConvertFilterValuesToList<Category>(query[SearchFilterKeys.Category]));
                    isOutlineSelected = filter.CategoryIds.Contains(Category.Outline);
                }

                
                if(searchSort == SearchSort.DatePublished && !(Profile.Permissions.Contains(Permission.HasPermissionSearchOutline) && isOutlineSelected ))
                {
                    searchSort = SearchSort.Relevancy;
                }
                return  ((int)searchSort).ToString();
            }
            else if(string.IsNullOrEmpty(sortParameter) && Profile.Permissions.Contains(Permission.HasPermissionSearchOutline))
            {
                StateItem<int> searchSort = new StateItem<int>(SearchKeys.Name, SearchKeys.SortOptionCookieKey, (int)SearchSort.DatePublished, StateItemStore.Cookie);
                stateItems.SetStateItem(searchSort);
                return ((int) SearchSort.DatePublished).ToString();
            }
            else
            {
                return ((int)SearchSort.Relevancy).ToString();
            }
            return ((int) SearchSort.Relevancy).ToString();
        }

        /// <summary>
        /// Create search filters for a search request
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private List<SearchFilter> BuildSearchFilters(NameValueCollection query)
        {
            string[] keys = query.AllKeys;
            List<SearchFilter> searchFilters = new List<SearchFilter>(keys.Length);
            SearchFilter searchFilter;
            SearchTextFilter keywordFilter = null;
            string clarificationOptions = null;
            string currentKeyword = GetSearchKeywordFromQueryString(query);

            for (int i = 0; i < keys.Length; ++i)
            {
                string[] values = query.GetValues(i);
                if (keys[i] != null && values.Length > 0)
                {
                    if (keys[i] == SearchFilterKeys.ClarificationSelection)
                    {
                        clarificationOptions = values[0];
                    }
                    else
                    {
                        searchFilter = GetSearchFilter(keys[i], HttpContext.Server.UrlDecode(values[0]));
                        if (searchFilter != null)
                        {
                            if (searchFilter.Id == SearchFilterType.SearchTextFilter)
                            {
                                keywordFilter = (SearchTextFilter)searchFilter;
                            }
                            else
                            {
                                searchFilters.Add(searchFilter);
                            }
                        }
                    }
                }
            }

            List<Clarification> clarificationsFromSession = new List<Clarification>();

            if (!string.IsNullOrEmpty(clarificationOptions))
            {
                clarificationsFromSession = GetClarificationsFromSession();
                if (clarificationsFromSession == null || clarificationsFromSession.Count == 0)
                {
                    // Get Previous Keyword with Clarification and its Clarifications
                    string previousClarifiedKeyword = GetPreviousClarifiedKeywordFromSession();
                    List<Clarification> previousClarifications = GetPreviousClarificationsFromSession();
                    if (previousClarifiedKeyword == currentKeyword)
                    {
                        clarificationsFromSession = previousClarifications;
                    }
                }
            }

            List<Clarification> selectedClarifications = GetSelectedClarifications(clarificationsFromSession, clarificationOptions);

            if (postSearchView != null)
            {
                string title = GetClarificationSearchTitle(selectedClarifications, keywordFilter, clarificationOptions);
                if (!string.IsNullOrEmpty(title))
                {
                    postSearchView.SearchResultTitle = title;
                }
            }

            keywordFilter = ModifyKeywordFilterForClarification(selectedClarifications, keywordFilter, clarificationOptions);
            if (keywordFilter != null)
            {
                searchFilters.Add(keywordFilter);
            }


            if (string.IsNullOrEmpty(clarificationOptions))
            {
                MoveKeywordAndClarificationToPreviousInState();
                DeleteKeywordAndClarificationsFromState();
            }

            return searchFilters;
        }

        private List<Clarification> GetClarificationsFromSession()
        {
            return stateItems.GetStateItemValue<List<Clarification>>(SearchKeys.Name, SearchKeys.Clarifications, StateItemStore.AspSession);
        }

        private string GetKeywordWithClarificationsFromSession()
        {
            return stateItems.GetStateItemValue<string>(SearchKeys.Name, SearchKeys.KeywordWithClarifications, StateItemStore.AspSession);
        }

        private List<Clarification> GetPreviousClarificationsFromSession()
        {
            return stateItems.GetStateItemValue<List<Clarification>>(SearchKeys.Name, SearchKeys.PreviousClarifications, StateItemStore.AspSession);
        }

        private string GetPreviousClarifiedKeywordFromSession()
        {
            return stateItems.GetStateItemValue<string>(SearchKeys.Name, SearchKeys.PreviousClarifiedKeyword, StateItemStore.AspSession);
        }

        private static List<Clarification> GetSelectedClarifications(List<Clarification> clarifications, string clarificationFlags)
        {
            List<Clarification> selectedClarifications = null;
            if (clarifications != null && !string.IsNullOrEmpty(clarificationFlags))
            {
                selectedClarifications = new List<Clarification>();
                string[] options = clarificationFlags.Split(',');

                for (int i = 0; (i < options.Length) && (i < clarifications.Count); i++)
                {
                    Clarification newClarification = new Clarification();
                    newClarification.Keyword = clarifications[i].Keyword;
                    newClarification.ClarifiedTerms = new List<ClarifiedTerm>();
                    char[] flags = options[i].ToCharArray();
                    for (int j = 0; (j < flags.Length) && (j < clarifications[i].ClarifiedTerms.Count); j++)
                    {
                        //todo: bug # 16534 Use an enum instead of '1'.
                        if (flags[j] == '1')
                        {
                            newClarification.ClarifiedTerms.Add(clarifications[i].ClarifiedTerms[j]);
                        }
                    }
                    if (newClarification.ClarifiedTerms.Count > 0)
                    {
                        selectedClarifications.Add(newClarification);
                    }
                }
            }
            return selectedClarifications;
        }

        private static SearchTextFilter ModifyKeywordFilterForClarification(List<Clarification> clarifications, SearchTextFilter filter, string options)
        {
            if (clarifications != null && filter != null && !string.IsNullOrEmpty(options))
            {
                // Get Keywords from the filter.
                List<string> userTerms = GetUserTermsFromKeywordFilter(filter);
                // Check for any Phrases in Clarifications
                List<string> phraseKeywords = GetPhrasesFromClarification(clarifications);

                // Remove the Keywords from filter that are part of Phrase
                RemovePhraseKeywordsAndAddPhrases(ref userTerms, phraseKeywords);

                if (userTerms.Count > 0)
                {
                    string separator;
                    //todo: bug # 16616 Extract the following into a method
                    foreach (Clarification c in clarifications)
                    {
                        string subExpression = string.Empty;
                        separator = string.Empty;
                        foreach (ClarifiedTerm term in c.ClarifiedTerms)
                        {
                            subExpression += string.Concat(separator, "(", term.Query.Trim(), ")");
                            //todo: bug # 16616 Make the "OR" a constant or enum.
                            separator = " OR ";
                        }

                        // Replace only keywords for which there are clarifications
                        // Keep the keywords as it is to pass back to search
                        if (!string.IsNullOrEmpty(subExpression))
                        {
                            for (int i = 0; i < userTerms.Count; i++)
                            {
                                if (string.Compare(userTerms[i], c.Keyword, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    userTerms[i] = string.Concat("(", subExpression, ")");
                                }
                            }
                        }
                    }

                    filter.Text = string.Join(" ", userTerms.ToArray());

                }
            }
            return filter;
        }

        /// <summary>
        /// Get Phrases from Clarifications
        /// </summary>
        /// <param name="clarifications"></param>
        /// <returns></returns>
        private static List<string> GetPhrasesFromClarification(List<Clarification> clarifications)
        {
            List<string> phraseKeywords = null;

            foreach (Clarification clarification in clarifications)
            {
                phraseKeywords = new List<string>();

                if (clarification.Keyword.Split(new char[] { ' ' }).Length > 1)
                {
                    phraseKeywords.Add(clarification.Keyword);
                }
            }

            if (phraseKeywords != null && phraseKeywords.Count == 0)
            {
                phraseKeywords = null;
            }

            return phraseKeywords;
        }

        /// <summary>
        /// Removes individual Keywords from UserTerms and Adds Keyword  Phrases
        /// </summary>
        /// <param name="userTerms">Keywords supplied in Query.</param>
        /// <param name="phraseKeywords">Combination of Keywords treated as phrases by Search Engine.</param>
        private static void RemovePhraseKeywordsAndAddPhrases(ref List<string> userTerms, List<string> phraseKeywords)
        {
            if (phraseKeywords != null)
            { 
                // Get individual keywords from all phraseKeywords
                List<string> individualKeywords = new List<string>();
                foreach (string phrase in phraseKeywords)
                { 
                    individualKeywords.AddRange(phrase.Split(new char[] {' '}));

                    // add the phrase to userTerms
                    userTerms.Add(phrase);
                }
                // Remove the individual keywords from phrase
                foreach (string individualKeyword in individualKeywords)
                {
                    userTerms.Remove(individualKeyword);
                }
            }
        }


        private static string GetClarificationSearchTitle(List<Clarification> clarifications, SearchTextFilter filter, string options)
        {
            string title = filter != null ? filter.Text : string.Empty;

            if (clarifications != null && filter != null && !string.IsNullOrEmpty(options))
            {
                title = filter.Text;
                List<string> userTerms = GetUserTermsFromKeywordFilter(filter);

                if (userTerms.Count > 0)
                {
                    title = string.Empty;
                    List<string> replacementTerms = new List<string>();
                    string separator;
                    //todo: bug # 16618 Extract the following into a method
                    foreach (Clarification clarification in clarifications)
                    {
                        string subExpression = string.Empty;
                        separator = string.Empty;
                        foreach (ClarifiedTerm term in clarification.ClarifiedTerms)
                        {
                            subExpression += string.Concat(separator, term.Text.Trim());
                            separator = ", ";
                        }

                        if (!string.IsNullOrEmpty(subExpression))
                        {
                            for (int i = 0; i < userTerms.Count; i++)
                            {
                                if (string.Compare(userTerms[i], clarification.Keyword, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    replacementTerms.Add(subExpression);
                                    userTerms[i] = string.Empty;
                                }
                            }
                        }
                    }

                    foreach (string s in userTerms)
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            replacementTerms.Add(s);
                        }
                    }
                    title = string.Join(", ", replacementTerms.ToArray());
                }
            }
            return title;
        }

        private static List<string> GetUserTermsFromKeywordFilter(SearchTextFilter filter)
        {
            List<string> userTerms = new List<string>();
            foreach (Match term in Regex.Matches(filter.Text, @"\w+"))
            {
                userTerms.Add(term.ToString());
            }
            return userTerms;
        }

        private void DeleteKeywordAndClarificationsFromState()
        {
            // Delete KeywordWithClarifications and Clarifications
            stateItems.DeleteStateItem(new StateItem<string>(
                SearchKeys.Name, 
                SearchKeys.KeywordWithClarifications, 
                string.Empty,
                StateItemStore.AspSession));
            stateItems.DeleteStateItem(new StateItem<List<Clarification>>(
                SearchKeys.Name, 
                SearchKeys.Clarifications,
                null, 
                StateItemStore.AspSession));
        }

        private void MoveKeywordAndClarificationToPreviousInState()
        {
            string previouslyStoredKeyword = GetKeywordWithClarificationsFromSession();
            List<Clarification> previouslyStoredClarifications = GetClarificationsFromSession();
            if (!string.IsNullOrEmpty(previouslyStoredKeyword))
            {
                // Save Previous KeywordWithClarifictions and its Clarifications to Session
                stateItems.SetStateItem(new StateItem<string>(SearchKeys.Name, SearchKeys.PreviousClarifiedKeyword, previouslyStoredKeyword, StateItemStore.AspSession));
                stateItems.SetStateItem(new StateItem<List<Clarification>>(SearchKeys.Name, SearchKeys.PreviousClarifications, previouslyStoredClarifications, StateItemStore.AspSession));
            }

        }

        private static SearchFilter GetSearchFilter(string key, string value)
        {
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            bool startDteFlag = false;
            bool endDateFlag = false;

            switch (key)
            {
                case SearchFilterKeys.ImageMadeAvailableDate:
                    string[] dates = value.Split(',');
                    if (dates.Length == 2)
                    {
                        startDteFlag = DateTime.TryParse(dates[0], out startDate);
                        endDateFlag = DateTime.TryParse(dates[1], out endDate);
                    }
                    if (startDteFlag && endDateFlag)
                    {
                        return new DatePhotographedFilter(startDate, endDate);
                    }
                    else
                    {
                        break;
                    }
                case SearchFilterKeys.Category:
                    return new CategoriesFilter(ConvertFilterValuesToList<Category>(value));

                case SearchFilterKeys.ColorFormat:
                    return new ColorFormatFilter(ConvertFilterValuesToList<ColorFormat>(value));

                case SearchFilterKeys.DatePhotographedCreated:
                    return new DatePhotographedRangeFilter(value);

                case SearchFilterKeys.Orientation:
                    return new OrientationFilter(ConvertFilterValuesToList<Orientation>(value));

                case SearchFilterKeys.ImageMadeAvailableDays:
                    int numberOfDays;
                    if(int.TryParse(value, out numberOfDays))
                    {
                        // The user can't specify more than the number of days to DateTime.MinValue
                        if (numberOfDays > (DateTime.Now - DateTime.MinValue).Days)
                        {
                            startDate = DateTime.MinValue;   
                        }
                        else
                        {
                            startDate = DateTime.Now.AddDays(-numberOfDays);
                        }
                        return new DatePhotographedFilter(startDate, DateTime.Now.Date);
                    }
                    break;

                case SearchFilterKeys.ImageNumbers:
                    return new Corbis.CommonSchema.Contracts.V1.ImageIdsFilter(value);

                case SearchFilterKeys.ImmediateAvailability:
                    return new Corbis.CommonSchema.Contracts.V1.ImageSizesFilter(ConvertFilterValuesToList<ImageAvailability>(value));
                
                case SearchFilterKeys.Keywords:
                    return new SearchTextFilter(value);

                case SearchFilterKeys.LicenseModel:
                    return new LicenseModelFilter(ConvertFilterValuesToList<LicenseModel>(value));

                case SearchFilterKeys.Location:
                    return new Corbis.CommonSchema.Contracts.V1.DisplayLocationFilter(value);

                case SearchFilterKeys.MediaType:
                    return new MediaTypeFilter(ConvertFilterValuesToList<MediaType>(value));
                
                case SearchFilterKeys.ModelReleased:
                    return new ModelReleasedFilter(value == "2");
                
                case SearchFilterKeys.NumberOfPeople:
                    return new NumberOfPeopleFilter(ConvertFilterValuesToList<NumberOfPeople>(value));

                case SearchFilterKeys.Photographer:
                    return new Corbis.CommonSchema.Contracts.V1.PhotographerNameFilter(value) ;

                case SearchFilterKeys.PointofView:
                    return new Corbis.CommonSchema.Contracts.V1.PointOfViewFilter(ConvertFilterValuesToList<PointOfView>(value)) ;

                case SearchFilterKeys.Provider:
                    return new Corbis.CommonSchema.Contracts.V1.SourceFilter(value);

                case SearchFilterKeys.MarketingCollection:
                    return new Corbis.CommonSchema.Contracts.V1.MarketingCollectionFilter(ConvertStringstoInt(value));     
                
                case SearchFilterKeys.QueryLink:
                    return new Corbis.CommonSchema.Contracts.V1.QueryLinkFilter(HttpUtility.UrlDecode(value));

            }

            return null;
        }

        private static List<int> ConvertStringstoInt(string marketingCollectionIdString)
        {
            List<int> marketingCollectionIds = new List<int>();
            string[] marketingCollection = marketingCollectionIdString.Split(',');

            foreach (string s in marketingCollection)
            {
                int marketinCollectionId;
                bool flag = int.TryParse(s, out marketinCollectionId);
                if(flag)
                    marketingCollectionIds.Add(marketinCollectionId);
            }

            return marketingCollectionIds;
        }

        public int ReadActiveLightboxId()
        {
            int activeLightboxId = 0;
            String activeLightbox =
                stateItems.GetStateItemValue<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey,
                                                     StateItemStore.Cookie);
            if (!String.IsNullOrEmpty(activeLightbox))
            {
                activeLightboxId =
                    int.Parse(
                        stateItems.GetStateItemValue<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey,
                                                             StateItemStore.Cookie));
            }

            return activeLightboxId;
        }

        private bool HasOnlyCurrentEventsSelected()
        {
            ISearchBaseView baseView = postSearchView ?? view as ISearchBaseView;
            if (
                    (baseView.CurrentEvents && baseView.Editorial) &&
                    (!baseView.Archival &&
                      !baseView.FineArt &&
                      !baseView.Creative &&
                      !baseView.Documentary &&
                      !baseView.Entertainment &&
                      !baseView.Outline
                    )
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void PopulateMoreSearchOptionsStateFromSession()
        {
            ISearchView searchView = view as ISearchView;
            if (searchView != null)
            {
                MoreSearchOptions options = stateItems.GetStateItemValue<MoreSearchOptions>(SearchKeys.Name, SearchKeys.MoreSearchOptions, StateItemStore.AspSession);
                searchView.MoreSearchOptionsSettings = options;
                SetControlStateInMSO(options);
            }
        }

        private void SetMoreSearchOptionsFromQuerystring(NameValueCollection qs)
        {
            ISearchView searchView = view as ISearchView;
            searchView.MoreSearchOptionsSettings = ConvertQuerystringToMSO(qs); ;
        }

        private static MoreSearchOptions ConvertQuerystringToMSO(NameValueCollection queryString)
        {
            MoreSearchOptions moreSearchOptions = new MoreSearchOptions();

            string betweenDates = HttpUtility.UrlDecode(queryString[SearchFilterKeys.ImageMadeAvailableDate]);
            if (betweenDates != null)
            {
                string[] betweenDatesArray = betweenDates.Split(',');
                if (betweenDatesArray != null && betweenDatesArray.Length == 2)
                {
                    moreSearchOptions.BeginDate = betweenDatesArray[0];
                    moreSearchOptions.EndDate = betweenDatesArray[1];
                    moreSearchOptions.DateRangeChecked = true;
                }
            }
            moreSearchOptions.DateCreated = HttpUtility.UrlDecode(queryString[SearchFilterKeys.DatePhotographedCreated]);
            moreSearchOptions.Days = HttpUtility.UrlDecode(queryString[SearchFilterKeys.ImageMadeAvailableDays]);
            moreSearchOptions.DaysChecked = !string.IsNullOrEmpty(moreSearchOptions.Days);
            string orientation = HttpUtility.UrlDecode(queryString[SearchFilterKeys.Orientation]);

            if (!string.IsNullOrEmpty(orientation))
            {
                try
                {
                    string[] orientations = orientation.Split(',');
                    foreach (string orientationValue in orientations)
                    {
                        Orientation o = (Orientation)Enum.Parse(typeof(Orientation), orientationValue);

                        switch (o)
                        {
                            case Orientation.Horizontal:
                                moreSearchOptions.HorizontalCheckbox = true;
                                break;
                            case Orientation.Vertical:
                                moreSearchOptions.VerticalCheckbox = true;
                                break;
                            case Orientation.Panorama:
                                moreSearchOptions.PanoramaCheckbox = true;
                                break;
                        }
                    }
                }
                catch (Exception)
                { }
            }
            moreSearchOptions.ImageNumbers = HttpUtility.UrlDecode(queryString[SearchFilterKeys.ImageNumbers]);
            moreSearchOptions.ImmediateAvailability = queryString[SearchFilterKeys.ImmediateAvailability];
            moreSearchOptions.Location = HttpUtility.UrlDecode(queryString[SearchFilterKeys.Location]);
            moreSearchOptions.NumberOfPeople = queryString[SearchFilterKeys.NumberOfPeople];
            moreSearchOptions.Photographer = HttpUtility.UrlDecode(queryString[SearchFilterKeys.Photographer]);
            moreSearchOptions.PointOfView = queryString[SearchFilterKeys.PointofView];
            moreSearchOptions.Provider = HttpUtility.UrlDecode(queryString[SearchFilterKeys.Provider]);
            string marketingCollectionString = HttpUtility.UrlDecode(queryString[SearchFilterKeys.MarketingCollection]); 
            if (!string.IsNullOrEmpty(marketingCollectionString) )
            {
                moreSearchOptions.SelectedMarketingCollection = new List<string>(marketingCollectionString.Split(','));
            }
            return moreSearchOptions;
        }

        public NameValueCollection GetPreviousSearchQuery()
        {
            string previousSearchQuery = stateItems.GetStateItemValue<string>(SearchSessionKeys.SearchQuery, SearchSessionKeys.PreviousSearch, StateItemStore.AspSession);
            return new System.Collections.Specialized.NameValueCollection(ConvertSearchQueryToNameValueCollection(previousSearchQuery));
        }

        public void SetMoreSearchOptions()
        {
            ISearchView searchView = view as ISearchView;
            if (searchView != null)
            {
                // if this MSO is visible, get 
                if (searchView.IsMSOVisible)
                {
                    System.Collections.Specialized.NameValueCollection queryString = new NameValueCollection();
                    queryString = this.GetPreviousSearchQuery();

                    if (!string.IsNullOrEmpty(queryString["options"]))
                    {
                        queryString.Remove(SearchPresenter.SearchFilterKeys.Keywords);
                        this.SetMoreSearchOptionsFromQuerystring(queryString);
                    }
                    else
                    {
                        this.PopulateMoreSearchOptionsStateFromSession();
                    }
                }	
                else
                {
                    // unopened MSO
                    if (postSearchView != null)
                    {
                        // Get the MSO values from the querystring
                        string previousSearchQuery = stateItems.GetStateItemValue<string>(SearchSessionKeys.SearchQuery, SearchSessionKeys.PreviousSearch, StateItemStore.AspSession);
                        System.Collections.Specialized.NameValueCollection queryString = new System.Collections.Specialized.NameValueCollection(ConvertSearchQueryToNameValueCollection(previousSearchQuery));
                        if (postSearchView.NoPeople)
                        {
                            queryString.Remove(SearchPresenter.SearchFilterKeys.NumberOfPeople);
                        }
                        this.SetMoreSearchOptionsFromQuerystring(queryString);
                    }
                    else
                    {
                        this.PopulateMoreSearchOptionsStateFromSession();
                    }
                }
            }
        }

        /// <summary>
        /// Save the UrlEncoded value of the Direclty Manipulated Keyword
        /// </summary>
        /// <param name="keyword"></param>
        //todo: bug # 16621 Remove the input parameter from the following method.  Retrieve the value from the View.
        public void SetDirectlyManipulatedKeyword(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                StateItem<string> directlyManipulatedKeyword =
                        new StateItem<string>(SearchSessionKeys.SearchQuery, SearchSessionKeys.DirectlyManipulatedKeyword, HttpUtility.UrlEncode(keyword), StateItemStore.AspSession);
                stateItems.SetStateItem(directlyManipulatedKeyword);
            }
        }

        /// <summary>
        /// Gets the UrlEncode value of the Stored Directly Manipulated Keyword
        /// </summary>
        /// <returns></returns>
        //todo: bug # 16622 Remove the return value from the following method.  Set a property on the View instead.
        //todo: bug # 16622 Fix the spelling mistake in the method name.
        public string GetDirecltyManipulatedKeyword()
        {
            string directlyManipulatedKeyword = string.Empty;
            try
            {
                directlyManipulatedKeyword = stateItems.GetStateItemValue<string>(SearchSessionKeys.SearchQuery, SearchSessionKeys.DirectlyManipulatedKeyword,
                                             StateItemStore.AspSession);
            }
            catch
            {
                directlyManipulatedKeyword = string.Empty;
            }
            return directlyManipulatedKeyword;

        }

        /// <summary>
        /// Sets the final state of MSO options prior to rendering page
        /// </summary>
        /// <remarks>
        /// The search page & presenter too frequently sets control state.  There are several 
        /// places where we read previous search criteria and set the view's control state based
        /// on them where we should be just setting control state on the current query.
        /// </remarks>
        /// <param name="queryString"></param>
        public void SetFinalMSOState(NameValueCollection queryString)
        {
            // TO DO: Refactor the search control, search results page, and search presenter to
            // make this cleaner, more matinatable, and probably more effieicnt.
            ISearchView searchView = view as ISearchView;
            if (searchView != null)
            {
                if (postSearchView == null)
                {
                    searchView.ShowOptionsAppliedStyle = false;
                    MoreSearchOptions moreSearchOptions;
                    moreSearchOptions = GetMoreSearchOptionsFromSession();
                    SetControlStateInMSO(moreSearchOptions);
                }
                else
                {
                    // update state based on current query string (we should be on SearchResults page)
                    searchView.ShowOptionsAppliedStyle = false;
                    SetMoreSearchOptionsFromQuerystring(queryString);

                    // verify that Keyword Search is cleared if appropritate
                    if (string.IsNullOrEmpty(queryString[SearchPresenter.SearchFilterKeys.Keywords]))
                    {
                        searchView.KeywordSearch = string.Empty;
                    }
                }
            }
        }
    }

    public static class SearchSessionKeys
    {
        public const string SearchQuery = "SearchQuery";
        public const string DirectlyManipulatedKeyword = "DirectlyManipulatedKeyword";
        public const string PreviousSearchUid = "PreviousSearchUid";
        public const string DirectlyManipulatedSearch = "UserSearchOptions";
        public const string PreviousSearch = "PreviousSearch";
        public const string Search = "Search";
        
    }
}
