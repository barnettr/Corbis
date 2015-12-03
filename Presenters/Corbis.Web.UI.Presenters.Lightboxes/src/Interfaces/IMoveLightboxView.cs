using System;
using System.Collections.Generic;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Search.Contracts.V1;
using Corbis.Web.UI.ViewInterfaces;


namespace Corbis.Web.UI.Lightboxes.ViewInterfaces
{
    public interface IMoveLightboxView : IView
    {
        int LightboxIdToMove {get; }
        int? NewParentLightboxId { get;  }
        List<Lightbox> Lightboxes {set; }
        LightboxTreeSort LightboxTreeSortBy { get; }
    }
}
