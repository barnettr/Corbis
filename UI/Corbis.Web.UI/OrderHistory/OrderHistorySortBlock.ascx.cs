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
using Corbis.WebOrders.Contracts.V1;

namespace Corbis.Web.UI.OrderHistory
{
    public partial class OrderHistorySortBlock : System.Web.UI.UserControl
    {
        private SortOrder _sortOption;
        private StateItemCollection stateItems = new StateItemCollection(System.Web.HttpContext.Current);

        public event GenericCommandDelegate GenericCommand;
        //public SearchSort SortOption { get; set; }
        public SortOrder SortOption
        {
            get
            {
                GetSearchSortOptionFromCookie();
                return _sortOption;
            }
            set
            {
                _sortOption = value;
                StateItem<int> searchSortOption = new StateItem<int>("OrderHistorySortOption", "OrderHistorySortKey", (int)_sortOption,
                                                     StateItemStore.Cookie, StatePersistenceDuration.NeverExpire);
                stateItems.SetStateItem(searchSortOption);
                InitializeControl();
            }
        }

        public SearchSort? HideSortOption { get; set; }

        private List<DisplayValue<SortOrder>> GetSortOptionList()
        {
            List<DisplayValue<SortOrder>> list = CorbisBasePage.GetEnumDisplayValues<SortOrder>();
            List<DisplayValue<SortOrder>> returnList = new List<DisplayValue<SortOrder>>();

            foreach (DisplayValue<SortOrder> item in list)
            {
                if (item.Value == SortOrder.None)
                {
                    continue;
                }

                returnList.Add(item);
            }
            return returnList;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
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
            sortOptionText.Text = CorbisBasePage.GetEnumDisplayText<SortOrder>(SortOption);
        }

        protected void sortOptionList_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            sortOptionList.DataSource = GetSortOptionList();
            sortOptionList.SelectedIndex = e.NewSelectedIndex;
            int kkk = (int)sortOptionList.SelectedValue;
            _sortOption = (SortOrder)Enum.Parse(typeof(SortOrder), sortOptionList.SelectedValue.ToString());
            sortOptionList.DataBind();
            StateItem<int> searchSortOption = new StateItem<int>("OrderHistory", "sort", (int)_sortOption,
                                                                 StateItemStore.Cookie, StatePersistenceDuration.NeverExpire);
            stateItems.SetStateItem(searchSortOption);
            if (GenericCommand != null)
            {
                sortOptionText.Text = GetSortOptionList()[e.NewSelectedIndex].Text;
                CommandEventArgs arg = new CommandEventArgs("SearchSort", (SearchSort)sortOptionList.SelectedValue);
                SortOption = (SortOrder)sortOptionList.SelectedValue;
                GenericCommand(this, arg);
            }
        }

        private void GetSearchSortOptionFromCookie()
        {
            int abc = stateItems.GetStateItemValue<int>("OrderHistory", "sort", StateItemStore.Cookie);

            if(abc != (int)SortOrder.None && Enum.IsDefined(typeof(SortOrder), abc))
            {
                _sortOption = (SortOrder)Enum.Parse(typeof(SortOrder), abc.ToString());
            }
            else
            {
                _sortOption = SortOrder.LicenseExpiration;
            }

        }

    }
}
