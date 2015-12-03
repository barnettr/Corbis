using System;
using System.Collections.Generic;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Search.Contracts.V1;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.Lightboxes.ViewInterfaces
{
    public interface IMyLightboxesView : IView
    {
        List<Lightbox> Lightboxes { get; set; }
        bool IsEmpty { get; set; }
        List<Corbis.LightboxCart.Contracts.V1.LightboxDisplayImage> Products { get; set; }
        int PageSize { get; set; }
        int TotalRecords { get; set; }
        int LightboxId { get; }
		Guid LightboxUid { get; set; }
		string LightboxName { get; set; }
		Guid NotesUid { get; set; }
		LightboxTreeSort LightboxTreeSortBy { get; }
        List<string> CartItems { get; set; }
        List<string> QuickPicList { get; set; }
        int CurrentPageNumber { get; set; }
        DateTime ModifiedDate { get; set; }
        DateTime CreatedDate { get; set; }
        string OwnerUsername { get; set; }
        string Client { get; set; }
        string Notes { get; set; }
        string Shared { get; set; }
    }
}
