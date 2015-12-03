using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Data;
using System.Text;
using System.Xml;
using Corbis.CommonSchema.Contracts.V1;
using Contracts = Corbis.Search.Contracts.V1;
using Corbis.Search.ServiceAgents.V1;
using Corbis.Web.UI.Presenters.Tools.Interfaces;
using Corbis.Search.Contracts.V1;

namespace Corbis.Web.UI.Presenters.Tools
{
    public class SearchPresenter : BasePresenter
    {
        Contracts.ISearchContract searchServiceAgent;
        ISearchView searchView;
        //int NumberOfImagesPerRow = 4;
        int HitsPerPage = 52, StartPosition = 1, CachePageSize = 1;
        double ElapsedTime;
        public int PageNumber = 1,  CurrentPageDisplayNumber = 0;
        public long TotalHitsFound = 0;
        string UserName, SortOrder = "Relevancy", Language = "English", UserCountry = "US", SearchText, MediaTypes, ColorFormats, Categories, ArchiveCollections;
        List<string> ExcludedCollections, CorbisIDs, MediaIDs;
        bool CorbisIdMode, ThumbnailMode = true, ExternalView = true, RoyaltyFree = true, RightsManaged = true, Core = true, Outline;
        private DataSet _ThumbnailList;
        private SearchPresenter.DisplayMode _displayMode = DisplayMode.Unknown;
        
        public enum DisplayMode
        {
            Unknown = 0,
            CorbisID = 1,
            Thumbnail = 2,
        }

        public SearchPresenter(ISearchView searchView, Contracts.ISearchContract searchServiceAgent)
        {
            if (searchView == null)
            {
                throw new ArgumentNullException("SearchPresenter: SearchPresenter() - Search view cannot be null.");
            }
            if (searchServiceAgent == null)
            {
                throw new ArgumentNullException("SearchPresenter: SearchPresenter() - Search service agent cannot be null.");
            }

            this.searchView = searchView;
            this.searchServiceAgent = searchServiceAgent;
        }

        public SearchPresenter(ISearchView searchView)
            : this(searchView, new SearchServiceAgent())
        {
        }

        #region ParseQueryString
        public void ParseQueryString()
        {
            string temp;
            NameValueCollection query = searchView.Query;

            temp = query["UserName"];
            if (!string.IsNullOrEmpty(temp))
            {
                UserName = temp;
            }

            temp = query["HitsPerPage"];
            if (!string.IsNullOrEmpty(temp) && DSString.IsNumeric(temp))
            {
                HitsPerPage = int.Parse(temp);
            }

            temp = query["StartPosition"];
            if (!string.IsNullOrEmpty(temp) && DSString.IsNumeric(temp))
            {
                StartPosition = int.Parse(temp);
            }

            temp = query["SortOrder"];
            if (!string.IsNullOrEmpty(temp))
            {
                SortOrder = temp;
            }

            temp = query["Lang"];
            if (!string.IsNullOrEmpty(temp))
            {
                Language = temp;
            }

            temp = query["UserCountry"];
            if (!string.IsNullOrEmpty(temp))
            {
                UserCountry = temp;
            }

            temp = query["CorbisIDs"];
            if (!string.IsNullOrEmpty(temp))
            {
                CorbisIDs = DSString.DivideString(temp, @"\n");
            }

            temp = query["MediaIDs"];
            if (!string.IsNullOrEmpty(temp))
            {
                MediaIDs = DSString.DivideString(temp, @"\n");
            }

            temp = query["MediaTypes"];
            if (!string.IsNullOrEmpty(temp))
            {
                MediaTypes = temp;
            }

            temp = query["ColorFormats"];
            if (!string.IsNullOrEmpty(temp))
            {
                ColorFormats = temp;
            }

            temp = query["ExcludedCollections"];
            if (!string.IsNullOrEmpty(temp))
            {
                ExcludedCollections = DSString.DivideString(temp, " ");
                //temp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }

            temp = query["SearchText"];
            if (!string.IsNullOrEmpty(temp))
            {
                SearchText = temp;
            }

            temp = query["CorbisIdMode"];
            if (!string.IsNullOrEmpty(temp))
            {
                CorbisIdMode = bool.Parse(temp);
                if (CorbisIdMode)
                {
                    _displayMode = DisplayMode.CorbisID;
                    searchView.DisplayMode = DisplayMode.CorbisID;
                }
            }

            temp = query["ThumbnailMode"];
            if (!string.IsNullOrEmpty(temp))
            {
                ThumbnailMode = bool.Parse(temp);
                if (ThumbnailMode)
                {
                    _displayMode = DisplayMode.Thumbnail;
                    searchView.DisplayMode = DisplayMode.Thumbnail;
                }
            }

            temp = query["ExternalView"];
            if (!string.IsNullOrEmpty(temp))
            {
                ExternalView = bool.Parse(temp);
            }

            temp = query["RoyaltyFree"];
            if (!string.IsNullOrEmpty(temp))
            {
                RoyaltyFree = bool.Parse(temp);
            }

            temp = query["RightsManaged"];
            if (!string.IsNullOrEmpty(temp))
            {
                RightsManaged = bool.Parse(temp);
            }

            temp = query["Core"];
            if (!string.IsNullOrEmpty(temp))
            {
                Core = bool.Parse(temp);
            }

            temp = query["Outline"];
            if (!string.IsNullOrEmpty(temp))
            {
                Outline = bool.Parse(temp);
            }

            temp = query["Categories"];
            if (!string.IsNullOrEmpty(temp))
            {
                Categories = temp;
            }

            temp = query["ArchiveCollections"];
            if (!string.IsNullOrEmpty(temp))
            {
                ArchiveCollections = temp;
            }
        }
        #endregion

        /*
        /// <summary>
        /// Get the value to set in the QueryString
        /// </summary>
        /// <returns></returns>
        public string BuildQuery()
        {
            StringBuilder query = new StringBuilder();
            string queryFormat = "{0}={1}&";

            // Keywords
            query.AppendFormat(queryFormat, SearchFilterKeys.Keywords, searchView.KeywordSearch);

            // LicenseModel
            if (searchView.RoyaltyFree && searchView.RightsManaged)
            {
                string[] licenseModels = new string[] { LicenseModel.RF.ToString(), LicenseModel.RM.ToString() };
                query.AppendFormat(queryFormat, SearchFilterKeys.LicenseModel, string.Join(",", licenseModels));
            }
            else if (searchView.RoyaltyFree)
            {
                query.AppendFormat(queryFormat, SearchFilterKeys.LicenseModel, LicenseModel.RF);
            }
            else if (searchView.RightsManaged)
            {
                query.AppendFormat(queryFormat, SearchFilterKeys.LicenseModel, LicenseModel.RM);
            }

            return query.ToString().Remove(query.ToString().Length - 1);
        }
        */

        private void GetCorbisIDDisplayList()
        {
            DataSet dataSetCorbisID = null;
            DataRow dataRow = null;
            //searchResults
            //searchView.CorbisIDList 
            //ThumbnailDisplayList

            dataSetCorbisID = new DataSet();
            dataSetCorbisID.Tables.Add(_ThumbnailList.Tables[0].TableName);
            dataSetCorbisID.Tables[0].Columns.Add("CorbisID", typeof(string));

            for (int i = 0; i < _ThumbnailList.Tables[0].Rows.Count; i++)
            {
                //ImportRow(_ThumbnailList.Tables[0].Rows[i - 1]
                dataRow = dataSetCorbisID.Tables[0].NewRow();

                dataRow[0] = _ThumbnailList.Tables[0].Rows[i]["CorbisID"];
                dataSetCorbisID.Tables[0].Rows.Add(dataRow);
            }

            searchView.CorbisIDList = dataSetCorbisID;
        }

        public void PrevPage()
        {
            if (PageNumber > 1)
            {
                PageNumber -= 1;
            }
        }

        public void NextPage()
        {
            PageNumber += 1;
        }

       
        public int DisplayFrom
        {
            get
            {
                return (PageNumber - 1) * HitsPerPage + StartPosition;
            }
        }

        public long DisplayTo
        {
            get
            {
                return Math.Min(DisplayFrom + HitsPerPage - 1, this.TotalHitsFound);
            }
        }

        public bool DisableDisplayTo
        {
            get
            {
                if ((DisplayFrom + HitsPerPage - 1) >= this.TotalHitsFound)
                    return true;
                else
                    return false;
            }
        }

        public string TranslateCultureName(string UICultureName)
        {
            string cultureName = "en-US";

            if (UICultureName.Equals("English")) cultureName = "en-US";
            else if (UICultureName.Equals("UKEnglish")) UICultureName.Equals("en-GB");

            return cultureName;
        }

        public DataSet ThumbnailList
        {
            set
            {
                _ThumbnailList = value;
                TotalHitsFound = _ThumbnailList.Tables[0].Rows.Count;
                if (_displayMode == DisplayMode.CorbisID)
                    GetCorbisIDDisplayList();
                else if (_displayMode == DisplayMode.Thumbnail)
                    GetThumbnailDisplayList();
            }
            get
            {
                return _ThumbnailList;
            }
        }

        public void GetThumbnailDisplayList()
        {
            DataSet dataSetFilter;

            dataSetFilter = new DataSet();
            dataSetFilter.Tables.Add(_ThumbnailList.Tables[0].TableName);
            foreach (DataColumn dataColumn in _ThumbnailList.Tables[0].Columns)
            {
                dataSetFilter.Tables[0].Columns.Add(dataColumn.ColumnName, dataColumn.DataType);
            }
            for (int i = DisplayFrom; i <= DisplayTo; i++)
            {
                dataSetFilter.Tables[0].ImportRow(_ThumbnailList.Tables[0].Rows[i - 1]);
            }

            searchView.ThumbnailDisplayList = dataSetFilter;
        }

        public void GetDiagnostic()        
        {
            XMLService xmlService = new XMLService();
            XmlElement xmlElement, xmlElementParent, xmlElementParent1;
            XmlDocument xmlDocument;
            string queryString;
            int index, breakPoint = 70;

            //Populate Input Fields
            xmlDocument = new XmlDocument();
            xmlElement = xmlService.CreateElement(xmlDocument, xmlDocument, "InputFields", string.Empty, string.Empty);
            xmlService.CreateAttribute(xmlElement, "Text", "Input Fields", "");

            xmlElement = xmlService.CreateElement(xmlDocument, xmlElement, "Query", string.Empty, string.Empty);
            //QueryString is too long. Break it up here.
            queryString = searchView.Query.ToString();
            if (queryString.Length > breakPoint)
            {
                index = queryString.IndexOf(@"&", breakPoint);
                while (index >= 0)
                {
                    queryString = queryString.Insert(index, @"<br/>");
                    if (queryString.Length > index + breakPoint)
                        index = queryString.IndexOf(@"&", index + breakPoint);
                    else
                        index = -1;
                }
            }
            xmlService.CreateAttribute(xmlElement, "Text", queryString, "");
            searchView.InputFields = xmlDocument;

            //Populate Corbis Query
            xmlDocument = new XmlDocument();
            xmlElementParent = xmlService.CreateElement(xmlDocument, xmlDocument, "CorbisQuery", string.Empty, string.Empty);
            xmlService.CreateAttribute(xmlElementParent, "Text", "Corbis Query", "");

            xmlElement = xmlService.CreateElement(xmlDocument, xmlElementParent, "PreProcessingQuery", string.Empty, string.Empty);
            xmlService.CreateAttribute(xmlElement, "Text", "Pre Processing Query : " + this.TotalHitsFound.ToString(), "");

            xmlElement = xmlService.CreateElement(xmlDocument, xmlElementParent, "PostProcessingQuery", string.Empty, string.Empty);
            xmlService.CreateAttribute(xmlElement, "Text", "Post Processing Query : " + this.TotalHitsFound.ToString(), "");
            searchView.CorbisQuery = xmlDocument;

            //Populate FAST Query
            xmlDocument = new XmlDocument();
            xmlElementParent = xmlService.CreateElement(xmlDocument, xmlDocument, "FASTQuery", string.Empty, string.Empty);
            xmlElementParent1 = xmlService.CreateElement(xmlDocument, xmlElementParent, "OriginalQuery", string.Empty, string.Empty);
            xmlService.CreateElement(xmlDocument, xmlElementParent1, "OriginalQuery", string.Empty, string.Empty);
            xmlService.CreateElement(xmlDocument, xmlElementParent1, "FastQTLemmatizer", string.Empty, string.Empty);
            xmlService.CreateElement(xmlDocument, xmlElementParent1, "FinalQuery", string.Empty, string.Empty);

            xmlElementParent1 = xmlService.CreateElement(xmlDocument, xmlElementParent, "ResubmittedQuery", string.Empty, string.Empty);
            xmlService.CreateElement(xmlDocument, xmlElementParent1, "OriginalQuery", string.Empty, string.Empty);

            searchView.FASTQuery = xmlDocument;

            //Populate FAST statistics
            xmlDocument = new XmlDocument();
            xmlElementParent = xmlService.CreateElement(xmlDocument, xmlDocument, "FASTStatistics", string.Empty, string.Empty);
            xmlService.CreateAttribute(xmlElementParent, "Text", "FAST Statistics", "");

            xmlElement = xmlService.CreateElement(xmlDocument, xmlElementParent, "DocumentsFound", string.Empty, string.Empty);
            xmlService.CreateAttribute(xmlElement, "Text", "Documents Found : " + this.TotalHitsFound.ToString(), "");

            xmlElement = xmlService.CreateElement(xmlDocument, xmlElementParent, "MaxRank", string.Empty, string.Empty);
            xmlService.CreateAttribute(xmlElement, "Text", "Max Rank : ", "");

            xmlElement = xmlService.CreateElement(xmlDocument, xmlElementParent, "TimeUsed", string.Empty, string.Empty);
            xmlService.CreateAttribute(xmlElement, "Text", "Time Used : " + this.ElapsedTime.ToString(), "");

            searchView.FASTStatistics = xmlDocument;


            //Populate FAST Navigators
            xmlDocument = new XmlDocument();
            xmlElementParent = xmlService.CreateElement(xmlDocument, xmlDocument, "FASTNavigators", string.Empty, string.Empty);
            xmlService.CreateAttribute(xmlElementParent, "Text", "FAST Navigators", "");

            xmlElement = xmlService.CreateElement(xmlDocument, xmlElementParent, "PreProcessingQuery", string.Empty, string.Empty);
            xmlService.CreateAttribute(xmlElement, "Text", "Pre Processing Query : " + this.TotalHitsFound.ToString(), "");

            xmlElement = xmlService.CreateElement(xmlDocument, xmlElementParent, "PostProcessingQuery", string.Empty, string.Empty);
            xmlService.CreateAttribute(xmlElement, "Text", "Post Processing Query : " + this.TotalHitsFound.ToString(), "");
            searchView.FASTNavigators = xmlDocument;
        }

        public void GetSearchResults()
        {
            Contracts.SearchResults searchResults = null;
            DataSet dataSet = null;
            DataTable dataTable;
            DataRow dataRow;
            TimeSpan timeSpan;

            NameValueCollection nameValueCollectionFilters = new NameValueCollection();
            nameValueCollectionFilters.Add("q", SearchText);
            nameValueCollectionFilters.Add("lic", (RoyaltyFree ? "RF," : "") + (RightsManaged ? "RM," : ""));
            nameValueCollectionFilters.Add("mt", MediaTypes);
            nameValueCollectionFilters.Add("cs", ColorFormats);
            nameValueCollectionFilters.Add("cat", Categories);
            nameValueCollectionFilters.Add("so", SortOrder);

            Contracts.SearchRequest request = new Contracts.SearchRequest();
            List<SearchFilter> searchFilters = BuildSearchFilters(nameValueCollectionFilters);
            request.CultureName = TranslateCultureName(Language);
            request.SortOrder = SortOrder;
            request.ItemsPerPage = this.HitsPerPage * this.CachePageSize;
            request.StartPosition = this.DisplayFrom; //this.StartPosition;
            ArrayOfSearchFilter searchFilterArray = new ArrayOfSearchFilter();
            searchFilterArray.AddRange(searchFilters);
            request.SearchFilters = searchFilterArray;

            long t1 = System.DateTime.Now.Ticks;
            try
            {
                //Corbis.Search.BusinessActions.V1.TexisProvider test = new Corbis.Search.BusinessActions.V1.TexisProvider();
                searchResults = searchServiceAgent.Search(request);
                //searchResults = test.Search(request);
            }
            catch (Exception ex)
            {
                HandleException(ex, null, "SearchPresenter: Search()");
                throw;
            }
            long t2 = System.DateTime.Now.Ticks;
            timeSpan = new TimeSpan(t2 - t1);
            ElapsedTime = timeSpan.TotalSeconds;
            //searchView.ElapsedTime = ElapsedTime;
            TotalHitsFound = searchResults.TotalResultsCount;

            //Math.Min(TotalHitsFound, MaxHits) is temporary as we need SearchEngine to follow the max hits.
            //searchView.TotalHitsFound = Math.Min(TotalHitsFound, MaxHits);

            dataSet = new DataSet();
            dataTable = dataSet.Tables.Add("Images");
            dataTable.Columns.Add("ImageUrl", typeof(string));
            dataTable.Columns.Add("CorbisID", typeof(string));
            dataTable.Columns.Add("MediaID", typeof(string));
            dataTable.Columns.Add("MediaRating", typeof(string));
            dataTable.Columns.Add("FastRank", typeof(string));
            dataTable.Columns.Add("InternalAddedDate", typeof(string));
            dataTable.Columns.Add("ExternalAddedDate", typeof(string));

            //foreach (Contracts.WebProduct webProduct in searchResults.WebProducts)
            //{
            //    dataRow = dataTable.NewRow();
            //    dataRow[0] = webProduct.Url128;
            //    dataRow[1] = webProduct.CorbisId;
            //    dataRow[2] = webProduct.MediaId;
            //    dataRow[3] = "B";
            //    dataRow[4] = "75";
            //    dataRow[5] = "09/10/1990"; //webProduct.ExtensionData.ToString();
            //    dataRow[6] = "09/10/1990";
            //    dataTable.Rows.Add(dataRow);
            //    this.CurrentPageDisplayNumber += 1;
            //}

            _ThumbnailList = dataSet;
            searchView.ThumbnailList = _ThumbnailList;
            GetDiagnostic();
            searchView.ThumbnailDisplayList = _ThumbnailList;
            searchView.CorbisIDList = _ThumbnailList;
            /*
            if (_displayMode == DisplayMode.CorbisID)
            {
                GetCorbisIDDisplayList();
            }
            else if (_displayMode == DisplayMode.Thumbnail)
                GetThumbnailDisplayList();
            */
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

            for (int i = 0; i < keys.Length; ++i)
            {
                string[] values = query.GetValues(i);
                if (values != null && keys[i] != null && values.Length > 0)
                {
                    searchFilter = GetSearchFilter(keys[i], values[0]);
                    if (searchFilter != null)
                    {
                        searchFilters.Add(searchFilter);
                    }
                }
            }

            return searchFilters;
        }

        #region Search Filter Keys

        public static class SearchFilterKeys
        {
            public const string Keywords = "q";
            public const string LicenseModel = "lic";
            public const string Category = "cat";
            public const string MediaType = "mt";
            public const string ColorFormat = "cf";
            public const string NumberOfPeople = "np";
        }

        private static SearchFilter GetSearchFilter(string key, string value)
        {
            switch (key)
            {
                case SearchFilterKeys.Keywords:
                    return new SearchTextFilter(value);
                case SearchFilterKeys.LicenseModel:
                    return new LicenseModelFilter(ConvertFilterValuesToList<Corbis.CommonSchema.Contracts.V1.LicenseModel>(value));
                case SearchFilterKeys.Category:
                    return new CategoriesFilter(ConvertFilterValuesToList<Corbis.CommonSchema.Contracts.V1.Category>(value));
                case SearchFilterKeys.MediaType:
                    return new MediaTypeFilter(ConvertFilterValuesToList<MediaType>(value));
                case SearchFilterKeys.ColorFormat:
                    return new ColorFormatFilter(ConvertFilterValuesToList<ColorFormat>(value));
                case SearchFilterKeys.NumberOfPeople:
                    return new NumberOfPeopleFilter(ConvertFilterValuesToList<NumberOfPeople>(value));
            }
            return null;
        }

        private static List<T> ConvertFilterValuesToList<T>(string value)
        {
            List<T> filterValueList = new List<T>();
            string[] filterValues = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < filterValues.Length; ++i)
            {
                //using try catch block instead of Enum.Isdefined so it'll work for both enum values and enum string
                try
                {
                    T filterValue = (T)Enum.Parse(typeof(T), filterValues[i]);
                    if (Enum.IsDefined(typeof(T), filterValue))
                    {
                        filterValueList.Add(filterValue);
                    }
                }
                catch { }
            }
            return filterValueList;
        }

        #endregion

        public LicenseModelFilter GetLicenseModelFilter(string value)
        {
            List<Corbis.CommonSchema.Contracts.V1.LicenseModel> licenseModels = new List<Corbis.CommonSchema.Contracts.V1.LicenseModel>();
            string[] licenseModelValues = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < licenseModelValues.Length; ++i)
            {
                if (Enum.IsDefined(typeof(Corbis.CommonSchema.Contracts.V1.LicenseModel), licenseModelValues[i]))
                {
                    licenseModels.Add((Corbis.CommonSchema.Contracts.V1.LicenseModel)Enum.Parse(
                        typeof(Corbis.CommonSchema.Contracts.V1.LicenseModel), licenseModelValues[i]));
                }
            }
            return new LicenseModelFilter(licenseModels);
        }

        public MediaTypeFilter GetMediaTypeFilter(string value)
        {
            List<MediaType> mediaTypes = new List<MediaType>();
            string[] mediaTypeValues = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < mediaTypeValues.Length; ++i)
            {
                if (Enum.IsDefined(typeof(MediaType), mediaTypeValues[i]))
                {
                    mediaTypes.Add((MediaType)Enum.Parse(typeof(MediaType), mediaTypeValues[i]));
                }
            }
            return new MediaTypeFilter(mediaTypes);
        }

        public ColorFormatFilter GetColorFormatFilter(string value)
        {
            List<ColorFormat> colorFormats = new List<ColorFormat>();
            string[] colorFormatValues = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < colorFormatValues.Length; ++i)
            {
                if (Enum.IsDefined(typeof(ColorFormat), colorFormatValues[i]))
                {
                    colorFormats.Add((ColorFormat)Enum.Parse(typeof(ColorFormat), colorFormatValues[i]));
                }
            }
            return new ColorFormatFilter(colorFormats);
        }

        public CategoriesFilter GetCategoryFilter(string value)
        {
            List<Corbis.CommonSchema.Contracts.V1.Category> categories = new List<Corbis.CommonSchema.Contracts.V1.Category>();
            string[] categoryValues = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < categoryValues.Length; ++i)
            {
                if (Enum.IsDefined(typeof(Corbis.CommonSchema.Contracts.V1.Category), categoryValues[i]))
                {
                    categories.Add((Corbis.CommonSchema.Contracts.V1.Category)Enum.Parse(typeof(Corbis.CommonSchema.Contracts.V1.Category), categoryValues[i]));
                }
            }
            return new CategoriesFilter(categories);
        }
    }
}