using System;
using System.Collections.Generic;
using System.Text;
using Corbis.WebOrders.Contracts.V1;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.Presenters.OrderHistory.Interfaces
{
    public interface IPrintOrderHistory : IView
    {
        List<OrderHistoryEntry> Order{ set;}
    }
}
