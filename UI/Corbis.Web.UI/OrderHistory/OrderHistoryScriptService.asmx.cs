using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Web.Script.Services;
using Corbis.Framework.Logging;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Utilities.StateManagement;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.WebOrders.Contracts.V1;
using Corbis.Web.UI.OrderHistory.Interfaces;
using Corbis.Web.UI.Presenters.OrderHistory.Interfaces;
using Contract = Corbis.WebOrders.Contracts.V1;
using Corbis.Web.Authentication;
using Corbis.Web.UI.Presenters.OrderHistory;
using Corbis.Web.Utilities;
using Corbis.Framework.Globalization;

namespace Corbis.Web.UI.OrderHistory
{
    /// <summary>
    /// Summary description for OrderHistoryScriptService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    [ToolboxItem(false)]
    public class OrderHistoryScriptService : Corbis.Web.UI.src.CorbisWebService, IView, IPrintOrderHistory
    {
        private ILogging loggingContext;
        private OrderHistoryPresenter orderHistoryPresenter;

        public OrderHistoryScriptService()
        {
            orderHistoryPresenter = new OrderHistoryPresenter(this);
        }

		[WebMethod(true)]
		public List<KeyValuePair<string, string>> GetProjectList(SortOrder sortOrdersBy)
		{
            List<KeyValuePair<string, string>> returnList = new List<KeyValuePair<string, string>>();

            // Fix for 15541 requires only first 50 items from order history results.
            orderHistoryPresenter.GetOrderHistory(1, 50, SortOrder.OrderDate);
            if (order != null)
            {
                foreach (OrderHistoryEntry orderEntry in order)
                {
                    returnList.Add(new KeyValuePair<string, string>(orderEntry.OrderDate.ToString(Language.CurrentCulture.DateTimeFormat.ShortDatePattern, Language.CurrentCulture.DateTimeFormat) + " - " + StringHelper.Truncate(orderEntry.ProjectName, 18) + " - #" + orderEntry.OrderNumber, orderEntry.OrderUid.ToString()));
                }
            }

            returnList.Add(new KeyValuePair<string, string>(HttpContext.GetLocalResourceObject("~/OrderHistory/OrderHistorySummary.aspx", "ReturnToMyOrder").ToString(), null));
            return returnList;            
		}

        #region IView Members

        public Corbis.Framework.Logging.ILogging LoggingContext
        {
            get
            {
                return loggingContext;
            }
            set
            {
                loggingContext = value;
            }
        }

        public System.Collections.Generic.IList<Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.ValidationDetail> ValidationErrors
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion


        #region IPrintOrderHistory Members
        private List<OrderHistoryEntry> order = null;
        public List<OrderHistoryEntry> Order
        {
            set { order = value; }
        }

        #endregion
    }
}
