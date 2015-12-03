using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.Entities.src
{
    public class COFFValidationResponse
    {
        public List<ValidatedCoffItem> ValidatedCoffItem { get; set; }
        public int InvalidItemCount { get; set; }
        public bool ValidationStatus { get; set; }
        public string CheckoutUrl { get; set; }
        public bool AllItemsInvalid { get; set; }
    }
}