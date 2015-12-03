using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.Web.Entities;
using Corbis.Web.UI.Navigation;
using Corbis.Web.Utilities.StateManagement;

namespace Corbis.Web.UI.Search
{
    public partial class SearchResultHeader : System.Web.UI.UserControl
    {
        private ItemsPerPage _itemsPerPage;
        private bool _showHeader = true;
        public event PageCommandDelegate PageCommand;
        public event PageSizeCommandDelegate PageSizeCommand;
        public event GenericCommandDelegate GenericCommand;
        private StateItemCollection stateItems;
        private string recentImageId = string.Empty;
        private string recentImageURL = string.Empty;


        public string RecentImageId
        {
            get { return recentImageId; }
            set
            {
                //recentImageId = value; 
                if (string.IsNullOrEmpty(value))
                {
                    this.recentImage.Visible = false;
                    this.recentImageId = string.Empty;
                }
                else
                {
                    recentImageId = value;
                    this.recentImage.Visible = true;
                    this.recentImageSelected.RecentImageId = value;
                }
            }
        }
        
        public string RecentImageURL
        {
            get { return recentImageURL; }
            set
            {
                //recentImageSelected.RecentImageURL = value;
                //recentImageSelected.OnClientClick = String.Format("javascript:EnlargeImagePopUp('../Enlargement/Enlargement.aspx?id={0}');return false;", RecentImageId);
                //recentImage.Visible = true;
                if (string.IsNullOrEmpty(value))
                {
                    this.recentImage.Visible = false;
                    this.recentImageSelected.Visible = false;
                    this.recentImageURL = string.Empty;
                }
                else
                {
                    this.recentImage.Visible = true;
                    this.recentImageSelected.Visible = true;
                    this.recentImageSelected.RecentImageURL = value;
                }
            }

        }

        public Decimal RecentImageRadio
        {
            set
            {
                this.recentImageSelected.RecentImageRadio = value;
            }
        }

        public int TotalRecords
        {
            get { return searchResultPager.TotalRecords; }
            set { searchResultPager.TotalRecords = value; }
        }

        public int TotalPages
        {
            get { return searchResultPager.TotalPages; }
        }

        public bool ShowHeader
        {
            get { return _showHeader;  }
            set 
            { 
                _showHeader = value;
            }
        }

        public string SearchResultTitle
        {
            set
            {
                if (Page is Corbis.Web.UI.MediaSetSearch.MediaSetSearch && ((Corbis.Web.UI.MediaSetSearch.MediaSetSearch)Page)._mediaSetspresenter != null)
                {
                    resultTitle.Text = "<b>"+ Server.HtmlEncode(value) + "</b>";
                    sortBlock.Visible = false;

                }
                else
                {
                    resultTitle.Text = "<b><span style='font-weight:normal; font-size:16px; color:#cccccc;'> " + (string)GetLocalResourceObject("searchResultTitle") + "</span> " + Server.HtmlEncode(value) + "</b>";
                    sortBlock.Visible = true;
                }
            }
        }

        public string SetPhotoGrapherNameSearchResultTitle
        {
            set
            {
                resultTitle.Text = (string)GetLocalResourceObject("searchResultTitle") + " <b><font color ='#FFFFCC'>" + (string)GetLocalResourceObject("photographer.Text") + " = " + Server.HtmlEncode(value) + "</font></b>";
            }
        }

        public string SetLocationSearchResultTitle
        {
            set
            {
                resultTitle.Text = (string)GetLocalResourceObject("searchResultTitle") + " <b><font color ='#FFFFCC'>" + (string)GetLocalResourceObject("location.Text") + " = " + Server.HtmlEncode(value) + "</font></b>";
            }
        }

        // The total number of items possible to return from a search. If this number of 
        // items is totalSearchHitCount is equal to this number, the counterMaxInfo resx is used.
        private int _maxSearchItems = int.MaxValue;
        public int MaxSearchItems
        {
            get { return _maxSearchItems; }
            set { _maxSearchItems = value; }
        }

        // TODO: Much of this logic should be in the presenter
        public void UpdateSearchTitle()
        {
            string title = string.Empty;

            if (totalSearchHitCount == 0)
            {
                title = (string) GetLocalResourceObject("zeroItemCounter");
                title = title.Replace("{0}", totalSearchHitCount.ToString("C0"));
            }
            else 
            {
                string formatString;
                int firstItem;
                int lastItem;

                // determine which format string to use based on number of hits
                if (totalSearchHitCount >= MaxSearchItems)
                {
                    formatString = (string)GetLocalResourceObject("counterMaxInfo");
                }
                else if (totalSearchHitCount == 1)
                {
                    formatString = (string)GetLocalResourceObject("countersingleInfo");
                }
                else
                {
                    formatString = (string)GetLocalResourceObject("counterInfo");
                }

                // calculate the values for the first and last items on this page
                if(totalSearchHitCount > (int) PageSize)
                {
                    firstItem = searchResultPager.StartingRecord;
                    lastItem = currentPageHitCount + searchResultPager.StartingRecord - 1;
                }
                else
                {
                    firstItem = searchResultPager.StartingRecord;
                    lastItem = totalSearchHitCount;
                }

                title = formatString.Replace("{0}", firstItem.ToString("C0"));
                title = title.Replace("{1}", lastItem.ToString("C0"));
                title = title.Replace("{2}", totalSearchHitCount.ToString("C0"));

            }
            indexInfo.Text = title;
        }

        public Button PreviousButton
        {
            get { return searchResultPager.PreviousButton; }
        }

        public Button NextButton
        {
            get { return searchResultPager.NextButton; }
        }

        public TextBox PageNumberTextBox
        {
            get { return searchResultPager.PageNumberTextBox; }
        }

        int totalSearchHitCount;
        public int TotalSearchHitCount
        {
            set 
            {
                totalSearchHitCount = value;
                UpdateSearchTitle();
                searchResultPager.TotalRecords = totalSearchHitCount;
            }
        }

        int currentPageHitCount;
        public int CurrentPageHitCount
        {
            set
            {
                currentPageHitCount = value;
                UpdateSearchTitle();
            }
        }

        public string showHidden
        {
            get
            {
                return showTermsClarification.Value;
            }
            set
            {
                ((CorbisBasePage)Page).AnalyticsData["prop7"] = value;
                showTermsClarification.Value = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            stateItems = new StateItemCollection(HttpContext.Current);
            ScriptManager manager = ScriptManager.GetCurrent(Page);
            manager.Services.Add(new ServiceReference("~/Search/SearchScriptService.asmx"));
            LoadPreferences();
        }



        private void LoadPreferences()
        {
            List<string> propertiesNotSet = stateItems.PopulateObjectFromState(this);
            foreach (string property in propertiesNotSet)
            {
                switch (property)
                {
                    case "PageSize":
                        //itemsPerPageList.SelectedValue = ((int)ItemsPerPage.items50).ToString();
                        this.PageSize = ItemsPerPage.items50;
                        break;
                    default:
                        break;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (true)
            {
                StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);

                itemsPerPageList.DataSource = GetPageSizeList();
                itemsPerPageList.CssClassField = "Value";
                itemsPerPageList.ValueField = "Key";
                if ((int)_itemsPerPage != 0)
                    itemsPerPageList.SelectedValue = ((int)_itemsPerPage).ToString();
                itemsPerPageList.DataBind();
            }

            previewList.DataSource = GetPreviewList();
            previewList.CssClassField = "Value";
            previewList.ValueField = "Key";
            if (Page is Corbis.Web.UI.Lightboxes.MyLightboxes)
            {
                previewList.SelectedValue = stateItems.GetStateItemValue<int>(LightboxCartKeys.Name, LightboxCartKeys.LightboxPreviewKey, StateItemStore.Cookie).ToString();
                this.optionDiv.Visible = false; // if implemnet we can this back
            }
            else
            {
                this.optionDiv.Visible = true;
                previewList.SelectedValue = stateItems.GetStateItemValue<int>(SearchKeys.Name, SearchKeys.SearchPreviewKey, StateItemStore.Cookie).ToString();
            }
            if (Page is Corbis.Web.UI.MediaSetSearch.MediaSetSearch)
            {
                this.Label1.Visible = false;
                this.previewList.Visible = false;
                this.displayOptions.Visible = false;
                this.arrowMeDown.Visible = false;
            }
           

            if (Page is Corbis.Web.UI.Lightboxes.EmailLightboxView)
            {
                headerlist.Visible = false;
            }
            previewList.DataBind();

            if (!string.IsNullOrEmpty(Request.QueryString[Corbis.Web.UI.Presenters.Search.SearchPresenter.SearchFilterKeys.Location]) || !string.IsNullOrEmpty(Request.QueryString[Corbis.Web.UI.Presenters.Search.SearchPresenter.SearchFilterKeys.Photographer]))
            {
                if (!string.IsNullOrEmpty(Request.QueryString[Corbis.Web.UI.Presenters.Search.SearchPresenter.SearchFilterKeys.Photographer]))
                {
                    resultTitle.Text = (string)GetLocalResourceObject("searchResultTitle") + " <b><font color ='#FFFFCC'>" + (string)GetLocalResourceObject("photographer.Text") + " = " + Server.HtmlEncode(Request.QueryString[Corbis.Web.UI.Presenters.Search.SearchPresenter.SearchFilterKeys.Photographer]) + "</font></b>";
                }
                else
                {
                    resultTitle.Text = (string)GetLocalResourceObject("searchResultTitle") + " <b><font color ='#FFFFCC'>" + (string)GetLocalResourceObject("location.Text") + " = " + Server.HtmlEncode(Request.QueryString[Corbis.Web.UI.Presenters.Search.SearchPresenter.SearchFilterKeys.Location]) + "</font></b>";
                }
            }
        }
        
        protected void PageChanged(object sender, PagerEventArgs e)
        {
            PageCommand(this, e);
        }

        public void PageSizeChanged(object sender, CommandEventArgs e)
        {
            PageSize = (ItemsPerPage)Enum.Parse(typeof(ItemsPerPage), e.CommandArgument.ToString());
            itemsPerPageList.SelectedValue = PageSize.ToString();
            // first set the pager to page 1 with new pagesize
            this.searchResultPager.PageSize = (int)PageSize;
            this.searchResultPager.PageIndex = 1;
            stateItems.SaveObjectToState(this);
            PageSizeCommand(sender, new PageSizeEventArgs(PageSize));
        }

        public void sortBlock_Sort(Object sender, CommandEventArgs e)
        {
            //string displayText = CorbisBasePage.LocalizeEnum<SearchSort>((SearchSort)e.CommandArgument);
            if (GenericCommand != null)
            {
                GenericCommand(sender, e);
            }
        }
        private Dictionary<int, string> GetPageSizeList()
        {
            Dictionary<int, string> pageSizeList = new Dictionary<int, string>();
            foreach (ItemsPerPage item in Enum.GetValues(typeof(ItemsPerPage)))
            {
                pageSizeList.Add((int)item, item.ToString());
            }
            return pageSizeList;
        }

        private Dictionary<int, string> GetPreviewList()
        {
            Dictionary<int, string> previewList = new Dictionary<int, string>();
            foreach (SearchItemPreview item in Enum.GetValues(typeof(SearchItemPreview)))
            {
                previewList.Add((int)item, item.ToString());
            }
            return previewList;
        }

        [StateItemDesc("SearchPrefs", "PageSize", StateItemStore.Cookie, StatePersistenceDuration.NeverExpire)]
        public ItemsPerPage PageSize
        {
            get { return _itemsPerPage; }
            set
            {
                _itemsPerPage = value;
                this.searchResultPager.PageSize = (int)_itemsPerPage;
                itemsPerPageList.SelectedValue = ((int) _itemsPerPage).ToString();
            }
        }

        public int CurrentPage
        {
            get
            {
                 return searchResultPager.PageIndex;
            }
            set { searchResultPager.PageIndex = value; }
        }

        public SearchSort SearchSortOption
        {
            get { return sortBlock.SortOption; }
            set { sortBlock.SortOption = value; }
        }
        public SearchSort? SearchSortOptionHidden
        {
            get { return sortBlock.HideSortOption; }
            set { sortBlock.HideSortOption = value; }
        }
    }
}
