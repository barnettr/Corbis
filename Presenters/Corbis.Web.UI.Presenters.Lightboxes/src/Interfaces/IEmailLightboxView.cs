using System;
using System.Collections.Generic;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Search.Contracts.V1;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.Lightboxes.ViewInterfaces
{
    public interface IEmailLightboxView : IView
    {
        int LightboxId { get; set; }
        Guid LightboxUid { get; }
        int PageSize { get; set; }
        int TotalRecords { get; set; }
        int CurrentPageNumber { get; set; }
        string LightboxName { get; set; }
        string ModifiedDate { get; set; }
        string CreatedDate { get; set; }
        string OwnerUsername { get; set; }
        string Client { get; set; }
        string Notes { get; set; }
        List<LightboxDisplayImage> Products { get; set; }
    }
}
