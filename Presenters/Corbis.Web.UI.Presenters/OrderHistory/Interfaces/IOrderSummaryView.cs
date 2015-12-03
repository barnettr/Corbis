using System;
using System.Collections.Generic;
using System.Text;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Entities;
using Corbis.WebOrders.Contracts.V1;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.Membership.Contracts.V1;

namespace Corbis.Web.UI.OrderHistory.Interfaces
{
    public interface IOrderSummaryView : IView
    {
        Guid OrderUid { get; set; }
		List<DownloadableImage> ImagesToDownload { get; set; }
        string OrderEmail { set; }
        string OrderNumber { get; set; }
		DateTime OrderDate { set; }
		List<MediaLicenseDetail> LicenseDetails { set; }
		string CurrencyCode { get; set; }
		bool PerContract { set; }

		string ProjectName { set; }
		string JobNumber { set; }
		string PoNumber { set; }
		string Licensee { set; }

		string DeliveryEmails { set; }
		DeliveryMethod OrderDelivery { set; }

		Corbis.WebOrders.Contracts.V1.PaymentMethod OrderPayment { set; }
		MemberAddress BillingAddress { set; }
		CreditCard CreditCardInformation { set; }
		string CorporateAccountName { set; }

		string SubTotal { set; }
		string PromotionValue { set; }
        List<OrderFee> Fees { set; }
		string Tax { set; }
		string Shipping { set; }
		string Total { set; }

        // catch error
        bool ShowError { set; }
    }
}
