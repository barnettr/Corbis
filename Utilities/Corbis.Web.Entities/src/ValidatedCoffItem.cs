using System;
using System.Collections.Generic;
using System.Text;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Image.Contracts.V1;
using Corbis.CommonSchema.Contracts.V1.Image;

namespace Corbis.Web.Entities
{
    public class ValidatedCoffItem
    {
        public List<ItemAvailableSize> AvailableSizes { get; set; }
        public CoffValidationResult CoffValidationResult { get; set;}
        public decimal AspectRatio { get; set; }
        public string LicenseModelText { get; set; }
    }
    public class ItemAvailableSize
    {
        public string Size { get; set; }
        public string LocalizedValue{ get; set; }
    }
}