using System;
using System.Collections.Generic;
using System.Text;

using Corbis.WebOrders.Contracts.V1;

namespace Corbis.Web.Entities
{
    public class OrderFee
    {
        public FeeType Type { get; set; }
        public int DisplayOrder { get; set; }
        public string DisplayText { get; set; }
        public string DisplayAmount { get; set; }
    }
}
