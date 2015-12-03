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
using Corbis.Web.Entities;
using Corbis.Web.UI.Navigation;
using Corbis.Web.Utilities.StateManagement;

namespace Corbis.Web.UI.Search
{
    public partial class SearchResultFooter : System.Web.UI.UserControl
    {
        #region duplicated code from searchResultHeader, haven't figured out how to make it cleaner yet.

        private ItemsPerPage _itemsPerPage;
        public event PageCommandDelegate PageCommand;
        public int TotalRecords
        {
            get { return searchResultPager.TotalRecords; }
            set { searchResultPager.TotalRecords = value; }
        }
        public int CurrentPage
        {
            get { return searchResultPager.PageIndex; }
            set { searchResultPager.PageIndex = value; }
        }
        public int TotalPages
        {
            get { return searchResultPager.TotalPages; }
            //set { searchResultPager.TotalPages = value; }
        }
        public void UpdateSearchTitle(int currentCount, int total)
        {
            string title = string.Empty;
            if (totalSearchHitCount > (int) this.PageSize)
            {
                title =
                    string.Format((string) GetLocalResourceObject("counterInfo"), this.searchResultPager.StartingRecord,
                                  currentCount + this.searchResultPager.StartingRecord - 1, total);
            }
            else
            {
                if (totalSearchHitCount == 0)
                {
                    title =
                        string.Format((string) GetLocalResourceObject("zeroItemCounter"), 0);
                                  
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


            this.indexInfo.Text = title;
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

        int totalSearchHitCount = 0;
        public int TotalSearchHitCount
        {
            set
            {
                totalSearchHitCount = value;
                this.searchResultPager.TotalRecords = this.totalSearchHitCount;
                UpdateSearchTitle(this.currentPageHitCount, this.totalSearchHitCount);
                
            }
        }

        int currentPageHitCount = 0;
        public int CurrentPageHitCount
        {
            set
            {
                currentPageHitCount = value;
                UpdateSearchTitle(this.currentPageHitCount, this.totalSearchHitCount);
            }
        }
        protected void PageChanged(object sender, PagerEventArgs e)
        {
            PageCommand(this, e);
            //RebindTooltip();
        }

        public ItemsPerPage PageSize
        {
            get { return _itemsPerPage;  }
            set
            {
                _itemsPerPage = value;
                this.searchResultPager.PageSize = (int) _itemsPerPage;
            }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}