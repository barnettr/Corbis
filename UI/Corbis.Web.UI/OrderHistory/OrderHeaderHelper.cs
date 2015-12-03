using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.Web.Entities;
using Corbis.Web.UI.Navigation;

namespace Corbis.Web.UI.OrderHistory
{
    public enum ThumbnailLayout { Thumbnail, ThumbnailLabels, ThumbnailDetails }
    public enum ThermClarification { On, Off }
    public enum SortType { SortByBestMatch, SortByBlah }
    public enum SearchItemPreview
    {
        previewOn = 0, 
        previewOff = 1
    }


    // Summary:
    //     Provides data for the Pager event.
    public class PageSizeEventArgs : EventArgs
    {
        private ItemsPerPage _itemsPerPage = ItemsPerPage.items50;

        public PageSizeEventArgs(PageSizeEventArgs e)
        {
            this.PageSize = e.PageSize;
        }

        public PageSizeEventArgs(ItemsPerPage itemsPerPage)
        {
            this.PageSize = itemsPerPage;
        }

        #region Event properties

        public ItemsPerPage PageSize
        {
            get { return _itemsPerPage; }
            set { _itemsPerPage = value; }
        }

        #endregion
    }
    public delegate void PageCommandDelegate(object sender, PagerEventArgs e);
    public delegate void PageSizeCommandDelegate(object sender, PageSizeEventArgs e);
    public delegate void PreviewCommandDelegate(object sender, CommandEventArgs e);

    /// <summary>
    /// this is a generic solution. We will remove PreviewCommanddelegate later
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void GenericCommandDelegate(object sender, CommandEventArgs e);

    //public class HeaderFooterBase:System.Web.UI.UserControl
    //{
    //    public delegate void PageCommandDelegate(object sender, PagerEventArgs e);
    //    public event PageCommandDelegate PageCommand;     
   
    //}
}
