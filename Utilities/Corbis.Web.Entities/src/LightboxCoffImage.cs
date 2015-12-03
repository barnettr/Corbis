using System;
using System.Collections.Generic;
using System.Text;
using Corbis.LightboxCart.Contracts.V1;

namespace Corbis.Web.Entities
{
    [Serializable]
    public class LightboxCoffImage : LightboxCopyImage
    {
        public string LicenseModelText { get; set; }
    }
}
