using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Corbis.Web.Entities;
using Corbis.Web.UI.Navigation;
using Corbis.Web.Utilities.StateManagement;

namespace Corbis.Web.UI.OrderHistory
{
    public partial class OrderHistoryHeader : UserControl
    {
        private ItemsPerPage _itemsPerPage;
        private bool _showHeader = true;
        public event PageCommandDelegate PageCommand;
        public event PageSizeCommandDelegate PageSizeCommand;
        public event GenericCommandDelegate GenericCommand;
        private StateItemCollection stateItems;

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

        public void UpdateSearchTitle(int currentCount, int total)
        {
            string title = string.Empty;
            if(totalSearchHitCount > (int) this.PageSize)
            {
                title = string.Format((string)GetLocalResourceObject("counterInfo"), searchResultPager.StartingRecord, currentCount + this.searchResultPager.StartingRecord - 1 , total);
            }
            else
            {
                if (totalSearchHitCount == 0)
                {
                    title = string.Format((string)GetLocalResourceObject("zeroItemCounter"), 0);
                }
                else if (totalSearchHitCount == 1)
                {
                    title =
                    string.Format((string)GetLocalResourceObject("countersingleInfo"), this.searchResultPager.StartingRecord,
                                  totalSearchHitCount, total);
                }
                else
                {
                    title =
                    string.Format((string)GetLocalResourceObject("counterInfo"), this.searchResultPager.StartingRecord,
                                  totalSearchHitCount, total);
                }

            }
            indexInfo.Text = title;
        }

        int totalSearchHitCount;
        public int TotalSearchHitCount
        {
            set 
            {
                totalSearchHitCount = value;
                UpdateSearchTitle(currentPageHitCount, totalSearchHitCount);
                searchResultPager.TotalRecords = totalSearchHitCount;
            }
        }

        int currentPageHitCount;
        public int CurrentPageHitCount
        {
            set
            {
                currentPageHitCount = value;
                UpdateSearchTitle(currentPageHitCount, totalSearchHitCount);
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
            if (!IsPostBack)
            {
                StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);

                itemsPerPageList.DataSource = GetPageSizeList();
                itemsPerPageList.CssClassField = "Value";
                itemsPerPageList.ValueField = "Key";
                if ((int)_itemsPerPage != 0)
                    itemsPerPageList.SelectedValue = ((int)_itemsPerPage).ToString();
                itemsPerPageList.DataBind();
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
    }
}
