using System;
using System.Collections.Generic;
using Corbis.WebOrders.Contracts.V1;
using Corbis.Web.UI.OrderHistory.Interfaces;
using Corbis.WebOrders.ServiceAgents.V1;
using Corbis.Web.UI.Presenters.OrderHistory.Interfaces;
using Contract = Corbis.WebOrders.Contracts.V1;
using Corbis.Web.Utilities.StateManagement;

using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;

namespace Corbis.Web.UI.Presenters.OrderHistory
{
	public class OrderHistoryPresenter : BasePresenter
	{
		private IOrderHistoryView view;
	    private IPrintOrderHistory printView;
		private Contract.IWebOrdersContract ordersAgent;
        private StateItemCollection stateItems;


		public OrderHistoryPresenter(IOrderHistoryView view): this(view, new WebOrdersServiceAgent())
		{
		}

        public OrderHistoryPresenter(IOrderHistoryView view, Contract.IWebOrdersContract ordersAgent)
		{
			if (view == null)
			{
				throw new ArgumentNullException("View object must not be null");
			}
			if (ordersAgent == null)
			{
				throw new ArgumentNullException("Orders service agent must not be null");
			}

			this.view = view;
			this.ordersAgent = ordersAgent;
            stateItems = new StateItemCollection(System.Web.HttpContext.Current);
            LoadPreferences();
		}

        public OrderHistoryPresenter(IPrintOrderHistory  view)
            : this(view, new WebOrdersServiceAgent())
        {
        }

        public OrderHistoryPresenter(IPrintOrderHistory view, Contract.IWebOrdersContract ordersAgent)
        {
            if (view == null)
            {
                throw new ArgumentNullException("View object must not be null");
            }
            if (ordersAgent == null)
            {
                throw new ArgumentNullException("Orders service agent must not be null");
            }

            this.printView = view;
            this.ordersAgent = ordersAgent;
        }

		public void GetOrderHistory(bool includeSummary)
		{
			try
			{
				//If user is not anonymous and we have a member uid
				if (Profile.IsAuthenticated && Profile.MemberUid != Guid.Empty)
				{
					Contract.OrderHistory orderHistory = ordersAgent.GetOrderHistoryPaged(Profile.MemberUid, view.PageIndex, (int) view.PageSize, view.SortOrdersBy, includeSummary);
                                        
                    if(orderHistory.Orders.Count == 0)
                    {
                        view.ShowBlankOrders = true;
                    }
                    if(this.printView != null)
                    {
                        printView.Order = orderHistory.Orders;
                    }

                    foreach (OrderHistoryEntry order in orderHistory.Orders)
                    {
                        if (order.ContractTypeId == 1 || order.ContractTypeId == 2)
                        {
                            order.OrderTotal = (string)HttpContext.GetGlobalResourceObject("Resource", "AsPerContract");
                        }   order.CurrencyCode = string.Empty;                  
                    }
				    view.ExpiredCount = orderHistory.ExpiredItemCount;
					view.ExpiringCount = orderHistory.ExpiringItemCount;
					view.TotalRecords = orderHistory.OrderCount;
					view.Orders = orderHistory.Orders;
                    view.DisplayOrderRecordsTitle();
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, view.LoggingContext, "OrderHistoryPresenter: GetOrderHistory()");
				throw;
			}
		}

        public void GetOrderHistory(int pageIndex, int pageSize, SortOrder sorting)
        {
            try
            {
                //If user is not anonymous and we have a member uid
                if (Profile.IsAuthenticated && Profile.MemberUid != Guid.Empty)
                {
                    Contract.OrderHistory orderHistory = ordersAgent.GetOrderHistoryPaged(Profile.MemberUid, pageIndex, pageSize, sorting, true);
                    printView.Order = orderHistory.Orders;

                }
            }
            catch (Exception ex)
            {
                HandleException(ex, view.LoggingContext, "OrderHistoryPresenter: GetOrderHistory()");
                throw;
            }
        }

        public void LoadPreferences()
        {
            List<string> propertiesNotSet = stateItems.PopulateObjectFromState(view);
            //Set default values for Properties not found in state
            foreach (string propertyName in propertiesNotSet)
            {
                switch (propertyName)
                {
                    case "PageSize" :
                        view.PageSize = OrdersPerPage.OrdersPerPage20;
                        break;

                    case "SortOrdersBy":
                        view.SortOrdersBy = SortOrder.LicenseExpiration;
                        break;

                    case "PageView":
                        view.PageView = OrdersView.ListDisplay;
                        break;

                    default:
                        // N/A
                        break;
                }
            }
        }

        public void SavePreferences()
        {
            stateItems.SaveObjectToState(view);
        }

	}
}
