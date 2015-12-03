using System;
using System.Collections.Generic;
using System.Text;
using Corbis.CommonSchema.Contracts.V1.Image;

namespace Corbis.Web.Entities.src
{
    [Serializable]
    public class COFFCheckoutItem
    {
        public Guid ProductUid{ get; set; }
        public FileSize FileSize { get; set; }
        public string CorbisId { get; set; }
    }
}