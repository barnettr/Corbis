using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.Authentication;
using Corbis.Web.Entities;
using Corbis.Web.Utilities.StateManagement;

namespace Corbis.Web.UI.Search
{
    public partial class SortBlock : System.Web.UI.UserControl
    {
        private SearchSort _sortOption;
        private StateItemCollection stateItems = new StateItemCollection(System.Web.HttpContext.Current);

        public event GenericCommandDelegate GenericCommand;
        //public SearchSort SortOption { get; set; }
        public SearchSort SortOption
        {
            get
            {
                GetSearchSortOptionFromCookie();
                return _sortOption;
            }
            set
            {
                ((CorbisBasePage)Page).AnalyticsData["prop8"] = value.ToString();
                
                _sortOption = value;
                StateItem<int> searchSortOption = new StateItem<int>(SearchKeys.Name, SearchKeys.SortOptionCookieKey, (int)_sortOption,
                                                     StateItemStore.Cookie, StatePersistenceDuration.NeverExpire);
                stateItems.SetStateItem(searchSortOption);
                InitializeControl();
            }
        }

        public SearchSort? HideSortOption { get; set; }

        private List<DisplayValue<SearchSort>> GetSortOptionList()
        {
            bool canSearchOutline = Profile.Current.Permissions.Contains(Permission.HasPermissionSearchOutline);
            List<DisplayValue<SearchSort>> list = CorbisBasePage.GetEnumDisplayValues<Corbis.CommonSchema.Contracts.V1.SearchSort>();
            List<DisplayValue<SearchSort>>  returnList = new List<DisplayValue<SearchSort>>();

            foreach (DisplayValue<SearchSort> item in list)
            {
                if (item.Value == SearchSort.DatePublished && !canSearchOutline)
                {
                    continue;
                }
                else if (item.Value == SearchSort.Unknown)
                {
                    continue;
                }
                else
                {

                    returnList.Add(item);
                }

            }
            return returnList;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (true)
            {
                InitializeControl();
            }
            GetSearchSortOptionFromCookie();

        }

        private void InitializeControl()
        {

            GetSearchSortOptionFromCookie();

            sortOptionList.DataSource = GetSortOptionList();
            for (int i = 0; i < GetSortOptionList().Count; i++)
            {
                if (GetSortOptionList()[i].Value == SortOption)
                {
                    sortOptionList.SelectedIndex = i;
                    break;
                }

            }
            sortOptionList.DataBind();
            sortOptionText.Text = CorbisBasePage.GetEnumDisplayText<SearchSort>(SortOption);
        }

        protected void sortOptionList_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            sortOptionList.DataSource = GetSortOptionList();
            sortOptionList.SelectedIndex = e.NewSelectedIndex;
            int kkk = (int)sortOptionList.SelectedValue;
            _sortOption = (SearchSort)Enum.Parse(typeof(SearchSort), sortOptionList.SelectedValue.ToString());
            sortOptionList.DataBind();
            StateItem<int> searchSortOption = new StateItem<int>(SearchKeys.Name, SearchKeys.SortOptionCookieKey, (int)_sortOption,
                                                                 StateItemStore.Cookie, StatePersistenceDuration.NeverExpire);
            stateItems.SetStateItem(searchSortOption);
            if (GenericCommand != null)
            {
                sortOptionText.Text = GetSortOptionList()[e.NewSelectedIndex].Text;
                CommandEventArgs arg = new CommandEventArgs("SearchSort", (SearchSort)sortOptionList.SelectedValue);
                SortOption = (SearchSort)sortOptionList.SelectedValue;
                GenericCommand(this, arg);
            }
        }

        private void GetSearchSortOptionFromCookie()
        {
            int abc = stateItems.GetStateItemValue<int>(SearchKeys.Name, SearchKeys.SortOptionCookieKey, StateItemStore.Cookie);

            if (abc == (int)SearchSort.Unknown && _sortOption != SearchSort.DatePublished && Profile.Current.Permissions.Contains(Permission.HasPermissionSearchOutline))
            {
                _sortOption = SearchSort.DatePublished;
            }
            else if (abc == (int)SearchSort.Unknown && _sortOption == SearchSort.DatePublished && Profile.Current.Permissions.Contains(Permission.HasPermissionSearchOutline))
            {
                // do nothing
            }

            else if(abc != (int)SearchSort.Unknown && Enum.IsDefined(typeof(SearchSort), abc)) 
            {

                _sortOption = (SearchSort)Enum.Parse(typeof(SearchSort), abc.ToString());
            }
            else
            {

                _sortOption = SearchSort.Relevancy;
            }

        }

    }
}
