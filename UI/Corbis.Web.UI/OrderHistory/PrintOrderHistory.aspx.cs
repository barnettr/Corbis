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
using Corbis.Framework.Logging;
using Corbis.WebOrders.Contracts.V1;
using Corbis.Web.UI.OrderHistory.Interfaces;
using Corbis.Web.UI.Presenters.OrderHistory;
using Corbis.Web.UI.Presenters.OrderHistory.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;

namespace Corbis.Web.UI.OrderHistory
{
 
    public partial class PrintOrderHistory : CorbisBasePage ,IPrintOrderHistory
    {
        private OrderHistoryPresenter presenter;

        public List<OrderHistoryEntry> Order
        {
            set 
            {
                this.ordersRepeater.DataSource = value;
                this.ordersRepeater.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            presenter = new OrderHistoryPresenter(this);
            int pageIndex=1, pageSize=10;
            int.TryParse(Request.QueryString["pageIndex"], out pageIndex);
            int.TryParse(Request.QueryString["pageSize"], out pageSize);
            string sortOrder = Request.QueryString["sortOrder"];

            SortOrder sorat = (SortOrder)Enum.Parse(typeof (SortOrder), sortOrder);

            presenter.GetOrderHistory(pageIndex, pageSize,sorat);

            
        }
    }
}
