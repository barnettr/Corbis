using System;
using System.Collections.Generic;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.WebOrders.Contracts.V1;
using Corbis.Web.UI.OrderHistory.Interfaces;
using Corbis.WebOrders.ServiceAgents.V1;
using Corbis.Web.UI.Presenters.OrderHistory.Interfaces;
using Contract = Corbis.WebOrders.Contracts.V1;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Framework.Globalization;
using Corbis.Web.Utilities;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Corbis.Web.Entities;
using System.Globalization;

namespace Corbis.Web.UI.Presenters.OrderHistory
{
	public class OrderHistoryPresenter : BasePresenter
	{
		private IOrderHistoryView view;
        private IOrderSummaryView orderSummaryView;
	    private IPrintOrderHistory printView;
		private Contract.IWebOrdersContract webOrderAgent;
        private StateItemCollection stateItems;


		public OrderHistoryPresenter(IOrderHistoryView view): this(view, new WebOrdersServiceAgent())
		{
		}

        public OrderHistoryPresenter(IOrderHistoryView view, Contract.IWebOrdersContract webOrderAgent)
		{
			if (view == null)
			{
				throw new ArgumentNullException("View object must not be null");
			}
			if (webOrderAgent == null)
			{
				throw new ArgumentNullException("Orders service agent must not be null");
			}

			this.view = view;
			this.webOrderAgent = webOrderAgent;
            stateItems = new StateItemCollection(System.Web.HttpContext.Current);
            LoadPreferences();
		}

        public OrderHistoryPresenter(IPrintOrderHistory  view)
            : this(view, new WebOrdersServiceAgent())
        {
        }

        public OrderHistoryPresenter(IPrintOrderHistory view, Contract.IWebOrdersContract webOrderAgent)
        {
            if (view == null)
            {
                throw new ArgumentNullException("View object must not be null");
            }
            if (webOrderAgent == null)
            {
                throw new ArgumentNullException("Orders service agent must not be null");
            }

            this.printView = view;
            this.webOrderAgent = webOrderAgent;
        }

        public OrderHistoryPresenter(IOrderSummaryView view)
            : this(view, new WebOrdersServiceAgent())
        {
        }

        public OrderHistoryPresenter(IOrderSummaryView view, Contract.IWebOrdersContract webOrderAgent)
        {
            if (view == null)
            {
                throw new ArgumentNullException("View object must not be null");
            }
            if (webOrderAgent == null)
            {
                throw new ArgumentNullException("Orders service agent must not be null");
            }

            this.orderSummaryView = view;
            this.webOrderAgent = webOrderAgent;
        }

		public void GetOrderHistory(bool includeSummary)
		{
			try
			{
				//If user is not anonymous and we have a member uid
				if (Profile.IsAuthenticated && Profile.MemberUid != Guid.Empty)
				{
					Contract.OrderHistory orderHistory = webOrderAgent.GetOrderHistoryPaged(Profile.MemberUid, view.PageIndex, (int) view.PageSize, view.SortOrdersBy, includeSummary);

                    if(orderHistory.Orders.Count == 0)
                    {
                        view.ShowBlankOrders = true;
                    }
                    if(this.printView != null)
                    {
                        printView.Order = orderHistory.Orders;
                    }

				    view.ExpiredCount = orderHistory.ExpiredItemCount;
					view.ExpiringCount = orderHistory.ExpiringItemCount;
					view.TotalRecords = orderHistory.OrderCount;
				
                    foreach (OrderHistoryEntry entry in orderHistory.Orders)
                    {
                        entry.OrderTotal = string.Format("{0}",CurrencyHelper.GetLocalizedCurrency(entry.OrderTotal));                      

                    }

                    foreach (OrderHistoryEntry order in orderHistory.Orders)
                    {
                        if (order.ContractTypeId == 1 || order.ContractTypeId == 2)
                        {
                            order.OrderTotal = (string)HttpContext.GetGlobalResourceObject("Resource", "AsPerContract");
                            order.CurrencyCode = string.Empty;
                        }
                    }

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
                    Contract.OrderHistory orderHistory = webOrderAgent.GetOrderHistoryPaged(Profile.MemberUid, pageIndex, pageSize, sorting, true);
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
                        view.PageSize = ItemsPerPage.items50;
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

        public void GetOrderHistorySummaryDetails()
        {
            try
            {
                OrderConfirmationDetails details = webOrderAgent.GetOrderHistorySummaryDetails(
                    Profile.UserName,
                    orderSummaryView.OrderUid,
                    Profile.CountryCode,
                    Language.CurrentLanguage.LanguageCode);

                if (details.Images != null && details.Images.Count > 0)
                {
                    orderSummaryView.ImagesToDownload = details.Images;
                    orderSummaryView.OrderEmail = details.OrderSummary.Delivery.ConfirmationEmailAddresses;
                    orderSummaryView.OrderNumber = details.OrderNumber;
                    orderSummaryView.CurrencyCode = details.OrderSummary.Costs.CurrencyCode;

                    orderSummaryView.OrderDate = details.OrderDate;

                    orderSummaryView.ProjectName = details.OrderSummary.Project.Name;
                    orderSummaryView.JobNumber = details.OrderSummary.Project.JobNumber;
                    orderSummaryView.PoNumber = details.OrderSummary.Project.PONumber;
                    orderSummaryView.Licensee = details.OrderSummary.Project.Licensee;

                    orderSummaryView.DeliveryEmails = details.OrderSummary.Delivery.ConfirmationEmailAddresses;
                    orderSummaryView.OrderDelivery = details.OrderSummary.Delivery.NonRfcdMethod;

                    orderSummaryView.OrderPayment = details.OrderSummary.Payment.Method;
                    orderSummaryView.BillingAddress = details.OrderSummary.BillingAddress;
                    if (details.OrderSummary.Payment.Method == Corbis.WebOrders.Contracts.V1.PaymentMethod.CorporateAccount)
                    {
                        orderSummaryView.CorporateAccountName = details.OrderSummary.Payment.CorporateAccountName;
                    }
                    if (details.OrderSummary.Payment.Method == Corbis.WebOrders.Contracts.V1.PaymentMethod.CreditCard)
                    {
                        details.OrderSummary.CreditCardInformation.CreditCardTypeCode = details.OrderSummary.CreditCardInformation.CreditCardType;
                      
                        orderSummaryView.CreditCardInformation = details.OrderSummary.CreditCardInformation;

                    }

                    if (details.OrderSummary.PurchasingContract == ContractType.Purchasing)
                    {
                        orderSummaryView.PerContract = true;
                    }
                    else
                    {
                        orderSummaryView.SubTotal = GetTotalDisplay(details.OrderSummary.Costs.Subtotal, details.OrderSummary.Costs.CurrencyCode);
                        orderSummaryView.PromotionValue = GetTotalDisplay(details.OrderSummary.Costs.PromoDiscount, details.OrderSummary.Costs.CurrencyCode);

                        // Get any fees
                        if (details.OrderSummary.Costs.Fees != null && details.OrderSummary.Costs.Fees.Count > 0)
                        {
                            List<OrderFee> orderFees = new List<OrderFee>();
                            foreach (Fee fee in details.OrderSummary.Costs.Fees)
                            {
                                OrderFee orderFee = new OrderFee();
                                orderFee.Type = fee.Type;
                                orderFee.DisplayAmount = GetTotalDisplay(fee.Amount, details.OrderSummary.Costs.CurrencyCode);
                                orderFees.Add(orderFee);
                            }
                            orderSummaryView.Fees = orderFees;
                        }
                        orderSummaryView.Shipping = GetTotalDisplay(details.OrderSummary.Costs.ShippingCost, details.OrderSummary.Costs.CurrencyCode);
                        orderSummaryView.Tax = GetTotalDisplay(details.OrderSummary.Costs.TaxOrVat, details.OrderSummary.Costs.CurrencyCode);
                        orderSummaryView.Total = GetTotalDisplay(details.OrderSummary.Costs.TotalCost, details.OrderSummary.Costs.CurrencyCode);
                    }

                    orderSummaryView.LicenseDetails = details.LicenseDetails;
                }
            }
            catch (Exception ex)
            {
                orderSummaryView.ShowError = true;
                HandleException(ex, orderSummaryView.LoggingContext, "GetOrderHistorySummaryDetails");
            }
        }

        public void SavePreferences()
        {
            stateItems.SaveObjectToState(view);
        }

        private string GetTotalDisplay(double totalValue, string currencyCode)
        {
            return String.Format("{0} ({1})", CurrencyHelper.GetLocalizedCurrency(totalValue.ToString(CultureInfo.InvariantCulture)), currencyCode);
        }


	}
}
