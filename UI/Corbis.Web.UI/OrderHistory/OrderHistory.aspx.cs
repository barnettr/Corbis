using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Corbis.Web.UI.Presenters.OrderHistory;
using Corbis.Web.UI.OrderHistory.Interfaces;
using Corbis.Web.UI.Navigation;
using Corbis.Web.Entities;
using Corbis.Web.Utilities.StateManagement;
using Corbis.WebOrders.Contracts.V1;
using Label=System.Web.UI.WebControls.Label;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.OrderHistory
{
	public partial class OrderHistory : CorbisBasePage, IOrderHistoryView
	{
		private OrderHistoryPresenter presenter;
        private List<OrderHistoryEntry> orderList;
        private ItemsPerPage pageSize;
        private OrdersView pageView;
        private SortOrder sortOrdersBy;

		protected override void OnInit(EventArgs e)
		{
		    RequiresSSL = false;
			base.OnInit(e);
			presenter = new OrderHistoryPresenter(this);
            this.AddScriptToPage(SiteUrls.OrderHistoryScript, "orderHistoryScript");
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Tooltips, "TooltipsCSS");

			HookupEventHandlers();
		}

		private void HookupEventHandlers()
		{
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				GetRecords(true);
			}
		}

        public void OrderBound(object sender, RepeaterItemEventArgs e)
        {
            OrderHistoryEntry order = (OrderHistoryEntry)e.Item.DataItem;
            Label expirationLabel = (Label) e.Item.FindControl("ExpirationsLabel");
            Label expiringTextLabel = (Label)e.Item.FindControl("ExpiringTextMessage");
            Label orderDate = (Label)e.Item.FindControl("orderDate");            
            Label expiredTextLabel = (Label)e.Item.FindControl("ExpiredTextMessage");
            HyperLink ordersummaryLink = (HyperLink)e.Item.FindControl("hyperProjectName");
            HtmlGenericControl expiringImageDivControl = (HtmlGenericControl)e.Item.FindControl("expiringImageDiv");
            HtmlGenericControl expiredImageDivControl = (HtmlGenericControl)e.Item.FindControl("expiredImageDiv");
            Label totalLabel = (Label)e.Item.FindControl("TotalLabel");
            if (orderDate != null && !string.IsNullOrEmpty(orderDate.Text))
            {
                orderDate.Text = Convert.ToDateTime(orderDate.Text.ToString()).ToShortDateString();
            }
            if (e.Item.ItemType != ListItemType.Separator)
            {
                ordersummaryLink.NavigateUrl = String.Format("{0}?{1}={2}",
                    SiteUrls.OrderHistorySummary,
                    OrderKeys.OrderUid,
                    order.OrderUid);
                if (!string.IsNullOrEmpty(order.ProjectName))
                {
                    ordersummaryLink.Text = Server.HtmlEncode(order.ProjectName.ToString());
                }
                else
                {
                    ordersummaryLink.Text = GetLocalResourceObject("project.Text").ToString() + Server.HtmlEncode(orderDate.Text);
                }
                if (order.ExpiredCount <= 0)
                {
                    expiredImageDivControl.Visible = false;
                }
                else
                {
                    expirationLabel.Visible = true;
                    expiredImageDivControl.Visible = true;
                    expiredTextLabel.Text = order.ExpiredCount + GetLocalResourceObject("Expired").ToString();
                }

                if (order.ExpiringCount <= 0)
                {
                    expiringImageDivControl.Visible = false;
                }
                else
                {
                    expirationLabel.Visible = true;
                    expiringImageDivControl.Visible = true;
                    expiringTextLabel.Text = order.ExpiringCount + GetLocalResourceObject("Expiring").ToString();
                }
            }
        }
        
		public void ViewChanged(object sender, CommandEventArgs e)
		{
			//TODO:redirect to correct thumbnail view page.
            
            // Save preference
            this.PageView = (OrdersView)Enum.Parse(typeof(OrdersView), e.CommandArgument.ToString());
            presenter.SavePreferences();

            Response.Redirect("OrderHistory.aspx");
		}

		public void PageChanged(object sender, PagerEventArgs e)
		{
			this.PageIndex = e.PageIndex;
			GetRecords(true);
		}

		public void PageSizeChanged(object sender, PageSizeEventArgs e)
		{
            PageSize = e.PageSize;
            // Save preference
            presenter.SavePreferences();
            //Reset page to first one
			this.PageIndex = 1;
			GetRecords(true);
		}

        protected void SortOrderChanged(Object sender, CommandEventArgs e)
	    {
            //Reset page to first one
            this.PageIndex = 1;
            // Save preference
            SortOrdersBy = (SortOrder)e.CommandArgument;
            presenter.SavePreferences();
            GetRecords(true);
        }

		#region IOrderHistoryView

		private string expiringText = "";
		private int expirationCount;
		public int ExpiringCount
		{
			set 
			{
				if (value == 0)
				{
				    //this.expiringImageMessagePopup.Visible = false;
					expiringText = "";
				}
				else
				{
					expiringText = string.Format("<span class=\"expiringOrderText\">{0} {1}</span>", value.ToString(), (value==1? GetLocalResourceObject("expiringLicence").ToString(): GetLocalResourceObject("expiringLicences").ToString()));
				}

				expirationCount += value;
				SetExpirationText();
			}
		}

		private string expiredText = "";
		public int ExpiredCount
		{
			set
			{
				if (value == 0)
				{
				    //this.expiredImageMessagePopup.Visible = false;
					expiredText = "";
				}
				else
				{
					expiredText = string.Format("<span class=\"expiredOrderText\">{0} {1}</span>", value.ToString(), (value == 1 ? GetLocalResourceObject("expiredLicence").ToString() : GetLocalResourceObject("expiredLicences").ToString()));
				}

				expirationCount += value;
				SetExpirationText();
			}
		}

		public int TotalRecords
		{
			get { return this.orderHistoryHeader.TotalRecords; }
			set
			{
			    orderHistoryHeader.TotalSearchHitCount = value;
			    orderHistoryFooter.TotalSearchHitCount = value;
			}
		}

	    public bool ShowBlankOrders
	    {
	        set
	        {
                this.orderHistoryBlank.Visible = value;

                
	        }
	    }

	    public void DisplayOrderRecordsTitle()
	    {
	        if(Orders.Count == 0)
	        {
	            OrderRecoredTitle.Text = GetLocalResourceObject("BlankOrdersMessage").ToString();
	        }
            else
	        {
	            OrderRecoredTitle.Text = GetLocalResourceObject("OrderRecordsTitle").ToString();
	        }
	    }

        [StateItemDesc("OrderHistoryPrefs", "PageSize", StateItemStore.Cookie, StatePersistenceDuration.NeverExpire)]
        public ItemsPerPage PageSize
		{
            get { return pageSize; }
			set
			{
                pageSize = value;
			    this.orderHistoryHeader.PageSize = value;
			    this.orderHistoryFooter.PageSize = value;
			}
		}

        [StateItemDesc("OrderHistoryPrefs", "OrdersView", StateItemStore.Cookie, StatePersistenceDuration.NeverExpire)]
        public OrdersView PageView
        {
            get { return pageView; }
            set { pageView = value; }
        }

        [StateItemDesc("OrderHistoryPrefs", "SortOrder", StateItemStore.Cookie, StatePersistenceDuration.NeverExpire)]
        public SortOrder SortOrdersBy
        {
            get { return sortOrdersBy; }
            set { sortOrdersBy = value; }
        }
        
        public int PageIndex
		{
            get { return orderHistoryHeader.CurrentPage; }
			set
			{
			    orderHistoryHeader.CurrentPage = value;
			    orderHistoryFooter.CurrentPage = value;
			}
		}

        public List<OrderHistoryEntry> Orders
		{
            get { return orderList; }
		    set
			{
                orderList = value;
				this.ordersRepeater.DataSource = value;
				this.ordersRepeater.DataBind();

			    this.orderHistoryHeader.CurrentPageHitCount = value.Count;
			    this.orderHistoryFooter.CurrentPageHitCount = value.Count;
			}
		}


		#endregion

		#region Helper Methods

		private void GetRecords(bool includeSummary)
		{
			presenter.GetOrderHistory(includeSummary);
     //       SetPopUpVisible();
		}

        private void SetPopUpVisible()
        {
            SetExpiringPopupVisible();
            foreach (OrderHistoryEntry order in this.Orders)
            {
                if (order.ExpiredCount > 0 )
                {
                    //this.expiredImageMessagePopup.Visible = true;
                    break;
                }
                else
                {
                    //this.expiredImageMessagePopup.Visible = false;
                }
                
            }
        }

	    private void SetExpiringPopupVisible()
	    {
            foreach (OrderHistoryEntry order in this.Orders)
	        {
                if (order.ExpiringCount > 0)
                {
                    //this.expiringImageMessagePopup.Visible = true;
                    break;
                }
                else
                {
                    //this.expiringImageMessagePopup.Visible = false;
                }
	        }
	    }

	    private void SetExpirationText()
		{
			string expirationTextString = "";
			if (expirationCount > 0)
			{
				expirationTextString = String.Format(
					GetLocalResourceObject((expirationCount==1? "expirationWarning": "expirationsWarning")).ToString(),
					expiredText,
					(expiredText != "" && expiringText != "" ? " " + GetLocalResourceObject("and") + " ": ""),
					expiringText);
			}

			//this.expirationText.Text = Server.HtmlEncode(expirationTextString);
            this.expirationText.Text = expirationTextString;
		}

		private string GetRecordInfo(int startingRecord, int endingRecord, int totalRecords)
		{
            if (this.Orders.Count != 0)
            {
                return String.Format(GetLocalResourceObject("recordInfo").ToString(),
                                     startingRecord.ToString(),
                                     endingRecord.ToString(),
                                     totalRecords.ToString());
            }
            else
            {
                return String.Format(GetLocalResourceObject("BlankOrderRecordTitle").ToString());
            }
		}

		private Dictionary<int, string> GetSortOrderList()
		{
			Dictionary<int,string> sortOrderList = new Dictionary<int,string>();
			foreach(SortOrder sortOrderItem in Enum.GetValues(typeof(SortOrder)))
			{
				if (sortOrderItem != SortOrder.None)
				{
				    sortOrderList.Add((int) sortOrderItem, sortOrderItem.ToString());
				}
			}
			return sortOrderList;
		}


        private Dictionary<int, string> GetPageViewList()
		{
			Dictionary<int,string> pageViewList = new Dictionary<int,string>();
            foreach (OrdersView item in Enum.GetValues(typeof(OrdersView)))
			{
                if (item != OrdersView.NotSet)
				{
                    // TODO: Add Local Resources
                    pageViewList.Add((int)item, item.ToString());
				}
			}
            return pageViewList;
		}



        private Dictionary<int, string> GetPageSizeList()
        {
            Dictionary<int, string> pageSizeList = new Dictionary<int, string>();
            foreach (OrdersPerPage item in Enum.GetValues(typeof(OrdersPerPage)))
            {
                if (item != OrdersPerPage.NotSet)
                {
                    pageSizeList.Add((int)item, item.ToString());
                }
            }
            return pageSizeList;
        }


		#endregion

        private void TextDisplayChange()
        {
            if (ViewState["PageNumber"] != null && (int)ViewState["PageNumber"] == orderHistoryHeader.CurrentPage)
            {
                GetRecords(true);
            }
            else
            {
                ViewState["PageNumber"] = orderHistoryHeader.CurrentPage;
            }

        }


        public string PageNumberView
        {
            get
            {
                string pageNumberView=(string)ViewState["PageNumber"];
                if (string.IsNullOrEmpty(pageNumberView) )
                {

                    return pageNumberView;
                }
                else
                {

                    return "3";
                }

            }

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            TextDisplayChange();
        }

        

	}
}
