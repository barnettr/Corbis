using System;
using Corbis.Image.Contracts.V1;

namespace Corbis.Web.UI.Presenters.MediaSetSearch
{
    public interface IMediaSetRedirectionView
    {
        int MediaSetId { get; set; }
        ImageMediaSetType MediaSetType { get; set; }
        Guid MediaSetUid { get; set; }
        Guid SessionSetUid { get; set; }
        int SessionSetId { get; set; }
        Guid RfcdUid { get; set; }
        string RfcdVolume { get; set; }
        Guid ImageUid { get; set; }
        string CorbisId { get; set; }
    }
}
