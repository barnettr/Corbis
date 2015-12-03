using System.Collections.Generic;
using Corbis.Web.UI.Presenters.Accounts;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Entities;
using Corbis.Membership.Contracts.V1;

namespace Corbis.Web.UI.Accounts.ViewInterfaces
{
    public interface ICreditInformationView : IView
    {
        CreditCard CreditCard { get; set; }
        List<CreditCard> CreditCards { get; set; }
        List<ContentItem> CardTypes { get; set; }
        string CardTypeDisplayText { get; set; }
        string CardNumber { get; set; }
        string CardNumberDisplayText { get; set; }
        string CardMonth { get; set; }
        string CardYear { get; set; }
        string CardHolderName { get; set; }
        CreditCardUsageType CreditCardUsageType { get; set; }
        bool DisplayCardListSection { get; set; }
        bool DisplayCardNumberDisplayText { get; set; }
        bool DisplayCardTypeDisplayText { get; set; }
        bool DisplayCardTypeList { get; set; }
        bool DisplayCardNumber { get; set; }
    }
}
