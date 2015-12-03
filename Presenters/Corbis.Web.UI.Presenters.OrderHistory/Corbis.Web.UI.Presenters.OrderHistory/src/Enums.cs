using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.UI.Presenters.OrderHistory
{
    public enum OrdersView
    {
        NotSet = 0,
        ListDisplay = 1,
        ThumbNailDisplay = 2
    }

    public enum OrdersPerPage
    {
        NotSet = 0,
        OrdersPerPage20 = 20,
        OrdersPerPage40 = 40,
        OrdersPerPage80 = 80,
        OrdersPerPage100 = 100
    }
}
