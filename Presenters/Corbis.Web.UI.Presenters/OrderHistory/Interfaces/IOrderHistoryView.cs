using System;
using System.Collections.Generic;
using Corbis.Web.Entities;
using Corbis.Web.Utilities.StateManagement;

using Corbis.Web.UI.ViewInterfaces;
using Corbis.WebOrders.Contracts.V1;

namespace Corbis.Web.UI.OrderHistory.Interfaces
{
	public interface IOrderHistoryView : IView
	{
		int ExpiredCount { set; }
		int ExpiringCount { set; }
		List<OrderHistoryEntry> Orders { set; }
        int PageIndex { get; set; }
        
        [StateItemDesc("OrderHistoryPrefs", "PageSize", StateItemStore.Cookie, StatePersistenceDuration.NeverExpire)]
        ItemsPerPage PageSize { get; set; }

        [StateItemDesc("OrderHistoryPrefs", "SortOrder", StateItemStore.Cookie, StatePersistenceDuration.NeverExpire)]
        SortOrder SortOrdersBy { get; set;}

        [StateItemDesc("OrderHistoryPrefs", "OrdersView", StateItemStore.Cookie, StatePersistenceDuration.NeverExpire)]
        Corbis.Web.UI.Presenters.OrderHistory.OrdersView PageView { get; set; }

        int TotalRecords { get; set; }
        bool ShowBlankOrders { set;}
	    void DisplayOrderRecordsTitle();
	}
}
